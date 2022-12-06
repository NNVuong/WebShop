using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using SharedObjects.Models;

namespace SharedObjects.ViewModels
{
    public class ProductImageViewModel
    {
        public int ProductId { get; set; }
        public List<IFormFile> ImageName { get; set; }
    }
}
