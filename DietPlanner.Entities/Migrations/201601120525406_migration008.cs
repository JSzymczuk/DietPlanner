namespace DietPlanner.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration008 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Recipes", "AuthorId", "dbo.AspNetUsers");
            CreateTable(
                "dbo.Units",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        NamePlural1 = c.String(),
                        NamePlural2 = c.String(),
                        NameDecimal = c.String(),
                        Short = c.String(),
                        BaseUnitId = c.Guid(),
                        RatioId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Units", t => t.BaseUnitId)
                .ForeignKey("dbo.UnitRatios", t => t.RatioId)
                .Index(t => t.BaseUnitId)
                .Index(t => t.RatioId);
            
            CreateTable(
                "dbo.ProductUnitUnitRatios",
                c => new
                    {
                        ProductId = c.Guid(nullable: false),
                        UnitId = c.Guid(nullable: false),
                        RatioId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.UnitId, t.RatioId })
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.UnitRatios", t => t.RatioId)
                .ForeignKey("dbo.Units", t => t.UnitId)
                .Index(t => t.ProductId)
                .Index(t => t.UnitId)
                .Index(t => t.RatioId);
            
            CreateTable(
                "dbo.UnitRatios",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Base = c.Int(nullable: false),
                        Derived = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        TargetId = c.Guid(nullable: false),
                        Subject = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        Checked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Recipes", "IsPrivate", c => c.Boolean(nullable: false));
            AddColumn("dbo.Recipes", "Verified", c => c.Boolean(nullable: false));
            AddColumn("dbo.Recipes", "AppUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Recipes", "AppUser_Id1", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Ingredient_Id", c => c.Guid());
            AddColumn("dbo.Products", "DefaultUnitId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Ingredients", "Quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Products", "Protein", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Products", "Carbohydrate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Products", "Fat", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Products", "Sugar", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Products", "SaturatedFat", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Products", "Salt", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Products", "Alcohol", c => c.Decimal(precision: 18, scale: 2));
            CreateIndex("dbo.Recipes", "AppUser_Id");
            CreateIndex("dbo.Recipes", "AppUser_Id1");
            CreateIndex("dbo.AspNetUsers", "Ingredient_Id");
            CreateIndex("dbo.Products", "DefaultUnitId");
            AddForeignKey("dbo.Recipes", "AppUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Ingredient_Id", "dbo.Ingredients", "Id");
            AddForeignKey("dbo.Products", "DefaultUnitId", "dbo.Units", "Id");
            AddForeignKey("dbo.Recipes", "AppUser_Id1", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Products", "Unit");
            DropColumn("dbo.Products", "Quantity");
            DropColumn("dbo.Products", "Pieces");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Pieces", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "Quantity", c => c.Int());
            AddColumn("dbo.Products", "Unit", c => c.Int(nullable: false));
            DropForeignKey("dbo.Recipes", "AppUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reports", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Products", "DefaultUnitId", "dbo.Units");
            DropForeignKey("dbo.ProductUnitUnitRatios", "UnitId", "dbo.Units");
            DropForeignKey("dbo.Units", "RatioId", "dbo.UnitRatios");
            DropForeignKey("dbo.ProductUnitUnitRatios", "RatioId", "dbo.UnitRatios");
            DropForeignKey("dbo.ProductUnitUnitRatios", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Units", "BaseUnitId", "dbo.Units");
            DropForeignKey("dbo.AspNetUsers", "Ingredient_Id", "dbo.Ingredients");
            DropForeignKey("dbo.Recipes", "AppUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Reports", new[] { "UserId" });
            DropIndex("dbo.ProductUnitUnitRatios", new[] { "RatioId" });
            DropIndex("dbo.ProductUnitUnitRatios", new[] { "UnitId" });
            DropIndex("dbo.ProductUnitUnitRatios", new[] { "ProductId" });
            DropIndex("dbo.Units", new[] { "RatioId" });
            DropIndex("dbo.Units", new[] { "BaseUnitId" });
            DropIndex("dbo.Products", new[] { "DefaultUnitId" });
            DropIndex("dbo.AspNetUsers", new[] { "Ingredient_Id" });
            DropIndex("dbo.Recipes", new[] { "AppUser_Id1" });
            DropIndex("dbo.Recipes", new[] { "AppUser_Id" });
            AlterColumn("dbo.Products", "Alcohol", c => c.Double());
            AlterColumn("dbo.Products", "Salt", c => c.Double());
            AlterColumn("dbo.Products", "SaturatedFat", c => c.Double());
            AlterColumn("dbo.Products", "Sugar", c => c.Double());
            AlterColumn("dbo.Products", "Fat", c => c.Double(nullable: false));
            AlterColumn("dbo.Products", "Carbohydrate", c => c.Double(nullable: false));
            AlterColumn("dbo.Products", "Protein", c => c.Double(nullable: false));
            AlterColumn("dbo.Ingredients", "Quantity", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "DefaultUnitId");
            DropColumn("dbo.AspNetUsers", "Ingredient_Id");
            DropColumn("dbo.Recipes", "AppUser_Id1");
            DropColumn("dbo.Recipes", "AppUser_Id");
            DropColumn("dbo.Recipes", "Verified");
            DropColumn("dbo.Recipes", "IsPrivate");
            DropTable("dbo.Reports");
            DropTable("dbo.UnitRatios");
            DropTable("dbo.ProductUnitUnitRatios");
            DropTable("dbo.Units");
            AddForeignKey("dbo.Recipes", "AuthorId", "dbo.AspNetUsers", "Id");
        }
    }
}
