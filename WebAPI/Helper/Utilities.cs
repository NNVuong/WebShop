using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace WebAPI.Helper
{
    public class Utilities : IUtilities
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public Utilities(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        public string SEOUrl(string url)
        {
            url = url.ToLower();
            url = Regex.Replace(url, @"[áàạảãâấầậẩẫăắằặẳẵ]", "a");
            url = Regex.Replace(url, @"[éèẹẻẽêếềệểễ]", "e");
            url = Regex.Replace(url, @"[óòọỏõôốồộổỗơớờợởỡ]", "o");
            url = Regex.Replace(url, @"[íìịỉĩ]", "i");
            url = Regex.Replace(url, @"[ýỳỵỉỹ]", "y");
            url = Regex.Replace(url, @"[úùụủũưứừựửữ]", "u");
            url = Regex.Replace(url, @"[đ]", "d");

            //2. Chỉ cho phép nhận:[0-9a-z-\s]
            url = Regex.Replace(url.Trim(), @"[^0-9a-z-\s]", "").Trim();
            //xử lý nhiều hơn 1 khoảng trắng --> 1 kt
            url = Regex.Replace(url.Trim(), @"\s+", "-");
            //thay khoảng trắng bằng -
            url = Regex.Replace(url, @"\s", "-");
            while (true)
            {
                if (url.IndexOf("--") != -1)
                {
                    url = url.Replace("--", "-");
                }
                else
                {
                    break;
                }
            }
            return url;
        }


        public async Task<string> FileUpLoad(IFormFile files, string sDirectory, string newname)
        {
            try
            {
                string path = Path.Combine(webHostEnvironment.WebRootPath, "images", sDirectory);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string pathfile = Path.Combine(webHostEnvironment.WebRootPath, "images", sDirectory, newname);
                var supportedTypes = new[] { "jpg", "jpeg", "png", "gif" };
                var fileExt = Path.GetExtension(files.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt.ToLower())) /// Khác các file định nghĩa
                {
                    return "Unsuccess";
                }
                else
                {
                    using (var stream = new FileStream(pathfile, FileMode.Create))
                    {
                        await files.CopyToAsync(stream);
                        stream.Flush();
                        return newname;
                    }
                }
            }
            catch
            {
                return "Catch Unsuccess";
            }
        }
        public string RemoveFile(string sDirectory, string nameFile)
        {
            try
            {
                string pathFile = Path.Combine(webHostEnvironment.WebRootPath, "images", sDirectory, nameFile);
                if (File.Exists(pathFile))
                {
                    File.Delete(pathFile);
                }
                return "Success";
            }
            catch
            {
                return "Unsuccess";
            }
        }
    }
}
