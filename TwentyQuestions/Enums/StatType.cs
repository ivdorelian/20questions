using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwentyQuestions.Enums
{
	/// <summary>
	/// Enumerates possible stat types.
	/// </summary>
	public enum StatType
	{
		/// <summary>
		/// Undefined.
		/// </summary>
		Undefined = 0,

		/// <summary>
		/// The number of Objects.
		/// </summary>
		NumberOfObjects = 1,

		/// <summary>
		/// The number of questions.
		/// </summary>
		NumberOfQuestions = 2,

		/// <summary>
		/// Games played.
		/// </summary>
		GamesPlayed = 3,

		/// <summary>
		/// Correct guesses (rand = 1).
		/// </summary>
		CorrectGuessesRank1 = 4,

		/// <summary>
		/// Guesses with rank between 2 and 10(rank in [2, 10]).
		/// </summary>
		GuessesRank210 = 5,

		/// <summary>
		/// Guesses with rank bigger than 10(rank > 10)
		/// </summary>
		GuessesRankOver10 = 6,

		/// <summary>
		/// Premature Guesses.
		/// </summary>
		PrematureGuesses = 7,

		/// <summary>
		/// Failed Guesses.
		/// </summary>
		FailedGuesses = 8,

		/// <summary>
		/// Games resulting in new Objects.
		/// </summary>
		GamesResultingInNewObject = 9,

		/// <summary>
		/// Knowledge Factor.
		/// </summary>
		KnowledgeFactor = 10,

		/// <summary>
		/// Dropped Games.
		/// </summary>
		DroppedGames = 11,
	}
}