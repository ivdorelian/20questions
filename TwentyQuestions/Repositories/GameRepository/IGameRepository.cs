using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwentyQuestions.Models;
using TwentyQuestions.ViewModels.Game;

namespace TwentyQuestions.Repositories
{
	public interface IGameRepository : IBaseRepository<Game>
	{
		Game StartNewGame();

		Task UpdateGameActivity(int idGame);

		Task DeleteOldGames();

		Task<Game> GetGameResult(int idGame);

		Task<GameQuestionExpectedAnswerJSONModel[]> GetExpectedAnswers(int idGame, int idEntity);

		Task SetCorrectGuess(int idGame, int idGuessedEntity, int attempt);

		Task<Entity[]> GetTopGuesses(int idGame);

		Task SetIncorrectGuess(int idGame, int idGuessedEntity, int attempt);

		Task<Game[]> GetRecentGames();
	}
}