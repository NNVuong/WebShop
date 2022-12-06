using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using SharedObjects.Commons;
using SharedObjects.Models;
using SharedObjects.ViewModels;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IOrderService orderService;
        private readonly INotyfService notyfService;

        public CheckoutController(IAccountService accountService, IOrderService orderService, INotyfService notyfService)
        {
            this.accountService = accountService;
            this.orderService = orderService;
            this.notyfService = notyfService;
        }
        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }

        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index()
        {
            //Lay gio hang ra de xu ly
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            MuaHangVM model = new MuaHangVM();
            model.UserId = User.GetSpecificClaim("Id");
            model.FullName = User.GetSpecificClaim("FullName");
            model.Phone = User.GetSpecificClaim("PhoneNumber");
            model.Address = User.GetSpecificClaim("Address");
            ViewBag.GioHang = cart;
            return View(model);
        }

        [HttpPost]
        [Route("checkout.html", Name = "Checkout")]
        public async Task<IActionResult> Index(MuaHangVM muaHang)
        {
            //Lay ra gio hang de xu ly
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            string token = User.GetSpecificClaim("token");
            UpdateUserViewModel user = new UpdateUserViewModel();
            user.UserId = muaHang.UserId;
            user.FullName = muaHang.FullName;
            user.Phone = muaHang.Phone;
            user.Address = muaHang.Address;
            await accountService.UpdateProfile(user, token);
            try
            {
                if (ModelState.IsValid)
                {
                    //Khoi tao don hang
                    OrderViewModel order = new OrderViewModel();
                    order.UserId = muaHang.UserId;
                    order.TransactStatusId = 1;
                    order.Address = muaHang.Address;
                    order.Phone = muaHang.Phone;
                    order.TotalMoney = Convert.ToInt32(cart.Sum(x => x.TotalMoney));
                    var orderId = await orderService.Add(order);

                    //tao danh sach don hang

                    foreach (var item in cart)
                    {
                        OrderDetailViewModel orderDetail = new OrderDetailViewModel();
                        orderDetail.OrderId = orderId;
                        orderDetail.ProductId = item.product.ProductId;
                        orderDetail.Amount = item.amount;
                        orderDetail.TotalMoney = order.TotalMoney;
                        orderDetail.Price = item.product.Price;
                        await orderService.AddOrderDetail(orderDetail);
                    }
                    //clear gio hang
                    HttpContext.Session.Remove("GioHang");
                    //Xuat thong bao
                    notyfService.Success("Đơn hàng đặt thành công");
                    //cap nhat thong tin khach hang
                    return RedirectToAction("Success");
                }
                else
                {
                    ViewBag.GioHang = cart;
                    return View(muaHang);
                }
            }
            catch
            {
                ViewBag.GioHang = cart;
                return View(muaHang);
            }
        }
        [Route("dat-hang-thanh-cong.html", Name = "Success")]
        public async Task<IActionResult> Success()
        {
            try
            {
                var taikhoanID = User.GetSpecificClaim("Id");
                string token = User.GetSpecificClaim("token");
                var user = await accountService.GetById(taikhoanID,token);
                if (string.IsNullOrEmpty(taikhoanID))
                {
                    return RedirectToAction("Login", "Accounts", new { returnUrl = "/dat-hang-thanh-cong.html" });
                }
                var OrderId = await orderService.GetOrderSuccess(taikhoanID);
                MuaHangSuccessVM successVM = new MuaHangSuccessVM();
                successVM.FullName = user.FullName;
                successVM.OrderId = OrderId;
                successVM.Phone = user.PhoneNumber;
                successVM.Address = user.Address;
                return View(successVM);
            }
            catch
            {
                return View();
            }
        }
    }
}