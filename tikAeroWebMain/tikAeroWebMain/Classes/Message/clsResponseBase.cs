using System;
using System.Collections.Generic;
using System.Text;

namespace tikAeroWebMain
{
    public abstract class ResponseBase
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
