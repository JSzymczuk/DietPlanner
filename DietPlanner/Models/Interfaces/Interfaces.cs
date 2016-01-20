using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietPlanner.Models
{
    public interface ICaloriesProvider
    {
        string Name { get; }
        int Calories { get; }
        decimal Protein { get; }
        decimal Carbohydrate { get; }
        decimal Fat { get; }
    }

    public interface ICategory
    {
        Guid Id { get; set; }
        string CategoryName { get; set; }
    }

    public interface IDeclinable
    {
        string Name { get; }
        string NamePlural1 { get; }
        string NamePlural2 { get; }
        string NameDecimal { get; }
        string Short { get; }
    }
}
