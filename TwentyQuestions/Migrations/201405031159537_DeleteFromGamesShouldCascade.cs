namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class DeleteFromGamesShouldCascade : DbMigration
	{
		public override void Up()
		{
			DropForeignKey("dbo.GameQuestions", "Game_IDGame", "dbo.Games");
			DropForeignKey("dbo.GameEntities", "Game_IDGame", "dbo.Games");

			AddForeignKey("dbo.GameQuestions", "Game_IDGame", "dbo.Games", cascadeDelete: true);
			AddForeignKey("dbo.GameEntities", "Game_IDGame", "dbo.Games", cascadeDelete: true);
		}

		public override void Down()
		{

		}
	}
}
