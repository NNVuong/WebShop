using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebAPI.Helper
{
    public interface IUtilities
    {
        string SEOUrl(string url);
        Task<string> FileUpLoad(IFormFile files, string sDirectory, string newname);
        string RemoveFile(string sDirectory, string nameFile);
    }
}
