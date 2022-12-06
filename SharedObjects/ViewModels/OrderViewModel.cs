using System;
namespace SharedObjects.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public int TransactStatusId { get; set; }
        public decimal TotalMoney { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
