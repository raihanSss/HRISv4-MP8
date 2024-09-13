using HRIS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Infrastructure.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly hrisDbContext _context;

        public ProjectRepository(hrisDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Projects>> GetAllProjectsAsync()
        {
            return await _context.Projects
                .Include(p => p.IdDeptNavigation)
                .Include(p => p.IdLocationNavigation)
                .ToListAsync();
        }

        public async Task<Projects?> GetProjectByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.IdDeptNavigation)
                .Include(p => p.IdLocationNavigation)
                .FirstOrDefaultAsync(p => p.IdProj == id);
        }

        public async Task<string> AddProjectAsync(Projects project)
        {

            var departmentExists = await _context.Departments.AnyAsync(d => d.IdDept == project.IdDept);
            if (!departmentExists)
            {
                return "Department ID not found";
            }


            var locationExists = await _context.Locations.AnyAsync(l => l.IdLocation == project.IdLocation);
            if (!locationExists)
            {
                return "Location ID not found";
            }


            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return "Project added successfully";
        }

        public async Task UpdateProjectAsync(Projects project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }
    }

}
