namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class RenameQuestionTestToQuestionBody : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.Questions", "QuestionBody", c => c.String());
			DropColumn("dbo.Questions", "QuestionTest");
		}

		public override void Down()
		{
			AddColumn("dbo.Questions", "QuestionTest", c => c.String());
			DropColumn("dbo.Questions", "QuestionBody");
		}
	}
}
