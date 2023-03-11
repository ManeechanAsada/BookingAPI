using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tikAeroWebMain
{
    public class SegmentFeeResponse : ResponseBase
    {
        public List<ServiceFee> ServiceFee { get; set; }
    }
}