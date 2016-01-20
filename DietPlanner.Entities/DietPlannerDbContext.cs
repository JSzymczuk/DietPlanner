using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietPlanner.Entities
{
    public class DietPlannerDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeCategory> RecipeCategories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UnitRatio> UnitRatios { get; set; }
        public DbSet<ProductUnitUnitRatio> ProductUnitUnitRatios { get; set; }
        public DbSet<Report> Reports { get; set; }

        public DietPlannerDbContext() : base("DefaultConnection", throwIfV1Schema: false) { }

        public static DietPlannerDbContext Create()
        {
            return new DietPlannerDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
