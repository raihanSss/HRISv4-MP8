using HRIS.Domain.Interfaces;
using HRIS.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRIS.API.Controllers
{
    
    [Route("api/dept")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,HR Manager,Department Manager,Employee Supervisor")]
        public async Task<ActionResult<IEnumerable<Departments>>> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDeptAsync();
            return Ok(departments);
        }

        
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,HR Manager,Department Manager,Employee Supervisor,Employee")]
        public async Task<ActionResult<Departments>> GetDepartmentById(int id)
        {
            var department = await _departmentService.GetDeptByIdAsync(id);

            if (department == null)
            {
                return NotFound("Department not found");
            }

            return Ok(department);
        }

        
        [HttpPost]
        [Authorize(Roles = "Administrator,Department Manager")]
        public async Task<ActionResult<string>> AddDepartment([FromBody] Departments department)
        {
            if (department == null || department.IdLocation == null)
            {
                return BadRequest("Location ID is required to create a department.");
            }

            var result = await _departmentService.AddDeptAsync(department);

            if (result == "Department added successfully")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,Department Manager")]
        public async Task<ActionResult<string>> UpdateDepartment(int id, [FromBody] Departments department)
        {
            if (id != department.IdDept)
            {
                return BadRequest("Department ID mismatch");
            }

            var existingDepartment = await _departmentService.GetDeptByIdAsync(id);
            if (existingDepartment == null)
            {
                return NotFound("Department not found");
            }

            var result = await _departmentService.UpdateDeptAsync(department);

            if (result == "Department updated successfully")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator,Department Manager")]
        public async Task<ActionResult<string>> DeleteDepartment(int id)
        {
            var existingDepartment = await _departmentService.GetDeptByIdAsync(id);
            if (existingDepartment == null)
            {
                return NotFound("Department not found");
            }

            var result = await _departmentService.DeleteDeptAsync(id);

            if (result == "Department deleted successfully")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
