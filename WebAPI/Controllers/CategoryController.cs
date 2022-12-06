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
    public class CategoryController : ControllerBase
    {
        private readonly WebShopDbContext context;
        private readonly IUtilities utilities;

        public CategoryController(WebShopDbContext context, IUtilities utilities)
        {
            this.context = context;
            this.utilities = utilities;
        }
        [HttpPost("add")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Add([FromForm] CategoryViewModel model)
        {
            try
            {
                var fthumb = "";
                if (model.Thumb != null)
                {
                    string extension = Path.GetExtension(model.Thumb.FileName);
                    string imageName = utilities.SEOUrl(model.CatName) + extension;
                    fthumb = await utilities.FileUpLoad(model.Thumb, @"category", imageName.ToLower());

                }
                if (model.Thumb == null) fthumb = "default.jpg";
                model.Alias = utilities.SEOUrl(model.CatName);
                await context.Database.ExecuteSqlRawAsync(CategorySP.SP_Add_Category, model.CatName, model.Alias, fthumb);
                return Ok(new ResponseResult(200));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseResult(400));
            }
        }
        [HttpPut("update")]
        [Authorize(AuthenticationSchemes = ("Bearer"))]
        public async Task<IActionResult> Update([FromForm] CategoryViewModel model)
        {
            try
            {
                var CurrentImage = GetById(model.CatId).Thumb;
                string fthumb = "";
                if (model.Thumb != null)
                {
                    utilities.RemoveFile(@"category", CurrentImage);

                    string extension = Path.GetExtension(model.Thumb.FileName);
                    string imageName = utilities.SEOUrl(model.CatName) + extension;
                    fthumb = await utilities.FileUpLoad(model.Thumb, @"category", imageName.ToLower());
                }
                else
                {
                    fthumb = CurrentImage;
                }
                model.Alias = utilities.SEOUrl(model.CatName);
                await context.Database.ExecuteSqlRawAsync(CategorySP.SP_Update_Category,model.CatId, model.CatName, model.Alias,fthumb);
                return Ok(new ResponseResult(200));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseResult(400));
            }
        }
        [HttpDelete("delete/{catId}")]
        [Authorize(AuthenticationSchemes = ("Bearer"))]
        public async Task<IActionResult> Delete(int catId)
        {
            try
            {
                var CurrentImage = GetById(catId).Thumb;
                if (CurrentImage != "default.jpg")
                {
                    utilities.RemoveFile(@"category", CurrentImage);
                }
                await context.Database.ExecuteSqlRawAsync(CategorySP.SP_Delete_Category, catId);
                return Ok(new ResponseResult(200));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseResult(400));
            }
        }

        [HttpGet("Get-By-Id/{id}")]
        public VCategory GetById(int id)
        {
            var result = context.Set<VCategory>().FromSqlRaw(CategorySP.SP_Get_Category_By_Id, id).AsNoTracking().AsEnumerable().FirstOrDefault();
            return result;
        }

        [HttpGet("get-all")]
        public async Task<List<VCategory>> GetAll()
        {
            var result = await context.Set<VCategory>().FromSqlRaw(CategorySP.SP_Get_All).AsNoTracking().ToListAsync();
            return result;
        }

        [HttpGet("result")]
        public async Task<List<VCategory>> SearchCategory(string search)
        {
            var result = await context.Set<VCategory>().FromSqlRaw(CategorySP.SP_Get_All).AsNoTracking().ToListAsync();
            if (!string.IsNullOrEmpty(search))
            {
                result = result.Where(c => c.CatName.ToLower().Contains(search)).ToList();
            }
            return result;
        }
    }
}