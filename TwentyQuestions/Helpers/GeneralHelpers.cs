using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TwentyQuestions.Helpers
{
	public static class GeneralHelpers
	{
		public static string NumberWithOrderSuffix(this HtmlHelper helper, int number)
		{
			int lastDigit = number % 10;
			int lastTwoDigits = number % 100;

			if (lastTwoDigits > 9 && lastTwoDigits < 20)
			{
				return number + "th";
			}

			if (lastDigit == 1)
			{
				return number + "st";
			}
			else if (lastDigit == 2)
			{
				return number + "nd";
			}
			else if (lastDigit == 3)
			{
				return number + "rd";
			}

			return number + "th";
		}
	}
}