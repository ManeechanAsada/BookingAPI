using System;
using System.Collections.Generic;
using System.Text;
using tikSystem.Web.Library;
using Avantik.Web.Service.COMHelper;

namespace tikAeroWebMain
{
    public static class MappingExtension
    {
        #region Recordset
        public static ADODB.Recordset FillRecordset(this IList<Mapping> mappings)
        {
            ADODB.Recordset rs = FabricateRecordset();
            for (int i = 0; i < mappings.Count; i++)
            {
                rs.AddNew();
                RecordsetHelper.AssignRsGuid(rs, "passenger_id", mappings[i].passenger_id);
                RecordsetHelper.AssignRsGuid(rs, "booking_segment_id", mappings[i].booking_segment_id);
                RecordsetHelper.AssignRsGuid(rs, "flight_id", mappings[i].flight_id); ;
                RecordsetHelper.AssignRsString(rs, "origin_rcd", mappings[i].origin_rcd);
                RecordsetHelper.AssignRsString(rs, "destination_rcd", mappings[i].destination_rcd);
                RecordsetHelper.AssignRsString(rs, "passenger_type_rcd", mappings[i].passenger_type_rcd);
                RecordsetHelper.AssignRsString(rs, "airline_rcd", mappings[i].airline_rcd);
                RecordsetHelper.AssignRsString(rs, "flight_number", mappings[i].flight_number);
                RecordsetHelper.AssignRsString(rs, "boarding_class_rcd", mappings[i].boarding_class_rcd);
                RecordsetHelper.AssignRsString(rs, "booking_class_rcd", mappings[i].booking_class_rcd);
                RecordsetHelper.AssignRsDateTime(rs, "departure_date", mappings[i].departure_date);
                RecordsetHelper.AssignRsString(rs, "inventory_class_rcd", mappings[i].inventory_class_rcd);
                RecordsetHelper.AssignRsString(rs, "currency_rcd", mappings[i].currency_rcd);
                RecordsetHelper.AssignRsDecimal(rs, "net_total", mappings[i].net_total);
                RecordsetHelper.AssignRsDecimal(rs, "tax_amount", mappings[i].tax_amount);
                RecordsetHelper.AssignRsDecimal(rs, "fare_amount", mappings[i].fare_amount);
                RecordsetHelper.AssignRsDecimal(rs, "yq_amount", mappings[i].yq_amount);
                RecordsetHelper.AssignRsDecimal(rs, "ticketing_fee_amount", mappings[i].ticketing_fee_amount);
                RecordsetHelper.AssignRsDecimal(rs, "reservation_fee_amount", mappings[i].reservation_fee_amount);
                RecordsetHelper.AssignRsDecimal(rs, "commission_amount", mappings[i].commission_amount);
                RecordsetHelper.AssignRsDecimal(rs, "commission_percentage", mappings[i].commission_percentage);
                RecordsetHelper.AssignRsDecimal(rs, "refund_charge", mappings[i].refund_charge);
                RecordsetHelper.AssignRsDecimal(rs, "fare_vat", mappings[i].fare_vat);
                RecordsetHelper.AssignRsDecimal(rs, "tax_vat", mappings[i].tax_vat);
                RecordsetHelper.AssignRsDecimal(rs, "yq_vat", mappings[i].yq_vat);
                RecordsetHelper.AssignRsDecimal(rs, "ticketing_fee_vat", mappings[i].ticketing_fee_vat);
                RecordsetHelper.AssignRsDecimal(rs, "reservation_fee_vat", mappings[i].reservation_fee_vat);
                RecordsetHelper.AssignRsDecimal(rs, "fare_amount_incl", mappings[i].fare_amount_incl);
                RecordsetHelper.AssignRsDecimal(rs, "tax_amount_incl", mappings[i].tax_amount_incl);
                RecordsetHelper.AssignRsDecimal(rs, "yq_amount_incl", mappings[i].yq_amount_incl);
                RecordsetHelper.AssignRsDecimal(rs, "commission_amount_incl", mappings[i].commission_amount_incl);
                RecordsetHelper.AssignRsDecimal(rs, "ticketing_fee_amount_incl", mappings[i].ticketing_fee_amount_incl);
                RecordsetHelper.AssignRsDecimal(rs, "reservation_fee_amount_incl", mappings[i].reservation_fee_amount_incl);
                RecordsetHelper.AssignRsString(rs, "od_origin_rcd", mappings[i].od_origin_rcd);
                RecordsetHelper.AssignRsString(rs, "od_destination_rcd", mappings[i].od_destination_rcd);
                RecordsetHelper.AssignRsGuid(rs, "fare_id", mappings[i].fare_id);
                RecordsetHelper.AssignRsString(rs, "fare_code", mappings[i].fare_code);
                RecordsetHelper.AssignRsBoolean(rs, "e_ticket_flag", mappings[i].e_ticket_flag);
                RecordsetHelper.AssignRsBoolean(rs, "refundable_flag", mappings[i].refundable_flag);
                RecordsetHelper.AssignRsBoolean(rs, "transferable_fare_flag", mappings[i].transferable_fare_flag);
                RecordsetHelper.AssignRsBoolean(rs, "duty_travel_flag", mappings[i].duty_travel_flag);
                RecordsetHelper.AssignRsBoolean(rs, "it_fare_flag", mappings[i].it_fare_flag);
                RecordsetHelper.AssignRsString(rs, "passenger_status_rcd", mappings[i].passenger_status_rcd);
                RecordsetHelper.AssignRsString(rs, "restriction_text", mappings[i].restriction_text);
                RecordsetHelper.AssignRsDecimal(rs, "baggage_weight", mappings[i].baggage_weight);
                RecordsetHelper.AssignRsString(rs, "fare_type_rcd", mappings[i].fare_type_rcd);
                RecordsetHelper.AssignRsDecimal(rs, "refund_with_charge_hours", mappings[i].refund_with_charge_hours);
                RecordsetHelper.AssignRsDecimal(rs, "refund_not_possible_hours", mappings[i].refund_not_possible_hours);
                RecordsetHelper.AssignRsDecimal(rs, "piece_allowance", mappings[i].piece_allowance);
                RecordsetHelper.AssignRsDouble(rs, "redemption_points", mappings[i].redemption_points);
                RecordsetHelper.AssignRsDateTime(rs, "fare_date_time", mappings[i].fare_date_time);
                RecordsetHelper.AssignRsString(rs, "create_name", mappings[i].create_name);
                RecordsetHelper.AssignRsString(rs, "update_name", mappings[i].update_name);
                RecordsetHelper.AssignRsString(rs, "agency_code", mappings[i].agency_code);
                RecordsetHelper.AssignRsGuid(rs, "flight_connection_id", mappings[i].flight_connection_id);
                RecordsetHelper.AssignRsString(rs, "seat_number", mappings[i].seat_number);
                RecordsetHelper.AssignRsString(rs, "seat_fee_rcd", mappings[i].seat_fee_rcd);
                RecordsetHelper.AssignRsBoolean(rs, "advanced_seating_flag", mappings[i].advanced_seating_flag);
            }

            return rs;
        }
        #endregion

        #region Fabricate Recordset
        private static ADODB.Recordset FabricateRecordset()
        {
            ADODB.Recordset rs = new ADODB.Recordset();

            rs.Fields.Append("passenger_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_segment_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("origin_rcd", ADODB.DataTypeEnum.adVarChar, 5, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("destination_rcd", ADODB.DataTypeEnum.adVarChar, 5, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passenger_type_rcd", ADODB.DataTypeEnum.adVarChar, 5, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("flight_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("airline_rcd", ADODB.DataTypeEnum.adVarChar, 3, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("flight_number", ADODB.DataTypeEnum.adVarChar, 5, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("boarding_class_rcd", ADODB.DataTypeEnum.adVarChar, 2, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_class_rcd", ADODB.DataTypeEnum.adVarChar, 2, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("departure_date", ADODB.DataTypeEnum.adDate, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("inventory_class_rcd", ADODB.DataTypeEnum.adVarChar, 3, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("currency_rcd", ADODB.DataTypeEnum.adVarChar, 3, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("net_total", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("tax_amount", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fare_amount", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("yq_amount", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("ticketing_fee_amount", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("reservation_fee_amount", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("commission_amount", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("commission_percentage", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("refund_charge", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fare_vat", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("tax_vat", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("yq_vat", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("ticketing_fee_vat", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("reservation_fee_vat", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fare_amount_incl", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("tax_amount_incl", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("yq_amount_incl", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("commission_amount_incl", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("ticketing_fee_amount_incl", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("reservation_fee_amount_incl", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("od_origin_rcd", ADODB.DataTypeEnum.adVarChar, 5, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("od_destination_rcd", ADODB.DataTypeEnum.adVarChar, 5, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fare_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fare_code", ADODB.DataTypeEnum.adVarChar, 20, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("e_ticket_flag", ADODB.DataTypeEnum.adBoolean, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("refundable_flag", ADODB.DataTypeEnum.adBoolean, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("transferable_fare_flag", ADODB.DataTypeEnum.adBoolean, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("duty_travel_flag", ADODB.DataTypeEnum.adBoolean, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("it_fare_flag", ADODB.DataTypeEnum.adBoolean, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passenger_status_rcd", ADODB.DataTypeEnum.adVarChar, 5, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("restriction_text", ADODB.DataTypeEnum.adVarChar, 300, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("baggage_weight", ADODB.DataTypeEnum.adDecimal, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fare_type_rcd", ADODB.DataTypeEnum.adVarChar, 10, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("refund_with_charge_hours", ADODB.DataTypeEnum.adUnsignedTinyInt, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("refund_not_possible_hours", ADODB.DataTypeEnum.adUnsignedTinyInt, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("piece_allowance", ADODB.DataTypeEnum.adUnsignedTinyInt, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("redemption_points", ADODB.DataTypeEnum.adDouble, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fare_date_time", ADODB.DataTypeEnum.adDBTimeStamp, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_name", ADODB.DataTypeEnum.adVarChar, 60, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_name", ADODB.DataTypeEnum.adVarChar, 60, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("agency_code", ADODB.DataTypeEnum.adVarChar, 20, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("flight_connection_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("seat_number", ADODB.DataTypeEnum.adVarChar, 20, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("seat_fee_rcd", ADODB.DataTypeEnum.adVarChar, 20, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("advanced_seating_flag", ADODB.DataTypeEnum.adBoolean, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);

            //Open Recordset
            rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient;
            rs.Open(System.Reflection.Missing.Value,
                    System.Reflection.Missing.Value,
                    ADODB.CursorTypeEnum.adOpenStatic,
                    ADODB.LockTypeEnum.adLockOptimistic, 0);

            return rs;
        }
        #endregion

        #region Method Extension
        public static IList<MappingView> CreateMappingView(this IList<Mapping> mappings)
        {
            if (mappings != null && mappings.Count > 0)
            {
                IList<MappingView> mps = new List<MappingView>();
                MappingView mv;
                for (int i = 0; i < mappings.Count; i++)
                {
                    mv = new MappingView();
                    mv.booking_segment_id = mappings[i].booking_segment_id;
                    mv.passenger_type_rcd = mappings[i].passenger_type_rcd;
                    mv.passenger_id = mappings[i].passenger_id;
                    mv.origin_rcd = mappings[i].origin_rcd;
                    mv.destination_rcd = mappings[i].destination_rcd;
                    mv.od_origin_rcd = mappings[i].od_origin_rcd;
                    mv.od_destination_rcd = mappings[i].od_destination_rcd;
                    mv.booking_class_rcd = mappings[i].booking_class_rcd;
                    mv.currency_rcd = mappings[i].currency_rcd;
                    mv.fare_code = mappings[i].fare_code;
                    mv.airline_rcd = mappings[i].airline_rcd;
                    mv.flight_number = mappings[i].flight_number;
                    mv.departure_date = mappings[i].departure_date;
                    mv.piece_allowance = mappings[i].piece_allowance;
                    mv.baggage_weight = mappings[i].baggage_weight;
                    mv.agency_code = mappings[i].agency_code;

                    mps.Add(mv);
                    mv = null;
                }
                return mps;
            }
            return null;
        }
        //public static IList<SeatMappingView> CreateSeatMappingView(this IList<Mapping> mappings)
        //{
        //    if (mappings != null && mappings.Count > 0)
        //    {
        //        IList<SeatMappingView> mps = new List<SeatMappingView>();
        //        for (int i = 0; i < mappings.Count; i++)
        //        {
        //            mps.Add(mappings[i].ToSeatMappingView());
        //        }
        //        return mps;
        //    }
        //    return null;
        //}
        public static IList<MappingView> CreateMappingView(this IList<Mapping> mappings, Guid bookingSegmentId)
        {
            if (mappings != null && mappings.Count > 0)
            {
                IList<MappingView> mps = new List<MappingView>();
                MappingView mv;
                for (int i = 0; i < mappings.Count; i++)
                {
                    if (mappings[i].booking_segment_id.Equals(bookingSegmentId))
                    {
                        mv = new MappingView();
                        mv.booking_segment_id = mappings[i].booking_segment_id;
                        mv.passenger_type_rcd = mappings[i].passenger_type_rcd;
                        mv.passenger_id = mappings[i].passenger_id;
                        mv.origin_rcd = mappings[i].origin_rcd;
                        mv.destination_rcd = mappings[i].destination_rcd;
                        mv.od_origin_rcd = mappings[i].od_origin_rcd;
                        mv.od_destination_rcd = mappings[i].od_destination_rcd;
                        mv.booking_class_rcd = mappings[i].booking_class_rcd;
                        mv.currency_rcd = mappings[i].currency_rcd;
                        mv.fare_code = mappings[i].fare_code;
                        mv.airline_rcd = mappings[i].airline_rcd;
                        mv.flight_number = mappings[i].flight_number;
                        mv.departure_date = mappings[i].departure_date;
                        mv.piece_allowance = mappings[i].piece_allowance;
                        mv.baggage_weight = mappings[i].baggage_weight;
                        mv.agency_code = mappings[i].agency_code;

                        mps.Add(mv);
                        mv = null;
                    }
                }
                return mps;
            }
            return null;
        }
        public static IList<Mapping> CreateMapping(this IList<MappingView> mappingViews)
        {
            if (mappingViews != null && mappingViews.Count > 0)
            {
                IList<Mapping> ms = new List<Mapping>();
                Mapping m;
                for (int i = 0; i < mappingViews.Count; i++)
                {
                    m = new Mapping();

                    m.booking_segment_id = mappingViews[i].booking_segment_id;
                    m.passenger_type_rcd = mappingViews[i].passenger_type_rcd;
                    m.passenger_id = mappingViews[i].passenger_id;
                    m.origin_rcd = mappingViews[i].origin_rcd;
                    m.destination_rcd = mappingViews[i].destination_rcd;
                    m.od_origin_rcd = mappingViews[i].od_origin_rcd;
                    m.od_destination_rcd = mappingViews[i].od_destination_rcd;
                    m.booking_class_rcd = mappingViews[i].booking_class_rcd;
                    m.currency_rcd = mappingViews[i].currency_rcd;
                    m.fare_code = mappingViews[i].fare_code;
                    m.airline_rcd = mappingViews[i].airline_rcd;
                    m.flight_number = mappingViews[i].flight_number;
                    m.departure_date = mappingViews[i].departure_date;
                    m.piece_allowance = mappingViews[i].piece_allowance;
                    m.baggage_weight = mappingViews[i].baggage_weight;
                    m.agency_code = mappingViews[i].agency_code;

                    ms.Add(m);
                    m = null;
                }
                return ms;
            }
            return null;
        }
        //public static IList<Mapping> CreateMapping(this IList<SeatMappingView> seatMappingViews)
        //{
        //    if (seatMappingViews != null && seatMappingViews.Count > 0)
        //    {
        //        IList<Mapping> ms = new List<Mapping>();
        //        for (int i = 0; i < seatMappingViews.Count; i++)
        //        {
        //            ms.Add(seatMappingViews[i].ToMapping());
        //        }
        //        return ms;
        //    }
        //    return null;
        //}
        #endregion
    }
}
