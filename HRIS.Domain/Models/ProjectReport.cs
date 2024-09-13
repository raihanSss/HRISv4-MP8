using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Models
{
    public class ProjectReport
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public decimal TotalHoursLogged { get; set; }
        public int NumberOfEmployeesInvolved { get; set; }
        public double AverageHoursPerEmployee { get; set; }
    }
}
