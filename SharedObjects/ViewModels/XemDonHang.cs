using System;
using System.Collections.Generic;
using SharedObjects.ValueObjects;

namespace SharedObjects.ViewModels
{
    public class XemDonHang
    {
        public VOrder DonHang { get; set; }
        public List<VOrderDetail> ChiTietDonHang { get; set; }
    }
}
