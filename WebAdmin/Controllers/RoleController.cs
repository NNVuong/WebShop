using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service.Interfaces;
using SharedObjects.Commons;
using SharedObjects.ViewModels;

namespace WebAdmin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IRoleService roleService;
        private readonly INotyfService notyfService;

        public RoleController(IConfiguration configuration, IRoleService roleService, INotyfService notyfService)
        {
            this.configuration = configuration;
            this.roleService = roleService;
            this.notyfService = notyfService;
        }
        public async Task<IActionResult> Role()
        {
            string token = User.GetSpecificClaim("token");
            var roles = await roleService.GetAll(token);
            return View(roles);
        }
        public async Task<IActionResult> Save([FromBody] RoleViewModel model)
        {
            string token = User.GetSpecificClaim("token");
            var result = new ResponseResult();
            if (string.IsNullOrEmpty(model.RoleId))
            {
                
                result = await roleService.Add(model, token);
                
            }
            else
            {
                
                result = await roleService.Update(model, token);
                
            }
            return Json(new { statusCode = result.StatusCode });
        }
        public async Task<IActionResult> GetById(string roleId)
        {
            string token = User.GetSpecificClaim("token");
            var role = await roleService.GetById(roleId, token);
            return Json(new { result = role });
        }

        public async Task<IActionResult> Delete(string roleId)
        {
            string token = User.GetSpecificClaim("token");
            
            var result = await roleService.Delete(roleId, token);
            
            return Json(new { statusCode = result.StatusCode });
        }

        public async Task<IActionResult> ManageRole(string roleName)
        {
            string token = User.GetSpecificClaim("token");
            var usersInRole = await roleService.GetUserInRole(roleName, token);
            ViewBag.usersInRole = usersInRole;
            var usersNotInRole = await roleService.GetUserNotInRole(roleName, token);
            ViewBag.usersNotInRole = usersNotInRole;
            ViewBag.roleName = roleName;
            return View();
        }
        public async Task<IActionResult> RemoveRole([FromBody] UserRoleViewModel model)
        {
            string token = User.GetSpecificClaim("token");
            var result = await roleService.RemoveRole(model, token);
            return Json(new { statusCode = result.StatusCode });
        }
        public async Task<IActionResult> AssignRole([FromBody] UserRoleViewModel model)
        {
            string token = User.GetSpecificClaim("token");
            var result = await roleService.AssignRole(model, token);
            return Json(new { statusCode = result.StatusCode });
        }
    }
}
