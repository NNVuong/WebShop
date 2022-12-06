using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SharedObjects.Models;

namespace SharedObjects.ValueObjects
{
    public class VProductCategory
    {
        [Key]
        public int CatId { get; set; }
        public string CatName { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }
}