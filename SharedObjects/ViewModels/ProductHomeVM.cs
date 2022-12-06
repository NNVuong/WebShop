using System;
using System.Collections.Generic;
using SharedObjects.Models;
using SharedObjects.ValueObjects;

namespace SharedObjects.ViewModels
{
    public class ProductHomeVM
    {
        public VCategory category { get; set; }
        public IEnumerable<VProduct> lsProducts { get; set; }
    }
}
