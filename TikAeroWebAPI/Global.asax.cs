using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using tikSystem.Web.Library;

namespace TikAeroWebAPI
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
     
        }

        protected void Session_End(object sender, EventArgs e)
        {
            //Release Seat
            BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];
            if (bookingHeader != null && bookingHeader.booking_id != Guid.Empty)
            {
                ServiceClient obj = new ServiceClient();
                obj.ReleaseFlightInventorySession(bookingHeader.booking_id.ToString(), string.Empty, string.Empty, string.Empty, false, true, true);
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}