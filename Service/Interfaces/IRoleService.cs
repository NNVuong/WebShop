using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SharedObjects.Commons;
using SharedObjects.Models;
using SharedObjects.ViewModels;

namespace Service.Interfaces
{
    public interface IRoleService
    {
        Task<ResponseResult> Add(RoleViewModel model, string token);
        Task<ResponseResult> Update(RoleViewModel model, string token);
        Task<ResponseResult> Delete(string roleId, string token);
        Task<List<AppRole>> GetAll(string token);
        Task<AppRole> GetById(string roleId, string token);
        Task<ResponseResult> AssignRole(UserRoleViewModel model, string token);
        Task<ResponseResult> RemoveRole(UserRoleViewModel model, string token);
        Task<List<AppUser>> GetUserInRole(string roleName,string token);
        Task<List<AppUser>> GetUserNotInRole(string roleName,string token);
    }
}
