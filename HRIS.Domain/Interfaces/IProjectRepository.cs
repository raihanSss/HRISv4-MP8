using HRIS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Projects>> GetAllProjectsAsync();
        Task<Projects?> GetProjectByIdAsync(int id);
        Task<string> AddProjectAsync(Projects project);
        Task UpdateProjectAsync(Projects project);
        Task DeleteProjectAsync(int id);
    }
}
