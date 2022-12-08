using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using SharedObjects.Commons;
using SharedObjects.ValueObjects;
using SharedObjects.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebShop.Controllers
{
    public class DonHangController : Controller
    {
        private readonly INotyfService notyfService;
        private readonly IOrderService orderService;

        public DonHangController(IOrderService orderService, INotyfService notyfService)
        {
            this.orderService = orderService;
            this.notyfService= notyfService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int orderId)
        {
            try
            {
                var userId = User.GetSpecificClaim("Id");
                var donhang = await orderService.GetOrder(userId, orderId);
                var chitietdonhang = await orderService.GetListOrdersDetail(orderId);
                XemDonHang donHang = new XemDonHang();
                donHang.DonHang = donhang;
                donHang.ChiTietDonHang = chitietdonhang;
                return PartialView("Details", donHang);
            }
            catch 
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> CancleStatus(int orderId)
        {
            try
            {
                var userId = User.GetSpecificClaim("Id");
                var order = await orderService.GetOrder(userId, orderId);
                var status = await orderService.GetTransactStatus();
                ViewBag.Status = status;
                order.TransactStatusId = 4;
                return PartialView("CancleStatus", order);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancleStatus(int orderId, VOrder model)
        {
            var result = await orderService.UpdateStatus(orderId, model);
            if (result.StatusCode == 200)
            {
                notyfService.Success("Đã hủy đơn thành công!");
                return RedirectToAction("Index", "Account");
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }
        }
    }
}
