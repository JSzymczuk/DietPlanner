namespace DietPlanner.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration000 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        UserId = c.String(nullable: false, maxLength: 128),
                        RecipeId = c.Guid(nullable: false),
                        Added = c.DateTime(nullable: false),
                        Content = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Recipes", t => t.RecipeId)
                .Index(t => t.UserId)
                .Index(t => t.RecipeId);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Added = c.DateTime(nullable: false),
                        PreparationTime = c.Int(nullable: false),
                        Vegetarian = c.Boolean(nullable: false),
                        AuthorId = c.String(nullable: false, maxLength: 128),
                        CategoryId = c.Guid(nullable: false),
                        Rating = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId)
                .ForeignKey("dbo.RecipeCategories", t => t.CategoryId)
                .Index(t => t.AuthorId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Weight = c.Int(nullable: false),
                        Heigth = c.Int(nullable: false),
                        BirthYear = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Meals",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        Name = c.String(nullable: false),
                        AuthorId = c.String(nullable: false, maxLength: 128),
                        DayId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId)
                .ForeignKey("dbo.Days", t => t.DayId)
                .Index(t => t.AuthorId)
                .Index(t => t.DayId);
            
            CreateTable(
                "dbo.Days",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        Name = c.String(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        Quantity = c.Int(nullable: false),
                        ProductId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        ProductName = c.String(nullable: false),
                        Unit = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Pieces = c.Int(nullable: false),
                        CategoryId = c.Guid(nullable: false),
                        Protein = c.Double(nullable: false),
                        Carbohydrate = c.Double(nullable: false),
                        Sugar = c.Double(nullable: false),
                        Fat = c.Double(nullable: false),
                        SaturatedFat = c.Double(nullable: false),
                        Salt = c.Double(nullable: false),
                        Roughage = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategories", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        CategoryName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        UserId = c.String(nullable: false, maxLength: 128),
                        RecipeId = c.Guid(nullable: false),
                        Stars = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Recipes", t => t.RecipeId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RecipeId);
            
            CreateTable(
                "dbo.RecipeCategories",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        CategoryName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.IngredientMeals",
                c => new
                    {
                        Ingredient_Id = c.Guid(nullable: false),
                        Meal_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ingredient_Id, t.Meal_Id })
                .ForeignKey("dbo.Ingredients", t => t.Ingredient_Id, cascadeDelete: true)
                .ForeignKey("dbo.Meals", t => t.Meal_Id, cascadeDelete: true)
                .Index(t => t.Ingredient_Id)
                .Index(t => t.Meal_Id);
            
            CreateTable(
                "dbo.IngredientRecipes",
                c => new
                    {
                        Ingredient_Id = c.Guid(nullable: false),
                        Recipe_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ingredient_Id, t.Recipe_Id })
                .ForeignKey("dbo.Ingredients", t => t.Ingredient_Id, cascadeDelete: true)
                .ForeignKey("dbo.Recipes", t => t.Recipe_Id, cascadeDelete: true)
                .Index(t => t.Ingredient_Id)
                .Index(t => t.Recipe_Id);
            
            CreateTable(
                "dbo.MealRecipes",
                c => new
                    {
                        Meal_Id = c.Guid(nullable: false),
                        Recipe_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Meal_Id, t.Recipe_Id })
                .ForeignKey("dbo.Meals", t => t.Meal_Id, cascadeDelete: true)
                .ForeignKey("dbo.Recipes", t => t.Recipe_Id, cascadeDelete: true)
                .Index(t => t.Meal_Id)
                .Index(t => t.Recipe_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Recipes", "CategoryId", "dbo.RecipeCategories");
            DropForeignKey("dbo.Comments", "RecipeId", "dbo.Recipes");
            DropForeignKey("dbo.Ratings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ratings", "RecipeId", "dbo.Recipes");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Recipes", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.MealRecipes", "Recipe_Id", "dbo.Recipes");
            DropForeignKey("dbo.MealRecipes", "Meal_Id", "dbo.Meals");
            DropForeignKey("dbo.IngredientRecipes", "Recipe_Id", "dbo.Recipes");
            DropForeignKey("dbo.IngredientRecipes", "Ingredient_Id", "dbo.Ingredients");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.ProductCategories");
            DropForeignKey("dbo.Ingredients", "ProductId", "dbo.Products");
            DropForeignKey("dbo.IngredientMeals", "Meal_Id", "dbo.Meals");
            DropForeignKey("dbo.IngredientMeals", "Ingredient_Id", "dbo.Ingredients");
            DropForeignKey("dbo.Meals", "DayId", "dbo.Days");
            DropForeignKey("dbo.Meals", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.MealRecipes", new[] { "Recipe_Id" });
            DropIndex("dbo.MealRecipes", new[] { "Meal_Id" });
            DropIndex("dbo.IngredientRecipes", new[] { "Recipe_Id" });
            DropIndex("dbo.IngredientRecipes", new[] { "Ingredient_Id" });
            DropIndex("dbo.IngredientMeals", new[] { "Meal_Id" });
            DropIndex("dbo.IngredientMeals", new[] { "Ingredient_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Ratings", new[] { "RecipeId" });
            DropIndex("dbo.Ratings", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.Ingredients", new[] { "ProductId" });
            DropIndex("dbo.Meals", new[] { "DayId" });
            DropIndex("dbo.Meals", new[] { "AuthorId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Recipes", new[] { "CategoryId" });
            DropIndex("dbo.Recipes", new[] { "AuthorId" });
            DropIndex("dbo.Comments", new[] { "RecipeId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropTable("dbo.MealRecipes");
            DropTable("dbo.IngredientRecipes");
            DropTable("dbo.IngredientMeals");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RecipeCategories");
            DropTable("dbo.Ratings");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Products");
            DropTable("dbo.Ingredients");
            DropTable("dbo.Days");
            DropTable("dbo.Meals");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Recipes");
            DropTable("dbo.Comments");
        }
    }
}
