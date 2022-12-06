using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Service.Interfaces;
using SharedObjects.Commons;
using SharedObjects.Models;
using SharedObjects.ValueObjects;
using SharedObjects.ViewModels;

namespace Service.Implementations
{
    public class ProductService : BaseService, IProductService
    {
        public async Task<ResponseResult> Add(ProductViewModel model, string token)
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
            requestContent.Add(new StringContent(model.ProductName), "ProductName");
            requestContent.Add(new StringContent(model.ShortDesc), "ShortDesc");
            requestContent.Add(new StringContent(model.Description), "Description");
            requestContent.Add(new StringContent(model.CatId.ToString()), "CatId");
            requestContent.Add(new StringContent(model.Price.ToString()), "Price");
            requestContent.Add(new StringContent(model.Discount.ToString()), "Discount");
            requestContent.Add(new StringContent(model.BestSellers.ToString()), "BestSellers");
            requestContent.Add(new StringContent(model.HomeFlag.ToString()), "HomeFlag");
            requestContent.Add(new StringContent(model.Title), "Title");
            requestContent.Add(new StringContent(model.UnitsInStock.ToString()), "UnitsInStock");


            using (var response = await httpClient.PostAsync("api/product/add", requestContent))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }
            return responseResult;
        }


        public async Task<ResponseResult> Update(ProductViewModel model, string token)
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
            requestContent.Add(new StringContent(model.ProductId.ToString()), "ProductId");
            requestContent.Add(new StringContent(model.ProductName), "ProductName");
            requestContent.Add(new StringContent(model.ShortDesc), "ShortDesc");
            requestContent.Add(new StringContent(model.Description), "Description");
            requestContent.Add(new StringContent(model.CatId.ToString()), "CatId");
            requestContent.Add(new StringContent(model.Price.ToString()), "Price");
            requestContent.Add(new StringContent(model.Discount.ToString()), "Discount");
            requestContent.Add(new StringContent(model.BestSellers.ToString()), "BestSellers");
            requestContent.Add(new StringContent(model.HomeFlag.ToString()), "HomeFlag");
            requestContent.Add(new StringContent(model.Title), "Title");
            requestContent.Add(new StringContent(model.UnitsInStock.ToString()), "UnitsInStock");

            using (var response = await httpClient.PutAsync("api/product/update", requestContent))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }
            return responseResult;
        }


        public async Task<int> CountProduct()
        {
            int result = 0;

            using (var response = await httpClient.GetAsync("api/Product/count"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<int>(apiResponse);
            }
            return result;
        }

        public async Task<List<VProduct>> GetPagination(PaginationViewModel model)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            List<VProduct> responseResult = new List<VProduct>();

            using (var response = await httpClient.PostAsync("api/Product/pagination/", content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<List<VProduct>>(apiResponse);
            }
            return responseResult;
        }
        public async Task<VProduct> GetById(int productId)
        {
            VProduct result = new VProduct();

            using (var response = await httpClient.GetAsync("api/product/Get-By-Id/" + productId))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<VProduct>(apiResponse);
            }
            return result;
        }
        public async Task<List<VProduct>> GetAll()
        {
            List<VProduct> result = new List<VProduct>();

            using (var response = await httpClient.GetAsync("api/product/get-all"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<VProduct>>(apiResponse);
            }
            return result;
        }
        public async Task<ResponseResult> Delete(int productId, string token)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            ResponseResult responseResult = new ResponseResult();
            using (var response = await httpClient.DeleteAsync("api/product/delete/" + productId))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }
            return responseResult;
        }
        public async Task<List<VProductCategory>> GetProductCategory()
        {
            List<VProductCategory> result = new List<VProductCategory>();

            using (var response = await httpClient.GetAsync("api/product/GetProductCategory"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<VProductCategory>>(apiResponse);
            }
            return result;
        }

        public async Task<List<Product>> Search(string keyword)
        {
            List<Product> result = new List<Product>();

            using (var response = await httpClient.GetAsync($"api/product/search?keyword={Uri.EscapeUriString(keyword)}"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
            }
            return result;
        }
    }
}
