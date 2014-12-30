namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class DeleteFromQuestionsShouldCascade : DbMigration
	{
		public override void Up()
		{
			DropForeignKey("dbo.GameQuestions", "Question_IDQuestion", "dbo.Questions");
			DropForeignKey("dbo.EntityQuestions", "Question_IDQuestion", "dbo.Questions");

			AddForeignKey("dbo.GameQuestions", "Question_IDQuestion", "dbo.Questions", cascadeDelete: true);
			AddForeignKey("dbo.EntityQuestions", "Question_IDQuestion", "dbo.Questions", cascadeDelete: true);
		}

		public override void Down()
		{
		}
	}
}
