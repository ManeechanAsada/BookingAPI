using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Security.Cryptography;
using tikSystem.Web.Library;
using System.Collections;
using System.Reflection;
using System.Xml.Linq;
using System.Configuration;
using System.Data.SqlClient;
using Avantik.Web.Service.COMHelper;


namespace tikAeroWebMain
{
    /// <summary>
    /// Summary description for tikAeroWebService
    /// </summary>
    [WebService(Namespace = "http://tikAeroWebMain.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class tikAeroWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public DataSet GetCoporateSessionProfile(string clientId, string lastname, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusClient objComplus = new ComplusClient();
            DataSet ds = null;
            clientId = "{" + clientId.ToUpper() + "}";
            string strResult = string.Empty;
            try
            {
                ds = objComplus.GetCorporateSessionProfile(clientId, lastname);
            }
            catch (Exception e)
            {
                Helper objHelper = new Helper();
                objHelper.SaveLog("GetCoporateSessionProfile", DateTime.Now, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);

                throw new Exception(e.Message);
            }
            finally
            {
                objComplus.Dispose();
            }

            return ds;
        }

        [WebMethod]
        public DataSet GetCorporateAgencyClients(string agency, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusClient objComplus = new ComplusClient();
            DataSet ds = null;
            string strResult = string.Empty;
            try
            {
                ds = objComplus.GetCorporateAgencyClients(agency);
            }
            catch (Exception e)
            {
                Helper objHelper = new Helper();
                objHelper.SaveLog("GetCorporateAgencyClients", DateTime.Now, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);

                throw new Exception(e.Message);
            }
            finally
            {
                objComplus.Dispose();
            }

            return ds;
        }


        [WebMethod]
        public DataSet GetAirlines(string language, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusInventory objComplus = new ComplusInventory();
            DataSet ds = null;

            try
            { ds = objComplus.GetAirlines(language); }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            {
                objComplus.Dispose();
            }
            return ds;
        }

        [WebMethod]
        public Routes GetOrigins(string strLanguage, bool b2cFlag, bool b2bFlag, bool b2EFlag, bool b2SFlag, bool APIFlag, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusInventory objInv = new ComplusInventory();
            try
            {
                return objInv.GetOrigins(strLanguage, b2cFlag, b2bFlag, b2EFlag, b2SFlag, APIFlag);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [WebMethod]
        public Routes GetDestination(string strLanguage, bool b2cFlag, bool b2bFlag, bool b2EFlag, bool b2SFlag, bool APIFlag, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusInventory objInv = new ComplusInventory();
            try
            {
                return objInv.GetDestination(strLanguage, b2cFlag, b2bFlag, b2EFlag, b2SFlag, APIFlag);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [WebMethod]
        public string GetAvailability(string Origin,
                                       string Destination,
                                       DateTime DepartDateFrom,
                                       DateTime DepartDateTo,
                                       DateTime ReturnDateFrom,
                                       DateTime ReturnDateTo,
                                       DateTime DateBooking,
                                       short Adult,
                                       short Child,
                                       short Infant,
                                       short Other,
                                       string OtherPassengerType,
                                       string BoardingClass,
                                       string BookingClass,
                                       string DayTimeIndicator,
                                       string AgencyCode,
                                       string CurrencyCode,
                                       string FlightId,
                                       string FareId,
                                       double MaxAmount,
                                       bool NonStopOnly,
                                       bool IncludeDeparted,
                                       bool IncludeCancelled,
                                       bool IncludeWaitlisted,
                                       bool IncludeSoldOut,
                                       bool Refundable,
                                       bool GroupFares,
                                       bool ItFaresOnly,
                                       bool bStaffFares,
                                       bool bApplyFareLogic,
                                       bool bUnknownTransit,
                                       string strTransitPoint,
                                       DateTime dtSelectedDateFrom,
                                       DateTime dtSelectedDateTo,
                                       string strReturn,
                                       bool bMapWithFares,
                                       bool bReturnRefundable,
                                       string strReturnDayTimeIndicator,
                                       string PromotionCode,
                                       string strFareType,
                                       bool bLowest,
                                       bool bLowestClass,
                                       bool bLowestGroup,
                                       bool bShowClosed,
                                       bool bSort,
                                       bool bDelete,
                                       string strLanguage,
                                       string strIpAddress,
                                       bool bLowFareFinder,
                                       string token,
                                       bool bNoVat)
        {
            try
            {
                if (!ValidateToken(token)) throw new Exception("Invalid token.");

                ComplusInventory objInv = new ComplusInventory();
                DateTime dtBookingDate = DateTime.MinValue;
                //Get availability.
                return objInv.GetAvailability(Origin,
                                                Destination,
                                                DepartDateFrom,
                                                DepartDateTo,
                                                ReturnDateFrom,
                                                ReturnDateTo,
                                                dtBookingDate,
                                                Adult,
                                                Child,
                                                Infant,
                                                Other,
                                                OtherPassengerType,
                                                BoardingClass,
                                                BookingClass,
                                                DayTimeIndicator,
                                                AgencyCode,
                                                CurrencyCode,
                                                FlightId,
                                                FareId,
                                                MaxAmount,
                                                NonStopOnly,
                                                IncludeDeparted,
                                                IncludeCancelled,
                                                IncludeWaitlisted,
                                                IncludeSoldOut,
                                                Refundable,
                                                GroupFares,
                                                ItFaresOnly,
                                                bStaffFares,
                                                bApplyFareLogic,
                                                bUnknownTransit,
                                                strTransitPoint,
                                                dtSelectedDateFrom,
                                                dtSelectedDateTo,
                                                strReturn,
                                                bMapWithFares,
                                                bReturnRefundable,
                                                strReturnDayTimeIndicator,
                                                PromotionCode,
                                                strFareType,
                                                bLowest,
                                                bLowestClass,
                                                bLowestGroup,
                                                bShowClosed,
                                                bSort,
                                                bDelete,
                                                strLanguage,
                                                strIpAddress,
                                                bLowFareFinder,
                                                bNoVat);
            }
            catch (Exception e)
            {
                Helper objHelper = new Helper();
                objHelper.SaveLog("GetAvailability", DateTime.Now, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);
                throw new Exception(e.Message);
            }

        }
        [WebMethod]
        public string GetAvailabilityAgencyLogin(string Origin,
                                               string Destination,
                                               DateTime DepartDateFrom,
                                               DateTime DepartDateTo,
                                               DateTime ReturnDateFrom,
                                               DateTime ReturnDateTo,
                                               DateTime DateBooking,
                                               short Adult,
                                               short Child,
                                               short Infant,
                                               short Other,
                                               string OtherPassengerType,
                                               string BoardingClass,
                                               string BookingClass,
                                               string DayTimeIndicator,
                                               string AgencyCode,
                                               string Password,
                                               string FlightId,
                                               string FareId,
                                               double MaxAmount,
                                               bool NonStopOnly,
                                               bool IncludeDeparted,
                                               bool IncludeCancelled,
                                               bool IncludeWaitlisted,
                                               bool IncludeSoldOut,
                                               bool Refundable,
                                               bool GroupFares,
                                               bool ItFaresOnly,
                                               bool bStaffFares,
                                               bool bApplyFareLogic,
                                               bool bUnknownTransit,
                                               string strTransitPoint,
                                               DateTime dteReturnFrom,
                                               DateTime dteReturnTo,
                                               string strReturn,
                                               bool bMapWithFares,
                                               bool bReturnRefundable,
                                               string strReturnDayTimeIndicator,
                                               string PromotionCode,
                                               string strFareType,
                                               bool bLowest,
                                               bool bLowestClass,
                                               bool bLowestGroup,
                                               bool bShowClosed,
                                               bool bSort,
                                               bool bDelet,
                                               string strLanguage,
                                               string strIpAddress,
                                               bool bLowFareFinder,
                                               string token,
                                               ref string CurrencyCode,
                                               bool bNoVat)
        {
            ComplusSystem objComplus = new ComplusSystem();

            if (objComplus.ValidAgencyLogin(AgencyCode, Password, Origin, Destination, ref CurrencyCode) == true)
            {
                return GetAvailability(Origin,
                                        Destination,
                                        DepartDateFrom,
                                        DepartDateTo,
                                        ReturnDateFrom,
                                        ReturnDateTo,
                                        DateBooking,
                                        Adult,
                                        Child,
                                        Infant,
                                        Other,
                                        OtherPassengerType,
                                        BoardingClass,
                                        BookingClass,
                                        DayTimeIndicator,
                                        AgencyCode,
                                        CurrencyCode,
                                        FlightId,
                                        FareId,
                                        MaxAmount,
                                        NonStopOnly,
                                        IncludeDeparted,
                                        IncludeCancelled,
                                        IncludeWaitlisted,
                                        IncludeSoldOut,
                                        Refundable,
                                        GroupFares,
                                        ItFaresOnly,
                                        bStaffFares,
                                        bApplyFareLogic,
                                        bUnknownTransit,
                                        strTransitPoint,
                                        dteReturnFrom,
                                        dteReturnTo,
                                        strReturn,
                                        bMapWithFares,
                                        bReturnRefundable,
                                        strReturnDayTimeIndicator,
                                        PromotionCode,
                                        strFareType,
                                        bLowest,
                                        bLowestClass,
                                        bLowestGroup,
                                        bShowClosed,
                                        bSort,
                                        bDelet,
                                        strLanguage,
                                        strIpAddress,
                                        bLowFareFinder,
                                        token,
                                        bNoVat);
            }
            else
            {
                return "205";
            }
        }

        [WebMethod]
        public string GetFlightSummaryAgencyLogin(tikSystem.Web.Library.Flights objFlights,
                                                  Passengers passengers,
                                                  string strAgencyCode,
                                                  string flightId,
                                                  string boardPoint,
                                                  string offPoint,
                                                  string airline,
                                                  string flight,
                                                  string boardingClass,
                                                  string bookingClass,
                                                  DateTime flightDate,
                                                  string fareId,
                                                  string currencyCode,
                                                  bool isRefundable,
                                                  bool isGroupBooking,
                                                  bool isNonRevenue,
                                                  string segmentId,
                                                  Int16 idReduction,
                                                  string fareType,
                                                  string language,
                                                  string token,
                                                  string password)
        {
            string strOrigin = string.Empty;
            string strDestination = string.Empty;
            ComplusSystem objComplus = new ComplusSystem();

            if (objFlights != null && objFlights.Count > 0)
            {
                strOrigin = objFlights[0].origin_rcd;
                strDestination = objFlights[0].destination_rcd;

                if (objComplus.ValidAgencyLogin(strAgencyCode, password, strOrigin, strDestination, ref currencyCode) == true)
                {
                    return GetFlightSummary(objFlights,
                                            passengers,
                                            strAgencyCode,
                                            flightId,
                                            boardPoint,
                                            offPoint,
                                            airline,
                                            flight,
                                            boardingClass,
                                            bookingClass,
                                            flightDate,
                                            fareId,
                                            currencyCode,
                                            isRefundable,
                                            isGroupBooking,
                                            isNonRevenue,
                                            segmentId,
                                            idReduction,
                                            fareType,
                                            language,
                                            token);
                }
                else
                {
                    return "9102";//Invalid Agency information 
                }
            }
            else
            {
                return "9101";//Invalid Flight
            }
        }


        [WebMethod]
        public bool ReleaseFlightInventorySession(string sessionId,
                                                string flightId,
                                                string bookingClasss,
                                                string bookingId,
                                                bool releaseTimeOut,
                                                bool ReleaseInventory,
                                                bool ReleaseBookingLock,
                                                string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusInventory obj = new ComplusInventory();
            bool result = false;

            try
            {
                result = obj.ReleaseFlightInventorySession(sessionId,
                                                           flightId,
                                                           bookingClasss,
                                                           bookingId,
                                                           releaseTimeOut,
                                                           ReleaseInventory,
                                                           ReleaseBookingLock);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                obj.Dispose();
            }
            return result;
        }

        [WebMethod]
        public string GetAgencyCode(string agencyCode, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusSetting objComplus = new ComplusSetting();

            DataSet ds = null;
            DateTime dtStart = DateTime.Now;
            Helper objHelper = new Helper();
            try
            {
                ds = objComplus.GetAgencyCode(agencyCode);
            }
            catch (Exception e)
            {
                objHelper.SaveLog("GetAgencyCode", dtStart, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);
                throw new Exception(e.Message);
            }
            finally
            { objComplus.Dispose(); }

            return ds.GetXml();
        }

        [WebMethod]
        public bool AddClientProfile(string CreateClientXml,string token)
        {

            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            DataSet dsClient = null;

            DataTable dtClient = null;
            DataTable dtPassengers = null;
            DataTable dtRemarks = null;

            bool bSuccess = false;
            DateTime dtStart = DateTime.Now;
            Helper objHelper = new Helper();

            if (string.IsNullOrEmpty(CreateClientXml) == false)
            {
                dsClient = new DataSet();
                dsClient.ReadXml(new StringReader(CreateClientXml));
                if (dsClient.Tables.Count > 0)
                {
                    if (dsClient.Tables["client"] != null)
                    {
                        dtClient = dsClient.Tables["client"];
                    }
                    if (dsClient.Tables["passenger"] != null)
                    {
                        dtPassengers = dsClient.Tables["passenger"];
                    }
                    if (dsClient.Tables["remark"] != null)
                    {
                        dtRemarks = dsClient.Tables["remark"];
                    }

                    ComplusClient objComplus = new ComplusClient();
                    try
                    {
                         bSuccess = objComplus.ClientSave(dtClient, dtPassengers, null);
                    }
                    catch (Exception e)
                    {
                        Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog(e,
                                                                                                                          "Client XML : " + CreateClientXml.Replace("<", "&lt;").Replace(">", "&gt;"));
                        throw;
                    }
                    finally
                    {
                        if (objComplus != null)
                        {
                            objComplus.Dispose();
                        }

                    }
                }
            }

            return bSuccess;
        }



        [WebMethod]
        public string ReadClientProfile(string clientId, string token)
        {

            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            DataTable dtClientRead = null;
            DateTime dtStart = DateTime.Now;
            Helper objHelper = new Helper();
            string result = string.Empty;


            if (string.IsNullOrEmpty(clientId) == false)
            {
                ComplusClient objComplus = new ComplusClient();
                try
                {
                    objComplus.ClientRead("{"+clientId+"}", ref dtClientRead, null, null, null, null);

                    // remove column
                    for (int index = dtClientRead.Columns.Count - 1; index >= 0; index--)
                    {
                        if (dtClientRead.Columns[index].ColumnName.ToString().ToLower() == "client_number")
                        {
                        }
                        else if (dtClientRead.Columns[index].ColumnName.ToString().ToLower() == "client_password")
                        {
                        }
                        else if (dtClientRead.Columns[index].ColumnName.ToString().ToLower() == "client_profile_id")
                        {
                        }
                        else
                        {
                            dtClientRead.Columns.RemoveAt(index);
                        }
                    }

                    if (dtClientRead != null)
                        result = dtClientRead.DataSet.GetXml();
                }
                catch (Exception e)
                {
                    Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog(e,
                                                                                                                      "Client XML : " + clientId.Replace("<", "&lt;").Replace(">", "&gt;"));
                    throw;
                }
                finally
                {
                    if (objComplus != null)
                    {
                        objComplus.Dispose();
                    }

                }
            }
            

            return result;
        }


        [WebMethod]
        public bool EditClientProfile(string CreateClientXml, string token)
        {

            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            DataSet dsClient = null;
            DataTable dtClient = null;
            DataTable dtPassengers = null;
            DataTable dtRemarks = null;

            bool bSuccess = false;
            DateTime dtStart = DateTime.Now;
            Helper objHelper = new Helper();

            dsClient = new DataSet();
            dsClient.ReadXml(new StringReader(CreateClientXml));

            if (dsClient.Tables.Count > 0)
            {
                if (dsClient.Tables["client"] != null)
                {
                    dtClient = dsClient.Tables["client"];
                }
                if (dsClient.Tables["passenger"] != null)
                {
                    dtPassengers = dsClient.Tables["passenger"];
                }
                if (dsClient.Tables["remark"] != null)
                {
                    dtRemarks = dsClient.Tables["remark"];
                }

                ComplusClient objComplus = new ComplusClient();
                try
                {
                    bSuccess = objComplus.EditClientProfile(dtClient, dtPassengers, null);
                }
                catch (Exception e)
                {
                    Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog(e,
                                                                                                                      "Client XML : " + CreateClientXml.Replace("<", "&lt;").Replace(">", "&gt;"));
                    throw;
                }
                finally
                {
                    if (objComplus != null)
                    {
                        objComplus.Dispose();
                    }

                }
            }



            return bSuccess;
        }

        [WebMethod]
        public string AddFlight(string agencyCode,
                                 string currency,
                                 string flightXml,
                                 string bookingID,
                                 short adults,
                                 short children,
                                 short infants,
                                 short others,
                                 string strOthers,
                                 string userId,
                                 string token,
                                 string strIpAddress,
                                 string strLanguageCode,
                                 bool bNoVat)
        {

            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            DataSet dsResult = null;
            DataTable dtFlight = null;

            DataTable header = null;
            DataTable segments = null;
            DataTable passengers = null;
            DataTable remarks = null;
            DataTable payments = null;
            DataTable mappings = null;
            DataTable services = null;
            DataTable taxes = null;
            DataTable fees = null;
            DataTable quotes = null;

            ComplusBooking objComplus = new ComplusBooking();
            ComplusInventory objInvent = new ComplusInventory();
            Helper objHelper = new Helper();
            dsResult = new DataSet();
            dsResult.ReadXml(new StringReader(flightXml));

            if (dsResult.Tables.Count > 0)
            { dtFlight = dsResult.Tables[0]; }

            //Clear dataset for reused.
            if (dsResult != null)
            {
                dsResult.Dispose();
                dsResult = null;
            }
            if (dtFlight.Rows.Count > 0)
            {
                if (!objInvent.CheckInfantOverLimit(dtFlight, infants))
                {
                    if (objComplus.GetEmpty(agencyCode,
                                        currency,
                                        ref header,
                                        ref segments,
                                        ref passengers,
                                        ref remarks,
                                        ref payments,
                                        ref mappings,
                                        ref services,
                                        ref taxes,
                                        ref fees,
                                        dtFlight,
                                        ref quotes,
                                        bookingID,
                                        adults,
                                        children,
                                        infants,
                                        others,
                                        strOthers,
                                        userId,
                                        strIpAddress,
                                        strLanguageCode,
                                        bNoVat) == true)
                    {
                        dsResult = new DataSet();
                        dsResult.DataSetName = "Booking";
                        if (header != null)
                        { dsResult.Tables.Add(header.Copy()); }
                        if (segments != null)
                        { dsResult.Tables.Add(segments.Copy()); }
                        if (passengers != null)
                        { dsResult.Tables.Add(passengers.Copy()); }
                        if (remarks != null)
                        { dsResult.Tables.Add(remarks.Copy()); }
                        if (payments != null)
                        { dsResult.Tables.Add(payments.Copy()); }
                        if (mappings != null)
                        { dsResult.Tables.Add(mappings.Copy()); }
                        if (services != null)
                        { dsResult.Tables.Add(services.Copy()); }
                        if (taxes != null)
                        { dsResult.Tables.Add(taxes.Copy()); }
                        if (fees != null)
                        { dsResult.Tables.Add(fees.Copy()); }
                        if (quotes != null)
                        { dsResult.Tables.Add(quotes.Copy()); }
                    }
                }
                else
                {
                    objHelper.CreateErrorDataset(ref dsResult, "200", "Infant over limit", string.Empty, "FlightAdd");
                }
            }

            objComplus.Dispose();

            if (dsResult != null)
            {
                return dsResult.GetXml();
            }
            else
            { return string.Empty; }
        }


        [WebMethod]
        public string AddFlightSubload(string agencyCode,
                                 string currency,
                                 string flightXml,
                                 string bookingID,
                                 short adults,
                                 short children,
                                 short infants,
                                 short others,
                                 string strOthers,
                                 string userId,
                                 string token,
                                 string strIpAddress,
                                 string strLanguageCode,
                                 bool bNoVat, string p)
        {

            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            DataSet dsResult = null;
            DataSet dsPass= null;
            DataTable dtFlight = null;
            DataTable dtPass = null;

            DataTable header = null;
            DataTable segments = null;
            DataTable passengers = null;
            DataTable remarks = null;
            DataTable payments = null;
            DataTable mappings = null;
            DataTable services = null;
            DataTable taxes = null;
            DataTable fees = null;
            DataTable quotes = null;

            ComplusBooking objComplus = new ComplusBooking();
            ComplusInventory objInvent = new ComplusInventory();
            Helper objHelper = new Helper();
            dsResult = new DataSet();
            dsResult.ReadXml(new StringReader(flightXml));

            dsPass = new DataSet();
            dsPass.ReadXml(new StringReader(p));

            if (dsResult.Tables.Count > 0)
            { dtFlight = dsResult.Tables[0]; }


            if (dsPass.Tables.Count > 0)
            { dtPass = dsPass.Tables[0]; }


            //Clear dataset for reused.
            if (dsResult != null)
            {
                dsResult.Dispose();
                dsResult = null;
            }
            if (dtFlight.Rows.Count > 0)
            {
                if (!objInvent.CheckInfantOverLimit(dtFlight, infants))
                {
                    if (objComplus.GetEmptySubload(agencyCode,
                                        currency,
                                        ref header,
                                        ref segments,
                                        ref passengers,
                                        ref remarks,
                                        ref payments,
                                        ref mappings,
                                        ref services,
                                        ref taxes,
                                        ref fees,
                                        dtFlight,
                                        ref quotes,
                                        bookingID,
                                        adults,
                                        children,
                                        infants,
                                        others,
                                        strOthers,
                                        userId,
                                        strIpAddress,
                                        strLanguageCode,
                                        bNoVat, dtPass) == true)
                    {
                        dsResult = new DataSet();
                        dsResult.DataSetName = "Booking";
                        if (header != null)
                        { dsResult.Tables.Add(header.Copy()); }
                        if (segments != null)
                        { dsResult.Tables.Add(segments.Copy()); }
                        if (passengers != null)
                        { dsResult.Tables.Add(passengers.Copy()); }
                        if (remarks != null)
                        { dsResult.Tables.Add(remarks.Copy()); }
                        if (payments != null)
                        { dsResult.Tables.Add(payments.Copy()); }
                        if (mappings != null)
                        { dsResult.Tables.Add(mappings.Copy()); }
                        if (services != null)
                        { dsResult.Tables.Add(services.Copy()); }
                        if (taxes != null)
                        { dsResult.Tables.Add(taxes.Copy()); }
                        if (fees != null)
                        { dsResult.Tables.Add(fees.Copy()); }
                        if (quotes != null)
                        { dsResult.Tables.Add(quotes.Copy()); }


                    }
                }
                else
                {
                    objHelper.CreateErrorDataset(ref dsResult, "200", "Infant over limit", string.Empty, "FlightAdd");
                }
            }

            objComplus.Dispose();

            if (dsResult != null)
            {
                return dsResult.GetXml();
            }
            else
            { return string.Empty; }
        }

        [WebMethod]
        public Int32 GetInfantCapacity(string flightId,
                                 string originRcd,
                                 string destinationRcd,
                                 string boardingClass,
                                 string token)
        {

            if (!ValidateToken(token)) throw new Exception("Invalid token.");


            Int32 infantCapacity = 0;

            ComplusBooking objComplus = new ComplusBooking();
            ComplusInventory objInvent = new ComplusInventory();
            ComplusFlight objFlights = new ComplusFlight();
            Helper objHelper = new Helper();

            if (!flightId.Equals(string.Empty))
            {
                if (objFlights.GetFlight(flightId))
                {
                    infantCapacity = objInvent.GetInfantCapacity(flightId, originRcd, destinationRcd, boardingClass);
                }
            }

            objComplus.Dispose();

            return infantCapacity;
        }

        [WebMethod]
        public string GetClient(string clientId,
                                string clientNumber,
                                string passengerId,
                                bool bShowRemark,
                                string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusBooking objComplus = new ComplusBooking();
            DataSet ds = null;

            try
            {
                ds = objComplus.GetClient(clientId, clientNumber, passengerId, bShowRemark);
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            { objComplus.Dispose(); }

            return ds.GetXml();
        }

        [WebMethod]
        public DataSet GetClientPassenger(string bookingId,
                                         string clientProfileId,
                                         string clientNumber,
                                         string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusClient objComplus = new ComplusClient();
            DataSet ds = null;

            try
            {
                ds = objComplus.GetClientPassenger(bookingId, clientProfileId, clientNumber);
                ds.DataSetName = "TikAero";
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            { objComplus.Dispose(); }

            return ds;
        }

        [WebMethod]
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
                                         int iBatch,
                                         bool bAllVoid,
                                         bool bAllExpired,
                                         bool bAuto,
                                         bool bManual,
                                         bool bAllPoint,
                                         string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusClient objComplus = new ComplusClient();
            DataSet ds = null;

            try
            {
                short sBatch = 0;
                short.TryParse(iBatch.ToString(), out sBatch);

                ds = objComplus.GetTransaction(strOrigin, strDestination, strAirline, strFlight, strSegmentType, strClientProfileId, strPassengerProfileId, strVendor, strCreditDebit, dtFlightFrom, dtFlightTo, dtTransactionFrom, dtTransactionTo, dtExpiryFrom, dtExpiryTo, dtVoidFrom, dtVoidTo, sBatch, bAllVoid, bAllExpired, bAuto, bManual, bAllPoint);
                ds.DataSetName = "TikAero";
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            { objComplus.Dispose(); }

            return ds;
        }

        [WebMethod]
        public Titles GetPassengerTitles(string language, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusReservation objComplus = new ComplusReservation();
            try
            {
                return objComplus.GetPassengerTitles(language);
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }

        [WebMethod]
        public Documents GetDocumentType(string language, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusInventory objComplus = new ComplusInventory();
            try
            {
                return objComplus.GetDocumentType(language);
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }

        [WebMethod]
        public Countries GetCountry(string language, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusInventory objComplus = new ComplusInventory();
            try
            {
                return objComplus.GetCountry(language);
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }
        [WebMethod]
        public Languages GetLanguages(string language, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusReservation objComplus = new ComplusReservation();
            try
            {
                return objComplus.GetLanguages(language);
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }
        [WebMethod]
        public DataSet GetSeatMap(string origin, string destination, string flightId, string boardingClass, string bookingClass, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusCheckin objComplus = new ComplusCheckin();
            DataSet ds = null;

            try
            {
                ds = objComplus.GetSeatMap(origin, destination, flightId, boardingClass, bookingClass);
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            { objComplus.Dispose(); }

            return ds;
        }

        [WebMethod]
        public DataSet GetSeatMapLayOut(string origin, string destination, string flightId, string boardingClass, string bookingClass, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusFlight objComplus = new ComplusFlight();
            DataSet ds = null;

            try
            {
                ds = objComplus.GetSeatMapLayout(origin, destination, flightId, boardingClass, bookingClass);
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            { objComplus.Dispose(); }

            return ds;
        }


        [WebMethod]
        public string GetFormOfPayments(string language, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusPayment objComplus = new ComplusPayment();
            DataSet ds = null;

            try
            {
                ds = objComplus.GetFormOfPayments(language);
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            { objComplus.Dispose(); }

            return ds.GetXml();
        }
        [WebMethod]
        public string GetFormOfPaymentSubTypes(string type, string language, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusPayment objComplus = new ComplusPayment();
            DataSet ds = null;

            try
            {
                ds = objComplus.GetFormOfPaymentSubTypes(type, language);
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            { objComplus.Dispose(); }

            return ds.GetXml();
        }

        [WebMethod]
        public DataSet ReadFormOfPayment(string type, string language, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusSetting objComplus = new ComplusSetting();
            DataSet ds = null;

            try
            {
                ds = objComplus.ReadFormOfPayment(type);
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            { objComplus.Dispose(); }

            return ds;
        }

        [WebMethod]
        public string GetAirportTimezone(string odOrigin, string language, string token)
        {
            string result = string.Empty;
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusSetting objComplus = new ComplusSetting();
            DataSet ds = null;

            try
            {
                ds = objComplus.GetAirportTimezone(odOrigin);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = ds.Tables[0].Rows[0]["timezone_name"].ToString();
                }
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            { objComplus.Dispose(); }

            return result;
        }

        [WebMethod]
        public string SaveBooking(string strBooking,
                                    bool createTickets,
                                    bool readBooking,
                                    bool readOnly,
                                    string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            Helper objHelper = new Helper();
            DataSet ds = new DataSet();

            DataSet dsResult = null;
            //Read xml to dataset
            ds.ReadXml(new StringReader(objHelper.DecompressString(strBooking)));

            //Call com plus
            ComplusBooking objComplus = new ComplusBooking();
            try
            {

                dsResult = objComplus.SaveBooking(ds.Tables["BookingHeader"],
                        ds.Tables["FlightSegment"],
                        ds.Tables["Passenger"],
                        ds.Tables["Remark"],
                        ds.Tables["Payment"],
                        ds.Tables["Mapping"],
                        ds.Tables["Service"],
                        ds.Tables["Tax"],
                        ds.Tables["Fee"],
                        createTickets,
                        readBooking,
                        readOnly,
                        true,
                        true,
                        true);

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    if (dsResult.Tables[0].Rows[0]["ResponseCode"].ToString() == "APPROVED")
                    {
                        if (createTickets == true)
                        {
                            objComplus.TicketCreate(ds.Tables["BookingHeader"].Rows[0]["agency_code"].ToString(),
                                                    ds.Tables["BookingHeader"].Rows[0]["update_by"].ToString(),
                                                    ds.Tables["BookingHeader"].Rows[0]["booking_id"].ToString(), null, null, false);
                        }

                        dsResult = objComplus.GetBooking(ds.Tables["BookingHeader"].Rows[0]["booking_id"].ToString(),
                                                string.Empty,
                                                0,
                                                false,
                                                false,
                                                string.Empty,
                                                true,
                                                true,
                                                true,
                                                true,
                                                true,
                                                true,
                                                true,
                                                true,
                                                true,
                                                true,
                                                string.Empty,
                                                string.Empty);

                    }
                }
            }
            catch (Exception e)
            {
                if (dsResult == null)
                {
                    Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog(e,
                                                                                                                          "Input XML : " + ds.GetXml().Replace("<", "&lt;").Replace(">", "&gt;") + "<br/>");
                }
                else
                {
                    Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog(e,
                                                                                                                         "Input XML : " + ds.GetXml().Replace("<", "&lt;").Replace(">", "&gt;") + "<br/>" +
                                                                                                                         "Result XML : " + dsResult.GetXml().Replace("<", "&lt;").Replace(">", "&gt;"));
                }
                throw;
            }
            finally
            { objComplus.Dispose(); }

            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                return objHelper.CompressString(dsResult.GetXml());
            }
            else
            {
                return string.Empty;
            }
        }

        [WebMethod]
        public string BookingSaveSubLoad(string strBooking,
                                    bool createTickets,
                                    bool readBooking,
                                    bool readOnly,
                                    string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            Helper objHelper = new Helper();
            DataSet ds = new DataSet();

            DataSet dsResult = null;
            //Read xml to dataset
            ds.ReadXml(new StringReader(objHelper.DecompressString(strBooking)));

            //Call com plus
            ComplusBooking objComplus = new ComplusBooking();
            try
            {
                dsResult = objComplus.BookingSaveSubLoad(ds.Tables["BookingHeader"],
                                            ds.Tables["FlightSegment"],
                                            ds.Tables["Passenger"],
                                            ds.Tables["Remark"],
                                            ds.Tables["Payment"],
                                            ds.Tables["Mapping"],
                                            ds.Tables["Service"],
                                            ds.Tables["Tax"],
                                            ds.Tables["Fee"],
                                            createTickets,
                                            readBooking,
                                            readOnly,
                                            true,
                                            true,
                                            true);

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    if (dsResult.Tables[0].Rows[0]["ResponseCode"].ToString() == "APPROVED")
                    {
                        if (createTickets == true)
                        {
                            objComplus.TicketCreate(ds.Tables["BookingHeader"].Rows[0]["agency_code"].ToString(),
                                                    ds.Tables["BookingHeader"].Rows[0]["update_by"].ToString(),
                                                    ds.Tables["BookingHeader"].Rows[0]["booking_id"].ToString(), null, null, false);
                        }

                        dsResult = objComplus.GetBooking(ds.Tables["BookingHeader"].Rows[0]["booking_id"].ToString(),
                                                string.Empty,
                                                0,
                                                false,
                                                false,
                                                string.Empty,
                                                true,
                                                true,
                                                true,
                                                true,
                                                true,
                                                true,
                                                true,
                                                true,
                                                true,
                                                true,
                                                string.Empty,
                                                string.Empty);

                    }
                }
            }
            catch (Exception e)
            {
                if (dsResult == null)
                {
                    Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog(e,
                                                                                                                          "Input XML : " + ds.GetXml().Replace("<", "&lt;").Replace(">", "&gt;") + "<br/>");
                }
                else
                {
                    Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog(e,
                                                                                                                         "Input XML : " + ds.GetXml().Replace("<", "&lt;").Replace(">", "&gt;") + "<br/>" +
                                                                                                                         "Result XML : " + dsResult.GetXml().Replace("<", "&lt;").Replace(">", "&gt;"));
                }
                throw;
            }
            finally
            { objComplus.Dispose(); }

            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                return objHelper.CompressString(dsResult.GetXml());
            }
            else
            {
                return string.Empty;
            }
        }



        [WebMethod]
        public bool PaymentSave(string strBooking, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AuthenUser"]) == false & string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AuthenPassword"]) == false)
            {
                string strKey = System.Configuration.ConfigurationManager.AppSettings["AuthenUser"] + System.Configuration.ConfigurationManager.AppSettings["AuthenPassword"];
                ComplusPayment objComplus = new ComplusPayment();
                DataSet ds = new DataSet();
                bool bResult = false;
                ds.ReadXml(new StringReader(SecurityHelper.DecryptString(strBooking, "")));

                bResult = objComplus.PaymentSave(ds.Tables["Payment"],
                                                ds.Tables["Allocation"],
                                                ds.Tables["refundVoucher"]);
                objComplus.Dispose();
                return bResult;
            }
            else
            {
                return false;
            }
        }
        [WebMethod]
        public string CalculateNewFees(string AgencyCode,
                                        string currency,
                                        DataSet dsBooking,
                                        bool checkBooking,
                                        bool checkSegment,
                                        bool checkName,
                                        bool checkSeat,
                                        bool checkSpecialService,
                                        string language,
                                        string token, bool bNovat)
        {

            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            DataTable dtHeader = dsBooking.Tables["BookingHeader"];
            DataTable dtSegment = dsBooking.Tables["FlightSegment"];
            DataTable dtPassenger = dsBooking.Tables["Passenger"];
            DataTable dtFees = dsBooking.Tables["Fee"];
            DataTable dtRemark = dsBooking.Tables["Remark"];
            DataTable dtPayment = dsBooking.Tables["Payment"];
            DataTable dtMapping = dsBooking.Tables["Mapping"];
            DataTable dtService = dsBooking.Tables["Service"];
            DataTable dtTax = dsBooking.Tables["Tax"];

            //Call com plus
            string strResult = string.Empty;
            ComplusFee objComplus = new ComplusFee();
            strResult = objComplus.CalculateNewFees(AgencyCode,
                                                    dtHeader,
                                                    dtSegment,
                                                    dtPassenger,
                                                    dtFees,
                                                    currency,
                                                    dtRemark,
                                                    dtPayment,
                                                    dtMapping,
                                                    dtService,
                                                    dtTax,
                                                    checkBooking,
                                                    checkSegment,
                                                    checkName,
                                                    checkSeat,
                                                    checkSpecialService,
                                                    language, bNovat);



            objComplus.Dispose();


            return strResult;
        }

        [WebMethod]
        public string GetVouchers(string recordLocator,
                                    string voucherNumber,
                                    string voucherID,
                                    string status,
                                    string recipient,
                                    string fOPSubType,
                                    string clientProfileId,
                                    string currency,
                                    string password,
                                    bool includeOpenVoucher,
                                    bool includeExpiredVoucher,
                                    bool includeUsedVoucher,
                                    bool includeVoidedVoucher,
                                    bool includeRefundable,
                                    bool includeFareOnly,
                                    bool write,
                                    string strAgencyCode,
                                    string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusMiscellaneous objComplus = new ComplusMiscellaneous();
            DataSet ds = null;

            try
            {
                ds = objComplus.GetVouchers(recordLocator,
                                            voucherNumber,
                                            voucherID,
                                            status,
                                            recipient,
                                            fOPSubType,
                                            clientProfileId,
                                            currency,
                                            password,
                                            includeOpenVoucher,
                                            includeExpiredVoucher,
                                            includeUsedVoucher,
                                            includeVoidedVoucher,
                                            includeRefundable,
                                            includeFareOnly,
                                            write,
                                            strAgencyCode);
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            { objComplus.Dispose(); }

            return ds.GetXml();
        }

        [WebMethod]
        public DataSet GetFormOfPaymentSubtypeFees(string formOfPayment,
                                                   string formOfPaymentSubtype,
                                                   string currencyRcd,
                                                   string agency,
                                                   DateTime feeDate,
                                                   string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusPayment objComplus = new ComplusPayment();
            DataSet ds = null;

            try
            {
                ds = objComplus.GetFormOfPaymentSubtypeFees(formOfPayment, formOfPaymentSubtype, currencyRcd, agency, feeDate);
            }
            catch (Exception e)
            {
                if (ds == null)
                {
                    Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog(e,
                                                                                                                          "formOfPayment : " + formOfPayment + "<br/>" +
                                                                                                                          "formOfPaymentSubtype : " + formOfPaymentSubtype + "<br/>" +
                                                                                                                          "currencyRcd : " + currencyRcd + "<br/>" +
                                                                                                                          "agency : " + agency + "<br/>" +
                                                                                                                          "feeDate : " + feeDate.ToString() + "<br/>" +
                                                                                                                          "token : " + token + "<br/>");
                }
                else
                {
                    Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog(e,
                                                                                                                         "formOfPayment : " + formOfPayment + "<br/>" +
                                                                                                                          "formOfPaymentSubtype : " + formOfPaymentSubtype + "<br/>" +
                                                                                                                          "currencyRcd : " + currencyRcd + "<br/>" +
                                                                                                                          "agency : " + agency + "<br/>" +
                                                                                                                          "feeDate : " + feeDate.ToString() + "<br/>" +
                                                                                                                          "token : " + token + "<br/>" +
                                                                                                                         "Result XML : " + ds.GetXml().Replace("<", "&lt;").Replace(">", "&gt;"));
                }
                throw;
            }
            finally
            { objComplus.Dispose(); }

            return ds;
        }

        [WebMethod]
        public string GetItinerary(string bookingId, string language, string passengerId, string agencyCode, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusXML objComplus = new ComplusXML();
            string strResult = string.Empty;

            try
            {
                if (language.Length == 0)
                {
                    //Set default language to english
                    language = "EN";
                }
                if (bookingId.Length > 0)
                {
                    strResult = objComplus.GetItinerary(new Guid(bookingId), language, passengerId, agencyCode);
                }
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            { objComplus.Dispose(); }

            return strResult;
        }
        [WebMethod]
        public string ItineraryRead(string recordLocator, string language, string passengerId, string agencyCode, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");
            ComplusXML objComplusXml = new ComplusXML();
            string strResult = string.Empty;

            try
            {
                strResult = objComplusXml.GetItinerary(recordLocator, language, passengerId, agencyCode);
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            {
                objComplusXml.Dispose();
            }

            return strResult;
        }
        [WebMethod]
        public bool QueueMail(string strFromAddress,
                              string strFromName,
                              string strToAddress,
                              string strToAddressCC,
                              string strToAddressBCC,
                              string strReplyToAddress,
                              string strSubject,
                              string strBody,
                              string strDocumentType,
                              string strAttachmentStream,
                              string strAttachmentFileName,
                              string strAttachmentFileType,
                              string strAttachmentParser,
                              bool bHtmlBody,
                              bool bConvertAttachmentFromHTML2PDF,
                              bool bRemoveFromQueue,
                              string strUserId,
                              string strBookingId,
                              string strVoucherId,
                              string strBookingSegmentID,
                              string strPassengerId,
                              string strClientProfileId,
                              string strDocumentId,
                              string strLanguageCode,
                              string token)
        {

            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusMessage objComplus = new ComplusMessage();
            bool bResult = false;

            try
            {
                Helper objHelper = new Helper();

                bResult = objComplus.QueueMail(strFromAddress,
                                              strFromName,
                                              strToAddress,
                                              strToAddressCC,
                                              strToAddressBCC,
                                              strReplyToAddress,
                                              strSubject,
                                              objHelper.DecompressString(strBody),
                                              strDocumentType,
                                              strAttachmentStream,
                                              strAttachmentFileName,
                                              strAttachmentFileType,
                                              strAttachmentParser,
                                              bHtmlBody,
                                              bConvertAttachmentFromHTML2PDF,
                                              bRemoveFromQueue,
                                              tikAeroProcess.MAIL_PRIORITY.NORMAL_PRIORITY,
                                              strUserId,
                                              strBookingId,
                                              strVoucherId,
                                              strBookingSegmentID,
                                              strPassengerId,
                                              strClientProfileId,
                                              strDocumentId,
                                              strLanguageCode);

                objHelper = null;
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            { objComplus.Dispose(); }

            return bResult;

        }
        [WebMethod]
        public bool CheckUniqueMailAddress(string strMail, string strClientProfileId, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusClient objComplus = new ComplusClient();
            bool iResult = false;

            try
            {
                iResult = objComplus.CheckUniqueMailAddress(strMail, strClientProfileId);
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            { objComplus.Dispose(); }

            return iResult;
        }
        [WebMethod]
        public string ServiceAuthentication(string strAuthen, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            XmlDocument xDoc = new XmlDocument();

            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AuthenUser"]) == false & string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AuthenPassword"]) == false)
            {
                string strKey = System.Configuration.ConfigurationManager.AppSettings["AuthenUser"] + System.Configuration.ConfigurationManager.AppSettings["AuthenPassword"];
                xDoc.LoadXml(SecurityHelper.DecryptString(strAuthen, strKey));
                if (xDoc.ChildNodes != null && xDoc.ChildNodes.Count > 0)
                {
                    string agencyCode = xDoc.ChildNodes[0]["agencyCode"].InnerXml;
                    string strUsername = xDoc.ChildNodes[0]["strUsername"].InnerXml;
                    string password = xDoc.ChildNodes[0]["password"].InnerXml;
                    string _strAgencyXml = "";

                    using (ComplusSystem objComplus = new ComplusSystem())
                    {
                        DataSet ds = objComplus.TravelAgentLogon(agencyCode, strUsername, password);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            string userID = "";
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                userID = dr["user_account_id"].ToString();
                                _strAgencyXml = "<Authen>" + objComplus.UserRead(userID).GetXml();
                                _strAgencyXml += objComplus.GetAgencySessionProfile(agencyCode, userID).GetXml() + "</Authen>";
                                break;
                            }

                        }
                    }
                    return SecurityHelper.EncryptString(_strAgencyXml, strKey);
                }
            }

            return string.Empty;
        }
        [WebMethod]
        public string SaveBookingCreditCard(string strBooking,
                                            string securityToken,
                                            string authenticationToken,
                                            string commerceIndicator,
                                            string bookingReference,
                                            string strRequestSource,
                                            bool createTickets,
                                            bool readBooking,
                                            bool readOnly,
                                            string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusBooking objComplus = new ComplusBooking();
            Helper objHelper = new Helper();
            DataSet dsResult = null;
            DateTime dtStart = DateTime.Now;
            DataSet ds = new DataSet();

            try
            {
                //Read xml to dataset
                ds.ReadXml(new StringReader(objHelper.DecompressString(strBooking)));

                //Call com plus
                //objHelper.DecryptCCValue(ds, Server.MapPath("~") + "/EncryptKey/tikAeroService.kez");

                dsResult = objComplus.SaveBookingCreditCard(ds.Tables["BookingHeader"],
                                                            ds.Tables["FlightSegment"],
                                                            ds.Tables["Passenger"],
                                                            ds.Tables["Remark"],
                                                            ds.Tables["Payment"],
                                                            ds.Tables["Mapping"],
                                                            ds.Tables["Service"],
                                                            ds.Tables["Tax"],
                                                            ds.Tables["Fee"],
                                                            ds.Tables["PaymentFee"],
                                                            null,
                                                            securityToken,
                                                            authenticationToken,
                                                            commerceIndicator,
                                                            bookingReference,
                                                            strRequestSource,
                                                            createTickets,
                                                            readBooking,
                                                            readOnly);



                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    if (dsResult.Tables[0].Rows[0]["ResponseCode"].ToString() == "APPROVED")
                    {
                        if (createTickets == true)
                        {
                            objComplus.TicketCreate(ds.Tables["BookingHeader"].Rows[0]["agency_code"].ToString(),
                                                    ds.Tables["BookingHeader"].Rows[0]["update_by"].ToString(),
                                                    ds.Tables["BookingHeader"].Rows[0]["booking_id"].ToString(), null, null, false);
                        }

                        dsResult = objComplus.GetBooking(ds.Tables["BookingHeader"].Rows[0]["booking_id"].ToString(),
                                                        string.Empty,
                                                        0,
                                                        false,
                                                        false,
                                                        string.Empty,
                                                        true,
                                                        true,
                                                        true,
                                                        true,
                                                        true,
                                                        true,
                                                        true,
                                                        true,
                                                        true,
                                                        true,
                                                        string.Empty,
                                                        string.Empty);

                    }
                }
            }
            catch (Exception e)
            {

                if (dsResult == null)
                {
                    Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog(e,
                                                                                                                          "Input XML : " + strBooking.Replace("<", "&lt;").Replace(">", "&gt;") + "<br/>");
                }
                else
                {
                    Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog(e,
                                                                                                                         "Input XML : " + strBooking.Replace("<", "&lt;").Replace(">", "&gt;") + "<br/>" +
                                                                                                                         "Result XML : " + dsResult.GetXml().Replace("<", "&lt;").Replace(">", "&gt;"));
                }

                throw;
            }
            finally
            { objComplus.Dispose(); }

            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                return objHelper.CompressString(dsResult.GetXml());
            }
            else
            {
                return string.Empty;
            }
        }
        [WebMethod]
        public string SaveBookingPayment(string strBooking,
                                            bool createTickets,
                                            bool readBooking,
                                            bool readOnly,
                                            string token)
        {
            DateTime dtStart = DateTime.Now;
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            Helper objHelper = new Helper();
            ComplusBooking objComplus = new ComplusBooking();
            DataSet dsResult = null;
            DataSet ds = new DataSet();
            try
            {
                //Read xml to dataset
                ds.ReadXml(new StringReader(objHelper.DecompressString(strBooking)));

                //Call com plus
                dsResult = objComplus.SaveBookingPayment(ds.Tables["BookingHeader"],
                                                 ds.Tables["FlightSegment"],
                                                 ds.Tables["Passenger"],
                                                 ds.Tables["Remark"],
                                                 ds.Tables["Payment"],
                                                 ds.Tables["Mapping"],
                                                 ds.Tables["Service"],
                                                 ds.Tables["Tax"],
                                                 ds.Tables["Fee"],
                                                 ds.Tables["PaymentFee"],
                                                 null,
                                                 null,
                                                 createTickets,
                                                 readBooking,
                                                 readOnly);

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    if (dsResult.Tables[0].Rows[0]["ResponseCode"].ToString() == "APPROVED")
                    {
                        if (createTickets == true)
                        {
                            objComplus.TicketCreate(ds.Tables["BookingHeader"].Rows[0]["agency_code"].ToString(),
                                                    ds.Tables["BookingHeader"].Rows[0]["update_by"].ToString(),
                                                    ds.Tables["BookingHeader"].Rows[0]["booking_id"].ToString(), null, null, false);
                        }

                        dsResult = objComplus.GetBooking(ds.Tables["BookingHeader"].Rows[0]["booking_id"].ToString(),
                                                string.Empty,
                                                0,
                                                false,
                                                false,
                                                string.Empty,
                                                true,
                                                true,
                                                true,
                                                true,
                                                true,
                                                true,
                                                true,
                                                true,
                                                true,
                                                true,
                                                string.Empty,
                                                string.Empty);

                    }
                }
            }
            catch (Exception e)
            {
                if (dsResult == null)
                {
                    Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog(e,
                                                                                                                          "Input XML : " + ds.GetXml().Replace("<", "&lt;").Replace(">", "&gt;") + "<br/>");
                }
                else
                {
                    Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog(e,
                                                                                                                         "Input XML : " + ds.GetXml().Replace("<", "&lt;").Replace(">", "&gt;") + "<br/>" +
                                                                                                                         "Result XML : " + dsResult.GetXml().Replace("<", "&lt;").Replace(">", "&gt;"));
                }
                throw;
            }
            finally
            { objComplus.Dispose(); }

            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                return objHelper.CompressString(dsResult.GetXml());
            }
            else
            {
                return string.Empty;
            }
        }
        [WebMethod]
        public string SaveBookingMultipleFormOfPayment(string strBookingId,
                                                       string strBooking,
                                                       bool bCreateTickets,
                                                       string strSecurityToken,
                                                       string strAuthenticationToken,
                                                       string strCommerceIndicator,
                                                       string strRequestSource,
                                                       string strLanguage,
                                                       string token)
        {
            DateTime dtStart = DateTime.Now;
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            Helper objHelper = new Helper();
            ComplusBooking objComplus = new ComplusBooking();
            DataSet dsResult = null;
            DataSet ds = new DataSet();
            try
            {
                //Read xml to dataset
                ds.ReadXml(new StringReader(objHelper.DecompressString(strBooking)));

                //Call com plus
                dsResult = objComplus.SaveBookingMultipleFormOfPayment(ds.Tables["BookingHeader"],
                                                                       ds.Tables["FlightSegment"],
                                                                       ds.Tables["Passenger"],
                                                                       ds.Tables["Remark"],
                                                                       ds.Tables["Payment"],
                                                                       ds.Tables["Mapping"],
                                                                       ds.Tables["Service"],
                                                                       ds.Tables["Tax"],
                                                                       ds.Tables["Fee"],
                                                                       ds.Tables["PaymentFee"],
                                                                       strBookingId,
                                                                       bCreateTickets,
                                                                       strSecurityToken,
                                                                       strAuthenticationToken,
                                                                       strCommerceIndicator,
                                                                       strRequestSource,
                                                                       strLanguage);

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    if (dsResult.Tables[0].Rows[0]["ResponseCode"].ToString() == "APPROVED")
                    {
                        if (bCreateTickets == true)
                        {
                            objComplus.TicketCreate(ds.Tables["BookingHeader"].Rows[0]["agency_code"].ToString(),
                                                    ds.Tables["BookingHeader"].Rows[0]["update_by"].ToString(),
                                                    ds.Tables["BookingHeader"].Rows[0]["booking_id"].ToString(), null, null, false);
                        }

                        dsResult = objComplus.GetBooking(ds.Tables["BookingHeader"].Rows[0]["booking_id"].ToString(),
                                                        string.Empty,
                                                        0,
                                                        false,
                                                        false,
                                                        string.Empty,
                                                        true,
                                                        true,
                                                        true,
                                                        true,
                                                        true,
                                                        true,
                                                        true,
                                                        true,
                                                        true,
                                                        true,
                                                        string.Empty,
                                                        string.Empty);

                    }
                }
            }
            catch (Exception e)
            {
                if (dsResult == null)
                {
                    Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog(e,
                                                                                                                          "Input XML : " + ds.GetXml().Replace("<", "&lt;").Replace(">", "&gt;") + "<br/>" +
                                                                                                                          "strBookingId : " + strBookingId + "<br/>" +
                                                                                                                          "bCreateTickets : " + bCreateTickets + "<br/>" +
                                                                                                                          "strSecurityToken : " + bCreateTickets + "<br/>" +
                                                                                                                          "strAuthenticationToken : " + strAuthenticationToken + "<br/>" +
                                                                                                                          "strCommerceIndicator : " + strCommerceIndicator + "<br/>" +
                                                                                                                          "strRequestSource : " + strRequestSource + "<br/>" +
                                                                                                                          "strLanguage : " + strLanguage + "<br/>");
                }
                else
                {
                    Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog(e,
                                                                                                                         "Input XML : " + ds.GetXml().Replace("<", "&lt;").Replace(">", "&gt;") + "<br/>" +
                                                                                                                         "Result XML : " + dsResult.GetXml().Replace("<", "&lt;").Replace(">", "&gt;") + "<br/>" +
                                                                                                                         "strBookingId : " + strBookingId + "<br/>" +
                                                                                                                         "bCreateTickets : " + bCreateTickets + "<br/>" +
                                                                                                                         "strSecurityToken : " + bCreateTickets + "<br/>" +
                                                                                                                         "strAuthenticationToken : " + strAuthenticationToken + "<br/>" +
                                                                                                                         "strCommerceIndicator : " + strCommerceIndicator + "<br/>" +
                                                                                                                         "strRequestSource : " + strRequestSource + "<br/>" +
                                                                                                                         "strLanguage : " + strLanguage + "<br/>");
                }
                throw;
            }
            finally
            { objComplus.Dispose(); }

            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                return objHelper.CompressString(dsResult.GetXml());
            }
            else
            {
                return string.Empty;
            }
        }


        [WebMethod]
        public tikAeroWebMain.Classes.WsWrapper BookingLogon(string recordLocator, string NameOrPhone, string strAgencyCode, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            tikAeroWebMain.Classes.WsWrapper wsResponse = new tikAeroWebMain.Classes.WsWrapper();
            DataSet ds = null;
            using (ComplusSystem objComplus = new ComplusSystem())
            {
                try
                {
                    using (ds = objComplus.BookingLogon(recordLocator, NameOrPhone))
                    {
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                if (strAgencyCode.Length == 0)
                                {
                                    strAgencyCode = DataHelper.DBToString(ds.Tables[0].Rows[0], "agency_code");
                                }

                                using (DataSet dsAgency = objComplus.GetAgencySessionProfile(strAgencyCode, string.Empty))
                                {
                                    if (dsAgency != null && dsAgency.Tables.Count > 0 && dsAgency.Tables[0].Rows.Count > 0)
                                    {
                                        tikSystem.Web.Library.Library objLi = new tikSystem.Web.Library.Library();

                                        //Set Response Error Code
                                        wsResponse.ErrorCode = "000";
                                        wsResponse.ErrorMessage = "SUCCESS";

                                        //Fill Agency information
                                        wsResponse.Agencies = new tikSystem.Web.Library.AgencyAccounts();
                                        tikSystem.Web.Library.AgencyAccount agent = new tikSystem.Web.Library.AgencyAccount();
                                        DataRow dr = dsAgency.Tables[0].Rows[0];

                                        agent.agency_code = DataHelper.DBToString(dr, "agency_code");
                                        agent.agency_logon = DataHelper.DBToString(dr, "agency_logon");
                                        agent.agency_password = DataHelper.DBToString(dr, "agency_password");
                                        agent.agency_name = DataHelper.DBToString(dr, "agency_name");
                                        agent.airport_rcd = DataHelper.DBToString(dr, "airport_rcd");
                                        agent.default_e_ticket_flag = DataHelper.DBToByte(dr, "default_e_ticket_flag");
                                        agent.contact_person = DataHelper.DBToString(dr, "contact_person");
                                        agent.address_line1 = DataHelper.DBToString(dr, "address_line1");
                                        agent.address_line2 = DataHelper.DBToString(dr, "address_line2");
                                        agent.street = DataHelper.DBToString(dr, "street");
                                        agent.state = DataHelper.DBToString(dr, "state");
                                        agent.district = DataHelper.DBToString(dr, "district");
                                        agent.province = DataHelper.DBToString(dr, "province");
                                        agent.city = DataHelper.DBToString(dr, "city");
                                        agent.zip_code = DataHelper.DBToString(dr, "zip_code");
                                        agent.po_box = DataHelper.DBToString(dr, "po_box");
                                        agent.phone = DataHelper.DBToString(dr, "phone");
                                        agent.fax = DataHelper.DBToString(dr, "fax");
                                        agent.email = DataHelper.DBToString(dr, "email");
                                        agent.website_address = DataHelper.DBToString(dr, "website_address");
                                        agent.currency_rcd = DataHelper.DBToString(dr, "currency_rcd");
                                        agent.country_rcd = DataHelper.DBToString(dr, "country_rcd");
                                        agent.agency_payment_type_rcd = DataHelper.DBToString(dr, "agency_payment_type_rcd");
                                        agent.status_code = DataHelper.DBToString(dr, "status_code");
                                        agent.default_show_passenger_flag = DataHelper.DBToByte(dr, "default_show_passenger_flag");
                                        agent.default_auto_print_ticket_flag = DataHelper.DBToByte(dr, "default_auto_print_ticket_flag");
                                        agent.default_ticket_on_save_flag = DataHelper.DBToByte(dr, "default_ticket_on_save_flag");
                                        agent.default_ticket_on_payment_flag = DataHelper.DBToByte(dr, "default_ticket_on_payment_flag");
                                        agent.web_agency_flag = DataHelper.DBToByte(dr, "web_agency_flag");
                                        agent.own_agency_flag = DataHelper.DBToByte(dr, "own_agency_flag");
                                        agent.tty_address = DataHelper.DBToString(dr, "tty_address");
                                        agent.b2b_credit_card_payment_flag = DataHelper.DBToByte(dr, "b2b_credit_card_payment_flag");
                                        agent.b2b_voucher_payment_flag = DataHelper.DBToByte(dr, "b2b_voucher_payment_flag");
                                        agent.b2b_post_paid_flag = DataHelper.DBToByte(dr, "b2b_post_paid_flag");
                                        agent.b2b_allow_seat_assignment_flag = DataHelper.DBToByte(dr, "b2b_allow_seat_assignment_flag");
                                        agent.b2b_allow_cancel_segment_flag = DataHelper.DBToByte(dr, "b2b_allow_cancel_segment_flag");
                                        agent.b2b_allow_change_flight_flag = DataHelper.DBToByte(dr, "b2b_allow_change_flight_flag");
                                        agent.b2b_allow_name_change_flag = DataHelper.DBToByte(dr, "b2b_allow_name_change_flag");
                                        agent.b2b_allow_change_details_flag = DataHelper.DBToByte(dr, "b2b_allow_change_details_flag");
                                        agent.ticket_stock_flag = DataHelper.DBToByte(dr, "ticket_stock_flag");
                                        agent.b2b_allow_split_flag = DataHelper.DBToByte(dr, "b2b_allow_split_flag");
                                        agent.b2b_allow_service_flag = DataHelper.DBToByte(dr, "b2b_allow_service_flag");
                                        agent.b2b_group_waitlist_flag = DataHelper.DBToByte(dr, "b2b_group_waitlist_flag");
                                        agent.avl_show_net_total_flag = DataHelper.DBToByte(dr, "avl_show_net_total_flag");
                                        agent.default_user_account_id = DataHelper.DBToGuid(dr, "default_user_account_id");
                                        agent.create_date_time = DataHelper.DBToDateTime(dr, "create_date_time");
                                        agent.update_date_time = DataHelper.DBToDateTime(dr, "update_date_time");
                                        agent.cashbook_closing_rcd = DataHelper.DBToString(dr, "cashbook_closing_rcd");
                                        agent.merchant_id = DataHelper.DBToGuid(dr, "merchant_id");
                                        agent.cashbook_closing_id = DataHelper.DBToGuid(dr, "cashbook_closing_id");
                                        agent.create_by = DataHelper.DBToGuid(dr, "create_by");

                                        wsResponse.Agencies.Add(agent);

                                        //Fill Booking information.
                                        wsResponse.Header = new tikSystem.Web.Library.BookingHeader();
                                        wsResponse.Header.booking_id = DataHelper.DBToGuid(ds.Tables[0].Rows[0], "booking_id");
                                        wsResponse.Header.currency_rcd = DataHelper.DBToString(ds.Tables[0].Rows[0], "currency_rcd");
                                        wsResponse.Header.own_agency_flag = DataHelper.DBToByte(ds.Tables[0].Rows[0], "own_agency_flag");
                                        wsResponse.Header.web_agency_flag = DataHelper.DBToByte(ds.Tables[0].Rows[0], "web_agency_flag");
                                        return wsResponse;
                                    }
                                    else
                                    {
                                        wsResponse.ErrorCode = "101";
                                        wsResponse.ErrorMessage = "Agency information not found";
                                        return wsResponse;
                                    }
                                }
                            }
                            else
                            {
                                wsResponse.ErrorCode = "100";
                                wsResponse.ErrorMessage = "BOOKING LOGON FAILED";
                                return wsResponse;
                            }
                        }
                        else
                        {
                            wsResponse.ErrorCode = "100";
                            wsResponse.ErrorMessage = "BOOKING LOGON FAILED";
                            return wsResponse;
                        }
                    }
                }
                catch (Exception e)
                {
                    wsResponse.ErrorCode = "101";
                    wsResponse.ErrorMessage = e.Message;
                    return wsResponse;
                }
            }
        }

        [WebMethod]
        public Currencies GetCurrencies(string strLanguage, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusPayment objPayment = new ComplusPayment();
            try
            {
                return objPayment.GetCurrencies(strLanguage);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [WebMethod]
        public Services GetSpecialServices(string strLanguage, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");
            try
            {
                ComplusReservation objReservation = new ComplusReservation();
                if (string.IsNullOrEmpty(strLanguage))
                {
                    strLanguage = "EN";
                }
                Services services = (Services)HttpRuntime.Cache["SpecialServices-" + strLanguage.ToUpper()];
                if (services != null && services.Count > 0)
                {
                    return services;
                }
                else
                {
                    services = objReservation.GetSpecialServices(strLanguage);

                    //Assign service to cache.
                    HttpRuntime.Cache.Insert("SpecialServices-" + strLanguage.ToUpper(),
                                            services,
                                            null,
                                            System.Web.Caching.Cache.NoAbsoluteExpiration,
                                            TimeSpan.FromMinutes(20),
                                            System.Web.Caching.CacheItemPriority.Normal,
                                            null);
                    return services;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [WebMethod]
        public Users TravelAgentLogon(string agencyCode, string agentLogon, string agentPassword, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");
            Users objUsers = new Users();
            ComplusSystem objComplus = new ComplusSystem();

            try
            {
                DataSet ds = objComplus.TravelAgentLogon(agencyCode, agentLogon, agentPassword);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string userID = "";
                    tikSystem.Web.Library.User u;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        userID = dr["user_account_id"].ToString();
                        DataSet dsUser = objComplus.UserRead(userID);

                        foreach (DataRow drUser in dsUser.Tables[0].Rows)
                        {
                            u = new tikSystem.Web.Library.User();
                            //convert Recordset to Object
                            u.user_account_id = DataHelper.DBToGuid(drUser, "user_account_id");
                            u.user_logon = DataHelper.DBToString(drUser, "user_logon");
                            u.lastname = DataHelper.DBToString(drUser, "lastname");
                            u.firstname = DataHelper.DBToString(drUser, "firstname");
                            u.email_address = DataHelper.DBToString(drUser, "email_address");
                            u.language_rcd = DataHelper.DBToString(drUser, "language_rcd");
                            u.make_bookings_for_others_flag = DataHelper.DBToByte(drUser, "make_bookings_for_others_flag");
                            u.address_default_code = DataHelper.DBToString(drUser, "address_default_code");
                            u.update_booking_flag = DataHelper.DBToByte(drUser, "update_booking_flag");
                            u.change_segment_flag = DataHelper.DBToByte(drUser, "change_segment_flag");
                            u.delete_segment_flag = DataHelper.DBToByte(drUser, "delete_segment_flag");
                            u.issue_ticket_flag = DataHelper.DBToByte(drUser, "issue_ticket_flag");
                            u.counter_sales_report_flag = DataHelper.DBToByte(drUser, "counter_sales_report_flag");
                            u.status_code = DataHelper.DBToString(drUser, "status_code");
                            u.create_by = DataHelper.DBToGuid(drUser, "create_by");
                            u.update_by = DataHelper.DBToGuid(drUser, "update_by");
                            u.system_admin_flag = DataHelper.DBToByte(drUser, "system_admin_flag");
                            u.mon_flag = DataHelper.DBToByte(drUser, "system_admin_flag");
                            u.tue_flag = DataHelper.DBToByte(drUser, "system_admin_flag");
                            u.wed_flag = DataHelper.DBToByte(drUser, "system_admin_flag");
                            u.thu_flag = DataHelper.DBToByte(drUser, "system_admin_flag");
                            u.fri_flag = DataHelper.DBToByte(drUser, "system_admin_flag");
                            u.sat_flag = DataHelper.DBToByte(drUser, "system_admin_flag");
                            u.sun_flag = DataHelper.DBToByte(drUser, "system_admin_flag");
                            u.create_date_time = DataHelper.DBToDateTime(drUser, "create_date_time");
                            u.update_date_time = DataHelper.DBToDateTime(drUser, "update_date_time");

                            objUsers.Add(u);
                            u = null;
                        }
                        break;
                    }

                }
                return objUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public bool UserSave(User objUser, string agencyCode, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");
            bool success = false;
            ComplusSetting objSetting = new ComplusSetting();
            try
            {
                success = objSetting.UserSave(objUser, agencyCode);
                return success;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [WebMethod]
        public Agents GetAgencySessionProfile(string strAgencyCode, string strUserId, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            try
            {
                ComplusSystem objComplus = new ComplusSystem();
                Agents agents = new Agents();
                DataSet ds = objComplus.GetAgencySessionProfile(strAgencyCode, strUserId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Agent ag;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ag = new Agent();

                        ag.agency_code = DataHelper.DBToString(dr, "agency_code");
                        ag.agency_logon = DataHelper.DBToString(dr, "agency_logon");
                        ag.agency_password = DataHelper.DBToString(dr, "agency_password");
                        ag.agency_name = DataHelper.DBToString(dr, "agency_name");
                        ag.airport_rcd = DataHelper.DBToString(dr, "airport_rcd");
                        ag.ag_language_rcd = DataHelper.DBToString(dr, "ag_language_rcd");
                        ag.default_e_ticket_flag = DataHelper.DBToByte(dr, "default_e_ticket_flag");
                        ag.email = DataHelper.DBToString(dr, "email");
                        ag.currency_rcd = DataHelper.DBToString(dr, "currency_rcd");
                        ag.country_rcd = DataHelper.DBToString(dr, "country_rcd");
                        ag.agency_payment_type_rcd = DataHelper.DBToString(dr, "agency_payment_type_rcd");
                        ag.status_code = DataHelper.DBToString(dr, "status_code");
                        ag.default_show_passenger_flag = DataHelper.DBToByte(dr, "default_show_passenger_flag");
                        ag.default_auto_print_ticket_flag = DataHelper.DBToByte(dr, "default_auto_print_ticket_flag");
                        ag.default_ticket_on_save_flag = DataHelper.DBToByte(dr, "default_ticket_on_save_flag");
                        ag.web_agency_flag = DataHelper.DBToByte(dr, "web_agency_flag");
                        ag.own_agency_flag = DataHelper.DBToByte(dr, "own_agency_flag");
                        ag.b2b_credit_card_payment_flag = DataHelper.DBToByte(dr, "b2b_credit_card_payment_flag");
                        ag.b2b_voucher_payment_flag = DataHelper.DBToByte(dr, "b2b_voucher_payment_flag");
                        ag.b2b_eft_flag = DataHelper.DBToByte(dr, "b2b_eft_flag");
                        ag.b2b_post_paid_flag = DataHelper.DBToByte(dr, "b2b_post_paid_flag");
                        ag.b2b_allow_seat_assignment_flag = DataHelper.DBToByte(dr, "b2b_allow_seat_assignment_flag");
                        ag.b2b_allow_cancel_segment_flag = DataHelper.DBToByte(dr, "b2b_allow_cancel_segment_flag");
                        ag.b2b_allow_change_flight_flag = DataHelper.DBToByte(dr, "b2b_allow_change_flight_flag");
                        ag.b2b_allow_name_change_flag = DataHelper.DBToByte(dr, "b2b_allow_name_change_flag");
                        ag.b2b_allow_change_details_flag = DataHelper.DBToByte(dr, "b2b_allow_change_details_flag");
                        ag.ticket_stock_flag = DataHelper.DBToByte(dr, "ticket_stock_flag");
                        ag.b2b_allow_split_flag = DataHelper.DBToByte(dr, "b2b_allow_split_flag");
                        ag.b2b_allow_service_flag = DataHelper.DBToByte(dr, "b2b_allow_service_flag");
                        ag.b2b_group_waitlist_flag = DataHelper.DBToByte(dr, "b2b_group_waitlist_flag");
                        ag.avl_show_net_total_flag = DataHelper.DBToByte(dr, "avl_show_net_total_flag");
                        ag.default_user_account_id = DataHelper.DBToGuid(dr, "default_user_account_id");
                        ag.merchant_id = DataHelper.DBToGuid(dr, "merchant_id");
                        ag.default_customer_document_id = DataHelper.DBToGuid(dr, "default_customer_document_id");
                        ag.default_small_itinerary_document_id = DataHelper.DBToGuid(dr, "default_small_itinerary_document_id");
                        ag.default_internal_itinerary_document_id = DataHelper.DBToGuid(dr, "default_internal_itinerary_document_id");
                        ag.payment_default_code = DataHelper.DBToString(dr, "payment_default_code");
                        ag.agency_type_code = DataHelper.DBToString(dr, "agency_type_code");
                        ag.user_account_id = DataHelper.DBToGuid(dr, "user_account_id");
                        ag.user_logon = DataHelper.DBToString(dr, "user_logon");
                        ag.user_code = DataHelper.DBToString(dr, "user_code");
                        ag.lastname = DataHelper.DBToString(dr, "lastname");
                        ag.firstname = DataHelper.DBToString(dr, "firstname");
                        ag.language_rcd = DataHelper.DBToString(dr, "language_rcd");
                        ag.make_bookings_for_others_flag = DataHelper.DBToByte(dr, "make_bookings_for_others_flag");
                        ag.origin_rcd = DataHelper.DBToString(dr, "origin_rcd");
                        ag.outstanding_invoice = DataHelper.DBToDecimal(dr, "outstanding_invoice");
                        ag.booking_payment = DataHelper.DBToDecimal(dr, "booking_payment");
                        ag.agency_account = DataHelper.DBToDecimal(dr, "agency_account");
                        ag.company_client_profile_id = DataHelper.DBToGuid(dr, "company_client_profile_id");
                        ag.invoice_days = DataHelper.DBToString(dr, "invoice_days");
                        ag.address_line1 = DataHelper.DBToString(dr, "address_line1");
                        ag.address_line2 = DataHelper.DBToString(dr, "address_line2");
                        ag.city = DataHelper.DBToString(dr, "city");
                        ag.bank_code = DataHelper.DBToString(dr, "bank_code");
                        ag.bank_name = DataHelper.DBToString(dr, "bank_name");
                        ag.bank_account = DataHelper.DBToString(dr, "bank_account");
                        ag.contact_person = DataHelper.DBToString(dr, "contact_person");
                        ag.district = DataHelper.DBToString(dr, "district");
                        ag.phone = DataHelper.DBToString(dr, "phone");
                        ag.fax = DataHelper.DBToString(dr, "fax");
                        ag.po_box = DataHelper.DBToString(dr, "po_box");
                        ag.province = DataHelper.DBToString(dr, "province");
                        ag.state = DataHelper.DBToString(dr, "state");
                        ag.street = DataHelper.DBToString(dr, "street");
                        ag.zip_code = DataHelper.DBToString(dr, "zip_code");
                        ag.consolidator_flag = DataHelper.DBToByte(dr, "consolidator_flag");
                        ag.b2b_credit_agency_and_invoice_flag = DataHelper.DBToByte(dr, "b2b_credit_agency_and_invoice_flag");
                        ag.b2b_download_sales_report_flag = DataHelper.DBToByte(dr, "b2b_download_sales_report_flag");
                        ag.b2b_show_remarks_flag = DataHelper.DBToByte(dr, "b2b_show_remarks_flag");
                        ag.private_fares_flag = DataHelper.DBToByte(dr, "private_fares_flag");
                        ag.b2b_allow_group_flag = DataHelper.DBToByte(dr, "b2b_allow_group_flag");
                        ag.b2b_allow_waitlist_flag = DataHelper.DBToByte(dr, "b2b_allow_waitlist_flag");
                        ag.b2b_bsp_billing_flag = DataHelper.DBToByte(dr, "b2b_bsp_billing_flag");
                        ag.b2b_bsp_from_date = DataHelper.DBToDateTime(dr, "b2b_bsp_from_date");
                        ag.iata_number = DataHelper.DBToString(dr, "iata_number");
                        ag.send_mailto_all_passenger = DataHelper.DBToByte(dr, "send_mailto_all_passenger");
                        ag.website_address = DataHelper.DBToString(dr, "website_address");
                        ag.tty_address = DataHelper.DBToString(dr, "tty_address");
                        ag.create_date_time = DataHelper.DBToDateTime(dr, "create_date_time");
                        ag.update_date_time = DataHelper.DBToDateTime(dr, "update_date_time");
                        ag.cashbook_closing_rcd = DataHelper.DBToString(dr, "cashbook_closing_rcd");
                        ag.cashbook_closing_id = DataHelper.DBToGuid(dr, "cashbook_closing_id");
                        ag.create_by = DataHelper.DBToGuid(dr, "create_by");
                        ag.legal_name = DataHelper.DBToString(dr, "legal_name");
                        ag.tax_id = DataHelper.DBToString(dr, "tax_id");
                        ag.tax_id_verified_date_time = DataHelper.DBToDateTime(dr, "tax_id_verified_date_time");
                        ag.no_vat_flag = DataHelper.DBToByte(dr, "no_vat_flag");
                        ag.allow_no_tax = DataHelper.DBToByte(dr, "allow_no_tax");

                        ag.allow_add_segment_flag = DataHelper.DBToByte(dr, "allow_add_segment_flag");
                        ag.individual_firmed_flag = DataHelper.DBToByte(dr, "individual_firmed_flag");
                        ag.individual_waitlist_flag = DataHelper.DBToByte(dr, "individual_waitlist_flag");
                        ag.group_firmed_flag = DataHelper.DBToByte(dr, "group_firmed_flag");
                        ag.group_waitlist_flag = DataHelper.DBToByte(dr, "group_waitlist_flag");
                        ag.disable_changes_through_b2c_flag = DataHelper.DBToByte(dr, "disable_changes_through_b2c_flag");
                        ag.disable_web_checkin_flag = DataHelper.DBToByte(dr, "disable_web_checkin_flag");
                        ag.commission_percentage = DataHelper.DBToDecimal(dr, "commission_percentage");
                        ag.balance_lock_flag = DataHelper.DBToByte(dr, "balance_lock_flag");

                        ag.column_1_tax_rcd = DataHelper.DBToString(dr, "column_1_tax_rcd");
                        ag.column_2_tax_rcd = DataHelper.DBToString(dr, "column_2_tax_rcd");
                        ag.column_3_tax_rcd = DataHelper.DBToString(dr, "column_3_tax_rcd");

                        agents.Add(ag);
                        ag = null;
                    }
                }
                return agents;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public Clients GetClientSessionProfile(Guid clientProfileId, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusSystem objComplus = new ComplusSystem();
            try
            {
                return objComplus.GetClientSessionProfile(clientProfileId.ToString());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [WebMethod]
        public string GetFlightSummary(tikSystem.Web.Library.Flights objFlights,
                                            Passengers passengers,
                                            string strAgencyCode,
                                            string flightId,
                                            string boardPoint,
                                            string offPoint,
                                            string airline,
                                            string flight,
                                            string boardingClass,
                                            string bookingClass,
                                            DateTime flightDate,
                                            string fareId,
                                            string currencyCode,
                                            bool isRefundable,
                                            bool isGroupBooking,
                                            bool isNonRevenue,
                                            string segmentId,
                                            Int16 idReduction,
                                            string fareType,
                                            string language,
                                            string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusFares objFares = new ComplusFares();
            try
            {
                return objFares.GetFlightSummary(passengers,
                                                objFlights,
                                                strAgencyCode,
                                                flightId,
                                                boardPoint,
                                                offPoint,
                                                airline,
                                                flight,
                                                boardingClass,
                                                bookingClass,
                                                flightDate,
                                                fareId,
                                                currencyCode,
                                                isRefundable,
                                                isGroupBooking,
                                                isNonRevenue,
                                                segmentId,
                                                idReduction,
                                                fareType,
                                                language,
                                                false);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

      

        [WebMethod]
        public DataSet GetBookings(string airline, string flightNumber, string flightId, string flightFrom, string flightTo,
                            string recordLocator, string origin, string destination, string passengerName, string seatNumber, string ticketNumber,
                            string phoneNumber, string agencyCode, string clientNumber, string memberNumber, string clientId, bool showHistory,
                            string language, bool bIndividual, bool bGroup, string createFrom, string createTo, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");
            ComplusReservation objReservationy = new ComplusReservation();

            DataSet ds = new DataSet("Bookings");

            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["TikAeroConnectionString"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    
                    string strPassengerName = string.Empty;
                    string strName1 = string.Empty;
                    string strName2 = string.Empty;

                    if (!string.IsNullOrEmpty(passengerName))
                        passengerName.Trim();
                    {
                        if (passengerName.Contains("/"))
                        {
                            string[] strName = passengerName.Split('/');
                            strName1 = strName[0].Trim();
                            strName2 = strName[1].Trim();
                        }
                        else if (passengerName.Length > 0)
                        {
                            string[] strName = passengerName.Split(' ');
                            strName1 = strName[0].Trim();
                            if (strName1.Length == 0)
                                strName1 = passengerName.Trim();

                        }
                    }

                    string sql = @"exec dbo.get_bookings_api";
                    if (string.IsNullOrEmpty(origin))
                        sql += @"'',";
                    else
                        sql += @"'"+origin+"',";

                    if (string.IsNullOrEmpty(destination))
                        sql += @"'',";
                    else
                        sql += @"'" + destination + "',";


                    if (string.IsNullOrEmpty(flightId))
                        sql += @"null,";
                    else
                        sql += @"'" + flightId + "',";


                    if (string.IsNullOrEmpty(airline))
                        sql += @"'',";
                    else
                        sql += @"'" + airline + "',";


                    if (string.IsNullOrEmpty(flightNumber))
                        sql += @"'',";
                    else
                        sql += @"'" + flightNumber + "',";


                    if (string.IsNullOrEmpty(flightFrom) || flightFrom =="0001-01-01")
                        sql += @"'',";
                    else
                    {
                        if (flightFrom.Contains("-"))
                        {
                            flightFrom = flightFrom.Replace("-","");
                            sql += @"'" + flightFrom + "',";
                        }
                        else
                        {
                            sql += @"'',";
                        }
                    }

                    if (string.IsNullOrEmpty(flightTo) || flightTo == "0001-01-01")
                        sql += @"'',";
                    else
                        if (flightTo.Contains("-"))
                        {
                            flightTo = flightTo.Replace("-", "");
                            sql += @"'" + flightTo + "',";
                        }
                        else
                        {
                            sql += @"'',";
                        }

                    if (string.IsNullOrEmpty(recordLocator))
                        sql += @"'',";
                    else
                        sql += @"'" + recordLocator + "',";

                    if (string.IsNullOrEmpty(ticketNumber))
                        sql += @"'',";
                    else
                        sql += @"'" + ticketNumber + "',";

                    if (string.IsNullOrEmpty(strName1))
                        sql += @"'',";
                    else
                        sql += @"'" + strName1 + "',";

                    if (string.IsNullOrEmpty(strName2))
                        sql += @"'',";
                    else
                        sql += @"'" + strName2 + "',";

                    if (string.IsNullOrEmpty(phoneNumber))
                        sql += @"'',";
                    else
                        sql += @"'" + phoneNumber + "',";

                    if (string.IsNullOrEmpty(seatNumber))
                        sql += @"'',";
                    else
                        sql += @"'" + seatNumber + "',";

                    if (string.IsNullOrEmpty(agencyCode))
                        sql += @"'',";
                    else
                        sql += @"'" + agencyCode + "',";


                    if (string.IsNullOrEmpty(clientId))
                        sql += @"null,";
                    else
                        sql += @"'" + clientId + "',";

                    sql += @"0,";
                    //0,-1,-1,";
                    if (showHistory)
                        sql += @"-1,";
                    else
                        sql += @"0,";

                    if (bIndividual)
                        sql += @"-1,";
                    else
                        sql += @"0,";

                    if (bGroup)
                        sql += @"-1,";
                    else
                        sql += @"0,";

                    sql += @"EN,";

                    if (string.IsNullOrEmpty(createFrom) || createFrom == "0001-01-01")
                        sql += @"'',";
                    else
                    {
                        if (createFrom.Contains("-"))
                        {
                            createFrom = createFrom.Replace("-", "");
                            sql += @"'" + createFrom + "',";
                        }
                        else
                        {
                            sql += @"'',";
                        }
                    }

                    if (string.IsNullOrEmpty(createTo) || createTo == "0001-01-01")
                        sql += @"'',";
                    else
                        if (createTo.Contains("-"))
                        {
                            createTo = createTo.Replace("-", "");
                            sql += @"'" + createTo + "',";
                        }
                        else
                        {
                            sql += @"'',";
                        }

                    //iata
                    sql += @"'',";
                    //userid
                    sql += @"null,";

                    //member number
                    if (string.IsNullOrEmpty(memberNumber))
                        sql += @"''";
                    else
                        sql += @"'" + memberNumber + "'";

                    SqlCommand sqlComm = new SqlCommand(sql, conn);

                    sqlComm.CommandType = CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);

                    ds.Tables[0].TableName = "Booking";

                }
                return ds;
            }
            catch (Exception e)
            {
                // throw new Exception(e.Message);
                throw new Exception("System error.");
            }
            finally
            {
                objReservationy.Dispose();
            }
        }


        [WebMethod]
        public DataSet GetActivities(string agencyCode,
                                    string remarkType,
                                    string nickname,
                                    DateTime timelimitFrom,
                                    DateTime timelimitTo,
                                    bool pendingOnly,
                                    bool incompleteOnly,
                                    bool includeRemarks,
                                    bool showUnassigned,
                                    string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");
            ComplusReservation objReservationy = new ComplusReservation();
            try
            {
                DataSet ds = objReservationy.GetActivities(agencyCode,
                                     remarkType,
                                     nickname,
                                     timelimitFrom,
                                     timelimitTo,
                                     pendingOnly,
                                     incompleteOnly,
                                     includeRemarks,
                                     showUnassigned);
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                objReservationy.Dispose();
            }
        }
        [WebMethod]
        public BaggageFeeResponse GetBaggageFee(BaggageFeeRequest request, string token, bool bNovat)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");
            BaggageFeeResponse response = new BaggageFeeResponse();
            ComplusFee objBaggageService = new ComplusFee();

            try
            {
                if (string.IsNullOrEmpty(request.AgencyCode))
                {
                    response.Success = false;
                    response.Message = "Agency Code is required";
                }
                else if (request.BookingSegmentId.Equals(Guid.Empty))
                {
                    response.Success = false;
                    response.Message = "Booking Segment Id is required";
                }
                else if (request.Mappings.Count == 0)
                {
                    response.Success = false;
                    response.Message = "Mappings is required";
                }
                else
                {
                    if (string.IsNullOrEmpty(request.LanguageCode))
                    {
                        request.LanguageCode = "EN";
                    }

                    IList<Fee> fee = objBaggageService.GetBaggageFeeOption(request.Mappings.CreateMapping(),
                                                                            request.BookingSegmentId,
                                                                            request.PassengerId,
                                                                            request.AgencyCode,
                                                                            request.LanguageCode,
                                                                            request.MaxUnit,
                                                                            request.Fees, bNovat);
                    if (fee != null && fee.Count > 0)
                    {
                        response.Success = true;
                        response.Message = "SUCCESS";
                        response.BaggageFees = fee.ToBaggageFee();
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "FAILED";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objBaggageService.Dispose();
            }
            return response;
        }
        [WebMethod]
        public string GetActiveBookings(string strClientProfileId, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusClient objComplus = new ComplusClient();
            DataSet ds = null;

            try
            {
                ds = objComplus.GetActiveBookings(strClientProfileId);
                ds.DataSetName = "Bookings";
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            { objComplus.Dispose(); }

            return ds.GetXml();
        }
        [WebMethod]
        public string GetFlownBookings(string strClientProfileId, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusClient objComplus = new ComplusClient();
            DataSet ds = null;

            try
            {
                ds = objComplus.GetFlownBookings(strClientProfileId);
                ds.DataSetName = "Bookings";
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            { objComplus.Dispose(); }

            return ds.GetXml();
        }
        [WebMethod]
        public GetFeeReponse GetFee(GetFeeRequest request, string token, bool bNovat)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");
            GetFeeReponse response = new GetFeeReponse();
            ComplusFee objFeeService = new ComplusFee();

            try
            {
                if (string.IsNullOrEmpty(request.AgencyCode))
                {
                    response.Success = false;
                    response.Message = "Agency Code is required";
                }
                else if (string.IsNullOrEmpty(request.CurrencyRcd))
                {
                    response.Success = false;
                    response.Message = "Currency is required";
                }
                else
                {
                    if (string.IsNullOrEmpty(request.LanguageCode))
                    {
                        request.LanguageCode = "EN";
                    }


                    IList<Fee> fee = objFeeService.GetFee(request.FeeRcd,
                                                        request.CurrencyRcd,
                                                        request.AgencyCode,
                                                        request.BookingClass,
                                                        request.FareBasis,
                                                        request.OriginRcd,
                                                        request.DestinationRcd,
                                                        request.FlightNumber,
                                                        request.DepartureDate,
                                                        request.LanguageCode, bNovat);
                    if (fee != null && fee.Count > 0)
                    {
                        response.Success = true;
                        response.Message = "SUCCESS";
                        response.Fees = fee.ToFeeView();
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "FAILED";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            finally
            {
                objFeeService.Dispose();
            }

            return response;
        }
        [WebMethod]
        public SegmentFeeResponse SpecialServiceFee(SegmentFeeRequest request, string token, bool bNovat)
        {
            return ReadSegmentFee(request, true, token, bNovat);
        }
        [WebMethod]
        public SegmentFeeResponse SegmentFee(SegmentFeeRequest request, string token, bool bNovat)
        {
            return ReadSegmentFee(request, false, token, bNovat);
        }


        [WebMethod]
        public string ClientLogon(string clientNumber, string clientPassword, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusSystem objComplus = new ComplusSystem();
            DataSet ds = null;
            try
            {
                ds = objComplus.ClientLogon(clientNumber, clientPassword);
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            {
                objComplus.Dispose();
            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.GetXml();
            }
            else
            {
                return string.Empty;
            }
        }
        [WebMethod]
        public string VoucherTemplateList(string voucherTemplateId,
                                          string voucherTemplate,
                                          DateTime fromDate,
                                          DateTime toDate,
                                          bool write,
                                          string status,
                                          string language,
                                          string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusSetting objComplus = new ComplusSetting();

            DataSet ds = null;
            DateTime dtStart = DateTime.Now;
            Helper objHelper = new Helper();
            try
            {
                ds = objComplus.VoucherTemplateList(voucherTemplateId,
                                                    voucherTemplate,
                                                    fromDate,
                                                    toDate,
                                                    write,
                                                    status,
                                                    language);
            }
            catch (Exception e)
            {
                objHelper.SaveLog("VoucherTemplateList", dtStart, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);
                throw new Exception(e.Message);
            }
            finally
            { objComplus.Dispose(); }

            return ds.GetXml();
        }

        [WebMethod]
        public bool SaveVoucher(string voucherPaymentXML, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusMiscellaneous objComplusMiscellaneous = null;

            DataSet ds = new DataSet();
            DateTime dtStart = DateTime.Now;
            Helper objHelper = new Helper();
            string strResult = string.Empty;
            bool bSuccess = false;

            try
            {
                //Read xml to dataset
                ds.ReadXml(new StringReader(voucherPaymentXML));
                if (ds.Tables.Count > 0)
                {
                    objComplusMiscellaneous = new ComplusMiscellaneous();
                    bSuccess = objComplusMiscellaneous.SaveVoucher(ds.Tables["Voucher"]);
                }
            }
            catch (Exception e)
            {
                objHelper.SaveLog("SaveVoucher", dtStart, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);
                throw new Exception(e.Message);
            }
            finally
            {
                if (objComplusMiscellaneous != null)
                {
                    objComplusMiscellaneous.Dispose();
                }

            }

            return bSuccess;
        }
        [WebMethod]
        public string VoucherPaymentCreditCard(string voucherPaymentXML, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusPayment objComplusPayment = null;

            DataSet ds = new DataSet();
            DateTime dtStart = DateTime.Now;
            Helper objHelper = new Helper();
            string strResult = string.Empty;

            try
            {
                //Read xml to dataset
                ds.ReadXml(new StringReader(voucherPaymentXML));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables["Payment"].Rows.Count > 0)
                    {
                        objComplusPayment = new ComplusPayment();
                        DataSet dsResult = objComplusPayment.PaymentVoucherCreditCard(ds.Tables["Payment"], null, ds.Tables["Voucher"]);
                        if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                        {
                            strResult = dsResult.GetXml();

                            if (dsResult != null)
                            {
                                dsResult.Dispose();
                            }
                        }

                        dsResult = null;
                    }
                    else
                    {
                        strResult = string.Empty;
                    }
                }
            }
            catch (Exception e)
            {
                objHelper.SaveLog("PaymentVoucherCreditCard", dtStart, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);
                throw new Exception(e.Message);
            }
            finally
            {
                if (objComplusPayment != null)
                {
                    objComplusPayment.Dispose();
                }
            }

            return strResult;
        }
        [WebMethod]
        public string ReadVoucher(Guid voucherId, double voucherNumber, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusMiscellaneous objComplus = new ComplusMiscellaneous();

            DataSet ds = null;
            DateTime dtStart = DateTime.Now;
            Helper objHelper = new Helper();
            try
            {
                string strVoucherId = String.Empty;
                if (Guid.Empty != voucherId)
                {
                    strVoucherId = voucherId.ToString();
                }

                ds = objComplus.ReadVoucher(strVoucherId, ref voucherNumber);
            }
            catch (Exception e)
            {
                objHelper.SaveLog("ReadVoucher", dtStart, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);
                throw new Exception(e.Message);
            }
            finally
            { objComplus.Dispose(); }

            return ds.GetXml();
        }

        [WebMethod]
        public bool VoidVoucher(Guid voucherId, Guid userId, DateTime voidDate, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusMiscellaneous objComplus = new ComplusMiscellaneous();

            DateTime dtStart = DateTime.Now;
            Helper objHelper = new Helper();

            bool bResult = false;
            try
            {
                bResult = objComplus.VoidVoucher(voucherId, userId, voidDate);
            }
            catch (Exception e)
            {
                objHelper.SaveLog("VoidVoucher", dtStart, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);
                throw new Exception(e.Message);
            }
            finally
            { objComplus.Dispose(); }

            return bResult;
        }

        [WebMethod]
        public bool AgencyRegistrationInsert(string token,
                                            string agencyName,
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
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusSetting objComplus = new ComplusSetting();

            DateTime dtStart = DateTime.Now;
            Helper objHelper = new Helper();
            try
            {
                return objComplus.AgencyRegistrationInsert(agencyName,
                                                            legalName,
                                                            agencyType,
                                                            IATA,
                                                            taxId,
                                                            mail,
                                                            fax,
                                                            phone,
                                                            address1,
                                                            address2,
                                                            street,
                                                            state,
                                                            district,
                                                            province,
                                                            city,
                                                            zipCode,
                                                            poBox,
                                                            website,
                                                            contactPerson,
                                                            lastName,
                                                            firstName,
                                                            title,
                                                            userLogon,
                                                            password,
                                                            country,
                                                            currency,
                                                            language,
                                                            comment);
            }
            catch (Exception e)
            {
                objHelper.SaveLog("AgencyRegistrationInsert", dtStart, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);
                throw new Exception(e.Message);
            }
            finally
            { objComplus.Dispose(); }
        }

        [WebMethod]
        public string RemarkAdd(string remarkType,
                                string bookingRemarkId,
                                string bookingId,
                                string clientProfileId,
                                string nickname,
                                string remarkText,
                                string agencyCode,
                                string addedBy,
                                string userId,
                                bool bProtected,
                                bool warning,
                                bool processMessage,
                                bool systemRemark,
                                DateTime timelimit,
                                DateTime timelimitUTC,
                                string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusBooking objComplus = new ComplusBooking();
            string strBookingRemarkId = string.Empty;

            try
            {
                strBookingRemarkId = objComplus.RemarkAdd(remarkType,
                                                        bookingRemarkId,
                                                        bookingId,
                                                        clientProfileId,
                                                        nickname,
                                                        remarkText,
                                                        agencyCode,
                                                        addedBy,
                                                        userId,
                                                        bProtected,
                                                        warning,
                                                        processMessage,
                                                        systemRemark,
                                                        timelimit,
                                                        timelimitUTC);
            }
            catch (Exception e)
            {
                Helper objHelper = new Helper();
                objHelper.SaveLog("RemarkAdd", DateTime.Now, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);

                throw new Exception(e.Message);
            }
            finally
            { objComplus.Dispose(); }

            return strBookingRemarkId;
        }

        [WebMethod]
        public bool RemarkDelete(Guid bookingRemarkId, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusBooking objComplus = new ComplusBooking();
            bool bResult = false;

            try
            {
                bResult = objComplus.RemarkDelete(bookingRemarkId);
            }
            catch (Exception e)
            {
                Helper objHelper = new Helper();
                objHelper.SaveLog("RemarkDelete", DateTime.Now, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);

                throw new Exception(e.Message);
            }
            finally
            { objComplus.Dispose(); }

            return bResult;
        }
        [WebMethod]
        public bool RemarkComplete(Guid bookingRemarkId, Guid userId, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusBooking objComplus = new ComplusBooking();
            bool bResult = false;

            try
            {
                bResult = objComplus.RemarkComplete(bookingRemarkId, userId);
            }
            catch (Exception e)
            {
                Helper objHelper = new Helper();
                objHelper.SaveLog("RemarkComplete", DateTime.Now, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);

                throw new Exception(e.Message);
            }
            finally
            { objComplus.Dispose(); }

            return bResult;
        }

        [WebMethod]
        public string RemarkRead(string remarkId,
                                string bookingId,
                                string bookingReference,
                                double bookingNumber,
                                bool readOnly,
                                string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusBooking objComplus = new ComplusBooking();
            string strResult = string.Empty;

            try
            {
                using (DataSet ds = objComplus.RemarkRead(remarkId, bookingId, bookingReference, bookingNumber, readOnly))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        strResult = ds.GetXml();
                    }
                }
            }
            catch (Exception e)
            {
                Helper objHelper = new Helper();
                objHelper.SaveLog("RemarkRead", DateTime.Now, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);

                throw new Exception(e.Message);
            }
            finally
            { objComplus.Dispose(); }

            return strResult;
        }

        [WebMethod]
        public bool RemarkSave(string strRemarkXml,
                                string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusBooking objComplus = new ComplusBooking();
            bool bResult = false;

            try
            {
                using (DataSet ds = new DataSet())
                {
                    //Read xml to dataset
                    ds.ReadXml(new StringReader(strRemarkXml));

                    bResult = objComplus.RemarkSave(ds.Tables["Remark"]);
                }
            }
            catch (Exception e)
            {
                Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog(e,
                                                                                                                      "Input XML : " + strRemarkXml.Replace("<", "&lt;").Replace(">", "&gt;") + "<br/>");

                throw;
            }
            finally
            { objComplus.Dispose(); }

            return bResult;
        }

        [WebMethod]
        public string GetPassenger(string airline,
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
                                    string language,
                                    string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");
            ComplusReservation objReservationy = new ComplusReservation();

            string strResult = string.Empty;
            try
            {
                using (DataSet ds = objReservationy.GetPassenger(airline,
                                                                flightNumber,
                                                                flightID,
                                                                flightFrom,
                                                                flightTo,
                                                                recordLocator,
                                                                origin,
                                                                destination,
                                                                passengerName,
                                                                seatNumber,
                                                                ticketNumber,
                                                                phoneNumber,
                                                                passengerStatus,
                                                                checkInStatus,
                                                                clientNumber,
                                                                memberNumber,
                                                                clientID,
                                                                passengerId,
                                                                booked,
                                                                listed,
                                                                eTicketOnly,
                                                                includeCancelled,
                                                                openSegments,
                                                                showHistory,
                                                                language))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        strResult = ds.GetXml();
                    }
                }

            }
            catch (Exception e)
            {
                Helper objHelper = new Helper();
                objHelper.SaveLog("GetPassenger", DateTime.Now, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);

                throw new Exception(e.Message);
            }
            finally
            {
                objReservationy.Dispose();
            }

            return strResult;
        }

        [WebMethod]
        public string GetQueueCount(string agency, bool unassigned, string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");
            ComplusReservation objReservationy = new ComplusReservation();

            string strResult = string.Empty;
            try
            {
                using (DataSet ds = objReservationy.GetQueueCount(agency, unassigned))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        strResult = ds.GetXml();
                    }
                }

            }
            catch (Exception e)
            {
                Helper objHelper = new Helper();
                objHelper.SaveLog("GetQueueCount", DateTime.Now, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);

                throw new Exception(e.Message);
            }
            finally
            {
                objReservationy.Dispose();
            }

            return strResult;
        }

        [WebMethod]
        public bool GetSingleFlight(string strFlightId,
                                    string strAirline,
                                    string strFlightNumber,
                                    DateTime dtFlightFrom,
                                    DateTime dtFlightTo,
                                    string strLanguage,
                                    string strOrigin,
                                    string strDestination,
                                    bool bWrite,
                                    bool bEmptyRs,
                                    string strScheduleId,
                                    bool bSingle,
                                    string token)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");

            ComplusFlight objComplus = new ComplusFlight();
            bool bResult = false;

            try
            {
                bResult = objComplus.GetSingleFlight(strFlightId,
                                                     strAirline,
                                                     strFlightNumber,
                                                     dtFlightFrom,
                                                     dtFlightTo,
                                                     strLanguage,
                                                     strOrigin,
                                                     strDestination,
                                                     bWrite,
                                                     bEmptyRs,
                                                     strScheduleId,
                                                     bSingle);
            }
            catch (Exception e)
            {
                Helper objHelper = new Helper();
                objHelper.SaveLog("GetSingleFlight", DateTime.Now, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);

                throw new Exception(e.Message);
            }
            finally
            { objComplus.Dispose(); }

            return bResult;
        }

        [WebMethod]
        public string GetPassengersManifest(string origin_rcd,
            string destination_rcd,
            string airline_rcd,
            string flight_number,
            string departure_date_from,
            string departure_date_to,
            string username,
            string password,
            string token,
            bool bReturnServices = false,
            bool bReturnBagTags = false,
            bool bReturnRemarks = false,
            bool bNotCheckedIn = false,
            bool bCheckedIn = false,
            bool bOffloaded = false,
            bool bNoShow = false,
            bool bInfants = false,
            bool bConfirmed = false,
            bool bWaitlisted = false,
            bool bCancelled = false,
            bool bStandby = false,
            bool bIndividual = false,
            bool bGroup = false,
            bool bTransit = false)
        {
            if (!ValidateToken(token)) throw new Exception("Invalid token.");
            ComplusCheckin objCheckin = new ComplusCheckin();

            string strResult = string.Empty;
            try
            {
                DateTime departureDateFrom = Convert.ToDateTime(departure_date_from);
                DateTime departureDateTo = Convert.ToDateTime(departure_date_to);
                clstikAeroWebService service = new clstikAeroWebService();

                //FlightsResponse response = service.GetFlightPassengerAPI(origin_rcd, destination_rcd, airline_rcd, flight_number, departureDateFrom, departureDateTo, username, password);
                // need directly to DB for get flight
                Classes.FlightsResponse response = getFlight(origin_rcd, destination_rcd, airline_rcd, flight_number, departureDateFrom, departureDateTo);

                //if (response == null) throw new Exception("Invalid passenger data");
                //if (response.Flights == null || response.Flights.Count() == 0) throw new Exception("Invalid flights");

                if (response != null && response.Flights != null && response.Flights.Count() > 0)
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.OmitXmlDeclaration = true;
                    settings.CheckCharacters = true;
                    settings.ConformanceLevel = ConformanceLevel.Auto;
                    settings.Encoding = Encoding.UTF8;
                    settings.Indent = false;

                    using (StringWriter writer = new StringWriter())
                    {
                        using (XmlWriter newDoc = XmlWriter.Create(writer, settings))
                        {
                            newDoc.WriteStartDocument();
                            newDoc.WriteStartElement("root");

                            for (int i = 0; i < response.Flights.Count(); i++)
                            {
                                newDoc.WriteStartElement("Flight");
                                newDoc.WriteElementString("flight_id", response.Flights[i].flight_id);
                                newDoc.WriteElementString("airline_rcd", response.Flights[i].airline_rcd);
                                newDoc.WriteElementString("flight_number", response.Flights[i].flight_number);
                                newDoc.WriteElementString("origin_rcd", response.Flights[i].origin_rcd);
                                newDoc.WriteElementString("destination_rcd", response.Flights[i].destination_rcd);
                                newDoc.WriteElementString("departure_date_from", departureDateFrom.ToString("s"));
                                newDoc.WriteElementString("departure_date_to", departureDateTo.ToString("s"));
                                newDoc.WriteElementString("flight_legs_count", response.Flights[i].flight_legs.Count().ToString());
                                for (int k = 0; k < response.Flights[i].flight_legs.Count(); k++)
                                {
                                    Classes.FlightLegs leg = response.Flights[i].flight_legs[k];
                                    newDoc.WriteStartElement("flight_leg");
                                    newDoc.WriteElementString("flight_id", leg.flight_id);
                                    newDoc.WriteElementString("arrival_airport_rcd", leg.arrival_airport_rcd);
                                    newDoc.WriteElementString("arrival_date", leg.arrival_date.Value.ToString("s"));
                                    newDoc.WriteElementString("utc_arrival_date_time", leg.utc_arrival_date_time.Value.ToString("yyyy-MM-ddThh:mm:sszzz"));
                                    newDoc.WriteElementString("planned_arrival_time", leg.planned_arrival_time);
                                    newDoc.WriteElementString("departure_airport_rcd", leg.departure_airport_rcd);
                                    newDoc.WriteElementString("departure_date", leg.departure_date.Value.ToString("s"));
                                    newDoc.WriteElementString("planned_departure_time", leg.planned_departure_time);
                                    newDoc.WriteElementString("utc_departure_date_time", leg.utc_departure_date_time.Value.ToString("yyyy-MM-ddThh:mm:sszzz"));
                                    newDoc.WriteEndElement();
                                }

                                using (DataSet ds = objCheckin.GetPassengersManifest(response.Flights[i].flight_id,
                                    origin_rcd,
                                    destination_rcd,
                                    bReturnServices,
                                    bReturnBagTags,
                                    bReturnRemarks,
                                    bNotCheckedIn,
                                    bCheckedIn,
                                    bOffloaded,
                                    bNoShow,
                                    bInfants,
                                    bConfirmed,
                                    bWaitlisted,
                                    bCancelled,
                                    bStandby,
                                    bIndividual,
                                    bGroup,
                                    bTransit))
                                {
                                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        XElement root = XElement.Parse(ds.GetXml());

                                        Dictionary<string, string> type = new Dictionary<string, string>() { { "Adult", "ADULT" }, { "Child", "CHD" }, { "Infant", "INF" }, { "Other", "OTHER" } };

                                        //Number of passenger in each passenger type by check-in status
                                        newDoc.WriteStartElement("number_of_leg");

                                        Dictionary<string, string> status = new Dictionary<string, string>() { { "Boarded", "BOARDED" }, { "Checked", "CHECKED" }, { "Flown", "FLOWN" }, { "Noshow", "NOSHOW" }, { "No_check-in_status", "" }, { "Offloaded", "OFFLOADED" } };

                                        foreach (var t in type)
                                        {
                                            newDoc.WriteStartElement(t.Key);
                                            foreach (var s in status)
                                            {
                                                if (t.Key.Equals("Other"))
                                                {
                                                    if (s.Key.Equals("No_check-in_status"))
                                                        newDoc.WriteElementString(s.Key,
                                                            (root.XPathSelectElements(string.Format("PassengersManifest[passenger_type_rcd !='{0}' and passenger_type_rcd !='{1}' and passenger_type_rcd !='{2}']", type["Adult"], type["Child"], type["Infant"])).Count() -
                                                            root.XPathSelectElements(string.Format("PassengersManifest[passenger_type_rcd !='{0}' and passenger_type_rcd !='{1}' and passenger_type_rcd !='{2}']/passenger_check_in_status_rcd", type["Adult"], type["Child"], type["Infant"])).Count()).ToString());
                                                    else
                                                        newDoc.WriteElementString(s.Key, root.XPathSelectElements(string.Format("PassengersManifest[passenger_type_rcd !='{0}' and passenger_type_rcd !='{1}' and passenger_type_rcd !='{2}' and passenger_check_in_status_rcd='{3}']", type["Adult"], type["Child"], type["Infant"], s.Value)).Count().ToString());

                                                }
                                                else
                                                {
                                                    if (s.Key.Equals("No_check-in_status"))
                                                        newDoc.WriteElementString(s.Key,
                                                            (root.XPathSelectElements(string.Format("PassengersManifest[passenger_type_rcd='{0}']", t.Value)).Count() -
                                                            root.XPathSelectElements(string.Format("PassengersManifest[passenger_type_rcd='{0}']/passenger_check_in_status_rcd", t.Value)).Count()).ToString());

                                                    else
                                                        newDoc.WriteElementString(s.Key, root.XPathSelectElements(string.Format("PassengersManifest[passenger_type_rcd='{0}' and passenger_check_in_status_rcd='{1}']", t.Value, s.Value)).Count().ToString());
                                                }
                                            }
                                            newDoc.WriteEndElement();
                                        }
                                        newDoc.WriteEndElement();
                                        ///////////////////////////////////////////////////////////////////////////

                                        //Number of passenger in each passenger type by boarding class
                                        newDoc.WriteStartElement("boarding_class");

                                        Dictionary<string, string> boarding = new Dictionary<string, string>() { { "First_class", "F" }, { "Business_class", "B" }, { "Economy_class", "Y" } };

                                        foreach (var t in type)
                                        {
                                            newDoc.WriteStartElement(t.Key);
                                            foreach (var b in boarding)
                                            {
                                                if (t.Key.Equals("Other"))
                                                    newDoc.WriteElementString(b.Key, root.XPathSelectElements(string.Format("PassengersManifest[passenger_type_rcd !='{0}' and passenger_type_rcd !='{1}' and passenger_type_rcd !='{2}' and boarding_class_rcd='{3}']", type["Adult"], type["Child"], type["Infant"], b.Value)).Count().ToString());
                                                else
                                                    newDoc.WriteElementString(b.Key, root.XPathSelectElements(string.Format("PassengersManifest[passenger_type_rcd='{0}' and boarding_class_rcd='{1}']", t.Value, b.Value)).Count().ToString());
                                            }
                                            newDoc.WriteEndElement();
                                        }
                                        newDoc.WriteEndElement();
                                        //////////////////////////////////////////////////////////////////////////

                                        foreach (var element in root.Elements("PassengersManifest"))
                                        {
                                            newDoc.WriteStartElement("Passenger");
                                            foreach (var el in element.Elements())
                                            {
                                                newDoc.WriteElementString(el.Name.LocalName, el.Value);
                                            }
                                            newDoc.WriteEndElement();
                                        }
                                    }
                                }
                                newDoc.WriteEndElement();
                            }

                            newDoc.WriteEndElement();
                            newDoc.WriteEndDocument();
                            newDoc.Flush();
                            strResult = writer.ToString();
                        }
                    }
                }
                else
                {
                    strResult = "Flight not found.";
                    return strResult;
                }
            }
            catch (Exception e)
            {
                Helper objHelper = new Helper();
                objHelper.SaveLog("GetPassengersManifest", DateTime.Now, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);
              //  throw new Exception(e.Message);
            }
            finally
            {
                objCheckin.Dispose();
            }

            return strResult;
        }

        #region Helper


        private string GroupXML(string strXml, string strPath, string currencyCode)
        {
            XPathDocument xmlDoc = new XPathDocument(new StringReader(strXml));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            StringWriter objWriter = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(objWriter);
            Helper objLi = new Helper();

            xmlWriter.WriteStartElement("Booking");
            {
                foreach (XPathNavigator n in nv.Select(strPath))
                {
                    xmlWriter.WriteStartElement("AvailabilityFlight");
                    {
                        xmlWriter.WriteStartElement("airline_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "airline_rcd", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("flight_number");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "flight_number", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("booking_class_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "booking_class_rcd", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("boarding_class_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "boarding_class_rcd", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("flight_id");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "flight_id", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("origin_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "origin_rcd", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("destination_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "destination_rcd", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("origin_name");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "origin_name", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("destination_name");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "destination_name", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("departure_date");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "departure_date", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("planned_departure_time");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "planned_departure_time", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("planned_arrival_time");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "planned_arrival_time", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("fare_id");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "fare_id", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("fare_code");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "fare_code", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_flight_id");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_flight_id", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_booking_class_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_booking_class_rcd", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_boarding_class_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_boarding_class_rcd", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_airport_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_airport_rcd", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_departure_date");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_departure_date", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_planned_departure_time");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_planned_departure_time", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_planned_arrival_time");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_planned_arrival_time", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_fare_id");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_fare_id", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_name");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_name", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_waitlist_open_flag");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_waitlist_open_flag", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("nesting_string");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "nesting_string", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("full_flight_flag");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "full_flight_flag", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("class_open_flag");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "class_open_flag", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("close_web_sales");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "close_web_sales", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("currency_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "currency_rcd", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("total_adult_fare");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "total_adult_fare", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("fare_column");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "fare_column", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("flight_comment");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "flight_comment", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("filter_logic_flag");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "filter_logic_flag", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("restriction_text");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "restriction_text", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("flight_duration");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "flight_duration", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("class_capacity");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "class_capacity", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("waitlist_open_flag");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "waitlist_open_flag", Helper.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("currency_rcd");
                        xmlWriter.WriteValue(currencyCode);
                        xmlWriter.WriteEndElement();

                    }
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();

            xmlDoc = null;
            nv = null;

            xmlWriter.Flush();
            xmlWriter.Close();
            xmlWriter = null;

            objLi = null;
            return objWriter.ToString();
        }

        public bool ValidateToken(string token)
        {
            string baseToken = System.Configuration.ConfigurationManager.AppSettings["AuthenUser"].ToString() +
               System.Configuration.ConfigurationManager.AppSettings["AuthenPassword"].ToString();
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(baseToken);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2")); 
            }

            if (sb.ToString() == token)
                return true;
            else
                return false;
        }
        private SegmentFeeResponse ReadSegmentFee(SegmentFeeRequest request, bool specialService, string token, bool bNovat)
        {
            SegmentFeeResponse response = new SegmentFeeResponse();
            ComplusFee objFeeService = new ComplusFee();

            try
            {
                if (!ValidateToken(token)) throw new Exception("Invalid token.");
                if (string.IsNullOrEmpty(request.AgencyCode))
                {
                    response.Success = false;
                    response.Message = "Agency Code is required";
                }
                else if (string.IsNullOrEmpty(request.CurrencyCode))
                {
                    response.Success = false;
                    response.Message = "Currency code is required";
                }
                else if (request.ServiceCode == null || request.ServiceCode.Length == 0)
                {
                    response.Success = false;
                    response.Message = "Special Service code is required";
                }
                else if (request.SegmentService == null || request.SegmentService.Count == 0)
                {
                    response.Success = false;
                    response.Message = "Segment information is required";
                }
                else
                {
                    if (string.IsNullOrEmpty(request.LanguageCode))
                    {
                        request.LanguageCode = "EN";
                    }

                    //Get Service from cache.
                    Services sv = GetSpecialServices(request.LanguageCode, token);
                    if (sv != null && sv.Count > 0 && request.ServiceCode != null && request.ServiceCode.Length > 0)
                    {
                        Services service = new Services();
                        Service s;
                        for (int i = 0; i < request.ServiceCode.Length; i++)
                        {
                            for (int j = 0; j < sv.Count; j++)
                            {
                                if (request.ServiceCode[i].Equals(sv[j].special_service_rcd))
                                {
                                    s = new Service();

                                    s.special_service_rcd = sv[j].special_service_rcd;
                                    s.display_name = sv[j].display_name;
                                    s.service_on_request_flag = sv[j].service_on_request_flag;
                                    s.cut_off_time = sv[j].cut_off_time;
                                    service.Add(s);
                                }
                            }
                        }

                        List<ServiceFee> fee = objFeeService.GetSegmentFee(request.AgencyCode,
                                                                           request.CurrencyCode,
                                                                           request.LanguageCode,
                                                                           request.NumberOfPassenger,
                                                                           request.NumberOfInfant,
                                                                           service,
                                                                           request.SegmentService,
                                                                           specialService, bNovat);
                        if (fee != null && fee.Count > 0)
                        {
                            response.Success = true;
                            response.Message = "SUCCESS";
                            response.ServiceFee = fee;
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "FAILED";
                        }
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Can't get special service definition";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            finally
            {
                objFeeService.Dispose();
            }

            return response;
        }

        private Classes.FlightsResponse getFlight(string origin_rcd,
                                                    string destination_rcd,
                                                    string airline_rcd,
                                                    string flight_number,
                                                    DateTime departure_date_from,
                                                    DateTime departure_date_to)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TikAeroConnectionString"].ConnectionString;

            Classes.FlightsRequest resq = new Classes.FlightsRequest();
            Classes.FlightsResponse response = new Classes.FlightsResponse();
            resq.origin_rcd = origin_rcd;
            resq.destination_rcd = destination_rcd;
            resq.airline_rcd = airline_rcd;
            resq.flight_number = flight_number;
            resq.departure_date_from = departure_date_from;
            resq.departure_date_to = departure_date_to;

            Classes.Flight objFlight = new Classes.Flight(connectionString);
            response = objFlight.get_flights(resq);

            return response;
        }
        #endregion
    }
}
