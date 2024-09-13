using HRIS.Domain.Dtos;
using HRIS.Domain.Models;
using HRIS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
        Task<IEnumerable<object>> GetAllEmployeesAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "Name",
            bool sortDesc = false,
            string filterBy = "",
            string filterValue = "");
        Task <string> AddEmployeeAsync(Employees employee);
        Task<string> UpdateEmployeeAsync(Employees employee);
        Task<string> DeleteEmployeeAsync(int id);

        Task<string> DeactivateEmployeeAsync(int id, string reason);

        Task<bool> CreateLeaveRequestAsync(LeaveReqDto leaveRequestDto);
        Task<IEnumerable<LeaveReqDto>> GetLeaveRequestsByEmployeeIdAsync(int employeeId);

        Task<byte[]> GenerateEmployeeReportPdfAsync(int departmentId, int pageNumber);

        Task<byte[]> GenerateLeaveReportPdfAsync(DateOnly startDate, DateOnly endDate);
        Task<byte[]> GenerateProjectReportPdfAsync(int projectId);




        Task<IEnumerable<LeaveReport>> GetTotalLeavesByTypeAsync(DateOnly startDate, DateOnly endDate);

        
        Task<ProjectReport> GetProjectReportAsync(int projectId);

       
        Task<IEnumerable<Employees>> GetEmployeesByDepartmentAsync(int departmentId, int pageNumber, int pageSize);
    }
}
