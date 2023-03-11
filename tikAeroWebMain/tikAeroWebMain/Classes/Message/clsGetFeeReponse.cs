using System;
using System.Collections.Generic;
using System.Web;
using tikSystem.Web.Library;
namespace tikAeroWebMain
{
    public class GetFeeReponse : ResponseBase
    {
        public List<FeeView> Fees { get; set; }
    }
}