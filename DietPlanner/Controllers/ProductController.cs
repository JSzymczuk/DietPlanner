using AutoMapper;
using DietPlanner.Contract;
using DietPlanner.Entities;
using DietPlanner.Helpers;
using DietPlanner.Models;
using DietPlanner.ViewModels;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DietPlanner.Controllers
{
    public class ProductController : Controller
    {
        private IProductManager productManager;
        private IMeasureManager measureManager;
        private const int defaultPageSize = 10;

        public IProductManager ProductManager
        {
            get { return productManager; }
            set { productManager = value; }
        }

        public IMeasureManager MeasureManager
        {
            get { return measureManager; }
            set { measureManager = value; }
        }

        public ProductController() { }

        public ProductController(IProductManager productManager, IMeasureManager measureManager)
        {
            ProductManager = productManager;
            MeasureManager = measureManager;
        }

        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            ViewBag.OrderedCategories = ProductManager.ProductCategories.ToOrderedSelectList();
            return View(new ProductSearchViewModel
            {
                PageNumber = 1,
                PageSize = defaultPageSize,
                SortType = ProductSortType.NameAscending,
                DataFilter = new ViewModels.Filter { SearchKeyword = string.Empty }
            });
        }

        [AllowAnonymous]
        public virtual ActionResult Create()
        {
            return View(new AddProductViewModel
            {
                CategoryTree = GetCategoryTree()
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddProductViewModel model)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                var product = Mapper.Map<AddProductViewModel, Entities.Product>(model);
                ProductManager.Create(product);
                ProductManager.Save();

                //return RedirectToAction("Index", "Home");
                return Json(product.Id);
            }

            return View(model);
        }

        [HttpGet]
        public PartialViewResult SearchProducts()
        {
            return PartialView("_SearchProducts", ProductManager.SearchProducts(1,
                defaultPageSize, ProductSortType.NameAscending, ViewModels.Filter.Default));
        }

        [HttpPost]
        public PartialViewResult SearchProducts(ProductSearchViewModel vm)
        {
            return PartialView("_SearchProducts", ProductManager.SearchProducts(vm.PageNumber,
                vm.PageSize, vm.SortType, vm.DataFilter));
        }

        [HttpGet]
        public PartialViewResult SearchProductsForRecipe()
        {
            return PartialView("_SearchProductsForRecipe", ProductManager.SearchProducts(1,
                defaultPageSize, ProductSortType.NameAscending, ViewModels.Filter.Default));
        }

        [HttpPost]
        public PartialViewResult SearchProductsForRecipe(ProductSearchViewModel vm)
        {
            return PartialView("_SearchProductsForRecipe", ProductManager.SearchProducts(vm.PageNumber,
                vm.PageSize, vm.SortType, vm.DataFilter));
        }

        [NonAction]
        private Dictionary<Guid, SelectList> GetCategoryTree()
        {
            var dict = new Dictionary<Guid, List<Entities.ProductCategory>>();

            foreach (var category in ProductManager.ProductCategories)
            {
                Guid key = category.ParentId.HasValue ? category.ParentId.Value : Guid.Empty;
                if (dict.ContainsKey(key))
                {
                    dict[key].Add(category);
                }
                else
                {
                    dict.Add(key, new List<Entities.ProductCategory> { category });
                }
            }

            var result = new Dictionary<Guid, SelectList>();
            foreach (var entry in dict)
            {
                result.Add(entry.Key, new SelectList(entry.Value, "Id", "CategoryName"));
            }

            return result;
        }

        public ActionResult Measures()
        {
            return View(Mapper.Map<IEnumerable<Unit>, IEnumerable<UnitInfoModel>>(MeasureManager.Measures));
        }

        public ActionResult MeasuresForProduct(Guid? id)
        {
            if (id.HasValue)
            {
                return View(MeasureManager.GetMeasuresForProduct(id.Value));
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult DefinedMeasures(Guid? productId)
        {
            if (productId.HasValue)
            {
                return View("DefinedMeasures", MeasureManager.DefinedMeasures(productId.Value));
            }
            else
            {
                return View("DefinedMeasures", new List<string>());
            }
        }

        [HttpGet]
        public ActionResult DefinableMeasures(Guid? productId)
        {
            if (productId.HasValue)
            {
                return View("DefinedMeasures", MeasureManager.DefinableMeasures(productId.Value));
            }
            else
            {
                return View("DefinedMeasures", new List<string>());
            }
        }

        [HttpGet]
        public ActionResult DefineMeasure(Guid? productId, Guid? unitId)
        {
            if(productId.HasValue && unitId.HasValue)
            {
                Product product = ProductManager.FindById(productId.Value);
                Unit unit = MeasureManager.FindMeasure(unitId.Value);

                if(product != null && unit != null && unit.BaseUnit != null)
                {
                    MeasureRatio model = new MeasureRatio 
                    {
                        Base = 100, 
                        Derived = 100, 
                        DerivedName = unit.NamePlural2 ?? unit.Name, 
                        BaseName = unit.BaseUnit.NamePlural2 ?? unit.BaseUnit.Name, 
                        ProductId = productId.Value, 
                        UnitId = unitId.Value 
                    };
                    return View(model);
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateProductMeasureRatio(MeasureRatio ratio)
        {
            if (true) //Jeśli konwersja jest możliwa
            {
                MeasureManager.Create(new ProductUnitUnitRatio 
                { 
                    ProductId = ratio.ProductId, 
                    UnitId = ratio.UnitId, 
                    Ratio = new UnitRatio 
                    { 
                        Base = ratio.Base, 
                        Derived = ratio.Derived 
                    } 
                });
                return View("Index");
            }
            //else
            //{
            //    return View();
            //}
        }

        [HttpGet]
        public PartialViewResult CreateProductMeasures()
        {
            var measures = new List<MeasureRatio>();
            return PartialView("_CreateProductMeasures", new CreateProductMeasuresViewModel 
            {
                ProductMeasures = measures, 
                DefinedMeasures = new SelectList(MeasureManager.DefinedMeasures(measures), "UnitId", "BaseName"),
                DefinableMeasures = new SelectList(MeasureManager.DefinableMeasures(measures), "UnitId", "BaseName")
            });
        }

        [HttpPost]
        public PartialViewResult CreateProductMeasures(CreateProductMeasuresViewModel model)
        {
            if (model.NewMeasureId.HasValue)
            {
                var msr = MeasureManager.FindMeasure(model.NewMeasureId.Value);
                if (model.ProductMeasures == null) { model.ProductMeasures = new List<MeasureRatio>(); }
                model.ProductMeasures.Add(new MeasureRatio
                {
                    UnitId = msr.Id,
                    BaseName = msr.BaseUnit.Name,
                    DerivedName = msr.Name,
                    Base = 100, 
                    Derived = 100
                });

                model.DefinableMeasures = new SelectList(
                    MeasureManager.DefinableMeasures(model.ProductMeasures), "UnitId", "BaseName");

                model.DefinedMeasures = new SelectList(
                    MeasureManager.DefinedMeasures(model.ProductMeasures), "UnitId", "BaseName");
            }            
            return PartialView("_CreateProductMeasures", model);
        }

        [HttpPost]
        public ActionResult CreateProductMeasures2(CreateProductMeasuresViewModel model)
        {
            foreach (var measure in model.ProductMeasures)
            {
                MeasureManager.Create(new ProductUnitUnitRatio 
                { 
                    ProductId = model.ProductId, 
                    UnitId = measure.UnitId, 
                    Ratio = new UnitRatio 
                    { 
                        Base = measure.Base, 
                        Derived = measure.Derived 
                    } 
                });
            }
            MeasureManager.Save();
            return RedirectToAction("Index", "Product");
        }

        /*
        [NonAction]
        private SelectList GetCategories(Guid? parentId)
        {
            var result = new List<ProductCategory>();
            foreach (var category in ProductManager.ProductCategories)
            {
                if (category.ParentId == parentId)
                {
                    result.Add(category);
                }
            }
            return new SelectList(result, "Id", "CategoryName");
        }*/
    }
}