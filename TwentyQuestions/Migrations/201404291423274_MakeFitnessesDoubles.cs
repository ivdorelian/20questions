namespace TwentyQuestions.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class MakeFitnessesDoubles : DbMigration
	{
		public override void Up()
		{
			Sql(@"
					IF OBJECT_ID('[DF_dbo.EntityQuestions_Fitness]', 'D') IS NOT NULL
					begin
						alter table dbo.EntityQuestions drop constraint [DF_dbo.EntityQuestions_Fitness]
					end");
			AlterColumn("dbo.EntityQuestions", "Fitness", c => c.Double(nullable: false));

			Sql(@"
					IF OBJECT_ID('[DF_dbo.GameEntities_Fitness]', 'D') IS NOT NULL
					begin
						alter table dbo.GameEntities drop constraint [DF_dbo.GameEntities_Fitness]
					end");
			AlterColumn("dbo.GameEntities", "Fitness", c => c.Double(nullable: false));
		}

		public override void Down()
		{
			Sql(@"
					IF OBJECT_ID('[DF_dbo.GameEntities_Fitness]', 'D') IS NOT NULL
					begin
						alter table dbo.GameEntities drop constraint [DF_dbo.GameEntities_Fitness]
					end");
			AlterColumn("dbo.GameEntities", "Fitness", c => c.Int(nullable: false));

			Sql(@"
					IF OBJECT_ID('[DF_dbo.EntityQuestions_Fitness]', 'D') IS NOT NULL
					begin
						alter table dbo.EntityQuestions drop constraint [DF_dbo.EntityQuestions_Fitness]
					end");
			AlterColumn("dbo.EntityQuestions", "Fitness", c => c.Int(nullable: false));
		}
	}
}
