using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwentyQuestions.Models;
using TwentyQuestions.ViewModels.SpecializedViews;

namespace TwentyQuestions.ViewModels.QuestionEntities
{
	/// <summary>
	/// Represents the question entities view model.
	/// </summary>
	public class QuestionEntitiesViewModel : LayoutViewModel
	{
		/// <summary>
		/// Gets or sets the question collection of the page.
		/// </summary>
		public PagedListView<EntityQuestion> EntityQuestionCollection
		{
			get;
			set;
		}

		public string QuestionBody
		{
			get
			{
				if (this.EntityQuestionCollection != null && this.EntityQuestionCollection.Items != null && this.EntityQuestionCollection.Items.Length > 0)
				{
					return this.EntityQuestionCollection.Items[0].Question.QuestionBody;
				}

				return string.Empty;
			}
		}

		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.ViewModels.QuestionEntities.QuestionEntitiesViewModel class.
		/// </summary>
		public QuestionEntitiesViewModel()
		{
			this.EntityQuestionCollection = new PagedListView<EntityQuestion>();
		}
	}
}