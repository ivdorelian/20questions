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
using TwentyQuestions.ViewModels.EntityQuestions;
using TwentyQuestions.ViewModels.QuestionEntities;

namespace TwentyQuestions.Controllers
{
	public class EntityQuestionsController : BaseController<EntityQuestionsViewModel>
	{
		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.Controllers.GameController class.
		/// </summary>
		public EntityQuestionsController()
			: base(PageType.Entities)
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

						await this.LoadQuestionsCollection(unitOfWork, id.Value);

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

		public async Task<ActionResult> UpdateEntityQuestionFitness(int idEntityQuestion, double newFitness)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					await unitOfWork.EntityQuestionsRepository.UpdateEntityQuestionFitness(idEntityQuestion, newFitness);

					return Json(AjaxActionResult.Json(true));
				}
			}
			catch (Exception ex)
			{
				return this.Redirect("Error: " + ex.Message);
			}
		}

		[Authorize]
		public async Task<ActionResult> SetLockedStatus(int idEntityQuestion, bool setToLocked)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					await unitOfWork.EntityQuestionsRepository.SetLockedStatus(idEntityQuestion, setToLocked);

					return Json(AjaxActionResult.Json(true));
				}
			}
			catch (Exception ex)
			{
				return this.Redirect("Error: " + ex.Message);
			}
		}

		[Authorize]
		public async Task<ActionResult> ForceMajorityAnswer(int idEntityQuestion, string type)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					await unitOfWork.EntityQuestionsRepository.ForceMajorityAnswer(idEntityQuestion, type);

					return Json(AjaxActionResult.Json(true));
				}
			}
			catch (Exception ex)
			{
				return this.Redirect("Error: " + ex.Message);
			}
		}

		private async Task LoadQuestionsCollection(UnitOfWork unitOfWork, int idEntity)
		{
			this.ViewModel.EntityQuestionCollection.Pager.PageSize = GamePlayConstants.EntriesPerPage;

			PagedCollection<EntityQuestion> pagedData = await unitOfWork.EntityQuestionsRepository.GetAllQuestionsForEntityPaged(
				idEntity,
				this.ViewModel.EntityQuestionCollection.Pager.PageIndex);

			if (pagedData != null)
			{
				this.ViewModel.EntityQuestionCollection.Items = pagedData.Items;
				this.ViewModel.EntityQuestionCollection.Pager.TotalItemsCount = pagedData.TotalCount;
			}

			this.ViewModel.PageStatistics = new PageStatistics
			{
				AssociatedQuestionsPercentage = await this.GetAssociatedQuestionsPercentage(idEntity)
			};
		}

		private async Task<double> GetAssociatedQuestionsPercentage(int idEntity)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					return await unitOfWork.EntityQuestionsRepository.GetAssociatedQuestionsPercentage(idEntity);
				}
			}
			catch (Exception ex)
			{ }

			return double.NaN;
		}
	}
}