using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TwentyQuestions.Repositories
{
	public interface IBaseRepository<TModel> where TModel : class
	{
		IEnumerable<TModel> GetAll();
		Task<TModel> GetByIDAsync(int? id);

		void Create(TModel model);
		void Update(TModel model);
		void Delete(TModel model);
	}
}