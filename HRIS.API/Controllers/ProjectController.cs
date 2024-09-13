using HRIS.Domain.Interfaces;
using HRIS.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRIS.API.Controllers
{
    [Route("api/project")]
    [ApiController]
    
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,Department Manager,Employee Supervisor")]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,Department Manager,Employee Supervisor,Employee")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);

            if (project == null)
            {
                return NotFound("Project not found");
            }

            return Ok(project);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Department Manager")]
        public async Task<IActionResult> AddProject([FromBody] Projects project)
        {
            if (project == null)
            {
                return BadRequest("Invalid project data");
            }

            Console.WriteLine($"IdDept: {project.IdDept}, IdLocation: {project.IdLocation}");

            var result = await _projectService.AddProjectAsync(project);
            if (result == "Project added successfully")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result); 
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,Department Manager")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] Projects project)
        {
            if (id != project.IdProj)
            {
                return BadRequest("Project ID mismatch");
            }

            var result = await _projectService.UpdateProjectAsync(project);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator,Department Manager")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var result = await _projectService.DeleteProjectAsync(id);
            if (result == "Project not found")
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
