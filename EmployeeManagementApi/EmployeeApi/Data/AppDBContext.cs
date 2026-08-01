using EmployeeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Data
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext>options):base(options) 
        {
                
        }

        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Designation> Designations { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(

                new Department
                {
                    DepartmentId = 1,
                    DepartmentName = "Human Resources (HR)",
                    Description = "Handles employee recruitment, training, attendance, payroll, and employee welfare."
                },

                new Department
                {
                    DepartmentId = 2,
                    DepartmentName = "Information Technology (IT)",
                    Description = "Manages software development, hardware maintenance, networks, and technical support."
                },

                new Department
                {
                    DepartmentId = 3,
                    DepartmentName = "Accounts & Finance",
                    Description = "Responsible for accounting, budgeting, financial reporting, and payment management."
                },

                new Department
                {
                    DepartmentId = 4,
                    DepartmentName = "Sales",
                    Description = "Manages customer relationships, quotations, sales orders, and revenue generation."
                },

                new Department
                {
                    DepartmentId = 5,
                    DepartmentName = "Marketing",
                    Description = "Promotes products, manages branding, advertising, and market research."
                },

                new Department
                {
                    DepartmentId = 6,
                    DepartmentName = "Production",
                    Description = "Responsible for manufacturing products and managing production operations."
                },

                new Department
                {
                    DepartmentId = 7,
                    DepartmentName = "Purchase",
                    Description = "Purchases raw materials, office supplies, and manages supplier relationships."
                },

                new Department
                {
                    DepartmentId = 8,
                    DepartmentName = "Inventory / Store",
                    Description = "Maintains stock, warehouse operations, and inventory transactions."
                },

                new Department
                {
                    DepartmentId = 9,
                    DepartmentName = "Quality Assurance (QA)",
                    Description = "Ensures product quality through inspection, testing, and quality control processes."
                },

                new Department
                {
                    DepartmentId = 10,
                    DepartmentName = "Administration",
                    Description = "Oversees office administration, documentation, assets, and facility management."
                }
            );
        }
    }
}
