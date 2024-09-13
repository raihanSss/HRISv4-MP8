using HRIS.Domain.Dtos;
using HRIS.Domain.Interfaces;
using HRIS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; 

namespace HRIS.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IProcessRepository _processRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardService(IEmployeeRepository employeeRepository, IProcessRepository processRepository, IHttpContextAccessor httpContextAccessor)
        {
            _employeeRepository = employeeRepository;
            _processRepository = processRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DashboardDto> GetDashboardDataAsync(int actorId)
        {
            var employeeDistribution = await _employeeRepository.GetEmployeeDistributionByDepartmentAsync();
            var topEmployees = await _employeeRepository.GetTop5EmployeesByPerformanceAsync();
            var avgSalary = await _employeeRepository.GetAverageSalaryByDepartmentAsync();


            var userRoles = _httpContextAccessor.HttpContext?.User?.Claims
         .Where(c => c.Type == ClaimTypes.Role)
         .Select(c => c.Value)
         .ToList();

            bool isHRManager = userRoles.Contains("HR Manager");
            bool isEmployeeSupervisor = userRoles.Contains("Employee Supervisor");


            IEnumerable<Process> processes = Enumerable.Empty<Process>();

            // Cek role dan ambil proses berdasarkan role yang ditemukan
            if (isHRManager)
            {
                processes = await _processRepository.GetProcessBasedOnRoleAsync("HR Manager");
            }
            else if (isEmployeeSupervisor)
            {
                processes = await _processRepository.GetProcessBasedOnRoleAsync("Employee Supervisor");
            }


            var processUsersDTO = processes.Select(p => new ProcessDetailDTO
            {
                ProcessId = p.ProcessId,
                WorkflowName = p.Workflow.WorkflowName,
                Requester = p.LeaveRequest.Employee.NameEmp,
                RequestDate = p.RequestDate,
                Status = p.Status,
                CurrentStep = p.CurrentStep.StepName
            }).ToList();

            return new DashboardDto
            {
                EmployeeDistributionByDepartment = employeeDistribution,
                TopEmployeesByPerformance = topEmployees,
                GetAverageSalaryByDepartmentAsync = avgSalary,
                ProcessDetails = processUsersDTO
            };
        }
    }
}
