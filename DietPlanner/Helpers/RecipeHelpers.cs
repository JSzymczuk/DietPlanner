using AutoMapper;
using DietPlanner.Contract;
using DietPlanner.Entities;
using DietPlanner.Models;
using DietPlanner.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DietPlanner.Helpers
{
    public static class RecipeHelper
    {
        public static bool IsGlutenFree(this Recipe recipe)
        {
            bool result = true;
            foreach (var ingr in recipe.Ingredients)
            {
                if (ingr.Product.GlutenFree.HasValue)
                {
                    result &= ingr.Product.GlutenFree.Value;
                }
                else
                {
                    return false;
                }
            }
            return result;
        }

        public static int GetWeight(this Recipe recipe)
        {
            decimal total = 0;
            foreach (var ingr in recipe.Ingredients)
            {
                total += ingr.GetWeight();
            }
            return (int)total;
        }

        public static NutritionData GetTotalNutrition(this Recipe recipe)
        {
            decimal totalP = 0;
            decimal totalC = 0;
            decimal totalF = 0;

            foreach (var ingr in recipe.Ingredients)
            {
                decimal mult = ingr.GetWeight() / 100;
                totalP += mult * ingr.Product.Protein;
                totalC += mult * ingr.Product.Carbohydrate;
                totalF += mult * ingr.Product.Fat;
            }

            return new NutritionData(totalP, totalC, totalF);
        }
        
        public static NutritionData GetNutrition(this Recipe recipe)
        {
            decimal weight = recipe.GetWeight();
            if (weight == 0) return new NutritionData();
            decimal mult = 100.0M / weight;
            NutritionData nd = recipe.GetTotalNutrition();
            return new NutritionData(nd.Protein * mult, nd.Carbohydrate * mult, nd.Fat * mult);            
        }

        public static decimal GetWeight(this Ingredient ingredient)
        {
            return ingredient.Quantity;
            /*Rozważyć skalowanie przy płynach
            if (ingredient.Product.Pieces)
            {
                return ingredient.Quantity * ingredient.Product.Quantity.Value;
            }
            else 
            { 
            }*/
        }

        public static string GetNutritionString(decimal protein, decimal carbs, decimal fat)
        {
            return string.Format("Białko: {0}g<br>Węglowodany: {1}g<br>Tłuszcz: {2}g", protein, carbs, fat);
        }

        public static SelectList EnumRecipeSortType
        {
            get { return GeneralHelper.SelectListForEnum(typeof(RecipeSortType)); }
        }

        public static string GetTotalNutritionString(ICollection<IngredientCreateViewModel> ingredients)
        {
            decimal protein = 0, carbs = 0, fat = 0;
            foreach (var ingr in ingredients)
            {
                protein += ingr.TotalProtein;
                carbs += ingr.TotalCarbohydrate;
                fat += ingr.TotalFat;
            }
            return GetNutritionString(protein / 100.0M, carbs / 100.0M, fat / 100.0M);
        }
        
        public static IOrderedEnumerable<RecipeSearchModel> SortRecipes(
            IEnumerable<RecipeSearchModel> data, RecipeSortType sortType)
        {
            switch (sortType)
            {
                case RecipeSortType.NameAscending: 
                    return data.OrderBy(item => item.Name);
                case RecipeSortType.NameDescending:
                    return data.OrderByDescending(item => item.Name);
                case RecipeSortType.RatingAscending:
                    return data.OrderBy(item => item.Rating);
                case RecipeSortType.RatingDescending:
                    return data.OrderByDescending(item => item.Rating);
                case RecipeSortType.ProteinAscending:
                    return data.OrderBy(item => item.Protein);
                case RecipeSortType.ProteinDescending:
                    return data.OrderByDescending(item => item.Protein);
                case RecipeSortType.FatAscending:
                    return data.OrderBy(item => item.Fat);
                case RecipeSortType.FatDescending:
                    return data.OrderByDescending(item => item.Fat);
                case RecipeSortType.CarbohydrateAscending:
                    return data.OrderBy(item => item.Carbohydrate);
                case RecipeSortType.CarbohydrateDescending:
                    return data.OrderByDescending(item => item.Carbohydrate);
                case RecipeSortType.CommentsAscending:
                    return data.OrderBy(item => item.Comments);
                case RecipeSortType.CommentsDescending:
                    return data.OrderByDescending(item => item.Comments);
                default:
                    return data.OrderBy(item => item.Name);
            }
        }

        private static IEnumerable<RecipeSearchModel> FilterProducts(this IRecipeManager recipeManager,
            IEnumerable<RecipeSearchModel> data, ViewModels.Filter filter)
        {
            if (filter.CategoryId.HasValue)
            {
                return data.Where(p =>
                    (!filter.GlutenFree || p.GlutenFree)
                    && recipeManager.InCategory(p.Id, filter.CategoryId.Value)
                    && p.InRangeCarbohydrate(filter.CarbRange)
                    && p.InRangeFat(filter.FatRange)
                    && p.InRangeProtein(filter.ProteinRange));
            }
            else
            {
                return data.Where(p =>
                    (!filter.GlutenFree || p.GlutenFree)
                    && p.InRangeCarbohydrate(filter.CarbRange)
                    && p.InRangeFat(filter.FatRange)
                    && p.InRangeProtein(filter.ProteinRange));
            }
        }

        public static PagedList<RecipeSearchModel> SearchRecipes(this IRecipeManager recipeManager,
            int page, int pageSize, RecipeSortType sortType, ViewModels.Filter filter)
        {
            if (pageSize < 1) 
            { pageSize = 1; }

            ICollection<RecipeSearchModel> data =
                Mapper.Map<IEnumerable<Entities.Recipe>, IEnumerable<RecipeSearchModel>>(
                recipeManager.FindByName(filter.SearchKeyword ?? string.Empty)).ToList();

            var orderedData = RecipeHelper.SortRecipes(recipeManager.FilterProducts(data, filter), sortType);

            int pages = (data.Count + pageSize - 1) / pageSize;
            if (page > pages) { page = pages; }
            if (page < 1) { page = 1; }

            return new PagedList<RecipeSearchModel>(orderedData, page, pageSize);
        }
    }
}