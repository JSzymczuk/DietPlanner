using AutoMapper;
using DietPlanner.Contract;
using DietPlanner.Entities;
using DietPlanner.Models;
using DietPlanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DietPlanner.Helpers
{
    public static class MeasureHelper
    {
        private static ProductUnitUnitRatio GetProductMeasure(this IMeasureManager manager, Guid productId, Guid measureId)
        {
            try
            {
                return manager.ProductMeasures.ToList().First(m => m.ProductId == productId && m.UnitId == measureId);
            }
            catch(Exception) { return null; }
        }

        public static List<MeasureInfo> GetMeasuresForProduct(this IMeasureManager manager, Guid id)
        {
            //Product p = manager.FindById(id);
            Queue<Unit> toCheck = new Queue<Unit>(manager.DerivedMeasures(null));

            List<MeasureInfo> result = new List<MeasureInfo>();

            while (toCheck.Count > 0)
            {
                Unit temp = toCheck.Dequeue();
                if (temp.Ratio != null || temp.BaseUnit == null)
                {
                    result.Add(Mapper.Map<Unit, MeasureInfo>(temp));
                    foreach (var u in manager.DerivedMeasures(temp.Id))
                    {
                        toCheck.Enqueue(u);
                    }                
                }
                else
                {
                    ProductUnitUnitRatio measure = manager.GetProductMeasure(id, temp.Id);
                    if (measure != null)
                    {
                        result.Add(Mapper.Map<ProductUnitUnitRatio, MeasureInfo>(measure));
                        foreach (var u in manager.DerivedMeasures(temp.Id))
                        {
                            toCheck.Enqueue(u);
                        }                
                    }
                }
            }

            return result;
        }

        public static SelectList DefinedMeasures(this IMeasureManager manager, Guid productId)
        {
            List<Unit> list = new List<Unit>();
            foreach (var unit in manager.Measures.ToList())
            {
                if (manager.MeasureIsDefined(unit.Id, productId))
                {
                    list.Add(unit);
                }
            }
            return new SelectList(list, "Id", "Name");
        }

        public static IEnumerable<string> DefinableMeasures(this IMeasureManager manager, Guid productId)
        {
            List<string> result = new List<string>();
            foreach (var unit in manager.Measures)
            {
                if (manager.MeasureCanBeDefined(unit.Id, productId))
                {
                    result.Add(unit.Name);
                }
            }
            return result;
        }

        public static IEnumerable<MeasureRatio> DefinableMeasures(this IMeasureManager manager)
        {
            return manager.DefinableMeasures(new List<MeasureRatio>());
        }

        public static IEnumerable<MeasureRatio> DefinedMeasures(this IMeasureManager manager, IEnumerable<MeasureRatio> defined)
        {
            List<MeasureRatio> result = new List<MeasureRatio>();
            var measures = manager.Measures.ToList();

            foreach (var m in measures)
            {
                bool success = true;
                var temp = m;
                while (temp != null && success)
                {
                    if (temp.Ratio != null || temp.BaseUnit == null || defined.Any(d => d.UnitId == temp.Id))
                    {
                        temp = temp.BaseUnit;
                    }
                    else
                    {
                        success = false;
                    }
                }
                if (success)
                {
                    result.Add(new MeasureRatio
                    {
                        UnitId = m.Id,
                        BaseName = m.Name
                    });
                }
            }

            return result;
        }

        public static IEnumerable<MeasureRatio> DefinableMeasures(this IMeasureManager manager, IEnumerable<MeasureRatio> defined)
        {
            List<MeasureRatio> result = new List<MeasureRatio>();
            var measures = manager.Measures.ToList();

            foreach (var m in measures)
            {
                if (m.Ratio == null && m.BaseUnit != null && !defined.Any(d => d.UnitId == m.Id))
                {
                    bool success = true;
                    var temp = m.BaseUnit;
                    while (temp != null && success)
                    {
                        if (temp.Ratio != null || temp.BaseUnit == null || defined.Any(d => d.UnitId == temp.Id))
                        {
                            temp = temp.BaseUnit;
                        }
                        else
                        {
                            success = false;
                        }
                    }
                    if (success)
                    {
                        result.Add(new MeasureRatio 
                        { 
                            UnitId = m.Id, 
                            BaseName = m.Name, 
                            DerivedName = m.BaseUnit.Name 
                        });
                    }
                }
            }

            return result;
        }
    }
}