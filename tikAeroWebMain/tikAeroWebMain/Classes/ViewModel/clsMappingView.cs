using System;
using System.Collections.Generic;
using System.Text;
using tikSystem.Web.Library;

namespace tikAeroWebMain
{
    public class MappingView
    {
        #region Property
        Guid _booking_segment_id;
        public Guid booking_segment_id
        {
            get { return _booking_segment_id;}
            set { _booking_segment_id = value; }
        }
        
        string _passenger_type_rcd;
        public string passenger_type_rcd
        {
            get { return _passenger_type_rcd; }
            set { _passenger_type_rcd = value; }
        }

        Guid _passenger_id;
        public Guid passenger_id
        {
            get { return _passenger_id; }
            set { _passenger_id = value; }
        }

        string _origin_rcd;
        public string origin_rcd
        {
            get { return _origin_rcd; }
            set { _origin_rcd = value; }
        }

        string _destination_rcd;
        public string destination_rcd
        {
            get { return _destination_rcd; }
            set { _destination_rcd = value; }
        }

        string _od_origin_rcd;
        public string od_origin_rcd
        {
            get { return _od_origin_rcd; }
            set { _od_origin_rcd = value; }
        }

        string _od_destination_rcd;
        public string od_destination_rcd
        {
            get { return _od_destination_rcd; }
            set { _od_destination_rcd = value; }
        }

        string _booking_class_rcd;
        public string booking_class_rcd
        {
            get { return _booking_class_rcd; }
            set { _booking_class_rcd = value; }
        }

        string _currency_rcd;
        public string currency_rcd
        {
            get { return _currency_rcd; }
            set { _currency_rcd = value; }
        }

        string _fare_code;
        public string fare_code
        {
            get { return _fare_code; }
            set { _fare_code = value; }
        }

        string _airline_rcd;
        public string airline_rcd
        {
            get { return _airline_rcd; }
            set { _airline_rcd = value; }
        }

        string _flight_number;
        public string flight_number
        {
            get { return _flight_number; }
            set { _flight_number = value; }
        }

        DateTime _departure_date;
        public DateTime departure_date
        {
            get { return _departure_date; }
            set { _departure_date = value; }
        }

        Int16 _piece_allowance;
        public Int16 piece_allowance
        {
            get { return _piece_allowance; }
            set { _piece_allowance = value; }
        }

        decimal _baggage_weight;
        public decimal baggage_weight
        {
            get { return _baggage_weight; }
            set { _baggage_weight = value; }
        }

        string _agency_code;
        public string agency_code
        {
            get { return _agency_code; }
            set { _agency_code = value; }
        }
        #endregion
        
        #region Method
        public Mapping ToMapping()
        {
            Mapping m = new Mapping();

            m.booking_segment_id = _booking_segment_id;
            m.passenger_type_rcd = _passenger_type_rcd;
            m.passenger_id = _passenger_id;
            m.origin_rcd = _origin_rcd;
            m.destination_rcd = _destination_rcd;
            m.od_origin_rcd = _od_origin_rcd;
            m.od_destination_rcd = _od_destination_rcd;
            m.booking_class_rcd = _booking_class_rcd;
            m.currency_rcd = _currency_rcd;
            m.fare_code = _fare_code;
            m.airline_rcd = _airline_rcd;
            m.flight_number = _flight_number;
            m.departure_date = _departure_date;
            m.piece_allowance = _piece_allowance;
            m.baggage_weight = _baggage_weight;
            m.agency_code = _agency_code;

            return m;
        }
        #endregion
    }
}
