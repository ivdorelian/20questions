using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TwentyQuestions.Constants;
using TwentyQuestions.Enums;
using TwentyQuestions.Models;
using TwentyQuestions.Repositories;
using TwentyQuestions.ViewModels;
using TwentyQuestions.ViewModels.Questions;

namespace TwentyQuestions.Controllers
{
	/// <summary>
	/// Represents the QuestionsController class.
	/// </summary>
	public class QuestionsController : BaseController<QuestionsViewModel>
	{
		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.Controllers.QuestionsController class.
		/// </summary>
		public QuestionsController()
			: base(PageType.Questions)
		{ }

		public async Task<ActionResult> Index(int page = 1)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					this.ViewModel.QuestionCollection.Pager.PageIndex = page;

					await this.LoadQuestionCollection(unitOfWork);

					return View(this.ViewModel);
				}
			}
			catch (Exception ex)
			{
				return this.Redirect("Error");
			}
		}

		public async Task<ActionResult> GetQuestionsForAutocomplete(string needle)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					Question[] questionList = await unitOfWork.QuestionRepository.QuestionsNamedLike(needle);

					return Json(AjaxActionResult.Json(questionList));
				}
			}
			catch (Exception ex)
			{
				return Json(AjaxActionResult.Fail(-1, "Error processing request!" + System.Environment.NewLine + ex.Message));
			}
		}

		[HttpPost]
		public async Task<ActionResult> SubmitNewQuestion(QuestionsViewModel model)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					if (!this.ModelState.IsValid)
					{
						await this.LoadQuestionCollection(unitOfWork);

						return View("Index", this.ViewModel);
					}

					Question question = await unitOfWork.QuestionRepository.SubmitNewQuestion(model.SubmittedQuestion.QuestionBody);
					if (question == null)
					{
						ModelState.AddModelError("AlreadyExists", "That question already exists!");
					}
					else
					{
						ModelState.Clear();
						this.ViewModel.SuccessfulSubmission = true;
					}

					await this.LoadQuestionCollection(unitOfWork);

					return View("Index", this.ViewModel);
				}
			}
			catch (Exception ex)
			{
				return this.Redirect("Error");
			}
		}

		private async Task LoadQuestionCollection(UnitOfWork unitOfWork)
		{
			this.ViewModel.QuestionCollection.Pager.PageSize = GamePlayConstants.EntriesPerPage;

			PagedCollection<Question> pagedData = await unitOfWork.QuestionRepository.GetAllQuestionsPaged(this.ViewModel.QuestionCollection.Pager.PageIndex);

			if (pagedData != null)
			{
				this.ViewModel.QuestionCollection.Items = pagedData.Items;
				this.ViewModel.QuestionCollection.Pager.TotalItemsCount = pagedData.TotalCount;
			}
		}

		[Authorize]
		public async Task<ActionResult> UpdateQuestionBody(int idQuestion, string newBody)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					await unitOfWork.QuestionRepository.UpdateQuestionBody(idQuestion, newBody);

					return Json(AjaxActionResult.Json(true));
				}
			}
			catch (Exception ex)
			{
				return this.Redirect("Error: " + ex.Message);
			}
		}

		[Authorize]
		public async Task<ActionResult> DeleteQuestion(int idQuestion)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					await unitOfWork.QuestionRepository.DeleteQuestion(idQuestion);

					return Json(AjaxActionResult.Json(true));
				}
			}
			catch (Exception ex)
			{
				return this.Redirect("Error: " + ex.Message);
			}
		}
	}
}