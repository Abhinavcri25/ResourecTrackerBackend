using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTracker.Models
{
    public class Resource
    {
        public int? EmpId { get; set; }
        public required string ResourceName { get; set; }
        public required string Designation { get; set; }
        public required string ReportingTo { get; set; }
        public bool Billable { get; set; }
        public required string TechnologySkill { get; set; }
        public required string ProjectAllocation { get; set; }
        public required string Location { get; set; }
        public required string EmailId { get; set; }
        public DateOnly CteDoj { get; set; }
        public required string Remarks { get; set; }


    }
}

