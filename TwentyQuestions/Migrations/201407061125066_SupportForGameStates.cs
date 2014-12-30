namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class SupportForGameStates : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.Games", "GameState", c => c.Int(nullable: false));
			AddColumn("dbo.GameQuestions", "IsLastQuestion", c => c.Boolean(nullable: false));
		}

		public override void Down()
		{
			DropColumn("dbo.GameQuestions", "IsLastQuestion");
			DropColumn("dbo.Games", "GameState");
		}
	}
}
