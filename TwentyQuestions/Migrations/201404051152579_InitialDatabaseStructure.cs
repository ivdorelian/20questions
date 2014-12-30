namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class InitialDatabaseStructure : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"dbo.Entities",
				c => new
					{
						IDEntity = c.Int(nullable: false, identity: true),
						Name = c.String(),
						FirstPlayed = c.DateTime(nullable: false),
						LastPlayed = c.DateTime(nullable: false),
						TimesPlayed = c.Int(nullable: false),
						TimesGuessed = c.Int(nullable: false),
					})
				.PrimaryKey(t => t.IDEntity);

			CreateTable(
				"dbo.EntityQuestions",
				c => new
					{
						IDEntityQuestion = c.Int(nullable: false, identity: true),
						Fitness = c.Int(nullable: false),
						YesCount = c.Int(nullable: false),
						NoCount = c.Int(nullable: false),
						UnknownCount = c.Int(nullable: false),
						ProbablyYesCount = c.Int(nullable: false),
						ProbablyNoCount = c.Int(nullable: false),
						Entity_IDEntity = c.Int(),
						Question_IDQuestion = c.Int(),
					})
				.PrimaryKey(t => t.IDEntityQuestion)
				.ForeignKey("dbo.Entities", t => t.Entity_IDEntity)
				.ForeignKey("dbo.Questions", t => t.Question_IDQuestion)
				.Index(t => t.Entity_IDEntity)
				.Index(t => t.Question_IDQuestion);

			CreateTable(
				"dbo.Questions",
				c => new
					{
						IDQuestion = c.Int(nullable: false, identity: true),
						QuestionTest = c.String(),
						FirstAsked = c.DateTime(nullable: false),
						LastAsked = c.DateTime(nullable: false),
						TimesAsked = c.Int(nullable: false),
					})
				.PrimaryKey(t => t.IDQuestion);

			CreateTable(
				"dbo.GameInstances",
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

			CreateTable(
				"dbo.Games",
				c => new
					{
						IDGame = c.Int(nullable: false, identity: true),
						LastActivity = c.DateTime(nullable: false),
						GuessedRank = c.Int(nullable: false),
						CertaintyPercentage = c.Double(nullable: false),
						GuessedObject_IDEntity = c.Int(),
						PlayedObject_IDEntity = c.Int(),
					})
				.PrimaryKey(t => t.IDGame)
				.ForeignKey("dbo.Entities", t => t.GuessedObject_IDEntity)
				.ForeignKey("dbo.Entities", t => t.PlayedObject_IDEntity)
				.Index(t => t.GuessedObject_IDEntity)
				.Index(t => t.PlayedObject_IDEntity);

			CreateTable(
				"dbo.GameQuestions",
				c => new
					{
						IDGameQuestion = c.Int(nullable: false, identity: true),
						QuestionIndex = c.Int(nullable: false),
						GivenAnswer = c.Int(nullable: false),
						Game_IDGame = c.Int(),
						Question_IDQuestion = c.Int(),
					})
				.PrimaryKey(t => t.IDGameQuestion)
				.ForeignKey("dbo.Games", t => t.Game_IDGame)
				.ForeignKey("dbo.Questions", t => t.Question_IDQuestion)
				.Index(t => t.Game_IDGame)
				.Index(t => t.Question_IDQuestion);

			CreateTable(
				"dbo.AspNetRoles",
				c => new
					{
						Id = c.String(nullable: false, maxLength: 128),
						Name = c.String(nullable: false),
					})
				.PrimaryKey(t => t.Id);

			CreateTable(
				"dbo.Statistics",
				c => new
					{
						IDStatistic = c.Int(nullable: false, identity: true),
						StatType = c.Int(nullable: false),
						StatValue = c.Double(nullable: false),
						StatInterpretationType = c.Int(nullable: false),
					})
				.PrimaryKey(t => t.IDStatistic);

			CreateTable(
				"dbo.AspNetUsers",
				c => new
					{
						Id = c.String(nullable: false, maxLength: 128),
						UserName = c.String(),
						PasswordHash = c.String(),
						SecurityStamp = c.String(),
						Discriminator = c.String(nullable: false, maxLength: 128),
					})
				.PrimaryKey(t => t.Id);

			CreateTable(
				"dbo.AspNetUserClaims",
				c => new
					{
						Id = c.Int(nullable: false, identity: true),
						ClaimType = c.String(),
						ClaimValue = c.String(),
						User_Id = c.String(nullable: false, maxLength: 128),
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
				.Index(t => t.User_Id);

			CreateTable(
				"dbo.AspNetUserLogins",
				c => new
					{
						UserId = c.String(nullable: false, maxLength: 128),
						LoginProvider = c.String(nullable: false, maxLength: 128),
						ProviderKey = c.String(nullable: false, maxLength: 128),
					})
				.PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
				.ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
				.Index(t => t.UserId);

			CreateTable(
				"dbo.AspNetUserRoles",
				c => new
					{
						UserId = c.String(nullable: false, maxLength: 128),
						RoleId = c.String(nullable: false, maxLength: 128),
					})
				.PrimaryKey(t => new { t.UserId, t.RoleId })
				.ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
				.ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
				.Index(t => t.RoleId)
				.Index(t => t.UserId);

		}

		public override void Down()
		{
			DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
			DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
			DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
			DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
			DropForeignKey("dbo.GameQuestions", "Question_IDQuestion", "dbo.Questions");
			DropForeignKey("dbo.GameQuestions", "Game_IDGame", "dbo.Games");
			DropForeignKey("dbo.GameInstances", "Game_IDGame", "dbo.Games");
			DropForeignKey("dbo.Games", "PlayedObject_IDEntity", "dbo.Entities");
			DropForeignKey("dbo.Games", "GuessedObject_IDEntity", "dbo.Entities");
			DropForeignKey("dbo.GameInstances", "Entity_IDEntity", "dbo.Entities");
			DropForeignKey("dbo.EntityQuestions", "Question_IDQuestion", "dbo.Questions");
			DropForeignKey("dbo.EntityQuestions", "Entity_IDEntity", "dbo.Entities");
			DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
			DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
			DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
			DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
			DropIndex("dbo.GameQuestions", new[] { "Question_IDQuestion" });
			DropIndex("dbo.GameQuestions", new[] { "Game_IDGame" });
			DropIndex("dbo.GameInstances", new[] { "Game_IDGame" });
			DropIndex("dbo.Games", new[] { "PlayedObject_IDEntity" });
			DropIndex("dbo.Games", new[] { "GuessedObject_IDEntity" });
			DropIndex("dbo.GameInstances", new[] { "Entity_IDEntity" });
			DropIndex("dbo.EntityQuestions", new[] { "Question_IDQuestion" });
			DropIndex("dbo.EntityQuestions", new[] { "Entity_IDEntity" });
			DropTable("dbo.AspNetUserRoles");
			DropTable("dbo.AspNetUserLogins");
			DropTable("dbo.AspNetUserClaims");
			DropTable("dbo.AspNetUsers");
			DropTable("dbo.Statistics");
			DropTable("dbo.AspNetRoles");
			DropTable("dbo.GameQuestions");
			DropTable("dbo.Games");
			DropTable("dbo.GameInstances");
			DropTable("dbo.Questions");
			DropTable("dbo.EntityQuestions");
			DropTable("dbo.Entities");
		}
	}
}
