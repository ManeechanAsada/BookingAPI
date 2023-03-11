using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tikAeroWebMain
{
    public class SegmentService
    {
        Guid _flight_connection_id;
        public Guid flight_connection_id
        {
            get { return _flight_connection_id; }
            set { _flight_connection_id = value; }
        }
        string _special_service_rcd = string.Empty;
        public string special_service_rcd
        {
            get { return _special_service_rcd; }
            set { _special_service_rcd = value; }
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
    }
}
