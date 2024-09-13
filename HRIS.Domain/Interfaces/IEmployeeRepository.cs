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
    public interface IEmployeeRepository
    {
        Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
        Task<IEnumerable<object>> GetAllEmployeesAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "Name",
            bool sortDesc = false,
            string filterBy = "",
            string filterValue = "");
        Task AddEmployeeAsync(Employees employee);
        Task UpdateEmployeeAsync(Employees employee);
        Task DeleteEmployeeAsync(int id);

        Task DeactivateEmployeeAsync(int id, string reason);

        Task<bool> AddLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByEmployeeIdAsync(int employeeId);

        Task<List<Employees>> GetAllEmployeesWithoutAppUserAsync();

        Task<Employees> GetEmployeeByEmailOrSsnAsync(string email, string ssn);

        Task<List<Employees>> GetEmployeesByDepartmentAsync(int departmentId, int pageNumber, int pageSize);

        Task<string> GetDepartmentNameByIdAsync(int departmentId);
        Task<string> GetTotalEmployeesByDepartmentAsync(int departmentId);

        Task<List<LeaveReport>> GetTotalLeavesByTypeAsync(DateOnly startDate, DateOnly endDate);

        
        Task<ProjectReport> GetProjectReportAsync(int projectId);

        Task<Dictionary<string, string>> GetEmployeeDistributionByDepartmentAsync();

       
        Task<List<EmployeePerformance>> GetTop5EmployeesByPerformanceAsync();

        Task<Dictionary<string, decimal>> GetAverageSalaryByDepartmentAsync();

    }
}
