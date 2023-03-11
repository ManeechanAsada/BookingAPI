using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class VoucherTemplateListRequest
    {
        public string VoucherTemplateId { get; set; }
        public string VoucherTemplate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Status { get; set; }
    

    }
}