using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TwentyQuestions.Enums;

namespace TwentyQuestions.Models
{
	/// <summary>
	/// This table holds the questions asked so far for a given game.
	/// </summary>
	public class GameQuestion
	{
		/// <summary>
		/// The ID of the game-question association, used to uniquely identify the association on the server side (PK).
		/// </summary>
		[Key]
		public int IDGameQuestion { get; set; }

		/// <summary>
		/// Reference to the game.
		/// </summary>
		public virtual Game Game { get; set; }

		/// <summary>
		/// Reference to the question.
		/// </summary>
		public virtual Question Question { get; set; }

		/// <summary>
		/// The index of this asked question. 
		/// The first asked question will have index 1, the second index 2 etc.
		/// </summary>
		public int QuestionIndex { get; set; }

		/// <summary>
		/// The answer given by the player for this question.
		/// 
		/// TODO: make nullable for when no answer has been given yet? 
		///       Right now, no answer = 0, which is defined in the enum as undefined.
		/// </summary>
		public AnswerType GivenAnswer { get; set; }
	}
}