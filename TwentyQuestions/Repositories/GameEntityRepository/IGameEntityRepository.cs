using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwentyQuestions.Models;
using TwentyQuestions.ViewModels.Game;

namespace TwentyQuestions.Repositories
{
	public interface IGameEntityRepository : IBaseRepository<GameEntity>
	{
		Task<GameEntity> GetTopGameEntityAsync(Game game);
		Task<TopGuessesViewModel> GetTopGuessesVMAsync(string gameAccessId);
		Task<GameEntity> GetTopGameEntityAsync(Game game, int indexEntity);

	}
}