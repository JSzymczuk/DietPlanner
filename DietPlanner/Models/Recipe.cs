using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DietPlanner.Models
{
    /*
    public class Recipe : ICaloriesProvider
    {
        [Key]
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

        public string AuthorId { get; set; }

        [Required]
        public Guid CategoryId { get; set; }


        public decimal Rating { get; set; }

        public int Weight { get; set; }

        public NutritionData TotalNutrition { get; set; }

        public NutritionData Nutrition
        {
            get
            {
                decimal weight = Weight;
                if (weight == 0) return new NutritionData();
                decimal mult = 100.0 / weight;
                NutritionData nd = TotalNutrition;
                return new NutritionData(nd.Protein * mult, nd.Carbohydrate * mult, nd.Fat * mult);
            }
        }

        public int Calories
        {
            get
            {
                return 0;
            }
        }

        public decimal Protein
        {
            get
            {
                return Nutrition.Protein;
            }
        }

        public decimal Carbohydrate
        {
            get
            {
                return Nutrition.Carbohydrate;
            }
        }

        public decimal Fat
        {
            get
            {
                return Nutrition.Fat;
            }
        }

        public bool? GlutenFree { get; set; }

        public bool IsGlutenFree 
        { 
            get 
            {
                return GlutenFree.HasValue && GlutenFree.Value;
            }
        }
    }
    */
}