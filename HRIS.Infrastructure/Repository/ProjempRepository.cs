using HRIS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace HRIS.Infrastructure.Repository
{
    public class ProjempRepository : IProjempRepository
    {
        private readonly hrisDbContext _context;

        public ProjempRepository(hrisDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Projemp>> GetAllProjempAsync()
        {
            return await _context.Projemp
                .Include(p => p.IdEmpNavigation)
                .Include(p => p.IdProjNavigation)
                .ToListAsync();
        }

        public async Task<Projemp?> GetProjempByIdAsync(int id)
        {
            return await _context.Projemp
                .Include(p => p.IdEmpNavigation)
                .Include(p => p.IdProjNavigation)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddProjempAsync(Projemp projemp)
        {
            _context.Projemp.Add(projemp);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProjempAsync(Projemp projemp)
        {
            _context.Projemp.Update(projemp);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProjempAsync(int id)
        {
            var projemp = await _context.Projemp.FindAsync(id);
            if (projemp != null)
            {
                _context.Projemp.Remove(projemp);
                await _context.SaveChangesAsync();
            }
        }
    }
}
