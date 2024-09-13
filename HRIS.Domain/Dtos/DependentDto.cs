using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Dtos
{
    public class DependentDto
    {
        public string Namedependent { get; set; } = null!;
        public string? Sexdependent { get; set; }
        public DateOnly Dobdependent { get; set; }
        public string Relation { get; set; } = null!;
    }
}
