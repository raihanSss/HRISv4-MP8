using HRIS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Dtos
{
    public class DashboardDto
    {
        public Dictionary<string, string> EmployeeDistributionByDepartment { get; set; }
        public List<EmployeePerformance> TopEmployeesByPerformance { get; set; }

       public Dictionary<string, decimal> GetAverageSalaryByDepartmentAsync {  get; set; }

       
        public List<ProcessDetailDTO> ProcessDetails { get; set; }
    }
}
