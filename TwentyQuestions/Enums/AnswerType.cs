using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwentyQuestions.Enums
{
	/// <summary>
	/// Enumerates possible answer types.
	/// </summary>
	public enum AnswerType
	{
		/// <summary>
		/// Undefined.
		/// </summary>
		Undefined = 0,

		/// <summary>
		/// Yes.(If the player thinks that the asked question is correct)
		/// </summary>
		Yes = 1,

		/// <summary>
		/// No.(If the player thinks that the asked question is not correct)
		/// </summary>
		No = 2,

		/// <summary>
		/// Unknown.(If the player doesn't know if the question is correct or not)
		/// </summary>
		Unknown = 3,

		/// <summary>
		/// ProbablyYes.(If the player thinks that the asked question is probably correct)
		/// </summary>
		ProbablyYes = 4,

		/// <summary>
		/// ProbablyNo.(If the player thinks that the asked question is probably not correct)
		/// </summary>
		ProbablyNo = 5,
	}
}