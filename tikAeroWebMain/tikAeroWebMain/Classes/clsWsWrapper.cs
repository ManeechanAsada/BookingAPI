using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tikSystem.Web.Library;

namespace tikAeroWebMain.Classes
{
    public class WsWrapper
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

        AgencyAccounts objAgencies;
        public AgencyAccounts Agencies
        {
            get { return objAgencies; }
            set { objAgencies = value; }
        }

        BookingHeader objHeader;
        public BookingHeader Header
        {
            get { return objHeader; }
            set { objHeader = value; }
        }
    }
}