using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class ProductImageController : ControllerBase
    {
        private readonly WebShopDbContext context;
        private readonly IUtilities utilities;

        public ProductImageController(WebShopDbContext context, IUtilities utilities)
        {
            this.context = context;
            this.utilities = utilities;
        }
        [HttpPost("add")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Add([FromForm] ProductImageViewModel model)
        {
            try
            {
                foreach (IFormFile f in model.ImageName)
                {
                    var fthumb = f.ToString();
                    string extension = Path.GetExtension(f.FileName);
                    string imageName = Guid.NewGuid().ToString() + extension;
                    fthumb = await utilities.FileUpLoad(f, @"product", imageName.ToLower());
                    await context.Database.ExecuteSqlRawAsync(ProductImageSP.SP_UpLoad_ProductImage, model.ProductId, fthumb);
                }
                return Ok(new ResponseResult(200));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseResult(400));
            }
        }

        [HttpGet("Get-By-Id/{id}")]
        public VProductImage GetById(int id)
        {
            var result = context.Set<VProductImage>().FromSqlRaw(ProductImageSP.SP_Get_ProductImage_By_Id, id).AsNoTracking().AsEnumerable().FirstOrDefault();
            return result;
        }

        [HttpGet("Get-Image/{productId}")]
        public async Task<List<VProductImage>> GetImage(int productId)
        {
            var result = await context.Set<VProductImage>().FromSqlRaw(ProductImageSP.SP_Get_ProductImage, productId).AsNoTracking().ToListAsync();
            return result;
        }
        [HttpDelete("delete/{productImageId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Delete(int productImageId)
        {
            try
            {
                var CurrentImage = GetById(productImageId).ImageName.ToString();
                utilities.RemoveFile(@"product", CurrentImage);

                await context.Database.ExecuteSqlRawAsync(ProductImageSP.SP_Delete_ProductImage, productImageId);
                return Ok(new ResponseResult(200));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseResult(400));
            }
        }
    }
}