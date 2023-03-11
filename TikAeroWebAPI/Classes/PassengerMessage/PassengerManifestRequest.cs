using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes.PassengerMessage
{
    public class PassengerManifestRequest
    {
        public string airline{get; set;}
        public string flightNumber{get; set;}
        public Guid flightID{get; set;}
        public DateTime flightFrom{get; set;}
        public DateTime flightTo{get; set;}
        public string recordLocator{get; set;}
        public string origin{get; set;}
        public string destination{get; set;}
        public string passengerName{get; set;}
        public string seatNumber{get; set;}
        public string ticketNumber{get; set;}
        public string phoneNumber{get; set;}
        public string passengerStatus{get; set;}
        public string checkInStatus{get; set;}
        public string clientNumber{get; set;}
        public string memberNumber{get; set;}
        public Guid clientID{get; set;}
        public Guid passengerId{get; set;}
        public bool booked{get; set;}
        public bool listed{get; set;}
        public bool eTicketOnly{get; set;}
        public bool includeCancelled{get; set;}
        public bool openSegments{get; set;}
        public bool showHistory{get; set;}
        public string language { get; set; }
    }
}