using System;
using SharedObjects.ValueObjects;

namespace SharedObjects.ViewModels
{
    public class CartItem
    {
        public VProduct product { get; set; }
        public int amount { get; set; }
        public decimal TotalMoney => amount * product.Price.Value;
    }
}
