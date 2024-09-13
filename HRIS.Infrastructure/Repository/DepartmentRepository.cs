using HRIS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Infrastructure.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly hrisDbContext _context;

        public DepartmentRepository(hrisDbContext context)
        {
            _context = context;
        }

        public async Task<Departments> GetDeptByIdAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.IdLocationNavigation) 
                .FirstOrDefaultAsync(d => d.IdDept == id);
        }

        public async Task<IEnumerable<Departments>> GetAllDeptAsync()
        {
            return await _context.Departments
                .Include(d => d.IdLocationNavigation) 
                .ToListAsync();
        }

        public async Task AddDeptAsync(Departments department)
        {
            if (department.IdLocation == null)
            {
                throw new ArgumentException("Location ID is required when creating a department.");
            }

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDeptAsync(Departments department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDeptAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
        }

    }
}
