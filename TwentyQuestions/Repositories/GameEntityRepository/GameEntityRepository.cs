using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwentyQuestions.Models;
using TwentyQuestions.ViewModels.Game;

namespace TwentyQuestions.Repositories
{
	public class GameEntityRepository : BaseRepository<GameEntity>, IGameEntityRepository
	{
		public GameEntityRepository(DbContext context, UnitOfWork unitOfWork) : base(context, unitOfWork) { }

		public async Task<GameEntity> GetTopGameEntityAsync(Game game)
		{
			return await this.dbSet
								.Where(ge => ge.Game.IDGame == game.IDGame)
								.Include(ge => ge.Entity)
								.OrderByDescending(ge => ge.Fitness)
								.Take(1)
								.FirstOrDefaultAsync();
		}

		public async Task<GameEntity> GetTopGameEntityAsync(Game game, int indexEntity)
		{
			return await this.dbSet
								.Where(ge => ge.Game.IDGame == game.IDGame)
								.Include(ge => ge.Entity)
								.OrderByDescending(ge => ge.Fitness)
								.Skip(indexEntity - 1)
								.Take(1)
								.FirstOrDefaultAsync();
		}

		public async Task<TopGuessesViewModel> GetTopGuessesVMAsync(string gameAccessId)
		{
			Game game = await this.unitOfWork.GameRepository.GetGameFromAccessIdAsync(gameAccessId);

			this.unitOfWork.GameRepository.UpdateGameActivityNoSave(game);
			await this.dbContext.SaveChangesAsync();

			TopGuessesViewModel topGuessesVM = new TopGuessesViewModel();
			topGuessesVM.AccessID = gameAccessId;
			topGuessesVM.TopGuesses = await this.dbContext
												.Set<GameEntity>()
												.Where(ge => ge.Game.IDGame == game.IDGame)
												.OrderByDescending(ge => ge.Fitness)
												.Skip(1)
												.Take(Constants.GamePlayConstants.MaxAlternativeEntities)
												.Select(ge => ge.Entity)
												.ToListAsync();

			return topGuessesVM;
		}
	}
}