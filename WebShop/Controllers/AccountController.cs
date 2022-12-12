using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using PagedList.Core;
using Service.Interfaces;
using SharedObjects.Commons;
using SharedObjects.ValueObjects;
using SharedObjects.ViewModels;

namespace WebShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IAccountService accountService;
        public readonly INotyfService notyfService;
        private readonly IOrderService orderService;

        public AccountController(IConfiguration configuration, IAccountService accountService, INotyfService notyfService, IOrderService orderService)
        {
            this.configuration = configuration;
            this.accountService = accountService;
            this.notyfService = notyfService;
            this.orderService = orderService;
        }
        [Authorize]
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var user = User.GetSpecificClaim("Id");
            string token = User.GetSpecificClaim("token");
            var orders = await orderService.GetListOrders(user);
            var result = orders.AsQueryable();
            var userInfo = await accountService.GetById(user, token);
            ViewBag.userInfo = userInfo;
            PagedList<VOrder> models = new PagedList<VOrder>(result, pageNumber, pageSize);
            return View(models);
        }
        [Route("dang-ky.html", Name = "DangKy")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Route("dang-ky.html", Name = "DangKy")]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var result = await accountService.Register(model);
                if (result.StatusCode == 200)
                {
                    var userPrincipal = ValidateToken(result.Message);
                    var authProperty = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
                        IsPersistent = false
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperty);
                    return Redirect("/Account/Index");
                }
                else
                {
                    notyfService.Error("Đăng ký không thành công!");
                    //ModelState.AddModelError(string.Empty, result.Message);
                    return View(model);
                }
            }
        }
        [Route("dang-nhap.html", Name = "DangNhap")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
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
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }
            else
            {
                notyfService.Error("Sai tên đăng nhập hoặc mật khẩu!");
                //ModelState.AddModelError(string.Empty, "Sai tên đăng nhập hoặc mật khẩu!");
                return View(model);
            }
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            string token = User.GetSpecificClaim("token");
            if (!ModelState.IsValid)
            {
                
                notyfService.Warning("Đổi mật khẩu thất bại!");
                if(model.Password != model.ConfirmPassword)
                {
                    notyfService.Warning("Nhập lại mật khẩu không chính xác!");
                }
                if (model.Password.Length < 6)
                {
                    notyfService.Warning("Mật khẩu tối thiểu 5 ký tự!");
                }
                return RedirectToAction("Index", "Account");
            }
            else
            {
                var result = await accountService.ChangePassword(model, token);
                if (result.StatusCode == 200)
                {
                    notyfService.Success("Đổi mật khẩu thành công!");
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    notyfService.Warning("Đổi mật khẩu thất bại!");
                    return RedirectToAction("Index", "Account");
                }
            }
        }

        public IActionResult ForgetPassword()
        {
            string token = User.GetSpecificClaim("token");
            //ViewBag.UserId = UserId;
            ViewBag.Token = token;
            return View();
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
            return RedirectToAction("Index", "Home");
        }
    }
}