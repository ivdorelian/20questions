using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwentyQuestions.Models;

namespace TwentyQuestions.ViewModels.Game
{
	/// <summary>
	/// Represents the game questions JSON data-model.
	/// </summary>
	public class GameQuestionsJSONModel
	{
		/// <summary>
		/// Gets or sets the current question.
		/// </summary>
		public Question CurrentQuestion { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating whether or not is the last question.
		/// </summary>
		public bool IsLastQuestion
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the current question index.
		/// </summary>
		public int CurrentQuestionIndex
		{
			get;
			set;
		}
	}
}