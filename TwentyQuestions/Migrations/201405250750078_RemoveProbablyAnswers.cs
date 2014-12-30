namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class RemoveProbablyAnswers : DbMigration
	{
		public override void Up()
		{
			DropColumn("dbo.EntityQuestions", "ProbablyYesCount");
			DropColumn("dbo.EntityQuestions", "ProbablyNoCount");
		}

		public override void Down()
		{
			AddColumn("dbo.EntityQuestions", "ProbablyNoCount", c => c.Int(nullable: false));
			AddColumn("dbo.EntityQuestions", "ProbablyYesCount", c => c.Int(nullable: false));
		}
	}
}
