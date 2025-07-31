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

                // Check if the database has already been seeded.
                if (context.Departments.Any()) return;

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

                // --- NEW: Seed Attendance Data ---
                var attendances = new Attendance[]
                {
                    // Employee 1 (John Wick)
                    new Attendance{EmployeeId=1, Date=DateTime.Today.AddDays(-1), IsPresent=true},
                    new Attendance{EmployeeId=1, Date=DateTime.Today.AddDays(-2), IsPresent=true},
                    new Attendance{EmployeeId=1, Date=DateTime.Today.AddDays(-3), IsPresent=false}, // Absent

                    // Employee 2 (Jane Smith)
                    new Attendance{EmployeeId=2, Date=DateTime.Today.AddDays(-1), IsPresent=true},
                    new Attendance{EmployeeId=2, Date=DateTime.Today.AddDays(-2), IsPresent=true},
                    new Attendance{EmployeeId=2, Date=DateTime.Today.AddDays(-3), IsPresent=true},

                    // Employee 3 (Peter Jones)
                    new Attendance{EmployeeId=3, Date=DateTime.Today.AddDays(-1), IsPresent=false}, // Absent
                    new Attendance{EmployeeId=3, Date=DateTime.Today.AddDays(-2), IsPresent=true},
                };
                context.Attendances.AddRange(attendances);
                context.SaveChanges();
            }
        }
    }
}
