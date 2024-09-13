using HRIS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace HRIS.Infrastructure.Repository
{
    public class DependentRepository : IDependentRepository
    {
        private readonly hrisDbContext _context;

        public DependentRepository(hrisDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dependents>> GetAllDependentsAsync()
        {
            return await _context.Dependents.ToListAsync();
        }

        public async Task<Dependents?> GetDependentByIdAsync(int id)
        {
            return await _context.Dependents.FindAsync(id);
        }
    }
}
