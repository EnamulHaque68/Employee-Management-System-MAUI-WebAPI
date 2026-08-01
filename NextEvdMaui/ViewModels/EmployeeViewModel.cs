using NextEvdMaui.Models;
using NextEvdMaui.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NextEvdMaui.ViewModels
{
    public class EmployeeListViewModel : BaseViewModel
    {
        private readonly ApiService _apiService = new();
        private bool _isRefreshing;

        public ObservableCollection<Employee> Employees { get; } = new();

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public ICommand RefreshCommand { get; }

        public EmployeeListViewModel()
        {
            RefreshCommand = new Command(async () => await LoadEmployeesAsync());
        }

        public async Task LoadEmployeesAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                IsRefreshing = true;

                var items = await _apiService.GetEmployeesAsync();

                Employees.Clear();

                foreach (var item in items)
                {
                    Employees.Add(item);
                }
            }
            finally
            {
                IsRefreshing = false;
                IsBusy = false;
            }
        }

        public async Task<ApiResult> DeleteEmployeeAsync(int id)
        {
            return await _apiService.DeleteEmployeeAsync(id);
        }
    }
}