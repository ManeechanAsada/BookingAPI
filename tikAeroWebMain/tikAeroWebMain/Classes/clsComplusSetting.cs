using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.InteropServices;
using Avantik.Web.Service.COMHelper;

namespace tikAeroWebMain
{
    public class ComplusSetting : RunComplus
    {
        public ComplusSetting() : base() { }

        public DataSet GetAgencyCode(string agencyCode)
        {
            tikAeroProcess.Settings objSettings = null;
            ADODB.Recordset rs = null;
            DateTime dtStart = DateTime.Now;
            Helper objHelper = new Helper();
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Settings", _server);
                    objSettings = (tikAeroProcess.Settings)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objSettings = new tikAeroProcess.Settings(); }

                rs = objSettings.AgencyRead(ref agencyCode);
                ds = RecordsetToDataset(rs, "Agent");
            }
            catch (Exception e)
            {
                objHelper.SaveLog("GetAgencyCode", dtStart, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);
            }
            finally
            {
                if (objSettings != null)
                { Marshal.FinalReleaseComObject(objSettings); }
                objSettings = null;

                ClearRecordset(ref rs);
            }

            return ds;
        }

        public DataSet ReadFormOfPayment(string type)
        {
            tikAeroProcess.Settings objSettings = null;
            ADODB.Recordset rs = null;
            DateTime dtStart = DateTime.Now;
            Helper objHelper = new Helper();
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Settings", _server);
                    objSettings = (tikAeroProcess.Settings)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objSettings = new tikAeroProcess.Settings(); }

                rs = objSettings.FormOfPaymentRead(type, "", "", false);
                ds = RecordsetToDataset(rs, "ReadFormOfPayment");
            }
            catch (Exception e)
            {
                objHelper.SaveLog("ReadFormOfPayment", dtStart, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);
            }
            finally
            {
                if (objSettings != null)
                { Marshal.FinalReleaseComObject(objSettings); }
                objSettings = null;

                ClearRecordset(ref rs);
            }

            return ds;
        }

        public DataSet GetAirportTimezone(string odOrigin)
        {
            tikAeroProcess.Settings objSettings = null;
            ADODB.Recordset rs = null;
            DateTime dtStart = DateTime.Now;
            Helper objHelper = new Helper();
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Settings", _server);
                    objSettings = (tikAeroProcess.Settings)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objSettings = new tikAeroProcess.Settings(); }

                rs = objSettings.AirportRead(odOrigin, "", "", "", "", false);
                ds = RecordsetToDataset(rs, "GetAirportTimezone");
            }
            catch (Exception e)
            {
                objHelper.SaveLog("GetAirportTimezone", dtStart, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);
            }
            finally
            {
                if (objSettings != null)
                { Marshal.FinalReleaseComObject(objSettings); }
                objSettings = null;

                ClearRecordset(ref rs);
            }

            return ds;
        }

        public DataSet UserList(string userLogon,
                               string userCode,
                               string lastName,
                               string firstName,
                               string agencyCode,
                               string statusCode,
                               bool airlineUser)
        {
            tikAeroProcess.Settings objSetting = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Settings", _server);
                    objSetting = (tikAeroProcess.Settings)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objSetting = new tikAeroProcess.Settings(); }

                rs = objSetting.UserList(ref userLogon,
                                        ref userCode,
                                        ref lastName,
                                        ref firstName,
                                        ref agencyCode,
                                        ref statusCode,
                                        ref airlineUser);

                ds = RecordsetToDataset(rs, "UserList");
            }
            catch
            { }
            finally
            {
                if (objSetting != null)
                {
                    Marshal.FinalReleaseComObject(objSetting);
                    objSetting = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet UserGetEmpty()
        {
            tikAeroProcess.Settings objSetting = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Settings", _server);
                    objSetting = (tikAeroProcess.Settings)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objSetting = new tikAeroProcess.Settings(); }

                rs = objSetting.UserGetEmpty();
                ds = RecordsetToDataset(rs, "User");
            }
            catch
            { }
            finally
            {
                if (objSetting != null)
                {
                    Marshal.FinalReleaseComObject(objSetting);
                    objSetting = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet AgencyUserMappingGetEmpty()
        {
            tikAeroProcess.Settings objSetting = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Settings", _server);
                    objSetting = (tikAeroProcess.Settings)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objSetting = new tikAeroProcess.Settings(); }

                rs = objSetting.AgencyUserMappingGetEmpty();
                ds = RecordsetToDataset(rs, "AgencyUserMapping");
            }
            catch
            { }
            finally
            {
                if (objSetting != null)
                {
                    Marshal.FinalReleaseComObject(objSetting);
                    objSetting = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet AgencyUserMappingRead(string userId, string agencyCode, bool write)
        {
            tikAeroProcess.Settings objSetting = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Settings", _server);
                    objSetting = (tikAeroProcess.Settings)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objSetting = new tikAeroProcess.Settings(); }

                rs = objSetting.AgencyUserMappingRead(ref userId, ref agencyCode, ref write);
                ds = RecordsetToDataset(rs, "AgencyUserMapping");
            }
            catch
            { }
            finally
            {
                if (objSetting != null)
                {
                    Marshal.FinalReleaseComObject(objSetting);
                    objSetting = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public bool AgencyUserMappingSave(DataTable agencyUserMapping)
        {
            tikAeroProcess.Settings objSetting = null;
            ADODB.Recordset rsAgencyUserMapping = DatasetToRecordset(agencyUserMapping);

            DataSet ds = null;
            bool result = false;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Settings", _server);
                    objSetting = (tikAeroProcess.Settings)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objSetting = new tikAeroProcess.Settings(); }

                result = objSetting.AgencyUserMappingSave(ref rsAgencyUserMapping);

                ds = RecordsetToDataset(rsAgencyUserMapping, "AgencyUserMapping");
                agencyUserMapping = ds.Tables[0];
            }
            catch
            { }
            finally
            {
                if (objSetting != null)
                {
                    Marshal.FinalReleaseComObject(objSetting);
                    objSetting = null;
                }
                ClearRecordset(ref rsAgencyUserMapping);
            }

            return result;
        }
        public bool UserSave(tikSystem.Web.Library.User objUser, string agencyCode)
        {
            tikAeroProcess.Settings objComSetting = null;
            ADODB.Recordset rsUser = null;
            ADODB.Recordset rsAgencyUserMappingGetEmpty = null;
            bool success = false;
            try
            {
                if (string.IsNullOrEmpty(_server) == false)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Settings", _server);
                    objComSetting = (tikAeroProcess.Settings)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objComSetting = new tikAeroProcess.Settings(); }

                if (objComSetting != null)
                {
                    if (objUser.user_account_id.ToString() == "00000000-0000-0000-0000-000000000000")
                    {
                        rsUser = objComSetting.UserGetEmpty();
                        rsAgencyUserMappingGetEmpty = objComSetting.AgencyUserMappingGetEmpty();
                        objUser.user_account_id = Guid.NewGuid();

                        // fill empty UserMapping
                        rsAgencyUserMappingGetEmpty.AddNew();
                        rsAgencyUserMappingGetEmpty.Fields["agency_code"].Value = agencyCode;
                        rsAgencyUserMappingGetEmpty.Fields["user_account_id"].Value = "{" + objUser.user_account_id.ToString() + "}";
                        rsAgencyUserMappingGetEmpty.Update();
                        objComSetting.AgencyUserMappingSave(rsAgencyUserMappingGetEmpty);
                        
                        // add new reccord
                        rsUser.AddNew();
                    }
                    else
                    {
                        rsUser = objComSetting.UserRead(objUser.user_account_id.ToString());
                    }

                    rsUser.Fields["user_account_id"].Value = objUser.user_account_id;
                    rsUser.Fields["user_logon"].Value = objUser.user_logon;
                    rsUser.Fields["lastname"].Value = objUser.lastname;
                    rsUser.Fields["firstname"].Value = objUser.firstname;
                    rsUser.Fields["email_address"].Value = objUser.email_address;
                    rsUser.Fields["language_rcd"].Value = objUser.language_rcd;
                    rsUser.Fields["make_bookings_for_others_flag"].Value = objUser.make_bookings_for_others_flag;
                    rsUser.Fields["address_default_code"].Value = objUser.address_default_code;
                    rsUser.Fields["update_booking_flag"].Value = objUser.update_booking_flag;
                    rsUser.Fields["change_segment_flag"].Value = objUser.change_segment_flag;
                    rsUser.Fields["delete_segment_flag"].Value = objUser.delete_segment_flag;
                    rsUser.Fields["issue_ticket_flag"].Value = objUser.issue_ticket_flag;
                    rsUser.Fields["counter_sales_report_flag"].Value = objUser.counter_sales_report_flag;
                    rsUser.Fields["status_code"].Value = objUser.status_code;
                    rsUser.Fields["system_admin_flag"].Value = objUser.system_admin_flag;
                    rsUser.Fields["mon_flag"].Value = objUser.mon_flag;
                    rsUser.Fields["tue_flag"].Value = objUser.tue_flag;
                    rsUser.Fields["wed_flag"].Value = objUser.wed_flag;
                    rsUser.Fields["thu_flag"].Value = objUser.thu_flag;
                    rsUser.Fields["fri_flag"].Value = objUser.fri_flag;
                    rsUser.Fields["sat_flag"].Value = objUser.sat_flag;
                    rsUser.Fields["sun_flag"].Value = objUser.sun_flag;
                    rsUser.Fields["create_by"].Value = objUser.create_by;
                    rsUser.Fields["create_date_time"].Value = DateTime.Now;
                    rsUser.Fields["update_by"].Value = objUser.update_by;
                    rsUser.Fields["update_date_time"].Value = DateTime.Now;

                    success = objComSetting.UserSave(rsUser);
                }

                return success;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Marshal.FinalReleaseComObject(objComSetting);
                objComSetting = null;
                RecordsetHelper.ClearRecordset(ref rsUser);
                RecordsetHelper.ClearRecordset(ref rsAgencyUserMappingGetEmpty);
                base.Dispose();
            }
        }
        public bool AgencyRegistrationInsert(string agencyName,
                                      string legalName,
                                      string agencyType,
                                      string IATA,
                                      string taxId,
                                      string mail,
                                      string fax,
                                      string phone,
                                      string address1,
                                      string address2,
                                      string street,
                                      string state,
                                      string district,
                                      string province,
                                      string city,
                                      string zipCode,
                                      string poBox,
                                      string website,
                                      string contactPerson,
                                      string lastName,
                                      string firstName,
                                      string title,
                                      string userLogon,
                                      string password,
                                      string country,
                                      string currency,
                                      string language,
                                      string comment)
        {
            tikAeroProcess.Settings objSettings = null;

            bool result = false;
            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Settings", _server);
                    objSettings = (tikAeroProcess.Settings)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objSettings = new tikAeroProcess.Settings(); }

                result = objSettings.AgencyRegistrationInsert(ref agencyName,
                                                            ref legalName,
                                                            ref agencyType,
                                                            ref IATA,
                                                            ref taxId,
                                                            ref mail,
                                                            ref fax,
                                                            ref phone,
                                                            ref address1,
                                                            ref address2,
                                                            ref street,
                                                            ref state,
                                                            ref district,
                                                            ref province,
                                                            ref city,
                                                            ref zipCode,
                                                            ref poBox,
                                                            ref website,
                                                            ref contactPerson,
                                                            ref lastName,
                                                            ref firstName,
                                                            ref title,
                                                            ref userLogon,
                                                            ref password,
                                                            ref country,
                                                            ref currency,
                                                            ref language,
                                                            ref comment);

            }
            catch
            { }
            finally
            {
                if (objSettings != null)
                { Marshal.FinalReleaseComObject(objSettings); }
                objSettings = null;
            }

            return result;
        }

        public DataSet VoucherTemplateList(string voucherTemplateId,
                                            string voucherTemplate,
                                            DateTime fromDate,
                                            DateTime toDate,
                                            bool write,
                                            string status,
                                            string language)
        {
            tikAeroProcess.Settings objSetting = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Settings", _server);
                    objSetting = (tikAeroProcess.Settings)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objSetting = new tikAeroProcess.Settings(); }

                rs = objSetting.VoucherTemplateList(voucherTemplateId,
                                                    voucherTemplate,
                                                    fromDate,
                                                    toDate,
                                                    write,
                                                    status,
                                                    language);
                ds = RecordsetToDataset(rs, "Voucher");
                if (ds != null)
                {
                    ds.DataSetName = "Template";
                }
            }
            catch
            { }
            finally
            {
                if (objSetting != null)
                {
                    Marshal.FinalReleaseComObject(objSetting);
                    objSetting = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }
    }
}