using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwentyQuestions.ViewModels.Game
{
	/// <summary>
	/// Represents the game view model.
	/// </summary>
	public class GameViewModel : LayoutViewModel
	{
		/// <summary>
		/// Gets or sets the game identifier.
		/// </summary>
		public int IDGame
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the current question index.
		/// </summary>
		public int CurrentQuestionIndex
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the current question index as string.
		/// </summary>
		public string CurrentQuestionIndexAsString
		{
			get
			{
				if (this.CurrentQuestionIndex > 0)
				{
					return this.CurrentQuestionIndex.ToString();
				}

				return string.Empty;
			}
		}

		/// <summary>
		/// Gets or sets the current question body.
		/// </summary>
		public string CurrentQuestionBody
		{
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.ViewModels.Game.GameViewModel class.
		/// </summary>
		public GameViewModel()
		{
			this.IDGame = -1;
			this.CurrentQuestionIndex = 0;
			this.CurrentQuestionBody = string.Empty;
		}
	}
}