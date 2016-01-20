using DietPlanner.Contract;
using DietPlanner.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietPlanner.Implementation
{
    public class ProductManager : DisposableManager, IProductManager
    {
        public IQueryable<Product> Entities
        {
            get { return Context.Products; }
        }

        public ProductManager(DietPlannerDbContext context) : base(context) {}

        public IQueryable<ProductCategory> ProductCategories 
        {
            get { return Context.ProductCategories; } 
        }

        public Product FindById(Guid id)
        {
            try { return Context.Products.First(c => c.Id == id); }
            catch (Exception) { return null; }
        }
        
        public IQueryable<Product> FindByName(string name)
        {
            return Context.Products.Where(c => c.Name.Contains(name));
        }

        public void Create(Product product)
        {
            if (product != null)
            {
                Context.Products.Add(product);
            }
        }

        public void Delete(Guid id)
        {
            Product product = Context.Products.Find(id);
            Context.Products.Remove(product);
        }

        public void Update(Product product)
        {
            Context.Entry(product).State = EntityState.Modified;
        }

        public ProductCategory FindCategoryByName(string name)
        {
            return Context.ProductCategories.First(c => c.CategoryName == name);
        }

        public ProductCategory FindCategoryById(Guid id)
        {
            return Context.ProductCategories.First(c => c.Id == id);
        }

        public void AddToCategory(Product product, ProductCategory category)
        {
            product.ProductCategory = category;
            category.CategoryMembers.Add(product);
        }
        
        public bool ProductInCategory(Guid productId, Guid categoryId)
        {
            return ProductInCategory(FindById(productId), FindCategoryById(categoryId));
        }
        
        public bool ProductInCategory(Product product, ProductCategory category)
        {
            if (product == null || category == null) { return false; }
            ProductCategory productCategory = product.ProductCategory;
            while (productCategory != null)
            {
                if (productCategory.Id == category.Id) { return true; }
                productCategory = productCategory.ParentCategory;
            }
            return false;
        }
        
        public void VerifyProduct(Product product)
        {
            product.Verified = true;
        }
    }
}
