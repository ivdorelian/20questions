using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwentyQuestions.ViewModels
{
	/// <summary>
	/// Represents a paged collection.
	/// </summary>
	/// <typeparam name="TModel">The type of items in the collection</typeparam>
	public class PagedCollection<TModel>
	{
		/// <summary>
		/// Gets or sets the the array of items
		/// </summary>
		public TModel[] Items { get; set; }

		/// <summary>
		/// Gets or sets the total items count.
		/// </summary>
		public int TotalCount { get; set; }

		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.ViewModels.PagedCollection class.
		/// </summary>
		public PagedCollection()
		{
			this.TotalCount = 0;
		}
	}
}