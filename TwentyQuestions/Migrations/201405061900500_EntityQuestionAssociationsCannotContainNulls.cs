namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class EntityQuestionAssociationsCannotContainNulls : DbMigration
	{
		public override void Up()
		{
			DropForeignKey("dbo.EntityQuestions", "Entity_IDEntity", "dbo.Entities");
			DropForeignKey("dbo.EntityQuestions", "Question_IDQuestion", "dbo.Questions");
			DropIndex("dbo.EntityQuestions", new[] { "Entity_IDEntity" });
			DropIndex("dbo.EntityQuestions", new[] { "Question_IDQuestion" });
			AlterColumn("dbo.EntityQuestions", "Entity_IDEntity", c => c.Int(nullable: false));
			AlterColumn("dbo.EntityQuestions", "Question_IDQuestion", c => c.Int(nullable: false));
			CreateIndex("dbo.EntityQuestions", "Entity_IDEntity");
			CreateIndex("dbo.EntityQuestions", "Question_IDQuestion");
			AddForeignKey("dbo.EntityQuestions", "Entity_IDEntity", "dbo.Entities", "IDEntity", cascadeDelete: true);
			AddForeignKey("dbo.EntityQuestions", "Question_IDQuestion", "dbo.Questions", "IDQuestion", cascadeDelete: true);
		}

		public override void Down()
		{
			DropForeignKey("dbo.EntityQuestions", "Question_IDQuestion", "dbo.Questions");
			DropForeignKey("dbo.EntityQuestions", "Entity_IDEntity", "dbo.Entities");
			DropIndex("dbo.EntityQuestions", new[] { "Question_IDQuestion" });
			DropIndex("dbo.EntityQuestions", new[] { "Entity_IDEntity" });
			AlterColumn("dbo.EntityQuestions", "Question_IDQuestion", c => c.Int());
			AlterColumn("dbo.EntityQuestions", "Entity_IDEntity", c => c.Int());
			CreateIndex("dbo.EntityQuestions", "Question_IDQuestion");
			CreateIndex("dbo.EntityQuestions", "Entity_IDEntity");
			AddForeignKey("dbo.EntityQuestions", "Question_IDQuestion", "dbo.Questions", "IDQuestion");
			AddForeignKey("dbo.EntityQuestions", "Entity_IDEntity", "dbo.Entities", "IDEntity");
		}
	}
}
