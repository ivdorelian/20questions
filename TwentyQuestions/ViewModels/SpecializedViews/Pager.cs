using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwentyQuestions.ViewModels.SpecializedViews
{
	/// <summary>
	/// Represents the pager data-model.
	/// </summary>
	public class Pager
	{
		/// <summary>
		/// Gets or sets the total items count.
		/// </summary>
		public int TotalItemsCount
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the total pages count of the items in the list view
		/// </summary>
		public int PageCount
		{
			get
			{
				return (this.TotalItemsCount == 0) ? 0 : ((this.TotalItemsCount < this.PageSize) ? 1 : (this.TotalItemsCount % this.PageSize != 0) ? ((this.TotalItemsCount / this.PageSize) + 1) : this.TotalItemsCount / this.PageSize); // we get the total number of pages that we can build
			}
		}

		/// <summary>
		/// Gets or sets the current page index in the paged list view.
		/// </summary>
		public int PageIndex
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the page size in the paged list view.
		/// </summary>
		public int PageSize
		{
			get;
			set;
		}

		/// <summary>
		/// Flag indicating whether the page requested exists or not.
		/// </summary>
		public bool PageExists
		{
			get
			{
				return this.PageIndex <= this.PageCount;
			}
		}

		/// <summary>
		/// Returns TRUE if its not the LAST subset of the item context collection.
		/// </summary>
		public bool HasNextPage
		{
			get
			{
				return this.PageExists && this.PageIndex < this.PageCount;
			}
		}

		/// <summary>
		/// Returns TRUE if its not the FIRST subset of the item context collection.
		/// </summary>
		public bool HasPreviousPage
		{
			get
			{
				return (this.PageIndex != 1 && this.PageIndex <= this.PageCount && this.PageExists);
			}
		}


		/// <summary>
		/// Gets or sets the pager detail.
		/// </summary>
		public int PagerDetail
		{
			get;
			set;
		}

		public Pager()
		{
			this.TotalItemsCount = 0;
			this.PageIndex = 1;
			this.PageSize = 25;
			this.PagerDetail = 5;
		}
	}
}