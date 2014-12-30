namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class QuestionNeedsDateAddedField : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.Questions", "DateAdded", c => c.DateTime(nullable: false));
		}

		public override void Down()
		{
			DropColumn("dbo.Questions", "DateAdded");
		}
	}
}
