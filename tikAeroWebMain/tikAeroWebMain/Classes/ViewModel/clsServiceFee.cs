using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tikAeroWebMain
{
    public class ServiceFee : SegmentService
    {
        string _fee_rcd = string.Empty;
        public string fee_rcd
        {
            get { return _fee_rcd; }
            set { _fee_rcd = value; }
        }
        string _display_name;
        public string display_name
        {
            get { return _display_name; }
            set { _display_name = value; }
        }
        string _currency_rcd;
        public string currency_rcd
        {
            get { return _currency_rcd; }
            set { _currency_rcd = value; }
        }
        decimal _fee_amount;
        public decimal fee_amount
        {
            get { return _fee_amount; }
            set { _fee_amount = value; }
        }
        decimal _fee_amount_incl;
        public decimal fee_amount_incl
        {
            get { return _fee_amount_incl; }
            set { _fee_amount_incl = value; }
        }
        decimal _total_fee_amount;
        public decimal total_fee_amount
        {
            get { return _total_fee_amount; }
            set { _total_fee_amount = value; }
        }
        decimal _total_fee_amount_incl;
        public decimal total_fee_amount_incl
        {
            get { return _total_fee_amount_incl; }
            set { _total_fee_amount_incl = value; }
        }
        bool _service_on_request_flag;
        public bool service_on_request_flag
        {
            get { return _service_on_request_flag; }
            set { _service_on_request_flag = value; }
        }
        bool _cut_off_time;
        public bool cut_off_time
        {
            get { return _cut_off_time; }
            set { _cut_off_time = value; }
        }
    }
}
