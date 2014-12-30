using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwentyQuestions.Models;

namespace TwentyQuestions.Repositories
{
	public class BaseRepository<TModel> : IBaseRepository<TModel> where TModel : class
	{
		protected DbContext dbContext;
		protected DbSet<TModel> dbSet;

		protected UnitOfWork unitOfWork;

		public BaseRepository(DbContext context, UnitOfWork unitOfWork)
		{
			this.dbContext = context;
			this.dbSet = dbContext.Set<TModel>();
			this.unitOfWork = unitOfWork;
		}

		public System.Collections.Generic.IEnumerable<TModel> GetAll()
		{
			var query = from t in dbSet select t;
			return query;
		}

		public async Task<TModel> GetByIDAsync(int? id)
		{
			return await dbSet.FindAsync(id);
		}

		public void Create(TModel model)
		{
			dbSet.Add(model);
		}

		public void Update(TModel model)
		{
			dbSet.Attach(model);
			dbContext.Entry(model).State = EntityState.Modified;
		}
		public void Delete(TModel model)
		{
			if (dbContext.Entry(model).State == EntityState.Detached)
			{
				dbSet.Attach(model);
			}
			dbSet.Remove(model);
		}
	}

}