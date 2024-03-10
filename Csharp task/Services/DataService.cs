using Csharp_task.Models;
using Newtonsoft.Json;

namespace Csharp_task.Services
{
    public class DataService
    {
        private readonly HttpClient _httpClient;

        public DataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<EmployeeModel>> GetDataFromApiAsync()
        {
            List<EmployeeModel>? data = new List<EmployeeModel>();
            HttpResponseMessage response = await _httpClient.GetAsync("https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==");

            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<List<EmployeeModel>>(jsonString);
            }

            return data;
        }
    }
}
