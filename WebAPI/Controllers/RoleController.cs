using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;

        public RoleController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpPost("add")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] RoleViewModel model)
        {
            var checkRole = await roleManager.FindByNameAsync(model.RoleName);
            if (checkRole != null)
            {
                return BadRequest(new ResponseResult(400, "Quyền đã tồn tại"));
            }

            var role = new AppRole();
            role.Name = model.RoleName;
            role.Description = model.Description;
            
            var result = await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return Ok(new ResponseResult(200));
            }
            else
            {
                return BadRequest(new ResponseResult(400, "Có lỗi xảy ra"));
            }
        }

        [HttpPut("update")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] RoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                return NotFound(new ResponseResult(404));
            }
            else
            {
                role.Name = model.RoleName;
                role.Description = model.Description;
                var result = await roleManager.UpdateAsync(role);
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
        [HttpDelete("delete/{roleId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound(new ResponseResult(404));
            }
            else
            {
                var result = await roleManager.DeleteAsync(role);
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
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public List<AppRole> GetAll()
        {
            var roles = roleManager.Roles.ToList();
            return roles;
        }

        [HttpGet("get-by-id/{roleId}")]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<AppRole> GetById(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            return role;
        }
        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AssignRole([FromBody] UserRoleViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            var result = await userManager.AddToRoleAsync(user, model.RoleName);
            if (result.Succeeded)
            {
                return Ok(new ResponseResult(200));
            }
            else
            {
                return BadRequest(new ResponseResult(400));
            }
        }
        [HttpPost("remove-role")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveRole([FromBody] UserRoleViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            var result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
            if (result.Succeeded)
            {
                return Ok(new ResponseResult(200));
            }
            else
            {
                return BadRequest(new ResponseResult(400));
            }
        }

        [HttpGet("get-user-in-role/{roleName}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<List<AppUser>> GetUserInRole(string roleName)
        {
            var users = (await userManager.GetUsersInRoleAsync(roleName)).ToList();

            return users;
        }
        [HttpGet("get-user-not-in-role/{roleName}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<List<AppUser>> GetUserNotInRole(string roleName)
        {
            var users = userManager.Users.ToList();
            var usersNotInRole = new List<AppUser>();
            foreach (var item in users)
            {
                if (! await userManager.IsInRoleAsync(item,roleName))
                {
                    usersNotInRole.Add(item);
                }
            }
            return usersNotInRole;
        }

    }
}