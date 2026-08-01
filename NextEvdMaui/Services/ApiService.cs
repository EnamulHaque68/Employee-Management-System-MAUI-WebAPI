using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NextEvdMaui.Models;

namespace NextEvdMaui.Services
{
    public class ApiService
    {
        // Matches [Route("api/[controller]")] on EmployeeController -> "api/Employee"
        private const string EmployeeRoute = "api/Employee";

        private static readonly JsonSerializerOptions JsonOptions =
            new(JsonSerializerDefaults.Web)
            {
                PropertyNameCaseInsensitive = true
            };

        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri(ApiSettings.BaseAddress),
            Timeout = TimeSpan.FromSeconds(60)
        };

        public async Task<List<Department>> GetDepartmentsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Department>>(
                $"{EmployeeRoute}/Departments",
                JsonOptions) ?? new List<Department>();
        }

        public async Task<List<Designation>> GetDesignationsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Designation>>(
                $"{EmployeeRoute}/Designations",
                JsonOptions) ?? new List<Designation>();
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Employee>>(
                EmployeeRoute,
                JsonOptions) ?? new List<Employee>();
        }

        // The API binds this endpoint with [FromForm] EmployeeDtos, so every field
        // has to be sent as its own form field (name must match the DTO property
        // name) rather than as one JSON blob.
        public async Task<ApiResult> SaveEmployeeAsync(Employee employee)
        {
            using var form = new MultipartFormDataContent();

            form.Add(new StringContent(employee.EmployeeName ?? ""), "EmployeeName");
            form.Add(new StringContent(employee.Email ?? ""), "Email");
            form.Add(new StringContent(employee.Phone ?? ""), "Phone");
            form.Add(new StringContent(
                employee.Salary.ToString(CultureInfo.InvariantCulture)), "Salary");
            form.Add(new StringContent(
                employee.JoinDate.ToString("o", CultureInfo.InvariantCulture)), "JoinDate");
            form.Add(new StringContent(employee.IsActive.ToString()), "IsActive");
            form.Add(new StringContent(
                employee.DepartmentId.ToString(CultureInfo.InvariantCulture)), "DepartmentId");

            var designationJson = JsonSerializer.Serialize(
                employee.Designations.Select(d => new
                {
                    d.DesignationName,
                    d.Grade,
                    d.BasicSalary
                }),
                JsonOptions);

            form.Add(new StringContent(designationJson, Encoding.UTF8), "DesignationJson");

            if (!string.IsNullOrWhiteSpace(employee.LocalImagePath) &&
                File.Exists(employee.LocalImagePath))
            {
                var imageContent = new StreamContent(
                    File.OpenRead(employee.LocalImagePath));

                form.Add(
                    imageContent,
                    "Profile",
                    Path.GetFileName(employee.LocalImagePath));
            }

            try
            {
                using var response = employee.EmployeeId == 0
                    ? await _httpClient.PostAsync(EmployeeRoute, form)
                    : await _httpClient.PutAsync($"{EmployeeRoute}/{employee.EmployeeId}", form);

                if (response.IsSuccessStatusCode)
                {
                    return ApiResult.Ok();
                }

                var body = await response.Content.ReadAsStringAsync();
                return ApiResult.Fail(ExtractErrorMessage(body));
            }
            catch (HttpRequestException ex)
            {
                return ApiResult.Fail(
                    $"API connection failed: {ex.Message}\n" +
                    $"Base address: {ApiSettings.BaseAddress}");
            }
            catch (TaskCanceledException)
            {
                return ApiResult.Fail("The API request timed out.");
            }
        }

        public async Task<ApiResult> DeleteEmployeeAsync(int id)
        {
            try
            {
                using var response = await _httpClient.DeleteAsync(
                    $"{EmployeeRoute}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return ApiResult.Ok();
                }

                var body = await response.Content.ReadAsStringAsync();
                return ApiResult.Fail(ExtractErrorMessage(body));
            }
            catch (HttpRequestException ex)
            {
                return ApiResult.Fail($"Delete failed: {ex.Message}");
            }
        }

        private static string ExtractErrorMessage(string body)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                return "The API request was unsuccessful.";
            }

            try
            {
                using var document = JsonDocument.Parse(body);

                if (document.RootElement.TryGetProperty(
                    "message",
                    out var message))
                {
                    return message.GetString() ?? body;
                }
            }
            catch (JsonException)
            {
            }

            return body;
        }
    }
}