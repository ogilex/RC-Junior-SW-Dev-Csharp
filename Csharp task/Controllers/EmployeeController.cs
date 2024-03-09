using Csharp_task.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json;

namespace Csharp_task.Controllers
{
    public class EmployeeController : Controller
    {   

        private readonly HttpClient client;

        public EmployeeController(HttpClient httpClient)
        {
            client = httpClient;
        }

        // GET: EmployeeController
        public async Task<IActionResult> Index()
        {
            Dictionary<string, long> employeesDictionary = new Dictionary<string, long>();
            var data = await getDataFromApi();
            
            foreach (var emp in data)
            {   
                DateTime parsedStartDate = DateTime.Parse(emp.StarTimeUtc);
                DateTime parsedEndDate = DateTime.Parse(emp.EndTimeUtc);
                TimeSpan diff = parsedEndDate - parsedStartDate;

                long diffInMs = (long)diff.TotalMilliseconds;

                if(emp.EmployeeName != null && diffInMs >= 0)
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

            List<EmployeeDto> employees = new List<EmployeeDto>();

            foreach (var kvp in employeesDictionary)
            {
                long hrsWorked = kvp.Value / 3600000;
                EmployeeDto employee = new EmployeeDto(kvp.Key, hrsWorked);
                
                employees.Add(employee);
            }

            employees.Sort();
            
            return View(employees);
        }
        
        private async Task<List<EmployeeModel>> getDataFromApi()
        {
            List<EmployeeModel> data = new List<EmployeeModel>();
            HttpResponseMessage response = await client.GetAsync("https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==");
      
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<List<EmployeeModel>>(jsonString);
                Debug.WriteLine("Data JSOn" + jsonString);
            }

            return data;
        }
    }
}
