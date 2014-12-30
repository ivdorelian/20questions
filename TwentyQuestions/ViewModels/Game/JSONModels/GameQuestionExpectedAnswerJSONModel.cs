using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwentyQuestions.Enums;
using TwentyQuestions.Models;
using TwentyQuestions.Utils;

namespace TwentyQuestions.ViewModels.Game
{
	/// <summary>
	/// Represents the game question expected answer json model.
	/// </summary>
	public class GameQuestionExpectedAnswerJSONModel
	{
		/// <summary>
		/// Gets or sets the game question data-model.
		/// </summary>
		public GameQuestion GameQuestion
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the entity question data-model.
		/// </summary>
		public EntityQuestion EntityQuestion
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the expected answer of the game question.
		/// </summary>
		public AnswerType ExpectedAnswer
		{
			get
			{
				if (this.EntityQuestion != null)
				{

					return
						this.EntityQuestion.YesCount == NumericUtils.Max(this.EntityQuestion.UnknownCount, this.EntityQuestion.NoCount, this.EntityQuestion.YesCount) ? AnswerType.Yes :
						this.EntityQuestion.NoCount == NumericUtils.Max(this.EntityQuestion.UnknownCount, this.EntityQuestion.YesCount, this.EntityQuestion.NoCount) ? AnswerType.No :
						AnswerType.Unknown;
				}

				return AnswerType.Undefined;
			}
		}

		/// <summary>
		/// Gets the expected answer as string.
		/// </summary>
		public string ExpectedAnswerString
		{
			get
			{
				if (this.ExpectedAnswer != AnswerType.Undefined)
				{
					return this.ExpectedAnswer.ToString();
				}

				return string.Empty;
			}
		}

		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.ViewModels.Game.GameQuestionExpectedAnswerJSONModel class.
		/// </summary>
		public GameQuestionExpectedAnswerJSONModel()
		{

		}
	}
}