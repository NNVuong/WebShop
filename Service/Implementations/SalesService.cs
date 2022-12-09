using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Service.Interfaces;
using SharedObjects.Commons;
using SharedObjects.Models;
using SharedObjects.ValueObjects;
using SharedObjects.ViewModels;

namespace Service.Implementations
{
    public class SalesService : BaseService, ISalesService
    {
        public async Task<List<VSales>> GetAll()
        {
            List<VSales> result = new List<VSales>();

            using (var response = await httpClient.GetAsync("api/donhang/get-all-sales"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<VSales>>(apiResponse);
            }
            return result;
        }

    }
}
