using System;
using System.Collections.Generic;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class SpecialServiceRequest
    {
        //Auto generated from client. Used as a reference for remove.
        public Guid PassengerSegmentServiceId { get; set; }
        public Guid PassengerId { get; set; }
        public Guid BookingSegmentId { get; set; }
        public string OriginRcd { get; set; }
        public string DestinationRcd { get; set; }
        public string SpecialServiceRcd { get; set; }
        public string ServiceText { get; set; }
        public short NumberOfUnit { get; set; }
        public bool ServiceOnRequestFlag { get; set; }
    }
}