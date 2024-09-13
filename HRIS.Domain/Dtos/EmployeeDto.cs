using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Dtos
{
    public class EmployeeDto
    {
        public string NameEmp { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Sex { get; set; }
        public string? Ssn { get; set; }
        public int? IdDept { get; set; }
        public DateOnly Dob { get; set; }
        public string? Email { get; set; }
        public string? JobPosition { get; set; }
        public string? Type { get; set; }
        public string? Level { get; set; }
        public string? Reason { get; set; }
        public string? Status { get; set; }

        public List<DependentDto> Dependents { get; set; } = new List<DependentDto>();
    }
}
