﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Xml.Serialization;

namespace tikSystem.Web.Library
{
    //checkin API
    public class APIPassengerFees : CollectionBase
    {
        public APIPassengerFees() { }

        public APIPassengerFee this[int index]
        {
            get { return (APIPassengerFee)this.List[index]; }
            set { this.List[index] = value; }
        }
        public int Add(APIPassengerFee Value)
        {
            return this.List.Add(Value);
        }
    }
}
