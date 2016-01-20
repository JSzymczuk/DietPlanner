using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DietPlanner.Models
{
    public class RecipeCategoryInfo : ICategory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public RecipeCategoryInfo() { }

        public RecipeCategoryInfo(string name)
        {
            CategoryName = name;
        }
    }
}