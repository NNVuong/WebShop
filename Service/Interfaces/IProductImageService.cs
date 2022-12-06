using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharedObjects.Commons;
using SharedObjects.ValueObjects;
using SharedObjects.ViewModels;

namespace Service.Interfaces
{
    public interface IProductImageService
    {
        Task<ResponseResult> Add(ProductImageViewModel model, string token);
        Task<List<VProductImage>> GetImage(int productId);
        Task<ResponseResult> Delete(int productImageId, string token);
    }
}
