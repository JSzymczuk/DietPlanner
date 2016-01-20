using DietPlanner.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DietPlanner.ViewModels
{
    public class IngredientCreateViewModel
    {
        public string Alias { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public Guid UnitId { get; set; }
        public SelectList Measures { get; set; }
        public decimal BaseProtein { get; set; }
        public decimal BaseCarbohydrate { get; set; }
        public decimal BaseFat { get; set; }
        public decimal TotalProtein { get; set; }
        public decimal TotalCarbohydrate { get; set; }
        public decimal TotalFat { get; set; }
    }

    public class IngredientListItemModel
    {
        public string Alias { get; set; }
        public string PortionWeight { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbohydrate { get; set; }
        public decimal Fat { get; set; }
    }

    public class AddRecipeViewModel : ProductSearchViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int PreparationTime { get; set; }
        public bool Vegetarian { get; set; }
        public Guid CategoryId { get; set; }
    }

    public class RecipeSearchModel : ICaloriesProvider
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AppUserInfo Author { get; set; }
        public RecipeCategoryInfo Category { get; set; }
        public decimal Rating { get; set; }
        public int Comments { get; set; }
        public int Weight { get; set; }
        public int Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbohydrate { get; set; }
        public decimal Fat { get; set; }
        public DateTime Added { get; set; }
        public int PreparationTime { get; set; }
        public bool Vegetarian { get; set; }
        public bool GlutenFree { get; set; }
    }

    public class RecipeSearchViewModel : SearchViewModel<RecipeSearchModel, RecipeSortType>
    {
    }

    public class RecipeDetailsViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Added { get; set; }
        public int PreparationTime { get; set; }
        public bool Vegetarian { get; set; }
        public decimal Rating { get; set; }
        public int VotesCount { get; set; }
        public bool UserLoggedIn { get; set; }
        public bool VoteAllowed { get; set; }
        public AppUserInfo AppUserInfo { get; set; }
        public RecipeCategoryInfo RecipeCategory { get; set; }
        public ICollection<IngredientListItemModel> Ingredients { get; set; }
        public ICollection<CommentModel> Comments { get; set; }
        public AddCommentModel NewComment { get; set; }
    }
}