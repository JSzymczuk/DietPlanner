namespace DietPlanner.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration009 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Cholesterol", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Products", "Fiber", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Products", "Water", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Water");
            DropColumn("dbo.Products", "Fiber");
            DropColumn("dbo.Products", "Cholesterol");
        }
    }
}
