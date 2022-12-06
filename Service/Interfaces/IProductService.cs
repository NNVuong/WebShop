using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharedObjects.Commons;
using SharedObjects.Models;
using SharedObjects.ValueObjects;
using SharedObjects.ViewModels;

namespace Service.Interfaces
{
    public interface IProductService
    {
        Task<ResponseResult> Add(ProductViewModel model, string token);
        Task<List<VProduct>> GetPagination(PaginationViewModel model);
        Task<int> CountProduct();
        Task<VProduct> GetById(int productId);
        Task<List<VProduct>> GetAll();
        Task<ResponseResult> Update(ProductViewModel model, string token);
        Task<ResponseResult> Delete(int productId, string token);
        Task<List<VProductCategory>> GetProductCategory();
        Task<List<Product>> Search(string keyword);
    }
}
