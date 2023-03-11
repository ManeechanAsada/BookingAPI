using System;
using System.Collections.Generic;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class ValidateVoucherRequest
    {
        public string VoucherNumber { get; set; }
        public string VoucherPassword { get; set; }
    }
}