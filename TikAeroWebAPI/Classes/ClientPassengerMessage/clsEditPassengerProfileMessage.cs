using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class EditPassengerProfileMessage : BasePassengerProfileMessage
    {
        public Guid passenger_profile_id { get; set; }
    }
}