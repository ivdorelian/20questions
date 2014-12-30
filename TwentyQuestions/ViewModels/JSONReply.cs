using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwentyQuestions.ViewModels
{
	/// <summary>
	/// Represents a JSON reply object.
	/// </summary>
	public class JSONReply
	{
		/// <summary>
		/// Gets or sets the object representing the data of the JSONReply.
		/// </summary>
		public object Data
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag indicating whether the reply was successful or not.
		/// </summary>
		public bool Success
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the error code of the JSONReply.
		/// </summary>
		public int ErrorCode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the error text of the JSONReply.
		/// </summary>
		public string ErrorText
		{
			get;
			set;
		}


		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.ViewModels.JSONReply class.
		/// </summary>
		public JSONReply()
		{
			this.Data = null;
			this.Success = false;
			this.ErrorCode = 1;
			this.ErrorText = string.Empty;
		}
	}
}