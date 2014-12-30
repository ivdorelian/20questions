using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace TwentyQuestions.Code
{
	/// <summary>
	/// Represents the text resource service of the current application.
	/// </summary>
	public static class TextResourcesService
	{
		/// <summary>
		/// Gets the resource text.
		/// </summary>
		/// <param name="resourceKey">The resource key.</param>
		/// <returns>System.String representing the resource text.</returns>
		public static string GetResourcesText(string resourceKey)
		{
			return TextResources.ResourceManager.GetString(resourceKey, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Gets the resource text.
		/// </summary>
		/// <param name="resourceKey">The resource key.</param>
		/// <param name="cultureInfo">The culture info.</param>
		/// <returns>System.String representing the resource text.</returns>
		public static string GetResourcesText(string resourceKey, CultureInfo cultureInfo)
		{
			return TextResources.ResourceManager.GetString(resourceKey, cultureInfo);
		}
	}
}