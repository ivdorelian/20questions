using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwentyQuestions.ViewModels.SpecializedViews
{
	/// <summary>
	/// Represents a list view data-model.
	/// </summary>
	public class ListView<TModel> : View
		where TModel : class
	{
		/// <summary>
		/// Gets or sets an array of items of type TModel.
		/// </summary>
		public TModel[] Items
		{
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.ViewModels.ListView
		/// </summary>
		public ListView()
		{
			this.Items = new TModel[] { };
		}
	}
}