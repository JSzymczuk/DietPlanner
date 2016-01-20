using AutoMapper;
using DietPlanner.Contract;
using DietPlanner.Entities;
using DietPlanner.Helpers;
using DietPlanner.Models;
using DietPlanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DietPlanner.Controllers
{
    public class RecipeController : Controller
    {
        private IRecipeManager recipeManager;
        private IProductManager productManager;

        private const string currentRecipeCookie = "CurrentRecipe";
        private const string createdRecipeIngredients = "Ingredients";

        public IRecipeManager RecipeManager { get { return recipeManager; } set { recipeManager = value; } }
        public IProductManager ProductManager { get { return productManager; } set { productManager = value; } }

        public RecipeController()
        {
        }

        public RecipeController(IRecipeManager recipeManager, IProductManager productManager)
        {
            RecipeManager = recipeManager;
            ProductManager = productManager;
        }

        private string createdRecipeName = "Name";
        private string createdRecipeDesc = "Desc";
        private string createdRecipeVege = "Vege";
        private string createdRecipeCate = "Cate";
        private string createdRecipePrep = "Prep";
        private int defaultPageSize = 10;


        [AllowAnonymous]
        public ActionResult Create()
        {
            ViewBag.Categories = RecipeManager.RecipeCategories.ToSelectList<Entities.RecipeCategory, RecipeCategoryInfo>();
            ViewBag.OrderedCategories = ProductManager.ProductCategories.ToOrderedSelectList();

            HttpCookie cookie = CookieHelper.GetCookie(currentRecipeCookie);
            if (cookie == null)
            {
                return View(new AddRecipeViewModel
                {
                    PageNumber = 1,
                    PageSize = defaultPageSize,
                    SortType = ProductSortType.NameAscending,
                    DataFilter = new ViewModels.Filter { SearchKeyword = string.Empty }, 
                    PreparationTime = 30
                });
            }
            else
            {
                AddRecipeViewModel model = new AddRecipeViewModel();
                model.Name = CookieHelper.ReadCookie(cookie, createdRecipeName);
                model.Description = CookieHelper.ReadCookie(cookie, createdRecipeDesc);

                bool vege;
                Guid cate;
                int prep;

                if (CookieHelper.TryReadCookie<bool>(cookie, createdRecipeVege, out vege))
                {
                    model.Vegetarian = vege;
                }
                if (CookieHelper.TryReadCookie<Guid>(cookie, createdRecipeCate, out cate))
                {
                    model.CategoryId = cate;
                }
                if (CookieHelper.TryReadCookie<int>(cookie, createdRecipePrep, out prep))
                {
                    model.PreparationTime = prep;
                }
                return View(model);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRecipe(AddRecipeViewModel model)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                var recipe = new Entities.Recipe
                {
                    Name = model.Name,
                    Description = model.Description,
                    Vegetarian = model.Vegetarian,
                    PreparationTime = model.PreparationTime,
                    Added = DateTime.Now, 
                    Author = null
                };

                List<Ingredient> ingredients = new List<Ingredient>();
                ICollection<IngredientCreateViewModel> ingredientsInfo;
                if (CookieHelper.TryReadCookie<ICollection<IngredientCreateViewModel>>(
                    currentRecipeCookie, createdRecipeIngredients, out ingredientsInfo))
                {
                    foreach (var ingr in ingredientsInfo)
	                {
		                ingredients.Add(new Ingredient{ 
                            Name = ingr.Alias, 
                            ProductId = ingr.ProductId,
                            Product = ProductManager.FindById(ingr.ProductId), 
                            Quantity = ingr.Quantity,
                            Recipe = recipe});
	                }
                }
                recipe.Ingredients = ingredients;

                RecipeManager.AddToCategory(recipe, RecipeManager.FindCategoryById(model.CategoryId));
                RecipeManager.Save();

                CookieHelper.DeleteCookie(currentRecipeCookie);

                return RedirectToAction("CreateResult");
            }

            ViewBag.Categories = RecipeManager.RecipeCategories.ToSelectList<Entities.RecipeCategory, RecipeCategoryInfo>();
            ViewBag.OrderedCategories = ProductManager.ProductCategories.ToOrderedSelectList();

            return View("Create");
        }

        [AllowAnonymous]
        public ActionResult CreateResult()
        { 
            return View(); 
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.RecipeCategories = RecipeManager.RecipeCategories.ToSelectList<Entities.RecipeCategory, RecipeCategoryInfo>();
            return View(new RecipeSearchViewModel
            {
                PageNumber = 1,
                PageSize = defaultPageSize,
                SortType = RecipeSortType.NameAscending,
                DataFilter = new ViewModels.Filter { SearchKeyword = string.Empty }
            });
        }

        public ActionResult Details(Guid? id)
        {
            
            if (id.HasValue)
            {
                return View(Mapper.Map<Recipe, RecipeDetailsViewModel>(RecipeManager.FindById(id.Value)));
            }
            else
            {
                return View();
            }
        }

        public PartialViewResult AddIngredient(Guid id)
        {
            HttpCookie cookie;
            var product = ProductManager.FindById(id);
            IngredientCreateViewModel ingredient = new IngredientCreateViewModel 
            {
                ProductId = id, 
                Quantity = 0, 
                ProductName = product.Name, 
                BaseProtein = product.Protein,
                BaseCarbohydrate = product.Carbohydrate, 
                BaseFat = product.Fat
            };
            ICollection<IngredientCreateViewModel> ingredients;

            if (CookieHelper.CookieExists(currentRecipeCookie, out cookie))
            {
                ingredients = CookieHelper.ReadCookie<ICollection<IngredientCreateViewModel>>(cookie, createdRecipeIngredients);
                
                if (ingredients == null)
                {
                    CookieHelper.DeleteCookie(currentRecipeCookie);
                    ingredients = new List<IngredientCreateViewModel> { ingredient };
                    CookieHelper.CreateCookie<ICollection<IngredientCreateViewModel>>(
                        currentRecipeCookie, createdRecipeIngredients, ingredients, new TimeSpan(0, 30, 0));
                }
                else
                {
                    if (!ingredients.Any(i => i.ProductId == id))
                    {
                        ingredients.Add(ingredient);
                        CookieHelper.UpdateCookie<ICollection<IngredientCreateViewModel>>(cookie, createdRecipeIngredients, ingredients);
                    }
                }
            }
            else
            {
                ingredients = new List<IngredientCreateViewModel> { ingredient };
                CookieHelper.CreateCookie<ICollection<IngredientCreateViewModel>>(
                    currentRecipeCookie, createdRecipeIngredients, ingredients, new TimeSpan(0, 30, 0));
            }

            return PartialView("_CreatedRecipeIngredients", ingredients);
        }

        public PartialViewResult RemoveIngredient(Guid id)
        {
            HttpCookie cookie;
            ICollection<IngredientCreateViewModel> ingredients;

            if (CookieHelper.CookieExists(currentRecipeCookie, out cookie))
            {
                ingredients = CookieHelper.ReadCookie<ICollection<IngredientCreateViewModel>>(cookie, createdRecipeIngredients);
                
                if (ingredients == null)
                {
                    CookieHelper.DeleteCookie(currentRecipeCookie);
                }
                else
                {

                    IngredientCreateViewModel ingredient = ingredients.First(i => i.ProductId == id);
                    if (ingredient != null)
                    {
                        ingredients.Remove(ingredient);
                        CookieHelper.UpdateCookie<ICollection<IngredientCreateViewModel>>(cookie, createdRecipeIngredients, ingredients);
                    }   
                }
            }
            else { ingredients = new List<IngredientCreateViewModel>(); }

            return PartialView("_CreatedRecipeIngredients", ingredients);
        }

        public void UpdateIngredientAlias(Guid productId, string alias)
        {
            if (CookieHelper.CookieExists(currentRecipeCookie))
            {
                ICollection<IngredientCreateViewModel> ingredients;
                if (CookieHelper.TryReadCookie<ICollection<IngredientCreateViewModel>>(
                    currentRecipeCookie, createdRecipeIngredients, out ingredients))
                {
                    foreach (var ingr in ingredients)
                    {
                        if (ingr.ProductId == productId)
                        {
                            ingr.Alias = alias;
                            break;
                        }
                    }
                    CookieHelper.UpdateCookie<ICollection<IngredientCreateViewModel>>(
                        currentRecipeCookie, createdRecipeIngredients, ingredients);
                }
            }
        }

        public MvcHtmlString UpdateIngredientQuantity(Guid productId, int quantity)
        {
            ICollection<IngredientCreateViewModel> ingredients;
            var product = ProductManager.FindById(productId);

            if (!TryUpdateCreatedQuantity(productId, quantity, out ingredients))
            {
                // Tworzymy nowe ciasteczko, jeśli jeszcze nie ma, albo wystąpiły problemy z jedno odczytem.
                
                if (product != null)
                {
                    ingredients = new List<IngredientCreateViewModel>{
                        new IngredientCreateViewModel { ProductId = productId, ProductName = product.Name, Quantity = quantity }
                    };
                    CookieHelper.CreateCookie<ICollection<IngredientCreateViewModel>>(
                        currentRecipeCookie, createdRecipeIngredients, ingredients, new TimeSpan(0, 30, 0));
                }
                else
                {
                    ingredients = CookieHelper.ReadCookie<ICollection<IngredientCreateViewModel>>(
                        currentRecipeCookie, createdRecipeIngredients) ?? new List<IngredientCreateViewModel>();
                }
            }
            return new MvcHtmlString("[\"" + RecipeHelper.GetNutritionString(
                quantity * product.Protein / 100, 
                quantity * product.Carbohydrate / 100, 
                quantity * product.Fat / 100) + "\",\"" +
                RecipeHelper.GetTotalNutritionString(ingredients) + "\"]");
            //return PartialView("_CreatedRecipeIngredients", ingredients);
        }

        [NonAction]
        private bool TryUpdateCreatedQuantity(Guid productId, int quantity, out ICollection<IngredientCreateViewModel> ingredients)
        {
            HttpCookie recipeCookie;
            if (CookieHelper.CookieExists(currentRecipeCookie, out recipeCookie))
            {
                ingredients = CookieHelper.ReadCookie<ICollection<IngredientCreateViewModel>>(
                    recipeCookie, createdRecipeIngredients);
                if (ingredients == null)
                {
                    // Ciasteczko jest uszkodzone.
                    CookieHelper.DeleteCookie(currentRecipeCookie);
                    return false;
                }
                IngredientCreateViewModel ing = ingredients.First(i => i.ProductId == productId);
                if (ing != null)
                {
                    ing.Quantity = quantity;
                }
                else
                {
                    var product = ProductManager.FindById(productId);
                    if (product != null)
                    {
                        ingredients.Add(new IngredientCreateViewModel { ProductId = productId, ProductName = product.Name, Quantity = quantity });
                    }
                    else
                    {
                        return false;
                    }
                }
                CookieHelper.UpdateCookie<ICollection<IngredientCreateViewModel>>(recipeCookie, createdRecipeIngredients, ingredients);
                return true;
            }
            ingredients = null;
            return false;
        }

        public PartialViewResult GetCreatedRecipe()
        {
            ICollection<IngredientCreateViewModel> ingredients;
            if (!CookieHelper.TryReadCookie<ICollection<IngredientCreateViewModel>>(
                currentRecipeCookie, createdRecipeIngredients, out ingredients))
            {
                ingredients = new List<IngredientCreateViewModel>();
            } 
            return PartialView("_CreatedRecipeIngredients", ingredients);
        }

        public ActionResult StoreRecipeAndAddProduct(AddRecipeViewModel model)
        {
            HttpCookie cookie = CookieHelper.GetOrCreateCookie(currentRecipeCookie, new TimeSpan(0,30,0));
            cookie.SetValue(createdRecipeName, model.Name);
            cookie.SetValue(createdRecipeDesc, model.Description);
            cookie.SetValue<bool>(createdRecipeVege, model.Vegetarian);
            cookie.SetValue<int>(createdRecipePrep, model.PreparationTime);
            cookie.SetValue<Guid>(createdRecipeCate, model.CategoryId);
            cookie.Save();
            return RedirectToAction("Create", "Product");
        }
        
        [HttpGet]
        public PartialViewResult SearchRecipes()
        {
            return PartialView("_SearchRecipes", RecipeManager.SearchRecipes(1,
                defaultPageSize, RecipeSortType.NameAscending, ViewModels.Filter.Default));
        }

        [HttpPost]
        public PartialViewResult SearchRecipes(RecipeSearchViewModel vm)
        {
            return PartialView("_SearchRecipes", RecipeManager.SearchRecipes(vm.PageNumber,
                vm.PageSize, vm.SortType, vm.DataFilter));
        }

        public ActionResult Rate(bool success = false, string error = null)
        {
            ViewBag.Success = success;
            if (!success)
            {
                ViewBag.Error = error;
            }
            return View();
        }

    }
}