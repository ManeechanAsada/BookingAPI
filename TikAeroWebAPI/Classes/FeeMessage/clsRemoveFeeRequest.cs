using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class RemoveFeeRequest
    {
        Guid _BookingFeeId;
        public Guid BookingFeeId
        {
            get { return _BookingFeeId; }
            set { _BookingFeeId = value; }
        }
    }
}