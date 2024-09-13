using HRIS.Domain.Interfaces;
using HRIS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<Departments> GetDeptByIdAsync(int id)
        {
            return await _departmentRepository.GetDeptByIdAsync(id);
        }

        public async Task<IEnumerable<Departments>> GetAllDeptAsync()
        {
            return await _departmentRepository.GetAllDeptAsync();
        }

        public async Task<string> AddDeptAsync(Departments department)
        {
            try
            {
                if (department.IdLocation == null)
                {
                    return "Location ID is required when creating a department.";
                }

                await _departmentRepository.AddDeptAsync(department);
                return "Department added successfully";
            }
            catch (Exception ex)
            {
                
                return $"An error occurred: {ex.Message}";
            }
        }

        public async Task<string> UpdateDeptAsync(Departments department)
        {
            try
            {
                await _departmentRepository.UpdateDeptAsync(department);
                return "Department updated successfully";
            }
            catch (Exception ex)
            {
                
                return $"An error occurred: {ex.Message}";
            }
        }

        public async Task<string> DeleteDeptAsync(int id)
        {
            try
            {
                await _departmentRepository.DeleteDeptAsync(id);
                return "Department deleted successfully";
            }
            catch (Exception ex)
            {
                
                return $"An error occurred: {ex.Message}";
            }
        }
    }
}
