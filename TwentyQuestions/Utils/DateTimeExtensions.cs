using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using TwentyQuestions.Constants;

namespace TwentyQuestions.Utils
{
	/// <summary>
	/// DateTime extension functions
	/// </summary>
	public static class DateTimeExtensions
	{

		/// <summary>
		/// Converts a .NET DateTime object to a string which represents a client-side date.
		/// Function uses String.Format to perform conversion.
		/// </summary>
		/// <param name="date">The DateTime object.</param>
		/// <returns>A string representing a client-side date .</returns>
		public static string ToClientSideString(this DateTime date)
		{
			return date.ToClientSideString(DateTimeConstants.ServerDateTimeFormat);
		}

		/// <summary>
		/// Converts a .NET DateTime object to a string which represents a 
		/// client-side date, using the specified format.
		/// Function uses String.Format to perform conversion.
		/// </summary>
		/// <param name="date">The DateTime object.</param>
		/// <param name="format">The conversion format. Will represent the argument of the String.Format function.</param>
		/// <returns>A string representing a client-side date.</returns>
		public static string ToClientSideString(this DateTime date, string format)
		{
			if (date > DateTime.MinValue)
			{
				return string.Format("{0:" + format + "}", date);
			}

			return string.Empty;
		}

		/// <summary>
		/// Converts a .NET DateTime? object to a string which represents a client-side date.
		/// Function uses String.Format to perform conversion.
		/// </summary>
		/// <param name="date">The DateTime object.</param>
		/// <returns>A string representing a client-side date .</returns>
		public static string ToClientSideNullableString(this DateTime? datetime)
		{
			return datetime.ToClientSideNullableString(DateTimeConstants.ServerDateTimeFormat);
		}

		/// <summary>
		/// Converts a .NET DateTime? object to a string which represents a 
		/// client-side date, using the specified format.
		/// Function uses String.Format to perform conversion.
		/// </summary>
		/// <param name="date">The DateTime object.</param>
		/// <param name="format">The conversion format. Will represent the argument of the String.Format function.</param>
		/// <returns>A string representing a client-side date.</returns>
		public static string ToClientSideNullableString(this DateTime? datetime, string format)
		{
			if (datetime.HasValue)
			{
				return datetime.Value.ToClientSideString(format);
			}

			return string.Empty;
		}




		/// <summary>
		/// Converts the client-side date to a .NET DateTime object.
		/// </summary>
		/// <param name="clientSideDate">The client-side date as string.</param>
		/// <returns>A .NET DateTime object if conversion succeeds, DateTime.MinValue otherwise.</returns>
		public static DateTime FromClientSideDate(this string clientSideDate)
		{
			return clientSideDate.FromClientSideDate(DateTimeConstants.ServerDateTimeFormat);
		}

		/// <summary>
		/// Converts the client-side date with a given format to a .NET DateTime object.
		/// </summary>
		/// <param name="clientSideDate">The client-side date as string.</param>
		/// <param name="format">The date format.</param>
		/// <returns>A .NET DateTime object if conversion succeeds, DateTime.MinValue otherwise</returns>
		public static DateTime FromClientSideDate(this string clientSideDate, string format)
		{
			if (!string.IsNullOrEmpty(clientSideDate))
			{
				if (!string.IsNullOrEmpty(clientSideDate))
				{
					DateTime convertedValue = DateTime.MinValue;
					if (!DateTime.TryParseExact(clientSideDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out convertedValue))
					{
						convertedValue = DateTime.MinValue;
					}

					return convertedValue;
				}
			}

			return DateTime.MinValue;
		}

		/// <summary>
		/// Converts the client-side date to a .NET DateTime? object.
		/// </summary>
		/// <param name="clientSideDate">The client-side date as string.</param>
		/// <returns>A .NET DateTime? object if conversion succeeds, null otherwise.</returns>
		public static DateTime? FromClientSideNullableDate(this string clientSideDate)
		{
			return clientSideDate.FromClientSideNullableDate(DateTimeConstants.ServerDateTimeFormat);
		}

		/// <summary>
		/// Converts the client-side date with a given format to a .NET DateTime? object
		/// </summary>
		/// <param name="clientSideDate">The client-side date as string.</param>
		/// <param name="format">The date format.</param>
		/// <returns>A .NET DateTime? object if conversion succeeds, null otherwise</returns>
		public static DateTime? FromClientSideNullableDate(this string clientSideDate, string format)
		{
			if (!string.IsNullOrEmpty(clientSideDate))
			{
				if (!string.IsNullOrEmpty(clientSideDate))
				{
					DateTime convertedValue = DateTime.MinValue;
					if (!DateTime.TryParseExact(clientSideDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out convertedValue))
					{
						convertedValue = DateTime.MinValue;
					}

					if (convertedValue > DateTime.MinValue)
					{
						return convertedValue;
					}
				}
			}

			return null;
		}
	}
}