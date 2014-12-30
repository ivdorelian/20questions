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
	public class TopGuessesViewModel : LayoutViewModel
	{
		/// <summary>
		/// Gets or sets the game identifier.
		/// </summary>
		public string AccessID
		{
			get;
			set;
		}

		public List<Entity> TopGuesses { get; set; }
	}
}