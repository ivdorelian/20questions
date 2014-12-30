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
using TwentyQuestions.ViewModels.QuestionEntities;

namespace TwentyQuestions.Controllers
{
	public class QuestionEntitiesController : BaseController<QuestionEntitiesViewModel>
	{
		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.Controllers.GameController class.
		/// </summary>
		public QuestionEntitiesController()
			: base(PageType.Questions)
		{ }

		public async Task<ActionResult> Index(int? id = null, int page = 1)
		{
			if (id.HasValue)
			{
				try
				{
					using (UnitOfWork unitOfWork = new UnitOfWork())
					{
						this.ViewModel.EntityQuestionCollection.Pager.PageIndex = page;

						await this.LoadEntitiesCollection(unitOfWork, id.Value);

						return View(this.ViewModel);
					}
				}
				catch (Exception ex)
				{
					return this.Redirect("Error: " + ex.Message);
				}
			}

			return this.View(this.ViewModel);
		}

		public async Task<ActionResult> UpdateQuestionEntityFitness(int idQuestionEntity, double newFitness)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					await unitOfWork.EntityQuestionsRepository.UpdateEntityQuestionFitness(idQuestionEntity, newFitness);

					return Json(AjaxActionResult.Json(true));
				}
			}
			catch (Exception ex)
			{
				return this.Redirect("Error: " + ex.Message);
			}
		}

		private async Task LoadEntitiesCollection(UnitOfWork unitOfWork, int idQuestion)
		{
			this.ViewModel.EntityQuestionCollection.Pager.PageSize = GamePlayConstants.EntriesPerPage;

			PagedCollection<EntityQuestion> pagedData = await unitOfWork.EntityQuestionsRepository.GetAllEntitiesForQuestionPaged(
				idQuestion,
				this.ViewModel.EntityQuestionCollection.Pager.PageIndex);

			if (pagedData != null)
			{
				this.ViewModel.EntityQuestionCollection.Items = pagedData.Items;
				this.ViewModel.EntityQuestionCollection.Pager.TotalItemsCount = pagedData.TotalCount;
			}

			this.ViewModel.PageStatistics = new PageStatistics
			{
				AssociatedEntitiesPercentage = await this.GetAssociatedQuestionsPercentage(idQuestion)
			};
		}

		private async Task<double> GetAssociatedQuestionsPercentage(int idQuestion)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					return await unitOfWork.EntityQuestionsRepository.GetAssociatedEntitiesPercentage(idQuestion);
				}
			}
			catch (Exception ex)
			{ }

			return double.NaN;
		}
	}
}