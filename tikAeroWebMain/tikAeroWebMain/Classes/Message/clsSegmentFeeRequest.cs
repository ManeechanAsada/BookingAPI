using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tikAeroWebMain
{
    public class SegmentFeeRequest
    {
        public string AgencyCode { get; set; }
        public string CurrencyCode { get; set; }
        public string LanguageCode { get; set; }
        public int NumberOfPassenger { get; set; }
        public int NumberOfInfant { get; set; }
        public string[] ServiceCode { get; set; }
        public List<SegmentService> SegmentService { get; set; }
    }
}