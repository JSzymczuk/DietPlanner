using DietPlanner.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DietPlanner.ViewModels
{
    public struct SearchRange
    {
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }

        public SearchRange(int? min, int? max)
            : this()
        {
            MinValue = min;
            MaxValue = max;
        }
    }

    public struct Filter
    {
        public string SearchKeyword { get; set; }
        public Guid? CategoryId { get; set; }
        public bool GlutenFree { get; set; }
        public SearchRange ProteinRange { get; set; }
        public SearchRange FatRange { get; set; }
        public SearchRange CarbRange { get; set; }

        public static Filter Default
        {
            get
            {
                return new ViewModels.Filter
                    {
                        GlutenFree = false,
                        SearchKeyword = string.Empty,
                    };
            }
        }
    }

    public abstract class SearchViewModel<DataType, DataSortType>
    {
        public DataSortType SortType { get; set; }
        public Filter DataFilter { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}