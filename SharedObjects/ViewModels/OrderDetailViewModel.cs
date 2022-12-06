using System;
namespace SharedObjects.ViewModels
{
    public class OrderDetailViewModel
    {
        public int OrderDetailId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? Amount { get; set; }
        public decimal TotalMoney { get; set; }
        public decimal? Price { get; set; }
    }
}
