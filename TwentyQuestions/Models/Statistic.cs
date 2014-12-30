using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TwentyQuestions.Code;
using TwentyQuestions.Enums;

namespace TwentyQuestions.Models
{
	/// <summary>
	/// This table holds various statistics.
	/// 
	/// TODO: is this even necessary? Maybe we should just compute them with queries
	///       and cache the computations for a few minutes, that should be good enough.
	/// </summary>
	public class Statistic
	{
		[Key]
		public int IDStatistic { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public StatType StatType { get; set; }

		/// <summary>
		/// Gets the stat type as string formmatted.
		/// </summary>
		[NotMapped]
		public string StatTypeString
		{
			get
			{
				switch (this.StatType)
				{
					case Enums.StatType.NumberOfObjects:
						{
							return TextResourcesService.GetResourcesText(ResourceStringKeys.StatType.NumberOfObjects);
						}

					case Enums.StatType.NumberOfQuestions:
						{
							return TextResourcesService.GetResourcesText(ResourceStringKeys.StatType.NumberOfQuestions);
						}

					case Enums.StatType.GamesPlayed:
						{
							return TextResourcesService.GetResourcesText(ResourceStringKeys.StatType.GamesPlayed);
						}

					case Enums.StatType.CorrectGuessesRank1:
						{
							return TextResourcesService.GetResourcesText(ResourceStringKeys.StatType.CorrectGuessesRank1);
						}

					case Enums.StatType.GuessesRank210:
						{
							return TextResourcesService.GetResourcesText(ResourceStringKeys.StatType.GuessesRank210);
						}

					case Enums.StatType.GuessesRankOver10:
						{
							return TextResourcesService.GetResourcesText(ResourceStringKeys.StatType.GuessesRankOver10);
						}

					case Enums.StatType.PrematureGuesses:
						{
							return TextResourcesService.GetResourcesText(ResourceStringKeys.StatType.PrematureGuesses);
						}

					case Enums.StatType.FailedGuesses:
						{
							return TextResourcesService.GetResourcesText(ResourceStringKeys.StatType.FailedGuesses);
						}

					case Enums.StatType.GamesResultingInNewObject:
						{
							return TextResourcesService.GetResourcesText(ResourceStringKeys.StatType.GamesResultingInNewObject);
						}

					case Enums.StatType.KnowledgeFactor:
						{
							return TextResourcesService.GetResourcesText(ResourceStringKeys.StatType.KnowledgeFactor);
						}

					case Enums.StatType.DroppedGames:
						{
							return TextResourcesService.GetResourcesText(ResourceStringKeys.StatType.DroppedGames);
						}

					case Enums.StatType.Undefined:
					default:
						return string.Empty;
				}
			}
		}

		/// <summary>
		/// Gets or sets the stat value.
		/// </summary>
		public double StatValue { get; set; }

		/// <summary>
		/// Gets or sets the stat interpretation type.
		/// </summary>
		public StatInterpretationType StatInterpretationType { get; set; }
	}
}