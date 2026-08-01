using NextEvdMaui.Models;
using NextEvdMaui.Services;
using System.Linq;

namespace NextEvdMaui;

[QueryProperty(nameof(EmployeeId), "EmployeeId")]
public partial class EmployeeDetailsPage : ContentPage
{
    private readonly ApiService _apiService = new();

    public string? EmployeeId { get; set; }

    public EmployeeDetailsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadEmployeeAsync();
    }

    private async Task LoadEmployeeAsync()
    {
        if (!int.TryParse(EmployeeId, out var id) || id <= 0)
        {
            return;
        }

        try
        {
            var employees = await _apiService.GetEmployeesAsync();
            var employee = employees.FirstOrDefault(x => x.EmployeeId == id);

            if (employee is null)
            {
                await DisplayAlert("Not Found", "This employee could not be found.", "OK");
                return;
            }

            BindingContext = employee;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Could not load employee: {ex.Message}", "OK");
        }
    }

    private async void OnEditClicked(object? sender, EventArgs e)
    {
        if (BindingContext is Employee employee)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(ManageEmployeePage)}?EmployeeId={employee.EmployeeId}");
        }
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (BindingContext is not Employee employee)
        {
            return;
        }

        var confirmed = await DisplayAlert(
            "Delete Employee",
            $"Delete {employee.EmployeeName}?",
            "Delete",
            "Cancel");

        if (!confirmed)
        {
            return;
        }

        var result = await _apiService.DeleteEmployeeAsync(employee.EmployeeId);

        if (result.Success)
        {
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await DisplayAlert("Error", result.ErrorMessage, "OK");
        }
    }
}
