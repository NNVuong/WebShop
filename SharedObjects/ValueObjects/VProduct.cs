﻿using System;
using System.ComponentModel.DataAnnotations;

namespace SharedObjects.ValueObjects
{
    public class VProduct
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ShortDesc { get; set; }
        public string Description { get; set; }
        public int? CatId { get; set; }
        public string CatName { get; set; }
        public decimal? Price { get; set; }
        public int? Discount { get; set; }
        public string Video { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool BestSellers { get; set; }
        public bool HomeFlag { get; set; }
        public bool Active { get; set; }
        public string Tags { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKey { get; set; }
        public int UnitsInStock { get; set; }
        public long? rc { get; set; }
        public string Avatar { get; set; }
    }
}
