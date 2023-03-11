using System;
using System.Collections.Generic;
using System.Text;
using tikSystem.Web.Library;

namespace tikAeroWebMain
{
    public static class FeeExtension
    {
        public static List<BaggageFee> ToBaggageFee(this IList<Fee> baggageFee)
        {
            if (baggageFee != null && baggageFee.Count > 0)
            {
                List<BaggageFee> bv = new List<BaggageFee>();
                BaggageFee b;
                for (int i = 0; i < baggageFee.Count; i++)
                {
                    b = new BaggageFee();
                    b.baggage_fee_option_id = baggageFee[i].baggage_fee_option_id;
                    b.passenger_id = baggageFee[i].passenger_id;
                    b.booking_segment_id = baggageFee[i].booking_segment_id;
                    b.fee_id = baggageFee[i].fee_id;
                    b.fee_rcd = baggageFee[i].fee_rcd;
                    b.fee_category_rcd = baggageFee[i].fee_category_rcd;
                    b.currency_rcd = baggageFee[i].currency_rcd;
                    b.display_name = baggageFee[i].display_name;
                    b.number_of_units = baggageFee[i].number_of_units;
                    b.fee_amount = baggageFee[i].fee_amount;
                    b.fee_amount_incl = baggageFee[i].fee_amount_incl;
                    b.total_amount = baggageFee[i].total_amount;
                    b.total_amount_incl = baggageFee[i].total_amount_incl;
                    b.vat_percentage = baggageFee[i].vat_percentage;

                    bv.Add(b);
                    b = null;
                }
                return bv;
            }
            return null;
        }
        public static List<FeeView> ToFeeView(this IList<Fee> fees)
        {
            if (fees != null && fees.Count > 0)
            {
                List<FeeView> fv = new List<FeeView>();
                FeeView f;
                for (int i = 0; i < fees.Count; i++)
                {
                    f = new FeeView();

                    f.fee_id = fees[i].fee_id;
                    f.fee_rcd = fees[i].fee_rcd;
                    f.fee_category_rcd = fees[i].fee_category_rcd;
                    f.currency_rcd = fees[i].currency_rcd;
                    f.display_name = fees[i].display_name;
                    f.fee_amount = fees[i].fee_amount;
                    f.fee_amount_incl = fees[i].fee_amount_incl;
                    f.vat_percentage = fees[i].vat_percentage;
                    f.fee_percentage = fees[i].fee_percentage;
                    f.minimum_fee_amount_flag = fees[i].minimum_fee_amount_flag;
                    f.od_flag = fees[i].od_flag;

                    fv.Add(f);
                    f = null;
                }
                return fv;
            }
            return null;
        }
    }
}
