using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using SharedObjects.ViewModels;
using SharedObjects.Commons;
using PagedList.Core;
using SharedObjects.ValueObjects;
using Microsoft.AspNetCore.Authorization;

namespace WebAdmin.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly INotyfService notyfService;
        public CategoryController(ICategoryService categoryService, INotyfService notyfService)
        {
            this.categoryService = categoryService;
            this.notyfService = notyfService;
        }
        public async Task<IActionResult> Index()
        {
            var category = await categoryService.GetAll();
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> FindCategory(string search)
        {
            var category = await categoryService.SearchCategory(search);
            return PartialView("ListCategoryPta", category);
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Add([FromForm] CategoryViewModel model)
        {
            string token = User.GetSpecificClaim("token");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var result = await categoryService.Add(model, token);
                if (result.StatusCode == 200)
                {
                    notyfService.Success("Thêm mới thành công!");
                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(model);
                }
            }
        }

        public async Task<IActionResult> Update(int catId)
        {
            var category = await categoryService.GetById(catId);
            return View(category);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] CategoryViewModel model)
        {
            string token = User.GetSpecificClaim("token");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var result = await categoryService.Update(model, token);
                if (result.StatusCode == 200)
                {
                    notyfService.Success("Cập nhật thành công!");
                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(model);
                }
            }
        }
        public async Task<IActionResult> Delete(int catId)
        {
            string token = User.GetSpecificClaim("token");

            var result = await categoryService.Delete(catId, token);

            return Json(new { statusCode = result.StatusCode });
        }
    }
}