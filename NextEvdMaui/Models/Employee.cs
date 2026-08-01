using NextEvdMaui.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using NextEvdMaui.Services;

namespace NextEvdMaui.Models
{
    public class Employee : BaseViewModel
    {
        private int _employeeId;
        private string _employeeName = string.Empty;
        private string _email = string.Empty;
        private string _phone = string.Empty;
        private decimal _salary;
        private DateTime _joinDate = DateTime.Today;
        private int _departmentId;
        private string _departmentName = string.Empty;
        private Department? _department;
        private bool _isActive = true;
        private string? _imageUrl;
        private string? _localImagePath;

        private ObservableCollection<Designation> _designations = new();

        public int EmployeeId
        {
            get => _employeeId;
            set => SetProperty(ref _employeeId, value);
        }

        public string EmployeeName
        {
            get => _employeeName;
            set => SetProperty(ref _employeeName, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        public decimal Salary
        {
            get => _salary;
            set => SetProperty(ref _salary, value);
        }

        public DateTime JoinDate
        {
            get => _joinDate;
            set => SetProperty(ref _joinDate, value);
        }

        public int DepartmentId
        {
            get => _departmentId;
            set => SetProperty(ref _departmentId, value);
        }

        public string DepartmentName
        {
            get => _departmentName;
            set => SetProperty(ref _departmentName, value);
        }

        // The API serializes the employee's department as a nested object
        // (e.g. "department": { "departmentId": 2, "departmentName": "IT" }),
        // not as a flat "departmentName" field. This property catches that
        // nested object during deserialization and keeps DepartmentName in sync.
        [JsonPropertyName("department")]
        public Department? Department
        {
            get => _department;
            set
            {
                if (SetProperty(ref _department, value) && value is not null)
                {
                    DepartmentName = value.DepartmentName;
                }
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        // Matches the API's Employee.ImageUrl (e.g. "/images/xxxx.jpg")
        public string? ImageUrl
        {
            get => _imageUrl;
            set
            {
                if (SetProperty(ref _imageUrl, value))
                {
                    OnPropertyChanged(nameof(DisplayedImageSource));
                }
            }
        }

        [JsonIgnore]
        public string? LocalImagePath
        {
            get => _localImagePath;
            set
            {
                if (SetProperty(ref _localImagePath, value))
                {
                    OnPropertyChanged(nameof(DisplayedImageSource));
                }
            }
        }

        public ObservableCollection<Designation> Designations
        {
            get => _designations;
            set => SetProperty(ref _designations, value ?? new());
        }

        [JsonIgnore]
        public ImageSource? DisplayedImageSource
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(LocalImagePath) &&
                    File.Exists(LocalImagePath))
                {
                    return ImageSource.FromFile(LocalImagePath);
                }

                if (!string.IsNullOrWhiteSpace(ImageUrl))
                {
                    return ImageSource.FromUri(
                        new Uri(ApiSettings.GetImageUrl(ImageUrl)));
                }

                return null;
            }
        }
    }
}