using DietPlanner.Models;
using DietPlanner.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DietPlanner.Helpers
{
    public static class GeneralHelper
    {
        public static SelectList ToSelectList<Category, CategoryInfo>(this IEnumerable<Category> categories)
            where CategoryInfo : ICategory
        {
            var categoriesInfo = AutoMapper.Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryInfo>>(categories);
            return new SelectList(categoriesInfo, "Id", "CategoryName");
        } 

        public static SelectList SelectListForEnum(Type enumType)
        {
            List<object> values = new List<object>();
            foreach (var e in Enum.GetValues(enumType))
            {
                var memInfo = enumType.GetMember(e.ToString());
                var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                var description = ((DescriptionAttribute)attributes[0]).Description;
                values.Add(new { Id = e, Name = description });
            }
            return new SelectList(values, "Id", "Name");
        }

        public static MvcHtmlString CategorySearchLink(this HtmlHelper htmlHelper, ICategory category)
        {
            return MvcHtmlString.Create("<a onclick=\"selectCategory('" + category.Id.ToString() 
                + "');\">" + category.CategoryName + "</a>");
        }
/*
 * Alternatywne rozwiązania tego wyżej. Obydwa mają plusy i minusy.
    @:<a onclick="selectCategory('@(category.Id)');">@(category.CategoryName)</a>
    @Html.ActionLink(category.CategoryName, "", "", new { @href = "#", @onclick = "selectCategory('" + category.Id + "');" })
*/

        public static MvcHtmlString ProductCategories(this HtmlHelper htmlHelper, ProductSearchModel product)
        {
            int counter = product.Categories.Count - 1;
            string result = string.Empty;
            if (counter >= 0)
            {
                for (int i = 0; i < counter; i++)
			    {
			        result += htmlHelper.CategorySearchLink(product.Categories[i]).ToString() + ", ";
			    }
                result += htmlHelper.CategorySearchLink(product.Categories[counter]);
            }
            return new MvcHtmlString(result);
        }
        
        public static string ToString(this decimal number, int digits)
        {
            return number.ToString(digits, true);
        }

        public static string ToString(this decimal number, int digits, bool truncate)
        {
            string format = new string(truncate ? '#' : '0', digits > 0 ? digits : 0);
            return string.Format("{0:0." + format + "}", number);
        }

        public static string QuantityString(this IDeclinable unit, decimal quantity, int digits, bool truncate, bool forceFullName)
        {
            string roundedVal = quantity.ToString(digits, truncate);
            decimal rounded = decimal.Parse(roundedVal);
            if(forceFullName || string.IsNullOrEmpty(unit.Short))
            {
                int roundedInt = Math.Abs((int)rounded);
                if (rounded - roundedInt != 0)
                {
                    return roundedVal + " " + unit.NameDecimal;
                }
                else if (roundedInt > 4 || roundedInt == 0)
                {
                    return roundedVal + " " + unit.NamePlural2;
                }
                else if(roundedInt > 1)
                {
                    return roundedVal + " " + unit.NamePlural1;
                }
                else
                {
                    return roundedVal + " " + unit.Name;
                }
            }
            else
            {
                return roundedVal + unit.Short;
            }
        }
    }
}