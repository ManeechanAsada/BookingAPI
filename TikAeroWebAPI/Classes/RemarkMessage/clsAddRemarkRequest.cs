using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class RemarkRequest
    {
        public Guid BookingId { get; set; }
        public Guid BookingRemarkId { get; set; }
        public Guid ClientProfileId { get; set; }
        public string RemarkTypeRcd { get; set; }
        public string RemarkText { get; set; }
        public string NickName { get; set; }
        public bool ProtectedFlag { get; set; }
        public bool WarningFlag { get; set; }
        public bool ProcessMessageFlag { get; set; }
    }
}                   