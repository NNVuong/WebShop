using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharedObjects.Commons;
using SharedObjects.ValueObjects;
using SharedObjects.ViewModels;

namespace Service.Interfaces
{
    public interface INewService
    {
        Task<ResponseResult> Add(NewViewModel model,string token);
        Task<List<VNew>> GetAll();
        Task<VNew> GetById(int postId);
        Task<ResponseResult> Update(NewViewModel model,string token);
        Task<ResponseResult> Delete(int postId,string token);
        Task<List<VNew>> Newfeed();
    }
}
