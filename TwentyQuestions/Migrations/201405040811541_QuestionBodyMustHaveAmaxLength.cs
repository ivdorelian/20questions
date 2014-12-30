namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class QuestionBodyMustHaveAmaxLength : DbMigration
	{
		public override void Up()
		{
			AlterColumn("dbo.Questions", "QuestionBody", c => c.String(nullable: false, maxLength: 256));
		}

		public override void Down()
		{
			AlterColumn("dbo.Questions", "QuestionBody", c => c.String());
		}
	}
}
