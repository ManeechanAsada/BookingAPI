using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikAeroWebAPI.Classes
{
    public class CustomFeeRequest
    {
        public Guid PassengerId { get; set; }
        public Guid BookingSegmentId { get; set; }
        public Guid FeeId { get; set; }

        public string FeeCategoryRcd { get; set; }
        public string FeeRcd { get; set; }
        public string CurrencyRcd { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal FeeAmountIncl { get; set; }
        public decimal VatPercentage { get; set; }
        public string ChargeCurrencyRcd { get; set; }
        public decimal ChargeAmount { get; set; }
        public decimal ChargeAmountIncl { get; set; }
        public decimal NumberOfUnits { get; set; }
        public string Units { get; set; }
        //Optional
        public string DocumentNumber{get;set;}
        public DateTime DocumentDateTime { get; set; }
        public string Comment { get; set; }
        public string ExternalReference { get; set; }
        public string VendorRcd { get; set; }
        public string OdOriginRcd { get; set; }
        public string OdDestinationRcd { get; set; }
    }
}