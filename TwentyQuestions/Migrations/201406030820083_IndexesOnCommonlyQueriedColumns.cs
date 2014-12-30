namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class IndexesOnCommonlyQueriedColumns : DbMigration
	{
		public override void Up()
		{
			CreateIndex("dbo.Games", "LastActivity", name: "IDX_Games_LastActivity");
			CreateIndex("dbo.GameEntities", "Fitness", name: "IDX_GameEntities_Fitness");
			CreateIndex("dbo.EntityQuestions", "Fitness", name: "IDX_EntityQuestions_Fitness");
			CreateIndex("dbo.EntityQuestions", "YesCount", name: "IDX_EntityQuestions_YesCount");
			CreateIndex("dbo.EntityQuestions", "NoCount", name: "IDX_EntityQuestions_NoCount");
			CreateIndex("dbo.EntityQuestions", "UnknownCount", name: "IDX_EntityQuestions_UnknownCount");
			CreateIndex("dbo.Questions", "TimesAsked", name: "IDX_Questions_TimesAsked");
		}

		public override void Down()
		{
			DropIndex("dbo.Games", "IDX_Games_LastActivity");
			DropIndex("dbo.GameEntities", "IDX_GameEntities_Fitness");
			DropIndex("dbo.EntityQuestions", "IDX_EntityQuestions_Fitness");
			DropIndex("dbo.EntityQuestions", "IDX_EntityQuestions_YesCount");
			DropIndex("dbo.EntityQuestions", "IDX_EntityQuestions_NoCount");
			DropIndex("dbo.EntityQuestions", "IDX_EntityQuestions_UnknownCount");
			DropIndex("dbo.EntityQuestions", "IDX_EntityQuestions_UnknownCount");
		}
	}
}
