using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwentyQuestions.Enums;
using TwentyQuestions.Models;

namespace TwentyQuestions.ViewModels.Game
{
	/// <summary>
	/// Represents the game view model.
	/// </summary>
	public class GuessViewModel : LayoutViewModel
	{
		public Models.Game Game { get; set; }

		public List<GameQuestion> AnsweredQuestions
		{
			get;
			set;
		}
	}
}