namespace TwentyQuestions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntityValidations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Entities", "Name", c => c.String(nullable: false, maxLength: 32));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Entities", "Name", c => c.String());
        }
    }
}
