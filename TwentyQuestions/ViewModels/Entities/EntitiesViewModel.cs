using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwentyQuestions.Models;
using TwentyQuestions.ViewModels.SpecializedViews;

namespace TwentyQuestions.ViewModels.Entities
{
	/// <summary>
	/// Represents the objects view model.
	/// </summary>
	public class EntitiesViewModel : LayoutViewModel
	{

		/// <summary>
		/// Gets or sets the entity collection of the page.
		/// </summary>
		public PagedListView<Entity> EntityCollection
		{
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.ViewModels.Entities.EntitiesViewModel class.
		/// </summary>
		public EntitiesViewModel()
		{
			this.EntityCollection = new PagedListView<Entity>();
		}

	}
}