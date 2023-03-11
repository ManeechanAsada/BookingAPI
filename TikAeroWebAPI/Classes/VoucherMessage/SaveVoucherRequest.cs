using System;
using System.Collections.Generic;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class SaveVoucherRequest
    {
        public VoucherPaymentRequest Payment { get; set; }
        public VoucherTemplateRequest Voucher { get; set; }
    }
}