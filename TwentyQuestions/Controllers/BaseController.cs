using Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TwentyQuestions.ViewModels;

namespace TwentyQuestions.Controllers
{
	/// <summary>
	/// Represents the base class for all controllers.
	/// </summary>
	/// <typeparam name="TModel">The type of the ViewModel.</typeparam>
	public abstract class BaseController<TModel> : Controller
		where TModel : TwentyQuestions.ViewModels.LayoutViewModel, new()
	{

		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.Controllers.BaseController class.
		/// </summary>
		/// <param name="pageType">The page type.</param>
		public BaseController(Enums.PageType pageType)
		{
			this.ViewModel = new TModel() { CurrentPage = pageType };
		}


		/// <summary>
		/// Gets the view model instance.
		/// </summary>
		protected TModel ViewModel
		{
			get;
			private set;
		}



		/// <summary>
		/// Gets the resource text.
		/// </summary>
		/// <param name="resourceKey">The resource key.</param>
		/// <returns>System.String representing the resource text.</returns>
		public string GetResouceText(string resourceKey)
		{
			return TextResources.ResourceManager.GetString(resourceKey, CultureInfo.CurrentCulture);
		}

		protected ActionResult AjaxResult(Func<AjaxActionResult> result)
		{
			try
			{
				AjaxActionResult ajaxResult = result();

				return Json(ajaxResult);
			}
			catch (Exception ex)
			{
				return Json(
					new AjaxActionResult()
					{
						ResultType = Enums.AjaxActionResultType.Error,
						ErrorText = ex.Message
					}
				);
			}
		}


		/// <summary>
		/// Invokes a controller action asynchronously.
		/// </summary>
		/// <param name="action">The provided action.</param>
		/// <returns>A System.Web.Mvc.ActionResult.</returns>
		public async Task<ActionResult> ControllerActionInvoker(Func<Task<ActionResult>> action)
		{
			try
			{
				// start timer
				Stopwatch watch = new Stopwatch();
				watch.Start();

				ActionResult result = await action();

				// stop timer
				watch.Stop();

				this.ViewModel.LoadingTime = watch.ElapsedMilliseconds;

				return result;
			}
			catch (Exception ex)
			{
				// handle exception

				return null; // change this
			}
		}




		protected string RenderPartialView(string viewName, object model)
		{
			if (string.IsNullOrEmpty(viewName))
			{
				viewName = ControllerContext.RouteData.GetRequiredString("action");
			}


			ViewData.Model = model;

			using (StringWriter sw = new StringWriter())
			{
				ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
				ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
				viewResult.View.Render(viewContext, sw);

				return sw.GetStringBuilder().ToString();
			}
		}
	}
}