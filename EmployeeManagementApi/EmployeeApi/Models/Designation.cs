namespace EmployeeApi.Models
{
    public class Designation
    {
        public int DesignationId { get; set; }

        public string? DesignationName { get; set; }

        public int Grade { get; set; }

        public decimal BasicSalary { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
