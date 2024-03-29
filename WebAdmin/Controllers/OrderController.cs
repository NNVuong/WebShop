﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using Service.Interfaces;
using SharedObjects.ValueObjects;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAdmin.Controllers
{
    [Authorize(Roles = "Admin,Editor")]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly INotyfService notyfService;

        public OrderController(IOrderService orderService, INotyfService notyfService)
        {
            this.orderService = orderService;
            this.notyfService = notyfService;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var orders = await orderService.GetAll();
            var result = orders.AsQueryable();
            PagedList<VOrder> models = new PagedList<VOrder>(result, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        public async Task<IActionResult> Details(int id)
        {
            var order = await orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            var chitietdonhang = await orderService.GetListOrdersDetail(id);
            ViewBag.ChiTiet = chitietdonhang;
            return View(order);
        }
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var order = await orderService.GetOrderById(id);
            var status = await orderService.GetTransactStatus();
            ViewBag.Status = status;
            return PartialView("ChangeStatus", order);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, VOrder model)
        {
            var result = await orderService.UpdateStatus(id, model);
            if (result.StatusCode == 200)
            {
                notyfService.Success("Cập nhật thành công!");
                return RedirectToAction("Index", "Order");
            }
            else
            {
                notyfService.Error("Đã xảy ra lỗi");
                notyfService.Information("Kiểm tra lại hàng tồn kho");
                //ModelState.AddModelError(string.Empty, result.Message);
                return RedirectToAction("Index", "Order");
            }
        }

        public async Task<IActionResult> PayStatus(int id)
        {
            var order = await orderService.GetOrderById(id);
            order.Paid = true;
            return PartialView("PayStatus", order);
        }

        [HttpPost]
        public async Task<IActionResult> PayStatus(int id, VOrder model)
        {
            
            var result = await orderService.UpdateStatus(id, model);
            if (result.StatusCode == 200)
            {
                notyfService.Success("Cập nhật thành công!");
                return RedirectToAction("Index", "Order");
            }
            else
            {
                notyfService.Error("Cập nhật không thành công!");
                //ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }
        }

        public async Task<IActionResult> NextStatus(int id)
        {
            var order = await orderService.GetOrderById(id);
            var status = await orderService.GetTransactStatus();
            ViewBag.Status = status;
            order.TransactStatusId++;
            return PartialView("NextStatus", order);
        }

        [HttpPost]
        public async Task<IActionResult> NextStatus(int id, VOrder model)
        {
            var result = await orderService.UpdateStatus(id, model);
            if (result.StatusCode == 200)
            {
                notyfService.Success("Cập nhật thành công!");
                return RedirectToAction("Index", "Order");
            }
            else
            {
                notyfService.Error("Không thể hủy đơn hàng");
                return RedirectToAction("Index", "Order");
            }
        }
    }
}
