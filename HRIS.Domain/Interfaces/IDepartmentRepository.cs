using HRIS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<Departments> GetDeptByIdAsync(int id);
        Task<IEnumerable<Departments>> GetAllDeptAsync();
        Task AddDeptAsync(Departments departments);
        Task UpdateDeptAsync(Departments departments);
        Task DeleteDeptAsync(int id);

    }
}
