using NextEvdMaui.Models;
using NextEvdMaui.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

namespace NextEvdMaui.ViewModels
{
    public class ManageEmployeeViewModel : BaseViewModel
    {
        private readonly ApiService _apiService = new();
        private Department? _selectedDepartment;

        public Employee Employee { get; }

        public ObservableCollection<Department> Departments { get; } = new();

        public Department? SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                if (SetProperty(ref _selectedDepartment, value) &&
                    value is not null)
                {
                    Employee.DepartmentId = value.DepartmentId;
                    Employee.DepartmentName = value.DepartmentName;
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand AddDesignationCommand { get; }
        public ICommand RemoveDesignationCommand { get; }
        public ICommand PickImageCommand { get; }

        public event Action? SaveSucceeded;
        public event Action<string>? SaveFailed;

        public ManageEmployeeViewModel(Employee sourceEmployee)
        {
            Employee = CopyEmployee(sourceEmployee);

            if (Employee.EmployeeId == 0)
            {
                Employee.JoinDate = DateTime.Today;
                Employee.IsActive = true;

                if (Employee.Designations.Count == 0)
                {
                    Employee.Designations.Add(new Designation());
                }
            }

            SaveCommand = new Command(async () => await SaveAsync());
            AddDesignationCommand = new Command(AddDesignation);
            RemoveDesignationCommand = new Command<Designation>(RemoveDesignation);
            PickImageCommand = new Command(async () => await PickImageAsync());

            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            try
            {
                var departments = await _apiService.GetDepartmentsAsync();

                Departments.Clear();

                foreach (var department in departments)
                {
                    Departments.Add(department);
                }

                SelectedDepartment = Departments.FirstOrDefault(x =>
                    x.DepartmentId == Employee.DepartmentId)
                    ?? Departments.FirstOrDefault();
            }
            catch (Exception ex)
            {
                SaveFailed?.Invoke($"Dropdown load failed: {ex.Message}");
            }
        }

        private void AddDesignation()
        {
            Employee.Designations.Add(new Designation());
        }

        private void RemoveDesignation(Designation? designation)
        {
            if (designation is null)
            {
                return;
            }

            Employee.Designations.Remove(designation);
        }

        private async Task PickImageAsync()
        {
            try
            {
                var result = await MediaPicker.Default.PickPhotoAsync(
                    new MediaPickerOptions
                    {
                        Title = "Select employee photo"
                    });

                if (result is null)
                {
                    return;
                }

                var extension = Path.GetExtension(result.FileName);

                if (string.IsNullOrWhiteSpace(extension))
                {
                    extension = ".jpg";
                }

                var localPath = Path.Combine(
                    FileSystem.CacheDirectory,
                    $"employee_{Guid.NewGuid():N}{extension}");

                await using var input = await result.OpenReadAsync();
                await using var output = File.Create(localPath);
                await input.CopyToAsync(output);

                Employee.LocalImagePath = localPath;
            }
            catch (Exception ex)
            {
                SaveFailed?.Invoke($"Image select failed: {ex.Message}");
            }
        }

        private async Task SaveAsync()
        {
            var validationMessage = Validate();

            if (validationMessage is not null)
            {
                SaveFailed?.Invoke(validationMessage);
                return;
            }

            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;

                var result = await _apiService.SaveEmployeeAsync(Employee);

                if (result.Success)
                {
                    SaveSucceeded?.Invoke();
                }
                else
                {
                    SaveFailed?.Invoke(result.ErrorMessage);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private string? Validate()
        {
            if (SelectedDepartment is null || Employee.DepartmentId <= 0)
                return "Please select a Department.";

            if (string.IsNullOrWhiteSpace(Employee.EmployeeName))
                return "Please enter the Employee Name.";

            if (string.IsNullOrWhiteSpace(Employee.Email))
                return "Please enter the Email.";

            if (string.IsNullOrWhiteSpace(Employee.Phone))
                return "Please enter the Phone number.";

            if (Employee.Designations.Count == 0)
                return "Please add at least one designation.";

            if (Employee.Designations.Any(x =>
                    string.IsNullOrWhiteSpace(x.DesignationName)))
            {
                return "Enter a name for every designation.";
            }

            return null;
        }

        private static Employee CopyEmployee(Employee source)
        {
            var copy = new Employee
            {
                EmployeeId = source.EmployeeId,
                EmployeeName = source.EmployeeName,
                Email = source.Email,
                Phone = source.Phone,
                Salary = source.Salary,
                JoinDate = source.JoinDate,
                IsActive = source.IsActive,
                ImageUrl = source.ImageUrl,
                DepartmentId = source.DepartmentId,
                DepartmentName = source.DepartmentName
            };

            copy.Designations = new ObservableCollection<Designation>(
                source.Designations.Select(d => new Designation
                {
                    DesignationId = d.DesignationId,
                    DesignationName = d.DesignationName,
                    Grade = d.Grade,
                    BasicSalary = d.BasicSalary
                }));

            return copy;
        }
    }
}