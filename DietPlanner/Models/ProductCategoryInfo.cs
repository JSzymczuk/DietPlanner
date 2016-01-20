using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DietPlanner.Models
{
    public class ProductCategoryInfo : ICategory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public Guid? ParentId { get; set; }
    }
}