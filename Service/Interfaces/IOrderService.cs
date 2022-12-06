using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharedObjects.Commons;
using SharedObjects.Models;
using SharedObjects.ValueObjects;
using SharedObjects.ViewModels;

namespace Service.Interfaces
{
    public interface IOrderService
    {
        Task<int> Add(OrderViewModel model);
        Task<ResponseResult> AddOrderDetail(OrderDetailViewModel model);
        Task<int> GetOrderSuccess(string userId);
        Task<List<VOrder>> GetListOrders(string userId);
        Task<List<VOrderDetail>> GetListOrdersDetail(int orderId);
        Task<VOrder> GetOrder(string userId, int orderId);
        Task<List<VOrder>> GetAll();
        Task<VOrder> GetOrderById(int orderId);
        Task<List<VTransactStatus>> GetTransactStatus();
        Task<ResponseResult> UpdateStatus(int id, VOrder model);
    }
}