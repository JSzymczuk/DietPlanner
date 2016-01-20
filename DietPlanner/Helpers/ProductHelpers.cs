using AutoMapper;
using DietPlanner.Contract;
using DietPlanner.Entities;
using DietPlanner.Models;
using DietPlanner.ViewModels;
using Microsoft.Practices.Unity.Utility;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace DietPlanner.Helpers
{
    public static class ProductHelper
    {
        public static decimal ScaleToQuantity(decimal in100g, decimal quantity)
        {
            if (quantity > 0)
            {
                return in100g / 100 * quantity;
            }
            return 0;
        }

        public static SelectList EnumProductSortType 
        {
            get { return GeneralHelper.SelectListForEnum(typeof(ProductSortType)); } 
        }

        public static SelectList EnumGlutenCategory
        {
            get { return GeneralHelper.SelectListForEnum(typeof(GlutenCategory)); }
        }

        public static SelectList ToOrderedSelectList(this IEnumerable<Entities.ProductCategory> categories)
        {
            var dict = new Dictionary<Guid, List<Entities.ProductCategory>>();
            Guid defaultGuid = Guid.Empty;

            foreach (var category in categories)
            {
                Guid key = category.ParentId.HasValue ? category.ParentId.Value : defaultGuid;
                if (dict.ContainsKey(key))
                {
                    dict[key].Add(category);
                }
                else
                {
                    dict.Add(key, new List<Entities.ProductCategory> { category });
                }
            }

            List<Entities.ProductCategory> orderedCategories;

            if (dict.ContainsKey(defaultGuid))
            {
                string defaultCategoryName = "Inne";
                Comparison<Entities.ProductCategory> comparision = (Entities.ProductCategory c1, Entities.ProductCategory c2) =>
                {
                    if (c1 == null || c1.CategoryName == defaultCategoryName) { return 1; }
                    else if (c2 == null || c2.CategoryName == defaultCategoryName) { return -1; }
                    else { return c1.CategoryName.CompareTo(c2.CategoryName); }
                };
                orderedCategories = GetOrderedCategories(dict, defaultGuid, comparision);
            }
            else
            {
                orderedCategories = new List<Entities.ProductCategory>();
            }

            List<Pair<Guid, string>> orderedNames = new List<Pair<Guid, string>>();
            foreach (var category in orderedCategories)
            {
                // Pozycje na liście zawierają wszystkie kategorie nadrzędne
                orderedNames.Add(new Pair<Guid, string>(category.Id, category.FullCategoryPath(", ")));
                // Wcięcia z twardych spacji
                //orderedNames.Add(new Pair<Guid, string>(category.Id, new string('\u00A0', 3 * category.DepthLevel()) + category.CategoryName));
            }

            return new SelectList(orderedNames, "First", "Second");
        }

        private static List<Entities.ProductCategory> GetOrderedCategories(
            Dictionary<Guid, List<Entities.ProductCategory>> groupedCategories, Guid root,
            Comparison<Entities.ProductCategory> comparision)
        {
            var result = new List<Entities.ProductCategory>();

            var temp = groupedCategories[root];
            temp.Sort(comparision);

            foreach (var category in temp)
            {
                result.Add(category);
                if (groupedCategories.ContainsKey(category.Id))
                {
                    result = result.Concat(GetOrderedCategories(
                        groupedCategories, category.Id, comparision)).ToList();
                }
            }

            return result;
        }

        public static IOrderedEnumerable<ProductSearchModel> SortProducts(
            IEnumerable<ProductSearchModel> data, ProductSortType productSortType)
        {
            switch (productSortType)
            {
                case ProductSortType.NameDescending:
                    return data.OrderByDescending(item => item.Name);
                case ProductSortType.ProteinAscending:
                    return data.OrderBy(item => item.Protein);
                case ProductSortType.ProteinDescending:
                    return data.OrderByDescending(item => item.Protein);
                case ProductSortType.CarbohydrateAscending:
                    return data.OrderBy(item => item.Carbohydrate);
                case ProductSortType.CarbohydrateDescending:
                    return data.OrderByDescending(item => item.Carbohydrate);
                case ProductSortType.FatAscending:
                    return data.OrderBy(item => item.Fat);
                case ProductSortType.FatDescending:
                    return data.OrderByDescending(item => item.Fat);
                default:
                    return data.OrderBy(item => item.Name);
            }
        }

        private static IEnumerable<ProductSearchModel> FilterProducts(this IProductManager productManager,
            IEnumerable<ProductSearchModel> data, ViewModels.Filter filter)
        {
            if (filter.CategoryId.HasValue)
            {
                return data.Where(p =>
                    (!filter.GlutenFree || p.GlutenFree.HasValue && p.GlutenFree.Value)
                    && productManager.ProductInCategory(p.Id, filter.CategoryId.Value)
                    && p.InRangeCarbohydrate(filter.CarbRange)
                    && p.InRangeFat(filter.FatRange)
                    && p.InRangeProtein(filter.ProteinRange));
            }
            else
            {
                return data.Where(p =>
                    (!filter.GlutenFree || p.GlutenFree.HasValue && p.GlutenFree.Value)
                    && p.InRangeCarbohydrate(filter.CarbRange)
                    && p.InRangeFat(filter.FatRange)
                    && p.InRangeProtein(filter.ProteinRange));
            }
        }

        public static PagedList<ProductSearchModel> SearchProducts(this IProductManager productManager,
            int page, int pageSize, ProductSortType sortType, ViewModels.Filter filter)
        {
            if (pageSize < 1) 
            { pageSize = 1; }

            ICollection<ProductSearchModel> data =
                Mapper.Map<IEnumerable<Entities.Product>, IEnumerable<ProductSearchModel>>(
                productManager.FindByName(filter.SearchKeyword ?? string.Empty)).ToList();

            var orderedData = ProductHelper.SortProducts(productManager.FilterProducts(data, filter), sortType);

            int pages = (data.Count + pageSize - 1) / pageSize;
            if (page > pages) { page = pages; }
            if (page < 1) { page = 1; }
            
            return new PagedList<ProductSearchModel>(orderedData, page, pageSize);
        }
    }

    public static partial class ExtensionMethods
    {
        public static GlutenCategory ToGlutenCategory(this bool? boolValue)
        {
            if (boolValue.HasValue)
            {
                return boolValue.Value ? GlutenCategory.GlutenFree : GlutenCategory.ContainsGluten;
            }
            else
            {
                return GlutenCategory.Undefined;
            }
        }

        public static bool? ToNullableBool(this GlutenCategory category)
        {
            switch (category)
            {
                case GlutenCategory.ContainsGluten:
                    return false;
                case GlutenCategory.GlutenFree:
                    return true;
            }
            return null;
        }

        public static string ToDisplayString(this GlutenCategory category)
        {
            switch (category)
            {
                case GlutenCategory.Undefined:
                    return "Nie określono";
                case GlutenCategory.ContainsGluten:
                    return "Produkt zawiera gluten";
                case GlutenCategory.GlutenFree:
                    return "Produkt bezglutenowy";
                default:
                    return string.Empty;
            }
        }

        public static int DepthLevel(this Entities.ProductCategory category)
        {
            Entities.ProductCategory tempCategory = category;
            int depth = -1;
            while (tempCategory != null)
            {
                depth++;
                tempCategory = tempCategory.ParentCategory;
            }
            return depth;
        }

        public static string FullCategoryPath(this Entities.ProductCategory category, string separator)
        {
            StringBuilder sb = new StringBuilder();
            Stack<Entities.ProductCategory> categories = new Stack<Entities.ProductCategory>();
            Entities.ProductCategory tempCategory = category;
            while (tempCategory != null)
            {
                categories.Push(tempCategory);
                tempCategory = tempCategory.ParentCategory;
            }
            while (categories.Count > 0)
            {
                sb.Append(categories.Pop().CategoryName);
                if (categories.Count > 0)
                {
                    sb.Append(separator);
                }
            }
            return sb.ToString();
        }

        public static bool InRangeProtein(this ICaloriesProvider product, int? min, int? max)
        {
            if (min.HasValue && product.Protein < min.Value) return false;
            if (max.HasValue && product.Protein > max.Value) return false;
            return true;
        }

        public static bool InRangeFat(this ICaloriesProvider product, int? min, int? max)
        {
            if (min.HasValue && product.Fat < min.Value) return false;
            if (max.HasValue && product.Fat > max.Value) return false;
            return true;
        }

        public static bool InRangeCarbohydrate(this ICaloriesProvider product, int? min, int? max)
        {
            if (min.HasValue && product.Carbohydrate < min.Value) return false;
            if (max.HasValue && product.Carbohydrate > max.Value) return false;
            return true;
        }

        public static bool InRangeProtein(this ICaloriesProvider product, SearchRange range)
        {
            return product.InRangeProtein(range.MinValue, range.MaxValue);
        }

        public static bool InRangeFat(this ICaloriesProvider product, SearchRange range)
        {
            return product.InRangeFat(range.MinValue, range.MaxValue);
        }

        public static bool InRangeCarbohydrate(this ICaloriesProvider product, SearchRange range)
        {
            return product.InRangeCarbohydrate(range.MinValue, range.MaxValue);
        }

        public static List<ProductCategoryInfo> GetCategoriesDescending(this Entities.Product product)
        {
            var categories = new Stack<Models.ProductCategoryInfo>();
            var category = product.ProductCategory;
            while (category != null)
            {
                categories.Push(Mapper.Map<Entities.ProductCategory, Models.ProductCategoryInfo>(category));
                category = category.ParentCategory;
            }
            return new List<Models.ProductCategoryInfo>(categories);
        }
    }
}