using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using TwentyQuestions.Constants;
using TwentyQuestions.Models;
using TwentyQuestions.ViewModels;

namespace TwentyQuestions.Repositories
{
	public class EntityRepository : BaseRepository<Entity>, IEntityRepository
	{
		private const int ENTITY_NAME_MIN_LENGTH = 3;
		private const int ENTITY_NAME_MAX_LENGTH = 32;

		public EntityRepository(DbContext context) : base(context) { }

		public async Task<Entity[]> EntitiesNamedLike(string needle)
		{
			return await this.dbSet.Where(t => t.Name.Contains(needle)).ToArrayAsync();
		}

		public async Task<Tuple<Entity, bool>> SubmitNewEntity(int idEntity, string entityName, string entityDescription)
		{
			if (idEntity < 0)
			{
				entityName = entityName.Trim();
				if (entityName.Length < ENTITY_NAME_MIN_LENGTH || entityName.Length > ENTITY_NAME_MAX_LENGTH)
				{
					return null;
				}

				// does it already exist?
				Entity existingEntityByName = await this.dbSet.FirstOrDefaultAsync(t => t.Name == entityName);
				if (existingEntityByName != null)
				{
					return new Tuple<Entity, bool>(existingEntityByName, true);
				}

				Entity newEntity = new Entity
				{
					Name = entityName,
					Description = entityDescription.Trim(),
					FirstPlayed = DateTime.Now,
					LastPlayed = DateTime.Now,
					TimesGuessed = 0,
					TimesPlayed = 0
				};

				this.dbSet.Add(newEntity);

				await this.dbContext.SaveChangesAsync();

				return new Tuple<Entity,bool>(newEntity, false);
			}

			return new Tuple<Entity, bool>(await this.dbSet.FirstOrDefaultAsync(t => t.IDEntity == idEntity), false);
		}

		public async Task<PagedCollection<Entity>> GetAllEntitiesPaged(int page) // page numbering starts from 1
		{
			if (page < 1)
			{
				return null;
			}

			PagedCollection<Entity> pageEntities = new PagedCollection<Entity>
			{
				Items = await this.dbSet
										.OrderBy(q => -q.TimesPlayed)
										.Skip((page - 1) * GamePlayConstants.EntriesPerPage)
										.Take(GamePlayConstants.EntriesPerPage)
										.ToArrayAsync(),

				TotalCount = await this.dbSet.CountAsync()
			};

			return pageEntities;
		}

		public async Task UpdateEntityName(int idEntity, string newName)
		{
			Entity toUpdate = await this.dbSet.FirstOrDefaultAsync(e => e.IDEntity == idEntity);

			toUpdate.Name = newName;

			await this.dbContext.SaveChangesAsync();
		}

		public async Task UpdateEntityDescription(int idEntity, string newDescription)
		{
			Entity toUpdate = await this.dbSet.FirstOrDefaultAsync(e => e.IDEntity == idEntity);

			toUpdate.Description = newDescription;

			await this.dbContext.SaveChangesAsync();
		}

		public async Task DeleteEntity(int idEntity)
		{
			Entity toDelete = await this.dbSet.FirstOrDefaultAsync(e => e.IDEntity == idEntity);

			await this.dbContext.Database.ExecuteSqlCommandAsync(@"
											update 
												Games
											set
												GuessedObject_IDEntity = null
											where
												GuessedObject_IDEntity = {0}", idEntity);

			await this.dbContext.Database.ExecuteSqlCommandAsync(@"
											update 
												Games
											set
												PlayedObject_IDEntity = null
											where
												PlayedObject_IDEntity = {0}", idEntity);

			this.dbSet.Remove(toDelete);

			await this.dbContext.SaveChangesAsync();
		}
	}
}