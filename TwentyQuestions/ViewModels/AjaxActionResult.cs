using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TwentyQuestions.Enums;

namespace TwentyQuestions.ViewModels
{
	/// <summary>
	/// Represents an ajax action result.
	/// </summary>
	public class AjaxActionResult
	{
		/// <summary>
		/// Gets or sets the ajax action result type.
		/// </summary>
		public AjaxActionResultType ResultType
		{
			get;
			set;
		}

		/// <summary>
		/// Represents the error code.
		/// </summary>
		public int ErrorCode
		{
			get;
			set;
		}

		/// <summary>
		/// Represents the error text.
		/// </summary>
		public string ErrorText
		{
			get;
			set;
		}

		public object Data
		{
			get;
			set;
		}


		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.ViewModels.AjaxActionResult class.
		/// </summary>
		public AjaxActionResult()
		{
			this.ResultType = AjaxActionResultType.Success;
			this.ErrorCode = 1;
			this.ErrorText = string.Empty;
		}

		/// <summary>
		/// Represents a failed ajax action result.
		/// </summary>
		/// <param name="errorCode">The error code.</param>
		/// <returns></returns>
		public static AjaxActionResult Fail(int errorCode)
		{
			return AjaxActionResult.Fail(errorCode, string.Empty);
		}

		/// <summary>
		/// Represents a failed ajax action result.
		/// </summary>
		/// <param name="errorCode">The error code.</param>
		/// <param name="errorText">The error text.</param>
		/// <returns></returns>
		public static AjaxActionResult Fail(int errorCode, string errorText)
		{
			return new AjaxActionResult() { ResultType = AjaxActionResultType.Error, ErrorCode = errorCode, ErrorText = errorText };
		}

		/// <summary>
		/// Represents an ajax action result with multiple results.
		/// </summary>
		/// <param name="resultList"></param>
		/// <returns></returns>
		public static AjaxActionResult List(params object[] resultList)
		{
			return new AjaxActionResult()
			{
				Data = resultList,
				ResultType = AjaxActionResultType.Success
			};
		}

		/// <summary>
		/// Represents an ajax action result that sends the data as json.
		/// </summary>
		/// <param name="data">The object data.</param>
		/// <returns></returns>
		public static AjaxActionResult Json(object data)
		{
			return new AjaxActionResult()
			{
				Data = data,
				ResultType = AjaxActionResultType.Success
			};
		}
	}
}