using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TwentyQuestions.Constants;

namespace TwentyQuestions.Models
{
	/// <summary>
	/// This table holds questions.
	/// Questions are asked by the system in order to gather information about what the user is thinking of and ultimately provide a guess.
	/// </summary>
	public class Question
	{
		/// <summary>
		/// The ID of the question, used to uniquely identify questions on the server side (PK).
		/// </summary>
		[Key]
		public int IDQuestion { get; set; }


		[Required(AllowEmptyStrings = false, ErrorMessage = "Question body is missing!")]
		[MinLength(TechnicalConstants.MinimumQuestionLength, ErrorMessage = "Question length is too short!")]
		[MaxLength(TechnicalConstants.MaximumQuestionLength, ErrorMessage = "Question length is too long!")]
		[RegularExpression(@"^[^\?]*\?{1}$", ErrorMessage = "There must be exactly one question mark at the end of the question!")]
		/// <summary>
		/// The body of the question,
		/// for example "Is your character a girl?", "Has your character ever been to space?" etc.
		/// </summary>
		public string QuestionBody { get; set; }

		/// <summary>
		/// The date and time that the question was added into the system.
		/// </summary>
		public DateTime DateAdded { get; set; }

		/// <summary>
		/// The date and time that this question was first asked.
		/// 
		/// Can be nullable because questions are not asked as they are introduced, they are asked later.
		/// </summary>
		public DateTime? FirstAsked { get; set; }

		/// <summary>
		/// The date and time that this question was last asked in a game.
		/// </summary>
		public DateTime? LastAsked { get; set; }

		/// <summary>
		/// The number of times that this question was asked.
		/// 
		/// TODO: maybe this should factor into entity-question association fitnesses somehow?
		/// </summary>
		public int TimesAsked { get; set; }
	}
}