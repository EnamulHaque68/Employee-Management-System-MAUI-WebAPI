using EmployeeApi.Data;
using EmployeeApi.DTOs;
using EmployeeApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EmployeeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDBContext db;
        private readonly IWebHostEnvironment env;

        public EmployeeController(AppDBContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }

        [HttpGet("Departments")]
        public async Task<IActionResult> GetDepartments() =>
            Ok(await db.Departments.ToListAsync());

        [HttpGet("Designations")]
        public async Task<IActionResult> GetDesignations() =>
            Ok(await db.Designations.ToListAsync());

        [HttpGet]
        public async Task<IActionResult> GetEmployee()
        {
            var employees = await db.Employees
                .Include(x => x.Department)
                .Include(x => x.Designations)
                .ToListAsync();

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await db.Employees
                .Include(x => x.Department)
                .Include(x => x.Designations)
                .FirstOrDefaultAsync(x => x.EmployeeId == id);

            if (employee == null) return NotFound();
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromForm] EmployeeDtos dto)
        {
            if (!await db.Departments.AnyAsync(d => d.DepartmentId == dto.DepartmentId))
                return BadRequest(new { message = $"DepartmentId {dto.DepartmentId} does not exist." });
            if (!TryParseDesignations(dto.DesignationJson, out var designationInputs, out var parseError))
                return BadRequest(new { message = parseError });

            var employee = new Employee
            {
                EmployeeName = dto.EmployeeName,
                Email = dto.Email,
                Phone = dto.Phone,
                Salary = dto.Salary,
                JoinDate = dto.JoinDate,
                IsActive = dto.IsActive,
                DepartmentId = dto.DepartmentId,
                ImageUrl = await SaveImage(dto.Profile)
            };

            foreach (var s in designationInputs)
            {
                employee.Designations.Add(new Designation
                {
                    DesignationName = s.DesignationName,
                    Grade = s.Grade,
                    BasicSalary = s.BasicSalary
                });
            }

            db.Employees.Add(employee);
            await db.SaveChangesAsync();
            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeById(int id, [FromForm] EmployeeDtos dto)
        {
            var employee = await db.Employees
                .Include(x => x.Designations)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null) return NotFound();

            if (!await db.Departments.AnyAsync(d => d.DepartmentId == dto.DepartmentId))
                return BadRequest(new { message = $"DepartmentId {dto.DepartmentId} does not exist." });

            if (!TryParseDesignations(dto.DesignationJson, out var designationInputs, out var parseError))
                return BadRequest(new { message = parseError });

            employee.EmployeeName = dto.EmployeeName;
            employee.Email = dto.Email;
            employee.Phone = dto.Phone;
            employee.Salary = dto.Salary;
            employee.JoinDate = dto.JoinDate;
            employee.IsActive = dto.IsActive;
            employee.DepartmentId = dto.DepartmentId;

            if (dto.Profile != null)
                employee.ImageUrl = await SaveImage(dto.Profile);

            employee.Designations.Clear();
            foreach (var s in designationInputs)
            {
                employee.Designations.Add(new Designation
                {
                    DesignationName = s.DesignationName,
                    Grade = s.Grade,
                    BasicSalary = s.BasicSalary
                });
            }

            await db.SaveChangesAsync();
            return Ok(employee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeById(int id)
        {
            var employee = await db.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            db.Employees.Remove(employee);
            await db.SaveChangesAsync();
            return Ok(new { message = "Deleted" });
        }
        private static bool TryParseDesignations(
            string? json,
            out List<DesignationDtos> designations,
            out string errorMessage)
        {
            designations = new List<DesignationDtos>();
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(json) || json.Trim() == "[]")
                return true;

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                designations = JsonSerializer.Deserialize<List<DesignationDtos>>(json, options)
                               ?? new List<DesignationDtos>();
                return true;
            }
            catch (JsonException)
            {
                errorMessage =
                    "DesignationJson is not a valid JSON array. Send it WITHOUT surrounding quotes, e.g. " +
                    "[{\"designationName\":\"Operator\",\"grade\":2,\"basicSalary\":15000}]";
                return false;
            }
        }

        private async Task<string> SaveImage(IFormFile? file)
        {
            if (file == null) return "";

            var root = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var folder = Path.Combine(root, "images");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = Path.Combine(folder, fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return "/images/" + fileName;
        }
    }
}