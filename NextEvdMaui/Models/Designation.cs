using NextEvdMaui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextEvdMaui.Models
{
    public class Designation : BaseViewModel
    {
        private int _designationId;
        private string _designationName = string.Empty;
        private int _grade;
        private decimal _basicSalary;

        public int DesignationId
        {
            get => _designationId;
            set => SetProperty(ref _designationId, value);
        }

        public string DesignationName
        {
            get => _designationName;
            set => SetProperty(ref _designationName, value);
        }

        public int Grade
        {
            get => _grade;
            set => SetProperty(ref _grade, value);
        }

        public decimal BasicSalary
        {
            get => _basicSalary;
            set => SetProperty(ref _basicSalary, value);
        }
    }
}