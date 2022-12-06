using System;
using System.ComponentModel.DataAnnotations;

namespace SharedObjects.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImageName { get; set; }
        public Product Product { get; set; }
    }
}
