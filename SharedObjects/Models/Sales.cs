using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SharedObjects.Models
{
    public partial class Sales
    {
        [Key]
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime? StartT { get; set; }

        public DateTime? EndT { get; set; }
    }
}
