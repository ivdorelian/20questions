using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TwentyQuestions.Constants;
using TwentyQuestions.Models;
using TwentyQuestions.ViewModels;

namespace TwentyQuestions.Repositories
{
	public class EntityQuestionsRepository : BaseRepository<EntityQuestion>, IEntityQuestionsRepository
	{
		public EntityQuestionsRepository(DbContext context) : base(context) { }

		public async Task<PagedCollection<EntityQuestion>> GetAllEntitiesForQuestionPaged(int idQuestion, int page) // page numbering starts from 1
		{
			if (page < 1)
			{
				return null;
			}

			int totalCount = await this.dbSet.CountAsync(q => q.Question.IDQuestion == idQuestion);
			PagedCollection<EntityQuestion> questionEntities = new PagedCollection<EntityQuestion>
			{
				Items = await this.dbSet
										.Include(e => e.Entity)
										.Include(q => q.Question)
										.Where(q => q.Question.IDQuestion == idQuestion)
										.OrderBy(q => -q.Fitness)
										.Skip((page - 1) * GamePlayConstants.EntriesPerPage)
										.Take(GamePlayConstants.EntriesPerPage)
										.ToArrayAsync(),

				TotalCount = totalCount,
			};

			return questionEntities;
		}

		public async Task<double> GetAssociatedEntitiesPercentage(int idQuestion)
		{
			return Math.Round(100.00 * (await this.dbSet.CountAsync(q => q.Question.IDQuestion == idQuestion)) / (await this.dbContext.Set<Entity>().CountAsync()), 2);
		}

		public async Task<PagedCollection<EntityQuestion>> GetAllQuestionsForEntityPaged(int idEntity, int page) // page numbering starts from 1
		{
			if (page < 1)
			{
				return null;
			}

			int totalCount = await this.dbSet.CountAsync(q => q.Entity.IDEntity == idEntity);
			PagedCollection<EntityQuestion> entityQuestions = new PagedCollection<EntityQuestion>
			{
				Items = await this.dbSet
										.Include(e => e.Entity)
										.Include(q => q.Question)
										.Where(q => q.Entity.IDEntity == idEntity)
										.OrderBy(q => -q.Fitness)
										.Skip((page - 1) * GamePlayConstants.EntriesPerPage)
										.Take(GamePlayConstants.EntriesPerPage)
										.ToArrayAsync(),

				TotalCount = totalCount,
			};

			return entityQuestions;
		}

		public async Task<double> GetAssociatedQuestionsPercentage(int idEntity)
		{
			return Math.Round(100.00 * (await this.dbSet.CountAsync(q => q.Entity.IDEntity == idEntity)) / (await this.dbContext.Set<Question>().CountAsync()), 2);
		}

		public async Task UpdateEntityQuestionFitness(int idEntityQuestion, double newFitness)
		{
			await this.dbContext.Database.ExecuteSqlCommandAsync(
								@"
									update
											EntityQuestions
									set
											Fitness = {0}
									where
											IDEntityQuestion = {1}", newFitness, idEntityQuestion);
		}

		public async Task SetLockedStatus(int idEntityQuestion, bool setToLocked)
		{
			await this.dbContext.Database.ExecuteSqlCommandAsync(
								@"
									update
											EntityQuestions
									set
											Locked = {0}
									where
											IDEntityQuestion = {1}", setToLocked ? 1 : 0, idEntityQuestion);
		}

		public async Task ForceMajorityAnswer(int idEntityQuestion, string type)
		{
			string col = type == "yes" ? "YesCount" : "NoCount";
			string otherCol = type == "yes" ? "NoCount" : "YesCount";
			await this.dbContext.Database.ExecuteSqlCommandAsync(
								@"
									update
											EntityQuestions
									set
											" + col + @" = 1000000,
											" + otherCol + @" = 0,
											UnknownCount = 0
									where
											IDEntityQuestion = {0}", idEntityQuestion);
		}
	}
}