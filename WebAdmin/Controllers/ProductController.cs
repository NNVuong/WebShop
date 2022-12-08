using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using SharedObjects.Commons;
using SharedObjects.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAdmin.Controllers
{
    [Authorize(Roles = "Admin,Editor")]
    public class ProductController : Controller
    {
        private readonly INotyfService notyfService;
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        private readonly IProductImageService productImage;

        public ProductController(INotyfService notyfService,IProductService productService, ICategoryService categoryService, IProductImageService productImage)
        {
            this.notyfService = notyfService;
            this.productService = productService;
            this.categoryService = categoryService;
            this.productImage = productImage;
        }

        public async Task<IActionResult> Index()
        {
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
        public async Task<IActionResult> Add()
        {
            var categories = await categoryService.GetAll();
            ViewBag.categories = categories;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] ProductViewModel model)
        {
            string token = User.GetSpecificClaim("token");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var result = await productService.Add(model, token);
                if (result.StatusCode == 200)
                {
                    notyfService.Success("Thêm mới thành công!");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(model);
                }
            }
        }
        public async Task<IActionResult> Update(int productId)
        {
            var categories = await categoryService.GetAll();
            ViewBag.categories = categories;
            var product = await productService.GetById(productId);
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromForm] ProductViewModel model)
        {
            string token = User.GetSpecificClaim("token");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var result = await productService.Update(model, token);
                if (result.StatusCode == 200)
                {
                    notyfService.Success("Cập nhật thành công!");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(model);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int productId)
        {
            string token = User.GetSpecificClaim("token");

            var result = await productService.Delete(productId, token);

            return Json(new { statusCode = result.StatusCode });
        }

        public async Task<IActionResult> ProductImage(int productId)
        {
            var images = await productImage.GetImage(productId);
            ViewBag.images = images;
            var product = await productService.GetById(productId);
            ViewBag.product = product;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            string token = User.GetSpecificClaim("token");

            var result = await productImage.Delete(imageId, token);

            return Json(new { statusCode = result.StatusCode });
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(ProductImageViewModel model)
        {
            string token = User.GetSpecificClaim("token");
            if (model.ImageName == null)
            {
                notyfService.Warning("Thêm mới thất bại!");
                return RedirectToAction("ProductImage", "Product", new { productId = model.ProductId });
            }
            var result = await productImage.Add(model, token);
            if (result.StatusCode == 200)
            {
                notyfService.Success("Thêm mới thành công!");
                return RedirectToAction("ProductImage", "Product",new {productId = model.ProductId });
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }
        }
    }
}