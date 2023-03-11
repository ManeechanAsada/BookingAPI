using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Data;
using Avantik.Web.Service.COMHelper;
using System.Configuration;
using System.Data.SqlClient;

namespace tikAeroWebMain
{
    public class ComplusReservation : RunComplus
    {
        public ComplusReservation() : base() { }

        public tikSystem.Web.Library.Titles GetPassengerTitles(string language)
        {
            tikAeroProcess.Reservation objReservation = null;
            ADODB.Recordset rs = null;
            tikSystem.Web.Library.Titles titles = new tikSystem.Web.Library.Titles();
            try
            {
                if (string.IsNullOrEmpty(_server) == false)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Reservation", _server);
                    objReservation = (tikAeroProcess.Reservation)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objReservation = new tikAeroProcess.Reservation(); }

                if (string.IsNullOrEmpty(language) == true)
                { language = "EN"; }
                rs = objReservation.GetPassengerTitles(ref language);

                //convert Recordset to Object
                if (rs != null && rs.RecordCount > 0)
                {
                    tikSystem.Web.Library.Title t;
                    rs.MoveFirst();
                    while (!rs.EOF)
                    {
                        t = new tikSystem.Web.Library.Title();

                        t.title_rcd = RecordsetHelper.ToString(rs, "title_rcd");
                        t.display_name = RecordsetHelper.ToString(rs, "display_name");
                        t.gender_type_rcd = RecordsetHelper.ToString(rs, "gender_type_rcd");

                        titles.Add(t);
                        t = null;
                        rs.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objReservation != null)
                {
                    Marshal.FinalReleaseComObject(objReservation);
                    objReservation = null;
                }

                RecordsetHelper.ClearRecordset(ref rs);
                base.Dispose();
            }

            return titles;
        }
        public tikSystem.Web.Library.Languages GetLanguages(string language)
        {
            tikAeroProcess.Reservation objReservation = null;
            ADODB.Recordset rs = null;

            tikSystem.Web.Library.Languages languages = new tikSystem.Web.Library.Languages();
            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Reservation", _server);
                    objReservation = (tikAeroProcess.Reservation)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objReservation = new tikAeroProcess.Reservation(); }
                //set default language to EN
                if (language.Length == 0)
                { language = "EN"; }
                rs = objReservation.GetLanguages(ref language);

                //convert Recordset to Object
                if (rs != null && rs.RecordCount > 0)
                {
                    tikSystem.Web.Library.Language l;
                    rs.MoveFirst();
                    while (!rs.EOF)
                    {
                        l = new tikSystem.Web.Library.Language();

                        l.language_rcd = RecordsetHelper.ToString(rs, "language_rcd");
                        l.display_name = RecordsetHelper.ToString(rs, "display_name");
                        l.character_set = RecordsetHelper.ToString(rs, "character_set");

                        languages.Add(l);
                        l = null;
                        rs.MoveNext();
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objReservation != null)
                { Marshal.FinalReleaseComObject(objReservation); }
                objReservation = null;

                RecordsetHelper.ClearRecordset(ref rs);
                base.Dispose();
            }

            return languages;
        }
        public DataSet GetTicketSales(string strAgencyCode,
                                     string strUserID,
                                     string strOrigin,
                                     string strDestination,
                                     string strAirline,
                                     string strFligtNumber,
                                     DateTime dteDateFrom,
                                     DateTime dteDateTo,
                                     DateTime dteTicketingFrom,
                                     DateTime dteTicketingTo,
                                     string strPassengerType,
                                     string strLanguage)
        {

            tikAeroProcess.Reservation objReservation = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Reservation", _server);
                    objReservation = (tikAeroProcess.Reservation)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objReservation = new tikAeroProcess.Reservation(); }
                rs = objReservation.GetTicketSales(ref strAgencyCode,
                                                    ref strUserID,
                                                    ref strOrigin,
                                                    ref strDestination,
                                                    ref strAirline,
                                                    ref strFligtNumber,
                                                    ref dteDateFrom,
                                                    ref dteDateTo,
                                                    ref dteTicketingFrom,
                                                    ref dteTicketingTo,
                                                    ref strPassengerType,
                                                    ref strLanguage);
                ds = RecordsetToDataset(rs, "TicketSales");
            }
            catch
            { }
            finally
            {
                if (objReservation != null)
                { Marshal.FinalReleaseComObject(objReservation); }
                objReservation = null;

                ClearRecordset(ref rs);
            }
            return ds;
        }

 

        public DataSet GetBookings(string Airline,
                            string FlightNumber,
                            string FlightId,
                            string FlightFrom,
                            string FlightTo,
                            string RecordLocator,
                            string Origin,
                            string Destination,
                            string PassengerName,
                            string SeatNumber,
                            string TicketNumber,
                            string PhoneNumber,
                            string AgencyCode,
                            string ClientNumber,
                            string MemberNumber,
                            string ClientId,
                            bool ShowHistory,
                            string Language,
                            bool bIndividual,
                            bool bGroup,
                            string CreateFrom,
                            string CreateTo)
        {

            DataSet ds = null;

            #region Directly Connect


            //         If dtFlightFrom = vbEmpty Then
            //    strDateFrom = ""
            //Else
            //    strDateFrom = DTE(dtFlightFrom)
            //End If

            //If dtFlightTo = vbEmpty Then
            //    strDateTo = strDateFrom
            //Else
            //    strDateTo = DTE(dtFlightTo)
            //End If

            //If dtCreateFrom = vbEmpty Then
            //    strCreateFrom = ""
            //Else
            //    strCreateFrom = DTE(dtCreateFrom)
            //End If

            //If dtCreateTo = vbEmpty Then
            //    strCreateTo = ""
            //Else
            //    strCreateTo = DTE(dtCreateTo)
            //End If

            //strPassengerName = Trim$(strPassengerName)

            //If Val(strPassengerName) > 0 Then
            //    strPhoneNumber = strPassengerName
            //ElseIf InStr(strPassengerName, "/") > 0 Then
            //    strName() = Split(strPassengerName, "/")
            //    strName1 = Trim$(strName(0))
            //    strName2 = Trim$(strName(1))
            //ElseIf Len(strPassengerName) > 0 Then
            //    strName() = Split(strPassengerName, " ")
            //    strName1 = Trim$(strName(0))
            //    If UBound(strName) > 0 Then
            //        strName2 = Trim$(strName(1))
            //    End If
            //End If




            string connectionString = ConfigurationManager.ConnectionStrings["TikAeroConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand sqlComm = new SqlCommand("get_bookings", conn);
                sqlComm.Parameters.AddWithValue("@origin", "");
                sqlComm.Parameters.AddWithValue("@destination", "");
                sqlComm.Parameters.AddWithValue("@flightid", "");
                sqlComm.Parameters.AddWithValue("@airline", "");
                sqlComm.Parameters.AddWithValue("@flight", "");
                sqlComm.Parameters.AddWithValue("@datefrom", "");
                sqlComm.Parameters.AddWithValue("@dateto", "99980706");
                sqlComm.Parameters.AddWithValue("@recordlocator", "");
                sqlComm.Parameters.AddWithValue("@ticketnumber", "");
                sqlComm.Parameters.AddWithValue("@name1", "");
                sqlComm.Parameters.AddWithValue("@name2", "");
                sqlComm.Parameters.AddWithValue("@phone", "");
                sqlComm.Parameters.AddWithValue("@seatnumber", "");
                sqlComm.Parameters.AddWithValue("@agencycode", "");
                sqlComm.Parameters.AddWithValue("@clientid", "");
                sqlComm.Parameters.AddWithValue("@includecancelled", 0);
                sqlComm.Parameters.AddWithValue("@includehistory", true);
                sqlComm.Parameters.AddWithValue("@includeindividuals", true);
                sqlComm.Parameters.AddWithValue("@includegroups", false);
                sqlComm.Parameters.AddWithValue("@language", "EN");
                sqlComm.Parameters.AddWithValue("@createfrom", "");
                sqlComm.Parameters.AddWithValue("@createto", "");
                sqlComm.Parameters.AddWithValue("@iata", "");
                sqlComm.Parameters.AddWithValue("@userid", "");
                sqlComm.Parameters.AddWithValue("@memberNumber", MemberNumber);

                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);
            }


            #endregion

            #region old way
            //tikAeroProcess.Reservation objReservation = null;
            //ADODB.Recordset rs = null;
            //DataSet ds = null;

            //try
            //{
            //    if (_server.Length > 0)
            //    {
            //        Type remote = Type.GetTypeFromProgID("tikAeroProcess.Reservation", _server);
            //        objReservation = (tikAeroProcess.Reservation)Activator.CreateInstance(remote);
            //        remote = null;
            //    }
            //    else
            //    { objReservation = new tikAeroProcess.Reservation(); }
            //    rs = objReservation.GetBookings(ref Airline,
            //                                    ref FlightNumber,
            //                                    ref FlightId,
            //                                    ref FlightFrom,
            //                                    ref FlightTo,
            //                                    ref RecordLocator,
            //                                    ref Origin,
            //                                    ref Destination,
            //                                    ref PassengerName,
            //                                    ref SeatNumber,
            //                                    ref TicketNumber,
            //                                    ref PhoneNumber,
            //                                    ref AgencyCode,
            //                                    ref ClientNumber,
            //                                    ref MemberNumber,
            //                                    ref ClientId,
            //                                    ref ShowHistory,
            //                                    ref bIndividual,
            //                                    ref bGroup,
            //                                    ref Language,
            //                                    ref CreateFrom,
            //                                    ref CreateTo);

            //    //Convert Recordset to Dataset
            //    ds = RecordsetToDataset(rs, "Booking");
            //    if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            //    {
            //        ds.DataSetName = "Bookings";
            //    }
            //}
            //catch
            //{ }
            //finally
            //{
            //    if (objReservation != null)
            //    { Marshal.FinalReleaseComObject(objReservation); }
            //    objReservation = null;

            //    ClearRecordset(ref rs);
            //}


            #endregion


            return ds;
        }


        public DataSet GetRemarkTypes(string language)
        {
            tikAeroProcess.Reservation objReservation = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Reservation", _server);
                    objReservation = (tikAeroProcess.Reservation)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objReservation = new tikAeroProcess.Reservation(); }
                //set default language to EN
                if (language.Length == 0)
                { language = "EN"; }

                rs = objReservation.GetRemarkTypes(ref language);
                ds = RecordsetToDataset(rs, "RemarkTypes");
            }
            catch
            { }
            finally
            {
                if (objReservation != null)
                {
                    Marshal.FinalReleaseComObject(objReservation);
                    objReservation = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public bool CompleteRemark(DataTable remark, string remarkId, string userId)
        {
            tikAeroProcess.Reservation objReservation = null;
            ADODB.Recordset rsRemark = DatasetToRecordset(remark);

            bool result = false;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Reservation", _server);
                    objReservation = (tikAeroProcess.Reservation)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objReservation = new tikAeroProcess.Reservation(); }

                result = objReservation.CompleteRemark(ref rsRemark, ref remarkId, ref userId);
            }
            catch
            { }
            finally
            {
                if (objReservation != null)
                {
                    Marshal.FinalReleaseComObject(objReservation);
                    objReservation = null;
                }

                ClearRecordset(ref rsRemark);
            }

            return result;
        }
        public DataSet GetActivities(string agencyCode,
                                    string remarkType,
                                    string nickname,
                                    DateTime timelimitFrom,
                                    DateTime timelimitTo,
                                    bool pendingOnly,
                                    bool incompleteOnly,
                                    bool includeRemarks,
                                    bool showUnassigned)
        {
            tikAeroProcess.Reservation objReservation = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Reservation", _server);
                    objReservation = (tikAeroProcess.Reservation)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objReservation = new tikAeroProcess.Reservation(); }

                rs = objReservation.GetActivities(ref agencyCode,
                                                ref remarkType,
                                                ref nickname,
                                                ref timelimitFrom,
                                                ref timelimitTo,
                                                ref pendingOnly,
                                                ref incompleteOnly,
                                                ref includeRemarks,
                                                ref showUnassigned);

                ds = RecordsetToDataset(rs, "Activity");
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    ds.DataSetName = "Activities";
                }
            }
            catch
            { }
            finally
            {
                if (objReservation != null)
                {
                    Marshal.FinalReleaseComObject(objReservation);
                    objReservation = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet GetBookingFees(string currency, DateTime dt, string agency, string type)
        {
            tikAeroProcess.Reservation objReservation = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Reservation", _server);
                    objReservation = (tikAeroProcess.Reservation)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objReservation = new tikAeroProcess.Reservation(); }

                rs = objReservation.GetBookingFees(ref currency, ref dt, ref agency, ref type);
                ds = RecordsetToDataset(rs, "BookingFees");
            }
            catch
            { }
            finally
            {
                if (objReservation != null)
                { Marshal.FinalReleaseComObject(objReservation); }
                objReservation = null;

                ClearRecordset(ref rs);
            }
            return ds;
        }
        public DataSet GetBookingsThisUser(string agencyCode,
                                        string userId,
                                        string airline,
                                        string flightNumber,
                                        DateTime flightFrom,
                                        DateTime flightTo,
                                        string recordLocator,
                                        string origin,
                                        string destination,
                                        string passengerName,
                                        string seatNumber,
                                        string ticketNumber,
                                        string phoneNumber,
                                        DateTime createFrom,
                                        DateTime createTo)
        {
            tikAeroProcess.Reservation objReservation = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Reservation", _server);
                    objReservation = (tikAeroProcess.Reservation)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objReservation = new tikAeroProcess.Reservation(); }
                rs = objReservation.GetBookingsThisUser(ref agencyCode,
                                                        ref userId,
                                                        ref airline,
                                                        ref flightNumber,
                                                        ref flightFrom,
                                                        ref flightTo,
                                                        ref recordLocator,
                                                        ref origin,
                                                        ref destination,
                                                        ref passengerName,
                                                        ref seatNumber,
                                                        ref ticketNumber,
                                                        ref phoneNumber,
                                                        ref createFrom,
                                                        ref createTo);

                ds = RecordsetToDataset(rs, "Bookings");
            }
            catch
            { }
            finally
            {
                if (objReservation != null)
                { Marshal.FinalReleaseComObject(objReservation); }
                objReservation = null;

                ClearRecordset(ref rs);
            }
            return ds;
        }

        public DataSet GetPassengerProfileSegments(string passengerProfileId)
        {
            tikAeroProcess.Reservation objReservation = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Reservation", _server);
                    objReservation = (tikAeroProcess.Reservation)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objReservation = new tikAeroProcess.Reservation(); }
                rs = objReservation.GetPassengerProfileSegments(ref passengerProfileId);

                ds = RecordsetToDataset(rs, "Bookings");
            }
            catch
            { }
            finally
            {
                if (objReservation != null)
                { Marshal.FinalReleaseComObject(objReservation); }
                objReservation = null;
                ClearRecordset(ref rs);
            }
            return ds;
        }
        public DataSet GetPassenger(string airline,
                                    string flightNumber,
                                    Guid flightID,
                                    DateTime flightFrom,
                                    DateTime flightTo,
                                    string recordLocator,
                                    string origin,
                                    string destination,
                                    string passengerName,
                                    string seatNumber,
                                    string ticketNumber,
                                    string phoneNumber,
                                    string passengerStatus,
                                    string checkInStatus,
                                    string clientNumber,
                                    string memberNumber,
                                    Guid clientID,
                                    Guid passengerId,
                                    bool booked,
                                    bool listed,
                                    bool eTicketOnly,
                                    bool includeCancelled,
                                    bool openSegments,
                                    bool showHistory,
                                    string language)
        {
            tikAeroProcess.Reservation objReservation = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Reservation", _server);
                    objReservation = (tikAeroProcess.Reservation)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objReservation = new tikAeroProcess.Reservation(); }

                string strFlightId = string.Empty;
                string strClientId = string.Empty;
                string strPassengerId = string.Empty;

                if (flightID.Equals(Guid.Empty) == false)
                {
                    strFlightId = flightID.ToString();
                }
                if (clientID.Equals(Guid.Empty) == false)
                {
                    strClientId = clientID.ToString();
                }
                if (passengerId.Equals(Guid.Empty) == false)
                {
                    strPassengerId = passengerId.ToString();
                }

                rs = objReservation.GetPassengers(ref airline,
                                                ref flightNumber,
                                                ref strFlightId,
                                                ref flightFrom,
                                                ref flightTo,
                                                ref recordLocator,
                                                ref origin,
                                                ref destination,
                                                ref passengerName,
                                                ref seatNumber,
                                                ref ticketNumber,
                                                ref phoneNumber,
                                                ref passengerStatus,
                                                ref checkInStatus,
                                                ref clientNumber,
                                                ref memberNumber,
                                                ref strClientId,
                                                ref strPassengerId,
                                                ref booked,
                                                ref listed,
                                                ref eTicketOnly,
                                                ref includeCancelled,
                                                ref openSegments,
                                                ref showHistory,
                                                ref language);

                //convert recordset to dataset.
                ds = RecordsetToDataset(rs, "PassengerManifest");
                if (ds != null)
                {
                    ds.DataSetName = "PassengersManifest";
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objReservation != null)
                { Marshal.FinalReleaseComObject(objReservation); }
                objReservation = null;
                ClearRecordset(ref rs);
            }
            return ds;
        }

        public DataSet GetQueueCount(string agency, bool unassigned)
        {
            tikAeroProcess.Reservation objReservation = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Reservation", _server);
                    objReservation = (tikAeroProcess.Reservation)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objReservation = new tikAeroProcess.Reservation(); }

                rs = objReservation.GetQueueCount(agency, unassigned);

                //convert recordset to dataset.
                ds = RecordsetToDataset(rs, "RemarkQueue");
                if (ds != null)
                {
                    ds.DataSetName = "RemarksQueue";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objReservation != null)
                { Marshal.FinalReleaseComObject(objReservation); }
                objReservation = null;
                ClearRecordset(ref rs);
            }
            return ds;
        }
        public tikSystem.Web.Library.Services GetSpecialServices(string strLanguage)
        {
            tikAeroProcess.Reservation objReservation = null;
            ADODB.Recordset rs = null;
            try
            {
                tikSystem.Web.Library.Services services = new tikSystem.Web.Library.Services();
                if (string.IsNullOrEmpty(_server) == false)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Reservation", _server);
                    objReservation = (tikAeroProcess.Reservation)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objReservation = new tikAeroProcess.Reservation(); }

                rs = objReservation.GetSpecialServices(strLanguage);
                //convert Recordset to Object
                if (rs != null && rs.RecordCount > 0)
                {
                    tikSystem.Web.Library.Service s;
                    rs.Filter = 0;
                    while (!rs.EOF)
                    {
                        s = new tikSystem.Web.Library.Service();

                        s.special_service_rcd = RecordsetHelper.ToString(rs, "special_service_rcd");
                        s.display_name = RecordsetHelper.ToString(rs, "display_name");
                        s.text_allowed_flag = RecordsetHelper.ToByte(rs, "text_allowed_flag");
                        s.manifest_flag = RecordsetHelper.ToByte(rs, "manifest_flag");
                        s.text_required_flag = RecordsetHelper.ToByte(rs, "text_required_flag");
                        s.service_on_request_flag = RecordsetHelper.ToByte(rs, "service_on_request_flag");
                        s.include_passenger_name_flag = RecordsetHelper.ToByte(rs, "include_passenger_name_flag");
                        s.include_flight_segment_flag = RecordsetHelper.ToByte(rs, "include_flight_segment_flag");
                        s.include_action_code_flag = RecordsetHelper.ToByte(rs, "include_action_code_flag");
                        s.include_number_of_service_flag = RecordsetHelper.ToByte(rs, "include_number_of_service_flag");
                        s.include_catering_flag = RecordsetHelper.ToByte(rs, "include_catering_flag");
                        s.include_passenger_assistance_flag = RecordsetHelper.ToByte(rs, "include_passenger_assistance_flag");
                        s.service_supported_flag = RecordsetHelper.ToByte(rs, "service_supported_flag");
                        s.send_interline_reply_flag = RecordsetHelper.ToByte(rs, "send_interline_reply_flag");
                        s.cut_off_time = RecordsetHelper.ToInt32(rs, "cut_off_time");
                        s.status_code = RecordsetHelper.ToString(rs, "status_code");
                        s.passenger_segment_service_id = RecordsetHelper.ToGuid(rs, "passenger_segment_service_id");
                        s.passenger_id = RecordsetHelper.ToGuid(rs, "passenger_id");
                        s.booking_segment_id = RecordsetHelper.ToGuid(rs, "booking_segment_id");
                        s.service_text = RecordsetHelper.ToString(rs, "service_text");
                        s.special_service_status_rcd = RecordsetHelper.ToString(rs, "special_service_status_rcd");

                        services.Add(s);
                        s = null;
                        rs.MoveNext();
                    }
                }

                return services;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objReservation != null)
                {
                    Marshal.FinalReleaseComObject(objReservation);
                    objReservation = null;
                }
                RecordsetHelper.ClearRecordset(ref rs);
                base.Dispose();
            }
        }
    }
}