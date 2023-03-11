using System;
using System.Collections.Generic;
using System.Text;

namespace tikAeroWebMain
{
    public class BaggageFee
    {
        public Guid baggage_fee_option_id { get; set; }
        public Guid passenger_id { get; set; }
        public Guid booking_segment_id { get; set; }
        public Guid fee_id { get; set; }
        public string fee_rcd { get; set; }
        public string fee_category_rcd { get; set; }
        public string currency_rcd { get; set; }
        public string display_name { get; set; }
        public decimal number_of_units { get; set; }
        public decimal fee_amount { get; set; }
        public decimal fee_amount_incl { get; set; }
        public decimal total_amount { get; set; }
        public decimal total_amount_incl { get; set; }
        public decimal vat_percentage { get; set; }               
    }
}
