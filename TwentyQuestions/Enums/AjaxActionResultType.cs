using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwentyQuestions.Enums
{
	/// <summary>
	/// Enumerates possible ajax actio result types.
	/// </summary>
	public enum AjaxActionResultType
	{
		/// <summary>
		/// Undefined.
		/// </summary>
		Undefined = 0,


		/// <summary>
		/// Represents a success ajax action result.
		/// </summary>
		Success = 1,


		/// <summary>
		/// Represents a failed ajax action result.
		/// </summary>
		Error = 2,
	}
}