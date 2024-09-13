using HRIS.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRIS.API.Controllers
{
    
    [Route("api/dependent")]
    [ApiController]
    public class DependentsController : ControllerBase
    {
        private readonly IDependentService _dependentService;

        public DependentsController(IDependentService dependentService)
        {
            _dependentService = dependentService;
        }

        
        [HttpGet]
        [Authorize(Roles = "Administrator,HR Manager,Employee")]
        public async Task<IActionResult> GetAllDependents()
        {
            var dependents = await _dependentService.GetAllDependentsAsync();
            return Ok(dependents);
        }

        
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,HR Manager,Employee")]
        public async Task<IActionResult> GetDependentById(int id)
        {
            var dependent = await _dependentService.GetDependentByIdAsync(id);

            if (dependent == null)
            {
                return NotFound("Dependent not found");
            }

            return Ok(dependent);
        }
    }
}