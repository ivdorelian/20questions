using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwentyQuestions.Constants
{
	public static class DateTimeConstants
	{
		/// <summary>
		/// The .NET datetime format.
		/// 
		/// When you try to change the date time formats keep in mind to change both of the values and to keep them syncronized .
		/// We need both formats because .NET DateTime formats are not the same as jquery-ui datepicker formats.
		/// </summary>
		public const string ServerDateTimeFormat = "dd.MM.yyyy HH:mm:ss";

		public const string ServerDateFormat = "dd.MM.yyyy";

		/// <summary>
		/// The jQuery-ui datetime format.
		/// 
		/// When you try to change the date time formats keep in mind to change both of the values and to keep them syncronized .
		/// We need both formats because .NET DateTime formats are not the same as jquery-ui datepicker formats.
		/// </summary>
		public const string ClientDateTimeFormat = "dd.mm.yy";
	}
}