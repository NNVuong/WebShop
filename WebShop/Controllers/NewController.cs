using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using Service.Interfaces;
using SharedObjects.Models;
using SharedObjects.ValueObjects;
using WebShop.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebShop.Controllers
{
    public class NewController : Controller
    {
        public readonly INotyfService notyfService;
        private readonly INewService newService;

        public NewController(INewService newService, INotyfService notyfService)
        {
            this.newService = newService;
            this.notyfService = notyfService;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 6;

            var news = await newService.GetAll();
            var result = news.OrderByDescending(x=>x.IsNewFeed).ThenByDescending(x=>x.CreatedDate).AsQueryable();

            PagedList<VNew> models = new PagedList<VNew>(result, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)result.Count() / (Double)pageSize);

            return View(models);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var newsRandom = await newService.GetAll();
            var result = newsRandom.PickRandom(3);
            ViewBag.newRandom = result;

            var news = await newService.GetById(id);
            return View(news);
        }
    }
}
