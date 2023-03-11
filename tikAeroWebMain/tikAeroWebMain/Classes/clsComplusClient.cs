using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Data;
using Avantik.Web.Service.Helpers.Database;
namespace tikAeroWebMain
{
    public class ComplusClient : RunComplus
    {
        public ComplusClient() : base() { }

        public DataSet GetClientPassenger(string bookingId,
                                         string clientProfileId,
                                         string clientNumber)
        {
            DataSet ds = new DataSet();

            //Convert Datatable to Recordset
            ADODB.Recordset rsClient = null;
            tikAeroProcess.Client objBooking = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Client", _server);
                    objBooking = (tikAeroProcess.Client)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Client(); }

                if (objBooking != null)
                {
                    //Call Complus function
                    rsClient = objBooking.GetClientPassenger(ref bookingId, ref clientProfileId, ref clientNumber);

                    //Convert Recordset to Datatable and Release com object
                    ds = RecordsetToDataset(rsClient, "Passenger");

                }
                
            }
            catch
            { }
            finally
            {
                Marshal.FinalReleaseComObject(objBooking);
                objBooking = null;

                ClearRecordset(ref rsClient);
            }
            

            return ds;
        }
        public DataSet GetTransaction(string strOrigin,
                                         string strDestination,
                                         string strAirline,
                                         string strFlight,
                                         string strSegmentType,
                                         string strClientProfileId,
                                         string strPassengerProfileId,
                                         string strVendor,
                                         string strCreditDebit,
                                         DateTime dtFlightFrom,
                                         DateTime dtFlightTo,
                                         DateTime dtTransactionFrom,
                                         DateTime dtTransactionTo,
                                         DateTime dtExpiryFrom,
                                         DateTime dtExpiryTo,
                                         DateTime dtVoidFrom,
                                         DateTime dtVoidTo,
                                         short iBatch,
                                         bool bAllVoid,
                                         bool bAllExpired,
                                         bool bAuto,
                                         bool bManual,
                                         bool bAllPoint)
        {
            DataSet ds = new DataSet();

            //Convert Datatable to Recordset
            ADODB.Recordset rs = null;
            tikAeroProcess.Client objBooking = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Client", _server);
                    objBooking = (tikAeroProcess.Client)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Client(); }

                if (objBooking != null)
                {
                    //Call Complus function
                    rs = objBooking.GetTransaction(ref strOrigin, ref strDestination, ref strAirline, ref strFlight, ref strSegmentType, ref strClientProfileId, ref strPassengerProfileId, ref strVendor, ref strCreditDebit, ref dtFlightFrom, ref dtFlightTo, ref dtTransactionFrom, ref dtTransactionTo, ref dtExpiryFrom, ref dtExpiryTo, ref dtVoidFrom, ref dtVoidTo, ref iBatch, ref bAllVoid, ref bAllExpired, ref bAuto, ref bManual, ref bAllPoint);

                    //Convert Recordset to Datatable and Release com object
                    ds = RecordsetToDataset(rs, "Transaction");

                }

            }
            catch
            { }
            finally
            {
                Marshal.FinalReleaseComObject(objBooking);
                objBooking = null;

                ClearRecordset(ref rs);
            }


            return ds;
        }
        public bool CheckUniqueMailAddress(string strMail, string strClientProfileId)
        {
            //Convert Datatable to Recordset
            bool iResult = false;
            tikAeroProcess.Client objClient = null;
            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Client", _server);
                    objClient = (tikAeroProcess.Client)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objClient = new tikAeroProcess.Client(); }

                if (objClient != null)
                {
                    //Call Complus function
                    iResult = objClient.CheckUniqueMailAddress(ref strMail, ref strClientProfileId);
                }
            }
            catch
            { }
            finally
            {
                Marshal.FinalReleaseComObject(objClient);
                objClient = null;
            }
            return iResult;
        }
        public DataSet GetCorporateSessionProfile(string clientId, string lastName)
        {
            tikAeroProcess.Client objClient = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Client", _server);
                    objClient = (tikAeroProcess.Client)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objClient = new tikAeroProcess.Client(); }
                rs = objClient.GetCorporateClients(ref clientId, ref lastName);
                ds = RecordsetToDataset(rs, "Corporate");
            }
            catch
            { }
            finally
            {
                if (objClient != null)
                {
                    Marshal.FinalReleaseComObject(objClient);
                    objClient = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public bool ClientGetEmpty(DataTable client,
                                   DataTable passenger,
                                   DataTable bookingRemark)
        {
            tikAeroProcess.Client objClient = null;
            ADODB.Recordset rsClient = DatasetToRecordset(client);
            ADODB.Recordset rsPassenger = DatasetToRecordset(passenger);
            ADODB.Recordset rsBookingRemark = DatasetToRecordset(bookingRemark);

            bool result = false;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Client", _server);
                    objClient = (tikAeroProcess.Client)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objClient = new tikAeroProcess.Client(); }
                result = objClient.GetEmpty(ref rsClient, ref rsPassenger, ref rsBookingRemark);
                if (result == true)
                {
                    client = RecordsetToDataset(rsClient, "client").Tables[0];
                    passenger = RecordsetToDataset(rsPassenger, "passenger").Tables[0];
                    bookingRemark = RecordsetToDataset(rsBookingRemark, "remark").Tables[0];
                }
            }
            catch
            { }
            finally
            {
                if (objClient != null)
                {
                    Marshal.FinalReleaseComObject(objClient);
                    objClient = null;
                }
                ClearRecordset(ref rsClient);
                ClearRecordset(ref rsPassenger);
                ClearRecordset(ref rsBookingRemark);
            }

            return result;
        }
        public bool ClientRead(string clientProfileId,
                               ref DataTable client,
                               DataTable employee,
                               DataTable passenger,
                               DataTable bookingRemark,
                               DataTable clientSegment)
        {
            tikAeroProcess.Client objClient = null;
            ADODB.Recordset rsClient = DatasetToRecordset(client);
            ADODB.Recordset rsEmployee = DatasetToRecordset(employee);
            ADODB.Recordset rsPassenger = DatasetToRecordset(passenger);
            ADODB.Recordset rsRemark = DatasetToRecordset(bookingRemark);
            ADODB.Recordset rsClientSegment = DatasetToRecordset(clientSegment);

            bool result = false;
            DataSet ds ;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Client", _server);
                    objClient = (tikAeroProcess.Client)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objClient = new tikAeroProcess.Client(); }
                result = objClient.Read(ref clientProfileId, ref rsClient, ref rsEmployee, ref rsPassenger, ref rsRemark, ref rsClientSegment);
                if (result == true)
                {

                    ds = RecordsetToDataset(rsClient, "Client");
                    if (ds.Tables.Count != 0)
                    { client = ds.Tables[0]; }

                    ds = RecordsetToDataset(rsEmployee, "Employee");
                    if (ds.Tables.Count != 0)
                    { employee = ds.Tables[0]; }

                    ds = RecordsetToDataset(rsPassenger, "Passenger");
                    if (ds.Tables.Count != 0)
                    { passenger = ds.Tables[0]; }

                    ds = RecordsetToDataset(rsRemark, "Remark");
                    if (ds.Tables.Count != 0)
                    { bookingRemark = ds.Tables[0]; }


                    ds = RecordsetToDataset(rsClientSegment, "ClientSegment");
                    if (ds.Tables.Count != 0)
                    { clientSegment = ds.Tables[0]; }

                    ds.Dispose();
                    ds = null;
                }
            }
            catch
            { }
            finally
            {
                if (objClient != null)
                {
                    Marshal.FinalReleaseComObject(objClient);
                    objClient = null;
                }
                ClearRecordset(ref rsClient);
                ClearRecordset(ref rsEmployee);
                ClearRecordset(ref rsPassenger);
                ClearRecordset(ref rsRemark);
                ClearRecordset(ref rsClientSegment);
            }

            return result;
        }

 

        public bool EditClientProfile(DataTable client,
                                      DataTable passenger,
                                      DataTable bookingRemark)
        {

            tikAeroProcess.Client objClient = null;
            ADODB.Recordset rsClient = new ADODB.Recordset();
            ADODB.Recordset rsPassenger = new ADODB.Recordset();
            ADODB.Recordset rsRemark = new ADODB.Recordset();
            ADODB.Recordset rsClientSegment = new ADODB.Recordset();

            string clientProfileId = string.Empty;
            
            bool result = false;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Client", _server);
                    objClient = (tikAeroProcess.Client)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objClient = new tikAeroProcess.Client(); }

                if (client != null && client.Rows.Count > 0)
                {
                    clientProfileId = client.Rows[0]["client_profile_id"].ToString();
                }
                else if (passenger != null && passenger.Rows.Count > 0)
                {
                    clientProfileId = passenger.Rows[0]["client_profile_id"].ToString();
                }

                //Check if client profile exist.
                if (string.IsNullOrEmpty(clientProfileId) == false)
                {
                    result = objClient.Read(clientProfileId,
                                        ref rsClient,
                                        null,
                                        ref rsPassenger,
                                        ref rsRemark,
                                        ref rsClientSegment);

                    //Update client profile recordset information.
                    if (client != null && client.Rows.Count > 0)
                    {
                        rsClient.MoveFirst();
                        while (!rsClient.EOF)
                        {
                            foreach (DataRow dr in client.Rows)
                            {
                                if (new Guid(rsClient.Fields["client_profile_id"].Value.ToString()) == dr.DBToGuid("client_profile_id"))
                                {
                                    if (string.IsNullOrEmpty(dr.DBToString("member_number")) == false)
                                    {
                                        rsClient.Fields["member_number"].Value = dr["member_number"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("firstname")) == false)
                                    {
                                        rsClient.Fields["firstname"].Value = dr["firstname"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("lastname")) == false)
                                    {
                                        rsClient.Fields["lastname"].Value = dr["lastname"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("middlename")) == false)
                                    {
                                        rsClient.Fields["middlename"].Value = dr["middlename"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("contact_name")) == false)
                                    {
                                        rsClient.Fields["contact_name"].Value = dr["contact_name"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("status_code")) == false)
                                    {
                                        rsClient.Fields["status_code"].Value = dr["status_code"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("address_line1")) == false)
                                    {
                                        rsClient.Fields["address_line1"].Value = dr["address_line1"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("address_line2")) == false)
                                    {
                                        rsClient.Fields["address_line2"].Value = dr["address_line2"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("street")) == false)
                                    {
                                        rsClient.Fields["street"].Value = dr["street"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("state")) == false)
                                    {
                                        rsClient.Fields["state"].Value = dr["state"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("district")) == false)
                                    {
                                        rsClient.Fields["district"].Value = dr["district"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("province")) == false)
                                    {
                                        rsClient.Fields["province"].Value = dr["province"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("city")) == false)
                                    {
                                        rsClient.Fields["city"].Value = dr["city"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("zip_code")) == false)
                                    {
                                        rsClient.Fields["zip_code"].Value = dr["zip_code"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("po_box")) == false)
                                    {
                                        rsClient.Fields["po_box"].Value = dr["po_box"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("country_rcd")) == false)
                                    {
                                        rsClient.Fields["country_rcd"].Value = dr["country_rcd"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("title_rcd")) == false)
                                    {
                                        rsClient.Fields["title_rcd"].Value = dr["title_rcd"];
                                    }

                                    if (dr.DBToGuid("update_by") != Guid.Empty)
                                    {
                                        rsClient.Fields["update_by"].Value = "{" + dr["update_by"].ToString().ToUpper() + "}";
                                    }
                                    rsClient.Fields["update_date_time"].Value = DateTime.Now;
                                    if (string.IsNullOrEmpty(dr.DBToString("phone_home")) == false)
                                    {
                                        rsClient.Fields["phone_home"].Value = dr["phone_home"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("phone_business")) == false)
                                    {
                                        rsClient.Fields["phone_business"].Value = dr["phone_business"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("phone_mobile")) == false)
                                    {
                                        rsClient.Fields["phone_mobile"].Value = dr["phone_mobile"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("phone_fax")) == false)
                                    {
                                        rsClient.Fields["phone_fax"].Value = dr["phone_fax"];
                                    }

                                    if (string.IsNullOrEmpty(dr.DBToString("contact_email")) == false)
                                    {
                                        rsClient.Fields["contact_email"].Value = dr["contact_email"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("emergency_contact")) == false)
                                    {
                                        rsClient.Fields["emergency_contact"].Value = dr["emergency_contact"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("website_address")) == false)
                                    {
                                        rsClient.Fields["website_address"].Value = dr["website_address"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("language_rcd")) == false)
                                    {
                                        rsClient.Fields["language_rcd"].Value = dr["language_rcd"];
                                    }
                                    if (dr.DBToGuid("client_profile_id") != Guid.Empty)
                                    {
                                        rsClient.Fields["client_profile_id"].Value = "{" + dr["client_profile_id"].ToString().ToUpper() + "}";
                                    }
                                    if (dr.DBToDateTime("member_since_date") != DateTime.MinValue)
                                    {
                                        rsClient.Fields["member_since_date"].Value = dr.DBToDateTime("member_since_date");
                                    }
                                    if (dr.DBToDateTime("member_expiry_date") != DateTime.MinValue)
                                    {
                                        rsClient.Fields["member_expiry_date"].Value = dr.DBToDateTime("member_expiry_date");
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("member_level_rcd")) == false)
                                    {
                                        rsClient.Fields["member_level_rcd"].Value = dr["member_level_rcd"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("search_home")) == false)
                                    {
                                        rsClient.Fields["search_home"].Value = dr["search_home"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("search_mobile")) == false)
                                    {
                                        rsClient.Fields["search_mobile"].Value = dr["search_mobile"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("client_password")) == false)
                                    {
                                        rsClient.Fields["client_password"].Value = dr["client_password"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("search_business")) == false)
                                    {
                                        rsClient.Fields["search_business"].Value = dr["search_business"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("external_ar_account")) == false)
                                    {
                                        rsClient.Fields["external_ar_account"].Value = dr["external_ar_account"];
                                    }
                                    if (dr.DBToDateTime("profile_on_hold_date_time") != DateTime.MinValue)
                                    {
                                        rsClient.Fields["profile_on_hold_date_time"].Value = dr.DBToDateTime("profile_on_hold_date_time");
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("profile_on_hold_comment")) == false)
                                    {
                                        rsClient.Fields["profile_on_hold_comment"].Value = dr["profile_on_hold_comment"];
                                    }
                                    if (dr.DBToGuid("profile_on_hold_by") != Guid.Empty)
                                    {
                                        rsClient.Fields["profile_on_hold_by"].Value = dr["profile_on_hold_by"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("client_type_rcd")) == false)
                                    {
                                        rsClient.Fields["client_type_rcd"].Value = dr["client_type_rcd"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("employee_type_rcd")) == false)
                                    {
                                        rsClient.Fields["employee_type_rcd"].Value = dr["employee_type_rcd"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("crew_type_rcd")) == false)
                                    {
                                        rsClient.Fields["crew_type_rcd"].Value = dr["crew_type_rcd"];
                                    }
                                    if (dr.DBToDateTime("employee_since_date") != DateTime.MinValue)
                                    {
                                        rsClient.Fields["employee_since_date"].Value = dr.DBToDateTime("employee_since_date");
                                    }
                                    if (dr.DBToDateTime("employee_leave_date") != DateTime.MinValue)
                                    {
                                        rsClient.Fields["employee_leave_date"].Value = dr.DBToDateTime("employee_leave_date");
                                    }
                                    if (dr.DBToDateTime("employee_retired_date") != DateTime.MinValue)
                                    {
                                        rsClient.Fields["employee_retired_date"].Value = dr.DBToDateTime("employee_retired_date");
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("corporate_agency_code")) == false)
                                    {
                                        rsClient.Fields["corporate_agency_code"].Value = dr["corporate_agency_code"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("origin_rcd")) == false)
                                    {
                                        rsClient.Fields["origin_rcd"].Value = dr["origin_rcd"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("destination_rcd")) == false)
                                    {
                                        rsClient.Fields["destination_rcd"].Value = dr["destination_rcd"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("mobile_email")) == false)
                                    {
                                        rsClient.Fields["mobile_email"].Value = dr["mobile_email"];
                                    }

                                    rsClient.Fields["company_flag"].Value = dr.DBToByte("company_flag");
                                    rsClient.Fields["number_of_children"].Value = dr.DBToInt32("number_of_children");

                                    rsClient.Fields["subscribe_newsletter_flag"].Value = dr.DBToByte("subscribe_newsletter_flag");

                                    if (string.IsNullOrEmpty(dr.DBToString("airport_rcd")) == false)
                                    {
                                        rsClient.Fields["airport_rcd"].Value = dr["airport_rcd"];
                                    }

                                }
                            }
                            rsClient.MoveNext();
                        }
                        rsClient.MoveFirst();
                    }


                    //Edit Passenger profile.
                    if (passenger != null && passenger.Rows.Count > 0)
                    {
                        rsPassenger.MoveFirst();
                        while (!rsPassenger.EOF)
                        {
                            foreach (DataRow dr in passenger.Rows)
                            {
                                if (new Guid(rsPassenger.Fields["passenger_profile_id"].Value.ToString()) == dr.DBToGuid("passenger_profile_id"))
                                {
                                    
                                    if (string.IsNullOrEmpty(dr.DBToString("title_rcd")) == false)
                                    {
                                        rsPassenger.Fields["title_rcd"].Value = dr["title_rcd"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("Lastname")) == false)
                                    {
                                        rsPassenger.Fields["Lastname"].Value = dr["Lastname"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("Firstname")) == false)
                                    {
                                        rsPassenger.Fields["Firstname"].Value = dr["Firstname"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("Middlename")) == false)
                                    {
                                        rsPassenger.Fields["Middlename"].Value = dr["Middlename"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("nationality_rcd")) == false)
                                    {
                                        rsPassenger.Fields["nationality_rcd"].Value = dr["nationality_rcd"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("gender_type_rcd")) == false)
                                    {
                                        rsPassenger.Fields["gender_type_rcd"].Value = dr["gender_type_rcd"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("document_type_rcd")) == false)
                                    {
                                        rsPassenger.Fields["document_type_rcd"].Value = dr["document_type_rcd"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("passenger_type_rcd")) == false)
                                    {
                                        rsPassenger.Fields["passenger_type_rcd"].Value = dr["passenger_type_rcd"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("passport_number")) == false)
                                    {
                                        rsPassenger.Fields["passport_number"].Value = dr["passport_number"];
                                    }
                                    if (dr.DBToDateTime("passport_issue_date") != DateTime.MinValue)
                                    {
                                        rsPassenger.Fields["passport_issue_date"].Value = dr.DBToDateTime("passport_issue_date");
                                    }
                                    if (dr.DBToDateTime("passport_expiry_date") != DateTime.MinValue)
                                    {
                                        rsPassenger.Fields["passport_expiry_date"].Value = dr.DBToDateTime("passport_expiry_date");
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("passport_issue_place")) == false)
                                    {
                                        rsPassenger.Fields["passport_issue_place"].Value = dr["passport_issue_place"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("passport_birth_place")) == false)
                                    {
                                        rsPassenger.Fields["passport_birth_place"].Value = dr["passport_birth_place"];
                                    }
                                    if (dr.DBToDateTime("date_of_birth") != DateTime.MinValue)
                                    {
                                        rsPassenger.Fields["date_of_birth"].Value = dr.DBToDateTime("date_of_birth");
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("passport_issue_country_rcd")) == false)
                                    {
                                        rsPassenger.Fields["passport_issue_country_rcd"].Value = dr["passport_issue_country_rcd"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("passenger_role_rcd")) == false)
                                    {
                                        rsPassenger.Fields["passenger_role_rcd"].Value = dr["passenger_role_rcd"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("member_level_rcd")) == false)
                                    {
                                        rsPassenger.Fields["member_level_rcd"].Value = dr["member_level_rcd"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("member_number")) == false)
                                    {
                                        rsPassenger.Fields["member_number"].Value = dr["member_number"];
                                    }
                                    if (string.IsNullOrEmpty(dr.DBToString("redress_number")) == false)
                                    {
                                        rsPassenger.Fields["redress_number"].Value = dr["redress_number"];
                                    }
                                    if (dr.DBToGuid("update_by") != Guid.Empty)
                                    {
                                        rsPassenger.Fields["update_by"].Value = "{" + dr["update_by"].ToString().ToUpper() + "}";
                                    }
                                    rsPassenger.Fields["update_date_time"].Value = DateTime.Now;
                                    rsPassenger.Fields["wheelchair_flag"].Value = dr.DBToByte("wheelchair_flag");
                                    rsPassenger.Fields["vip_flag"].Value = dr.DBToByte("vip_flag");
                                    rsPassenger.Fields["window_seat_flag"].Value = dr.DBToByte("window_seat_flag");
                                    rsPassenger.Fields["passenger_weight"].Value = dr.DBToDecimal("passenger_weight");

                                    if (string.IsNullOrEmpty(dr.DBToString("comment")) == false)
                                    {
                                        rsPassenger.Fields["comment"].Value = dr["comment"];
                                    }

                                    if (string.IsNullOrEmpty(dr.DBToString("medical_conditions")) == false)
                                    {
                                        rsPassenger.Fields["medical_conditions"].Value = dr["medical_conditions"];
                                    }

                                    //add KnownTravelerNumber
                                    if (string.IsNullOrEmpty(dr.DBToString("known_traveler_number")) == false)
                                    {
                                        rsPassenger.Fields["known_traveler_number"].Value = dr["known_traveler_number"];
                                    }

                                }
                            }

                            rsPassenger.MoveNext();
                        }
                        rsPassenger.MoveFirst();
                    }
                    else
                    {
                        //Clear passenger profile record when no update is required.
                        ClearRecordset(ref rsPassenger);
                    }
                    rsRemark = DatasetToRecordset(bookingRemark);

                    //Save client profile information.
                    result = objClient.Save(ref rsClient, ref rsPassenger, ref rsRemark);
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (objClient != null)
                {
                    Marshal.FinalReleaseComObject(objClient);
                    objClient = null;
                }
                ClearRecordset(ref rsClient);
                ClearRecordset(ref rsPassenger);
                ClearRecordset(ref rsRemark);
            }

            return result;
        }


        public bool ClientSave(DataTable client,
                               DataTable passenger,
                               DataTable bookingRemark)
        {
            tikAeroProcess.Client objClient = null;
            ADODB.Recordset rsClient = null;
            ADODB.Recordset rsPassenger = null;
            ADODB.Recordset rsRemark = null;

            bool result = false;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Client", _server);
                    objClient = (tikAeroProcess.Client)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objClient = new tikAeroProcess.Client(); }

                //Get Empty recordset.
                objClient.GetEmpty(ref rsClient, ref rsPassenger, ref rsRemark);
                
                if (client != null)
                {
                    DatasetToRecordset(client, ref rsClient);
                }
                if (passenger != null)
                {
                    DatasetToRecordset(passenger, ref rsPassenger);
                }
                if (bookingRemark != null)
                {
                    DatasetToRecordset(bookingRemark, ref rsRemark);
                }

                //Save Recordset.
                result = objClient.Save(ref rsClient, ref rsPassenger, ref rsRemark);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (objClient != null)
                {
                    Marshal.FinalReleaseComObject(objClient);
                    objClient = null;
                }

                ClearRecordset(ref rsClient);
                ClearRecordset(ref rsPassenger);
                ClearRecordset(ref rsRemark);
            }

            return result;
        }
        public DataSet GetCorporateAgencyClients(string agencyCode)
        {
            tikAeroProcess.Client objClient = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Client", _server);
                    objClient = (tikAeroProcess.Client)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objClient = new tikAeroProcess.Client(); }
                rs = objClient.GetCorporateAgencyClients(ref agencyCode);
                ds = RecordsetToDataset(rs, "Clients");
            }
            catch
            { }
            finally
            {
                if (objClient != null)
                {
                    Marshal.FinalReleaseComObject(objClient);
                    objClient = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet GetActiveBookings(string strClientProfileId)
        {
            tikAeroProcess.Client objClient = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Client", _server);
                    objClient = (tikAeroProcess.Client)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objClient = new tikAeroProcess.Client(); }
                rs = objClient.GetActiveBookings("@@@" + strClientProfileId);
                ds = RecordsetToDataset(rs, "ActiveBooking");
            }
            catch
            { }
            finally
            {
                if (objClient != null)
                {
                    Marshal.FinalReleaseComObject(objClient);
                    objClient = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet GetFlownBookings(string strClientProfileId)
        {
            tikAeroProcess.Client objClient = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Client", _server);
                    objClient = (tikAeroProcess.Client)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objClient = new tikAeroProcess.Client(); }
                rs = objClient.GetFlownBookings("@@@" + strClientProfileId);
                ds = RecordsetToDataset(rs, "FlownBooking");
            }
            catch
            { }
            finally
            {
                if (objClient != null)
                {
                    Marshal.FinalReleaseComObject(objClient);
                    objClient = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }
    }
}