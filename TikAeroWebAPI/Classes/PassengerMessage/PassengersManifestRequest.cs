using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;

namespace TikAeroWebAPI.Classes.PassengerMessage
{
    public class PassengersManifestRequest
    {
        public string origin_rcd { get; set; }
        public string destination_rcd { get; set; }
        public string airline_rcd { get; set; }
        public string flight_number { get; set; }

        private DateTime? _departure_date_from;
        public string departure_date_from
        {
            get
            {
                return _departure_date_from.HasValue ? _departure_date_from.Value.ToString("yyyy-MM-dd") : null;
            }
            set
            {
                DateTime from = new DateTime();
                if (DateTime.TryParse(value, out from))
                {
                    _departure_date_from = from;
                }
            }
        }

        private DateTime? _departure_date_to;
        public string departure_date_to
        {
            get
            {
                return _departure_date_to.HasValue ? _departure_date_to.Value.ToString("yyyy-MM-dd") : null;
            }
            set
            {
                DateTime to = new DateTime();
                if (DateTime.TryParse(value, out to))
                {
                    _departure_date_to = to;
                }
            }
        }

        [DefaultValue(false)]
        public bool bReturnServices { get; set; }

        [DefaultValue(false)]
        public bool bReturnBagTags { get; set; }

        [DefaultValue(false)]
        public bool bReturnRemarks { get; set; }

        [DefaultValue(false)]
        public bool bNotCheckedIn { get; set; }

        [DefaultValue(false)]
        public bool bCheckedIn { get; set; }

        [DefaultValue(false)]
        public bool bOffloaded { get; set; }

        [DefaultValue(false)]
        public bool bNoShow { get; set; }

        [DefaultValue(false)]
        public bool bInfants { get; set; }

        [DefaultValue(false)]
        public bool bConfirmed { get; set; }

        [DefaultValue(false)]
        public bool bWaitlisted { get; set; }

        [DefaultValue(false)]
        public bool bCancelled { get; set; }

        [DefaultValue(false)]
        public bool bStandby { get; set; }

        [DefaultValue(false)]
        public bool bIndividual { get; set; }

        [DefaultValue(false)]
        public bool bGroup { get; set; }

        [DefaultValue(false)]
        public bool bTransit { get; set; }
    }
}