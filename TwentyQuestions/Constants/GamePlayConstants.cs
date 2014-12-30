using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwentyQuestions.Constants
{
	public static class GamePlayConstants
	{
		public const int MaxQuestionsUntilFirstGuess = 20;
		public const int MaxAlternativeEntities = 15;
		public const int MaxQuestionSubmitAutoCompleteEntries = 10;

		public const int EntriesPerPage = 25;

		public const int PlayedRankNewEntity = -1;
	}
}