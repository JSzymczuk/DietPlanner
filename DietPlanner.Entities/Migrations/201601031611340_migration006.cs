namespace DietPlanner.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration006 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IngredientMeals", "Ingredient_Id", "dbo.Ingredients");
            DropForeignKey("dbo.IngredientMeals", "Meal_Id", "dbo.Meals");
            DropForeignKey("dbo.IngredientRecipes", "Ingredient_Id", "dbo.Ingredients");
            DropForeignKey("dbo.IngredientRecipes", "Recipe_Id", "dbo.Recipes");
            DropIndex("dbo.Recipes", new[] { "AuthorId" });
            DropIndex("dbo.IngredientMeals", new[] { "Ingredient_Id" });
            DropIndex("dbo.IngredientMeals", new[] { "Meal_Id" });
            DropIndex("dbo.IngredientRecipes", new[] { "Ingredient_Id" });
            DropIndex("dbo.IngredientRecipes", new[] { "Recipe_Id" });
            AddColumn("dbo.Ingredients", "Name", c => c.String());
            AddColumn("dbo.Ingredients", "RecipeId", c => c.Guid(nullable: false));
            AddColumn("dbo.Ingredients", "Meal_Id", c => c.Guid());
            AlterColumn("dbo.Recipes", "AuthorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Recipes", "AuthorId");
            CreateIndex("dbo.Ingredients", "RecipeId");
            CreateIndex("dbo.Ingredients", "Meal_Id");
            AddForeignKey("dbo.Ingredients", "RecipeId", "dbo.Recipes", "Id");
            AddForeignKey("dbo.Ingredients", "Meal_Id", "dbo.Meals", "Id");
            DropColumn("dbo.Recipes", "Rating");
            DropTable("dbo.IngredientMeals");
            DropTable("dbo.IngredientRecipes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.IngredientRecipes",
                c => new
                    {
                        Ingredient_Id = c.Guid(nullable: false),
                        Recipe_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ingredient_Id, t.Recipe_Id });
            
            CreateTable(
                "dbo.IngredientMeals",
                c => new
                    {
                        Ingredient_Id = c.Guid(nullable: false),
                        Meal_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ingredient_Id, t.Meal_Id });
            
            AddColumn("dbo.Recipes", "Rating", c => c.Double(nullable: false));
            DropForeignKey("dbo.Ingredients", "Meal_Id", "dbo.Meals");
            DropForeignKey("dbo.Ingredients", "RecipeId", "dbo.Recipes");
            DropIndex("dbo.Ingredients", new[] { "Meal_Id" });
            DropIndex("dbo.Ingredients", new[] { "RecipeId" });
            DropIndex("dbo.Recipes", new[] { "AuthorId" });
            AlterColumn("dbo.Recipes", "AuthorId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Ingredients", "Meal_Id");
            DropColumn("dbo.Ingredients", "RecipeId");
            DropColumn("dbo.Ingredients", "Name");
            CreateIndex("dbo.IngredientRecipes", "Recipe_Id");
            CreateIndex("dbo.IngredientRecipes", "Ingredient_Id");
            CreateIndex("dbo.IngredientMeals", "Meal_Id");
            CreateIndex("dbo.IngredientMeals", "Ingredient_Id");
            CreateIndex("dbo.Recipes", "AuthorId");
            AddForeignKey("dbo.IngredientRecipes", "Recipe_Id", "dbo.Recipes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.IngredientRecipes", "Ingredient_Id", "dbo.Ingredients", "Id", cascadeDelete: true);
            AddForeignKey("dbo.IngredientMeals", "Meal_Id", "dbo.Meals", "Id", cascadeDelete: true);
            AddForeignKey("dbo.IngredientMeals", "Ingredient_Id", "dbo.Ingredients", "Id", cascadeDelete: true);
        }
    }
}
