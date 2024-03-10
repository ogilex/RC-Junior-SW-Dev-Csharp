using Csharp_task.Helpers;
using Csharp_task.Models;
using Csharp_task.Services;
using Microsoft.AspNetCore.Mvc;

namespace Csharp_task.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly DataService dataService;
        private List<EmployeeDto> employees = new List<EmployeeDto>();
        public EmployeeController(DataService service)
        {
            dataService = service;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                await manipulateData();
                return View(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the data. " + ex);
            }

        }

        public async Task<IActionResult> GenerateAndDownloadPieChart()
        {
            try
            {
                await manipulateData();
                byte[] chartBytes = EmployeePieChartHelper.GenerateEmployeePieChart(employees);
                return File(chartBytes, "image/png", "pie_chart.png");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the data. " + ex);
            }

        }

        private async Task manipulateData()
        {
            var data = await dataService.GetDataFromApiAsync();
            Dictionary<string, long> employeesDictionary = new Dictionary<string, long>();
            foreach (var emp in data)
            {
                DateTime parsedStartDate = DateTime.Parse(emp.StarTimeUtc);
                DateTime parsedEndDate = DateTime.Parse(emp.EndTimeUtc);
                TimeSpan diff = parsedEndDate - parsedStartDate;

                long diffInMs = (long)diff.TotalMilliseconds;

                if (emp.EmployeeName != null && diffInMs >= 0)
                {
                    EmployeeDto empDtp = new EmployeeDto(emp.EmployeeName, diffInMs);
                    if (employeesDictionary.ContainsKey(empDtp.EmployeeName))
                    {
                        long oldValue = employeesDictionary[empDtp.EmployeeName];
                        employeesDictionary[empDtp.EmployeeName] = oldValue + empDtp.HoursWorked;
                    }
                    else
                    {
                        employeesDictionary.Add(empDtp.EmployeeName, diffInMs);
                    }
                }

            }

            foreach (var kvp in employeesDictionary)
            {
                long hrsWorked = kvp.Value / 3600000;
                EmployeeDto employee = new EmployeeDto(kvp.Key, hrsWorked);

                employees.Add(employee);
            }

            employees.Sort();

        }
    }
}

