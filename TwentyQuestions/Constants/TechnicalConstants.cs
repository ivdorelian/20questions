using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwentyQuestions.Constants
{
	public static class TechnicalConstants
	{
		/// <summary>
		/// Delete games older than 20 minutes.
		/// </summary>
		public const int DeleteOldGamesAfterMinutes = 20;

		/// <summary>
		/// Delete old games cache duration. 20 * 60 seconds.
		/// </summary>
		public const int DeleteOldGamesCacheDuration = 20 * 60; // 20 minutes in seconds

		/// <summary>
		/// How many seconds to cache display stuff, like statistics.
		/// </summary>
		public const int GeneralDisplayCacheDuration = 300; // 5 minutes

		/// <summary>
		/// How many recent games entries to display on the front page.
		/// </summary>
		public const int RecentGamesListLength = 10;

		/// <summary>
		/// Gets the minimum question length(10).
		/// </summary>
		public const int MinimumQuestionLength = 10;

		/// <summary>
		/// Gets the maximum question length(256).
		/// </summary>
		public const int MaximumQuestionLength = 256;

		/// <summary>
		/// Gets the Instance Fitness Base Update Value (IFBUV). The IFBUV specifies the base value
		/// used in the instance fitness update formula, which is:
		/// 
		/// IFBUV + (QuestionNumber - 1) / IFUW) * IFS
		/// </summary>
		public const int InstanceFitnessBaseUpdateValue = 2;

		/// <summary>
		/// Gets the Instance Fitness Update Window (IFUW). The IFUW specifies the number of questions
		/// after which entities get their fitness updated with a larger value. For example, a IFUW of length 3
		/// means that the first three questions will update entities by increasing their fitness with the constant Step,
		/// the next 3 questions will update entities by increasing their fitness with the constant k0 + Step etc.
		/// </summary>
		public const int InstanceFitnessUpdateWindow = 20;

		/// <summary>
		/// Gets the Instance Fitness Step (IFS). The IFS specifies by how much each IFUW should increase the added fitness.
		/// </summary>
		public const int InstanceFitnessStep = 2;

		/// <summary>
		/// Gets the Mandatory Number Of Proper Questions (MNPQ). The MNPQ constant specifies how many questions should be asked
		/// before the system can decide to take shortcuts, such as give an early guess, ask questions not associated
		/// with the top guess(es) etc.
		/// </summary>
		public const int MandatoryNumberOfProperQuestions = 10;

		/// <summary>
		/// Gets the Number Of Considered Questions For Binary Split (NCQBS). This specifies how many questions which
		/// were asked at least MTABSQ times should be returned. This should always be high enough to return them all (needed for a subquery).
		/// 
		/// TODO: compute this dynamically to eliminate this constant.
		/// </summary>
		public const int NumberOfConsideredQuestionsForBinarySplit = 50;

		/// <summary>
		/// Gets the Minimum Times Asked For Binary Split Questions (MTABSQ). This specifies how many times a question
		/// needs to have been asked in order to be considered as a binary split question. This should always return
		/// a rather small number of questions.
		/// 
		/// TODO: compute this dynamically to eliminate this constant.
		/// 
		/// Only questions that have been asked at least this many times may be considered for Binary Split Questions.
		/// </summary>
		public const int MinimumTimesAskedForBinarySplitQuestions = 150;

		/// <summary>
		/// Gets the number of top entities to potentially select questions from.
		/// </summary>
		public const int SelectTopQuestionsFromNumberOfEntities = 1;

		/// <summary>
		/// Gets the Mandatory Certainty Percentage Before Shortcuts (MCPBS). The MCPBS constant specifies the minimum
		/// certainty percentage the current top guess must have before the system can decide to take shortcuts, such as give an early guess,
		/// ask questions not associated with the top guess(es) etc.
		/// 
		/// The certainty percentage is never computed until MNPQ questions have been asked.
		/// </summary>
		public const double MandatoryCertaintyPercentageBeforeShortcuts = 80.0;

		/// <summary>
		/// Gets the Mandatory Certainty Percentage Diff Before Shortcuts (MCPDBS). The MCPDBS specifies
		/// the interval that the dfiffence between the top two certainty percentages needs to NOT be in 
		/// in order for the system to decide to take shortcuts, such as give an early guess, 
		/// ask questions not associated with the top guess(es) etc.
		/// 
		/// This is never computed unless the certainty percentage of the current top guess exceeds the MCPBS.
		/// </summary>
		public const double MandatoryMinimumCertaintyPercentageDiffBeforeShortcuts = 0.0;

		public const double MandatoryMaximumCertaintyPercentageDiffBeforeShortcuts = 15.0;

		/// <summary>
		/// Entity-Question associations for which the given answer was No will get their fitness updated, but the
		/// update value will be multiplied by this coefficient.
		/// </summary>
		public const double CorrectNoAnswerFitnessUpdateCoefficient = 0.3;

		/// <summary>
		/// Gets the bonus fitness given for correct answering a bullseye question. A bullseye question
		/// is a question for which a correct answer almost definitely reduces the search space to the
		/// entities for which the question is marked as a bullseye question.
		/// 
		/// TODO: Use this.
		/// </summary>
		public const double BonusFitnessForBullsEye = 6; 

		/// <summary>
		/// Gets the Mandatory Early Guess Number Of Questions (MEGNQ). The MEGNQ 
		/// specifies the minimum number of questions that need to be asked before the system will give an early
		/// guess.
		/// 
		/// Note that this only happens iff all shortcut conditions still hold.
		/// </summary>
		public const double MandatoryEarlyGuessNumberOfQuestions = 16;

		/// <summary>
		/// Gets the Early Guess Probability (EGP). Even if we can early guess,
		/// we only do it with this probability.
		/// 
		/// Higher means focus on learning.
		/// Lower means focus on performing.
		/// </summary>
		public const double EarlyGuessProbability = 0.7;

		/// <summary>
		/// Gets the Pick Amongst Number. The PAN specifies how many questions should be selected as
		/// potential next questions. Out of these, a random one will be asked.
		/// 
		/// TODO: Make this more involved? The general questions might be ok to vary more, but the latter
		/// ones maybe should be more fixed.
		/// </summary>
		public const int PickAmongstNumber = 3;

		/// <summary>
		/// Gets the GameIdLength, meaning the length of the string identifiers that uniquely identify games to users.
		/// </summary>
		public const int GameIdLength = 10;
	}
}