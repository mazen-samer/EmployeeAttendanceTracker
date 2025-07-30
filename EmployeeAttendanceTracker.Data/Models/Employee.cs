using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeAttendanceTracker.Data.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeCode { get; set; } // System generated, cannot be edited [cite: 26]

        [Required]
        [RegularExpression(@"^[A-Za-z]{2,}\s[A-Za-z]{2,}\s[A-Za-z]{2,}\s[A-Za-z]{2,}$",
            ErrorMessage = "Full Name must consist of four names, each with at least two letters.")]
        public string FullName { get; set; } // Validation for four names will be in the Business Layer [cite: 27]

        [Required]
        [EmailAddress]
        public string Email { get; set; } // Must be unique [cite: 28]

        // Foreign key for Department
        [Required(ErrorMessage = "Please select a department.")]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; } // Navigation property [cite: 29]

        // Navigation property for related Attendance records
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}