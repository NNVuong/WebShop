using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using Service.Interfaces;
using SharedObjects.Commons;
using SharedObjects.ValueObjects;
using SharedObjects.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAdmin.Controllers
{
    [Authorize(Roles = "Admin,Editor")]
    public class NewController : Controller
    {
        private readonly INotyfService notyfService;
        private readonly INewService newService;

        public NewController(INotyfService notyfService, INewService newService)
        {
            this.notyfService = notyfService;
            this.newService = newService;
        }
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var news = await newService.GetAll();
            var result = news.AsQueryable();
            PagedList<VNew> models = new PagedList<VNew>(result, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        public async Task<IActionResult> Details(int id)
        {
            var news = await newService.GetById(id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] NewViewModel model)
        {
            string token = User.GetSpecificClaim("token");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var result = await newService.Add(model, token);
                if (result.StatusCode == 200)
                {
                    notyfService.Success("Thêm mới thành công!");
                    return RedirectToAction("Index", "New");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(model);
                }
            }
        }
        public async Task<IActionResult> Update(int id)
        {
            var news = await newService.GetById(id);
            return View(news);
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromForm] NewViewModel model)
        {
            string token = User.GetSpecificClaim("token");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var result = await newService.Update(model, token);
                if (result.StatusCode == 200)
                {
                    notyfService.Success("Cập nhật thành công!");
                    return RedirectToAction("Index", "New");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(model);
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            string token = User.GetSpecificClaim("token");

            var result = await newService.Delete(id, token);

            return Json(new { statusCode = result.StatusCode });
        }
    }
}
