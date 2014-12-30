using System.Web;
using System.Web.Optimization;

namespace TwentyQuestions
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
						"~/Scripts/jquery-ui-{version}.js"));



			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
						"~/Scripts/bootstrap.min.js",
						"~/Scripts/respond.min.js"));

			//bundles.Add(new ScriptBundle("~/bundles/less").Include(
			//			"~/Scripts/less/less-1.6.0.min.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
				//"~/Content/bootstrap.css",
				"~/Content/fonts.css",
				"~/Content/jquery-ui-1.10.3.css",
				"~/Content/bootstrap-yeti.css",
				"~/Content/site.css"
			));

			bundles.Add(new ScriptBundle("~/bundles/mvcwebpage").Include(
						"~/Scripts/Plugins/MvcWebPage.js"));


			bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
						"~/Scripts/Plugins/PreventSubmit.js",
						"~/Scripts/Plugins/JSON.js",
						"~/Scripts/Plugins/AjaxRoller.js",
						"~/Scripts/Plugins/ForceNumeric.js",
						"~/Scripts/Plugins/GoogleImageSearch.js"));




			bundles.Add(new ScriptBundle("~/bundles/jquerytmpl").Include(
						"~/Scripts/jquery.tmpl.min.js",
						"~/Scripts/jquery.tmplPlus.min.js"));



			/*
			======================================================================
									Page scripts mapping
			======================================================================
			*/
			bundles.Add(new ScriptBundle("~/pagescripts/game").Include(
						"~/Scripts/PageScripts/GamePage.js"));

			bundles.Add(new ScriptBundle("~/pagescripts/QuestionsPage").Include(
						"~/Scripts/PageScripts/QuestionsPage.js"));

			bundles.Add(new ScriptBundle("~/pagescripts/questionentities").Include(
						"~/Scripts/PageScripts/QuestionEntities.js"));

			bundles.Add(new ScriptBundle("~/pagescripts/entityquestions").Include(
						"~/Scripts/PageScripts/EntityQuestions.js"));

			bundles.Add(new ScriptBundle("~/pagescripts/entity").Include(
			"~/Scripts/PageScripts/Entity.js"));

			//bundles.Add(new StyleBundle("~/Content/less").Include(
			//		"~/Content/less/Style.less"));



			bundles.Add(new LessBundle("~/Content/lessfiles").Include(
						"~/Content/less/*.less"));
		}
	}
}