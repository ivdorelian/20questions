using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwentyQuestions.Enums;

namespace TwentyQuestions.ViewModels
{
	/// <summary>
	/// Represents the base class for all viewmodels.
	/// </summary>
	public class LayoutViewModel
	{
		/// <summary>
		/// Gets or sets the current page type.
		/// </summary>
		public PageType CurrentPage
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the current page's load time in milliseconds.
		/// </summary>
		public long LoadingTime
		{
			get;
			set;
		}

		public PageStatistics PageStatistics { get; set; }

		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.ViewModels.LayoutViewModel class.
		/// </summary>
		public LayoutViewModel()
		{
			this.CurrentPage = PageType.Undefined;
			this.PageStatistics = new PageStatistics();
		}
	}
}