using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwentyQuestions.Controllers;
using System.Web.Mvc;

namespace TwentyQuestions.Tests.Controllers
{
	[TestClass]
	public class GameControllerTest
	{
		[TestMethod]
		public void CreateNewGame()
		{
			GameController gameController = new GameController();

			// Act
			ViewResult result = gameController.NewGame().Result as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}
	}
}
