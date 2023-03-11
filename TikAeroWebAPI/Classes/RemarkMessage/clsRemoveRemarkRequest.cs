using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class RemoveRemarkRequest
    {
        Guid _BookingRemarkId;
        public Guid BookingRemarkId
        {
            get { return _BookingRemarkId; }
            set { _BookingRemarkId = value; }
        }
    }
}