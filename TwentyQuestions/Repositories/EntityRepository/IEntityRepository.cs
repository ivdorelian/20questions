using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwentyQuestions.Models;
using TwentyQuestions.ViewModels;

namespace TwentyQuestions.Repositories
{
	public interface IEntityRepository : IBaseRepository<Entity>
	{
		Task<Entity[]> EntitiesNamedLike(string needle);

		Task<Tuple<Entity, bool>> SubmitNewEntity(int idEntity, string entityName, string entityDescription);

		Task<PagedCollection<Entity>> GetAllEntitiesPaged(int page);

		Task UpdateEntityName(int idEntity, string newName);

		Task UpdateEntityDescription(int idEntity, string newName);

		Task DeleteEntity(int idEntity);
	}
}