namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class RemoveImageURLFromEntities : DbMigration
	{
		public override void Up()
		{
			DropColumn("dbo.Entities", "PictureURL");
		}

		public override void Down()
		{
			AddColumn("dbo.Entities", "PictureURL", c => c.String(nullable: false, maxLength: 64));
		}
	}
}
