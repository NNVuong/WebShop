using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SharedObjects.Commons;
using SharedObjects.Models;
using SharedObjects.ViewModels;


namespace Service.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseResult> Add(AddUserViewModel model, string token);
        Task<ResponseResult> Register(RegisterViewModel model);
        Task<ResponseResult> Login(LoginViewModel model);
        Task<ResponseResult> ChangePassword(ChangePasswordViewModel model, string token);

        Task<ResponseResult> ResetPassword(ResetPasswordViewModel model);


        Task<ResponseResult> UpdateProfile(UpdateUserViewModel model, string token);
        Task<ResponseResult> Delete(string userId, string token);

        Task<List<AppUser>> GetAll(string token);
        Task<AppUser> GetById(string userId, string token);
        Task<string> GetUserRole(string userId, string token);
    }
}