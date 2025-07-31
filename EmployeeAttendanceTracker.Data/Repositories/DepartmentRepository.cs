using EmployeeAttendanceTracker.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAttendanceTracker.Data.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddDepartmentAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartmentAsync(Department department)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DepartmentExistsAsync(string name, string code, int? excludeId = null)
        {
            var query = _context.Departments.AsQueryable();

            if (excludeId.HasValue)
            {
                query = query.Where(d => d.Id != excludeId.Value);
            }

            return await query.AnyAsync(d => d.DepartmentName == name || d.DepartmentCode == code);
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments.Include(d => d.Employees).ToListAsync();
        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }
    }
}