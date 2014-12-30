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
	public class GamePlayViewModel : LayoutViewModel
	{
		/// <summary>
		/// Gets or sets the game identifier.
		/// </summary>
		public string AccessID
		{
			get;
			set;
		}

		public List<GameQuestion> AnsweredQuestions
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the current question body.
		/// </summary>
		public Question CurrentQuestion
		{
			get;
			set;
		}

		public bool IsLastQuestion
		{
			get;
			set;
		}
	}
}