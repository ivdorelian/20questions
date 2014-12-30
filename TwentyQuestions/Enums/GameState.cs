using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwentyQuestions.Enums
{
	public enum GameState
	{
		Undefined = 0,
		Playing,
		LastQuestionAnswered,
		FirstGuessMarkedCorrect,
		FirstGuessMarkedIncorrect,
		SelectedFromTopGuessesList,
		MustEnterWhoItWas,
		EnteredWhoItWas,
		ClosedBeforeProperFinish
	}
}