using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedObjects.Commons;
using SharedObjects.Models;
using SharedObjects.ViewModels;
using WebAPI.Helper;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly IUtilities utilities;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, IUtilities utilities)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.utilities = utilities;
        }
        [HttpPost("add")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromForm] AddUserViewModel model)
        {
            var checkEmailAndUser = await userManager.FindByEmailAsync(model.Email);
            if (checkEmailAndUser != null)
            {
                return BadRequest(new ResponseResult(400, "Email đã tồn tại"));
            }
            checkEmailAndUser = await userManager.FindByNameAsync(model.UserName);
            if (checkEmailAndUser != null)
            {
                return BadRequest(new ResponseResult(400, "Tên tài khoản đã tồn tại"));
            }
            var user = new AppUser();
            user.UserName = model.UserName;
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.Phone;
            user.Address = model.Address;
            user.BirthDay = model.BirthDay;

            if (model.Avatar != null)
            {
                string extension = Path.GetExtension(model.Avatar.FileName);
                string imageName = model.UserName + extension;
                user.Avatar = await utilities.FileUpLoad(model.Avatar, @"account", imageName.ToLower());
                
            }
            if (model.Avatar == null) user.Avatar = "default.jpg";


            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok(new ResponseResult(200));
            }
            else
            {
                return BadRequest(new ResponseResult(400, "Có lỗi xảy ra"));
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var result = await signInManager.PasswordSignInAsync(
                    model.UserNameOrEmail,
                    model.Password,
                    model.RememberMe,
                    false
                );
            if (!result.Succeeded)
            {
                var user = await userManager.FindByEmailAsync(model.UserNameOrEmail);
                if (user == null)
                {
                    return NotFound(new ResponseResult(404, "Tên đăng nhập hoặc email không tồn tại"));
                }
                else
                {
                    result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (!result.Succeeded)
                    {
                        return BadRequest(new ResponseResult(400, "Sai mật khẩu"));
                    }
                    else
                    {
                        //Đăng nhập thành công => tạo ra lần đăng nhập cuối
                        user.LastLogin = DateTime.Now;
                        await userManager.UpdateAsync(user);
                        //


                        var roles = (await userManager.GetRolesAsync(user)).ToList();

                        List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.NameIdentifier,user.Id),
                        new Claim("FullName",user.FullName),
                        new Claim("BirthDay",user.BirthDay.ToString()),
                        new Claim("Address",user.Address),
                        new Claim("PhoneNumber",user.PhoneNumber),
                        new Claim("Email",user.Email),
                        new Claim("Avatar",user.Avatar),
                        new Claim("Id",user.Id),
                        new Claim("Roles", string.Join(",", roles))
                    };
                        var claimIdentity = new ClaimsIdentity(claims);

                        

                        var claimRoles = new List<Claim>();
                        foreach (var item in roles)
                        {
                            claimRoles.Add(new Claim(ClaimTypes.Role, item));
                        }
                        claimIdentity.AddClaims(claimRoles);

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken
                        (
                            configuration["Jwt:Issuer"],
                            configuration["Jwt:Audience"],
                            claimIdentity.Claims,
                            expires: DateTime.UtcNow.AddDays(1),
                            signingCredentials: signIn);
                        string strToken = new JwtSecurityTokenHandler().WriteToken(token);
                        return Ok(new ResponseResult(200, strToken));
                    }
                }
            }
            else
            {
                var user = await userManager.FindByNameAsync(model.UserNameOrEmail);
                if (user == null)
                {
                    return NotFound(new ResponseResult(404, "Tên đăng nhập hoặc email không tồn tại"));
                }
                else
                {
                    result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (!result.Succeeded)
                    {
                        return BadRequest(new ResponseResult(400, "Sai mật khẩu"));
                    }
                    else
                    {
                        //Đăng nhập thành công => tạo ra lần đăng nhập cuối
                        user.LastLogin = DateTime.Now;
                        await userManager.UpdateAsync(user);
                        //


                        var roles = (await userManager.GetRolesAsync(user)).ToList();
                        List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.NameIdentifier,user.Id),
                        new Claim("FullName",user.FullName),
                        new Claim("BirthDay",user.BirthDay.ToString()),
                        new Claim("Address",user.Address),
                        new Claim("PhoneNumber",user.PhoneNumber),
                        new Claim("Avatar",user.Avatar),
                        new Claim("Email",user.Email),
                        new Claim("Id",user.Id),
                        new Claim("Roles", string.Join(",", roles))
                    };
                        var claimIdentity = new ClaimsIdentity(claims);

                        

                        var claimRoles = new List<Claim>();
                        foreach (var item in roles)
                        {
                            claimRoles.Add(new Claim(ClaimTypes.Role, item));
                        }
                        claimIdentity.AddClaims(claimRoles);

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken
                        (
                            configuration["Jwt:Issuer"],
                            configuration["Jwt:Audience"],
                            claimIdentity.Claims,
                            expires: DateTime.UtcNow.AddDays(1),
                            signingCredentials: signIn);
                        string strToken = new JwtSecurityTokenHandler().WriteToken(token);
                        return Ok(new ResponseResult(200, strToken));
                    }
                }
            }
        }


        [HttpPost("ChangePassword")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ChangePasswordProfile([FromBody] ChangePasswordViewModel model)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound(new ResponseResult(404));
            }
            else
            {
                var result = await userManager.ChangePasswordAsync(user, model.PasswordNow, model.Password);
                if (result.Succeeded)
                {
                    return Ok(new ResponseResult(200));
                }
                else
                {
                    return BadRequest(new ResponseResult(400));
                }
            }
        }
        
        [HttpPut("update-profile")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserViewModel model)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound(new ResponseResult(404));
            }
            if (ModelState.IsValid)
            {
                try
                {
                    user.FullName = model.FullName;
                    user.PhoneNumber = model.Phone;
                    user.Address = model.Address;
                    user.BirthDay = model.BirthDay;
                    if (model.Avatar != null)
                    {
                        string extension = Path.GetExtension(model.Avatar.FileName);
                        string imageName = user.UserName + extension;
                        user.Avatar = await utilities.FileUpLoad(model.Avatar, @"account", imageName.ToLower());
                    }
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Ok(new ResponseResult(200));
                    }
                    else
                    {
                        return BadRequest(new ResponseResult(400));
                    }
                }
                catch
                {
                    return BadRequest(new ResponseResult(400));
                }
            }
            return BadRequest(new ResponseResult(400));
        }
        
        [HttpDelete("delete/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new ResponseResult(404));
            }
            else
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok(new ResponseResult(200));
                }
                else
                {
                    return BadRequest(new ResponseResult(400));
                }
            }
        }

        [HttpGet("get-all")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public List<AppUser> GetAll()
        {
            var user = userManager.Users.ToList();
            return user;
        }

        [HttpGet("get-by-Id/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<AppUser> GetById(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            return user;
        }
        [HttpGet("get-user-role/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<string> GetUserRole(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var roleName = (await userManager.GetRolesAsync(user)).ToList().FirstOrDefault();
            return roleName;
        }
        [HttpPost("Register")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            var checkEmailAndUser = await userManager.FindByEmailAsync(model.Email);
            if (checkEmailAndUser != null)
            {
                return BadRequest(new ResponseResult(400, "Email đã tồn tại"));
            }
            checkEmailAndUser = await userManager.FindByNameAsync(model.UserName);
            if (checkEmailAndUser != null)
            {
                return BadRequest(new ResponseResult(400, "Tên tài khoản đã tồn tại"));
            }
            var user = new AppUser();
            user.UserName = model.UserName;
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.Phone;
            user.Address = model.Address;

            if (model.Avatar == null) user.Avatar = "default.jpg";


            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                user.LastLogin = DateTime.Now;
                await userManager.UpdateAsync(user);
                //
                var roles = (await userManager.GetRolesAsync(user)).ToList();

                List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.NameIdentifier,user.Id),
                        new Claim("FullName",user.FullName),
                        new Claim("BirthDay",user.BirthDay.ToString()),
                        new Claim("Address",user.Address),
                        new Claim("PhoneNumber",user.PhoneNumber),
                        new Claim("Email",user.Email),
                        new Claim("Avatar",user.Avatar),
                        new Claim("Id",user.Id),
                        new Claim("Roles", string.Join(",", roles))
                    };
                var claimIdentity = new ClaimsIdentity(claims);
                var claimRoles = new List<Claim>();
                foreach (var item in roles)
                {
                    claimRoles.Add(new Claim(ClaimTypes.Role, item));
                }
                claimIdentity.AddClaims(claimRoles);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken
                (
                    configuration["Jwt:Issuer"],
                    configuration["Jwt:Audience"],
                    claimIdentity.Claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: signIn);
                string strToken = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new ResponseResult(200, strToken));
            }
            else
            {
                return BadRequest(new ResponseResult(400, "Có lỗi xảy ra"));
            }
        }
    }
}