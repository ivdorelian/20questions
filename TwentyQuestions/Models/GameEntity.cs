using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TwentyQuestions.Models
{
	/// <summary>
	/// Stores game-entity association.
	/// The entire list of entities for a given game represents that game's instance.
	/// </summary>
	public class GameEntity
	{
		/// <summary>
		/// The ID of the association, used to uniquely identify the association on the server side (PK).
		/// </summary>
		[Key]
		public int IDGameInstance { get; set; }

		/// <summary>
		/// Reference to the game.
		/// </summary>
		public virtual Game Game { get; set; }

		/// <summary>
		/// Reference to the entity.
		/// </summary>
		public virtual Entity Entity { get; set; }

		/// <summary>
		/// The fitness of the entity for the current game, based on the answers given to the questions asked so far.
		/// The higher this is, the more likely that this entity is what the player is thinking of.
		/// 
		/// TODO: when updating the fitness of a game-entity association, take into account the fitness of the question
		///       that was asked as well. Note that this will also affect the certainty percentage calculation.
		/// </summary>
		/// 
		[Index]
		public double Fitness { get; set; }
	}
}