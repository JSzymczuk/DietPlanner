using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietPlanner.Entities
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Guid CategoryId { get; set; }         // 1 do wielu

        [ForeignKey("CategoryId")]
        public virtual ProductCategory ProductCategory { get; set; }

        [Required]
        public Guid DefaultUnitId { get; set; }         // 1 do wielu

        [ForeignKey("DefaultUnitId")]
        public virtual Unit DefaultUnit { get; set; }   

        public virtual ICollection<Ingredient> Ingredients { get; set; }

        public bool Verified { get; set; }

        [Required]
        public decimal Protein { get; set; }         // Białko

        [Required]
        public decimal Carbohydrate { get; set; }    // Węglowodany

        [Required]
        public decimal Fat { get; set; }             // Tłuszcze

        public decimal? Sugar { get; set; }           // Cukry proste

        public decimal? SaturatedFat { get; set; }    // Tłuszcze nasycone

        public decimal? Cholesterol { get; set; }

        public decimal? Salt { get; set; }            // Sól

        public decimal? Fiber { get; set; }           // Błonnik

        public decimal? Water { get; set; }

        public decimal? Alcohol { get; set; }

        public bool? GlutenFree { get; set; }
        
        public virtual ICollection<ProductUnitUnitRatio> ProductUnitUnitRatios { get; set; }

        public Product() 
        {
            Verified = false;
            Ingredients = new List<Ingredient>();
        }

        public override string ToString() { return Name; }
    }
    
    [Table("ProductCategories")]
    public class ProductCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public Guid? ParentId { get; set; }         // 1 do wielu
        [ForeignKey("ParentId")]
        public virtual ProductCategory ParentCategory { get; set; }

        public virtual ICollection<ProductCategory> ChildCategories { get; set; }

        public virtual ICollection<Product> CategoryMembers { get; set; }   // 1 do wielu
    }

    public class Unit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Dla 2 - 4
        public string NamePlural1 { get; set; }

        // Dla 5+
        public string NamePlural2 { get; set; }

        // Dla ułamków
        public string NameDecimal { get; set; }

        public string Short { get; set; }
        
        public Guid? BaseUnitId { get; set; }

        [ForeignKey("BaseUnitId")]
        public virtual Unit BaseUnit { get; set; }

        // Jeśli nie null to używany przelicznik Ratio.Derived * Unit = Ratio.Base * BaseUnit 
        // np. 100g = 89ml => 1g = 0.89ml => 1ml = 100/89g.
        // Jeśli null, to zależy to szukamy w zależnoścu od produktu.

        public Guid? RatioId { get; set; }

        [ForeignKey("RatioId")]
        public virtual UnitRatio Ratio { get; set; }

        public virtual ICollection<ProductUnitUnitRatio> ProductUnitUnitRatios { get; set; }
    }

    public class UnitRatio
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public int Base { get; set; }

        [Required]
        public int Derived { get; set; }

        public virtual ICollection<Unit> Unit { get; set; }

        public virtual ICollection<ProductUnitUnitRatio> ProductUnitUnitRatios { get; set; }
    }

    public class ProductUnitUnitRatio
    {
        [Key, Column(Order = 0)]
        public Guid ProductId { get; set; }

        [Key, Column(Order = 1)]
        public Guid UnitId { get; set; }

        [Key, Column(Order = 2)]
        public Guid RatioId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("UnitId")]
        public virtual Unit Unit { get; set; }

        [ForeignKey("RatioId")]
        public virtual UnitRatio Ratio { get; set; }
    }
}
