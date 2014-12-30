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
		Task<GamePlayViewModel> GetGamePlayVMAsync(string gameAccessId);
		Task AnswerQuestionAndUpdateInstanceAsync(string gameAccessId, AnswerType answer);

		Task<List<GameQuestion>> GetAnsweredQuestionsAsync(int idGame);
	}
}