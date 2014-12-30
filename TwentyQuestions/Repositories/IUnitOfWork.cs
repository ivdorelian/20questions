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
		EntityRepository EntityRepository { get; }
		GameEntityRepository GameEntityRepository { get; }
		GameRepository GameRepository { get; }

		/// <summary>
		/// Gets the question repository instance.
		/// </summary>
		QuestionRepository QuestionRepository { get; }

		Task SaveAsync();
	}
}