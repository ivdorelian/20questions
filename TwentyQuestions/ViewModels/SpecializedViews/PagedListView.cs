using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwentyQuestions.ViewModels.SpecializedViews
{
	/// <summary>
	/// Represents a paged list view data-model.
	/// </summary>
	public class PagedListView<TModel> : ListView<TModel>
		where TModel : class
	{
		/// <summary>
		/// Gets or sets the pager data-model.
		/// </summary>
		public Pager Pager
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the current number of the item in total item collection.
		/// </summary>
		/// <param name="index">The index of the item in the current page</param>
		/// <returns>System.Int32 representing the current position of the item in the total item collection</returns>
		public int NrCrt(int index)
		{
			return (((this.Pager.PageIndex - 1) * this.Pager.PageSize) + index + 1);
		}

		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.ViewModels.PagedListView
		/// </summary>
		public PagedListView()
		{
			this.Pager = new Pager();
		}
	}
}