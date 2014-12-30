namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;
	
	public partial class EntityMetadata : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.Entities", "Description", c => c.String(nullable: false, maxLength: 64));
			AddColumn("dbo.Entities", "PictureURL", c => c.String(nullable: false, maxLength: 64));
			AddColumn("dbo.Entities", "IsActive", c => c.Boolean(nullable: false));
		}
		
		public override void Down()
		{
			DropColumn("dbo.Entities", "IsActive");
			DropColumn("dbo.Entities", "PictureURL");
			DropColumn("dbo.Entities", "Description");
		}
	}
}
