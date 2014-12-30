using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using TwentyQuestions.Models;
using TwentyQuestions.Constants;
using TwentyQuestions.ViewModels;

namespace TwentyQuestions.Repositories
{
	public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
	{
		public QuestionRepository(DbContext context, UnitOfWork unitOfWork) : base(context, unitOfWork) { }

		public async Task<Question[]> QuestionsNamedLike(string needle)
		{
			return await this.dbSet.Where(t => t.QuestionBody.Contains(needle)).Take(GamePlayConstants.MaxQuestionSubmitAutoCompleteEntries).ToArrayAsync();
		}

		public async Task<PagedCollection<Question>> GetAllQuestionsPaged(int page) // page numbering starts from 1
		{
			if (page < 1)
			{
				return null;
			}

			PagedCollection<Question> pageQuestions = new PagedCollection<Question>
			{
				Items = await this.dbSet
										.OrderBy(q => -q.TimesAsked)
										.Skip((page - 1) * GamePlayConstants.EntriesPerPage)
										.Take(GamePlayConstants.EntriesPerPage)
										.ToArrayAsync(),

				TotalCount = await this.dbSet.CountAsync()
			};

			return pageQuestions;
		}

		public async Task<Question> SubmitNewQuestion(string questionBody)
		{
			Question newQuestion = new Question
			{
				DateAdded = DateTime.Now,
				FirstAsked = null,
				LastAsked = null,
				TimesAsked = 0,
				QuestionBody = questionBody.Trim()
			};

			if (await this.dbSet.FirstOrDefaultAsync(q => q.QuestionBody == questionBody) == null)
			{
				this.dbSet.Add(newQuestion);

				await this.dbContext.SaveChangesAsync();

				return newQuestion;
			}

			return null;
		}

		public async Task UpdateQuestionBody(int idQuestion, string newBody)
		{
			Question toUpdate = await this.dbSet.FirstOrDefaultAsync(e => e.IDQuestion == idQuestion);

			toUpdate.QuestionBody = newBody;

			await this.dbContext.SaveChangesAsync();
		}

		public async Task DeleteQuestion(int idQuestion)
		{
			Question toDelete = await this.dbSet.FirstOrDefaultAsync(q => q.IDQuestion == idQuestion);

			this.dbSet.Remove(toDelete);

			await this.dbContext.SaveChangesAsync();
		}

	}
}