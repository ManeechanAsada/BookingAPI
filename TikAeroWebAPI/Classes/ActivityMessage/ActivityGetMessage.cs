using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes.ActivityMessage
{
    public class ActivityGetRequestMessage
    {
       public string RemarkType { get; set; }
       public string Nickname { get; set; }
       public DateTime TimelimitFrom { get; set; }
       public DateTime TimelimitTo { get; set; }
       public bool PendingOnly { get; set; }
       public bool IncompleteOnly { get; set; }
       public bool IncludeRemarks { get; set; }
       public bool showUnassigned { get; set; }
       public string AgencyCode { get; set; }

    }
}

