using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Dtos
{
    public class RefreshTokenResponseDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiredOn { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
