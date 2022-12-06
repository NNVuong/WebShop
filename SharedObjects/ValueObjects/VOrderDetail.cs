using System;
using System.ComponentModel.DataAnnotations;

namespace SharedObjects.ValueObjects
{
    public class VOrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public decimal TotalMoney { get; set; }
    }
}
