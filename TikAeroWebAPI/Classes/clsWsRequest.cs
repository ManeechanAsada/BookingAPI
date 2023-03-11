using System;
using System.Collections.Generic;
using System.Web;
using tikSystem.Web.Library;

namespace TikAeroWebAPI.Classes
{
    public abstract class WsRequest
    {
        public string AgencyCode { get; set; }
        public string UserCode { get; set; }
        public string Password { get; set; }
    }
}