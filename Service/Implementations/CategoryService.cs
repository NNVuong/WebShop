using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Service.Interfaces;
using SharedObjects.Commons;
using SharedObjects.ValueObjects;
using SharedObjects.ViewModels;

namespace Service.Implementations
{
    public class CategoryService : BaseService, ICategoryService
    {
        public async Task<ResponseResult> Add(CategoryViewModel model, string token)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            ResponseResult responseResult = new ResponseResult();
            var requestContent = new MultipartFormDataContent();
            if (model.Thumb != null)
            {
                byte[] data;
                using (var br = new BinaryReader(model.Thumb.OpenReadStream()))
                {
                    data = br.ReadBytes((int)model.Thumb.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "Thumb", model.Thumb.FileName);
            }

            requestContent.Add(new StringContent(model.CatName), "CatName");

            using (var response = await httpClient.PostAsync("api/Category/add", requestContent))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }
            return responseResult;
        }

        public async Task<ResponseResult> Delete(int catId, string token)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            ResponseResult responseResult = new ResponseResult();
            using (var response = await httpClient.DeleteAsync("api/Category/delete/" + catId))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }
            return responseResult;
        }

        
        public async Task<ResponseResult> Update(CategoryViewModel model, string token)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            ResponseResult responseResult = new ResponseResult();
            var requestContent = new MultipartFormDataContent();

            if (model.Thumb != null)
            {
                byte[] data;
                using (var br = new BinaryReader(model.Thumb.OpenReadStream()))
                {
                    data = br.ReadBytes((int)model.Thumb.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                requestContent.Add(bytes, "Thumb", model.Thumb.FileName);
            }

            requestContent.Add(new StringContent(model.CatName), "CatName");
            requestContent.Add(new StringContent(model.CatId.ToString()), "CatId");

            using (var response = await httpClient.PutAsync("api/Category/update", requestContent))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }
            return responseResult;
        }
        public async Task<List<VCategory>> GetAll()
        {
            List<VCategory> result = new List<VCategory>();

            using (var response = await httpClient.GetAsync("api/Category/get-all"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<VCategory>>(apiResponse);
            }
            return result;
        }
        public async Task<VCategory> GetById(int id)
        {
            VCategory result = new VCategory();

            using (var response = await httpClient.GetAsync("api/Category/Get-By-Id/" + id))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<VCategory>(apiResponse);
            }
            return result;
        }

        public async Task<List<VCategory>> SearchCategory(string search)
        {
            List<VCategory> result = new List<VCategory>();

            using (var response = await httpClient.GetAsync("api/Category/result/?search=" + search))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<VCategory>>(apiResponse);
            }
            return result;
        }
    }
}
