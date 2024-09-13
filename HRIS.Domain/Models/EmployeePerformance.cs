using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Models
{
    public class EmployeePerformance
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public decimal TotalHoursWorked { get; set; }
    }

}
