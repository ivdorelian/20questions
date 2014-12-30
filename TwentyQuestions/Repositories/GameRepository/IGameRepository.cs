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
	public interface IGameRepository : IBaseRepository<Game>
	{
		Task<string> StartNewGame();

		Task JudgeFirstGuessAsync(string gameAccessId, bool correctGuess);

		Task JudgeTopGuessAsync(string gameAccessId, int indexGuess);

		void UpdateGameActivityNoSave(Game game);

		Task<GuessViewModel> GetGuessVMAsync(string gameAccessId);

		Task DeleteOldGames();

		Task<Game[]> GetRecentGames();

		Task<Game> GetGameFromAccessIdAsync(string gameAccessId);

		Task<GameState> GetGameStateAsync(int idGame);

		Task<GameState> GetGameStateAsync(string gameAccessId);
	}
}