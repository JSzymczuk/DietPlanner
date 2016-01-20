using DietPlanner.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietPlanner.Contract
{
    public interface IProductManager : IBaseManager<Product, Guid>
    {
        IQueryable<ProductCategory> ProductCategories { get; }
        IQueryable<Product> FindByName(string name);
        ProductCategory FindCategoryById(Guid id);
        ProductCategory FindCategoryByName(string name);
        bool ProductInCategory(Guid productId, Guid categoryId);
        bool ProductInCategory(Product product, ProductCategory category);
        void AddToCategory(Product product, ProductCategory category);
        void VerifyProduct(Product product);
    }
}
