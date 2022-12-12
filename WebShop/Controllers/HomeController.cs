using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Interfaces;
using SharedObjects.ViewModels;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        private readonly IProductImageService productImage;
        private readonly INewService newService;
        public readonly INotyfService notyfService;

        public HomeController(IProductService productService, ICategoryService categoryService, IProductImageService productImage, INewService newService, INotyfService notyfService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            this.productImage = productImage;
            this.newService = newService;
            this.notyfService = notyfService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await productService.GetProductCategory();
            ViewBag.categories = categories;

            HomeViewModel model = new HomeViewModel();
            var lsProducts = await productService.GetAll();
            List<ProductHomeVM> lsProductViews = new List<ProductHomeVM>();
            var lsCats = await categoryService.GetAll();
            foreach (var item in lsCats)
            {
                ProductHomeVM productHome = new ProductHomeVM();
                productHome.category = item;
                productHome.lsProducts = lsProducts.Where(x => x.CatId == item.CatId).ToList();
                lsProductViews.Add(productHome);
                model.Products = lsProductViews;
                ViewBag.AllProducts = lsProducts;
            }
            var lsNews = await newService.Newfeed();
            model.News = lsNews;
            return View(model);
        }
        public IActionResult Contact()
        {
            return View();
        }
        public async Task<IActionResult> About()
        {
            var newsRandom = await newService.GetAll();
            var result = newsRandom.PickRandom(4);
            ViewBag.newRandom = result;
            return View();
        }
    }
}
