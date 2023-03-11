using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class LogonResponse : WsResponse
    {
        string strUrl;
        public string url
        {
            get { return strUrl; }
            set { strUrl = value; }
        }

        string strRecord_locator;
        public string record_locator
        {
            get { return strRecord_locator; }
            set { strRecord_locator = value; }
        }
    }
}