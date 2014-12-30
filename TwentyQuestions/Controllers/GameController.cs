using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
	public class GameController : BaseController<ViewModels.Game.GamePlayViewModel>
	{
		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.Controllers.GameController class.
		/// </summary>
		public GameController()
			: base(PageType.Game)
		{ }

		public async Task<ActionResult> NewGame()
		{
			using (UnitOfWork unitOfWork = new UnitOfWork())
			{
				string newGameId = await unitOfWork.GameRepository.StartNewGame();

				while (newGameId == null)
				{
					newGameId = await unitOfWork.GameRepository.StartNewGame();
				}

				return RedirectToRoute("GameRoute", new { id = newGameId });
			}
		}

		public async Task<ActionResult> Play(string id)
		{
			if (id == null || id.Length != TechnicalConstants.GameIdLength)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}


			using (UnitOfWork unitOfWork = new UnitOfWork())
			{
				GameState gameState = await unitOfWork.GameRepository.GetGameStateAsync(id);

				if (gameState == GameState.Undefined)
				{
					return new HttpStatusCodeResult(HttpStatusCode.NotFound);
				}

				switch (gameState)
				{
					case GameState.Playing:
						GamePlayViewModel gamePlayVM = await unitOfWork.GameQuestionsRepository.GetGamePlayVMAsync(id);
						return View("Play", gamePlayVM);
					case GameState.LastQuestionAnswered:
						GuessViewModel firstGuessVM = await unitOfWork.GameRepository.GetGuessVMAsync(id);
						return View("FirstGuessFeedback", firstGuessVM);
					case GameState.FirstGuessMarkedCorrect:
					case GameState.SelectedFromTopGuessesList:
					case GameState.EnteredWhoItWas:
						GuessViewModel guessVM = await unitOfWork.GameRepository.GetGuessVMAsync(id);
						return View("CorrectGuess", guessVM);
					case GameState.FirstGuessMarkedIncorrect:
						TopGuessesViewModel topGuessesVM = await unitOfWork.GameEntityRepository.GetTopGuessesVMAsync(id);
						return View("TopGuesses", topGuessesVM);
					case GameState.MustEnterWhoItWas:
						SubmitNewEntityViewModel newEntitySubmissionVM = new SubmitNewEntityViewModel
						{
							AccessID = id,
							SubmittedEntity = new Entity()
						};
						return View("SubmitNewEntity", newEntitySubmissionVM);
					default:
						return null;
				}
			}
		}

		[HttpPost]
		[ActionName("Play")]
		public async Task<ActionResult> SubmitNewEntity(SubmitNewEntityViewModel newEntity)
		{
			using (UnitOfWork unitOfWork = new UnitOfWork())
			{
				GameState gameState = await unitOfWork.GameRepository.GetGameStateAsync(newEntity.AccessID);

				if (gameState != GameState.MustEnterWhoItWas)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}

				if (ModelState.IsValid)
				{
					// add to database and update state

				}

				return RedirectToAction("Play", new { id = newEntity.AccessID });
			}
		}

		public async Task<JsonResult> NextQuestion(string gameAccessId)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					GamePlayViewModel gamePlayVM = await unitOfWork.GameQuestionsRepository.GetGamePlayVMAsync(gameAccessId);

					return Json(
						AjaxActionResult.List(
							this.RenderPartialView("~/Views/Game/PlayPartial/_AnsweredQuestionsPartial.cshtml", gamePlayVM.AnsweredQuestions),
							gamePlayVM.CurrentQuestion.QuestionBody,
							gamePlayVM.AnsweredQuestions.Count + 1,
							gamePlayVM.IsLastQuestion));
				}
			}
			catch (Exception ex)
			{
				return Json(AjaxActionResult.Fail(-1, "Error processing request!" + System.Environment.NewLine + ex.Message));
			}
		}

		public async Task<JsonResult> AnswerQuestion(string gameAccessId, int answer)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					await unitOfWork.GameQuestionsRepository.AnswerQuestionAndUpdateInstanceAsync(gameAccessId, (AnswerType)answer);

					return Json(AjaxActionResult.Json(true));
				}
			}
			catch (Exception ex)
			{
				return Json(AjaxActionResult.Fail(-1, "Error processing request!" + System.Environment.NewLine + ex.Message));
			}
		}

		public async Task<JsonResult> JudgeFirstGuess(string gameAccessId, bool correctGuess)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					await unitOfWork.GameRepository.JudgeFirstGuessAsync(gameAccessId, correctGuess);

					return Json(AjaxActionResult.Json(true));
				}
			}
			catch (Exception ex)
			{
				return Json(AjaxActionResult.Fail(-1, "Error processing request!" + System.Environment.NewLine + ex.Message));
			}
		}

		public async Task<JsonResult> JudgeTopGuess(string gameAccessId, int indexGuess)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					await unitOfWork.GameRepository.JudgeTopGuessAsync(gameAccessId, indexGuess);

					return Json(AjaxActionResult.Json(true));
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

		//public async Task<ActionResult> SubmitNewEntity(int idGame, int idEntity, string entityName, string entityDescription)
		//{
		//	try
		//	{
		//		using (UnitOfWork unitOfWork = new UnitOfWork())
		//		{
		//			Tuple<Entity, bool> entity = await unitOfWork.EntityRepository.SubmitNewEntity(idEntity, entityName, entityDescription);
		//			if (entity != null)
		//			{
		//				await unitOfWork.GameRepository.SetCorrectGuess(idGame, entity.Item1.IDEntity, 3);

		//				if (idEntity > 0 || entity.Item2)
		//				{
		//					return Json(
		//						AjaxActionResult.List(
		//							this.RenderPartialView(
		//								"~/Views/Game/Partial/_AnsweredQuestionTemplate.cshtml",
		//								await unitOfWork.GameRepository.GetExpectedAnswers(idGame, entity.Item1.IDEntity)),
		//							entity.Item1));
		//				}
		//				else
		//				{
		//					return Json(AjaxActionResult.Json(true));
		//				}
		//			}
		//			else
		//			{
		//				return Json(AjaxActionResult.Fail(-1, "Failed to add the new entity!"));
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		return Json(AjaxActionResult.Fail(-1, "Error processing request!" + System.Environment.NewLine + ex.Message));
		//	}
		//}

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