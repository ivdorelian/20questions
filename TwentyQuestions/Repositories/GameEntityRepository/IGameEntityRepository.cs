using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwentyQuestions.Models;

namespace TwentyQuestions.Repositories
{
	public interface IGameEntityRepository : IBaseRepository<GameEntity>
	{
		Task<Game> StartNewGameInstanceAsync();
	}
}