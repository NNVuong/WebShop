using System;
using Microsoft.AspNetCore.Http;

namespace SharedObjects.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ShortDesc { get; set; }
        public string Description { get; set; }
        public int? CatId { get; set; }
        public decimal? Price { get; set; }
        public int? Discount { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool BestSellers { get; set; }
        public bool HomeFlag { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public int? UnitsInStock { get; set; }
        public IFormFile Avatar { get; set; }
    }
}
