using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwentyQuestions.Constants;
using TwentyQuestions.Enums;
using TwentyQuestions.Models;
using TwentyQuestions.ViewModels.Game;

namespace TwentyQuestions.Repositories
{
	public class GameRepository : BaseRepository<Game>, IGameRepository
	{
		public GameRepository(DbContext context) : base(context) { }

		public Game StartNewGame()
		{
			Game newGame = new Game
			{
				LastActivity = DateTime.Now
			};

			dbSet.Add(newGame);

			return newGame;
		}

		public async Task UpdateGameActivity(int idGame)
		{
			await this.dbContext.Database.ExecuteSqlCommandAsync(
																	@"
																		update
																				Games
																		set
																				LastActivity = GETDATE()
																		where
																				IDGame = {0}", idGame);
		}

		public async Task DeleteOldGames()
		{
			await this.dbContext.Database.ExecuteSqlCommandAsync(
																	@"
																		delete from
																					Games
																		where
																					PlayedObject_IDEntity is null and
																					DATEDIFF(MINUTE, LastActivity, GETDATE()) > {0}", TechnicalConstants.DeleteOldGamesAfterMinutes);
		}

		public static double MaxFitness(int askedQuestions)
		{
			int n = askedQuestions / TechnicalConstants.InstanceFitnessUpdateWindow;
			int m = askedQuestions % TechnicalConstants.InstanceFitnessUpdateWindow;
			int multiplier = n * (n - 1) / 2;
			double maxFitness = 1 + askedQuestions * TechnicalConstants.InstanceFitnessBaseUpdateValue +
								TechnicalConstants.InstanceFitnessStep *
								(TechnicalConstants.InstanceFitnessUpdateWindow * multiplier + m * n);

			return maxFitness;
		}

		public async Task<Game> GetGameResult(int idGame)
		{
			await this.UpdateGameActivity(idGame);

			Entity guessedEntity = await this.dbContext.Database.SqlQuery<Entity>(
																	 @"select top 1
																				Entity.*
																	from
																				Entities as Entity
																	inner join
																				GameEntities as Instance on Instance.Entity_IDEntity = Entity.[IDEntity]
																	where
																				Instance.Game_IDGame = {0}
																	order by
																				Instance.Fitness desc", idGame).FirstOrDefaultAsync();

			int askedQuestions = await this.dbContext.Database.SqlQuery<int>(
																	@"
																		select 
																				count(*) 
																		from 
																				GameQuestions 
																		where 
																				Game_IDGame = {0}", idGame).FirstAsync();

			await this.dbContext.Database.ExecuteSqlCommandAsync(
										@"
											update 
													Game 
											set 
													Game.GuessedObject_IDEntity = {0},
													Game.CertaintyPercentage =	100.0 * 
																				(select top 1 Fitness from GameEntities where Game_IDGame = {1} order by Fitness desc) /
																				({2})
											from
													Games as Game
											where
													Game.IDGame = {1}", guessedEntity.IDEntity, idGame, GameRepository.MaxFitness(askedQuestions));

			Game gameResult = await this.dbSet.Where(g => g.IDGame == idGame).Include(e => e.GuessedObject).FirstOrDefaultAsync();

			return gameResult;
		}

		public async Task<GameQuestionExpectedAnswerJSONModel[]> GetExpectedAnswers(int idGame, int idEntity)
		{
			// get the asked questions together with their expected answers
			return await this.dbContext.Set<GameQuestion>()
									.Where(w => w.Game.IDGame == idGame)
									.GroupJoin(
										this.dbContext.Set<EntityQuestion>(),
										gq => gq.Question.IDQuestion,
										eq => eq.Question.IDQuestion,
										(gq, eq) => new
										{
											GameQuestion = gq,
											EntityQuestion = eq.Where(w => w.Entity.IDEntity == idEntity).FirstOrDefault()
										})
									.Select(
										w => new GameQuestionExpectedAnswerJSONModel()
										{
											GameQuestion = w.GameQuestion,
											EntityQuestion = w.EntityQuestion
										}).ToArrayAsync();

		}

		private async Task UpdateEntityQuestions(int idGame, int idEntity, int multiplier)
		{
			int askedQuestions = await this.dbContext.Set<GameQuestion>().CountAsync(gq => gq.Game.IDGame == idGame);

			await this.dbContext.Database.ExecuteSqlCommandAsync(
									@"
										insert into EntityQuestions (
														Entity_IDEntity,	Question_IDQuestion,				Fitness,	YesCount,	NoCount,	UnknownCount)
										(
											select
														{0},				GameQuestion.Question_IDQuestion,	0.0,		0,			0,			0
											from
														GameQuestions as GameQuestion
											inner join
														Games as Game on Game.IDGame = GameQuestion.Game_IDGame
											where
														Game.IDGame = {1}
														and
														not exists
														(	
															select 
																		eq.Entity_IDEntity,	
																		eq.Question_IDQuestion
															from
																		EntityQuestions	as eq
															where
																		eq.Entity_IDEntity = {0} and
																		eq.Question_IDQuestion = GameQuestion.Question_IDQuestion
														)
										)", idEntity, idGame);

			// Update fitnesses. Where updating positively, only update if the given answer was yes, when updating negatively only update if it was no.
			await this.dbContext.Database.ExecuteSqlCommandAsync(
								@"
									update
										EntityQuestion
									set
										EntityQuestion.YesCount = 	case
																		when GameQuestion.GivenAnswer = {0} and {3} > 0 and EntityQuestion.Locked = 0 then EntityQuestion.YesCount + 1
																		else EntityQuestion.YesCount
																	end,
										EntityQuestion.NoCount = 	case
																		when GameQuestion.GivenAnswer = {1} and {3} > 0 and EntityQuestion.Locked = 0 then EntityQuestion.NoCount + 1
																		else EntityQuestion.NoCount
																	end,
										EntityQuestion.UnknownCount = 	case
																			when GameQuestion.GivenAnswer = {2} and {3} > 0 and EntityQuestion.Locked = 0 then EntityQuestion.UnknownCount + 1
																			else EntityQuestion.UnknownCount
																		end,
										EntityQuestion.Fitness =	case
																		when GameQuestion.GivenAnswer = {0} then 
																			EntityQuestion.Fitness + {3} * exp(1 - (GameQuestion.QuestionIndex - 1.0) / {4})
																		when GameQuestion.GivenAnswer = {1} then
																			EntityQuestion.Fitness + {7} * {3} * exp(1 - (GameQuestion.QuestionIndex - 1.0) / {4})
																		else
																			EntityQuestion.Fitness
																	end
									from
										EntityQuestions as EntityQuestion
									inner join
										GameQuestions as GameQuestion on GameQuestion.Question_IDQuestion = EntityQuestion.Question_IDQuestion
									where
										EntityQuestion.Entity_IDEntity = {5} and
										GameQuestion.Game_IDGame = {6}
									", (int)AnswerType.Yes, (int)AnswerType.No, (int)AnswerType.Unknown,
										multiplier, askedQuestions, idEntity, idGame, TechnicalConstants.CorrectNoAnswerFitnessUpdateCoefficient);
		}

		public async Task SetCorrectGuess(int idGame, int idGuessedEntity, int attempt)
		{
			double fitness = await this.dbContext
										.Set<GameEntity>()
										.Where(ge => ge.Game.IDGame == idGame && ge.Entity.IDEntity == idGuessedEntity)
										.Select(ge => ge.Fitness)
										.FirstOrDefaultAsync<double>();

			int playedRank = fitness == default(double) ? GamePlayConstants.PlayedRankNewEntity : await this.dbContext
																											.Set<GameEntity>()
																											.CountAsync(ge => ge.Game.IDGame == idGame && ge.Fitness > fitness) + 1;

			await this.UpdateGameActivity(idGame);

			await this.dbContext.Database.ExecuteSqlCommandAsync(
									@"
													update 
																Games 
													set 
																PlayedObject_IDEntity = {0}, 
																PlayedRank = {1} 
													where IDGame = {2}", idGuessedEntity, playedRank, idGame);

			// delete the game entities for this game, no longer need them
			await this.dbContext.Database.ExecuteSqlCommandAsync(
									@"
													delete from
														GameEntities
													where
														Game_IDGame = {0}", idGame);

			// update entity-questions associations positively
			await UpdateEntityQuestions(idGame, idGuessedEntity, 1);

			await this.dbContext.Database.ExecuteSqlCommandAsync(
									@"
										update 
													Entities 
										set 
													TimesPlayed = TimesPlayed + 1,
													TimesGuessed =	case
																		when {0} = 1 then TimesGuessed + 1
																		else TimesGuessed
																	end,
													LastPlayed = GETDATE()
										where 
													[IDEntity] = {1}", attempt, idGuessedEntity);
		}

		public async Task<Entity[]> GetTopGuesses(int idGame)
		{
			await this.UpdateGameActivity(idGame);

			return await this.dbContext.Database.SqlQuery<Entity>(
								@"
									select
												Entity.*
									from
												Entities as Entity
									inner join
												GameEntities as Instance on Instance.Entity_IDEntity = Entity.[IDEntity] and Instance.Game_IDGame = {0}
									order by
												Instance.Fitness desc
									offset 1 rows
									fetch next " + GamePlayConstants.MaxAlternativeEntities + " rows only", idGame).ToArrayAsync();
		}

		public async Task SetIncorrectGuess(int idGame, int idGuessedEntity, int attempt)
		{
			await this.UpdateGameActivity(idGame);

			// update entity-questions associations negatively
			await UpdateEntityQuestions(idGame, idGuessedEntity, -1);
		}

		public async Task<Game[]> GetRecentGames()
		{
			return await this.dbSet
								.OrderByDescending(t => t.LastActivity)
								.Where(g => g.GuessedObject != null && g.PlayedObject != null)
								.Include(g => g.GuessedObject)
								.Include(g => g.PlayedObject)
								.Take(TechnicalConstants.RecentGamesListLength)
								.ToArrayAsync();
		}
	}
}