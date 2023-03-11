using System;
using System.Collections.Generic;
using System.Web;

namespace tikAeroWebMain
{
    public class GetFeeRequest
    {
        public string FeeRcd { get; set; }
        public string CurrencyRcd { get; set; }
        public string AgencyCode { get; set; }
        public string BookingClass { get; set; }
        public string FareBasis { get; set; }
        public string OriginRcd { get; set; }
        public string DestinationRcd { get; set; }
        public string FlightNumber { get; set; }
        public DateTime DepartureDate { get; set; }
        public string LanguageCode { get; set; }
    }
}