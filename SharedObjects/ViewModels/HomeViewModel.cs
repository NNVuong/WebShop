using System;
using System.Collections.Generic;
using SharedObjects.ValueObjects;

namespace SharedObjects.ViewModels
{
    public class HomeViewModel
    {
        public List<ProductHomeVM> Products { get; set; }
        public List<VNew> News { get; set; }
    }
}
