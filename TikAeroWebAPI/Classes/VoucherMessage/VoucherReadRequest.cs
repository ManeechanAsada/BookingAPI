using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class VoucherReadRequest
    {
        public Guid VoucherId { get; set; }
        public Double VoucherNumber { get; set; }
    }
}