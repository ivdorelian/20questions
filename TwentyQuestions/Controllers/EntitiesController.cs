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

namespace TwentyQuestions.Controllers
{
	/// <summary>
	/// Represents the ObjectsController class.
	/// </summary>
	public class EntitiesController : BaseController<ViewModels.Entities.EntitiesViewModel>
	{
		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.Controllers.ObjectsController class.
		/// </summary>
		public EntitiesController()
			: base(PageType.Entities)
		{ }

		public async Task<ActionResult> Index(int page = 1)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					this.ViewModel.EntityCollection.Pager.PageIndex = page;

					await this.LoadEntityCollection(unitOfWork);

					return View(this.ViewModel);
				}
			}
			catch (Exception ex)
			{
				return this.Redirect("Error: " + ex.Message);
			}
		}

		private async Task LoadEntityCollection(UnitOfWork unitOfWork)
		{
			this.ViewModel.EntityCollection.Pager.PageSize = GamePlayConstants.EntriesPerPage;

			PagedCollection<Entity> pagedData = await unitOfWork.EntityRepository.GetAllEntitiesPaged(this.ViewModel.EntityCollection.Pager.PageIndex);

			if (pagedData != null)
			{
				this.ViewModel.EntityCollection.Items = pagedData.Items;
				this.ViewModel.EntityCollection.Pager.TotalItemsCount = pagedData.TotalCount;
			}
		}

		[Authorize]
		public async Task<ActionResult> UpdateEntityName(int idEntity, string newName)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					await unitOfWork.EntityRepository.UpdateEntityName(idEntity, newName);

					return Json(AjaxActionResult.Json(true));
				}
			}
			catch (Exception ex)
			{
				return this.Redirect("Error: " + ex.Message);
			}
		}

		[Authorize]
		public async Task<ActionResult> UpdateEntityDescription(int idEntity, string newDescription)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					await unitOfWork.EntityRepository.UpdateEntityDescription(idEntity, newDescription);

					return Json(AjaxActionResult.Json(true));
				}
			}
			catch (Exception ex)
			{
				return this.Redirect("Error: " + ex.Message);
			}
		}

		[Authorize]
		public async Task<ActionResult> DeleteEntity(int idEntity)
		{
			try
			{
				using (UnitOfWork unitOfWork = new UnitOfWork())
				{
					await unitOfWork.EntityRepository.DeleteEntity(idEntity);

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