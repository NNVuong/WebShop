using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedObjects.Commons;
using SharedObjects.Models;
using SharedObjects.ValueObjects;
using SharedObjects.ViewModels;
using WebAPI.StoredProcedure;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly WebShopDbContext context;

        public DonHangController(WebShopDbContext context)
        {
            this.context = context;
        }
        [HttpPost("add")]
        public async Task<int> Add(OrderViewModel model)
        {
            Order donhang = new Order();
            donhang.UserId = model.UserId;
            donhang.TransactStatusId = model.TransactStatusId;
            donhang.TotalMoney = model.TotalMoney;
            donhang.Address = model.Address;
            donhang.Phone = model.Phone;
            donhang.OrderDate = DateTime.Now;
            donhang.Deleted = false;
            donhang.Paid = false;
            context.Add(donhang);
            await context.SaveChangesAsync();
            return donhang.OrderId;
        }
        [HttpGet("get-order-success/{userId}")]
        public async Task<int> GetOrderSuccess(string userId)
        {
            var donhang = await context.Orders
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.OrderDate)
                    .FirstOrDefaultAsync();
            return donhang.OrderId;
        }

        [HttpPost("add-order-detail")]
        public async Task<IActionResult> AddOrderDetail(OrderDetailViewModel model)
        {
            try
            {
                await context.Database.ExecuteSqlRawAsync(OrderSP.SP_Add_OrderDetail, model.OrderId, model.ProductId, model.Amount, model.TotalMoney, model.Price);
                return Ok(new ResponseResult(200));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseResult(400));
            }
        }
        [HttpGet("get-list-orders/{userId}")]
        public async Task<List<VOrder>> GetListOrders(string userId)
        {
            var result = await context.Set<VOrder>().FromSqlRaw(OrderSP.SP_Get_Order_By_User, userId).AsNoTracking().ToListAsync();
            return result;
        }
        [HttpGet("get-list-order-detail/{orderId}")]
        public async Task<List<VOrderDetail>> GetListOrdersDetail(int orderId)
        {
            var result = await context.Set<VOrderDetail>().FromSqlRaw(OrderSP.SP_Get_List_OrderDetail, orderId).AsNoTracking().ToListAsync();
            return result;
        }
        [HttpGet("get-orders/{userId}/{orderId}")]
        public VOrder GetOrder(string userId,int orderId)
        {
            var result = context.Set<VOrder>().FromSqlRaw(OrderSP.SP_Get_Order, userId,orderId).AsNoTracking().AsEnumerable().FirstOrDefault();
            return result;
        }
        [HttpGet("get-all")]
        public async Task<List<VOrder>> GetAll()
        {
            var result = await context.Set<VOrder>().FromSqlRaw(OrderSP.SP_Get_All).AsNoTracking().ToListAsync();
            return result;
        }
        [HttpGet("get-order-by-id/{orderId}")]
        public VOrder GetOrderById(int orderId)
        {
            var result = context.Set<VOrder>().FromSqlRaw(OrderSP.SP_Get_Order_By_Id, orderId).AsNoTracking().AsEnumerable().FirstOrDefault();
            return result;
        }
        [HttpGet("get-transactStatus")]
        public async Task<List<VTransactStatus>> GetTransactStatus()
        {
            var result = await context.Set<VTransactStatus>().FromSqlRaw(OrderSP.SP_Get_TransacStatus).AsNoTracking().ToListAsync();
            return result;
        }

        [HttpPut("update-transactStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, VOrder model)
        {
            try
            {
                var donhang = context.Orders.AsNoTracking().FirstOrDefault(x => x.OrderId == id);
                donhang.Paid = model.Paid;
                donhang.TransactStatusId = model.TransactStatusId;
                if (donhang.Paid == true)
                {
                    donhang.PaymentDate = DateTime.Now;
                }
                if (donhang.TransactStatusId == 4) donhang.Deleted = true;
                else donhang.Deleted = false;
                if (donhang.TransactStatusId == 2 || donhang.TransactStatusId == 3) donhang.ShipDate = DateTime.Now;
                context.Update(donhang);
                await context.SaveChangesAsync();
                return Ok(new ResponseResult(200));
            }
            catch 
            {
                return BadRequest(new ResponseResult(400));
            }
        }

        
    }
}