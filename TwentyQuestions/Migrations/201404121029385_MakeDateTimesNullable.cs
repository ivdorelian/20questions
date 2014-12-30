namespace TwentyQuestions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeDateTimesNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Entities", "FirstPlayed", c => c.DateTime());
            AlterColumn("dbo.Entities", "LastPlayed", c => c.DateTime());
            AlterColumn("dbo.Questions", "FirstAsked", c => c.DateTime());
            AlterColumn("dbo.Questions", "LastAsked", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questions", "LastAsked", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Questions", "FirstAsked", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Entities", "LastPlayed", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Entities", "FirstPlayed", c => c.DateTime(nullable: false));
        }
    }
}
