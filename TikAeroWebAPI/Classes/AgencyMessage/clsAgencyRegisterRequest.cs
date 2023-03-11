using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class AgencyRegisterRequest : WsRequest
    {
        public AgencyDetail AgencyInput { get; set; }
    }
}