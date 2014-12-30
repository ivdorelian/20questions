using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TwentyQuestions.Models
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public DbSet<Game> Games { get; set; }
		public DbSet<Entity> Entities { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<GameEntity> GameInstances { get; set; }
		public DbSet<GameQuestion> GameQuestions { get; set; }
		public DbSet<EntityQuestion> EntityQuestions { get; set; }
		public DbSet<Statistic> Statistics { get; set; }

		public ApplicationDbContext()
			: base("DefaultConnection")
		{
		}
	}
}