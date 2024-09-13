using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Models
{
    public class RegisterEmployeeModel
    {
        public string NameEmp { get; set; } = null!;
        public string Ssn { get; set; } = null!;
        public DateOnly Dob { get; set; }
        public string? Address { get; set; }
        public string? Sex { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? JobPosition { get; set; }
        public string? Level { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public string? Reason { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
    }

}
