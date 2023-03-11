using System;
using System.Collections.Generic;
using System.Web;
using tikSystem.Web.Library;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace TikAeroWebAPI.Classes
{
    public class FlightSegmentRequest
    {
        public Guid FlightId { get; set; }
        public Guid FareId { get; set; }
        public string OriginRcd { get; set; }
        public string DestinationRcd { get; set; }
        [SoapElement(IsNullable = true)]
        public string BoardingClassRcd { get; set; }
        [SoapElement(IsNullable = true)]
        public string BookingClassRcd { get; set; }
        public DateTime DepartureDate { get; set; }
        [SoapElement(IsNullable = true)]
        public DateTime TransitDepartureDate { get; set; }
        [SoapElement(IsNullable = true)]
        public Guid TransitFlightId { get; set; }
        [SoapElement(IsNullable = true)]
        public string TransitAirportRcd { get; set; }
        [SoapElement(IsNullable = true)]
        public string TransitBoardingClassRcd { get; set; }
        [SoapElement(IsNullable = true)]
        public string TransitBookingClassRcd { get; set; }
    }
}