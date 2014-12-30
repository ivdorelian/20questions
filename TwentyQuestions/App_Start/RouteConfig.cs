using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TwentyQuestions
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			//routes.MapRoute(
			//	name: "GameRoute",
			//	url: "Game/{id}",
			//	defaults: new { controller = "Game", action = "Index", id = UrlParameter.Optional }
			//);


			routes.MapRoute(
				name: "GameRoute",
				url: "Game/{id}",
				defaults: new { controller = "Game", action = "Index" },
				constraints: new { id = @"\d+" }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "DetailPageWithPagination",
				url: "{controller}/{action}/{id}/{page}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, page = UrlParameter.Optional }
			);
		}
	}
}
