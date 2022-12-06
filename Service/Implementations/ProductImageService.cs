using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Service.Interfaces;
using SharedObjects.Commons;
using SharedObjects.ValueObjects;
using SharedObjects.ViewModels;

namespace Service.Implementations
{
    public class ProductImageService : BaseService, IProductImageService
    {
        public async Task<ResponseResult> Add(ProductImageViewModel model, string token)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            ResponseResult responseResult = new ResponseResult();
            var requestContent = new MultipartFormDataContent();
            foreach (IFormFile f in model.ImageName)
            {
                byte[] data;
                using (var br = new BinaryReader(f.OpenReadStream()))
                {
                    data = br.ReadBytes((int)f.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "ImageName", f.FileName);
            }
            requestContent.Add(new StringContent(model.ProductId.ToString()), "ProductId");

            using (var response = await httpClient.PostAsync("api/productimage/add", requestContent))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }
            return responseResult;
        }

        public async Task<ResponseResult> Delete(int productImageId, string token)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            ResponseResult responseResult = new ResponseResult();
            using (var response = await httpClient.DeleteAsync("api/productimage/delete/" + productImageId))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }
            return responseResult;
        }

        public async Task<List<VProductImage>> GetImage(int productId)
        {
            List<VProductImage> result = new List<VProductImage>();

            using (var response = await httpClient.GetAsync("api/productimage/Get-Image/" + productId))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<VProductImage>>(apiResponse);
            }
            return result;
        }
    }
}
