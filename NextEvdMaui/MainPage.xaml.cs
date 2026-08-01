using NextEvdMaui.Models;
using NextEvdMaui.ViewModels;
using System.Linq;

namespace NextEvdMaui;

public partial class MainPage : ContentPage
{
    private readonly EmployeeListViewModel _viewModel = new();

    public MainPage()
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadEmployeesAsync();
    }

    private async void OnAddClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ManageEmployeePage));
    }

    private async void OnEmployeeSelected(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is CollectionView collectionView)
        {
            collectionView.SelectedItem = null;
        }

        if (e.CurrentSelection.FirstOrDefault() is Employee employee)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(EmployeeDetailsPage)}?EmployeeId={employee.EmployeeId}");
        }
    }

    private async void OnEditInvoked(object? sender, EventArgs e)
    {
        if (sender is SwipeItem { CommandParameter: Employee employee })
        {
            await Shell.Current.GoToAsync(
                $"{nameof(ManageEmployeePage)}?EmployeeId={employee.EmployeeId}");
        }
    }

    private async void OnDeleteInvoked(object? sender, EventArgs e)
    {
        if (sender is not SwipeItem { CommandParameter: Employee employee })
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

        var result = await _viewModel.DeleteEmployeeAsync(employee.EmployeeId);

        if (result.Success)
        {
            await _viewModel.LoadEmployeesAsync();
        }
        else
        {
            await DisplayAlert("Error", result.ErrorMessage, "OK");
        }
    }
}
