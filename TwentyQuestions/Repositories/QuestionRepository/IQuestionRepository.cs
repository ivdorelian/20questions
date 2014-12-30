using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwentyQuestions.Models;
using TwentyQuestions.ViewModels;

namespace TwentyQuestions.Repositories
{
	public interface IQuestionRepository : IBaseRepository<Question>
	{
		Task<Question[]> QuestionsNamedLike(string needle);

		Task<PagedCollection<Question>> GetAllQuestionsPaged(int page);

		Task<Question> SubmitNewQuestion(string questionBody);

		Task UpdateQuestionBody(int idQuestion, string newBody);

		Task DeleteQuestion(int idQuestion);
	}
}