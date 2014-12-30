namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class ChangeGuessedRankToPlayedRank : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.Games", "PlayedRank", c => c.Int());
			DropColumn("dbo.Games", "GuessedRank");
		}

		public override void Down()
		{
			AddColumn("dbo.Games", "GuessedRank", c => c.Int(nullable: false));
			DropColumn("dbo.Games", "PlayedRank");
		}
	}
}
