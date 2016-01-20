namespace DietPlanner.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration005 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Products", "ProductName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ProductName", c => c.String(nullable: false));
            DropColumn("dbo.Products", "Name");
        }
    }
}
