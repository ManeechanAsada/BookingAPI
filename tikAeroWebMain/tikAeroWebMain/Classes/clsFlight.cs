using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace tikAeroWebMain.Classes
{
    public class FlightsRequest
    {
        public DateTime departure_date_from;
        public DateTime departure_date_to;
        public string airline_rcd;
        public string flight_number;
        public string origin_rcd;
        public string destination_rcd;
    }

    public class FlightsResponse
    {
        public Flights[] Flights;
        public string result;
        public int errorCode;
        public string errorMessage;
    }

    public class FlightLegs
    {
        public string flight_id;
        public string departure_airport_rcd;
        public DateTime? departure_date;
        public DateTime? utc_departure_date_time;
        public string planned_departure_time;
        public string arrival_airport_rcd;
        public DateTime? arrival_date;
        public DateTime? utc_arrival_date_time;
        public string planned_arrival_time;
    }

    public class Flights
    {
        public string flight_id;
        public DateTime? utc_departure_date;
        public string airline_rcd;
        public string flight_number;
        public string flight_status_rcd;
        public string aircraft_type_rcd;
        public string matriculation_rcd;
        public string operating_airline_rcd;
        public string operating_flight_number;
        public string origin_rcd;
        public string destination_rcd;
        public string flight_comment;
        public string internal_comment;
        public string controlling_agency_code;
        public string schedule_id;
        public byte? free_seating_flag;
        public byte? auto_open_checkin_flag;
        public byte? allow_web_checkin_flag;
        public string flight_information_1;
        public string flight_information_2;
        public string flight_information_3;
        public byte? exclude_statistics_flag;
        public string dot_reporting_date_time;
        public FlightLegs[] flight_legs;
    }

    public class Flight
    {
        private string connectionString = "";
        public string lastErrorMessage = "";
        public int lastErrorCode = 0;

        public Flight(string connectionString)
        {
            this.connectionString = connectionString;
        }
        private string strDate(DateTime dt)
        {
            if (dt == DateTime.MinValue) return "";
            return dt.Year.ToString("0000") + dt.Month.ToString("00") + dt.Day.ToString("00");
        }
        FlightsResponse ReturnFlightsErrorCode(int errorCode, string errorMessage)
        {
            this.lastErrorCode = errorCode;
            this.lastErrorMessage = errorMessage;
            return ReturnFlightsErrorCode();
        }
        FlightsResponse ReturnFlightsErrorCode()
        {
            FlightsResponse fres = new FlightsResponse();
            fres.errorCode = this.lastErrorCode;
            fres.errorMessage = this.lastErrorMessage;
            if (this.lastErrorCode == 0) fres.result = "OK"; else fres.result = "NOK";
            return fres;
        }


        public FlightsResponse get_flights(FlightsRequest freq)
        {
            string sqlCmd = "";
            FlightsResponse fres = new FlightsResponse();
            try
            {
                DataSet flights = new DataSet();
                #region retrieve flight_new table
                sqlCmd += "SELECT TOP 2000 f.* FROM flight_new f WITH(NOLOCK) ";
                sqlCmd += " INNER JOIN flight_segment fs WITH(NOLOCK) ";
                sqlCmd += " ON fs.flight_id = f.flight_id ";
                sqlCmd += "WHERE 1=1 ";

                if (freq.departure_date_from != null && freq.departure_date_from > DateTime.MinValue)
                    sqlCmd += "   AND fs.departure_date BETWEEN '" + strDate(freq.departure_date_from) + "' AND '" + strDate(freq.departure_date_to) + "'";

                if (freq.airline_rcd != "" && freq.airline_rcd != null)
                    sqlCmd += "   AND fs.airline_rcd     = '" + freq.airline_rcd + "' ";

                if (freq.flight_number != "" && freq.flight_number != null)
                    sqlCmd += "   AND fs.flight_number   = '" + freq.flight_number + "' ";

                if (freq.origin_rcd != "" && freq.origin_rcd != null)
                    sqlCmd += "   AND fs.origin_rcd      = '" + freq.origin_rcd + "' ";

                if (freq.destination_rcd != "" && freq.destination_rcd != null)
                    sqlCmd += "   AND fs.destination_rcd = '" + freq.destination_rcd + "' ";


                SqlConnection cnn = new SqlConnection(this.connectionString);
                SqlDataAdapter da = new SqlDataAdapter(sqlCmd, cnn);
                cnn.Open();
                da.Fill(flights, "Flights");

                if (flights.Tables["Flights"].Rows.Count <= 0)
                {
                    //No data
                    return ReturnFlightsErrorCode(1012, "Record not found");
                }
                else
                {
                    #region Load data to flightArray
                    Flights[] flightsArray = new Flights[flights.Tables["Flights"].Rows.Count];
                    for (int i = 0; i < flights.Tables["Flights"].Rows.Count; i++)
                    {
                        flightsArray[i] = new Flights();
                        flightsArray[i].flight_id = DbGuIdToString(flights.Tables["Flights"].Rows[i]["flight_id"]);

                        if (flights.Tables["Flights"].Rows[i]["utc_departure_date"] != DBNull.Value)
                            flightsArray[i].utc_departure_date = (DateTime)flights.Tables["Flights"].Rows[i]["utc_departure_date"];

                        flightsArray[i].airline_rcd = DbObjectToString(flights.Tables["Flights"].Rows[i]["airline_rcd"]);
                        flightsArray[i].flight_number = DbObjectToString(flights.Tables["Flights"].Rows[i]["flight_number"]);
                        flightsArray[i].flight_status_rcd = DbObjectToString(flights.Tables["Flights"].Rows[i]["flight_status_rcd"]);
                        flightsArray[i].aircraft_type_rcd = DbObjectToString(flights.Tables["Flights"].Rows[i]["aircraft_type_rcd"]);
                        flightsArray[i].matriculation_rcd = DbObjectToString(flights.Tables["Flights"].Rows[i]["matriculation_rcd"]);
                        flightsArray[i].operating_airline_rcd = DbObjectToString(flights.Tables["Flights"].Rows[i]["operating_airline_rcd"]);
                        flightsArray[i].operating_flight_number = DbObjectToString(flights.Tables["Flights"].Rows[i]["operating_flight_number"]);
                        flightsArray[i].origin_rcd = DbObjectToString(flights.Tables["Flights"].Rows[i]["origin_rcd"]);
                        flightsArray[i].destination_rcd = DbObjectToString(flights.Tables["Flights"].Rows[i]["destination_rcd"]);
                        flightsArray[i].flight_comment = DbObjectToString(flights.Tables["Flights"].Rows[i]["flight_comment"]);
                        flightsArray[i].internal_comment = DbObjectToString(flights.Tables["Flights"].Rows[i]["internal_comment"]);
                        flightsArray[i].controlling_agency_code = DbObjectToString(flights.Tables["Flights"].Rows[i]["controlling_agency_code"]);
                        flightsArray[i].schedule_id = DbGuIdToString(flights.Tables["Flights"].Rows[i]["schedule_id"]);
                        flightsArray[i].free_seating_flag = DbObjectToByte(flights.Tables["Flights"].Rows[i]["free_seating_flag"]);
                        flightsArray[i].auto_open_checkin_flag = DbObjectToByte(flights.Tables["Flights"].Rows[i]["auto_open_checkin_flag"]);
                        flightsArray[i].allow_web_checkin_flag = DbObjectToByte(flights.Tables["Flights"].Rows[i]["allow_web_checkin_flag"]);
                        flightsArray[i].flight_information_1 = DbObjectToString(flights.Tables["Flights"].Rows[i]["flight_information_1"]);
                        flightsArray[i].flight_information_2 = DbObjectToString(flights.Tables["Flights"].Rows[i]["flight_information_2"]);
                        flightsArray[i].flight_information_3 = DbObjectToString(flights.Tables["Flights"].Rows[i]["flight_information_3"]);
                        flightsArray[i].exclude_statistics_flag = DbObjectToByte(flights.Tables["Flights"].Rows[i]["exclude_statistics_flag"]);
                        flightsArray[i].dot_reporting_date_time = DbObjectToString(flights.Tables["Flights"].Rows[i]["dot_reporting_date_time"]);

                        #region get flight legs
                        //Use stored procedure
                        SqlCommand cmdFlightLeg = new SqlCommand("get_flight_details", cnn);
                        cmdFlightLeg.CommandType = CommandType.StoredProcedure;
                        cmdFlightLeg.Parameters.Add("@origin", SqlDbType.VarChar).Value = flightsArray[i].origin_rcd;
                        cmdFlightLeg.Parameters.Add("@destination", SqlDbType.VarChar).Value = flightsArray[i].destination_rcd;
                        cmdFlightLeg.Parameters.Add("@flightid", SqlDbType.VarChar).Value = "{" + flightsArray[i].flight_id + "}";

                        SqlDataAdapter daLeg = new SqlDataAdapter(cmdFlightLeg);
                        DataSet dsLegs = new DataSet();
                        daLeg.Fill(dsLegs, "FlightLegs");
                        if (dsLegs.Tables["FlightLegs"].Rows.Count > 0)
                        {
                            FlightLegs[] fl = new FlightLegs[dsLegs.Tables["FlightLegs"].Rows.Count];
                            for (int j = 0; j < dsLegs.Tables["FlightLegs"].Rows.Count; j++)
                            {
                                fl[j] = new FlightLegs();
                                fl[j].flight_id = DbGuIdToString(dsLegs.Tables["FlightLegs"].Rows[j]["flight_id"]);
                                fl[j].departure_airport_rcd = DbObjectToString(dsLegs.Tables["FlightLegs"].Rows[j]["origin_rcd"]);
                                fl[j].departure_date = DbObjToDateTime(dsLegs.Tables["FlightLegs"].Rows[j]["departure_date"]);
                                fl[j].utc_departure_date_time = DbObjToDateTime(dsLegs.Tables["FlightLegs"].Rows[j]["utc_departure_date"]);
                                fl[j].planned_departure_time = DbObjectToHHmm(dsLegs.Tables["FlightLegs"].Rows[j]["planned_departure_time"]);
                                fl[j].arrival_airport_rcd = DbObjectToString(dsLegs.Tables["FlightLegs"].Rows[j]["destination_rcd"]);
                                fl[j].arrival_date = DbObjToDateTime(dsLegs.Tables["FlightLegs"].Rows[j]["arrival_date"]);
                                fl[j].utc_arrival_date_time = DbObjToDateTime(dsLegs.Tables["FlightLegs"].Rows[j]["arrival_date"]);
                                fl[j].planned_arrival_time = DbObjectToHHmm(dsLegs.Tables["FlightLegs"].Rows[j]["planned_arrival_time"]);
                            }
                            flightsArray[i].flight_legs = fl;
                        }
                        #endregion

                    }
                    #endregion

                    fres.Flights = flightsArray;
                    fres.errorCode = 0;
                    fres.errorMessage = "";
                    fres.result = "OK";
                    this.lastErrorCode = 0;
                    this.lastErrorMessage = "";
                }
                #endregion

                return fres;
            }
            catch (SqlException se)
            {
                this.lastErrorCode = se.Number;
                this.lastErrorMessage = "Sql Exception : function get_flights(...) " + se.Message;
                return ReturnFlightsErrorCode();
            }
            catch (Exception ex)
            {
                this.lastErrorCode = 9999;
                this.lastErrorMessage = "Exception : function get_flights(...) " + ex.Message;
                return ReturnFlightsErrorCode();
            }
        }

        #region Convert data
        private DateTime ConvDb(object obj)
        {
            if (obj == System.DBNull.Value) return DateTime.MinValue; else return (DateTime)obj;
        }
        private string DbObjectToString(object obj)
        {
            if (obj == System.DBNull.Value) return ""; else return (string)obj;
        }
        private string DbGuIdToString(object obj)
        {
            try
            {
                if (obj == System.DBNull.Value)
                    return "";
                else
                {
                    Guid gu = new Guid();
                    gu = (Guid)obj;
                    return gu.ToString();
                }
            }
            catch
            {
                return "";
            }
        }
        private int? DbObjectToInt32(object obj)
        {
            if (obj == System.DBNull.Value) return null; else return Convert.ToInt32(obj);
        }
        private string DbObjectToHHmm(object obj)
        {
            try
            {
                if (obj == System.DBNull.Value)
                {
                    return "";
                }
                else
                {
                    int tm = Convert.ToInt32(obj);
                    return tm.ToString("0000");
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private byte? DbObjectToByte(object obj)
        {
            if (obj == System.DBNull.Value) return null; else return (byte)obj;
        }
        private DateTime? DbObjToDateTime(object obj)
        {
            if (obj == DBNull.Value) return null;
            return (DateTime)obj;
        }
        #endregion

    }
}