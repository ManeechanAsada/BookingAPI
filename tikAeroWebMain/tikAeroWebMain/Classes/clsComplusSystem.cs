using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Data;
using Avantik.Web.Service.COMHelper;

namespace tikAeroWebMain
{
    public class ComplusSystem : RunComplus
    {
        public ComplusSystem() : base() { }

        public DataSet TravelAgentLogon(string agencyCode, string agentLogon, string agentPassword)
        {
            tikAeroProcess.System objSystem = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;
            DateTime dtStart = DateTime.Now;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.System", _server);
                    objSystem = (tikAeroProcess.System)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objSystem = new tikAeroProcess.System(); }
                rs = objSystem.TravelAgentLogon(ref agencyCode, ref agentLogon, ref agentPassword);
                ds = RecordsetToDataset(rs, "TravelAgent");
            }
            catch (Exception ex)
            {
                Helper objHelper = new Helper();
                objHelper.SaveLog("TravelAgentLogon", dtStart, DateTime.Now, ex.Message + "\n" + ex.StackTrace.ToString(), string.Empty);
            }
            finally
            {
                if (objSystem != null)
                { Marshal.FinalReleaseComObject(objSystem); }
                objSystem = null;

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet BookingLogon(string recordLocator, string NameOrPhone)
        {
            tikAeroProcess.System objSystem = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.System", _server);
                    objSystem = (tikAeroProcess.System)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objSystem = new tikAeroProcess.System(); }
                rs = objSystem.BookingLogon(ref recordLocator, ref NameOrPhone);
                ds = RecordsetToDataset(rs, "Logon");
                ds.DataSetName = "Booking";
            }
            catch
            { }
            finally
            {
                if (objSystem != null)
                { Marshal.FinalReleaseComObject(objSystem); }
                objSystem = null;

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet GetAgencySessionProfile(string agencyCode, string userAccountID)
        {
            tikAeroProcess.System objSystem = null;
            ADODB.Recordset rs = null;
            DataSet ds = new DataSet();
            ds.DataSetName = "AgencySessionProfile";

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.System", _server);
                    objSystem = (tikAeroProcess.System)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objSystem = new tikAeroProcess.System(); }
                rs = objSystem.GetAgencySessionProfile(ref agencyCode, ref userAccountID);
                RecordsetToDataset(ds, rs, "Agency");
            }
            catch
            { }
            finally
            {
                if (objSystem != null)
                { Marshal.FinalReleaseComObject(objSystem); }
                objSystem = null;

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public bool ValidAgencyLogin(string agencyCode, 
                                    string password, 
                                    string originRcd, 
                                    string destinationRcd, 
                                    ref string strCurrencyCode)
        {
            tikAeroProcess.System objSystem = null;
            ADODB.Recordset rs = null;
            DateTime dtStart = DateTime.Now;
            Helper objHelper = new Helper();

            bool bResult = false;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.System", _server);
                    objSystem = (tikAeroProcess.System)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objSystem = new tikAeroProcess.System(); }

                rs = objSystem.GetAgencySessionProfile(ref agencyCode);
                if (rs.RecordCount > 0 && rs.Fields["status_code"].Value != null)
                {
                    if (rs.Fields["agency_code"].Value.Equals(agencyCode) && rs.Fields["agency_password"].Value.Equals(password) && rs.Fields["status_code"].Value.ToString().ToUpper().Equals("A"))
                    {
                        if (RecordsetHelper.ToByte(rs, "use_origin_currency_flag") == 1 && string.IsNullOrEmpty(strCurrencyCode))
                        {
                            //Find currency from route.
                            if (string.IsNullOrEmpty(originRcd) == false && string.IsNullOrEmpty(destinationRcd) == false)
                            {
                                ComplusInventory objInv = new ComplusInventory();
                                tikSystem.Web.Library.Routes routes = objInv.GetDestination("EN", 
                                                                                            false, 
                                                                                            false, 
                                                                                            false, 
                                                                                            false, 
                                                                                            true);
                                
                                for (int i = 0; i < routes.Count; i++)
                                {
                                    if (routes[i].origin_rcd.ToUpper() == originRcd.ToUpper() && routes[i].destination_rcd.ToUpper() == destinationRcd.ToUpper())
                                    {
                                        strCurrencyCode = routes[i].currency_rcd;
                                        break;
                                    }
                                }

                                objInv = null;
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(strCurrencyCode))
                                {
                                    strCurrencyCode = rs.Fields["currency_rcd"].Value.ToString();
                                }
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(strCurrencyCode))
                            {
                                strCurrencyCode = rs.Fields["currency_rcd"].Value.ToString();
                            }
                        }

                        if (string.IsNullOrEmpty(strCurrencyCode))
                        {
                            strCurrencyCode = rs.Fields["currency_rcd"].Value.ToString();
                        }

                        bResult = true;

                    }
                    else
                    {
                        strCurrencyCode = string.Empty;
                        bResult = false;
                    }
                }

            }
            catch (Exception e)
            {
                objHelper.SaveLog("GetAgencyCode", dtStart, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);
            }
            finally
            {
                if (objSystem != null)
                { Marshal.FinalReleaseComObject(objSystem); }
                objSystem = null;

                ClearRecordset(ref rs);
            }

            return bResult;
        }
        public DataSet ClientLogon(string clientNumber, string clientPassword)
        {
            tikAeroProcess.System objSystem = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.System", _server);
                    objSystem = (tikAeroProcess.System)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objSystem = new tikAeroProcess.System(); }

                rs = objSystem.ClientLogon(ref clientNumber, ref clientPassword);

                ds = RecordsetToDataset(rs, "Client");
                ds.DataSetName = "Clients";
            }
            catch
            { }
            finally
            {
                if (objSystem != null)
                { Marshal.FinalReleaseComObject(objSystem); }
                objSystem = null;

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet UserRead(string UserId)
        {
            tikAeroProcess.Settings objSetting = null;
            ADODB.Recordset rs = null;

            DataSet ds = new DataSet();
            ds.DataSetName = "Users";
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

                rs = objSetting.UserRead(ref UserId);
                RecordsetToDataset(ds, rs, "User");
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
        public tikSystem.Web.Library.Clients GetClientSessionProfile(string clientProfileId)
        {
            tikAeroProcess.System objSystem = null;

            ADODB.Recordset rsClient = null;
            tikSystem.Web.Library.Clients clients = new tikSystem.Web.Library.Clients();

            try
            {
                if (string.IsNullOrEmpty(_server) == false)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.System", _server);
                    objSystem = (tikAeroProcess.System)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objSystem = new tikAeroProcess.System(); }

                if (objSystem != null)
                {
                    //Get Agency information.
                    rsClient = objSystem.GetClientSessionProfile(clientProfileId);
                    //Loop Agency information to object.
                    if (rsClient != null && rsClient.RecordCount > 0)
                    {
                        tikSystem.Web.Library.Client client;
                        //Agent ag;
                        rsClient.MoveFirst();
                        while (!rsClient.EOF)
                        {
                            client = new tikSystem.Web.Library.Client();
                            Type type = client.GetType();
                            System.Reflection.PropertyInfo[] props = type.GetProperties();
                            for (int i = 0; i < props.Length; i++)
                            {
                                object value = RecordsetHelper.ToObject(rsClient, props[i].Name, props[i]);
                                if (!string.IsNullOrEmpty(value.ToString()))
                                {
                                    System.Reflection.PropertyInfo property = type.GetProperty(props[i].Name);
                                    property.SetValue(client, value, null);
                                }
                            }

                            clients.Add(client);
                            rsClient.MoveNext();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Marshal.FinalReleaseComObject(objSystem);
                objSystem = null;
                RecordsetHelper.ClearRecordset(ref rsClient);
                base.Dispose();
            }

            return clients;
        }
    }
}