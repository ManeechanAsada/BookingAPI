using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class BookingGetRequest
    {
        
        public string airline { get; set; }
        public string flightNumber { get; set; }
        public string flightId { get; set; }
        public string flightFrom { get; set; }
        public string flightTo { get; set; }
        public string recordLocator { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
        public string passengerName { get; set; }
        public string seatNumber { get; set; }
        public string ticketNumber { get; set; }
        public string phoneNumber { get; set; }
        public string agencyCode { get; set; }
        public string clientNumber { get; set; }
        public string memberNumber { get; set; }
        public string clientId { get; set; }
        public bool showHistory { get; set; }
        public string language { get; set; }
        public bool bIndividual { get; set; }
        public bool bGroup { get; set; }
        public string createFrom { get; set; }
        public string createTo { get; set; }
    }
}