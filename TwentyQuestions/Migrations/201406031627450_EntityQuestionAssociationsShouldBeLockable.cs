namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class EntityQuestionAssociationsShouldBeLockable : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.EntityQuestions", "Locked", c => c.Boolean(nullable: false));
		}

		public override void Down()
		{
			DropColumn("dbo.EntityQuestions", "Locked");
		}
	}
}
