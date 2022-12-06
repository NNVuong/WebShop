using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using SharedObjects.Commons;
using SharedObjects.ViewModels;
using Service.Interfaces;
using Microsoft.AspNetCore.Http;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using PagedList.Core;
using SharedObjects.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAdmin.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IAccountService accountService;
        private readonly INotyfService notyfService;
        private readonly IRoleService roleService;

        public AccountController(IConfiguration configuration, IAccountService accountService, INotyfService notyfService, IRoleService roleService)
        {
            this.configuration = configuration;
            this.accountService = accountService;
            this.notyfService = notyfService;
            this.roleService = roleService;
        }
        public async Task<IActionResult> Index(int? page)
        {
            string token = User.GetSpecificClaim("token");
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var users = await accountService.GetAll(token);
            var result = users.AsQueryable();
            PagedList<AppUser> models = new PagedList<AppUser>(result, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromForm]AddUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                string token = User.GetSpecificClaim("token");
                var result = await accountService.Add(model,token);
                if (result.StatusCode == 200)
                {
                    notyfService.Success("Thêm mới thành công!");
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(model);
                }
            }
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await accountService.Login(model);
            if (result.StatusCode == 200)
            {
                var userPrincipal = ValidateToken(result.Message);
                var authProperty = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
                    IsPersistent = model.RememberMe
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperty);
                notyfService.Success("Xin chào: " + userPrincipal.GetSpecificClaim("FullName"));
                return Redirect("/Home/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sai tên đăng nhập hoặc mật khẩu!");
                return View(model);
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }
            string token = User.GetSpecificClaim("token");
            var user = await accountService.GetById(userId, token);
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirm(string userId)
        {
            string token = User.GetSpecificClaim("token");
            var result = await accountService.Delete(userId, token);
            if (result.StatusCode == 200)
            {
                notyfService.Warning("Xóa thành công");
                return Redirect("/Account/Index");
            }
            else
            {
                notyfService.Warning("Xóa không thành công");
                return Redirect("/Account/Index");
            }
        }

        public IActionResult UpdateProfile()
        {
            return View();
        }
        [HttpPost, ActionName("UpdateProfile")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserViewModel model)
        {
            string token = User.GetSpecificClaim("token");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var result = await accountService.UpdateProfile(model, token);
                if (result.StatusCode == 200)
                {
                    notyfService.Success("Cập nhật thành công!");
                    return RedirectToAction("UpdateProfile", "Account");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(model);
                }
            }
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            string token = User.GetSpecificClaim("token");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var result = await accountService.ChangePassword(model, token);
                if (result.StatusCode == 200)
                {
                    notyfService.Success("Đổi mật khẩu thành công!");
                    return RedirectToAction("ChangePassword", "Account");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(model);
                }
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Detail(string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }
            string token = User.GetSpecificClaim("token");
            var user = await accountService.GetById(userId, token);
            var role = await accountService.GetUserRole(userId, token);
            ViewBag.role = role;
            return View(user);
        }

        

        private ClaimsPrincipal ValidateToken(string token)
        {
            IdentityModelEventSource.ShowPII = true;
            SecurityToken validateToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();
            validationParameters.ValidateLifetime = true;
            validationParameters.ValidAudience = configuration["Jwt:Audience"];
            validationParameters.ValidIssuer = configuration["Jwt:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out validateToken);
            var claims = new[] { new Claim("token", token), };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            principal.AddIdentity(claimsIdentity);
            return principal;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
