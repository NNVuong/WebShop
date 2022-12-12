using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using Service.Interfaces;
using SharedObjects.ValueObjects;
using SharedObjects.ViewModels;

namespace WebAdmin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SalesController : Controller
    {
        private readonly ISalesService salesService;
        private readonly INotyfService notyfService;

        public SalesController(ISalesService salesService, INotyfService notyfService)
        {
            this.salesService = salesService;
            this.notyfService = notyfService;
        }

        public async Task<IActionResult> Index(FilterViewModel model, int page = 1)
        {
            var pageNumber = page;
            var pageSize = 100;

            var sales = await salesService.GetAll();
            var result = sales.Where(x => (model.BeginTime <= x.PaymentDate && x.PaymentDate <= model.EndTime)).AsQueryable();

            double TotalSum = 0;
            if (result.Count() > 0)
            {
                foreach (var r in result)
                {
                    TotalSum += (double)r.Price * r.Amount;
                }
            }

            PagedList<VSales> models = new PagedList<VSales>(result, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)result.Count() / (Double)pageSize);
            ViewBag.Begin = model.BeginTime;
            ViewBag.End = model.EndTime;
            ViewBag.TotalSum = TotalSum;

            return View(models);
        }

        public IActionResult Filter()
        {
            FilterViewModel model = new FilterViewModel();
            model.BeginTime = DateTime.Today;
            model.EndTime = DateTime.Today.AddDays(1);
            return PartialView("Filter", model);
        }

        [HttpPost]
        public IActionResult Filter(FilterViewModel model)
        {
            if (model.EndTime < model.BeginTime)
            {
                model.BeginTime = DateTime.Today;
                model.EndTime = DateTime.Today.AddDays(1);
                notyfService.Warning("Chọn sai mốc thời gian!");
                return RedirectToAction("Index", model);
            }
            notyfService.Success("Đã lọc thành công!");
            return RedirectToAction("Index", model);
        }
    }
}
