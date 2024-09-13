using HRIS.Domain.Dtos;
using HRIS.Domain.Interfaces;
using HRIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Infrastructure.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly hrisDbContext _context;

        public EmployeeRepository(hrisDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllEmployeesAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string sortBy = "NameEmp",
        bool sortDesc = false,
        string filterBy = "",
        string filterValue = "")
        {
            var query = _context.Employees.AsQueryable();

            
            if (!string.IsNullOrEmpty(filterBy) && !string.IsNullOrEmpty(filterValue))
            {
                query = filterBy switch
                {
                    "NameEmp" => query.Where(e => e.NameEmp.Contains(filterValue)),
                    "JobPosition" => query.Where(e => e.JobPosition.Contains(filterValue)),
                    _ => query
                };
            }

            // Sorting
            query = sortDesc ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
                             : query.OrderBy(e => EF.Property<object>(e, sortBy));

            // Pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            
            var employees = await query
                .Select(e => new
                {
                    e.NameEmp,
                    e.Ssn,
                    DepartmentName = e.IdDeptNavigation.NameDept, 
                    e.JobPosition,
                    e.Level,
                    e.Type,
                    e.Lastupdate
                })
                .ToListAsync();

            return employees;
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employees
                .Where(e => e.IdEmp == id)
                .Select(e => new EmployeeDto
                {
                    NameEmp = e.NameEmp,
                    Address = e.Address,
                    Phone = e.Phone,
                    Email = e.Email,
                    JobPosition = e.JobPosition,
                    Type = e.Type,
                    Reason = e.Reason,
                    Status = e.Status
                })
                .FirstOrDefaultAsync();

            return employee;
        }


        public async Task AddEmployeeAsync(Employees employee)
        {
     
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AddLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            try
            {
                await _context.LeaveRequests.AddAsync(leaveRequest);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByEmployeeIdAsync(int employeeId)
        {
            return await _context.LeaveRequests
                .Where(lr => lr.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task UpdateEmployeeAsync(Employees employee)
        {
            employee.Lastupdate = DateTime.UtcNow;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeactivateEmployeeAsync(int id, string reason)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                throw new KeyNotFoundException("Employee tidak ditemukan");
            }

            employee.Status = "Not Active";
            employee.Reason = reason;
            employee.Lastupdate = DateTime.UtcNow;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Employees>> GetAllEmployeesWithoutAppUserAsync()
        {
            return await _context.Employees
                .Where(e => !_context.Users.Any(u => u.EmployeeId == e.IdEmp)) 
                .ToListAsync();
        }

        public async Task<Employees> GetEmployeeByEmailOrSsnAsync(string email, string ssn)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == email || e.Ssn == ssn);
        }


        public async Task<List<Employees>> GetEmployeesByDepartmentAsync(int departmentId, int pageNumber, int pageSize)
        {
            return await _context.Employees
                .Where(e => e.IdDept == departmentId)
                .OrderBy(e => e.IdEmp) 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<string> GetTotalEmployeesByDepartmentAsync(int departmentId)
        {
            var totalEmployees = await _context.Employees
                .Where(e => e.IdDept == departmentId)
                .CountAsync();

            return totalEmployees.ToString(); 
        }

        public async Task<string> GetDepartmentNameByIdAsync(int departmentId)
        {
            var department = await _context.Departments
                .Where(d => d.IdDept == departmentId)
                .Select(d => d.NameDept)
                .FirstOrDefaultAsync();

            return department ?? "Unknown Department"; 
        }


        public async Task<List<LeaveReport>> GetTotalLeavesByTypeAsync(DateOnly startDate, DateOnly endDate)
        {
            return await _context.LeaveRequests
                .Include(lr => lr.Processes)
                .Where(lr => lr.StartDate >= startDate && lr.EndDate <= endDate &&
                             lr.Processes.Any(p => p.Status == "Approved")) 
                .GroupBy(lr => lr.LeaveType)
                .Select(group => new LeaveReport
                {
                    LeaveType = group.Key,
                    TotalLeaves = group.Count()
                })
                .ToListAsync();
        }

        public async Task<ProjectReport> GetProjectReportAsync(int projectId)
        {
            var projectLogs = await _context.Projemp
                .Include(pe => pe.IdProjNavigation) 
                .Where(pe => pe.IdProjNavigation.IdProj == projectId)
                .ToListAsync();

            var totalHoursLogged = projectLogs.Sum(pl => pl.Hoursperweek); 
            var numberOfEmployees = projectLogs.Select(pl => pl.IdEmp).Distinct().Count();
            var averageHoursPerEmployee = numberOfEmployees > 0 ? (double)totalHoursLogged / numberOfEmployees : 0;

            return new ProjectReport
            {
                ProjectId = projectId,
                ProjectName = projectLogs.FirstOrDefault()?.IdProjNavigation.Nameproj ?? "Unknown Project",
                TotalHoursLogged = totalHoursLogged,
                NumberOfEmployeesInvolved = numberOfEmployees,
                AverageHoursPerEmployee = averageHoursPerEmployee
            };
        }


        public async Task<Dictionary<string, string>> GetEmployeeDistributionByDepartmentAsync()
        {
            var totalEmployees = await _context.Employees.CountAsync();

            var departmentDistribution = await _context.Employees
                .Where(e => e.IdDeptNavigation.NameDept != null) 
                .GroupBy(e => e.IdDeptNavigation.NameDept)
                .Select(group => new
                {
                    DepartmentName = group.Key ?? "Unknown", 
                    Count = group.Count()
                })
                .ToListAsync();

            
            return departmentDistribution
                .ToDictionary(
                    x => x.DepartmentName,
                    x => $"{(int)Math.Round((double)x.Count / totalEmployees * 100)}%"); 
        }


        public async Task<List<EmployeePerformance>> GetTop5EmployeesByPerformanceAsync()
        {
            var performanceData = await _context.Projemp
                .GroupBy(p => p.IdEmpNavigation)
                .Select(group => new EmployeePerformance
                {
                    EmployeeId = group.Key.IdEmp,
                    EmployeeName = group.Key.NameEmp,
                    TotalHoursWorked = group.Sum(g => g.Hoursperweek)
                })
                .OrderByDescending(p => p.TotalHoursWorked)
                .Take(5)
                .ToListAsync();

            return performanceData;
        }

        public async Task<Dictionary<string, decimal>> GetAverageSalaryByDepartmentAsync()
        {
            var salaryDistribution = await _context.Employees
                .Where(e => e.Salary.HasValue && e.IdDeptNavigation.NameDept != null) 
                .GroupBy(e => e.IdDeptNavigation.NameDept)
                .Select(group => new
                {
                    DepartmentName = group.Key ?? "Unknown",
                    AverageSalary = group.Average(e => e.Salary ?? 0)
                })
                .ToListAsync();

            
            return salaryDistribution
                .ToDictionary(
                    x => x.DepartmentName,
                    x => Math.Round(x.AverageSalary, 2));
        }

    }
}
