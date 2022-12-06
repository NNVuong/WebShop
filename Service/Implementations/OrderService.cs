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
    public class OrderService : BaseService, IOrderService
    {
        public async Task<int> Add(OrderViewModel model)
        {
            int responseResult = 0;
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            
            using (var response = await httpClient.PostAsync("api/donhang/add", content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<int>(apiResponse);
            }
            return responseResult;
        }

        public async Task<ResponseResult> AddOrderDetail(OrderDetailViewModel model)
        {
            ResponseResult responseResult = new ResponseResult();
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            using (var response = await httpClient.PostAsync("api/donhang/add-order-detail", content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }
            return responseResult;
        }

        public async Task<List<VOrder>> GetAll()
        {
            List<VOrder> result = new List<VOrder>();

            using (var response = await httpClient.GetAsync("api/donhang/get-all"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<VOrder>>(apiResponse);
            }
            return result;
        }

        public async Task<List<VOrder>> GetListOrders(string userId)
        {
            List<VOrder> result = new List<VOrder>();

            using (var response = await httpClient.GetAsync("api/donhang/get-list-orders/" + userId))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<VOrder>>(apiResponse);
            }
            return result;
        }

        public async Task<List<VOrderDetail>> GetListOrdersDetail(int orderId)
        {
            List<VOrderDetail> result = new List<VOrderDetail>();

            using (var response = await httpClient.GetAsync("api/donhang/get-list-order-detail/" + orderId))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<VOrderDetail>>(apiResponse);
            }
            return result;
        }

        public async Task<VOrder> GetOrder(string userId, int orderId)
        {
            VOrder result = new VOrder();

            using (var response = await httpClient.GetAsync($"api/donhang/get-orders/{userId}/{orderId}"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<VOrder>(apiResponse);
            }
            return result;
        }

        public async Task<VOrder> GetOrderById(int orderId)
        {
            VOrder result = new VOrder();

            using (var response = await httpClient.GetAsync($"api/donhang/get-order-by-id/{orderId}"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<VOrder>(apiResponse);
            }
            return result;
        }

        public async Task<int> GetOrderSuccess(string userId)
        {
            int responseResult = 0;
            using (var response = await httpClient.GetAsync("api/donhang/get-order-success/" + userId))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<int>(apiResponse);
            }
            return responseResult;
        }
        public async Task<List<VTransactStatus>> GetTransactStatus()
        {
            List<VTransactStatus> result = new List<VTransactStatus>();

            using (var response = await httpClient.GetAsync("api/donhang/get-transactStatus"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<VTransactStatus>>(apiResponse);
            }
            return result;
        }
        public async Task<ResponseResult> UpdateStatus(int id, VOrder model)
        {
            ResponseResult responseResult = new ResponseResult();
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            using (var response = await httpClient.PutAsync($"api/donhang/update-transactStatus/{id}", content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }
            return responseResult;
        }
    }
}
