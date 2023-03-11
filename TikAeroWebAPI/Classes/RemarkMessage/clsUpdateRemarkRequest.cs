using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class updateRemarkRequest
    {

        public Guid BookingId { get; set; }
        public System.Collections.Generic.List<RemarkBase> remarkUpdateRequest{get; set;}
    }
}