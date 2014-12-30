using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace TwentyQuestions.Models
{
	/// <summary>
	/// This table maps entities to questions, providing statistical data that is used in the guessing algorithm.
	/// For each entity, there are a number of questions associated with it. Each association contains the number of
	/// times that each answer was given to that question when the associated entity was played, regardless of whether
	/// the played entity was correctly guessed or not (1). Each association also contains a fitness that describes how 
	/// important this particular question is for this particular entity.
	/// 
	/// TODO:
	/// (1): Is this ok? should we have a more complex updating scheme? 
	/// </summary>
	public class EntityQuestion
	{
		/// <summary>
		/// The ID of each association, used to uniquely identify an association on the server side (PK).
		/// </summary>
		[Key]
		public int IDEntityQuestion { get; set; }

		[Required]
		/// <summary>
		/// The entity of the current association.
		/// </summary>
		public virtual Entity Entity { get; set; }

		[Required]
		/// <summary>
		/// The question of the current association.
		/// </summary>
		public virtual Question Question { get; set; }

		/// <summary>
		/// The fitness. This value describes how fit the question is to identify the associated entity.
		/// 
		/// Higher fitness = more general question.
		/// Lower fitness  = more particular question. TODO: more particular questions should increase the fitness of matched objects more (or the other way around?).
		/// Currently the other way around -- need to experiment.
		/// 
		/// This is updated after each game as follows:
		/// 1. If the game was won on the first attempt, each asked question gets its fitness incremented using the formula:
		/// e^(1 - QuestionIndex / QUESTIONS_ASKED_IN_THE_GAME) (1).
		/// 2. If the game was not won the first attempt, each asked question gets its fitness decremented using the same formula (2).
		/// 
		/// TODO:
		/// (1) Should we have a more complex updating scheme? This does not take into account the statistics for the given answers.
		///     However, should it? That is taken into account when updating the instance fitnesses.
		/// (2) Is this too harsh? What if the game is won on the second attempt? In that case, the question still helped, so it 
		///     should not take such a strong punishment. Maybe run the updates both times: when the game is first loss,
		///     decrement based on the number of questions and question indexes valid then. When it is won,
		///     increment based on the new values. This will lead to a gain in fitness, but a smaller one, and potentiall a decrease
		///     in case of 3rd-4th guesses.
		/// </summary>
		public double Fitness { get; set; }

		/// <summary>
		/// The number of times the answer to this question was Yes for the associated Entity.
		/// </summary>
		public int YesCount { get; set; }

		/// <summary>
		/// The number of times the answer to this question was No for the associated Entity.
		/// </summary>
		public int NoCount { get; set; }

		/// <summary>
		/// The number of times the answer to this question was Unknown for the associated Entity.
		/// </summary>
		public int UnknownCount { get; set; }

		/// <summary>
		/// Gets of sets whether or not this association is locked or not. Locked associations do not get their answers
		/// stats updated.
		/// </summary>
		public bool Locked { get; set; }
	}
}