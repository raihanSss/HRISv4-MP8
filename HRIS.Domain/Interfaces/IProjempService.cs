using HRIS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Interfaces
{
    public interface IProjempService
    {
        Task<IEnumerable<Projemp>> GetAllProjempAsync();
        Task<Projemp?> GetProjempByIdAsync(int id);
        Task<string> AddProjempAsync(Projemp projemp);
        Task<string> UpdateProjempAsync(Projemp projemp);
        Task<string> DeleteProjempAsync(int id);
    }
}
