using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using SharedObjects.ViewModels;
using WebShop.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebShop.Controllers
{
    public class WishListController : Controller
    {
        private readonly IProductService productService;
        public readonly INotyfService notyfService;

        public WishListController(IProductService productService, INotyfService notyfService)
        {
            this.productService = productService;
            this.notyfService = notyfService;
        }
        public List<WishList> WishList
        {
            get
            {
                var gh = HttpContext.Session.Get<List<WishList>>("WishList");
                if (gh == default(List<WishList>))
                {
                    gh = new List<WishList>();
                }
                return gh;
            }
        }
        [HttpPost]
        [Route("api/wishlist/add")]
        public async Task<IActionResult> AddToWishList(int productID)
        {
            List<WishList> wishList = WishList;

            try
            {
                //Them san pham vao gio hang
                WishList item = wishList.SingleOrDefault(p => p.product.ProductId == productID);
                if (item != null)
                {
                    //luu lai session
                    HttpContext.Session.Set<List<WishList>>("WishList", wishList);
                }
                else
                {
                    var hh = await productService.GetById(productID);
                    item = new WishList
                    {
                        product = hh
                    };
                    wishList.Add(item);//Them vao danh sach
                }
                //Luu lai Session
                HttpContext.Session.Set<List<WishList>>("WishList", wishList);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        [Route("api/wishlist/remove")]
        public ActionResult Remove(int productID)
        {
            try
            {
                List<WishList> wishList = WishList;
                WishList item = wishList.SingleOrDefault(p => p.product.ProductId == productID);
                if (item != null)
                {
                    wishList.Remove(item);
                }
                //luu lai session
                HttpContext.Session.Set<List<WishList>>("WishList", wishList);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
        public IActionResult Index()
        {
            return View(WishList);
        }
    }
}
