using System;
using System.ComponentModel.DataAnnotations;

namespace SharedObjects.ValueObjects
{
    public class VSales
    {
        [Key]
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime? StartT { get; set; }

        public DateTime? EndT { get; set; }
       
    }
}