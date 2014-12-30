using System.Web.Mvc;
using System.Linq;
using TwentyQuestions.ViewModels.Home;
using TwentyQuestions.Repositories;
using System.Threading.Tasks;

namespace TwentyQuestions.Controllers
{
	/// <summary>
	/// Represents the HomeController class.
	/// </summary>
	public class HomeController : BaseController<HomeViewModel>
	{
		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.Controllers.HomeController class.
		/// </summary>
		public HomeController()
			: base(Enums.PageType.Home)
		{ }



		public async Task<ActionResult> Index()
		{
			using (UnitOfWork unitOfWork = new UnitOfWork())
			{
				this.ViewModel.RecentGames = await unitOfWork.GameRepository.GetRecentGames();

				return View(this.ViewModel);
			}
		}

		public ActionResult About()
		{
			return View(this.ViewModel);
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}