using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietPlanner.Entities
{
    public class Meal
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; }

        [Required]
        public string AuthorId { get; set; }         // 1 do wielu
        [ForeignKey("AuthorId")]
        public virtual AppUser Author { get; set; }

        [Required]
        public Guid DayId { get; set; }         // 1 do wielu
        [ForeignKey("DayId")]
        public virtual Day Day { get; set; }
    }

    public class Day
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        public virtual ICollection<Meal> Meals { get; set; }
    }
}
