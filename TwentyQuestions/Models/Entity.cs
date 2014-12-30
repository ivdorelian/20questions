using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TwentyQuestions.Utils;

namespace TwentyQuestions.Models
{
	/// <summary>
	/// This table holds entities. 
	/// Entities are things that players can think of and that the system needs to guess by asking questions.
	/// </summary>
	public class Entity
	{
		public const int MAX_DESCRIPTION_LENGTH = 64;

		/// <summary>
		/// The ID of the entity, used only to uniquely identify the entities on the server side (PK).
		/// </summary>
		[Key]
		public int IDEntity { get; set; }

		/// <summary>
		/// The name of the entity, as it will appear to the user / player.
		/// Example: Angela Merkel, Batman, Spiderman.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The date and time that this entity was first played.
		/// </summary>
		public DateTime? FirstPlayed { get; set; }

		[NotMapped]
		public string FirstPlayedString
		{
			get
			{
				return this.FirstPlayed.ToClientSideNullableString();
			}
		}

		/// <summary>
		/// The date and time that this entity was last played.
		/// </summary>
		public DateTime? LastPlayed { get; set; }

		[NotMapped]
		public string LastPlayedString
		{
			get
			{
				return this.LastPlayed.ToClientSideNullableString();
			}
		}

		/// <summary>
		/// The number of times that this entity was played.
		/// </summary>
		public int TimesPlayed { get; set; }

		/// <summary>
		/// The number of times that this entity was correctly guessed on the first attempt.
		/// </summary>
		public int TimesGuessed { get; set; }

		[Required]
		[StringLength(MAX_DESCRIPTION_LENGTH)]
		/// <summary>
		/// The description of the entity. For example: President of the United States.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// True if the entity has been approved by a moderator and false otherwise.
		/// </summary>
		public bool IsActive { get; set; }
	}
}