using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwentyQuestions.Models;
using TwentyQuestions.ViewModels.SpecializedViews;

namespace TwentyQuestions.ViewModels.EntityQuestions
{
	/// <summary>
	/// Represents the question entities view model.
	/// </summary>
	public class EntityQuestionsViewModel : LayoutViewModel
	{
		/// <summary>
		/// Gets or sets the question collection of the page.
		/// </summary>
		public PagedListView<EntityQuestion> EntityQuestionCollection
		{
			get;
			set;
		}

		public string EntityName
		{
			get
			{
				if (this.EntityQuestionCollection != null && this.EntityQuestionCollection.Items != null && this.EntityQuestionCollection.Items.Length > 0)
				{
					return this.EntityQuestionCollection.Items[0].Entity.Name;
				}

				return string.Empty;
			}
		}

		public string EntityDescription
		{
			get
			{
				if (this.EntityQuestionCollection != null && this.EntityQuestionCollection.Items != null && this.EntityQuestionCollection.Items.Length > 0)
				{
					return this.EntityQuestionCollection.Items[0].Entity.Description;
				}

				return string.Empty;
			}
		}

		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.ViewModels.EntityQuestions.EntityQuestionsViewModel class.
		/// </summary>
		public EntityQuestionsViewModel()
		{
			this.EntityQuestionCollection = new PagedListView<EntityQuestion>();
		}
	}
}