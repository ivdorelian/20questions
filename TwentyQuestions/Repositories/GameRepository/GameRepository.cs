using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwentyQuestions.Constants;
using TwentyQuestions.Enums;
using TwentyQuestions.Models;
using TwentyQuestions.Utils;
using TwentyQuestions.ViewModels.Game;

namespace TwentyQuestions.Repositories
{
	public class GameRepository : BaseRepository<Game>, IGameRepository
	{
		public GameRepository(DbContext context, UnitOfWork unitOfWork) : base(context, unitOfWork) { }



		public async Task<string> StartNewGame()
		{
			RandomString randomId = new RandomString();
			string newGameId = randomId.GetRandomString(TechnicalConstants.GameIdLength);

			Game newGame = new Game
			{
				LastActivity = DateTime.Now,
				AccessID = newGameId,
				GameState = GameState.Playing
			};

			try
			{
				this.dbSet.Add(newGame);

				await this.dbContext.SaveChangesAsync();

				await dbContext.Database.ExecuteSqlCommandAsync(
				@"	INSERT INTO GameEntities (
						Game_IDGame,		Entity_IDEntity,		Fitness)
					SELECT
						{0},				[IDEntity],				{1}
					FROM
						Entities", newGame.IDGame, 1);
			}
			catch (DbUpdateException updateEx)
			{
				var inner1 = updateEx.InnerException;
				if (inner1 != null && 
					inner1.InnerException != null &&
					(inner1.InnerException as SqlException).Errors.OfType<SqlError>().Any(se => se.Number == 2601 || se.Number == 2627 /* PK/UKC violation */))
				{
					// it's a dupe, handle on controller
					this.dbSet.Remove(newGame);

					return null;
				}
				else
				{
					throw;
				}
			}

			return newGame.AccessID;
		}

		public void UpdateGameActivityNoSave(Game game)
		{
			game.LastActivity = DateTime.Now;
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

		public async Task ComputeExpectedAnswersAsync(Game game, int idEntity)
		{
			// save the expected answers to database
			await this.dbContext.Database.ExecuteSqlCommandAsync(
				@"
					update
						gq
					set
						gq.ExpectedAnswer = case
												when eq.YesCount > eq.NoCount and eq.YesCount > eq.UnknownCount then {2}
												when eq.NoCount > eq.YesCount and eq.NoCount > eq.UnknownCount then {3}
												else {4}
											end
					from
						GameQuestions as gq
					inner join
						EntityQuestions as eq on eq.Entity_IDEntity = {1}
					where
						gq.Game_IDGame = {0}", game.IDGame, idEntity, (int)AnswerType.Yes, (int)AnswerType.No, (int)AnswerType.Unknown); 

		}

		private async Task UpdateEntityQuestionsAsync(int idGame, int idEntity, int multiplier)
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

		private async Task SetCorrectGuessAsync(int idGame, int idGuessedEntity, int attempt)
		{
			double fitness = await this.dbContext
										.Set<GameEntity>()
										.Where(ge => ge.Game.IDGame == idGame && ge.Entity.IDEntity == idGuessedEntity)
										.Select(ge => ge.Fitness)
										.FirstOrDefaultAsync<double>();

			int playedRank = fitness == default(double) ? GamePlayConstants.PlayedRankNewEntity : await this.dbContext
																											.Set<GameEntity>()
																											.CountAsync(ge => ge.Game.IDGame == idGame && ge.Fitness > fitness) + 1;

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
			await this.UpdateEntityQuestionsAsync(idGame, idGuessedEntity, 1);

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

		private async Task SetIncorrectGuessAsync(int idGame, int idGuessedEntity, int attempt)
		{
			// update entity-questions associations negatively
			await this.UpdateEntityQuestionsAsync(idGame, idGuessedEntity, -1);
		}


		public async Task<GuessViewModel> GetGuessVMAsync(string gameAccessId)
		{
			Game game = await this.unitOfWork.GameRepository.GetGameFromAccessIdAsync(gameAccessId);

			this.unitOfWork.GameRepository.UpdateGameActivityNoSave(game);

			GuessViewModel firstGuessVM = new GuessViewModel();
			
			firstGuessVM.Game = game;
			firstGuessVM.AnsweredQuestions = await this.unitOfWork.GameQuestionsRepository.GetAnsweredQuestionsAsync(game.IDGame);

			await this.dbContext.SaveChangesAsync();

			return firstGuessVM;
		}

		public async Task JudgeFirstGuessAsync(string gameAccessId, bool correctGuess)
		{
			Game game = await this.GetGameFromAccessIdAsync(gameAccessId);

			if (game.GameState != GameState.LastQuestionAnswered)
			{
				return;
			}

			this.UpdateGameActivityNoSave(game);

			if (correctGuess)
			{
				await this.SetCorrectGuessAsync(game.IDGame, game.GuessedObject.IDEntity, 1);
				await this.ComputeExpectedAnswersAsync(game, game.GuessedObject.IDEntity);
				game.GameState = GameState.FirstGuessMarkedCorrect;
			}
			else
			{
				await this.SetIncorrectGuessAsync(game.IDGame, game.GuessedObject.IDEntity, 1);
				game.GameState = GameState.FirstGuessMarkedIncorrect;
			}

			await this.dbContext.SaveChangesAsync();
		}

		public async Task JudgeTopGuessAsync(string gameAccessId, int indexGuess)
		{
			Game game = await this.GetGameFromAccessIdAsync(gameAccessId);

			this.UpdateGameActivityNoSave(game);

			if (indexGuess < 2)
			{
				game.GameState = GameState.MustEnterWhoItWas;
				await this.dbContext.SaveChangesAsync();
				
				return;
			}

			if (game.GameState != GameState.FirstGuessMarkedIncorrect || indexGuess > 2 + GamePlayConstants.MaxAlternativeEntities)
			{
				return;
			}

			GameEntity selectedEntity = await this.unitOfWork.GameEntityRepository.GetTopGameEntityAsync(game, indexGuess);
			await this.SetCorrectGuessAsync(game.IDGame, selectedEntity.Entity.IDEntity, 2);
			await this.ComputeExpectedAnswersAsync(game, selectedEntity.Entity.IDEntity);

			game.GameState = GameState.SelectedFromTopGuessesList;

			await this.dbContext.SaveChangesAsync();
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

		public async Task<Game> GetGameFromAccessIdAsync(string gameAccessId)
		{
			return await this.dbSet
								.Where(g => g.AccessID == gameAccessId)
								.Include(g => g.GuessedObject)
								.Include(g => g.PlayedObject)
								.FirstOrDefaultAsync();
		}
		
		public async Task<GameState> GetGameStateAsync(int idGame)
		{
			return await this.dbSet.Where(g => g.IDGame == idGame).Select(g => g.GameState).FirstOrDefaultAsync();
		}

		public async Task<GameState> GetGameStateAsync(string gameAccessId)
		{
			return await this.dbSet.Where(g => g.AccessID == gameAccessId).Select(g => g.GameState).FirstOrDefaultAsync();
		}
	}
}