using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwentyQuestions.Models
{
	/// <summary>
	/// Holds the application's resource string keys.
	/// </summary>
	public abstract class ResourceStringKeys
	{
		public abstract class StatType
		{
			public const string NumberOfObjects = "StatType_NumberOfObjects";
			public const string NumberOfQuestions = "StatType_NumberOfQuestions";
			public const string GamesPlayed = "StatType_GamesPlayed";
			public const string CorrectGuessesRank1 = "StatType_CorrectGuessesRank1";
			public const string GuessesRank210 = "StatType_GuessesRank210";
			public const string GuessesRankOver10 = "StatType_GuessesRankOver10";
			public const string PrematureGuesses = "StatType_PrematureGuesses";
			public const string FailedGuesses = "StatType_FailedGuesses";
			public const string GamesResultingInNewObject = "StatType_GamesResultingInNewObject";
			public const string KnowledgeFactor = "StatType_KnowledgeFactor";
			public const string DroppedGames = "StatType_DroppedGames";
		}

	}
}