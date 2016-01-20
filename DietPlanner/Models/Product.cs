using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DietPlanner.Models
{
    public class MeasureInfo
    {
        [Key]
        public Guid Id { get; set; }
        public UnitName Name { get; set; }
        public UnitName BaseName { get; set; }
        public bool HasBase { get; set; }
        public bool IsGeneralMeasure { get; set; }
        public int Base { get; set; }
        public int Derived { get; set; }
        public bool Verified { get; set; }
    }
}