using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwentyQuestions.Constants;
using TwentyQuestions.Enums;
using TwentyQuestions.Models;
using TwentyQuestions.ViewModels.Game;

namespace TwentyQuestions.Repositories
{
	public class GameQuestionsRepository : BaseRepository<GameQuestion>, IGameQuestionsRepository
	{
		public GameQuestionsRepository(DbContext context, UnitOfWork unitOfWork) : base(context, unitOfWork) { }

		Random r = new Random();

		private async Task<Entity> FulfillsShortcutConditions(int idGame, int askedQuestions)
		{
			if (await this.dbSet.Where(g => g.Game.IDGame == idGame).CountAsync() < TechnicalConstants.MandatoryNumberOfProperQuestions)
			{
				return null;
			}

			GameEntity[] topGuesses = await this.dbContext.Set<GameEntity>()
											.Where(g => g.Game.IDGame == idGame)
											.OrderByDescending(i => i.Fitness)
											.Include(g => g.Entity)
											.Take(2)
											.ToArrayAsync();

			double[] certainties = new double[] {	100.0 * topGuesses[0].Fitness / GameRepository.MaxFitness(askedQuestions),
													100.0 * topGuesses[1].Fitness / GameRepository.MaxFitness(askedQuestions) };

			double topDiff = certainties[0] - certainties[1]; 
			if (certainties[0] < TechnicalConstants.MandatoryCertaintyPercentageBeforeShortcuts ||
				(topDiff >= TechnicalConstants.MandatoryMinimumCertaintyPercentageDiffBeforeShortcuts && topDiff <= TechnicalConstants.MandatoryMaximumCertaintyPercentageDiffBeforeShortcuts))
			{
				return null;
			}

			return topGuesses[0].Entity;
		}
		private async Task<bool> CanEarlyGuess(int idGame)
		{
			if (await this.dbSet.Where(g => g.Game.IDGame == idGame).CountAsync() < TechnicalConstants.MandatoryEarlyGuessNumberOfQuestions ||
				r.NextDouble() >= TechnicalConstants.EarlyGuessProbability)
			{
				return false;
			}

			return true;
		}

		private async Task AnswerQuestionAsync(Game game, int questionIndex, AnswerType answer)
		{
			this.unitOfWork.GameRepository.UpdateGameActivityNoSave(game);

			GameQuestion gameQuestion = await this.dbSet.Where(g => g.Game.IDGame == game.IDGame && g.QuestionIndex == questionIndex)
													.FirstOrDefaultAsync();

			if (gameQuestion != null)
			{
				gameQuestion.GivenAnswer = answer;
			}

			await this.dbContext.SaveChangesAsync();
		}

		private async Task<Question> GetShortcutQuestion(int idTopEntity, int idGame)
		{
			Question ret = new Question();
			ret = await this.dbContext.Database.SqlQuery<Question>(
													@"
														select top 1
															Question.*
														from
															Questions as Question
														where
															Question.IDQuestion not in (	select
																								eq.Question_IDQuestion
																							from
																								EntityQuestions as eq
																							where
																								eq.Entity_IDEntity = {0}
																						) and
															Question.IDQuestion not in (
																							select
																								GQ.Question_IDQuestion
																							from
																								GameQuestions as GQ
																							where
																								GQ.Game_IDGame = {1}
																						)
														order by
															Question.TimesAsked
													", idTopEntity, idGame).FirstOrDefaultAsync();

			if (ret == null)
			{
				ret = await this.dbContext.Database.SqlQuery<Question>(
												@"
														select top 1
															Question.*
														from
															EntityQuestions as EntityQuestion 
														inner join
															Questions as Question on Question.IDQuestion = EntityQuestion.Question_IDQuestion
														where
															EntityQuestion.Entity_IDEntity = {0} and
															EntityQuestion.Question_IDQuestion not in (	select
																											GQ.Question_IDQuestion
																										from
																											GameQuestions as GQ
																										where
																											GQ.Game_IDGame = {1}
																										)
														order by
															Question.TimesAsked
													", idTopEntity, idGame).FirstOrDefaultAsync();
			}

			return ret;
		}

		private async Task<Question[]> GetBinarySplitQuestion(int idGame, int topCandidatesCount)
		{
			return await this.dbContext.Database.SqlQuery<Question>(
							@"
									declare @maxFitness int
									select 
										@maxFitness = max(ge.Fitness)
									from
										GameEntities as ge
									where
										ge.Game_IDGame = {0}

									declare @countMaxFitness int = {2}

									select top 1
										*,
										((AnswersDifference + EntitiesCount) / 2 + @countMaxFitness - EntitiesCount) as WorstCaseRemaining
									from
									(
										select top " + TechnicalConstants.NumberOfConsideredQuestionsForBinarySplit + @"
											Question.*,
											abs
											(
												sum
												(
													case 
														when
															EntityQuestion.YesCount > EntityQuestion.NoCount and 
															EntityQuestion.YesCount > EntityQuestion.UnknownCount
														then
															1
														else
															0
													end
												)
												-
												sum
												(
													case
														when
															EntityQuestion.[NoCount] > EntityQuestion.YesCount and 
															EntityQuestion.[NoCount] > EntityQuestion.UnknownCount
														then
															1
														else
															0
													end
												)
											) as AnswersDifference,
											count(*) as EntitiesCount
										from
											EntityQuestions as EntityQuestion
										inner join
											Questions as Question on Question.IDQuestion = EntityQuestion.Question_IDQuestion
										where
											EntityQuestion.Entity_IDEntity in
											(
												select 
													ge.Entity_IDEntity
												from 
													GameEntities as ge
												where
													ge.Game_IDGame = {0} and
													ge.Fitness = @maxFitness
											) and
											Question.IDQuestion not in
											(
												select 
													gq.Question_IDQuestion
												from
													GameQuestions as gq
												where
													gq.Game_IDGame = {0}
											) and
											(EntityQuestion.UnknownCount < EntityQuestion.YesCount or EntityQuestion.UnknownCount < EntityQuestion.[NoCount]) and
											TimesAsked > {1}
										group by
											Question.QuestionBody,
											Question.IDQuestion,
											Question.DateAdded,
											Question.FirstAsked,
											Question.LastAsked,
											Question.TimesAsked
									) as InnerQuery
									order by
										WorstCaseRemaining", idGame, TechnicalConstants.MinimumTimesAskedForBinarySplitQuestions, topCandidatesCount).ToArrayAsync();
		}

		private async Task<Question[]> GetTopEnforcementQuestion(int idGame)
		{
			Question[] potentialNext = await this.dbContext.Database.SqlQuery<Question>(
									@"
										select top " + TechnicalConstants.PickAmongstNumber + @"
											Question.*
										from
											Questions as Question
										inner join
											EntityQuestions as EntityQuestion on EntityQuestion.Question_IDQuestion = Question.IDQuestion
										where
											EntityQuestion.Entity_IDEntity in	(
																					select top " + TechnicalConstants.SelectTopQuestionsFromNumberOfEntities + @"
																						ge.Entity_IDEntity
																					from 
																						GameEntities as ge
																					where
																						ge.Game_IDGame = {0}
																					order by 
																						Fitness desc
																				)
												and
											Question.IDQuestion not in	(
																			select 
																				gq.Question_IDQuestion
																			from
																				GameQuestions as gq
																			where
																				gq.Game_IDGame = {0}
																		)
												and
											EntityQuestion.YesCount > EntityQuestion.NoCount and
											EntityQuestion.YesCount > EntityQuestion.UnknownCount
										order by
											EntityQuestion.Fitness desc", idGame).ToArrayAsync();

			if (potentialNext == null || potentialNext.Length == 0)
			{
				potentialNext = await this.dbContext.Database.SqlQuery<Question>(
							@"
										select top " + TechnicalConstants.PickAmongstNumber + @"
											Question.*
										from
											Questions as Question
										inner join
											EntityQuestions as EntityQuestion on EntityQuestion.Question_IDQuestion = Question.IDQuestion
										where
											EntityQuestion.Entity_IDEntity 	in (
																					select top " + TechnicalConstants.SelectTopQuestionsFromNumberOfEntities + @"
																						ge.Entity_IDEntity
																					from 
																						GameEntities as ge
																					where
																						ge.Game_IDGame = {0}
																					order by 
																						Fitness desc
																				)
												and
											Question.IDQuestion not in	(
																			select 
																				gq.Question_IDQuestion
																			from
																				GameQuestions as gq
																			where
																				gq.Game_IDGame = {0}
																		)
										order by
											EntityQuestion.Fitness desc", idGame).ToArrayAsync();

				if (potentialNext == null || potentialNext.Length == 0)
				{
					GameEntity topGameEntity = await this.dbContext.Set<GameEntity>()
												.Where(g => g.Game.IDGame == idGame)
												.OrderByDescending(i => i.Fitness)
												.Include(g => g.Entity)
												.FirstOrDefaultAsync();

					potentialNext = new Question[] { await GetShortcutQuestion(topGameEntity.Entity.IDEntity, idGame) };
				}
			}

			return potentialNext;
		}

		public async Task<List<GameQuestion>> GetAnsweredQuestionsAsync(int idGame)
		{
			return await this.dbSet
								.Where(gq => gq.Game.IDGame == idGame && gq.GivenAnswer != AnswerType.Undefined)
								.Include(gq => gq.Question)
								.OrderBy(gq => gq.QuestionIndex)
								.ToListAsync();
		}

		public async Task<GamePlayViewModel> GetGamePlayVMAsync(string gameAccessId)
		{
			Game game = await this.unitOfWork.GameRepository.GetGameFromAccessIdAsync(gameAccessId);

			GameQuestion lastAskedQuestion = await this.dbSet
														.Where(g => g.Game.IDGame == game.IDGame)
														.Where(g => g.QuestionIndex == this.dbSet.Where(t => t.Game.IDGame == game.IDGame).Max(t => t.QuestionIndex))
														.Include(g => g.Question)
														.FirstOrDefaultAsync();

			GamePlayViewModel gamePlayVM = new GamePlayViewModel();
			gamePlayVM.AccessID = gameAccessId;
			gamePlayVM.AnsweredQuestions = await this.GetAnsweredQuestionsAsync(game.IDGame);
			if (gamePlayVM.AnsweredQuestions == null)
			{
				gamePlayVM.AnsweredQuestions = new List<GameQuestion>();
			}

			if (lastAskedQuestion != null && lastAskedQuestion.GivenAnswer == AnswerType.Undefined)
			{
				gamePlayVM.CurrentQuestion = lastAskedQuestion.Question;
				return gamePlayVM;
			}

			if ((lastAskedQuestion != null && lastAskedQuestion.QuestionIndex < GamePlayConstants.MaxQuestionsUntilFirstGuess && !lastAskedQuestion.IsLastQuestion) || 
				 lastAskedQuestion == null)
			{
				Entity topEntity = await this.FulfillsShortcutConditions(game.IDGame, gamePlayVM.AnsweredQuestions.Count);

				gamePlayVM.IsLastQuestion = (topEntity != null && (await this.CanEarlyGuess(game.IDGame)));

				if (topEntity != null)
				{
					gamePlayVM.CurrentQuestion = await this.GetShortcutQuestion(topEntity.IDEntity, game.IDGame);
				}
				else
				{
					Question[] potentialNext = new Question[TechnicalConstants.PickAmongstNumber];

					int topCandidatesCount = await this.dbContext.Database.SqlQuery<int>(
													@"
												select 
													count(*) 
												from 
													GameEntities 
												where 
													Game_IDGame = {0} and
													Fitness = 
													(
														select 
															max(Fitness) 
														from
															GameEntities
														where 
															Game_IDGame = {0}
													)", game.IDGame).FirstOrDefaultAsync();

					if (topCandidatesCount > 1)
					{
						potentialNext = await GetBinarySplitQuestion(game.IDGame, topCandidatesCount);
					}
					else
					{
						potentialNext = await GetTopEnforcementQuestion(game.IDGame);
					}


					if (potentialNext == null || potentialNext.Length == 0)
					{
						// TODO: pick a random question or something
						gamePlayVM.CurrentQuestion = null;
					}
					else
					{
						gamePlayVM.CurrentQuestion = potentialNext[r.Next(potentialNext.Length)];
					}
				}

				// update asked stats for current question
				await this.dbContext.Database.ExecuteSqlCommandAsync(
													@"
												update 
														Questions 
												set
														FirstAsked =	case when FirstAsked is null
																			then GETDATE()
																			else FirstAsked
																		end,

														LastAsked = GETDATE(),
														TimesAsked = TimesAsked + 1
												where
														IDQuestion = {0}", gamePlayVM.CurrentQuestion.IDQuestion);

				gamePlayVM.IsLastQuestion |= gamePlayVM.AnsweredQuestions.Count + 1 == GamePlayConstants.MaxQuestionsUntilFirstGuess;
					
				await this.dbContext.Database.ExecuteSqlCommandAsync(
								@"	insert into GameQuestions (
										QuestionIndex,	GivenAnswer,	Game_IDGame,	Question_IDQuestion,	IsLastQuestion)
									values (
										{0},			{1},			{2},			{3},					{4})",
										gamePlayVM.AnsweredQuestions.Count + 1, (int)AnswerType.Undefined, game.IDGame, gamePlayVM.CurrentQuestion.IDQuestion, gamePlayVM.IsLastQuestion);

				return gamePlayVM;
			}
			
			// If we get here, we tried to answer a non-existing question or one that was already answered.
			// This should never happen through the interface, so the user must be trying to cheat. Therefore,
			// we can just return null and let them get errors.

			return null;
		}

		public async Task AnswerQuestionAndUpdateInstanceAsync(string gameAccessId, AnswerType answer)
		{
			Game game = await this.dbContext.Set<Game>().FirstOrDefaultAsync(t => t.AccessID == gameAccessId);

			if (game.GameState != GameState.Playing)
			{
				return;
			}

			GameQuestion answeredQuestion = await this.dbSet
											.Where(t => t.Game.IDGame == game.IDGame)
											.Where(t => t.GivenAnswer == AnswerType.Undefined)
											.FirstOrDefaultAsync();

			await this.AnswerQuestionAsync(game, answeredQuestion.QuestionIndex, answer);

			double updateValue = TechnicalConstants.InstanceFitnessBaseUpdateValue + ((answeredQuestion.QuestionIndex - 1) / TechnicalConstants.InstanceFitnessUpdateWindow) * TechnicalConstants.InstanceFitnessStep;
			if (answer == AnswerType.ProbablyYes || answer == AnswerType.ProbablyNo)
			{
				updateValue /= 2.0;

				if (answer == AnswerType.ProbablyYes)
				{
					answer = AnswerType.Yes;
				}
				else
				{
					answer = AnswerType.No;
				}
			}

			if (answer != AnswerType.Unknown) // don't knows should not affect rankings
			{
				await this.dbContext.Database.ExecuteSqlCommandAsync(
									@"
										update
											GameInstance
										set
											GameInstance.Fitness = GameInstance.Fitness + {2}
										from
											GameEntities as GameInstance
										inner join
											EntityQuestions as EntityQuestion on EntityQuestion.Entity_IDEntity = GameInstance.Entity_IDEntity
										where
											GameInstance.Game_IDGame = {0} and
											EntityQuestion.Question_IDQuestion = (	select
																						Question_IDQuestion
																					from
																						GameQuestions
																					where
																						Game_IDGame = {0} and
																						QuestionIndex = {1}
																				) and
											EntityQuestion." + answer.ToString() + @"Count =	(select 
																				case
																					when	YesCount > [NoCount] and 
																							YesCount > UnknownCount
																					then YesCount
																				
																					when	[NoCount] > YesCount and 
																							[NoCount] > UnknownCount
																					then [NoCount]
																				
																					when	UnknownCount > YesCount and 
																							UnknownCount > [NoCount]
																					then UnknownCount
																				end
																		from
																				EntityQuestions
																		where
																				IDEntityQuestion = EntityQuestion.IDEntityQuestion)
									", game.IDGame, answeredQuestion.QuestionIndex, updateValue);
			}

			if (answeredQuestion.IsLastQuestion)
			{
				game.GameState = GameState.LastQuestionAnswered;
				GameEntity topGameEntity = await this.unitOfWork.GameEntityRepository.GetTopGameEntityAsync(game);
				game.GuessedObject = topGameEntity.Entity;
				game.CertaintyPercentage = 100.0 * topGameEntity.Fitness / GameRepository.MaxFitness(answeredQuestion.QuestionIndex);

				await this.dbContext.SaveChangesAsync();
			}
		}
	}
}