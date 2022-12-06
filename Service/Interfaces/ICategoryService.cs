using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharedObjects.Commons;
using SharedObjects.ValueObjects;
using SharedObjects.ViewModels;

namespace Service.Interfaces
{
    public interface ICategoryService
    {
        Task<ResponseResult> Add(CategoryViewModel model, string token);
        Task<ResponseResult> Update(CategoryViewModel model, string token);
        Task<ResponseResult> Delete(int catId, string token);
        Task<VCategory> GetById(int id);
        Task<List<VCategory>> GetAll();
        Task<List<VCategory>> SearchCategory(string search);
    }
}
