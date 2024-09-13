using HRIS.Domain.Interfaces;
using HRIS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Application.Services
{
    public class DependentService : IDependentService
    {
        private readonly IDependentRepository _dependentRepository;

        public DependentService(IDependentRepository dependentRepository)
        {
            _dependentRepository = dependentRepository;
        }

        public async Task<IEnumerable<Dependents>> GetAllDependentsAsync()
        {
            return await _dependentRepository.GetAllDependentsAsync();
        }

        public async Task<Dependents?> GetDependentByIdAsync(int id)
        {
            return await _dependentRepository.GetDependentByIdAsync(id);
        }
    }
}
