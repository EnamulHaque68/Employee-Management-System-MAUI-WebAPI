using EmployeeApi.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.DTOs
{
    public class DesignationDtos
    {
        public int DesignationId { get; set; }
        public string? DesignationName { get; set; }
        public int Grade { get; set; }
        public decimal BasicSalary { get; set; }
    }

    public class EmployeeDtos
    {
        public string EmployeeName { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^(?:\+?880|0)1[3-9]\d{8}$",
            ErrorMessage = "Enter a valid Bangladeshi mobile number, e.g. 01521550535 or +8801521550535.")]
        public string Phone { get; set; } = string.Empty;

        public decimal Salary { get; set; }
        public DateTime JoinDate { get; set; }
        public bool IsActive { get; set; }
        public IFormFile? Profile { get; set; }
        public int DepartmentId { get; set; }
        public string? DesignationJson { get; set; } = "[]";
    }
}