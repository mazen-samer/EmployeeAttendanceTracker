using System.Text.RegularExpressions;
using EmployeeAttendanceTracker.Data.Models;
using EmployeeAttendanceTracker.Data.Repositories;

namespace EmployeeAttendanceTracker.Business.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<(bool Success, string ErrorMessage)> CreateDepartmentAsync(Department department)
        {
            // Business Rule: Validate department code format [cite: 16]
            if (!Regex.IsMatch(department.DepartmentCode, @"^[A-Z]{4}$"))
            {
                return (false, "Department Code must be exactly 4 uppercase alphabetic characters.");
            }

            // Business Rule: Prevent duplicate department names or codes [cite: 18]
            if (await _departmentRepository.DepartmentExistsAsync(department.DepartmentName, department.DepartmentCode))
            {
                return (false, "A department with the same name or code already exists.");
            }

            await _departmentRepository.AddDepartmentAsync(department);
            return (true, string.Empty);
        }

        public async Task<(bool Success, string ErrorMessage)> DeleteDepartmentAsync(int id)
        {
            var department = await _departmentRepository.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return (false, "Department not found.");
            }

            await _departmentRepository.DeleteDepartmentAsync(department);
            return (true, string.Empty);
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await _departmentRepository.GetAllDepartmentsAsync();
        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            return await _departmentRepository.GetDepartmentByIdAsync(id);
        }

        public async Task<(bool Success, string ErrorMessage)> UpdateDepartmentAsync(Department department)
        {
            // Business Rule: Validate department code format [cite: 16]
            if (!Regex.IsMatch(department.DepartmentCode, @"^[A-Z]{4}$"))
            {
                return (false, "Department Code must be exactly 4 uppercase alphabetic characters.");
            }

            // Business Rule: Prevent duplicate department names or codes, excluding the current department [cite: 18]
            if (await _departmentRepository.DepartmentExistsAsync(department.DepartmentName, department.DepartmentCode, department.Id))
            {
                return (false, "Another department with the same name or code already exists.");
            }

            await _departmentRepository.UpdateDepartmentAsync(department);
            return (true, string.Empty);
        }
    }
}