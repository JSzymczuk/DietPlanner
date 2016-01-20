namespace DietPlanner.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration002 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Quantity", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Quantity", c => c.Int(nullable: false));
        }
    }
}
