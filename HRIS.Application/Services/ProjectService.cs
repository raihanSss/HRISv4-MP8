using HRIS.Domain.Interfaces;
using HRIS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Projects>> GetAllProjectsAsync()
        {
            return await _projectRepository.GetAllProjectsAsync();
        }

        public async Task<Projects?> GetProjectByIdAsync(int id)
        {
            return await _projectRepository.GetProjectByIdAsync(id);
        }

        public async Task<string> AddProjectAsync(Projects project)
        {
            return await _projectRepository.AddProjectAsync(project);
        }

        public async Task<string> UpdateProjectAsync(Projects project)
        {
            await _projectRepository.UpdateProjectAsync(project);
            return "Project updated successfully";
        }

        public async Task<string> DeleteProjectAsync(int id)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);
            if (project == null)
            {
                return "Project not found";
            }

            await _projectRepository.DeleteProjectAsync(id);
            return "Project deleted successfully";
        }
    }
}
