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
	public interface IUnitOfWork
	{
		IEntityRepository EntityRepository { get; }
		IGameEntityRepository GameEntityRepository { get; }
		IGameRepository GameRepository { get; }
		IGameQuestionsRepository GameQuestionsRepository { get; }
		IEntityQuestionsRepository EntityQuestionsRepository { get; }
		IQuestionRepository QuestionRepository { get; }

		Task SaveAsync();
	}
}