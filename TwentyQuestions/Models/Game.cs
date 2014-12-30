using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwentyQuestions.Models
{
	/// <summary>
	/// This table stores currently the on-going games.
	/// </summary>
	public class Game
	{
		/// <summary>
		/// The ID of a game, used to uniquely identify games on the server side (PK).
		/// </summary>
		[Key]
		public int IDGame { get; set; }

		/// <summary>
		/// The last activity of the current game.
		/// Games with no activity for a while will be deleted.
		/// </summary>
		public DateTime LastActivity { get; set; }

		/// <summary>
		/// The object that was guessed on the first attempt.
		/// 
		/// TODO: make a GameGuesses table that stores all guessing attempts for a game?
		/// </summary>
		public virtual Entity GuessedObject { get; set; }

		/// <summary>
		/// The rank within the internal generated entity hierarchy at which the played object can be found. 
		/// </summary>
		public int? PlayedRank { get; set; }

		/// <summary>
		/// The certainty percentage within the internal generated entity hierarchy of the guessed object.
		/// 
		/// Each entity in an instance gets +2 fitness for each question to which the given answer matches
		/// the most popular answer in that entity-question association. 100% = 2 * QUESTIONS_ASKED_IN_GAME + 1 fitness.
		/// 
		/// TODO: make nullable? Make decimal?
		/// </summary>
		public double CertaintyPercentage { get; set; }

		/// <summary>
		/// The played object, as given to the system by the user at the end of the game.
		/// </summary>
		public virtual Entity PlayedObject { get; set; }
	}
}