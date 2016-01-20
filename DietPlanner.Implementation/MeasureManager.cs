using DietPlanner.Contract;
using DietPlanner.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietPlanner.Implementation
{
    public class MeasureManager : DisposableManager, IMeasureManager 
    {
        public MeasureManager(DietPlannerDbContext context) : base(context) { }

        public IQueryable<Unit> Measures
        {
            get { return Context.Units; }
        }

        public IQueryable<ProductUnitUnitRatio> ProductMeasures
        {
            get { return Context.ProductUnitUnitRatios; }
        }

        public IEnumerable<Unit> DerivedMeasures(Guid? id)
        {
            return Measures.Where(u => u.BaseUnit.Id == id);
        }

        public Unit FindMeasure(Guid id)
        {
            try { return Measures.First(c => c.Id == id); }
            catch (Exception) { return null; }
        }

        public void Create(ProductUnitUnitRatio productMeasureRatio)
        {
            if (productMeasureRatio != null)
            {
                Context.ProductUnitUnitRatios.Add(productMeasureRatio);
            }
        }

        public UnitRatio RatioFor(Guid unitId, Guid productId)
        {
            try
            {
                Unit unit = Measures.First(u => u.Id == unitId);
                if (unit.BaseUnit == null) { return null; }
                if (unit.Ratio != null)
                {
                    return unit.Ratio;
                }
                else
                {
                    return ProductMeasures.First(m => m.ProductId == productId && m.UnitId == unitId).Ratio;
                }
            }
            catch (Exception) { return null; }
        }

        private bool MeasureIsDefined(Unit measure, Guid productId)
        {
            if (measure == null) { return false; }
            while (measure != null)
            {
                if (measure.Ratio != null || RatioFor(measure.Id, productId) != null || measure.BaseUnit == null)
                {
                    measure = measure.BaseUnit;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public bool MeasureIsDefined(Guid measureId, Guid productId)
        {
            return MeasureIsDefined(FindMeasure(measureId), productId);
        }

        public bool MeasureCanBeDefined(Guid measureId, Guid productId)
        {
            Unit measure = FindMeasure(measureId);
            if (measure != null
                && measure.BaseUnit != null
                && measure.Ratio == null                    // można definiować tylko miary nieuniwersalne
                && RatioFor(measure.Id, productId) == null) // miare nieuniwersalna nie jest zdefiniowana
            {
                return MeasureIsDefined(measure.BaseUnit, productId);
            }
            return false;
        }
    }
}
