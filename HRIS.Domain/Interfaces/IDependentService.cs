using HRIS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Interfaces
{
    public interface IDependentService
    {
        Task<IEnumerable<Dependents>> GetAllDependentsAsync();
        Task<Dependents?> GetDependentByIdAsync(int id);
    }
}
