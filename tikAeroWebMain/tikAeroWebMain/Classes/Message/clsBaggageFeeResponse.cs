using System;
using System.Collections.Generic;
using System.Text;

namespace tikAeroWebMain
{
    public class BaggageFeeResponse : ResponseBase
    {
        public List<BaggageFee> BaggageFees { get; set; } 
    }
}
