using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class SeatRequest
    {
        public Guid BookingSegmentId { get; set; }
        public Guid PassengerId { get; set; }
        public int SeatRow { get; set; }
        public string SeatColumn { get; set; }
        public string SeatFeeRcd { get; set; }
    }
}