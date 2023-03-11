using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class AgencyDetail
    {
        public string application_type { get; set; }
        public string agency_name { get; set; }
        public string legal_name { get; set; }
        public string iata_number { get; set; }
        public string tax_id { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string website_address { get; set; }
        public string currency_rcd { get; set; }
        public string language_rcd { get; set; }
        public string contact_person { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string street { get; set; }
        public string state { get; set; }
        public string district { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string po_box { get; set; }
        public string zip_code { get; set; }
        public string country_rcd { get; set; }

        public string lastname { get; set; }
        public string firstname { get; set; }
        public string title_rcd { get; set; }
        public string user_logon { get; set; }
        public string agency_password { get; set; }
        public string comment { get; set; }
    }
}