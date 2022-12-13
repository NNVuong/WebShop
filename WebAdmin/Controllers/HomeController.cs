using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PagedList.Core;
using Service.Implementations;
using Service.Interfaces;
using SharedObjects.ValueObjects;
using WebAdmin.Models;
using System.Security.Claims;
using SharedObjects.Commons;

namespace WebAdmin.Controllers
{
    [Authorize(Roles = "Admin,Editor")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISalesService salesService;
        private readonly IOrderService orderService;

        public HomeController(ILogger<HomeController> logger, ISalesService salesService, IOrderService orderService)
        {
            _logger = logger;
            this.salesService = salesService;
            this.orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            DateTime date = DateTime.Today;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var sales = await salesService.GetAll();
            var orders = await orderService.GetAll();

            double TotalSum = 0;
            if (sales.Where(x => firstDayOfMonth <= x.PaymentDate && x.PaymentDate <= lastDayOfMonth).AsQueryable().Count() > 0)
            {
                foreach (var r in sales.Where(x => (firstDayOfMonth <= x.PaymentDate && x.PaymentDate <= lastDayOfMonth)).AsQueryable())
                {
                    TotalSum += (double)r.Price * r.Amount;
                }
            }

            var result = orders.Where(x => x.TransactStatusId == 3 && firstDayOfMonth <= x.ShipDate && x.ShipDate <= lastDayOfMonth).AsQueryable();

            PagedList<VOrder> models = new PagedList<VOrder>(result, 1, 10);


            double[] Sum = new double [13];

            for (int i = 1; i <= 12; i++)
            {
                firstDayOfMonth = new DateTime(date.Year, i, 1);
                lastDayOfMonth = firstDayOfMonth.AddMonths(1);

                double SumF = 0;

                if (sales.Where(x => (firstDayOfMonth <= x.PaymentDate && x.PaymentDate <= lastDayOfMonth)).AsQueryable().Count() > 0)
                {
                    foreach (var r in sales.Where(x => (firstDayOfMonth <= x.PaymentDate && x.PaymentDate <= lastDayOfMonth)).AsQueryable())
                    {
                        SumF += (double)r.Price * r.Amount;
                    }
                }

                Sum[i] = SumF/1000;
            }

            ViewBag.T1 = Sum[1];
            ViewBag.T2 = Sum[2];
            ViewBag.T3 = Sum[3];
            ViewBag.T4 = Sum[4];
            ViewBag.T5 = Sum[5];
            ViewBag.T6 = Sum[6];
            ViewBag.T7 = Sum[7];
            ViewBag.T8 = Sum[8];
            ViewBag.T9 = Sum[9];
            ViewBag.T10 = Sum[10];
            ViewBag.T11 = Sum[11];
            ViewBag.T12 = Sum[12];

            ViewBag.TotalSum = TotalSum;
            ViewBag.TotalOrder = result.Count();
            return View(models);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
