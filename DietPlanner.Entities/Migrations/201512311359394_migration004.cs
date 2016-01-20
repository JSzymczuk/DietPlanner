namespace DietPlanner.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration004 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Salt", c => c.Double());
            //AlterColumn("dbo.Products", "Water", c => c.Double());
            //AlterColumn("dbo.Products", "Fiber", c => c.Double());
            //AlterColumn("dbo.Products", "Cholesterol", c => c.Double());
            AlterColumn("dbo.Products", "GlutenFree", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "GlutenFree", c => c.Boolean(nullable: false));
            //AlterColumn("dbo.Products", "Cholesterol", c => c.Double(nullable: false));
            //AlterColumn("dbo.Products", "Fiber", c => c.Double(nullable: false));
            //AlterColumn("dbo.Products", "Water", c => c.Double(nullable: false));
            DropColumn("dbo.Products", "Salt");
        }
    }
}
