using System;
using SharedObjects.ValueObjects;

namespace SharedObjects.ViewModels
{
    public class WishList
    {
        public VProduct product { get; set; }
        public decimal Price { get; set; }
        public int amount { get; set; }
    }
}
