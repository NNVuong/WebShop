using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Service.Interfaces;
using SharedObjects.Commons;
using SharedObjects.ValueObjects;
using SharedObjects.ViewModels;

namespace Service.Implementations
{
    public class NewService : BaseService, INewService
    {
        public async Task<ResponseResult> Add(NewViewModel model, string token)
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
            requestContent.Add(new StringContent(model.Title), "Title");
            requestContent.Add(new StringContent(model.Contents), "Contents");
            requestContent.Add(new StringContent(model.UserId), "UserId");
            requestContent.Add(new StringContent(model.IsNewFeed.ToString()), "IsNewFeed");
            using (var response = await httpClient.PostAsync("api/new/add", requestContent))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }
            return responseResult;
        }

        public async Task<ResponseResult> Delete(int postId,string token)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            ResponseResult responseResult = new ResponseResult();
            using (var response = await httpClient.DeleteAsync("api/new/delete/" + postId))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }
            return responseResult;
        }

        public async Task<List<VNew>> GetAll()
        {
            List<VNew> result = new List<VNew>();

            using (var response = await httpClient.GetAsync("api/new/get-all"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<VNew>>(apiResponse);
            }
            return result;
        }

        public async Task<VNew> GetById(int postId)
        {
            VNew result = new VNew();

            using (var response = await httpClient.GetAsync("api/new/get-by-id/" + postId))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<VNew>(apiResponse);
            }
            return result;
        }

        public async Task<List<VNew>> Newfeed()
        {
            List<VNew> result = new List<VNew>();

            using (var response = await httpClient.GetAsync("api/new/newfeed"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<VNew>>(apiResponse);
            }
            return result;
        }

        public async Task<ResponseResult> Update(NewViewModel model, string token)
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
            requestContent.Add(new StringContent(model.Title), "Title");
            requestContent.Add(new StringContent(model.Contents), "Contents");
            requestContent.Add(new StringContent(model.PostId.ToString()), "PostId");
            requestContent.Add(new StringContent(model.IsNewFeed.ToString()), "IsNewFeed");
            using (var response = await httpClient.PutAsync("api/new/update", requestContent))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseResult = JsonConvert.DeserializeObject<ResponseResult>(apiResponse);
            }
            return responseResult;
        }
    }
}
