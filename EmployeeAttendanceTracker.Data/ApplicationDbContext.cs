using EmployeeAttendanceTracker.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAttendanceTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Department: Configure unique constraints for DepartmentName and DepartmentCode
            modelBuilder.Entity<Department>()
                .HasIndex(d => d.DepartmentName)
                .IsUnique();

            modelBuilder.Entity<Department>()
                .HasIndex(d => d.DepartmentCode)
                .IsUnique();

            // Employee: Configure unique constraint for Email
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            // Attendance: Prevent an employee from having more than one record on the same day
            modelBuilder.Entity<Attendance>()
                .HasIndex(a => new { a.EmployeeId, a.Date })
                .IsUnique();
        }
    }
}