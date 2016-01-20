namespace DietPlanner.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration007 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Title", c => c.String());
            AddColumn("dbo.Recipes", "Rating", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recipes", "Rating");
            DropColumn("dbo.Comments", "Title");
        }
    }
}
