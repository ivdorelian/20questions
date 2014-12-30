namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class DeleteFromEntitiesShouldCascade : DbMigration
	{
		public override void Up()
		{
			DropForeignKey("dbo.GameEntities", "Entity_IDEntity", "dbo.Entities");
			DropForeignKey("dbo.Games", "GuessedObject_IDEntity", "dbo.Entities");
			DropForeignKey("dbo.Games", "PlayedObject_IDEntity", "dbo.Entities");
			DropForeignKey("dbo.EntityQuestions", "Entity_IDEntity", "dbo.Entities");

			AddForeignKey("dbo.GameEntities", "Entity_IDEntity", "dbo.Entities", cascadeDelete: true);
			AddForeignKey("dbo.Games", "GuessedObject_IDEntity", "dbo.Entities");
			AddForeignKey("dbo.Games", "PlayedObject_IDEntity", "dbo.Entities");
			AddForeignKey("dbo.EntityQuestions", "Entity_IDEntity", "dbo.Entities", cascadeDelete: true);
		}

		public override void Down()
		{
		}
	}
}
