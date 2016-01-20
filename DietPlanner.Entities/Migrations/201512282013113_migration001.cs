namespace DietPlanner.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration001 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Verified", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "GlutenFree", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProductCategories", "ParentId", c => c.Guid());
            AlterColumn("dbo.Products", "Pieces", c => c.Boolean(nullable: false));
            CreateIndex("dbo.ProductCategories", "ParentId");
            AddForeignKey("dbo.ProductCategories", "ParentId", "dbo.ProductCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductCategories", "ParentId", "dbo.ProductCategories");
            DropIndex("dbo.ProductCategories", new[] { "ParentId" });
            AlterColumn("dbo.Products", "Pieces", c => c.Int(nullable: false));
            DropColumn("dbo.ProductCategories", "ParentId");
            DropColumn("dbo.Products", "GlutenFree");
            DropColumn("dbo.Products", "Verified");
        }
    }
}
