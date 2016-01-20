using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DietPlanner.Models
{
    public struct NutritionData
    {
        public decimal Protein { get; set; }
        public decimal Carbohydrate { get; set; }
        public decimal Fat { get; set; }

        public NutritionData(decimal protein, decimal carbs, decimal fat)
            : this()
        {
            Protein = protein;
            Carbohydrate = carbs;
            Fat = fat;
        }
    }

    public struct UnitName : IDeclinable
    {
        public string Name { get; set; }
        public string NamePlural1 { get; set; }
        public string NamePlural2 { get; set; }
        public string NameDecimal { get; set; }
        public string Short { get; set; }
    }
}