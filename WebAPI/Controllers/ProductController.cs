using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SharedObjects.Commons;
using SharedObjects.Models;
using SharedObjects.ValueObjects;
using SharedObjects.ViewModels;
using WebAPI.Helper;
using WebAPI.StoredProcedure;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly WebShopDbContext context;
        private readonly IUtilities utilities;

        public ProductController(WebShopDbContext context, IUtilities utilities)
        {
            this.context = context;
            this.utilities = utilities;
        }
        [HttpPost("add")]
        [Authorize(AuthenticationSchemes = ("Bearer"))]
        public async Task<IActionResult> Add([FromForm] ProductViewModel model)
        {
            try
            {
                string avatar = "";
                if (model.Avatar != null)
                {
                    string extension = Path.GetExtension(model.Avatar.FileName);
                    string imageName = Guid.NewGuid() + extension;
                    avatar = await utilities.FileUpLoad(model.Avatar, @"product", imageName);
                }
                if (model.Avatar == null) avatar = "default.jpg";
                model.Alias = utilities.SEOUrl(model.ProductName);

                await context.Database.ExecuteSqlRawAsync(ProductSP.SP_Add_Product,model.ProductName,model.ShortDesc,model.Description,model.CatId,model.Price,model.Discount,model.DateCreated,model.BestSellers,model.HomeFlag,model.Title,model.Alias,model.UnitsInStock,avatar);
                return Ok(new ResponseResult(200));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseResult(400));
            }
        }
        [HttpPost("pagination")]
        public async Task<List<VProduct>> GetPagination([FromBody] PaginationViewModel model)
        {
            var result = await context.Set<VProduct>().FromSqlRaw(ProductSP.SP_FindProduct, model.PageIndex, model.PageSize,model.Search,model.CatId,model.sortPrice).AsNoTracking().ToListAsync();
            return result;
        }

        [HttpGet("count")]
        public async Task<int> CountProduct()
        {
            var output = new SqlParameter
            {
                DbType = DbType.Int32,
                Direction = ParameterDirection.Output
            };

            await context.Database.ExecuteSqlRawAsync(ProductSP.SP_Count_Product, output);
            var result = (int)output.Value;
            return result;
        }
        [HttpGet("get-by-id/{productId}")]
        public VProduct GetById(int productId)
        {
            var result = context.Set<VProduct>().FromSqlRaw(ProductSP.SP_Get_Product_By_Id, productId).AsNoTracking().AsEnumerable().FirstOrDefault();
            return result;
        }
        [HttpGet("get-all")]
        public async Task<List<VProduct>> GetAll()
        {
            var result = await context.Set<VProduct>().FromSqlRaw(ProductSP.SP_Get_All).AsNoTracking().ToListAsync();
            return result;
        }

        [HttpDelete("delete/{productId}")]
        [Authorize(AuthenticationSchemes = ("Bearer"))]
        public async Task<IActionResult> Delete(int productId)
        {
            try
            {
                var CurrentImage = GetById(productId).Avatar;
                if (CurrentImage != "default.jpg")
                {
                    utilities.RemoveFile(@"product", CurrentImage);
                }
                await context.Database.ExecuteSqlRawAsync(ProductSP.SP_Delete_Product, productId);
                return Ok(new ResponseResult(200));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseResult(400));
            }
        }
        [HttpPut("update")]
        [Authorize(AuthenticationSchemes = ("Bearer"))]
        public async Task<IActionResult> Update([FromForm] ProductViewModel model)
        {
            try
            {
                var CurrentImage = GetById(model.ProductId).Avatar;
                string avatar = "";
                if (model.Avatar != null)
                {
                    utilities.RemoveFile(@"product", CurrentImage);

                    string extension = Path.GetExtension(model.Avatar.FileName);
                    string imageName = Guid.NewGuid() + extension;
                    avatar = await utilities.FileUpLoad(model.Avatar, @"product", imageName);
                }
                else
                {
                    avatar = CurrentImage;
                }

                model.Alias = utilities.SEOUrl(model.ProductName);
                await context.Database.ExecuteSqlRawAsync(ProductSP.SP_Update_Product, model.ProductId, model.ProductName, model.ShortDesc, model.Description, model.CatId, model.Price, model.Discount, model.BestSellers, model.HomeFlag, model.Title, model.Alias, model.UnitsInStock,avatar);
                return Ok(new ResponseResult(200));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseResult(400));
            }
        }
        [HttpGet("GetProductCategory")]
        public async Task<List<VProductCategory>> GetProductCategory()
        {
            var result = await context.Set<VProductCategory>().FromSqlRaw(ProductSP.SP_Get_All).AsNoTracking().ToListAsync();
            return result;
        }
        [HttpGet("search")]
        public async Task<List<Product>> Search(string keyword)
        {
            var result = await context.Products.Where(x => x.ProductName.Contains(keyword.ToLower().Trim())).ToListAsync();
            return result;
        }
    }
}