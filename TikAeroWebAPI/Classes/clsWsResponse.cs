using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tikSystem.Web.Library;

namespace TikAeroWebAPI.Classes
{
    public class WsResponse
    {
        string strErrorCode;
        public string ErrorCode
        {
            get { return strErrorCode; }
            set { strErrorCode = value; }
        }
        string strErrorMessage;
        public string ErrorMessage
        {
            get { return strErrorMessage; }
            set { strErrorMessage = value; }
        }
    }
}