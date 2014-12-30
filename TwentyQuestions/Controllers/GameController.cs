using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TwentyQuestions.Constants;
using TwentyQuestions.Enums;
using TwentyQuestions.Models;
using TwentyQuestions.Repositories;
using TwentyQuestions.ViewModels;
using TwentyQuestions.ViewModels.Game;

namespace TwentyQuestions.Controllers
{
	/// <summary>
	/// Represents the GameController class.
	/// </summary>
	public class GameController : BaseController<ViewModels.Game.GameViewModel>
	{
		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.Controllers.GameController class.
		/// </summary>
		public GameController()
			: base(PageType.Game)
		{ }

		public async Task<ActionResult> Index()
		{
			using (UnitOfWork unitOfWork = new UnitOfWork())
			{
				Stopwatch timer = new Stopwatch();
				timer.Start();
				Models.Game game = await unitOfWork.GameEntityRepository.StartNewGameInstanceAsync();
				this.ViewModel.LoadingTime = timer.ElapsedMilliseconds;
				timer.Stop();

				if (game != null)
				{
					this.ViewModel.IDGame = game.IDGame;

					GameQuestionsJSONModel questions = await unitOfWork.GameQuestionsRepository.GetNextQuestionAsync(this.ViewModel.IDGame);

					if (questions != null)
					{
						this.ViewModel.CurrentQuestionIndex = questions.CurrentQuestionIndex;
						this.ViewModel.CurrentQuestionBody = questions.CurrentQuestion.QuestionBody;
					}

					return View(this.ViewModel);
				}

				return this.Redirect("Error");
			}
		}


		public async Task<ActionResult> NewGame()
		{
			using (UnitOfWork unitOfWork = new UnitOfWork())
			{
				Models.Game game = await unitOfWork.GameEntityRepository.StartNewGameInstanceAsync();

				if (game != null)
				{
					return this.RedirectToRoute("GameRoute", new { id = game.IDGame });
				}
				else
				{
					return this.Redirect("Error");
				}
			}
		}

		public async Task<ActionResult> Tralala(int x, string y)
		{
			return await this.ControllerActionInvoker(
				() =>
				{
					// do something with x and y
					return new Task<ActionResult>(
						() =>
						{


							return View();
						});
				});
		}

		public async Task<ActionResult> NextQuestion(int idGame)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					GameQuestionsJSONModel questions = await unitOfWork.GameQuestionsRepository.GetNextQuestionAsync(idGame);

					GameQuestionExpectedAnswerJSONModel[] answeredQuestions = await unitOfWork.GameQuestionsRepository.GetQuestionsAnsweredForGameAsync(idGame);

					return Json(
						AjaxActionResult.List(
							this.RenderPartialView("~/Views/Game/Partial/_AnsweredQuestionTemplate.cshtml", answeredQuestions),
							questions.CurrentQuestion,
							new
							{
								IsLastQuestion = questions.IsLastQuestion,
								CurrentQuestionIndex = questions.CurrentQuestionIndex
							}
					));
				}
			}
			catch (Exception ex)
			{
				return Json(AjaxActionResult.Fail(-1, "Error processing request!" + System.Environment.NewLine + ex.Message));
			}
		}

		public async Task<ActionResult> AnswerQuestion(int idGame, int questionIndex, int answer)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					await unitOfWork.GameQuestionsRepository.AnswerQuestionAndUpdateInstanceAsync(idGame, questionIndex, (AnswerType)answer);

					return Json(AjaxActionResult.Json(true));
				}
			}
			catch (Exception ex)
			{
				return Json(AjaxActionResult.Fail(-1, "Error processing request!" + System.Environment.NewLine + ex.Message));
			}
		}


		public async Task<ActionResult> GetGameResult(int idGame, int questionIndex, int answer)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					await unitOfWork.GameQuestionsRepository.AnswerQuestionAndUpdateInstanceAsync(idGame, questionIndex, (AnswerType)answer);

					Game gameResult = await unitOfWork.GameRepository.GetGameResult(idGame);

					GameQuestionExpectedAnswerJSONModel[] answeredQuestions = await unitOfWork.GameQuestionsRepository.GetQuestionsAnsweredForGameAsync(idGame);

					return Json(
						AjaxActionResult.List(
						this.RenderPartialView("~/Views/Game/Partial/_AnsweredQuestionTemplate.cshtml", answeredQuestions),
						gameResult));
				}
			}
			catch (Exception ex)
			{
				return Json(AjaxActionResult.Fail(-1, "Error processing request!" + System.Environment.NewLine + ex.Message));
			}
		}


		public async Task<ActionResult> SetCorrectGuess(int idGame, int idGuessedEntity, int attempt = 1)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					await unitOfWork.GameRepository.SetCorrectGuess(idGame, idGuessedEntity, attempt);

					return Json(
						AjaxActionResult.Json(
						this.RenderPartialView("~/Views/Game/Partial/_AnsweredQuestionTemplate.cshtml", await unitOfWork.GameRepository.GetExpectedAnswers(idGame, idGuessedEntity))));
				}
			}
			catch (Exception ex)
			{
				return Json(AjaxActionResult.Fail(-1, "Error processing request!" + System.Environment.NewLine + ex.Message));
			}
		}


		public async Task<ActionResult> SetIncorrectGuess(int idGame, int idGuessedEntity, int attempt)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					await unitOfWork.GameRepository.SetIncorrectGuess(idGame, idGuessedEntity, attempt);

					return Json(AjaxActionResult.Json(true));
				}
			}
			catch (Exception ex)
			{
				return Json(AjaxActionResult.Fail(-1, "Error processing request!" + System.Environment.NewLine + ex.Message));
			}
		}


		public async Task<ActionResult> GetTopGuesses(int idGame)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					return Json(
						AjaxActionResult.Json(
							this.RenderPartialView(
								"~/Views/Game/Partial/_TopGuessesListTemplate.cshtml",
								await unitOfWork.GameRepository.GetTopGuesses(idGame))));
				}
			}
			catch (Exception ex)
			{
				return Json(AjaxActionResult.Fail(-1, "Error processing request!" + System.Environment.NewLine + ex.Message));
			}
		}

		public async Task<ActionResult> EntitiesNamedLike(string needle)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					return Json(
						AjaxActionResult.Json(
						await unitOfWork.EntityRepository.EntitiesNamedLike(needle)));
				}
			}
			catch (Exception ex)
			{
				return Json(AjaxActionResult.Fail(-1, "Error processing request!" + System.Environment.NewLine + ex.Message));
			}
		}

		public async Task<ActionResult> SubmitNewEntity(int idGame, int idEntity, string entityName, string entityDescription)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					Tuple<Entity, bool> entity = await unitOfWork.EntityRepository.SubmitNewEntity(idEntity, entityName, entityDescription);
					if (entity != null)
					{
						await unitOfWork.GameRepository.SetCorrectGuess(idGame, entity.Item1.IDEntity, 3);

						if (idEntity > 0 || entity.Item2)
						{
							return Json(
								AjaxActionResult.List(
									this.RenderPartialView(
										"~/Views/Game/Partial/_AnsweredQuestionTemplate.cshtml",
										await unitOfWork.GameRepository.GetExpectedAnswers(idGame, entity.Item1.IDEntity)),
									entity.Item1));
						}
						else
						{
							return Json(AjaxActionResult.Json(true));
						}
					}
					else
					{
						return Json(AjaxActionResult.Fail(-1, "Failed to add the new entity!"));
					}
				}
			}
			catch (Exception ex)
			{
				return Json(AjaxActionResult.Fail(-1, "Error processing request!" + System.Environment.NewLine + ex.Message));
			}
		}

		[OutputCache(Duration = TechnicalConstants.DeleteOldGamesCacheDuration, VaryByParam = "none")]
		public async Task<ActionResult> DeleteOldGames()
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					await unitOfWork.GameRepository.DeleteOldGames();
				}
			}
			catch (Exception ex)
			{ }

			return Json(AjaxActionResult.Json(true)); // we don`t care for the exception here!
		}
	}
}