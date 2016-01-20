namespace DietPlanner.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration003 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Products", "Water", c => c.Double(nullable: false));
            AddColumn("dbo.Products", "Alcohol", c => c.Double());
            //AddColumn("dbo.Products", "Fiber", c => c.Double(nullable: false));
            //AddColumn("dbo.Products", "Cholesterol", c => c.Double(nullable: false));
            AlterColumn("dbo.Products", "Sugar", c => c.Double());
            AlterColumn("dbo.Products", "SaturatedFat", c => c.Double());
            DropColumn("dbo.Products", "Salt");
            DropColumn("dbo.Products", "Roughage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Roughage", c => c.Double(nullable: false));
            AddColumn("dbo.Products", "Salt", c => c.Double(nullable: false));
            AlterColumn("dbo.Products", "SaturatedFat", c => c.Double(nullable: false));
            AlterColumn("dbo.Products", "Sugar", c => c.Double(nullable: false));
            //DropColumn("dbo.Products", "Cholesterol");
            //DropColumn("dbo.Products", "Fiber");
            DropColumn("dbo.Products", "Alcohol");
            //DropColumn("dbo.Products", "Water");
        }
    }
}
