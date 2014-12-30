using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwentyQuestions.Models;
using TwentyQuestions.ViewModels.SpecializedViews;

namespace TwentyQuestions.ViewModels.Questions
{
	/// <summary>
	/// Represents the questions view model.
	/// </summary>
	public class QuestionsViewModel : LayoutViewModel
	{
		/// <summary>
		/// Gets or sets the question collection of the page.
		/// </summary>
		public PagedListView<Question> QuestionCollection
		{
			get;
			set;
		}

		public bool SuccessfulSubmission
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the submitted questions.
		/// </summary>
		public Question SubmittedQuestion
		{
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.ViewModels.Questions.QuestionsViewModel class.
		/// </summary>
		public QuestionsViewModel()
		{
			this.SuccessfulSubmission = false;
			this.QuestionCollection = new PagedListView<Question>();
			this.SubmittedQuestion = new Question();
		}
	}
}