using DietPlanner.Contract;
using DietPlanner.Entities;
using System;
using System.Data.Entity;
using System.Linq;

namespace DietPlanner.Implementation
{
    public class RecipeManager : DisposableManager, IRecipeManager
    {
        public IQueryable<Recipe> Entities
        {
            get { return Context.Recipes; }
        }

        public IQueryable<RecipeCategory> RecipeCategories 
        { 
            get { return Context.RecipeCategories; } 
        }

        public RecipeManager(DietPlannerDbContext context) : base(context) { }

        public Recipe FindById(Guid id)
        {
            return Context.Recipes.First(c => c.Id == id);
        }

        public void Create(Recipe recipe)
        {
            if (recipe != null)
            {
                Context.Recipes.Add(recipe);
            }
        }

        public IQueryable<Recipe> FindByName(string name)
        {
            return Context.Recipes.Where(c => c.Name.Contains(name));
        }

        public void Delete(Guid id)
        {
            Recipe recipe = Context.Recipes.Find(id);
            Context.Recipes.Remove(recipe);
        }

        public void Update(Recipe recipe)
        {
            Context.Entry(recipe).State = EntityState.Modified;
        }

        public RecipeCategory FindCategoryById(Guid id)
        {
            return Context.RecipeCategories.First(c => c.Id == id);
        }

        public void AddToCategory(Recipe recipe, RecipeCategory category)
        {
            recipe.RecipeCategory = category;
            category.Recipes.Add(recipe);
        }

        public bool InCategory(Guid recipeId, Guid categoryId)
        {
            return InCategory(FindById(recipeId), FindCategoryById(categoryId));
        }

        public bool InCategory(Recipe recipe, RecipeCategory category)
        {
            if (recipe == null || category == null) { return false; }
            return recipe.CategoryId == category.Id;
        }
    }
}
