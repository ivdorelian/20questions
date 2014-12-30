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

		public UnitOfWork()
		{
			dbContext = new ApplicationDbContext();

			if (Debugger.IsAttached)
			{
				this.dbContext.Database.Log = (message) => { Debug.Write(message); };
			}

			this.EntityRepository = new EntityRepository(dbContext);
			this.GameEntityRepository = new GameEntityRepository(dbContext);
			this.GameRepository = new GameRepository(dbContext);
			this.GameQuestionsRepository = new GameQuestionsRepository(dbContext);
			this.QuestionRepository = new QuestionRepository(dbContext);
			this.EntityQuestionsRepository = new EntityQuestionsRepository(dbContext);
		}

		public EntityRepository EntityRepository { get; private set; }
		public GameEntityRepository GameEntityRepository { get; private set; }
		public GameRepository GameRepository { get; private set; }
		public GameQuestionsRepository GameQuestionsRepository { get; private set; }

		/// <summary>
		/// Gets the question repository instance.
		/// </summary>
		public QuestionRepository QuestionRepository { get; private set; }

		/// <summary>
		/// Gets the entity questions repository instance.
		/// </summary>
		public EntityQuestionsRepository EntityQuestionsRepository { get; private set; }

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