namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class IndexesFromAttributes : DbMigration
	{
		public override void Up()
		{
			CreateIndex("dbo.EntityQuestions", "Fitness");
			CreateIndex("dbo.EntityQuestions", "YesCount");
			CreateIndex("dbo.EntityQuestions", "NoCount");
			CreateIndex("dbo.EntityQuestions", "UnknownCount");
			CreateIndex("dbo.Questions", "TimesAsked");
			CreateIndex("dbo.GameEntities", "Fitness");
			CreateIndex("dbo.Games", "LastActivity");
		}

		public override void Down()
		{
			DropIndex("dbo.Games", new[] { "LastActivity" });
			DropIndex("dbo.GameEntities", new[] { "Fitness" });
			DropIndex("dbo.Questions", new[] { "TimesAsked" });
			DropIndex("dbo.EntityQuestions", new[] { "UnknownCount" });
			DropIndex("dbo.EntityQuestions", new[] { "NoCount" });
			DropIndex("dbo.EntityQuestions", new[] { "YesCount" });
			DropIndex("dbo.EntityQuestions", new[] { "Fitness" });
		}
	}
}
