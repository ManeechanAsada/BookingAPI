using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class APIPassengerService
    {
        //checkin API
        #region Property
        Guid _passenger_segment_service_id = Guid.Empty;
        public Guid passenger_segment_service_id
        {
            get { return _passenger_segment_service_id; }
            set { _passenger_segment_service_id = value; }
        }

        Guid _passenger_id = Guid.Empty;
        public Guid passenger_id
        {
            get { return _passenger_id; }
            set { _passenger_id = value; }
        }

        Guid _booking_segment_id = Guid.Empty;
        public Guid booking_segment_id
        {
            get { return _booking_segment_id; }
            set { _booking_segment_id = value; }
        }

        string _special_service_status_rcd = string.Empty;
        public string special_service_status_rcd
        {
            get { return _special_service_status_rcd; }
            set { _special_service_status_rcd = value; }
        }

        string _special_service_change_status_rcd = string.Empty;
        public string special_service_change_status_rcd
        {
            get { return _special_service_change_status_rcd; }
            set { _special_service_change_status_rcd = value; }
        }

        string _special_service_rcd = string.Empty;
        public string special_service_rcd
        {
            get { return _special_service_rcd; }
            set { _special_service_rcd = value; }
        }

        string _service_text;
        public string service_text
        {
            get { return _service_text; }
            set { _service_text = value; }
        }

        string _flight_id;
        public string flight_id
        {
            get { return _flight_id; }
            set { _flight_id = value; }
        }

        string _fee_id;
        public string fee_id
        {
            get { return _fee_id; }
            set { _fee_id = value; }
        }

        Int16 _number_of_units;
        public Int16 number_of_units
        {
            get { return _number_of_units; }
            set { _number_of_units = value; }
        }

        string _origin_rcd = string.Empty;
        public string origin_rcd
        {
            get { return _origin_rcd; }
            set { _origin_rcd = value; }
        }

        string _destination_rcd = string.Empty;
        public string destination_rcd
        {
            get { return _destination_rcd; }
            set { _destination_rcd = value; }
        }

        string _display_name = string.Empty;
        public string display_name
        {
            get { return _display_name; }
            set { _display_name = value; }
        }
        #endregion
    }
}
