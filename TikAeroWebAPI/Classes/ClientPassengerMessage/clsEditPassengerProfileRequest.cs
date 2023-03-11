using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class EditPassengerProfileRequest
    {
        public Guid client_profile_id { get; set; }
        public List<EditPassengerProfileMessage> PassengerProfile { get; set; }
    }
}