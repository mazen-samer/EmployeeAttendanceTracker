using System.ComponentModel.DataAnnotations;

namespace EmployeeAttendanceTracker.Data.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string DepartmentName { get; set; }

        [Required]
        [StringLength(4)]
        [RegularExpression(@"^[A-Z]{4}$", ErrorMessage = "Department Code must be exactly 4 uppercase letters.")]
        public string DepartmentCode { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}