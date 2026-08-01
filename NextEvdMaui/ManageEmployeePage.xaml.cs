using NextEvdMaui.Models;
using NextEvdMaui.Services;
using NextEvdMaui.ViewModels;

namespace NextEvdMaui;

[QueryProperty(nameof(EmployeeId), "EmployeeId")]
public partial class ManageEmployeePage : ContentPage
{
    private readonly ApiService _apiService = new();
    private ManageEmployeeViewModel? _viewModel;

    public string? EmployeeId { get; set; }

    public ManageEmployeePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel is not null)
        {
            return;
        }

        var employee = new Employee();

        if (int.TryParse(EmployeeId, out var id) && id > 0)
        {
            try
            {
                var employees = await _apiService.GetEmployeesAsync();
                employee = employees.FirstOrDefault(x => x.EmployeeId == id) ?? employee;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Could not load employee: {ex.Message}", "OK");
            }
        }

        _viewModel = new ManageEmployeeViewModel(employee);
        _viewModel.SaveSucceeded += OnSaveSucceeded;
        _viewModel.SaveFailed += OnSaveFailed;

        BindingContext = _viewModel;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (_viewModel is not null)
        {
            _viewModel.SaveSucceeded -= OnSaveSucceeded;
            _viewModel.SaveFailed -= OnSaveFailed;
        }
    }

    private async void OnSaveSucceeded()
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnSaveFailed(string message)
    {
        await DisplayAlert("Error", message, "OK");
    }
}