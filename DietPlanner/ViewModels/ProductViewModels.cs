using DietPlanner.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DietPlanner.ViewModels
{
    public class UnitInfoModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NamePlural1 { get; set; }
        public string NamePlural2 { get; set; }
        public string NameDecimal { get; set; }
        public string Short { get; set; }
        public string BaseUnitName { get; set; }
        public bool IsGeneralMeasure { get; set; }
        public int Base { get; set; }
        public int Derived { get; set; }
        public bool Verified { get; set; }
    }

    public class MeasureRatio
    {
        public Guid UnitId { get; set; }
        public Guid ProductId { get; set; }
        public int Base { get; set; }
        public string BaseName { get; set; }
        public int Derived { get; set; }
        public string DerivedName { get; set; }
    }

    public class ProductSearchModel : ICaloriesProvider
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string PortionWeight { get; set; } // masa sztuki

        public List<ProductCategoryInfo> Categories { get; set; } 
        // kategorie uporządkowane od najbardziej ogólnej do najbardziej szczegółowej
        
        public decimal Protein { get; set; }
        public decimal Carbohydrate { get; set; } 
        public decimal Fat { get; set; }

        public decimal? Sugar { get; set; }
        public decimal? SaturatedFat { get; set; }
        public decimal? Cholesterol { get; set; }
        public decimal? Salt { get; set; }
        public decimal? Fiber { get; set; }
        public decimal? Water { get; set; }
        public decimal? Alcohol { get; set; }

        public bool? GlutenFree { get; set; }

        public int Calories
        {
            get { return (int)(Protein * 4 + Carbohydrate * 4 + Fat * 9); }
        }
    }

    public class ProductSearchViewModel : SearchViewModel<ProductSearchModel, ProductSortType>
    { 
    }

    public class CreateProductMeasuresViewModel
    {
        public Guid ProductId { get; set; }

        public Guid DefaultUnitId { get; set; }

        public Guid? NewMeasureId { get; set; }

        public SelectList DefinedMeasures { get; set; }

        public SelectList DefinableMeasures { get; set; }

        public IList<MeasureRatio> ProductMeasures { get; set; }
    }
    
    public class AddProductViewModel
    {
        [Required]
        [Display(Name="Nazwa produktu")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "Kategoria produktu")]
        public Guid CategoryId { get; set; }

        [Required]
        [Display(Name = "Domyślna jednostka")]
        public Guid UnitId { get; set; }

        [Required]
        [Display(Name = "Białko")]
        public decimal Protein { get; set; }

        [Required]
        [Display(Name = "Węglowodany")]
        public decimal Carbohydrate { get; set; }

        [Required]
        [Display(Name = "Tłuszcze")]
        public decimal Fat { get; set; }

        [Display(Name = " w tym cukry")]
        public decimal? Sugar { get; set; }

        [Display(Name = " w tym tłuszcze nasycone")]
        public decimal? SaturatedFat { get; set; }
        
        [Display(Name = "Woda")]
        public decimal? Water { get; set; }

        [Display(Name = "Alkohol")]
        public decimal? Alcohol { get; set; }

        [Display(Name = "Sól")]
        public decimal? Salt { get; set; }

        [Display(Name = "Błonnik")]
        public decimal? Fiber { get; set; }

        [Display(Name = "Cholesterol")]
        public decimal? Cholesterol { get; set; }

        [Display(Name = "Zawartość glutenu")]
        public GlutenCategory GlutenCategory { get; set; }

        public Dictionary<Guid, SelectList> CategoryTree { get; set; }
    }
}