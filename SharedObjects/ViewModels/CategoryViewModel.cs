using System;
using Microsoft.AspNetCore.Http;

namespace SharedObjects.ViewModels
{
    public class CategoryViewModel
    {
        public int CatId { get; set; }
        public string CatName { get; set; }
        public string Alias { get; set; }
        public IFormFile Thumb { get; set; }
    }
}
