using HRIS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Interfaces
{
    public interface IProjempRepository
    {
        Task<IEnumerable<Projemp>> GetAllProjempAsync();
        Task<Projemp?> GetProjempByIdAsync(int id);
        Task AddProjempAsync(Projemp projemp);
        Task UpdateProjempAsync(Projemp projemp);
        Task DeleteProjempAsync(int id);
    }
}
