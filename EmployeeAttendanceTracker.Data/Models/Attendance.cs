namespace EmployeeAttendanceTracker.Data.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; } // True for "Present", False for "Absent"

        // Foreign key for Employee
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } // Navigation property
    }
}