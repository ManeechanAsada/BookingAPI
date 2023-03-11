using System;
using System.Collections.Generic;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class VoucherPaymentRequest
    {
        public Guid booking_payment_id { get; set; }
        public Guid payment_by { get; set; }
        public string form_of_payment_rcd { get; set; }
        public string form_of_payment_subtype_rcd { get; set; }
        public string agency_code { get; set; }
        public decimal payment_amount { get; set; }
        public string currency_rcd { get; set; }
        public string NameOnCard { get; set; }
        public string CreditCardNumber { get; set; }
        public string IssueMonth { get; set; }
        public string IssueYear { get; set; }
        public string IssueNumber { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string CCV { get; set; }
    }
}