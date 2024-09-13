using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Models
{
    public class ResponeModel
    {
        public string Status {  get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public DateTime ExpiredOn { get; set; }
    }
}
