using EmployeeAttendanceTracker.Data;
using EmployeeAttendanceTracker.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAttendanceTracker.Presentation
{
    public static class DataSeeder
    {
        public static void Seed(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (context == null || !context.Database.IsInMemory()) return;

                context.Database.EnsureCreated();

                if (context.Departments.Any()) return; // DB has been seeded

                var departments = new Department[]
                {
                    new Department{DepartmentName="Technology", DepartmentCode="TECH", Location="Building A"},
                    new Department{DepartmentName="Human Resources", DepartmentCode="HRMG", Location="Building B"},
                };
                context.Departments.AddRange(departments);
                context.SaveChanges();

                var employees = new Employee[]
                {
                    new Employee{FullName="John Wick Baba Yaga", Email="john.wick@example.com", DepartmentId=1},
                    new Employee{FullName="Jane Mary Anne Smith", Email="jane.smith@example.com", DepartmentId=2},
                    new Employee{FullName="Peter Benjamin Franklin Jones", Email="peter.jones@example.com", DepartmentId=1},
                };
                context.Employees.AddRange(employees);
                context.SaveChanges();
            }
        }
    }
}