using HRIS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Interfaces
{
    public interface IDepartmentService
    {
        Task<Departments> GetDeptByIdAsync(int id);
        Task<IEnumerable<Departments>> GetAllDeptAsync();
        Task <string> AddDeptAsync(Departments departments);
        Task <string> UpdateDeptAsync(Departments departments);
        Task <string> DeleteDeptAsync(int id);

    }
}
