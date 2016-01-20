using DietPlanner.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietPlanner.Contract
{
    public interface IRecipeManager : IBaseManager<Recipe, Guid>
    {
        IQueryable<RecipeCategory> RecipeCategories { get; }
        IQueryable<Recipe> FindByName(string name);
        RecipeCategory FindCategoryById(Guid id);
        void AddToCategory(Recipe recipe, RecipeCategory category);
        bool InCategory(Guid recipeId, Guid categoryId);
        bool InCategory(Recipe recipe, RecipeCategory category);

        /*IQueryable<ProductCategory> ProductCategories { get; }
        IQueryable<Product> FindByName(string name);
        ProductCategory FindCategoryByName(string name);
        
        void VerifyProduct(Product product);*/
    }
}
