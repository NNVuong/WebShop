using System;
using System.ComponentModel.DataAnnotations;

namespace SharedObjects.ValueObjects
{
    public class VProductImage
    {
        [Key]
        public int Id { get; set; }
        public string ImageName { get; set; }
    }
}