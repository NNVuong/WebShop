using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using SharedObjects.Commons;
using SharedObjects.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebShop.Controllers
{
    public class DonHangController : Controller
    {
        private readonly IOrderService orderService;

        public DonHangController(IOrderService orderService)
        {
            this.orderService = orderService;
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
    }
}
