using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public abstract class BaseClientMessage
    {
        public string title_rcd { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string language_rcd { get; set; }
        public string nationality_rcd { get; set; }
        public string passenger_weight { get; set; }
        public string gender_type_rcd { get; set; }
        public string passenger_type_rcd { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string zip_code { get; set; }
        public string po_box { get; set; }
        public string country_rcd { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string document_type_rcd { get; set; }
        public string residence_country_rcd { get; set; }

        public string passport_number { get; set; }
        public string passport_issue_date { get; set; }
        public string passport_expiry_date { get; set; }
        public string passport_issue_place { get; set; }
        public string passport_birth_place { get; set; }
        public string date_of_birth { get; set; }
        public string passport_issue_country_rcd { get; set; }
        public string contact_name { get; set; }
        public string contact_email { get; set; }
        public string mobile_email { get; set; }
        public string phone_mobile { get; set; }
        public string phone_home { get; set; }
        public string phone_fax { get; set; }
        public string phone_business { get; set; }
        public string employee_number { get; set; }
        public bool wheelchair_flag { get; set; }
        public bool vip_flag { get; set; }
        public string member_level_rcd { get; set; }
        public string member_number { get; set; }
        public bool window_seat_flag { get; set; }
        public string redress_number { get; set; }
        public string status_code { get; set; }
        public string client_password { get; set; }
        public bool company_flag { get; set; }
        public string client_type_rcd { get; set; }
        public string member_since_date { get; set; }
        public byte newsletter_flag { get; set; }
        public string airport_rcd { get; set; }


    }


    public abstract class BasePassengerProfileMessage
    {
        public string document_type_rcd { get; set; }

        public string title_rcd { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string language_rcd { get; set; }
        public string nationality_rcd { get; set; }
        public string passenger_weight { get; set; }
        public string gender_type_rcd { get; set; }
        public string passenger_type_rcd { get; set; }
        public string passport_number { get; set; }
        public string passport_issue_date { get; set; }
        public string passport_expiry_date { get; set; }
        public string passport_issue_place { get; set; }
        public string passport_birth_place { get; set; }
        public string date_of_birth { get; set; }
        public string passport_issue_country_rcd { get; set; }
        public string employee_number { get; set; }
        public bool wheelchair_flag { get; set; }
        public bool vip_flag { get; set; }
        public string member_level_rcd { get; set; }
        public string member_number { get; set; }
        public bool window_seat_flag { get; set; }
        public string redress_number { get; set; }
        public string passenger_role_rcd { get; set; }
        public string comment { get; set; }
        public string medical_conditions { get; set; }
        //add KnownTravelerNumber
        public string known_traveler_number { get; set; }

        
    }

}