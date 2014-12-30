using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwentyQuestions.Enums;
using TwentyQuestions.Models;
using TwentyQuestions.ViewModels.Game;

namespace TwentyQuestions.Repositories
{
	public interface IGameQuestionsRepository : IBaseRepository<GameQuestion>
	{
		Task<GameQuestionsJSONModel> GetNextQuestionAsync(int idGame);
		Task<GameQuestionExpectedAnswerJSONModel[]> GetQuestionsAnsweredForGameAsync(int idGame);
		Task AnswerQuestionAndUpdateInstanceAsync(int idGame, int questionIndex, AnswerType answer);
	}
}