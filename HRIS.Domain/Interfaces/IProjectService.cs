using HRIS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Projects>> GetAllProjectsAsync();
        Task<Projects?> GetProjectByIdAsync(int id);
        Task<string> AddProjectAsync(Projects project);
        Task<string> UpdateProjectAsync(Projects project);
        Task<string> DeleteProjectAsync(int id);
    }
}
