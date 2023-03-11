using System;
using System.Collections.Generic;
using System.Web;

namespace tikAeroWebMain
{
    public class FeeView
    {
        public Guid fee_id { get; set; }
        public string fee_rcd { get; set; }
        public string currency_rcd { get; set; }
        public string display_name { get; set; }
        public string fee_category_rcd { get; set; }

        public decimal fee_amount { get; set; }
        public decimal fee_amount_incl { get; set; }
        public decimal vat_percentage { get; set; }
        
        public decimal fee_percentage { get; set; }

        public byte minimum_fee_amount_flag { get; set; }
        public byte od_flag { get; set; }
    }
}