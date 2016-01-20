using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DietPlanner.Models
{
    public enum GlutenCategory
    {
        [Description("Nie określono")]
        Undefined,
        [Description("Produkt zawiera gluten")]
        ContainsGluten,
        [Description("Produkt bezglutenowy")]
        GlutenFree
    }


    public enum ProductSortType
    {
        [Description("Rosnąco względem nazwy")]
        NameAscending,
        [Description("Malejąco względem nazwy")]
        NameDescending,
        [Description("Rosnąco względem zawartości białka")]
        ProteinAscending,
        [Description("Malejąco względem zawartości białka")]
        ProteinDescending,
        [Description("Rosnąco względem zawartości tłuszczu")]
        FatAscending,
        [Description("Malejąco względem zawartości tłuszczu")]
        FatDescending,
        [Description("Rosnąco względem zawartości węglowodanów")]
        CarbohydrateAscending,
        [Description("Malejąco względem zawartości węglowodanów")]
        CarbohydrateDescending
    }

    public enum RecipeSortType
    {
        [Description("Rosnąco względem nazwy")]
        NameAscending,
        [Description("Malejąco względem nazwy")]
        NameDescending,
        [Description("Rosnąco względem oceny")]
        RatingAscending,
        [Description("Malejąco względem oceny")]
        RatingDescending,
        [Description("Rosnąco względem zawartości białka")]
        ProteinAscending,
        [Description("Malejąco względem zawartości białka")]
        ProteinDescending,
        [Description("Rosnąco względem zawartości tłuszczu")]
        FatAscending,
        [Description("Malejąco względem zawartości tłuszczu")]
        FatDescending,
        [Description("Rosnąco względem zawartości węglowodanów")]
        CarbohydrateAscending,
        [Description("Malejąco względem zawartości węglowodanów")]
        CarbohydrateDescending,
        [Description("Rosnąco względem liczby komentarzy")]
        CommentsAscending,
        [Description("Malejąco względem liczby komentarzy")]
        CommentsDescending
    }
}