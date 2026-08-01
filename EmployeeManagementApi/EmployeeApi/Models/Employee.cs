using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } ="";
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Phone]
        public string Phone { get; set; } = string.Empty;
        public decimal Salary { get; set; }

        public DateTime JoinDate { get; set; }
        public bool IsActive { get; set; }
        public string? ImageUrl {  get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; } = null!;

        public ICollection<Designation> Designations { get; set; } = new List<Designation>();
    }
}
