namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class StringIDForGames : DbMigration
	{
		public override void Up()
		{
			Sql("delete from Games");
			AddColumn("dbo.Games", "AccessID", c => c.String(nullable: false, maxLength: 10));
			CreateIndex("dbo.Games", "AccessID", unique: true);
		}

		public override void Down()
		{
			DropIndex("dbo.Games", new[] { "AccessID" });
			DropColumn("dbo.Games", "AccessID");
		}
	}
}
