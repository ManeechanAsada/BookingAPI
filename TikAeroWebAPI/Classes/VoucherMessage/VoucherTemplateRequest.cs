using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class VoucherTemplateRequest
    {
        public Guid voucher_template_id
        {
            get;
            set;
        }
        public string currency_rcd
        {
            get;
            set;
        }
        public DateTime valid_to_date
        {
            get;
            set;
        }
        public int valid_days
        {
            get;
            set;
        }
        public DateTime valid_from_date
        {
            get;
            set;
        }
        public string display_name
        {
            get;
            set;
        }
        public string status_code
        {
            get;
            set;
        }
        public string origins
        {
            get;
            set;
        }
        public string destinations
        {
            get;
            set;
        }
        public Int16 passenger_segments
        {
            get;
            set;
        }
        public string voucher_use_code
        {
            get;
            set;
        }
        public byte recipient_only_flag
        {
            get;
            set;
        }
        public decimal discount_percentage
        {
            get;
            set;
        }
        public string valid_for_class
        {
            get;
            set;
        }
        public byte ticket_flag
        {
            get;
            set;
        }
        public byte seat_fee_flag
        {
            get;
            set;
        }
        public byte other_fee_flag
        {
            get;
            set;
        }
        public byte b2c_flag
        {
            get;
            set;
        }
        public byte b2b_flag
        {
            get;
            set;
        }
        public byte b2e_flag
        {
            get;
            set;
        }
        public byte airline_flag
        {
            get;
            set;
        }
        public byte fare_only_flag
        {
            get;
            set;
        }
        public decimal voucher_value
        {
            get;
            set;
        }
        public decimal charge_amount
        {
            get;
            set;
        }
        public byte multiple_payment_flag
        {
            get;
            set;
        }
        public decimal payment_total
        {
            get;
            set;
        }
        public DateTime expiry_date_time
        {
            get;
            set;
        }
        public byte refundable_flag
        {
            get;
            set;
        }
        public byte percentage_flag
        {
            get;
            set;
        }
        public byte single_flight_flag
        {
            get;
            set;
        }
        public byte single_passenger_flag
        {
            get;
            set;
        }
        public string recipient_name
        {
            get;
            set;
        }
        public string voucher_password
        {
            get;
            set;
        }
        public string form_of_payment_rcd
        {
            get;
            set;
        }
        public string form_of_payment_subtype_rcd
        {
            get;
            set;
        }

    }
}