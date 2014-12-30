using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwentyQuestions.Models;

namespace TwentyQuestions.Repositories
{
	public class GameEntityRepository : BaseRepository<GameEntity>, IGameEntityRepository
	{
		public GameEntityRepository(DbContext context) : base(context) { }

		public async Task<Game> StartNewGameInstanceAsync()
		{
			GameRepository gameRepo = new GameRepository(this.dbContext);

			Game newGame = gameRepo.StartNewGame();

			await this.dbContext.SaveChangesAsync();

			await dbContext.Database.ExecuteSqlCommandAsync(
				@"	INSERT INTO GameEntities (
						Game_IDGame,		Entity_IDEntity,		Fitness)
					SELECT
						{0},				[IDEntity],				{1}
					FROM
						Entities", newGame.IDGame, 1);

			await this.dbContext.SaveChangesAsync();


			return newGame;
		}
	}
}