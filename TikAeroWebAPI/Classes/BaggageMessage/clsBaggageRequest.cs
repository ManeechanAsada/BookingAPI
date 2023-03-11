using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class BaggageRequest
    {
        public Guid BookingSegmentId { get; set; }
        public Guid PassengerId { get; set; }
        public bool OutBoundFlight { get; set; }
        public Int16 NumberOfUnit { get; set; }
    }
}