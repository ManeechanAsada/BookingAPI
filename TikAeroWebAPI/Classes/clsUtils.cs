using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using tikSystem.Web.Library;
using System.Text.RegularExpressions;
using NLog;
using System.Xml;

namespace TikAeroWebAPI.Classes
{
    public static class Utils
    {
      //  public static ILogger _logger = NLog.LogManager.GetCurrentClassLogger();
      //  private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static string ErrorXml(string ErrorCode, string strMessage)
        {
            StringBuilder stb = new StringBuilder();

            stb.Append("<error>" + Environment.NewLine);
            stb.Append("<code>" + ErrorCode + "</code>" + Environment.NewLine);
            stb.Append("<message>" + strMessage + "</message>" + Environment.NewLine);
            stb.Append("</error>" + Environment.NewLine);

            return stb.ToString();
        }

        public static string ErrorXmlInit(string ErrorCode, string strMessage, string ServerName)
        {
            StringBuilder stb = new StringBuilder();

            stb.Append("<error>" + Environment.NewLine);
            stb.Append("<code>" + ErrorCode + "</code>" + Environment.NewLine);
            stb.Append("<message>" + strMessage + "</message>" + Environment.NewLine);
            stb.Append("<server_id>" + ServerName + "</server_id>" + Environment.NewLine);
            stb.Append("</error>" + Environment.NewLine);

            return stb.ToString();
        }
        public static string ErrorXml(string ErrorCode, string strMessage, string strReferenceID)
        {

            StringBuilder stb = new StringBuilder();

            stb.Append("<error>" + Environment.NewLine);
            stb.Append("<code>" + ErrorCode + "</code>" + Environment.NewLine);
            stb.Append("<message>" + strMessage + "</message>" + Environment.NewLine);
            stb.Append("<referrenceId>" + strReferenceID + "</referrenceId>" + Environment.NewLine);
            stb.Append("</error>" + Environment.NewLine);

            return stb.ToString();
        }
        public static string ErrorXmlAddClient(string ErrorCode, string strMessage, string strClient)
        {
            StringBuilder stb = new StringBuilder();

            stb.Append("<error>" + Environment.NewLine);
            stb.Append("<code>" + ErrorCode + "</code>" + Environment.NewLine);
            stb.Append("<message>" + strMessage + "</message>" + Environment.NewLine);
            stb.Append(strClient.Replace("NewDataSet", "Clients") + Environment.NewLine);
            stb.Append("</error>" + Environment.NewLine);

            return stb.ToString();
        }

        public static void SaveLog(string strFunctionName, DateTime dtStart, DateTime dtEnd, string strMessage, string strInput)
        {
            string strSessionId = string.Empty;
            string ip = string.Empty;
            string ServerName = string.Empty;
            bool UseNlog = true;

            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UseNlog"]) == false &&
                    Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["UseNlog"]) == 0)
            {
                UseNlog = false;
            }

            try
            {
                strSessionId = HttpContext.Current.Session.SessionID;
                ip = DataHelper.GetClientIpAddress();

                ServerName = Environment.MachineName;

            }
            catch (System.Exception ex)
            {
            }

            if (strMessage.Length > 0)
            {
                // remove string input when error
                strInput = string.Empty;
                StringBuilder stb = new StringBuilder();

                if (UseNlog == false)
                {
                    string strPath = HttpContext.Current.Server.MapPath("~") + @"\Log\" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
                    StreamWriter stw = null;

                    try
                    {
                        using (stw = new StreamWriter(strPath, true))
                        {
                            stb.Append("------------------------------(" + strFunctionName + ")" + Environment.NewLine);
                            stb.Append("ServerName " + ServerName + Environment.NewLine);
                            stb.Append("IPAddress " + ip + Environment.NewLine);
                            stb.Append("SessionId " + strSessionId + Environment.NewLine);
                            stb.Append("******Start " + String.Format("{0:dd/MM/yyyy hh:mm:ss}", dtStart) + Environment.NewLine);
                            stb.Append("******End " + String.Format("{0:dd/MM/yyyy hh:mm:ss}", dtEnd) + Environment.NewLine);
                            stb.Append("******Message" + Environment.NewLine);
                            stb.Append(strMessage + Environment.NewLine);
                            stb.Append("Input Data" + Environment.NewLine);
                            stb.Append(strInput + Environment.NewLine);

                            stw.WriteLine(stb.ToString());
                            stw.Flush();
                        }
                    }
                    catch
                    {
                        if (stw != null)
                        {
                            stw.Close();
                        }
                    }
                }
                else
                {
                    var _logger1 = LogManager.GetLogger("ErrorLogger");

                    try
                    {
                        stb.Append("------------------------------(" + strFunctionName + ")" + Environment.NewLine);
                        stb.Append("ServerName: " + ServerName + Environment.NewLine);
                        stb.Append("IPAddress: " + ip + Environment.NewLine);
                        stb.Append("SessionId: " + strSessionId + Environment.NewLine);
                        stb.Append("******Start " + String.Format("{0:dd/MM/yyyy hh:mm:ss}", dtStart) + Environment.NewLine);
                        stb.Append("******End " + String.Format("{0:dd/MM/yyyy hh:mm:ss}", dtEnd) + Environment.NewLine);
                        stb.Append("******Message" + Environment.NewLine);
                        stb.Append(strMessage + Environment.NewLine);
                        stb.Append("Input Data" + Environment.NewLine);
                        stb.Append(strInput + Environment.NewLine);

                        _logger1.Error(stb.ToString());
                    }
                    catch
                    {
                    }
                }
            }
        }


        public static void SaveProcessLog(string strFunctionName, DateTime dtStart, DateTime dtEnd, string strInput)
        {
            string strSessionId = string.Empty;
            string ip = string.Empty;
            string ServerName = string.Empty;
            bool UseNlog = true;

            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UseNlog"]) == false &&
                    Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["UseNlog"]) == 0)
            {
                UseNlog = false;
            }

            try
            {
                strSessionId = HttpContext.Current.Session.SessionID;
                ip = DataHelper.GetClientIpAddress();

                ServerName  = Environment.MachineName;

            }
            catch (System.Exception ex)
            {
            }

            if (strInput != null)
            {
                StringBuilder stb = new StringBuilder();

                if (UseNlog == false)
                {
                    string strPath = HttpContext.Current.Server.MapPath("~") + @"\Log\Process_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
                    StreamWriter stw = null;
                    try
                    {
                        using (stw = new StreamWriter(strPath, true))
                        {
                            stb.Append("------------------------------(" + strFunctionName + ")" + Environment.NewLine);
                            stb.Append("ServerName " + ServerName + Environment.NewLine);
                            stb.Append("IPAddress " + ip + Environment.NewLine);
                            stb.Append("SessionId " + strSessionId + Environment.NewLine);
                            stb.Append("******Start " + String.Format("{0:dd/MM/yyyy hh:mm:ss}", dtStart) + Environment.NewLine);
                            stb.Append("******End " + String.Format("{0:dd/MM/yyyy hh:mm:ss}", dtEnd) + Environment.NewLine);
                            stb.Append("Input Data" + Environment.NewLine);
                            stb.Append(strInput + Environment.NewLine);

                            stw.WriteLine(stb.ToString());
                            stw.Flush();
                        }
                    }
                    catch
                    {
                        if (stw != null)
                        {
                            stw.Close();
                        }
                    }

                }
                else
                {
                    var _logger2 = LogManager.GetLogger("InfoLogger");

                    try
                    {
                        stb.Append("------------------------------(" + strFunctionName + ")" + Environment.NewLine);
                        stb.Append("ServerName: " + ServerName + Environment.NewLine);
                        stb.Append("IPAddress: " + ip + Environment.NewLine);
                        stb.Append("SessionId: " + strSessionId + Environment.NewLine);
                        stb.Append("******Start " + String.Format("{0:dd/MM/yyyy hh:mm:ss}", dtStart) + Environment.NewLine);
                        stb.Append("******End " + String.Format("{0:dd/MM/yyyy hh:mm:ss}", dtEnd) + Environment.NewLine);
                        stb.Append("Input Data" + Environment.NewLine);
                        stb.Append(strInput + Environment.NewLine);

                        _logger2.Info(stb.ToString());
                    }
                    catch
                    {
                    }
                }
            }
        }

      


    }
    public static class BookingUtil
    {
        public static Routes CacheOrigin()
        {
            try
            {
                Routes route = (Routes)HttpRuntime.Cache["origin"];
                if (route != null && route.Count > 0)
                {
                    return route;
                }
                else
                {
                    ServiceClient srvClient = new ServiceClient();
                    route = srvClient.GetOrigins("EN", true, false, false, false, false);

                    if (route != null && route.Count > 0)
                    {
                        HttpRuntime.Cache.Insert("origin", route, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20), System.Web.Caching.CacheItemPriority.Normal, null);
                    }

                    return route;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Routes CacheDestination()
        {
            try
            {
                Routes routes = (Routes)HttpRuntime.Cache["destination"];
                if (routes != null && routes.Count > 0)
                {
                    return routes;
                }
                else
                {
                    ServiceClient srvClient = new ServiceClient();
                    routes = srvClient.GetDestination("EN", true, false, false, false, false);

                    if (routes != null && routes.Count > 0)
                    {
                        HttpRuntime.Cache.Insert("destination", routes, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20), System.Web.Caching.CacheItemPriority.Normal, null);
                    }
                    return routes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public static class ConfigurationUtil
    {
        public enum ConfigName
        {
            BookingLogon = 0,
            SeatMap = 1,
            BaggageFee = 2,
            SpecialService = 3,
            ClientProfile = 4,
            GetTitles = 5,
            GetCountry = 6,
            GetCreditCardType = 7,
            GetCreditCardFee = 8,
            GetFeeDefinition = 9,
            AddCustomFee = 10,
            AddRemark = 11,
            Voucher = 12,
            GetCurrency = 13,
            GetLanguage = 14,
            Booking = 15,
            Passenger = 16,
            GetActivity = 17,
            MultipleVoucher = 18,
            MultipleFOP = 19,
            MaxVoucher = 20,
            GetPassengerManifest = 21,
            OverideContactDetail=22,
            bAllowMMIssueTicket =23,
            bFareDiscount = 24
        }
        public static bool GetFunctionalSetting(ConfigName name)
        {
            
            if (System.Configuration.ConfigurationManager.GetSection("FunctionalSetting") != null)
            {
                System.Collections.Specialized.NameValueCollection objSetting = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FunctionalSetting");
                switch (name)
                {
                    case ConfigName.BookingLogon:
                        if (objSetting["BookingLogon"] == null)
                        {
                            return true;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "BookingLogon");
                        }
                    case ConfigName.SeatMap:
                        if (objSetting["SeatMap"] == null)
                        {
                            return true;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "SeatMap");
                        }
                    case ConfigName.BaggageFee:
                        if (objSetting["BaggageFee"] == null)
                        {
                            return true;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "BaggageFee");
                        }
                    case ConfigName.SpecialService:
                        if (objSetting["SpecialService"] == null)
                        {
                            return true;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "SpecialService");
                        }
                    case ConfigName.ClientProfile:
                        if (objSetting["ClientProfile"] == null)
                        {
                            return true;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "ClientProfile");
                        }
                    case ConfigName.GetTitles:
                        if (objSetting["GetTitles"] == null)
                        {
                            return true;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "GetTitles");
                        }
                    case ConfigName.GetCountry:
                        if (objSetting["GetCountry"] == null)
                        {
                            return true;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "GetCountry");
                        }
                    case ConfigName.GetCreditCardType:
                        if (objSetting["GetCreditCardType"] == null)
                        {
                            return true;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "GetCreditCardType");
                        }
                    case ConfigName.GetCreditCardFee:
                        if (objSetting["GetCreditCardFee"] == null)
                        {
                            return true;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "GetCreditCardFee");
                        }
                    case ConfigName.GetFeeDefinition:
                        if (objSetting["GetFeeDefinition"] == null)
                        {
                            return true;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "GetFeeDefinition");
                        }
                    case ConfigName.AddCustomFee:
                        if (objSetting["AddCustomFee"] == null)
                        {
                            return true;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "AddCustomFee");
                        }
                    case ConfigName.AddRemark:
                        if (objSetting["AddRemark"] == null)
                        {
                            return true;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "AddRemark");
                        }
                    case ConfigName.Voucher:
                        if (objSetting["Voucher"] == null)
                        {
                            return true;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "Voucher");
                        }
                    case ConfigName.Booking:
                        if (objSetting["Booking"] == null)
                        {
                            return true;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "Booking");
                        }
                    case ConfigName.Passenger:
                        if (objSetting["Passenger"] == null)
                        {
                            return true;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "Passenger");
                        }
                    case ConfigName.GetActivity:
                        if (objSetting["GetActivity"] == null)
                        {
                            return true;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "GetActivity");
                        }
                    case ConfigName.MultipleVoucher :
                        if (objSetting["MultipleVoucher"] == null)
                        {
                            return false;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "MultipleVoucher");
                        }
                    case ConfigName.MultipleFOP:
                        if (objSetting["MultipleFOP"] == null)
                        {
                            return false;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "MultipleFOP");
                        }
                    case ConfigName.MaxVoucher:
                        if (objSetting["MaxVoucher"] == null)
                        {
                            return false;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "MaxVoucher");
                        }
                    case ConfigName.GetPassengerManifest:
                        if (objSetting["GetPassengerManifest"] == null)
                        {
                            return false;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "GetPassengerManifest");
                        }

                    case ConfigName.OverideContactDetail:
                        if (objSetting["OverideContactDetail"] == null)
                        {
                            return false;
                        }
                        else
                        {
                            return ConfigurationHelper.ToBoolean(objSetting, "OverideContactDetail");
                        }

                    case ConfigName.bAllowMMIssueTicket:
                    if (objSetting["bAllowMMIssueTicket"] == null)
                    {
                        return false;
                    }
                    else
                    {
                        return ConfigurationHelper.ToBoolean(objSetting, "bAllowMMIssueTicket");
                    }

                    //fare discount for t3
                    case ConfigName.bFareDiscount:
                    if (objSetting["bFareDiscount"] == null)
                    {
                        return false;
                    }
                    else
                    {
                        return ConfigurationHelper.ToBoolean(objSetting, "bFareDiscount");
                    }

                }
            }
            
            return true;
        }

        public static int GetFunctionalSettingInt(ConfigName name)
        {
            int iResult = 0;
            if (System.Configuration.ConfigurationManager.GetSection("FunctionalSetting") != null)
            {
                System.Collections.Specialized.NameValueCollection objSetting = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FunctionalSetting");

                if (name == ConfigName.MaxVoucher)
                {
                    if (objSetting["MaxVoucher"] == null)
                    {
                        iResult = 0;
                    }
                    else
                    {
                        iResult = ConfigurationHelper.ToInt32(objSetting, "MaxVoucher");
                    }
                }
            }

            return iResult;
        }
    }
}