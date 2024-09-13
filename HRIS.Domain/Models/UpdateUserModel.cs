using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Models
{
    public class UpdateUserModel
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        
        public string? Role { get; set; }
    }
}
