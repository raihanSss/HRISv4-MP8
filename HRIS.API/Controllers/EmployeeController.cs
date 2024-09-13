using HRIS.Application.Services;
using HRIS.Domain.Dtos;
using HRIS.Domain.Interfaces;
using HRIS.Domain.Models;
using HRIS.Infrastructure;
using HRIS.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRIS.API.Controllers
{
    [Route("api/employee")]
    [ApiController]

    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAuthService _authService;
        private readonly IWorkflowService _workflowService;

        public EmployeeController(IEmployeeService employeeService, IAuthService authService, IWorkflowService workflowService)
        {
            _employeeService = employeeService;
            _authService = authService;
            _workflowService = workflowService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,HR Manager,Department Manager,Employee Supervisor")]
        public async Task<IActionResult> GetAllEmployees(
        int pageNumber = 1,
        int pageSize = 10,
        string sortBy = "NameEmp",
        bool sortDesc = false,
        string filterBy = "",
        string filterValue = "")
        {
            var employees = await _employeeService.GetAllEmployeesAsync(pageNumber, pageSize, sortBy, sortDesc, filterBy, filterValue);

            return Ok(employees);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,HR Manager,Department Manager,Employee Supervisor,Employee")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost("AddUserToExistingEmployee")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddUserToExistingEmployee([FromBody] AddUserToExistingEmployeeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var employee = await _employeeService.GetEmployeeByIdAsync(model.EmployeeId);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }


            var result = await _authService.RegisterAsync(new RegisterEmployeeModel
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                Role = model.Role,

                NameEmp = employee.NameEmp,
                Ssn = employee.Ssn,
                Dob = employee.Dob,
                Address = employee.Address,
                Phone = employee.Phone,
                JobPosition = employee.JobPosition,
                Level = employee.Level,
                Type = employee.Type,
                Status = employee.Status,
                Reason = employee.Reason
            });

            if (result.Status == "Success")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,HR Manager")]
        public async Task<IActionResult> PutEmployee(int id, Employees employee)
        {
            if (id != employee.IdEmp)
            {
                return BadRequest();
            }

            await _employeeService.UpdateEmployeeAsync(employee);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator,HR Manager")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            await _employeeService.DeleteEmployeeAsync(id);
            return NoContent();
        }

        [HttpPut("deactivate/{id}")]
        [Authorize(Roles = "Administrator,HR Manager")]
        public async Task<IActionResult> DeactivateEmployee(int id, [FromBody] string reason)
        {
            var result = await _employeeService.DeactivateEmployeeAsync(id, reason);

            if (result == "Employee deactivated successfully")
            {
                return Ok(result);
            }
            else if (result == "Employee not found")
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("createleave")]
        
        public async Task<IActionResult> CreateLeaveRequest([FromBody] LeaveReqDto leaveRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _employeeService.CreateLeaveRequestAsync(leaveRequestDto);

            if (result)
            {
                return Ok(new { message = "Leave request created successfully." });
            }
            else
            {
                return StatusCode(500, "An error occurred while creating the leave request.");
            }
        }


        [HttpGet("employeeLeave/{employeeId}")]
        public async Task<IActionResult> GetLeaveRequestsByEmployeeId(int employeeId)
        {
            var leaveRequests = await _employeeService.GetLeaveRequestsByEmployeeIdAsync(employeeId);

            if (leaveRequests == null || !leaveRequests.Any())
            {
                return NotFound(new { message = "No leave requests found for this employee." });
            }

            return Ok(leaveRequests);
        }

        [HttpPost("employeeLeave/decision/{leaveRequestId}")]
        
        public async Task<IActionResult> ProcessLeaveRequest(int leaveRequestId, [FromBody] ProcessLeaveRequestDto dto)
        {
            
            if (dto == null || dto.UserId <= 0 || string.IsNullOrEmpty(dto.Comments) || string.IsNullOrEmpty(dto.Status))
            {
                return BadRequest("Invalid data.");
            }

           
            if (!dto.Status.Equals("approve", StringComparison.OrdinalIgnoreCase) &&
                !dto.Status.Equals("reject", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Status must be either 'approve' or 'reject'.");
            }

            try
            {
                
                var result = await _workflowService.ProcessLeaveRequestAsync(leaveRequestId,dto.UserId, dto.Comments, dto.Status);

                if (result)
                {
                    return Ok("Leave request processed.");
                }
                else
                {
                    return NotFound("Leave request not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        public class ProcessLeaveRequestDto
        {
            public int UserId { get; set; }
            public string Comments { get; set; }
            public string Status { get; set; } // Should be either "approve" or "reject"
        }



        [HttpGet("leavereports")]
        public async Task<IActionResult> GetLeaveReports([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
        {
            var leaveReports = await _employeeService.GetTotalLeavesByTypeAsync(startDate, endDate);
            return Ok(leaveReports);
        }

        
        [HttpGet("projectreport/{projectId}")]
        public async Task<IActionResult> GetProjectReport(int projectId)
        {
            var projectReport = await _employeeService.GetProjectReportAsync(projectId);
            return Ok(projectReport);
        }

        
        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployeesByDepartment([FromQuery] int departmentId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var employeeList = await _employeeService.GetEmployeesByDepartmentAsync(departmentId, pageNumber, pageSize);
            return Ok(employeeList);
        }

    }
}

