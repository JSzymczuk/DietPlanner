using AutoMapper;
using DietPlanner.ViewModels;
using DietPlanner.Helpers;
using DietPlanner.Entities;
using DietPlanner.Models;
using System.Linq;

namespace DietPlanner
{
    public class AutoMapperConfig
    {
        public static void RegisterMapping()
        {            
            Mapper.CreateMap<Models.GlutenCategory, bool?>();

            Mapper.CreateMap<ProductCategory, ProductCategoryInfo>();
            Mapper.CreateMap<RecipeCategory, RecipeCategoryInfo>();
            Mapper.CreateMap<AppUser, AppUserInfo>();

            Mapper.CreateMap<Product, ProductSearchModel>().ForMember(
                model => model.PortionWeight, entity => entity.MapFrom(
                e => /*e.Pieces ? string.Format("{0}{1}", e.Quantity, e.Unit) : */string.Empty)).ForMember(
                model => model.Categories, entity => entity.MapFrom(
                e=>e.GetCategoriesDescending()));

            Mapper.CreateMap<ViewModels.AddProductViewModel, Product>().ForMember(
                entity => entity.Name, model => model.MapFrom(m => m.ProductName)).ForMember(
                entity => entity.GlutenFree, model => model.MapFrom(m => m.GlutenCategory.ToNullableBool())).ForMember(
                entity => entity.DefaultUnitId, model => model.MapFrom(m => m.UnitId)).AfterMap((m, e) => { 
                e.Verified = false; });

            Mapper.CreateMap<Recipe, RecipeSearchModel>().BeforeMap((e, i) => 
            { 
                int weight = e.GetWeight(); 
                i.Weight = weight;
                NutritionData nd = e.GetNutrition(); 
                i.Protein = ProductHelper.ScaleToQuantity(nd.Protein, weight);
                i.Carbohydrate = ProductHelper.ScaleToQuantity(nd.Carbohydrate, weight);
                i.Fat = ProductHelper.ScaleToQuantity(nd.Fat, weight); 
                i.GlutenFree = e.IsGlutenFree();
                i.Calories = (int)(4 * i.Protein + 4 * i.Carbohydrate + 9 * i.Fat);
            }).ForMember(
                item => item.Category, entity => entity.MapFrom(
                    e => e.RecipeCategory)).ForMember(
                    item => item.Comments, entity => entity.MapFrom(
                        e=>e.Comments.Count));

            Mapper.CreateMap<Ingredient, IngredientListItemModel>().BeforeMap((e, i) =>
                {
                    i.Alias = string.IsNullOrWhiteSpace(e.Name) ? e.Product.Name : e.Name;
                    decimal weight = e.GetWeight();
                    i.PortionWeight = weight.ToString() + "g";
                    i.Protein = ProductHelper.ScaleToQuantity(e.Product.Protein, weight);
                    i.Fat = ProductHelper.ScaleToQuantity(e.Product.Fat, weight);
                    i.Carbohydrate = ProductHelper.ScaleToQuantity(e.Product.Carbohydrate, weight);
                });

            Mapper.CreateMap<Comment, CommentModel>().ForMember(
                model => model.AppUserInfo, cmt => cmt.MapFrom(c => c.User));

            Mapper.CreateMap<AddCommentModel, Comment>().BeforeMap((m, e) =>
                {
                    e.UserId = AccountHelper.GetLoggedUserId();
                });

            Mapper.CreateMap<Recipe, RecipeDetailsViewModel>().BeforeMap((rcp, model) =>
                {
                    model.VotesCount = rcp.Votes.Count;
                    string userId = AccountHelper.GetLoggedUserId();
                    if (userId != null)
                    {
                        model.UserLoggedIn = true;
                        model.VoteAllowed = !rcp.Votes.Any(v => v.UserId == userId);
                    }
                    else
                    {
                        model.UserLoggedIn = false;
                        model.VoteAllowed = false;
                    }
                }).ForMember(model => model.AppUserInfo, rcp => rcp.MapFrom(r => r.Author));

            Mapper.CreateMap<RegisterViewModel, AppUser>();

            Mapper.CreateMap<Unit, UnitInfoModel>().BeforeMap((e, m) =>
            {
                m.BaseUnitName = e.BaseUnit != null ? e.BaseUnit.Name : "brak";
                if (e.Ratio != null)
                {
                    m.IsGeneralMeasure = true;
                    m.Base = e.Ratio.Base;
                    m.Derived = e.Ratio.Derived;
                }
                else if (e.BaseUnit == null)
                {
                    m.IsGeneralMeasure = true;
                    m.Base = 1;
                    m.Derived = 1;
                }
                else
                { 
                    m.IsGeneralMeasure = false; 
                }                
            });

            Mapper.CreateMap<Unit, MeasureInfo>().ForMember(ms => ms.Name, opt => opt.Ignore()).AfterMap((e, m) =>
                {
                    m.Name = new UnitName
                    {
                        Name = e.Name,
                        NameDecimal = e.NameDecimal,
                        NamePlural1 = e.NamePlural1,
                        NamePlural2 = e.NamePlural2,
                        Short = e.Short
                    }; 
                    if (e.BaseUnit == null)
                    {
                        m.IsGeneralMeasure = true;
                        m.Base = 1;
                        m.Derived = 1;
                        m.HasBase = false;
                    }
                    else
                    {
                        m.HasBase = true;
                        m.BaseName = new UnitName
                        {
                            Name = e.BaseUnit.Name,
                            NameDecimal = e.BaseUnit.NameDecimal,
                            NamePlural1 = e.BaseUnit.NamePlural1,
                            NamePlural2 = e.BaseUnit.NamePlural2,
                            Short = e.BaseUnit.Short
                        }; 
                        if (e.Ratio != null)
                        {
                            m.IsGeneralMeasure = true;
                            m.Base = e.Ratio.Base;
                            m.Derived = e.Ratio.Derived;
                        }
                        else
                        {
                            m.IsGeneralMeasure = false;
                        } 
                    }
                });

            Mapper.CreateMap<ProductUnitUnitRatio, MeasureInfo>().AfterMap((e, m) =>
            {
                Unit u = e.Unit;
                m.Name = new UnitName
                {
                    Name = u.Name,
                    NameDecimal = u.NameDecimal,
                    NamePlural1 = u.NamePlural1,
                    NamePlural2 = u.NamePlural2,
                    Short = u.Short
                };
                m.HasBase = true;
                m.BaseName = new UnitName
                {
                    Name = u.BaseUnit.Name,
                    NameDecimal = u.BaseUnit.NameDecimal,
                    NamePlural1 = u.BaseUnit.NamePlural1,
                    NamePlural2 = u.BaseUnit.NamePlural2,
                    Short = u.BaseUnit.Short
                };
                m.Id = u.Id;
                m.Base = e.Ratio.Base;
                m.Derived = e.Ratio.Derived;
                m.IsGeneralMeasure = false;
            });
        }
    }
}
