using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TwentyQuestions.Models;
using TwentyQuestions.ViewModels;

namespace TwentyQuestions.Repositories
{
	public interface IEntityQuestionsRepository : IBaseRepository<EntityQuestion>
	{
		Task<PagedCollection<EntityQuestion>> GetAllEntitiesForQuestionPaged(int idQuestion, int page);

		Task<PagedCollection<EntityQuestion>> GetAllQuestionsForEntityPaged(int idEntity, int page);

		Task<double> GetAssociatedQuestionsPercentage(int idEntity);

		Task<double> GetAssociatedEntitiesPercentage(int idQuestion);

		Task UpdateEntityQuestionFitness(int idEntityQuestion, double newFitness);

		Task SetLockedStatus(int idEntityQuestion, bool setToLocked);

		Task ForceMajorityAnswer(int idEntityQuestion, string type);
	}
}