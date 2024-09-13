using HRIS.Domain.Interfaces;
using HRIS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Application.Services
{
    public class ProjempService : IProjempService
    {
        private readonly IProjempRepository _projempRepository;

        public ProjempService(IProjempRepository projempRepository)
        {
            _projempRepository = projempRepository;
        }

        public async Task<IEnumerable<Projemp>> GetAllProjempAsync()
        {
            return await _projempRepository.GetAllProjempAsync();
        }

        public async Task<Projemp?> GetProjempByIdAsync(int id)
        {
            return await _projempRepository.GetProjempByIdAsync(id);
        }

        public async Task<string> AddProjempAsync(Projemp projemp)
        {
            await _projempRepository.AddProjempAsync(projemp);
            return "Projemp added successfully";
        }

        public async Task<string> UpdateProjempAsync(Projemp projemp)
        {
            await _projempRepository.UpdateProjempAsync(projemp);
            return "Projemp updated successfully";
        }

        public async Task<string> DeleteProjempAsync(int id)
        {
            var projemp = await _projempRepository.GetProjempByIdAsync(id);
            if (projemp == null)
            {
                return "Projemp not found";
            }

            await _projempRepository.DeleteProjempAsync(id);
            return "Projemp deleted successfully";
        }
    }
}
