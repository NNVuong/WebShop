using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Interfaces;
using SharedObjects.Commons;
using SharedObjects.Models;
using SharedObjects.ViewModels;


namespace Service.Implementations
{
    public class AccountService : BaseService, IAccountService
    {
        public async Task<ResponseResult> Add(AddUserViewModel model,string token)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            ResponseResult responseResult = new ResponseResult();

            var requestContent = new MultipartFormDataContent();
            if (model.Avatar != null)
            {
                byte[] data;
                using (var br = new BinaryReader(model.Avatar.OpenReadStream()))
                {
                    data = br.ReadBytes((int)model.Avatar.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "Avatar", model.Avatar.FileName);
            }
            requestContent.Add(new StringContent(model.UserName.ToString()), "UserName");
            requestContent.Add(new StringContent(model.FullName.ToString()), "FullName");
            requestContent.Add(new StringContent(model.Email.ToString()), "Email");
            requestContent.Add(new StringContent(model.Phone.ToString()), "Phone");
            requestContent.Add(new StringContent(model.Address.ToString()), "Address");
            requestContent.Add(new StringContent(model.BirthDay.ToString()), "BirthDay");
            requestContent.Add(new StringContent(model.Password.ToString()), "Password");
            requestContent.Add(new StringContent(model.ConfirmPassword.ToString()), "ConfirmPassword");


            using (var response = await httpClient.PostAsync("api/account/add", requestContent))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }

            return responseResult;
        }

        public async Task<ResponseResult> Register(RegisterViewModel model)
        {
            ResponseResult responseResult = new ResponseResult();

            var requestContent = new MultipartFormDataContent();
            requestContent.Add(new StringContent(model.UserName.ToString()), "UserName");
            requestContent.Add(new StringContent(model.FullName.ToString()), "FullName");
            requestContent.Add(new StringContent(model.Email.ToString()), "Email");
            requestContent.Add(new StringContent(model.Phone.ToString()), "Phone");
            requestContent.Add(new StringContent(model.Address.ToString()), "Address");
            requestContent.Add(new StringContent(model.Password.ToString()), "Password");
            requestContent.Add(new StringContent(model.ConfirmPassword.ToString()), "ConfirmPassword");


            using (var response = await httpClient.PostAsync("api/account/register", requestContent))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }

            return responseResult;
        }

        public async Task<ResponseResult> Delete(string userId, string token)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            ResponseResult responseResult = new ResponseResult();
            using (var response = await httpClient.DeleteAsync("api/account/delete/" + userId))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }
            return responseResult;
        }


        public async Task<List<AppUser>> GetAll(string token)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            List<AppUser> result = new List<AppUser>();

            using (var response = await httpClient.GetAsync("api/account/get-all"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<AppUser>>(apiResponse);
            }
            return result;
        }

        public async Task<AppUser> GetById(string userId, string token)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            AppUser result = new AppUser();

            using (var response = await httpClient.GetAsync("api/account/get-by-Id/" + userId))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<AppUser>(apiResponse);
            }
            return result;
        }

        public async Task<ResponseResult> Login(LoginViewModel model)
        {

            ResponseResult responseResult = new ResponseResult();
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            using (var response = await httpClient.PostAsync("api/account/login", content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }
            return responseResult;
        }

        public async Task<ResponseResult> UpdateProfile(UpdateUserViewModel model, string token)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            ResponseResult responseResult = new ResponseResult();

            var requestContent = new MultipartFormDataContent();
            if (model.Avatar != null)
            {
                byte[] data;
                using (var br = new BinaryReader(model.Avatar.OpenReadStream()))
                {
                    data = br.ReadBytes((int)model.Avatar.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "Avatar", model.Avatar.FileName);
            }
            requestContent.Add(new StringContent(model.FullName.ToString()), "FullName");
            requestContent.Add(new StringContent(model.Phone.ToString()), "Phone");
            requestContent.Add(new StringContent(model.Address.ToString()), "Address");
            requestContent.Add(new StringContent(model.BirthDay.ToString()), "BirthDay");

            using (var response = await httpClient.PutAsync("api/account/update-profile", requestContent))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }
            return responseResult;
        }
        public async Task<ResponseResult> ChangePassword(ChangePasswordViewModel model, string token)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            ResponseResult responseResult = new ResponseResult();
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            using (var response = await httpClient.PostAsync("api/account/ChangePassword", content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }
            return responseResult;
        }

        public async Task<ResponseResult> ResetPassword(ResetPasswordViewModel model)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + model.token);
            ResponseResult responseResult = new ResponseResult();

            var requestContent = new MultipartFormDataContent();
            
            requestContent.Add(new StringContent(model.UserId.ToString()), "UserId");
            requestContent.Add(new StringContent(model.Password.ToString()), "Password");
            requestContent.Add(new StringContent(model.ConfirmPassword.ToString()), "ConfirmPassword");
            requestContent.Add(new StringContent(model.token.ToString()), "token");
            requestContent.Add(new StringContent(model.ByPass.ToString()), "ByPass");

            using (var response = await httpClient.PutAsync("api/account/ResetPassword", requestContent))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }
            return responseResult;
        }


        public async Task<string> GetUserRole(string userId, string token)
        {
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await httpClient.GetAsync("api/account/get-user-role/" + userId);
            string apiResponse = await response.Content.ReadAsStringAsync();
            return apiResponse;
        }

        
    }
}