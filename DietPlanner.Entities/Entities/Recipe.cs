using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietPlanner.Entities
{
    public class Recipe
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Added { get; set; }

        [Required]
        public int PreparationTime { get; set; }

        [Required]
        public bool Vegetarian { get; set; }

        public double Rating { get; set; }

        public bool IsPrivate { get; set; }

        public bool Verified { get; set; }

        public string AuthorId { get; set; }         // 1 do wielu
        [ForeignKey("AuthorId")]
        public virtual AppUser Author { get; set; }

        [Required]
        public Guid CategoryId { get; set; }        // 1 do wielu
        [ForeignKey("CategoryId")]
        public virtual RecipeCategory RecipeCategory { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; } // 1 do wielu

        public virtual ICollection<Rating> Votes { get; set; } // 1 do wielu

        public virtual ICollection<Comment> Comments { get; set; } // 1 do wielu

        public virtual ICollection<Meal> Meals { get; set; } // wiele do wielu
    }

    [Table("RecipeCategories")]
    public class RecipeCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }    // 1 do wielu
    }

    public class Ingredient
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        public string Name { get; set; }

        [Required]
        public Guid ProductId { get; set; }         // 1 do wielu
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Required]
        public Guid RecipeId { get; set; }         // 1 do wielu
        [ForeignKey("RecipeId")]
        public virtual Recipe Recipe { get; set; }

        public virtual ICollection<AppUser> FavedBy { get; set; }
    }
}
