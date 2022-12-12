using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using SharedObjects.ViewModels;
using WebShop.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        private readonly IProductImageService productImage;
        public readonly INotyfService notyfService;

        public ProductController(IProductService productService, ICategoryService categoryService, IProductImageService productImage, INotyfService notyfService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            this.productImage = productImage;
            this.notyfService = notyfService;
        }

        public async Task<IActionResult> Index()
        {
            var count = await productService.CountProduct();
            ViewBag.count = count;
            var categories = await categoryService.GetAll();
            return View(categories);
        }
        public async Task<IActionResult> CountPagination()
        {
            var count = await productService.CountProduct();
            return Json(new { result = count });
        }
        public async Task<IActionResult> GetPagination_Pta([FromBody] PaginationViewModel model)
        {
            var result = await productService.GetPagination(model);
            return PartialView(result);
        }
        public async Task<IActionResult> Detail(int productId)
        {
            var products = await productService.GetAll();
            var productRandom = products.PickRandom(6);
            ViewBag.productRandom = productRandom;

            var image = await productImage.GetImage(productId);
            ViewBag.image = image;
            var product = await productService.GetById(productId);
            return View(product);
        }
        public async Task<IActionResult> GetById(int productId)
        {
            var product = await productService.GetById(productId);
            return Json(new { result = product });
        }
        public async Task<IActionResult> HomeSearch(string keyword)
        {
            if (keyword == null)
            {
                return Ok();
            }
            var product = await productService.Search(keyword);
            return PartialView(product);
        }
    }
}
