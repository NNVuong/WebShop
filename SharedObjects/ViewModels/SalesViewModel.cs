using System;
namespace SharedObjects.ViewModels
{
    public class SalesViewModel
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime? StartT { get; set; }
        public DateTime? EndT { get; set; }
    }
}
