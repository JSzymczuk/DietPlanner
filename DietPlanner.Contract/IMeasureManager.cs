using DietPlanner.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietPlanner.Contract
{
    public interface IMeasureManager
    {
        IQueryable<Unit> Measures { get; }
        IQueryable<ProductUnitUnitRatio> ProductMeasures { get; }
        void Create(ProductUnitUnitRatio productMeasureRatio);
        Unit FindMeasure(Guid id);
        UnitRatio RatioFor(Guid unitId, Guid productId);
        bool MeasureIsDefined(Guid measureId, Guid productId);    // czy istnieje miara dla produktu
        bool MeasureCanBeDefined(Guid measureId, Guid productId); // czy można dodać miarę do produktu
        IEnumerable<Unit> DerivedMeasures(Guid? measureId);
        void Save();
    }
}
