using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwentyQuestions.Enums;
using TwentyQuestions.Models;
using TwentyQuestions.ViewModels.Game;

namespace TwentyQuestions.Repositories
{
	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		private ApplicationDbContext dbContext;

		private IEntityRepository entityRepository;
		private IGameEntityRepository gameEntityRepository;
		private IGameRepository gameRepository;
		private IGameQuestionsRepository gameQuestionsRepository;
		private IQuestionRepository questionRepository;
		private IEntityQuestionsRepository entityQuestionsRepository;

		public UnitOfWork()
		{
			dbContext = new ApplicationDbContext();

			if (Debugger.IsAttached)
			{
				this.dbContext.Database.Log = (message) => { Debug.Write(message); };
			}
		}

		public IEntityRepository EntityRepository
		{
			get
			{
				if (this.entityRepository == null)
				{
					this.entityRepository = new EntityRepository(this.dbContext, this);
				}

				return this.entityRepository;
			}
		}

		public IGameEntityRepository GameEntityRepository
		{
			get
			{
				if (this.gameEntityRepository == null)
				{
					this.gameEntityRepository = new GameEntityRepository(this.dbContext, this);
				}

				return this.gameEntityRepository;
			}
		}
		public IGameRepository GameRepository
		{
			get
			{
				if (this.gameRepository == null)
				{
					this.gameRepository = new GameRepository(this.dbContext, this);
				}

				return this.gameRepository;
			}
		}
		public IGameQuestionsRepository GameQuestionsRepository
		{
			get
			{
				if (this.gameQuestionsRepository == null)
				{
					this.gameQuestionsRepository = new GameQuestionsRepository(this.dbContext, this);
				}

				return this.gameQuestionsRepository;
			}
		}
		public IQuestionRepository QuestionRepository
		{
			get
			{
				if (this.questionRepository == null)
				{
					this.questionRepository = new QuestionRepository(this.dbContext, this);
				}

				return this.questionRepository;
			}
		}
		public IEntityQuestionsRepository EntityQuestionsRepository
		{
			get
			{
				if (this.entityQuestionsRepository == null)
				{
					this.entityQuestionsRepository = new EntityQuestionsRepository(this.dbContext, this);
				}

				return this.entityQuestionsRepository;
			}
		}

		public async Task SaveAsync()
		{
			await this.dbContext.SaveChangesAsync();
		}

		public void Dispose()
		{
			if (this.dbContext != null)
			{
				this.dbContext.Dispose();
			}
		}
	}
}