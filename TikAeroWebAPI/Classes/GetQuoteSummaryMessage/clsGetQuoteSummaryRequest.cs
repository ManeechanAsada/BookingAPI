using System;
using System.Collections.Generic;
using System.Web;
using tikSystem.Web.Library;

namespace TikAeroWebAPI.Classes
{
    public class GetQuoteSummaryRequest
    {
        public Int16 Adult { get; set; }
        public Int16 Child { get; set; }
        public Int16 Infant { get; set; }
        public Int16 Other { get; set; }
        public FlightSegmentRequest FlightSegmentRequest { get; set; }
        public string AgencyCode { get; set; }
        public string Language { get; set; }
        public string CurrencyCode { get; set; }
        public bool NoVat { get; set; }
        public string Password { get; set; }
        public string OtherPassengerType { get; set; }
    }
}