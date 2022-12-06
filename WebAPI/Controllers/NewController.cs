using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class NewController : ControllerBase
    {
        private readonly WebShopDbContext context;
        private readonly IUtilities utilities;

        public NewController(WebShopDbContext context, IUtilities utilities)
        {
            this.context = context;
            this.utilities = utilities;
        }
        [HttpPost("add")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Add([FromForm] NewViewModel model)
        {
            try
            {
                var fthumb = "";
                if (model.Thumb != null)
                {
                    string extension = Path.GetExtension(model.Thumb.FileName);
                    string imageName = Guid.NewGuid().ToString() + extension;
                    fthumb = await utilities.FileUpLoad(model.Thumb, @"new", imageName.ToLower());
                }
                if (model.Thumb == null) fthumb = "default.jpg";
                model.Alias = utilities.SEOUrl(model.Title);
                await context.Database.ExecuteSqlRawAsync(NewSP.SP_Add_New, model.Title, model.Contents, fthumb,model.Alias,model.UserId,model.IsNewFeed);
                return Ok(new ResponseResult(200));
            }
            catch
            {
                return BadRequest(new ResponseResult(400));
            }
        }
        [HttpGet("get-all")]
        public async Task<List<VNew>> GetAll()
        {
            var result = await context.Set<VNew>().FromSqlRaw(NewSP.SP_Get_New).AsNoTracking().ToListAsync();
            return result;
        }
        [HttpGet("newfeed")]
        public async Task<List<VNew>> Newfeed(int? postId, bool isNewfeed = true)
        {
            var result = await context.Set<VNew>().FromSqlRaw(NewSP.SP_Newfeed,postId,isNewfeed).AsNoTracking().ToListAsync();
            return result;
        }
        [HttpGet("get-by-id/{postId}")]
        public VNew GetById(int postId)
        {
            var result = context.Set<VNew>().FromSqlRaw(NewSP.SP_Get_New_By_Id, postId).AsNoTracking().AsEnumerable().FirstOrDefault();
            return result;
        }
        [HttpPut("update")]
        [Authorize(AuthenticationSchemes = ("Bearer"))]
        public async Task<IActionResult> Update([FromForm] NewViewModel model)
        {
            try
            {
                var CurrentImage = GetById(model.PostId).Thumb;
                string thumb = "";
                if (model.Thumb != null)
                {
                    utilities.RemoveFile(@"new", CurrentImage);

                    string extension = Path.GetExtension(model.Thumb.FileName);
                    string imageName = Guid.NewGuid() + extension;
                    thumb = await utilities.FileUpLoad(model.Thumb, @"new", imageName);
                }
                else
                {
                    thumb = CurrentImage;
                }

                model.Alias = utilities.SEOUrl(model.Title);
                await context.Database.ExecuteSqlRawAsync(NewSP.SP_Update_New, model.PostId, model.Title, model.Contents,thumb,model.Alias,model.IsNewFeed);
                return Ok(new ResponseResult(200));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseResult(400));
            }
        }
        [HttpDelete("delete/{postId}")]
        [Authorize(AuthenticationSchemes = ("Bearer"))]
        public async Task<IActionResult> Delete(int postId)
        {
            try
            {
                var CurrentImage = GetById(postId).Thumb;
                if (CurrentImage != "default.jpg")
                {
                    utilities.RemoveFile(@"new", CurrentImage);
                }
                await context.Database.ExecuteSqlRawAsync(NewSP.SP_Delete_New, postId);
                return Ok(new ResponseResult(200));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseResult(400));
            }
        }
    }
}