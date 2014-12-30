namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class StoreExpectedAnswersInTheDatabase : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.GameQuestions", "ExpectedAnswer", c => c.Int(nullable: false));
		}

		public override void Down()
		{
			DropColumn("dbo.GameQuestions", "ExpectedAnswer");
		}
	}
}
