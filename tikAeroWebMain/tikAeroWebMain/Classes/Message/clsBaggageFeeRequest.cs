using System;
using System.Collections.Generic;
using System.Text;
using tikSystem.Web.Library;

namespace tikAeroWebMain
{ 
    public class BaggageFeeRequest
    {
        public Guid BookingSegmentId { get; set; }
        public Guid PassengerId { get; set; }
        public string AgencyCode { get; set; }
        public string LanguageCode { get; set; }
        public List<MappingView> Mappings { get; set; }
        public int MaxUnit { get; set; }
        public List<Fee> Fees { get; set; }
    }
}
