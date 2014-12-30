namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class RenameGameInstanceToGameEntity : DbMigration
	{
		public override void Up()
		{
			DropForeignKey("dbo.GameInstances", "Entity_IDEntity", "dbo.Entities");
			DropForeignKey("dbo.GameInstances", "Game_IDGame", "dbo.Games");
			DropIndex("dbo.GameInstances", new[] { "Entity_IDEntity" });
			DropIndex("dbo.GameInstances", new[] { "Game_IDGame" });
			CreateTable(
				"dbo.GameEntities",
				c => new
					{
						IDGameInstance = c.Int(nullable: false, identity: true),
						Fitness = c.Int(nullable: false),
						Entity_IDEntity = c.Int(),
						Game_IDGame = c.Int(),
					})
				.PrimaryKey(t => t.IDGameInstance)
				.ForeignKey("dbo.Entities", t => t.Entity_IDEntity)
				.ForeignKey("dbo.Games", t => t.Game_IDGame)
				.Index(t => t.Entity_IDEntity)
				.Index(t => t.Game_IDGame);

			DropTable("dbo.GameInstances");
		}

		public override void Down()
		{
			CreateTable(
				"dbo.GameInstances",
				c => new
					{
						IDGameInstance = c.Int(nullable: false, identity: true),
						Fitness = c.Int(nullable: false),
						Entity_IDEntity = c.Int(),
						Game_IDGame = c.Int(),
					})
				.PrimaryKey(t => t.IDGameInstance);

			DropForeignKey("dbo.GameEntities", "Game_IDGame", "dbo.Games");
			DropForeignKey("dbo.GameEntities", "Entity_IDEntity", "dbo.Entities");
			DropIndex("dbo.GameEntities", new[] { "Game_IDGame" });
			DropIndex("dbo.GameEntities", new[] { "Entity_IDEntity" });
			DropTable("dbo.GameEntities");
			CreateIndex("dbo.GameInstances", "Game_IDGame");
			CreateIndex("dbo.GameInstances", "Entity_IDEntity");
			AddForeignKey("dbo.GameInstances", "Game_IDGame", "dbo.Games", "IDGame");
			AddForeignKey("dbo.GameInstances", "Entity_IDEntity", "dbo.Entities", "IDEntity");
		}
	}
}
