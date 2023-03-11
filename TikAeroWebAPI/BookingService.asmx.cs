using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Text;
using tikSystem.Web.Library;

using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.ServiceModel.Description;
using System.IdentityModel.Tokens;
using System.IdentityModel.Selectors;
using TikAeroWebAPI.Classes;
using TikAeroWebAPI.Classes.PassengerMessage;
using TikAeroWebAPI.Classes.ActivityMessage;
using TikAeroWebAPI.Classes.RemarkMessage;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Text.RegularExpressions;

namespace TikAeroWebAPI
{
    public class MyX509Validator : X509CertificateValidator
    {

        public override void Validate(X509Certificate2 certificate)
        {

            // validate argument

            if (certificate == null)

                throw new ArgumentNullException("certificate");



            // check if the name of the certifcate matches

            //if (certificate.SubjectName.Name != "CN=localhost")

            //    throw new SecurityTokenValidationException("Certificated was not issued by thrusted issuer");
        }
    }

    /// <summary>
    /// Summary description for BookingService
    /// </summary>
    //[WebService(Namespace = "http://mercator.asia/BookingService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class BookingService : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public string GetCoporateSessionProfile(string agency)
        {
            string strResult = string.Empty;
            string clientProfileId = string.Empty;
            string lastname = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd;
            DataSet dsClientList = null;
            DataSet dsAgency = null;

            ServiceClient objClient = new ServiceClient();

            try
            {
                Agents objAgents = (Agents)Session["Agents"];
                if (objAgents == null)
                {
                    strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                    dtEnd = DateTime.Now;
                    return strResult;
                }
                else
                {
                    // prepare data

                    // get client profiles
                    dsAgency = objClient.GetCorporateAgencyClients(agency);
                    clientProfileId = dsAgency.Tables[0].Rows[0]["client_profile_id"].ToString();

                    if (dsAgency.Tables[0].Rows[0]["client_type_rcd"].ToString() != null && dsAgency.Tables[0].Rows[0]["client_type_rcd"].ToString().ToUpper() == "COR")
                    {
                        // get client list call web service clientid  lasrname
                        dsClientList = objClient.GetCoporateSessionProfile(clientProfileId, string.Empty);

                        string xmlAgent = dsAgency.GetXml();

                        strResult = xmlAgent.Replace("Clients", "ClientProfile").Replace("NewDataSet", "Corporate");
                        strResult = @"<CorporateClient>" + strResult;
                        string xmlClientList = dsClientList.GetXml();
                        xmlClientList = xmlClientList.Replace("Corporate", "Employee").Replace("NewDataSet", "Employees");

                        strResult += xmlClientList;

                        strResult += @"</CorporateClient>";

                        // add to client session
                        XPathDocument xmlDoc = new XPathDocument(new StringReader(xmlAgent));
                        XPathNavigator nv = xmlDoc.CreateNavigator();
                        foreach (XPathNavigator n in nv.Select("NewDataSet/Clients"))
                        {
                            Client client = new Client();
                            client.client_number = long.Parse(XmlHelper.XpathValueNullToEmpty(n, "client_number"));
                            client.client_profile_id = new Guid(XmlHelper.XpathValueNullToEmpty(n, "client_profile_id"));
                            client.title_rcd = XmlHelper.XpathValueNullToEmpty(n, "title_rcd");
                            client.lastname = XmlHelper.XpathValueNullToEmpty(n, "lastname");
                            client.firstname = XmlHelper.XpathValueNullToEmpty(n, "firstname");
                            client.contact_name = XmlHelper.XpathValueNullToEmpty(n, "contact_name");
                            client.contact_email = XmlHelper.XpathValueNullToEmpty(n, "contact_email");
                            client.address_line1 = XmlHelper.XpathValueNullToEmpty(n, "address_line1");
                            client.address_line2 = XmlHelper.XpathValueNullToEmpty(n, "address_line2");
                            client.city = XmlHelper.XpathValueNullToEmpty(n, "city");
                            client.zip_code = XmlHelper.XpathValueNullToEmpty(n, "zip_code");
                            client.country_rcd = XmlHelper.XpathValueNullToEmpty(n, "country_rcd");
                            client.phone_mobile = XmlHelper.XpathValueNullToEmpty(n, "phone_mobile");
                            client.phone_home = XmlHelper.XpathValueNullToEmpty(n, "phone_home");
                            client.phone_business = XmlHelper.XpathValueNullToEmpty(n, "phone_business");
                            client.state = XmlHelper.XpathValueNullToEmpty(n, "state");

                            //client.passenger_type_rcd = XmlHelper.XpathValueNullToEmpty(n, "passenger_type_rcd");
                            //client.date_of_birth = Convert.ToDateTime(XmlHelper.XpathValueNullToEmpty(n, "date_of_birth"));
                            //client.member_level_rcd = XmlHelper.XpathValueNullToEmpty(n, "member_level_rcd");
                            client.client_type_rcd = "COR";
                            //client.member_number = XmlHelper.XpathValueNullToEmpty(n, "member_number");
                            //client.employee_number = XmlHelper.XpathValueNullToEmpty(n, "employee_number");

                            Session["CorClient"] = client;
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("109", "Agency is not corporate type");
                        dtEnd = DateTime.Now;
                        return strResult;
                    }
                }
            }
            catch (Exception ex)
            {
                strResult = Utils.ErrorXml("103", "General Error");

                dtEnd = DateTime.Now;
                Utils.SaveLog("GetCoporateSessionProfile", dtStart, dtEnd, ex.Message, "");
            }
            finally
            {
                objClient = null;

                //Save Process Log
                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                    Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
                {
                    Utils.SaveProcessLog("GetCoporateSessionProfile", dtStart, DateTime.Now, "");
                }
            }

            return strResult;
        }
        

        [WebMethod(EnableSession = true)]
        public string ServiceInitialize(string strAgencyCode, string strUserName, string strPassword, string strLanguageCode)
        {
            ServiceClient obj = new ServiceClient();
            string strXml = string.Empty;
            string strErrorXml = string.Empty;
            string ServerName = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd;
            Agents objAgents = (Agents)Session["Agents"];
            string InitializeResult = "";

            //if (System.Configuration.ConfigurationManager.AppSettings["ServerName"] != null)
            //{
            //    ServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"];
            //}

            ServerName = Environment.MachineName;


            if (objAgents != null && objAgents.Count > 0)
            { BookingInitialize(); }

            if (strAgencyCode.Length > 0 && strUserName.Length > 0 && strPassword.Length > 0)
            {

                Session["LanguageCode"] = strLanguageCode;
                if (objAgents != null &&
                    objAgents[0].agency_code != null &&
                    objAgents[0].user_logon != null &&
                    objAgents[0].agency_code.ToUpper().Equals(strAgencyCode.ToUpper()) &&
                    objAgents[0].user_logon.Equals(strUserName.ToUpper()))
                {
                    strErrorXml = Utils.ErrorXmlInit("000", "Success Request Transaction", ServerName);
                    InitializeResult = "000";
                }
                else
                {
                    strXml = obj.ServiceAuthentication(strAgencyCode, strUserName, strPassword, strAgencyCode);

                    if (strXml.Length > 0)
                    {
                        XPathDocument xmlDoc = new XPathDocument(new StringReader(strXml));
                        XPathNavigator nv = xmlDoc.CreateNavigator();
                        Library objLi = new Library();
                        if (objAgents == null)
                        {
                            objAgents = new Agents();
                        }
                        objAgents.Clear();

                        objAgents.AddAgent(nv.Select("Authen/AgencySessionProfile/Agency"));
                        if (objAgents.Count > 0)
                        {
                            if (objAgents[0].api_flag == 1)
                            {
                                if (nv.Select("Authen/Users/User").Count > 0)
                                {
                                    //Get User information
                                    foreach (XPathNavigator n in nv.Select("Authen/Users/User"))
                                    {
                                        Session["UserId"] = objLi.getXPathNodevalue(n, "user_account_id", Library.xmlReturnType.value);

                                        //   objAgents[0].issue_ticket_flag = byte.Parse(objLi.getXPathNodevalue(n, "issue_ticket_flag", Library.xmlReturnType.value));

                                    }

                                    objAgents[0].b2b_credit_card_payment_flag = 1;
                                    objAgents[0].b2b_voucher_payment_flag = 1;

                                    Session["Agents"] = objAgents;
                                    objLi = null;
                                    nv = null;
                                    xmlDoc = null;

                                    // string xx = XmlHelper.Serialize(objAgents,false);
                                    strErrorXml = Utils.ErrorXmlInit("000", "Success Request Transaction", ServerName);
                                    InitializeResult = "000";
                                }
                                else
                                {
                                    strErrorXml = Utils.ErrorXmlInit("100", "Fail To Initialize API Service", ServerName);
                                    InitializeResult = "100";

                                    dtEnd = DateTime.Now;
                                    Utils.SaveLog("ServiceInitialize", dtStart, dtEnd, strErrorXml, strXml);
                                }
                            }
                            else
                            {
                                strErrorXml = Utils.ErrorXmlInit("104", "This Agency is not allowed to create booking", ServerName);
                                InitializeResult = "104";

                                dtEnd = DateTime.Now;
                               // Utils.SaveLog("ServiceInitialize", dtStart, dtEnd, strErrorXml, string.Empty);
                            }
                        }
                        else
                        {
                            strErrorXml = Utils.ErrorXmlInit("105", "Agency information not found", ServerName);
                            InitializeResult = "105";

                            dtEnd = DateTime.Now;
                           // Utils.SaveLog("ServiceInitialize", dtStart, dtEnd, strErrorXml, strXml);
                        }
                    }
                    else
                    {
                        strErrorXml = Utils.ErrorXmlInit("100", "Fail To Initialize API Service", ServerName);
                        InitializeResult = "100";

                        dtEnd = DateTime.Now;
                       // Utils.SaveLog("ServiceInitialize", dtStart, dtEnd, strErrorXml, string.Empty);
                    }
                }
            }
            else
            {
                strErrorXml = Utils.ErrorXmlInit("101", "Login Information Required", ServerName);
                InitializeResult = "101";

                dtEnd = DateTime.Now;
               // Utils.SaveLog("ServiceInitialize", dtStart, dtEnd, strErrorXml, string.Empty);
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                string strParam = string.Empty;
                strParam = "strAgencyCode    : " + strAgencyCode + Environment.NewLine +
                            "strUserName      : " + strUserName + Environment.NewLine +
                            "strPassword      : " + strPassword + Environment.NewLine;

                Utils.SaveProcessLog("ServiceInitialize", dtStart, DateTime.Now, strParam + "\n" + "InitializeResult: " + InitializeResult);
            }

            return strErrorXml;
        }

        [WebMethod(EnableSession = true)]
        public string FlightAvailabilityStaff(string strAgencyCode, string strPassword,
                                        string strOrigin, string strDestination,
                                        string strDepartFrom, string strDepartTo,
                                        string strReturnFrom, string strReturnTo,
                                        short iAdult, short iChild,
                                        short iInfant, short iOther,
                                        string strBookingClass, string strBoardingClass,
                                        string strPromoCode, string strLanguageCode, string strOtherPassengerType, string strCurrency)
        {
            return GetAvailability(strAgencyCode,
                                   strPassword,
                                   strOrigin,
                                   strDestination,
                                   strDepartFrom,
                                   strDepartTo,
                                   strReturnFrom,
                                   strReturnTo,
                                   iAdult,
                                   iChild,
                                   iInfant,
                                   iOther,
                                   strBookingClass,
                                   strBoardingClass,
                                   strPromoCode,
                                   "SF",
                                   strLanguageCode, strOtherPassengerType, strCurrency);
        }



        [WebMethod(EnableSession = true)]
        public string FlightAvailability(string strAgencyCode, string strPassword,
                                        string strOrigin, string strDestination,
                                        string strDepartFrom, string strDepartTo,
                                        string strReturnFrom, string strReturnTo,
                                        short iAdult, short iChild,
                                        short iInfant, short iOther,
                                        string strBookingClass, string strBoardingClass,
                                        string strPromoCode, string strLanguageCode, string strOtherPassengerType, string strCurrency)
        {
            return GetAvailability(strAgencyCode,
                                   strPassword,
                                   strOrigin,
                                   strDestination,
                                   strDepartFrom,
                                   strDepartTo,
                                   strReturnFrom,
                                   strReturnTo,
                                   iAdult,
                                   iChild,
                                   iInfant,
                                   iOther,
                                   strBookingClass,
                                   strBoardingClass,
                                   strPromoCode,
                                   "AF",
                                   strLanguageCode, strOtherPassengerType, strCurrency);
        }


        [WebMethod(EnableSession = true)]
        public string FlightLowestFareAvailability(string strAgencyCode, string strPassword,
                                                    string strOrigin, string strDestination,
                                                    string strDepartFrom, string strDepartTo,
                                                    string strReturnFrom, string strReturnTo,
                                                    short iAdult, short iChild,
                                                    short iInfant, short iOther,
                                                    string strBookingClass, string strBoardingClass,
                                                    string strPromoCode, string strLanguageCode, string strOtherPassengerType, string strCurrency)
        {
            return GetAvailability(strAgencyCode,
                                   strPassword,
                                   strOrigin,
                                   strDestination,
                                   strDepartFrom,
                                   strDepartTo,
                                   strReturnFrom,
                                   strReturnTo,
                                   iAdult,
                                   iChild,
                                   iInfant,
                                   iOther,
                                   strBookingClass,
                                   strBoardingClass,
                                   strPromoCode,
                                   "LF",
                                   strLanguageCode, strOtherPassengerType, strCurrency);
        }

        [WebMethod(EnableSession = true)]
        public string FlightGroupFareAvailability(string strAgencyCode, string strPassword,
                                                    string strOrigin, string strDestination,
                                                    string strDepartFrom, string strDepartTo,
                                                    string strReturnFrom, string strReturnTo,
                                                    short iAdult, short iChild,
                                                    short iInfant, short iOther,
                                                    string strBookingClass, string strBoardingClass,
                                                    string strPromoCode, string strLanguageCode, string strOtherPassengerType, string strCurrency)
        {
            return GetAvailability(strAgencyCode,
                                   strPassword,
                                   strOrigin,
                                   strDestination,
                                   strDepartFrom,
                                   strDepartTo,
                                   strReturnFrom,
                                   strReturnTo,
                                   iAdult,
                                   iChild,
                                   iInfant,
                                   iOther,
                                   strBookingClass,
                                   strBoardingClass,
                                   strPromoCode,
                                   "GF",
                                   strLanguageCode, strOtherPassengerType, strCurrency);
        }

        [WebMethod(EnableSession = true)]
        public string FlightRedemptionAvailability(string strAgencyCode, string strPassword,
                                            string strOrigin, string strDestination,
                                            string strDepartFrom, string strDepartTo,
                                            string strReturnFrom, string strReturnTo,
                                            short iAdult, short iChild,
                                            short iInfant, short iOther,
                                            string strBookingClass, string strBoardingClass,
                                            string strPromoCode, string strLanguageCode, string strOtherPassengerType, string strCurrency)
        {
            return GetAvailability(strAgencyCode,
                                   strPassword,
                                   strOrigin,
                                   strDestination,
                                   strDepartFrom,
                                   strDepartTo,
                                   strReturnFrom,
                                   strReturnTo,
                                   iAdult,
                                   iChild,
                                   iInfant,
                                   iOther,
                                   strBookingClass,
                                   strBoardingClass,
                                   strPromoCode,
                                   "PF",
                                   strLanguageCode, strOtherPassengerType, strCurrency);
        }

        [WebMethod(EnableSession = true)]
        public string FlightAdd(string strXml)
        {
            string strResult = string.Empty;
            string currency_rcd = string.Empty;
            XElement elements = null;
            //bool bError = false;
            string FlightAddError = "";

            DateTime dtStart = DateTime.Now;
            DateTime dtEnd;
            string bookingId = string.Empty;

            if (string.IsNullOrEmpty(strXml))
            {
                strResult = Utils.ErrorXml("300", "Flight XML Not Found");
                FlightAddError = "300";
                dtEnd = DateTime.Now;
               // Utils.SaveLog("FlightAdd", dtStart, dtEnd, strResult, string.Empty);
            }
            else
            {
                elements = XElement.Parse(strXml);
                FlightSegment segment = null;
                foreach (var ele in elements.Elements("FlightSegment"))
                {
                    ele.Elements().Where(e => string.IsNullOrEmpty(e.Value)).Remove();
                    segment = new FlightSegment();
                    segment = XmlHelper.Deserialize(ele.ToString(), typeof(FlightSegment)) as FlightSegment;
                }

                if (segment == null)
                {
                    strResult = Utils.ErrorXml("341", "Flight segment Not Found");
                    FlightAddError = "341";
                    dtEnd = DateTime.Now;
                   // Utils.SaveLog("FlightAdd", dtStart, dtEnd, strResult, string.Empty);
                   // return strResult;
                }
                else
                {
                    //Clear previous booking session.
                    BookingInitialize();

                    MAVariable objUv = new MAVariable();

                    XPathDocument inputXml = new XPathDocument(new StringReader(strXml));
                    XPathNavigator nv = inputXml.CreateNavigator();

                    Agents objAgents = (Agents)Session["Agents"];

                    if (objAgents != null && objAgents.Count > 0)
                    {
                        Flights objFlights = new Flights();
                        string strLanguageCode = string.Empty;
                        if (Session["LanguageCode"] != null)
                        {
                            strLanguageCode = Session["LanguageCode"].ToString();
                        }
                        else
                        {
                            strLanguageCode = "EN";
                        }
                        try
                        {

                            Guid gFlightConnectionId = Guid.Empty;
                            string strTransitFlightId = string.Empty;
                            bool noVat = false;
                            if (nv.Select("Booking/Header").Count == 1)
                            {
                                foreach (XPathNavigator n in nv.Select("Booking/Header"))
                                {
                                    objUv.Adult = XmlHelper.XpathValueNullToInt16(n, "adult");
                                    objUv.Child = XmlHelper.XpathValueNullToInt16(n, "child");
                                    objUv.Infant = XmlHelper.XpathValueNullToInt16(n, "infant");
                                    objUv.Other = XmlHelper.XpathValueNullToInt16(n, "other");
                                    objUv.OtherPassengerType = XmlHelper.XpathValueNullToEmpty(n, "other_passenger_type");

                                    if (XmlHelper.XpathValueNullToEmpty(n, "currency_rcd") != string.Empty)
                                        currency_rcd = XmlHelper.XpathValueNullToEmpty(n, "currency_rcd").ToUpper();

                                }
                            }

                            foreach (XPathNavigator n in nv.Select("Booking/FlightSegment"))
                            {
                                Flight f = new Flight();
                                f.flight_id = XmlHelper.XpathValueNullToGUID(n, "flight_id");
                                f.fare_id = XmlHelper.XpathValueNullToGUID(n, "fare_id");

                                if(XmlHelper.XpathValueNullToEmpty(n, "origin_rcd") != string.Empty)
                                    f.origin_rcd = XmlHelper.XpathValueNullToEmpty(n, "origin_rcd").ToUpper();

                                strTransitFlightId = XmlHelper.XpathValueNullToEmpty(n, "transit_flight_id");
                                f.transit_points = XmlHelper.XpathValueNullToEmpty(n, "transit_points");
                                f.transit_points_name = XmlHelper.XpathValueNullToEmpty(n, "transit_points_name");
                                f.boarding_class_rcd = XmlHelper.XpathValueNullToEmpty(n, "boarding_class_rcd");
                                f.booking_class_rcd = XmlHelper.XpathValueNullToEmpty(n, "booking_class_rcd");
                                f.eticket_flag = 1;

                                if (strTransitFlightId != string.Empty)
                                {
                                    gFlightConnectionId = Guid.NewGuid();

                                    if (XmlHelper.XpathValueNullToEmpty(n, "origin_rcd") != string.Empty)
                                        f.od_origin_rcd = XmlHelper.XpathValueNullToEmpty(n, "origin_rcd").ToUpper();

                                    if (XmlHelper.XpathValueNullToEmpty(n, "destination_rcd") != string.Empty)
                                        f.od_destination_rcd = XmlHelper.XpathValueNullToEmpty(n, "destination_rcd").ToUpper();

                                    f.flight_connection_id = gFlightConnectionId;
                                    f.destination_rcd = XmlHelper.XpathValueNullToEmpty(n, "transit_airport_rcd");
                                }
                                else
                                {
                                    if (XmlHelper.XpathValueNullToEmpty(n, "destination_rcd") != string.Empty)
                                        f.destination_rcd = XmlHelper.XpathValueNullToEmpty(n, "destination_rcd").ToUpper();
                                }


                                objFlights.Add(f);

                                f = null;
                                //Transit flight information
                                if (strTransitFlightId != string.Empty)
                                {
                                    Flight transitFlight = new Flight();

                                    if(XmlHelper.XpathValueNullToEmpty(n, "destination_rcd") != string.Empty)
                                        transitFlight.destination_rcd = XmlHelper.XpathValueNullToEmpty(n, "destination_rcd").ToUpper();

                                    transitFlight.flight_id = new Guid(strTransitFlightId);
                                    transitFlight.origin_rcd = XmlHelper.XpathValueNullToEmpty(n, "transit_airport_rcd");
                                    transitFlight.fare_id = XmlHelper.XpathValueNullToGUID(n, "fare_id");
                                    transitFlight.eticket_flag = 1;

                                    if (XmlHelper.XpathValueNullToEmpty(n, "origin_rcd") != string.Empty)
                                        transitFlight.od_origin_rcd = XmlHelper.XpathValueNullToEmpty(n, "origin_rcd").ToUpper();

                                    if (XmlHelper.XpathValueNullToEmpty(n, "destination_rcd") != string.Empty)
                                        transitFlight.od_destination_rcd = XmlHelper.XpathValueNullToEmpty(n, "destination_rcd").ToUpper();

                                    transitFlight.flight_connection_id = gFlightConnectionId;
                                    transitFlight.boarding_class_rcd = XmlHelper.XpathValueNullToEmpty(n, "transit_boarding_class_rcd");
                                    transitFlight.booking_class_rcd = XmlHelper.XpathValueNullToEmpty(n, "transit_booking_class_rcd");
                                    objFlights.Add(transitFlight);
                                    transitFlight = null;
                                }
                            }

                            // use currency from agent  if header is empty
                            if (string.IsNullOrEmpty(currency_rcd))
                            {
                                currency_rcd = objAgents[0].currency_rcd;
                            }

                            if (objFlights.Count > 0)
                            {
                                string xmlResult = objFlights.AddFlight(objAgents[0].agency_code,
                                                                        currency_rcd,
                                                                        bookingId,
                                                                        objUv.Adult,
                                                                        objUv.Child,
                                                                        objUv.Infant,
                                                                        objUv.Other,
                                                                        objUv.OtherPassengerType,
                                                                        Session["UserId"].ToString(),
                                                                        string.Empty,
                                                                        strLanguageCode,
                                                                        noVat);

                                if (String.IsNullOrEmpty(xmlResult) == false)
                                {
                                    XPathDocument xmlCr = new XPathDocument(new StringReader(xmlResult));
                                    XPathNavigator nvCr = xmlCr.CreateNavigator();
                                    if (nvCr.Select("NewDataSet/FlightAdd").Count > 0)
                                    {
                                        foreach (XPathNavigator nCr in nvCr.Select("NewDataSet/FlightAdd"))
                                        {
                                            strResult = InternalPaymentErrorMapping(nCr);
                                            dtEnd = DateTime.Now;
                                            Utils.SaveLog("FlightAdd", dtStart, dtEnd, strResult, strXml);
                                        }

                                    }
                                    else
                                    {
                                        BookingHeader bookingHeader = new BookingHeader();
                                        Itinerary itinerary = new Itinerary();
                                        Passengers passengers = new Passengers();
                                        Quotes quotes = new Quotes();
                                        Fees fees = new Fees();
                                        Mappings mappings = new Mappings();
                                        Services services = new Services();
                                        Remarks remarks = new Remarks();
                                        Payments payments = new Payments();
                                        Taxes taxes = new Taxes();

                                        Library objLi = new Library();

                                        objLi.FillBooking(xmlResult,
                                                           ref bookingHeader,
                                                           ref passengers,
                                                           ref itinerary,
                                                           ref mappings,
                                                           ref payments,
                                                           ref remarks,
                                                           ref taxes,
                                                           ref quotes,
                                                           ref fees,
                                                           ref services);


                                        //start TA-2462
                                        //string Airline = "";
                                        //if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["Airline"]))
                                        //{
                                        //    Airline = System.Configuration.ConfigurationManager.AppSettings["Airline"].ToString();
                                        //}

                                        bool ApplyFareDiscount = false;
                                        if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.bFareDiscount) == true)
                                        {
                                            ApplyFareDiscount = true;
                                        }

                                        if (ApplyFareDiscount)
                                        {
                                            try
                                            {
                                                Guid fare_id = Guid.Empty;
                                                ArrayList AlFareIdDiscount = new ArrayList();
                                                var fare_discount_percentage = string.Empty;
                                                var isVaid = true;
                                                var iResult = 0;
                                                XPathDocument xmlReq = new XPathDocument(new StringReader(strXml));
                                                XPathNavigator nvReq = xmlReq.CreateNavigator();

                                                //verify fare_discount_percentage
                                                foreach (XPathNavigator nReq in nvReq.Select("Booking/FlightSegment"))
                                                {
                                                    fare_discount_percentage = XmlHelper.XpathValueNullToEmpty(nReq, "fare_discount_percentage");

                                                    if (!string.IsNullOrEmpty(fare_discount_percentage))
                                                    {
                                                        if (fare_discount_percentage.Length > 0 && !int.TryParse(fare_discount_percentage, out iResult))
                                                        {
                                                            isVaid = false;
                                                        }
                                                        else if (int.Parse(fare_discount_percentage) < 0 || int.Parse(fare_discount_percentage) > 100)
                                                        {
                                                            isVaid = false;
                                                        }
                                                    }
                                                }

                                                if (isVaid)
                                                {
                                                    foreach (XPathNavigator nReq in nvReq.Select("Booking/FlightSegment"))
                                                    {
                                                        if (nReq.Select("fare_discount_percentage").Count > 0)
                                                        {
                                                            fare_id = XmlHelper.XpathValueNullToGUID(nReq, "fare_id");
                                                            fare_discount_percentage = XmlHelper.XpathValueNullToEmpty(nReq, "fare_discount_percentage");

                                                            decimal IntFareDiscountPercentage = decimal.Parse(fare_discount_percentage);
                                                            decimal FareDiscountPercentage = 0;

                                                            if (IntFareDiscountPercentage >= 0 && IntFareDiscountPercentage <= 100)
                                                            {
                                                                FareDiscountPercentage = (IntFareDiscountPercentage == 100) ? 0 : IntFareDiscountPercentage;

                                                                foreach (Mapping m in mappings)
                                                                {
                                                                    if (m.fare_id == fare_id)
                                                                    {

                                                                        m.fare_amount = m.fare_amount - Math.Ceiling(m.fare_amount * (FareDiscountPercentage / 100));
                                                                        m.fare_amount_incl = m.fare_amount_incl - Math.Ceiling(m.fare_amount_incl * (FareDiscountPercentage / 100));

                                                                        m.net_total = m.fare_amount + m.yq_amount + m.tax_amount;

                                                                        //no transfer refund
                                                                        //m.transferable_fare_flag = 1;
                                                                        //m.refundable_flag = 1;
                                                                        //m.refund_charge = 10;
                                                                    }
                                                                }

                                                                // check for just add 1 fare if same fare
                                                                if (!AlFareIdDiscount.Contains(fare_id))
                                                                {
                                                                    //add remark
                                                                    Guid userID = new Guid(Session["UserId"].ToString());
                                                                    Remark remark = new Remark();
                                                                    remark.booking_remark_id = Guid.NewGuid();
                                                                    remark.booking_id = bookingHeader.booking_id;
                                                                    remark.agency_code = objAgents[0].agency_code;
                                                                    remark.remark_type_rcd = "PNRRMK";
                                                                    remark.remark_text = "Fare id: " + fare_id.ToString() + ",discount " + FareDiscountPercentage + " % ";
                                                                    remark.update_by = userID;
                                                                    remark.update_date_time = DateTime.Now;
                                                                    remark.create_by = userID;
                                                                    remark.create_date_time = DateTime.Now;

                                                                    remarks.Add(remark);
                                                                    remark = null;
                                                                }

                                                                AlFareIdDiscount.Add(fare_id);

                                                            }                                                          
                                                            
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    strResult = Utils.ErrorXml("999", "Invalid fare discount percentage");
                                                    dtEnd = DateTime.Now;
                                                    Utils.SaveLog("FlightAdd", dtStart, dtEnd, strResult, string.Empty);
                                                    return strResult;
                                                }
                                            }
                                            catch(System.Exception ex)
                                            {
                                                strResult = Utils.ErrorXml("999", "Invalid fare discount percentage");
                                                dtEnd = DateTime.Now;
                                                Utils.SaveLog("FlightAdd", dtStart, dtEnd, strResult, string.Empty);
                                                return strResult;
                                            }

                                        }
                                        //end TA-2462

                                        string usFlight = itinerary.FindUSSegment();
                                        if (string.IsNullOrEmpty(usFlight))
                                        {
                                            //Update Transit information
                                            itinerary.FillExtendedSegmentInformation(objFlights);

                                            //Keep booking information to session.

                                            Session["MaVariable"] = objUv;
                                            Session["BookingHeader"] = bookingHeader;
                                            Session["Itinerary"] = itinerary;
                                            Session["Passengers"] = passengers;
                                            Session["Quotes"] = quotes;
                                            Session["Fees"] = fees;
                                            Session["Mappings"] = mappings;
                                            Session["Services"] = services;
                                            Session["Remarks"] = remarks;
                                            Session["Payments"] = payments;
                                            Session["Taxes"] = taxes;

                                            bookingId = bookingHeader.booking_id.ToString();

                                            strResult = Utils.ErrorXml("000", "Success Request Transaction");
                                            FlightAddError = "000";
                                        }
                                        else
                                        {
                                            strResult = Utils.ErrorXml("302", "Your Selected Flight is full.");
                                            FlightAddError = "302";
                                        }
                                    }
                                }
                                else
                                {
                                    strResult = Utils.ErrorXml("305", "One of your selected flights can not be added.");
                                    FlightAddError = "305";
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            strResult = Utils.ErrorXml("103", string.Format("General Error: {0}", e.Message));
                            FlightAddError = "103";
                            dtEnd = DateTime.Now;
                          //  Utils.SaveLog("FlightAdd", dtStart, dtEnd, e.Message, strXml);
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        FlightAddError = "100";

                        dtEnd = DateTime.Now;
                        // Utils.SaveLog("FlightAdd", dtStart, dtEnd, strResult, strXml);
                    }
                }
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("FlightAdd", dtStart, DateTime.Now, strXml + "\n" + "booking_id: " + bookingId + "\n" + "FlightAddResult: " + FlightAddError);

            }
            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string FlightAddSubLoad(string strXml)
        {
            string strResult = string.Empty;
            string currency_rcd = string.Empty;
            XElement elements = null;
            //bool bError = false;

            DateTime dtStart = DateTime.Now;
            DateTime dtEnd;

            string bookingId = string.Empty;
            Guid gFlightConnectionId = Guid.Empty;
            string strTransitFlightId = string.Empty;

            if (string.IsNullOrEmpty(strXml))
            {
                strResult = Utils.ErrorXml("300", "Flight XML Not Found");

                dtEnd = DateTime.Now;
                Utils.SaveLog("FlightAdd", dtStart, dtEnd, strResult, string.Empty);
            }
            else
            {
                elements = XElement.Parse(strXml);
                FlightSegment segment = null;
                foreach (var ele in elements.Elements("FlightSegment"))
                {
                    ele.Elements().Where(e => string.IsNullOrEmpty(e.Value)).Remove();
                    segment = new FlightSegment();
                    segment = XmlHelper.Deserialize(ele.ToString(), typeof(FlightSegment)) as FlightSegment;
                }

                if (segment == null)
                {
                    strResult = Utils.ErrorXml("341", "Flight segment Not Found");
                    dtEnd = DateTime.Now;
                    Utils.SaveLog("FlightAdd", dtStart, dtEnd, strResult, string.Empty);
                    return strResult;
                }

                //Clear previous booking session.
                BookingInitialize();

                MAVariable objUv = new MAVariable();

                XPathDocument inputXml = new XPathDocument(new StringReader(strXml));
                XPathNavigator nv = inputXml.CreateNavigator();

                Agents objAgents = (Agents)Session["Agents"];

                if (objAgents != null && objAgents.Count > 0)
                {
                    Flights objFlights = new Flights();
                    string strLanguageCode = string.Empty;
                    if (Session["LanguageCode"] != null)
                    {
                        strLanguageCode = Session["LanguageCode"].ToString();
                    }
                    else
                    {
                        strLanguageCode = "EN";
                    }
                    try
                    {

                        if (nv.Select("Booking/Header").Count == 1)
                        {
                            foreach (XPathNavigator n in nv.Select("Booking/Header"))
                            {
                                objUv.Adult = XmlHelper.XpathValueNullToInt16(n, "adult");
                                objUv.Child = XmlHelper.XpathValueNullToInt16(n, "child");
                                objUv.Infant = XmlHelper.XpathValueNullToInt16(n, "infant");
                                objUv.Other = XmlHelper.XpathValueNullToInt16(n, "other");
                                objUv.OtherPassengerType = XmlHelper.XpathValueNullToEmpty(n, "other_passenger_type");
                                currency_rcd = XmlHelper.XpathValueNullToEmpty(n, "currency_rcd");
                            }

                        }

                        foreach (XPathNavigator n in nv.Select("Booking/FlightSegment"))
                        {
                            Flight f = new Flight();
                            f.flight_id = XmlHelper.XpathValueNullToGUID(n, "flight_id");
                            f.fare_id = XmlHelper.XpathValueNullToGUID(n, "fare_id");
                            f.origin_rcd = XmlHelper.XpathValueNullToEmpty(n, "origin_rcd");
                            strTransitFlightId = XmlHelper.XpathValueNullToEmpty(n, "transit_flight_id");
                            f.transit_points = XmlHelper.XpathValueNullToEmpty(n, "transit_points");
                            f.transit_points_name = XmlHelper.XpathValueNullToEmpty(n, "transit_points_name");
                            f.boarding_class_rcd = XmlHelper.XpathValueNullToEmpty(n, "boarding_class_rcd");
                            f.booking_class_rcd = XmlHelper.XpathValueNullToEmpty(n, "booking_class_rcd");
                            f.eticket_flag = 1;

                            if (strTransitFlightId != string.Empty)
                            {
                                gFlightConnectionId = Guid.NewGuid();
                                f.od_origin_rcd = XmlHelper.XpathValueNullToEmpty(n, "origin_rcd");
                                f.od_destination_rcd = XmlHelper.XpathValueNullToEmpty(n, "destination_rcd");
                                f.flight_connection_id = gFlightConnectionId;
                                f.destination_rcd = XmlHelper.XpathValueNullToEmpty(n, "transit_airport_rcd");
                            }
                            else
                            { f.destination_rcd = XmlHelper.XpathValueNullToEmpty(n, "destination_rcd"); }
                            objFlights.Add(f);
                            f = null;
                            //Transit flight information
                            if (strTransitFlightId != string.Empty)
                            {
                                Flight transitFlight = new Flight();
                                transitFlight.destination_rcd = XmlHelper.XpathValueNullToEmpty(n, "destination_rcd");
                                transitFlight.flight_id = new Guid(strTransitFlightId);
                                transitFlight.origin_rcd = XmlHelper.XpathValueNullToEmpty(n, "transit_airport_rcd");
                                transitFlight.fare_id = XmlHelper.XpathValueNullToGUID(n, "fare_id");
                                transitFlight.eticket_flag = 1;
                                transitFlight.od_origin_rcd = XmlHelper.XpathValueNullToEmpty(n, "origin_rcd");
                                transitFlight.od_destination_rcd = XmlHelper.XpathValueNullToEmpty(n, "destination_rcd");
                                transitFlight.flight_connection_id = gFlightConnectionId;
                                transitFlight.boarding_class_rcd = XmlHelper.XpathValueNullToEmpty(n, "transit_boarding_class_rcd");
                                transitFlight.booking_class_rcd = XmlHelper.XpathValueNullToEmpty(n, "transit_booking_class_rcd");
                                objFlights.Add(transitFlight);
                                transitFlight = null;
                            }
                        }

                        // use currency from agent  if header is empty
                        if (string.IsNullOrEmpty(currency_rcd))
                        {
                            currency_rcd = objAgents[0].currency_rcd;
                        }

                        //gen Booking
                        BookingHeader bookingHeader = new BookingHeader();
                       
                        bookingHeader.agency_code = objAgents[0].agency_code;
                        bookingHeader.currency_rcd = currency_rcd; ;
                        bookingHeader.received_from = "API";
                        bookingHeader.standby_flag = 1;

                        Itinerary itinerary = new Itinerary();
                        Passengers passengers = new Passengers();
                        Quotes quotes = new Quotes();
                        Fees fees = new Fees();
                        Mappings mappings = new Mappings();
                        Services services = new Services();
                        Remarks remarks = new Remarks();
                        Payments payments = new Payments();
                        Taxes taxes = new Taxes();

                        Library objLi = new Library();

                        //fill passenger
                        string strDateInput = string.Empty;
                        passengers = new Passengers();

                        for (int i = 0; i < objUv.Adult;i++ )
                        {
                            Passenger p = new Passenger();
                            p.passenger_id = Guid.NewGuid();
                            p.passenger_type_rcd = "ADULT";
                            p.passenger_status_rcd = "MM";
                            passengers.Add(p);

                        }
                        for (int i = 0; i < objUv.Child; i++)
                        {
                            Passenger p = new Passenger();
                            p.passenger_id = Guid.NewGuid();
                            p.passenger_type_rcd = "CHD";
                            p.passenger_status_rcd = "MM";
                            passengers.Add(p);

                        }
                        for (int i = 0; i < objUv.Infant; i++)
                        {
                            Passenger p = new Passenger();
                            p.passenger_id = Guid.NewGuid();
                            p.passenger_type_rcd = "INF";
                            p.passenger_status_rcd = "MM";
                            passengers.Add(p);

                        }
                        for (int i = 0; i < objUv.Other; i++)
                        {
                            Passenger p = new Passenger();
                            p.passenger_id = Guid.NewGuid();
                            p.passenger_type_rcd = objUv.OtherPassengerType;
                            p.passenger_status_rcd = "MM";
                            passengers.Add(p);

                        }

                        string xmlPassenger = XmlHelper.Serialize(passengers, false);


                        if (objFlights.Count > 0)
                        {
                            string xmlResult = objFlights.AddFlightSubLoad(objAgents[0].agency_code,
                                                                    currency_rcd,
                                                                    bookingId,
                                                                    objUv.Adult,
                                                                    objUv.Child,
                                                                    objUv.Infant,
                                                                    objUv.Other,
                                                                    objUv.OtherPassengerType,
                                                                    Session["UserId"].ToString(),
                                                                    string.Empty,
                                                                    strLanguageCode,
                                                                    false, xmlPassenger);



                            objLi.FillBooking(xmlResult,
                                               ref bookingHeader,
                                               ref passengers,
                                               ref itinerary,
                                               ref mappings,
                                               ref payments,
                                               ref remarks,
                                               ref taxes,
                                               ref quotes,
                                               ref fees,
                                               ref services);
                        }

                        // remove fill flight

                        bookingHeader.booking_id = itinerary[0].booking_id;

                        foreach(Passenger p in passengers)
                        {
                            p.booking_id = itinerary[0].booking_id;
                        }

                        // fill MM to segment and mapping , passengers are filled in booking save
                        foreach (FlightSegment f in itinerary)
                        {
                            f.segment_status_rcd = "MM";
                        }

                        // staff type is MMM
                        foreach (Mapping m in mappings)
                        {
                            m.passenger_status_rcd = "MM";
                        }

                        Session["MaVariable"] = objUv;
                        Session["BookingHeader"] = bookingHeader;
                        Session["Itinerary"] = itinerary;
                        Session["Passengers"] = passengers;
                        Session["Quotes"] = quotes;
                        Session["Fees"] = fees;
                        Session["Mappings"] = mappings;
                        Session["Services"] = services;
                        Session["Remarks"] = remarks;
                        Session["Payments"] = payments;
                        Session["Taxes"] = taxes;

                        bookingId = bookingHeader.booking_id.ToString();

                    }
                    catch (Exception e)
                    {
                        strResult = Utils.ErrorXml("103", string.Format("General Error: {0}", e.Message));

                        dtEnd = DateTime.Now;
                        Utils.SaveLog("FlightAdd", dtStart, dtEnd, e.Message, strXml);
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");

                    dtEnd = DateTime.Now;
                    Utils.SaveLog("FlightAdd", dtStart, dtEnd, strResult, strXml);
                }
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("FlightAdd", dtStart, DateTime.Now, strXml);
                Utils.SaveProcessLog("FlightAdd", dtStart, DateTime.Now, strXml + "@\n" + "booking_id: " + bookingId);
            }
            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string BookingInitialize()
        {
            string strResult = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd;
            try
            {
                Itinerary itinerary = (Itinerary)Session["Itinerary"];
                Passengers passengers = (Passengers)Session["Passengers"];
                Quotes quotes = (Quotes)Session["Quotes"];
                Fees fees = (Fees)Session["Fees"];
                Mappings mappings = (Mappings)Session["Mappings"];
                Services services = (Services)Session["Services"];
                Remarks remarks = (Remarks)Session["Remarks"];
                Payments payments = (Payments)Session["Payments"];
                Taxes taxes = (Taxes)Session["Taxes"];

                //Release Seat
                BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];
                if (bookingHeader != null && bookingHeader.booking_id != Guid.Empty)
                {
                    ServiceClient obj = new ServiceClient();

                    obj.ReleaseFlightInventorySession(bookingHeader.booking_id.ToString(), string.Empty, string.Empty, string.Empty, false, true, true);
                }

                Session.Remove("AvailabilityOutbound");
                Session.Remove("AvailabilityReturn");

                Session.Remove("Booking");
                Session.Remove("BookingHeader");
                Session.Remove("Itinerary");
                Session.Remove("Passengers");
                Session.Remove("Quotes");
                Session.Remove("Fees");
                Session.Remove("Mappings");
                Session.Remove("Services");
                Session.Remove("Remarks");
                Session.Remove("Payments");
                Session.Remove("Taxes");
                Session.Remove("Token");

                strResult = Utils.ErrorXml("000", "Success Request Transaction");
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                Utils.SaveLog("BookingInitialize", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                strResult = Utils.ErrorXml("103", "General Error");
            }

            //Save Process Log
            //if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
            //    Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            //{
            //    Utils.SaveProcessLog("BookingInitialize", dtStart, DateTime.Now, string.Empty);
            //}
            return strResult;
        }
        [WebMethod(EnableSession = true)]
        public string BookingGetSession()
        {
            string strResult = string.Empty;
            string bookingId = string.Empty;


            DateTime dtStart = DateTime.Now;
            DateTime dtEnd;

            try
            {
                if (Session["BookingHeader"] != null)
                {

                    BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];
                    Itinerary itinerary = (Itinerary)Session["Itinerary"];
                    Passengers passengers = (Passengers)Session["Passengers"];
                    Quotes quotes = (Quotes)Session["Quotes"];
                    Fees fees = (Fees)Session["Fees"];
                    Mappings mappings = (Mappings)Session["Mappings"];
                    Services services = (Services)Session["Services"];
                    Remarks remarks = (Remarks)Session["Remarks"];
                    Payments payments = (Payments)Session["Payments"];
                    Taxes taxes = (Taxes)Session["Taxes"];

                    bookingId = bookingHeader.booking_id.ToString();

                    Library objLi = new Library();
                    strResult = objLi.BuiltBookingXml(bookingHeader,
                                                        itinerary,
                                                        passengers,
                                                        quotes,
                                                        fees,
                                                        mappings,
                                                        services,
                                                        remarks,
                                                        payments,
                                                        taxes,
                                                        true);
                }
                else
                {
                    strResult = Utils.ErrorXml("400", "No Booking Information Found");

                    dtEnd = DateTime.Now;
                    Utils.SaveLog("BookingGetSession", dtStart, dtEnd, strResult, "booking_id: "+ bookingId);
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                Utils.SaveLog("BookingGetSession", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, strResult);
                strResult = Utils.ErrorXml("103", "General Error");
            }

            //Save Process Log
            //if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
            //    Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            //{
            //    Utils.SaveProcessLog("BookingGetSession", dtStart, DateTime.Now, "booking_id: " + bookingId);
            //}
            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string BookingSave(string strXml)
        {
            int iVoucherCount = 0;
            string strResult = string.Empty;
            bool bProcessSave = true;
            bool bValid = true;
            bool bPaid = false;
            bool bVoucherProcess = false;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            Agents objAgents = (Agents)Session["Agents"];
            List<ValidateVoucherRequest> objVoucherValidates = new List<ValidateVoucherRequest>();
            Payment objCCPayment = new Payment();
            Payments objPaymentsInput = new Payments();
            string bookingId = string.Empty;
            bool createTikets = true;
            string document_number = string.Empty;
            string cvv_card = string.Empty;
            //   string expiry_month = "";
            //   string expiry_year = "";
            int iPaymentCount = 0;
            string saveBookingResult = string.Empty;

            try
            {
                if (objAgents != null && objAgents.Count > 0)
                {
                    BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];
                    if (bookingHeader != null)
                    {
                        MAVariable objUv = (MAVariable)Session["MaVariable"];

                        Itinerary itinerary = (Itinerary)Session["Itinerary"];
                        Passengers passengers = (Passengers)Session["Passengers"];
                        Quotes quotes = (Quotes)Session["Quotes"];
                        Fees fees = (Fees)Session["Fees"];
                        Mappings mappings = (Mappings)Session["Mappings"];
                        Services services = (Services)Session["Services"];
                        Remarks remarks = (Remarks)Session["Remarks"];
                        Payments payments = (Payments)Session["Payments"];
                        Taxes taxes = (Taxes)Session["Taxes"];

                        Library objLi = new Library();
                        Clients clientsPassenger = null;
                        Client client = (Client)Session["ClientProfile"];
                        bool IsClientAndZeroTotal = false;

                        string strUsSegment = itinerary.FindUSSegment();

                        if (string.IsNullOrEmpty(strUsSegment))
                        {
                            XPathDocument xmlDoc = new XPathDocument(new StringReader(strXml));
                            XPathNavigator nv = xmlDoc.CreateNavigator();
                            //validate XML input.

                            bookingId = bookingHeader.booking_id.ToString();

                            string strLanguageCode = string.Empty;
                            if (Session["LanguageCode"] != null)
                            {
                                strLanguageCode = Session["LanguageCode"].ToString();
                            }
                            else
                            {
                                strLanguageCode = "EN";
                            }

                            // ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.GetPassengerManifest);

                            //overide contact details
                            if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.OverideContactDetail) == true)
                            {
                                #region Contact detail
                                //Add Contact detail information
                                foreach (XPathNavigator n in nv.Select("Booking/BookingHeader"))
                                {
                                    bookingHeader.number_of_adults = objUv.Adult;
                                    bookingHeader.number_of_children = objUv.Child;
                                    bookingHeader.number_of_infants = objUv.Infant;
                                    bookingHeader.number_of_others = objUv.Other;
                                    bookingHeader.received_from = "API";

                                    #region fill in contact detail from client
                                    if (XmlHelper.XpathValueNullToBoolean(n, "apply_client_profile_flag") == true)
                                    {
                                        //  found client login
                                        if (client != null)
                                        {
                                            bookingHeader.client_number = client.client_number;
                                            bookingHeader.client_profile_id = client.client_profile_id;
                                            bookingHeader.title_rcd = client.title_rcd;
                                            bookingHeader.lastname = client.lastname;
                                            bookingHeader.firstname = client.firstname;
                                            bookingHeader.contact_name = client.contact_name;
                                            bookingHeader.contact_email = client.contact_email;
                                            bookingHeader.address_line1 = client.address_line1;
                                            bookingHeader.address_line2 = client.address_line2;
                                            bookingHeader.city = client.city;
                                            bookingHeader.zip_code = client.zip_code;
                                            bookingHeader.country_rcd = client.country_rcd;
                                            bookingHeader.phone_mobile = client.phone_mobile;
                                            bookingHeader.phone_home = client.phone_home;
                                            bookingHeader.phone_business = client.phone_business;
                                            bookingHeader.state = client.state;

                                            //mobile_email
                                            bookingHeader.mobile_email = client.mobile_email;
                                            //add new letter flag
                                            bookingHeader.newsletter_flag = XmlHelper.XpathValueNullToByte(n, "newsletter_flag");
                                        }
                                        else
                                        {
                                            bProcessSave = false;
                                            strResult = Utils.ErrorXml("507", "No client information found. Please do client login before process.");
                                            dtEnd = DateTime.Now;
                                            // Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, strXml);
                                            saveBookingResult = "507";
                                        }

                                    }
                                    #endregion

                                    // fill contact details
                                    if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "title_rcd")))
                                        bookingHeader.title_rcd = XmlHelper.XpathValueNullToEmpty(n, "title_rcd").ToUpper();

                                    if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "lastname")))
                                        bookingHeader.lastname = XmlHelper.XpathValueNullToEmpty(n, "lastname");

                                    if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "firstname")))
                                        bookingHeader.firstname = XmlHelper.XpathValueNullToEmpty(n, "firstname");

                                    if (string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "contact_name")))
                                        bookingHeader.contact_name = bookingHeader.firstname + " " + bookingHeader.lastname;
                                    else
                                        bookingHeader.contact_name = XmlHelper.XpathValueNullToEmpty(n, "contact_name");


                                    if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "email")))
                                        bookingHeader.contact_email = XmlHelper.XpathValueNullToEmpty(n, "email");

                                    if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "address_line1")))
                                        bookingHeader.address_line1 = XmlHelper.XpathValueNullToEmpty(n, "address_line1");

                                    if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "address_line2")))
                                        bookingHeader.address_line2 = XmlHelper.XpathValueNullToEmpty(n, "address_line2");

                                    if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "city")))
                                        bookingHeader.city = XmlHelper.XpathValueNullToEmpty(n, "city");

                                    if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "zip_code")))
                                        bookingHeader.zip_code = XmlHelper.XpathValueNullToEmpty(n, "zip_code");

                                    if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "country_rcd")))
                                        bookingHeader.country_rcd = XmlHelper.XpathValueNullToEmpty(n, "country_rcd");

                                    if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "phone_mobile")))
                                        bookingHeader.phone_mobile = XmlHelper.XpathValueNullToEmpty(n, "phone_mobile");

                                    if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "phone_home")))
                                        bookingHeader.phone_home = XmlHelper.XpathValueNullToEmpty(n, "phone_home");

                                    if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "phone_business")))
                                        bookingHeader.phone_business = XmlHelper.XpathValueNullToEmpty(n, "phone_business");

                                    if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "phone_search")))
                                        bookingHeader.phone_search = XmlHelper.XpathValueNullToEmpty(n, "phone_search");

                                    if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "state")))
                                        bookingHeader.state = XmlHelper.XpathValueNullToEmpty(n, "state");

                                    //add new letter flag
                                    bookingHeader.newsletter_flag = XmlHelper.XpathValueNullToByte(n, "newsletter_flag");

                                    // group name
                                    bookingHeader.group_booking_flag = XmlHelper.XpathValueNullToByte(n, "group_booking_flag");
                                    bookingHeader.group_name = XmlHelper.XpathValueNullToEmpty(n, "group_name");

                                    bookingHeader.language_rcd = strLanguageCode;
                                    bookingHeader.ip_address = DataHelper.GetClientIpAddress();//127.0.0.1;

                                    //mobile_email
                                    if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "mobile_email")))
                                        bookingHeader.mobile_email = XmlHelper.XpathValueNullToEmpty(n, "mobile_email");

                                    //Check agency type.
                                    if (objAgents[0].own_agency_flag == 0)
                                    {
                                        bookingHeader.booking_source_rcd = "B2B";
                                    }
                                    else if (objAgents[0].web_agency_flag == 0)
                                    {
                                        bookingHeader.booking_source_rcd = "OWN";
                                    }
                                    else
                                    {
                                        bookingHeader.booking_source_rcd = "B2C";
                                    }
                                }
                                #endregion
                            }
                            // do the same original
                            else
                            {
                                #region Contact detail
                                //Add Contact detail information
                                foreach (XPathNavigator n in nv.Select("Booking/BookingHeader"))
                                {
                                    bookingHeader.number_of_adults = objUv.Adult;
                                    bookingHeader.number_of_children = objUv.Child;
                                    bookingHeader.number_of_infants = objUv.Infant;
                                    bookingHeader.received_from = "API";

                                    if (XmlHelper.XpathValueNullToBoolean(n, "apply_client_profile_flag") == false)
                                    {
                                        bookingHeader.title_rcd = XmlHelper.XpathValueNullToEmpty(n, "title_rcd").ToUpper();
                                        bookingHeader.lastname = XmlHelper.XpathValueNullToEmpty(n, "lastname");
                                        bookingHeader.firstname = XmlHelper.XpathValueNullToEmpty(n, "firstname");

                                        if (string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "contact_name")))
                                            bookingHeader.contact_name = bookingHeader.firstname + " " + bookingHeader.lastname;
                                        else
                                            bookingHeader.contact_name = XmlHelper.XpathValueNullToEmpty(n, "contact_name");

                                        bookingHeader.contact_email = XmlHelper.XpathValueNullToEmpty(n, "email");
                                        bookingHeader.address_line1 = XmlHelper.XpathValueNullToEmpty(n, "address_line1");
                                        bookingHeader.address_line2 = XmlHelper.XpathValueNullToEmpty(n, "address_line2");
                                        bookingHeader.city = XmlHelper.XpathValueNullToEmpty(n, "city");
                                        bookingHeader.zip_code = XmlHelper.XpathValueNullToEmpty(n, "zip_code");
                                        bookingHeader.country_rcd = XmlHelper.XpathValueNullToEmpty(n, "country_rcd");
                                        bookingHeader.phone_mobile = XmlHelper.XpathValueNullToEmpty(n, "phone_mobile");
                                        bookingHeader.phone_home = XmlHelper.XpathValueNullToEmpty(n, "phone_home");
                                        bookingHeader.phone_business = XmlHelper.XpathValueNullToEmpty(n, "phone_business");
                                        bookingHeader.phone_search = XmlHelper.XpathValueNullToEmpty(n, "phone_search");
                                        bookingHeader.state = XmlHelper.XpathValueNullToEmpty(n, "state");

                                        //add new letter flag
                                        bookingHeader.newsletter_flag = XmlHelper.XpathValueNullToByte(n, "newsletter_flag");

                                        // group name
                                        bookingHeader.group_booking_flag = XmlHelper.XpathValueNullToByte(n, "group_booking_flag");
                                        bookingHeader.group_name = XmlHelper.XpathValueNullToEmpty(n, "group_name");

                                        //mobile_email
                                        if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "mobile_email")))
                                            bookingHeader.mobile_email = XmlHelper.XpathValueNullToEmpty(n, "mobile_email");

                                    }
                                    else
                                    {
                                        //  found client login

                                        if (client != null)
                                        {
                                            bookingHeader.client_number = client.client_number;
                                            bookingHeader.client_profile_id = client.client_profile_id;
                                            bookingHeader.title_rcd = client.title_rcd;
                                            bookingHeader.lastname = client.lastname;
                                            bookingHeader.firstname = client.firstname;
                                            bookingHeader.contact_name = client.contact_name;
                                            bookingHeader.contact_email = client.contact_email;
                                            bookingHeader.address_line1 = client.address_line1;
                                            bookingHeader.address_line2 = client.address_line2;
                                            bookingHeader.city = client.city;
                                            bookingHeader.zip_code = client.zip_code;
                                            bookingHeader.country_rcd = client.country_rcd;
                                            bookingHeader.phone_mobile = client.phone_mobile;
                                            bookingHeader.phone_home = client.phone_home;
                                            bookingHeader.phone_business = client.phone_business;
                                            bookingHeader.state = client.state;

                                            //mobile_email
                                            bookingHeader.mobile_email = client.mobile_email;

                                            //add new letter flag
                                            bookingHeader.newsletter_flag = XmlHelper.XpathValueNullToByte(n, "newsletter_flag");

                                        }
                                        else
                                        {
                                            bProcessSave = false;
                                            strResult = Utils.ErrorXml("507", "No client information found. Please do client login before process.");
                                            dtEnd = DateTime.Now;
                                            // Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, strXml);
                                            saveBookingResult = "507";
                                        }
                                    }
                                    bookingHeader.language_rcd = strLanguageCode;
                                    bookingHeader.ip_address = DataHelper.GetClientIpAddress();//127.0.0.1;

                                    //Check agency type.
                                    if (objAgents[0].own_agency_flag == 0)
                                    {
                                        bookingHeader.booking_source_rcd = "B2B";
                                    }
                                    else if (objAgents[0].web_agency_flag == 0)
                                    {
                                        bookingHeader.booking_source_rcd = "OWN";
                                    }
                                    else
                                    {
                                        bookingHeader.booking_source_rcd = "B2C";
                                    }
                                }
                                #endregion
                            }

                            if (bProcessSave == true)
                            {
                                #region Passenger
                                //Add passenger information
                                string strDateInput = string.Empty;
                                if (bookingHeader.client_number > 0)
                                {
                                    clientsPassenger = new Clients();
                                    clientsPassenger.ReadClientPassenger(string.Empty, string.Empty, bookingHeader.client_number.ToString());
                                }

                                foreach (XPathNavigator n in nv.Select("Booking/Passenger"))
                                {
                                    foreach (Passenger p in passengers)
                                    {
                                        if (p.passenger_id.Equals(XmlHelper.XpathValueNullToGUID(n, "passenger_id")) == true)
                                        {
                                            //Fill client information when passenger profile id found.
                                            p.passenger_profile_id = XmlHelper.XpathValueNullToGUID(n, "passenger_profile_id");

                                            if (clientsPassenger != null && clientsPassenger.Count > 0 && p.passenger_profile_id.Equals(Guid.Empty) == false)
                                            {
                                                FillClientInformation(clientsPassenger, p);
                                            }
                                            else
                                            {
                                                // for co client
                                                Client CorClient = (Client)Session["CorClient"];
                                                if (CorClient != null)
                                                {
                                                    FillClientCo(CorClient.client_profile_id.ToString(), clientsPassenger);

                                                    FillClientInformation(clientsPassenger, p);
                                                }
                                                else
                                                {
                                                    p.lastname = XmlHelper.XpathValueNullToEmpty(n, "lastname").ToUpper();
                                                    p.firstname = XmlHelper.XpathValueNullToEmpty(n, "firstname").ToUpper();
                                                    p.title_rcd = XmlHelper.XpathValueNullToEmpty(n, "title_rcd").ToUpper();
                                                    p.passport_number = XmlHelper.XpathValueNullToEmpty(n, "passport_number");
                                                    p.document_type_rcd = XmlHelper.XpathValueNullToEmpty(n, "document_type_rcd");
                                                    p.known_traveler_number = XmlHelper.XpathValueNullToEmpty(n, "known_traveler_number");

                                                    strDateInput = XmlHelper.XpathValueNullToEmpty(n, "passport_issue_date");
                                                    if (string.IsNullOrEmpty(strDateInput) == false && strDateInput.Length == 8)
                                                    {
                                                        p.passport_issue_date = DataHelper.ParseDate(strDateInput);
                                                    }

                                                    strDateInput = XmlHelper.XpathValueNullToEmpty(n, "passport_expiry_date");

                                                    if (string.IsNullOrEmpty(strDateInput) == false && strDateInput.Length == 8)
                                                    {
                                                        p.passport_expiry_date = DataHelper.ParseDate(strDateInput);
                                                    }

                                                    p.passport_issue_place = XmlHelper.XpathValueNullToEmpty(n, "passport_issue_place");
                                                    p.passport_birth_place = XmlHelper.XpathValueNullToEmpty(n, "passport_birth_place");
                                                    p.passport_issue_country_rcd = XmlHelper.XpathValueNullToEmpty(n, "passport_issue_country_rcd");
                                                    p.nationality_rcd = XmlHelper.XpathValueNullToEmpty(n, "nationality_rcd");
                                                    p.state = XmlHelper.XpathValueNullToEmpty(n, "state");
                                                    p.gender_type_rcd = XmlHelper.XpathValueNullToEmpty(n, "gender_type_rcd");

                                                    strDateInput = XmlHelper.XpathValueNullToEmpty(n, "date_of_birth");
                                                    if (string.IsNullOrEmpty(strDateInput) == false && strDateInput.Length == 8)
                                                    {
                                                        p.date_of_birth = DataHelper.ParseDate(strDateInput);
                                                    }
                                                }
                                            }

                                            //overide passenger information
                                            // {

                                            if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "lastname")))
                                                p.lastname = XmlHelper.XpathValueNullToEmpty(n, "lastname").ToUpper();

                                            if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "firstname")))
                                                p.firstname = XmlHelper.XpathValueNullToEmpty(n, "firstname").ToUpper();

                                            if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "title_rcd")))
                                                p.title_rcd = XmlHelper.XpathValueNullToEmpty(n, "title_rcd").ToUpper();

                                            if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "passport_number")))
                                                p.passport_number = XmlHelper.XpathValueNullToEmpty(n, "passport_number");

                                            if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "document_type_rcd")))
                                                p.document_type_rcd = XmlHelper.XpathValueNullToEmpty(n, "document_type_rcd");

                                            if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "known_traveler_number")))
                                                p.known_traveler_number = XmlHelper.XpathValueNullToEmpty(n, "known_traveler_number");

                                            //overide member_number
                                            if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "member_number")))
                                                p.member_number = XmlHelper.XpathValueNullToEmpty(n, "member_number");

                                            if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "passport_issue_date")))
                                            {
                                                strDateInput = XmlHelper.XpathValueNullToEmpty(n, "passport_issue_date");
                                                if (string.IsNullOrEmpty(strDateInput) == false && strDateInput.Length == 8)
                                                {
                                                    p.passport_issue_date = DataHelper.ParseDate(strDateInput);
                                                }
                                            }

                                            if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "passport_expiry_date")))
                                            {
                                                strDateInput = XmlHelper.XpathValueNullToEmpty(n, "passport_expiry_date");

                                                if (string.IsNullOrEmpty(strDateInput) == false && strDateInput.Length == 8)
                                                {
                                                    p.passport_expiry_date = DataHelper.ParseDate(strDateInput);
                                                }
                                            }

                                            if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "passport_issue_place")))
                                                p.passport_issue_place = XmlHelper.XpathValueNullToEmpty(n, "passport_issue_place");

                                            if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "passport_birth_place")))
                                                p.passport_birth_place = XmlHelper.XpathValueNullToEmpty(n, "passport_birth_place");

                                            if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "passport_issue_country_rcd")))
                                                p.passport_issue_country_rcd = XmlHelper.XpathValueNullToEmpty(n, "passport_issue_country_rcd");

                                            if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "nationality_rcd")))
                                                p.nationality_rcd = XmlHelper.XpathValueNullToEmpty(n, "nationality_rcd");

                                            if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "state")))
                                                p.state = XmlHelper.XpathValueNullToEmpty(n, "state");

                                            if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "gender_type_rcd")))
                                                p.gender_type_rcd = XmlHelper.XpathValueNullToEmpty(n, "gender_type_rcd");

                                            if (!string.IsNullOrEmpty(XmlHelper.XpathValueNullToEmpty(n, "date_of_birth")))
                                            {
                                                strDateInput = XmlHelper.XpathValueNullToEmpty(n, "date_of_birth");
                                                if (string.IsNullOrEmpty(strDateInput) == false && strDateInput.Length == 8)
                                                {
                                                    p.date_of_birth = DataHelper.ParseDate(strDateInput);
                                                }
                                            }


                                            //}

                                        }
                                    }
                                }
                                #endregion

                                #region Mapping
                                //Add name to Mapping
                                foreach (Passenger p in passengers)
                                {
                                    foreach (Mapping mp in mappings)
                                    {
                                        if (mp.passenger_id.Equals(p.passenger_id) == true)
                                        {
                                            mp.lastname = p.lastname;
                                            mp.firstname = p.firstname;
                                            mp.title_rcd = p.title_rcd;
                                            mp.date_of_birth = p.date_of_birth;
                                            mp.gender_type_rcd = p.gender_type_rcd;
                                        }
                                    }
                                }
                                #endregion

                                #region Mapping
                                //Add record locator to Mapping
                                //
                                foreach (Mapping mp in mappings)
                                {
                                    mp.record_locator = bookingHeader.record_locator;
                                    //  mp.record_locator_display = bookingHeader.record_locator;
                                }
                                #endregion


                                // agency off
                                if (objAgents[0].default_ticket_on_save_flag == 0)
                                {
                                    createTikets = false;
                                }
                                else
                                {
                                    // MM and config off
                                    foreach (Mapping mp in mappings)
                                    {
                                        if (mp.passenger_status_rcd == "MM" && ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.bAllowMMIssueTicket) == false)
                                        {
                                            createTikets = false;
                                            break;
                                        }
                                    }

                                }


                                if (services != null)
                                {
                                    foreach (Mapping mp in mappings)
                                    {
                                        foreach (Service s in services)
                                        {
                                            if (mp.passenger_status_rcd == "MM" && mp.booking_segment_id == s.booking_segment_id)
                                                s.special_service_status_rcd = "RQ";
                                        }

                                    }
                                }


                                #region Payment
                                //Set update information
                                objUv.UserId = new Guid(Session["UserId"].ToString());
                                SetCreateUpdateInformation(objUv.UserId, bookingHeader, itinerary, passengers, fees, mappings, services, remarks, payments, taxes, null);

                                iPaymentCount = nv.Select("Booking/Payment").Count;


                                //  check outstanding balance and FFP
                                decimal dOutStandingBalance = objLi.CalOutStandingBalance(quotes, fees, payments);
                                if (dOutStandingBalance == 0 && client != null)
                                {
                                    IsClientAndZeroTotal = true;
                                }

                                if (iPaymentCount == 0)
                                {
                                    //Save Booking without payment
                                    #region Book Now Pay Later
                                    Booking objBooking = new Booking();
                                    strResult = objBooking.SaveBooking(IsClientAndZeroTotal,
                                                                    ref bookingHeader,
                                                                    ref itinerary,
                                                                    ref passengers,
                                                                    ref quotes,
                                                                    ref fees,
                                                                    ref mappings,
                                                                    ref services,
                                                                    ref remarks,
                                                                    ref payments,
                                                                    ref taxes,
                                                                    string.Empty);

                                    strResult = GetErrorPayment(strResult, strXml, dtStart, dtEnd);
                                    #endregion
                                }
                                else
                                {
                                    //get form Of Payment Fee.
                                    Payment objPaymentInput = null;
                                    ServiceClient objClient = new ServiceClient();
                                    Payments objPayment = new Payments();

                                    string strFOP = string.Empty;
                                    foreach (XPathNavigator n in nv.Select("Booking/Payment"))
                                    {
                                        strFOP = objLi.getXPathNodevalue(n, "form_of_payment_rcd", Library.xmlReturnType.value);
                                        if (strFOP == "CC" && bValid)
                                        {
                                            if (nv.Select("Booking/Payment[form_of_payment_rcd = 'CC']").Count > 1)
                                            {
                                                strResult = Utils.ErrorXml("515", "Credit card XML must has only one section.");
                                                bValid = false;
                                                dtEnd = DateTime.Now;
                                                // Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, "");
                                                saveBookingResult = "515";
                                            }
                                            else if (iPaymentCount > 1 && ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.MultipleFOP) == false)
                                            {
                                                //strResult = Utils.ErrorXml("500", "Payment XML Can only have one node");
                                                strResult = Utils.ErrorXml("516", "Multiple form of payments not allowed.");
                                                bValid = false;
                                                dtEnd = DateTime.Now;
                                                //  Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, "");
                                                saveBookingResult = "516";
                                            }
                                            else
                                            {
                                                #region Credit Card Payment
                                                if (objAgents[0].b2b_credit_card_payment_flag == 1)
                                                {
                                                    //Credit Card Payment.
                                                    objPaymentInput = new Payment();
                                                    //Start Input Section

                                                    if(XmlHelper.XpathValueNullToEmpty(n, "form_of_payment_rcd") != string.Empty)
                                                        objPaymentInput.form_of_payment_rcd = XmlHelper.XpathValueNullToEmpty(n, "form_of_payment_rcd").ToUpper();

                                                    if (XmlHelper.XpathValueNullToEmpty(n, "form_of_payment_subtype_rcd") != string.Empty)
                                                        objPaymentInput.form_of_payment_subtype_rcd = XmlHelper.XpathValueNullToEmpty(n, "form_of_payment_subtype_rcd").ToUpper();

                                                    objPaymentInput.name_on_card = XmlHelper.XpathValueNullToEmpty(n, "NameOnCard");
                                                    objPaymentInput.document_number = XmlHelper.XpathValueNullToEmpty(n, "CreditCardNumber");
                                                    objPaymentInput.cvv_code = XmlHelper.XpathValueNullToEmpty(n, "CVV");
                                                    objPaymentInput.issue_month = XmlHelper.XpathValueNullToInt(n, "IssueMonth");
                                                    objPaymentInput.issue_year = XmlHelper.XpathValueNullToInt(n, "IssueYear");
                                                    objPaymentInput.issue_number = XmlHelper.XpathValueNullToEmpty(n, "IssueNumber");
                                                    objPaymentInput.expiry_month = XmlHelper.XpathValueNullToInt(n, "ExpiryMonth");
                                                    objPaymentInput.expiry_year = XmlHelper.XpathValueNullToInt(n, "ExpiryYear");
                                                    objPaymentInput.address_line1 = XmlHelper.XpathValueNullToEmpty(n, "Addr1") + " " + XmlHelper.XpathValueNullToEmpty(n, "Addr2");
                                                    objPaymentInput.street = XmlHelper.XpathValueNullToEmpty(n, "Street");
                                                    objPaymentInput.state = XmlHelper.XpathValueNullToEmpty(n, "State");
                                                    objPaymentInput.city = XmlHelper.XpathValueNullToEmpty(n, "City");
                                                    objPaymentInput.zip_code = XmlHelper.XpathValueNullToEmpty(n, "ZipCode");
                                                    objPaymentInput.country_rcd = XmlHelper.XpathValueNullToEmpty(n, "Country");

                                                    cvv_card = objPaymentInput.cvv_code;
                                                    document_number = objPaymentInput.document_number;
                                                   // expiry_month = objPaymentInput.expiry_month + "";
                                                   // expiry_year = objPaymentInput.expiry_year + "";

                                                    if (iPaymentCount == 1)
                                                    {
                                                        strResult = objPayment.PaymentCreditCard(ref bookingHeader,
                                                                                                 ref itinerary,
                                                                                                 ref passengers,
                                                                                                 ref quotes,
                                                                                                 ref fees,
                                                                                                 ref mappings,
                                                                                                 ref services,
                                                                                                 ref remarks,
                                                                                                 ref payments,
                                                                                                 ref taxes,
                                                                                                 objPaymentInput,
                                                                                                 string.Empty,
                                                                                                 strLanguageCode,
                                                                                                 objUv.UserId,
                                                                                                 objUv.booking_payment_id,
                                                                                                 DataHelper.GetClientIpAddress(),//127.0.0.1
                                                                                                 string.Empty,
                                                                                                 objUv.Adult,
                                                                                                 objUv.Child,
                                                                                                 objUv.Infant,
                                                                                                 createTikets);

                                                        strResult = GetErrorPayment(strResult, strXml, dtStart, dtEnd);
                                                    }
                                                    else
                                                    {
                                                        objCCPayment = objPaymentInput;
                                                        if ((iVoucherCount + 1) == iPaymentCount)
                                                        {
                                                            objPaymentsInput.Add(objCCPayment);
                                                            strResult = objPayment.PaymentMultipleForm(ref bookingHeader,
                                                                                                        ref itinerary,
                                                                                                        ref passengers,
                                                                                                        ref quotes,
                                                                                                        ref fees,
                                                                                                        ref mappings,
                                                                                                        ref services,
                                                                                                        ref remarks,
                                                                                                        ref payments,
                                                                                                        ref taxes,
                                                                                                        objPaymentsInput,
                                                                                                        string.Empty,
                                                                                                        bookingHeader.language_rcd,
                                                                                                        objUv.UserId,
                                                                                                        bookingHeader.ip_address,
                                                                                                        objUv.FormOfPaymentFee,
                                                                                                        objUv.Adult,
                                                                                                        objUv.Child,
                                                                                                        objUv.Infant,
                                                                                                        objUv.booking_payment_id, createTikets);

                                                            if (strResult.Length > 0)
                                                            {
                                                                strResult = GetErrorPayment(strResult, strXml, dtStart, dtEnd);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    strResult = Utils.ErrorXml("505", "Payment by credit card is not allowed.");
                                                    bValid = false;
                                                    dtEnd = DateTime.Now;
                                                    // Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, strXml);
                                                    saveBookingResult = "505";
                                                }
                                                #endregion
                                                //i++;
                                            }
                                        }
                                        else if ((strFOP.Equals("CRAGT") || strFOP.Equals("INV")) && bValid)
                                        {
                                            if (iPaymentCount > 1)
                                            {
                                                strResult = Utils.ErrorXml("500", "Payment XML can have only one section.");
                                                dtEnd = DateTime.Now;
                                                //  Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, strXml);
                                                saveBookingResult = "500";
                                            }
                                            else
                                            {
                                                #region Credit Agency & Invoice Payment
                                                if (objAgents[0].b2b_credit_agency_and_invoice_flag == 1)
                                                {
                                                    if (objAgents[0].agency_payment_type_rcd.Equals(strFOP) == false)
                                                    {
                                                        strResult = Utils.ErrorXml("508", "Your selected form of payment is not support.");

                                                        dtEnd = DateTime.Now;
                                                        //  Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, strXml);
                                                        saveBookingResult = "508";
                                                    }
                                                    else if ((objAgents[0].agency_account - objAgents[0].booking_payment) > objLi.CalOutStandingBalance(quotes, fees, payments))
                                                    {
                                                        //Payment Credit Agency
                                                        objPaymentInput = new Payment();
                                                        objPaymentInput.form_of_payment_rcd = strFOP;
                                                        objPaymentInput.document_number = XmlHelper.XpathValueNullToEmpty(n, "document_number");

                                                        strResult = objPayment.PaymentCreditAgent(ref bookingHeader,
                                                                                                  ref itinerary,
                                                                                                  ref passengers,
                                                                                                  ref quotes,
                                                                                                  ref fees,
                                                                                                  ref mappings,
                                                                                                  ref services,
                                                                                                  ref remarks,
                                                                                                  ref payments,
                                                                                                  ref taxes,
                                                                                                  objPaymentInput,
                                                                                                  strLanguageCode,
                                                                                                  objUv.UserId,
                                                                                                  string.Empty,
                                                                                                  objAgents[0]);

                                                        strResult = GetErrorPayment(strResult, strXml, dtStart, dtEnd);
                                                    }
                                                    else
                                                    {
                                                        strResult = Utils.ErrorXml("503", "Not Enough Credit Balance.");

                                                        dtEnd = DateTime.Now;
                                                        // Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, strXml);
                                                        saveBookingResult = "503";
                                                    }
                                                }
                                                else
                                                {
                                                    strResult = Utils.ErrorXml("506", "Payment by credit agent is not allowed.");

                                                    dtEnd = DateTime.Now;
                                                    //  Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, strXml);
                                                    saveBookingResult = "506";
                                                }
                                                #endregion
                                            }
                                        }
                                        else if (strFOP.Equals("VOUCHER") && bValid)
                                        {
                                            #region Voucher
                                            if (!bPaid)
                                            {
                                                int iVoucherNode = nv.Select("Booking/Payment[form_of_payment_rcd = 'VOUCHER']").Count;
                                                if (iVoucherNode > 1 && ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.MultipleVoucher) == false)
                                                {
                                                    strResult = Utils.ErrorXml("7010", "Multiple voucher payment not allowed.");
                                                    bValid = false;
                                                    dtEnd = DateTime.Now;
                                                    // Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, strXml);
                                                    saveBookingResult = "7010";
                                                }
                                                else if (iVoucherNode > ConfigurationUtil.GetFunctionalSettingInt(ConfigurationUtil.ConfigName.MaxVoucher))
                                                {
                                                    strResult = Utils.ErrorXml("7009", "Number of vouchers over limit.");
                                                    bValid = false;
                                                    dtEnd = DateTime.Now;
                                                    // Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, strXml);
                                                    saveBookingResult = "7009";
                                                }
                                                else
                                                {
                                                    if (objAgents[0].b2b_voucher_payment_flag == 1)
                                                    {
                                                        ValidateVoucherRequest objVoucherValidate = new ValidateVoucherRequest();
                                                        objVoucherValidate.VoucherNumber = XmlHelper.XpathValueNullToEmpty(n, "voucher_number");
                                                        objVoucherValidate.VoucherPassword = XmlHelper.XpathValueNullToEmpty(n, "password");
                                                        objVoucherValidates.Add(objVoucherValidate);
                                                        iVoucherCount++;

                                                        if (objVoucherValidates.Count > 0)
                                                        {
                                                            strResult = ValidateVoucher(objVoucherValidates, iPaymentCount, ref objPaymentsInput);

                                                            if (strResult.Equals(string.Empty))
                                                            {
                                                                if (iVoucherCount == iPaymentCount)
                                                                {
                                                                    bVoucherProcess = true;
                                                                }
                                                                else
                                                                {
                                                                    if ((iVoucherCount + 1) == iPaymentCount)
                                                                    {
                                                                        if (objCCPayment.form_of_payment_rcd.Equals("CC"))
                                                                        {
                                                                            objPaymentsInput.Add(objCCPayment);
                                                                            bVoucherProcess = true;
                                                                        }
                                                                    }
                                                                }

                                                                if (bVoucherProcess)
                                                                {
                                                                    strResult = objPayment.PaymentMultipleForm(ref bookingHeader,
                                                                                                                ref itinerary,
                                                                                                                ref passengers,
                                                                                                                ref quotes,
                                                                                                                ref fees,
                                                                                                                ref mappings,
                                                                                                                ref services,
                                                                                                                ref remarks,
                                                                                                                ref payments,
                                                                                                                ref taxes,
                                                                                                                objPaymentsInput,
                                                                                                                string.Empty,
                                                                                                                bookingHeader.language_rcd,
                                                                                                                objUv.UserId,
                                                                                                                bookingHeader.ip_address,
                                                                                                                objUv.FormOfPaymentFee,
                                                                                                                objUv.Adult,
                                                                                                                objUv.Child,
                                                                                                                objUv.Infant,
                                                                                                                objUv.booking_payment_id, createTikets);

                                                                    if (strResult.Length > 0)
                                                                    {
                                                                        strResult = GetErrorPayment(strResult, strXml, dtStart, dtEnd);
                                                                        bPaid = true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            strResult = Utils.ErrorXml("514", "Voucher not found.");
                                                            bValid = false;
                                                            dtEnd = DateTime.Now;
                                                            // Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, strXml);
                                                            saveBookingResult = "514";
                                                        }

                                                    }
                                                    else
                                                    {
                                                        strResult = Utils.ErrorXml("513", "Payment by voucher is not allowed.");
                                                        bValid = false;
                                                        dtEnd = DateTime.Now;
                                                      //  Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, strXml);
                                                        saveBookingResult = "513";
                                                    }
                                                }
                                            }
                                            //bPaid = true;
                                            #endregion
                                        }
                                        else
                                        {
                                            if (bValid)
                                            {
                                                strResult = Utils.ErrorXml("502", "Form of payment not found.");
                                                bValid = false;
                                                dtEnd = DateTime.Now;
                                                //Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, strXml);
                                                saveBookingResult = "502";
                                            }
                                        }
                                    }

                                }
                                Session["BookingHeader"] = bookingHeader;
                                #endregion
                            }
                            Session.Remove("AvailabilityOutbound");
                            Session.Remove("AvailabilityReturn");
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("401", "Find US Segment.");
                            bValid = false;
                            dtEnd = DateTime.Now;
                            //   Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, strXml);
                            saveBookingResult = "401";
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("400", "No Booking Information Found.");
                        bValid = false;
                        dtEnd = DateTime.Now;
                        //Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, strXml);
                        saveBookingResult = "400";
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("100", "Fail To Initialize API Service.");
                    dtEnd = DateTime.Now;
                    saveBookingResult = "100";
                }
            }

            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                Utils.SaveLog("BookingSave", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, strResult);
                strResult = Utils.ErrorXml("103", "General Error.");
                saveBookingResult = "103";
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                //replace cc number
                if (iPaymentCount > 0 && !string.IsNullOrEmpty(cvv_card) && !string.IsNullOrEmpty(document_number) && document_number.Length > 15)
                {
                    strXml = WrapCCNumber(strXml, document_number);
                }

                if (strResult.Contains("000"))
                    saveBookingResult = "000";
                else
                    saveBookingResult = strResult;

                Utils.SaveProcessLog("BookingSave", dtStart, DateTime.Now, strXml + "\n" + "booking_id: " + bookingId + "\n" + "SaveResult: " + saveBookingResult);
            }

            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string BookingGetItinerary(string strRecordLocator)
        {
            ServiceClient objClient = new ServiceClient();
            DateTime dtStart = DateTime.Now;
            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                string strParam = "strRecordLocator      : " + strRecordLocator + Environment.NewLine;
                Utils.SaveProcessLog("BookingGetItinerary", dtStart, DateTime.Now, strParam);
            }

            string strLanguageCode = string.Empty;
            if (Session["LanguageCode"] != null)
            {
                strLanguageCode = Session["LanguageCode"].ToString();
            }
            else
            {
                strLanguageCode = "EN";
            }
            return objClient.ItineraryRead(strRecordLocator, strLanguageCode, string.Empty, string.Empty);


        }
        [WebMethod(EnableSession = true)]
        public string GetAgency()
        {
            string strResult = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd;

            Agents objAgents = (Agents)Session["Agents"];
            if (objAgents != null && objAgents.Count > 0)
            {
                strResult = "<Agencies>" +
                                "<Agency>" +
                                    "<agency_code>" + objAgents[0].agency_code + "</agency_code>" +
                                    "<b2b_credit_card_payment_flag>" + objAgents[0].b2b_credit_card_payment_flag + "</b2b_credit_card_payment_flag>" +
                                    "<b2b_credit_agency_and_invoice_flag>" + objAgents[0].b2b_credit_agency_and_invoice_flag + "</b2b_credit_agency_and_invoice_flag>" +
                                    "<booking_payment>" + objAgents[0].booking_payment + "</booking_payment>" +
                                    "<agency_account>" + objAgents[0].agency_account + "</agency_account>" +
                                    "<agency_payment_type_rcd>" + objAgents[0].agency_payment_type_rcd + "</agency_payment_type_rcd>" +
                                    "<b2b_allow_service_flag>" + objAgents[0].b2b_allow_service_flag + "</b2b_allow_service_flag>" +
                                    "<process_baggage_tag_flag>" + objAgents[0].process_baggage_tag_flag + "</process_baggage_tag_flag>" +
                                "</Agency>" +
                            "</Agencies>";
            }
            else
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("102", "Service Need To Be Initialize");
                Utils.SaveLog("UserAgencyInformation", dtStart, dtEnd, strResult, string.Empty);
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("GetAgency", dtStart, DateTime.Now, string.Empty);
            }

            return strResult;
        }
        [WebMethod(EnableSession = true)]
        public string GetPassengerManifest(PassengersManifestRequest PassengersManifestRequest)
        {
            string strResult = "";
            string strManifest = "";
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd;
            clstikAeroWebService objService = new clstikAeroWebService();

            if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.GetPassengerManifest) == true)
            {
                try
                {
                    Agents objAgents = (Agents)Session["Agents"];
                    if (objAgents == null)
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        return strResult;
                    }

                    PassengersManifestRequest p = PassengersManifestRequest;
                    string username = System.Configuration.ConfigurationManager.AppSettings["apiUserName"];
                    string password = System.Configuration.ConfigurationManager.AppSettings["apiPassword"];

                    DateTime refDate = new DateTime(1900, 1, 1);
                    DateTime dateDepFrom = DateTime.MinValue;
                    DateTime dateDepTo = DateTime.MinValue;

                    bool bDateFrom = false;
                    if (!string.IsNullOrEmpty(p.departure_date_from))
                    {
                        bDateFrom = DateTime.TryParse(p.departure_date_from, out dateDepFrom);
                    }

                    bool bDateTo = false;
                    if (!string.IsNullOrEmpty(p.departure_date_to))
                    {
                        bDateTo = DateTime.TryParse(p.departure_date_to, out dateDepTo);
                    }

                    if (p.departure_date_from == null)
                    {
                        strResult = Utils.ErrorXml("V004", "Departure date from parameter is required.");
                        dtEnd = DateTime.Now;
                        Utils.SaveLog("GetPassengerManifest", dtStart, dtEnd, strResult, strManifest);
                    }
                    else if (bDateFrom && dateDepFrom < refDate)
                    {
                        strResult = Utils.ErrorXml("422", "Departure date from parameter is invalid range.");
                        dtEnd = DateTime.Now;
                        Utils.SaveLog("GetPassengerManifest", dtStart, dtEnd, strResult, strManifest);
                    }
                    else if (bDateTo && dateDepTo < refDate)
                    {
                        strResult = Utils.ErrorXml("423", "Departure date to parameter is invalid range.");
                        dtEnd = DateTime.Now;
                        Utils.SaveLog("GetPassengerManifest", dtStart, dtEnd, strResult, strManifest);
                    }
                    else if (string.IsNullOrEmpty(p.flight_number))
                    {
                        strResult = Utils.ErrorXml("114", "Invalid/Missing Flight Number");
                        dtEnd = DateTime.Now;
                        Utils.SaveLog("GetPassengerManifest", dtStart, dtEnd, strResult, strManifest);
                    }
                    else if (string.IsNullOrEmpty(p.airline_rcd))
                    {
                        strResult = Utils.ErrorXml("114", "Invalid/Missing Airline Code");
                        dtEnd = DateTime.Now;
                        Utils.SaveLog("GetPassengerManifest", dtStart, dtEnd, strResult, strManifest);
                    }
                    else
                    {
                        string dateFrom = p.departure_date_from;
                        string dateTo = !string.IsNullOrEmpty(p.departure_date_to) ? p.departure_date_to : dateFrom;

                        strResult = objService.GetPassengerManifest(
                            p.origin_rcd,
                            p.destination_rcd,
                            p.airline_rcd,
                            p.flight_number,
                            dateFrom,
                            dateTo,
                            username,
                            password,
                            p.bReturnServices,
                            p.bReturnBagTags,
                            p.bReturnRemarks,
                            p.bNotCheckedIn,
                            p.bCheckedIn,
                            p.bOffloaded,
                            p.bNoShow,
                            p.bInfants,
                            p.bConfirmed,
                            p.bWaitlisted,
                            p.bCancelled,
                            p.bStandby,
                            p.bIndividual,
                            p.bGroup,
                            p.bTransit);
                    }
                }
                catch (Exception ex)
                {
                    strResult = Utils.ErrorXml("103", "General Error");

                    dtEnd = DateTime.Now;
                    Utils.SaveLog("GetPassengerManifest", dtStart, dtEnd, ex.Message, strManifest);
                }
                finally
                {
                    objService = null;

                    //Save Process Log
                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                        Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
                    {
                        Utils.SaveProcessLog("GetPassengerManifest", dtStart, DateTime.Now, strManifest);
                    }
                }
            }
            else
            {
                strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                dtEnd = DateTime.Now;
            }
            return strResult;
        }

        #region Extended Method
        [WebMethod]
        public TikAeroWebAPI.Classes.LogonResponse BookingLogon(string recordLocator, string NameOrPhone, string strLanguageCode, string strAgencyCode)
        {
            DateTime dtStart = DateTime.Now;
            clstikAeroWebService objService = new clstikAeroWebService();
            TikAeroWebAPI.Classes.LogonResponse obj = new TikAeroWebAPI.Classes.LogonResponse();

            if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.BookingLogon) == true)
            {
                if (recordLocator.Length == 0)
                {
                    obj.ErrorCode = "604";
                    obj.ErrorMessage = "Record locator is required.";
                }
                else if (NameOrPhone.Length == 0)
                {
                    obj.ErrorCode = "605";
                    obj.ErrorMessage = "Name or Phone is required.";
                }
                else if (strLanguageCode.Length == 0)
                {
                    obj.ErrorCode = "606";
                    obj.ErrorMessage = "Language is required.";
                }
                else
                {
                    tikSystem.Web.Library.tikAeroWebService.WsWrapper objResult = objService.BookingLogon(recordLocator, NameOrPhone, strAgencyCode);

                    if (objResult != null)
                    {
                        if (objResult.ErrorCode.Equals("000"))
                        {
                            obj.ErrorCode = objResult.ErrorCode;
                            obj.ErrorMessage = objResult.ErrorMessage;

                            obj.record_locator = recordLocator.ToUpper();
                            if (System.Configuration.ConfigurationManager.AppSettings["CobUrl"] != null)
                            {
                                Library objLi = new Library();
                                obj.url = System.Configuration.ConfigurationManager.AppSettings["CobUrl"] + "?sn=" + objLi.GenerateCobToken(objResult.Header.booking_id.ToString(),
                                                                                                                                            "",
                                                                                                                                            objResult.Agencies[0].default_user_account_id.ToString(),
                                                                                                                                            objResult.Agencies[0].agency_code,
                                                                                                                                            strLanguageCode,
                                                                                                                                            Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["ShowHeader"]));
                            }
                            else
                            {
                                obj.ErrorCode = "601";
                                obj.ErrorMessage = "Can't return Booking Url";
                            }
                        }
                        else if (objResult.ErrorCode.Equals("101"))
                        {
                            obj.ErrorCode = "603";
                            obj.ErrorMessage = objResult.ErrorMessage;
                        }
                        else
                        {
                            obj.ErrorCode = "600";
                            obj.ErrorMessage = "Logon Failed";
                        }
                    }
                    else
                    {
                        obj.ErrorCode = "602";
                        obj.ErrorMessage = "No response from server.";
                    }
                }

                //Save Process Log
                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                    Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
                {
                    string strParam = "recordLocator    : " + recordLocator + Environment.NewLine +
                                      "NameOrPhone      : " + NameOrPhone + Environment.NewLine +
                                      "strLanguage      : " + strLanguageCode + Environment.NewLine +
                                      "strAgencyCode    : " + strAgencyCode + Environment.NewLine;

                    Utils.SaveProcessLog("BookingLogon", dtStart, DateTime.Now, strParam);
                }
            }
            else
            {
                obj.ErrorCode = "106";
                obj.ErrorMessage = "This funation call is not allowed.";
            }

            return obj;

        }

        [WebMethod(EnableSession = true)]
        public string GetSpecialServiceFee(string[] ssrCode,
                                           Guid[] bookingSegmentId)
        {
            string strResult = string.Empty;
            bool bSuccess = true;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            int iNumberOfInfant = 0;
            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.SpecialService) == true)
                {
                    BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];
                    Mappings mappings = (Mappings)Session["Mappings"];
                    Passengers passengers = (Passengers)Session["Passengers"];
                    Agents objAgents = (Agents)Session["Agents"];

                    if (objAgents != null && objAgents.Count > 0 && bookingHeader != null &&
                        mappings != null && mappings.Count > 0 &&
                        passengers != null && passengers.Count > 0)
                    {

                        string agencyCode = bookingHeader.agency_code;
                        string currencyCode = bookingHeader.currency_rcd;

                        if (ssrCode == null || ssrCode.Length == 0)
                        {
                            strResult = Utils.ErrorXml("701", "SSR code not found");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                        }
                        else if (bookingSegmentId == null || bookingSegmentId.Length == 0)
                        {
                            strResult = Utils.ErrorXml("702", "Flight information not found");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                        }
                        else
                        {
                            string strLanguageCode = string.Empty;
                            if (Session["LanguageCode"] != null)
                            {
                                strLanguageCode = Session["LanguageCode"].ToString();
                            }
                            else
                            {
                                strLanguageCode = "EN";
                            }
                            Fees fees = new Fees();
                            for (int i = 0; i < passengers.Count; i++)
                            {
                                if (passengers[i].passenger_type_rcd.Equals("INF"))
                                {
                                    iNumberOfInfant++;
                                }
                            }
                            // Fill Mapping information.
                            Mappings mp = new Mappings();
                            for (int i = 0; i < bookingSegmentId.Length; i++)
                            {
                                for (int j = 0; j < mappings.Count; j++)
                                {
                                    if (bookingSegmentId[i].Equals(mappings[j].booking_segment_id))
                                    {
                                        mp.Add(mappings[j]);
                                        break;
                                    }
                                }
                            }

                            Routes destnation = BookingUtil.CacheDestination();
                            Agent agent = new Agent();
                            agent.b2b_allow_service_flag = 1;

                            strResult = fees.SegmentFee(agent, destnation, bookingHeader, mp, ssrCode, null, passengers.Count, iNumberOfInfant, strLanguageCode, true, Convert.ToBoolean(bookingHeader.no_vat_flag));

                            //Convert to xml output format...
                            if (string.IsNullOrEmpty(strResult) == false)
                            {
                                using (StringReader srd = new StringReader(strResult))
                                {
                                    strResult = string.Empty;
                                    XPathDocument xmlDoc = new XPathDocument(srd);
                                    XPathNavigator nv = xmlDoc.CreateNavigator();

                                    StringBuilder stb = new StringBuilder();
                                    using (StringWriter stw = new StringWriter(stb))
                                    {
                                        XmlTextWriter objXmlWriter = new XmlTextWriter(stw);

                                        objXmlWriter.WriteStartElement("service");
                                        {
                                            foreach (XPathNavigator n in nv.Select("ServiceFees/*"))
                                            {
                                                objXmlWriter.WriteStartElement("service_fee");
                                                {
                                                    objXmlWriter.WriteStartElement("special_service_rcd");
                                                    {
                                                        objXmlWriter.WriteValue(n.Name);
                                                    }
                                                    objXmlWriter.WriteEndElement();
                                                    objXmlWriter.WriteStartElement("display_name");
                                                    {
                                                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "display_name"));
                                                    }
                                                    objXmlWriter.WriteEndElement();
                                                    objXmlWriter.WriteStartElement("origin_rcd");
                                                    {
                                                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "origin_rcd"));
                                                    }
                                                    objXmlWriter.WriteEndElement();
                                                    objXmlWriter.WriteStartElement("destination_rcd");
                                                    {
                                                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "destination_rcd"));
                                                    }
                                                    objXmlWriter.WriteEndElement();
                                                    objXmlWriter.WriteStartElement("od_origin_rcd");
                                                    {
                                                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "od_origin_rcd"));
                                                    }
                                                    objXmlWriter.WriteEndElement();
                                                    objXmlWriter.WriteStartElement("od_destination_rcd");
                                                    {
                                                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "od_destination_rcd"));
                                                    }
                                                    objXmlWriter.WriteEndElement();
                                                    objXmlWriter.WriteStartElement("booking_class_rcd");
                                                    {
                                                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "booking_class_rcd"));
                                                    }
                                                    objXmlWriter.WriteEndElement();
                                                    objXmlWriter.WriteStartElement("fare_code");
                                                    {
                                                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "fare_code"));
                                                    }
                                                    objXmlWriter.WriteEndElement();
                                                    objXmlWriter.WriteStartElement("airline_rcd");
                                                    {
                                                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "airline_rcd"));
                                                    }
                                                    objXmlWriter.WriteEndElement();
                                                    objXmlWriter.WriteStartElement("flight_number");
                                                    {
                                                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "flight_number"));
                                                    }
                                                    objXmlWriter.WriteEndElement();
                                                    objXmlWriter.WriteStartElement("departure_date");
                                                    {
                                                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "departure_date"));
                                                    }
                                                    objXmlWriter.WriteEndElement();
                                                    objXmlWriter.WriteStartElement("currency_rcd");
                                                    {
                                                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "currency_rcd"));
                                                    }
                                                    objXmlWriter.WriteEndElement();
                                                    objXmlWriter.WriteStartElement("fee_amount");
                                                    {
                                                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "fee_amount"));
                                                    }
                                                    objXmlWriter.WriteEndElement();
                                                    objXmlWriter.WriteStartElement("fee_amount_incl");
                                                    {
                                                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "fee_amount_incl"));
                                                    }
                                                    objXmlWriter.WriteEndElement();
                                                    objXmlWriter.WriteStartElement("total_fee_amount");
                                                    {
                                                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "total_fee_amount"));
                                                    }
                                                    objXmlWriter.WriteEndElement();
                                                    objXmlWriter.WriteStartElement("total_fee_amount_incl");
                                                    {
                                                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "total_fee_amount_incl"));
                                                    }
                                                    objXmlWriter.WriteEndElement();
                                                    objXmlWriter.WriteStartElement("service_on_request");
                                                    {
                                                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToByte(n, "service_on_request"));
                                                    }
                                                    objXmlWriter.WriteEndElement();
                                                }
                                                objXmlWriter.WriteEndElement();//service_fee
                                            }
                                        }
                                        objXmlWriter.WriteEndElement();//service
                                        objXmlWriter.Flush();
                                        objXmlWriter.Close();
                                    }
                                    strResult = stb.ToString();
                                }
                            }
                        }

                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }

                    if (bSuccess == false)
                    {
                        Utils.SaveLog("GetSpecialServiceFee", dtStart, dtEnd, strResult, "");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetSpecialServiceFee", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("GetSpecialServiceFee", dtStart, DateTime.Now, string.Empty);
            }
            return strResult;
        }
        [WebMethod(EnableSession = true)]
        public string GetSeatMap(string originRcd,
                                string destinationRcd,
                                Guid flightId,
                                string boardingClass,
                                string bookingClass)
        {
            string strResult = string.Empty;
            bool bSuccess = true;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            Agents objAgents = (Agents)Session["Agents"];

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.SeatMap) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        if (string.IsNullOrEmpty(originRcd))
                        {
                            strResult = Utils.ErrorXml("800", "Please supply origin");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                        }
                        else if (string.IsNullOrEmpty(destinationRcd))
                        {
                            strResult = Utils.ErrorXml("801", "Please supply destination");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                        }
                        else if (flightId.Equals(Guid.Empty))
                        {
                            strResult = Utils.ErrorXml("802", "Please supply flight Id");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                        }
                        else
                        {
                            string strLanguageCode = string.Empty;
                            if (Session["LanguageCode"] != null)
                            {
                                strLanguageCode = Session["LanguageCode"].ToString();
                            }
                            else
                            {
                                strLanguageCode = "EN";
                            }

                            ServiceClient objService = new ServiceClient();
                            DataSet ds = objService.GetSeatMap(originRcd, destinationRcd, flightId.ToString(), boardingClass, bookingClass, strLanguageCode);

                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                ds.DataSetName = "SeatMaps";
                                strResult = ds.GetXml();
                            }
                            else
                            {
                                strResult = Utils.ErrorXml("803", "No seat information found");
                                dtEnd = DateTime.Now;
                                bSuccess = false;
                            }
                        }

                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }

                    if (bSuccess == false)
                    {
                        Utils.SaveLog("GetSeatMap", dtStart, dtEnd, strResult, "");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetSeatMap", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                string strParam = "originRcd              : " + originRcd + Environment.NewLine +
                                  "destinationRcd         : " + destinationRcd + Environment.NewLine +
                                  "flightId               : " + flightId + Environment.NewLine +
                                  "boardingClass          : " + boardingClass + Environment.NewLine +
                                  "bookingClass           : " + bookingClass + Environment.NewLine;
                Utils.SaveProcessLog("GetSeatMap", dtStart, DateTime.Now, strParam);
            }

            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string GetBaggageFee(Guid bookingSegmentId,
                                    Guid passengerId,
                                    bool outBoundFlight)
        {
            Agents objAgents = (Agents)Session["Agents"];
            string strResult = string.Empty;
            string strAgencyCode = string.Empty;
            bool bSuccess = true;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.BaggageFee) == true)
                {
                    BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        strAgencyCode = objAgents[0].agency_code;
                        if (string.IsNullOrEmpty(strAgencyCode) == false)
                        {
                            if (string.IsNullOrEmpty(strAgencyCode))
                            {
                                strResult = Utils.ErrorXml("900", "Agency Code is required");
                                dtEnd = DateTime.Now;
                                bSuccess = false;
                            }
                            else if (bookingSegmentId.Equals(Guid.Empty))
                            {
                                strResult = Utils.ErrorXml("901", "Booking segment id is required");
                                dtEnd = DateTime.Now;
                                bSuccess = false;
                            }
                            else if (passengerId.Equals(Guid.Empty))
                            {
                                strResult = Utils.ErrorXml("902", "Passenger Id is required");
                                dtEnd = DateTime.Now;
                                bSuccess = false;
                            }
                            else
                            {
                                Mappings mappings = (Mappings)Session["Mappings"];
                                if (mappings != null)
                                {
                                    //Clear Bagggae Session.
                                    ClearBagggaeSession(outBoundFlight);

                                    Fees objFee = new Fees();
                                    string strLanguageCode = string.Empty;
                                    if (Session["LanguageCode"] != null)
                                    {
                                        strLanguageCode = Session["LanguageCode"].ToString();
                                    }
                                    else
                                    {
                                        strLanguageCode = "EN";
                                    }
                                    objFee.GetBaggageFeeOptions(mappings, bookingSegmentId, passengerId, strAgencyCode, strLanguageCode, 0, null, false, Convert.ToBoolean(bookingHeader.no_vat_flag));
                                    if (objFee != null && objFee.Count > 0)
                                    {
                                        if (outBoundFlight == true)
                                        {
                                            Session["BaggageOutBound"] = objFee;
                                        }
                                        else
                                        {
                                            Session["BaggageInBound"] = objFee;
                                        }

                                        strResult = objFee.GetBaggageFeeXml();
                                    }
                                    else
                                    {
                                        strResult = Utils.ErrorXml("904", "Baggage Fee not found");
                                        dtEnd = DateTime.Now;
                                        bSuccess = false;
                                    }
                                }
                                else
                                {
                                    strResult = Utils.ErrorXml("903", "No Mapping information found");
                                    dtEnd = DateTime.Now;
                                    bSuccess = false;
                                }
                            }

                        }
                        else
                        {
                            strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                        }

                        if (bSuccess == false)
                        {
                            Utils.SaveLog("GetSeatMap", dtStart, dtEnd, strResult, "");
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetSeatMap", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                string strParam = "bookingSegmentId         : " + bookingSegmentId + Environment.NewLine +
                                  "passengerId              : " + passengerId + Environment.NewLine +
                                  "outBoundFlight           : " + outBoundFlight + Environment.NewLine;
                Utils.SaveProcessLog("GetBaggageFee", dtStart, DateTime.Now, strParam);
            }
            return strResult;
        }
        [WebMethod(EnableSession = true)]
        public string AddBaggageFee(System.Collections.Generic.List<BaggageRequest> baggageRequest)
        {
            Agents objAgents = (Agents)Session["Agents"];
            string strAgencyCode = string.Empty;
            string strResult = string.Empty;

            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;

            bool bSuccess = true;
            bool bNewFeesCreate = false;
            BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];
            string AddBaggageResult = "";
            string booking_id = bookingHeader.booking_id.ToString();

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.BaggageFee) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        strAgencyCode = objAgents[0].agency_code;
                        if (string.IsNullOrEmpty(strAgencyCode) == false)
                        {
                            if (baggageRequest == null || baggageRequest.Count == 0)
                            {
                                strResult = Utils.ErrorXml("1000", "Invalid input parameter");
                                dtEnd = DateTime.Now;
                                bSuccess = false;
                                AddBaggageResult = "1000";
                            }
                            else if (baggageRequest == null || baggageRequest.Count > 2)
                            {
                                strResult = Utils.ErrorXml("1001", "Can't add more than two Items of baggage information.");
                                dtEnd = DateTime.Now;
                                bSuccess = false;
                                AddBaggageResult = "1001";
                            }
                            else
                            {
                                Fees fees = (Fees)Session["Fees"];
                                if (fees == null)
                                {
                                    bNewFeesCreate = true;
                                    fees = new Fees();
                                }

                                Fees objBaggageFee;
                                Fee f;
                                for (int i = 0; i < baggageRequest.Count; i++)
                                {
                                    //Read baggage fee list from session.
                                    if (baggageRequest[i].OutBoundFlight == true)
                                    {
                                        objBaggageFee = (Fees)Session["BaggageOutBound"];
                                    }
                                    else
                                    {
                                        objBaggageFee = (Fees)Session["BaggageInBound"];
                                    }

                                    if (objBaggageFee == null || objBaggageFee.Count == 0)
                                    {
                                        strResult = Utils.ErrorXml("1002", "GetBaggageFee have to be successfully call before we can process this call");
                                        dtEnd = DateTime.Now;
                                        bSuccess = false;
                                        AddBaggageResult = "1002";
                                        break;
                                    }
                                    else
                                    {

                                        for (int j = 0; j < objBaggageFee.Count; j++)
                                        {
                                            if (objBaggageFee[j].booking_segment_id.Equals(baggageRequest[i].BookingSegmentId) &&
                                                objBaggageFee[j].passenger_id.Equals(baggageRequest[i].PassengerId) &&
                                                objBaggageFee[j].number_of_units.Equals(baggageRequest[i].NumberOfUnit))
                                            {
                                                f = new Fee();

                                                f.booking_fee_id = Guid.NewGuid();
                                                f.passenger_id = baggageRequest[i].PassengerId;
                                                f.booking_segment_id = baggageRequest[i].BookingSegmentId;
                                                f.fee_id = objBaggageFee[j].fee_id;
                                                f.fee_rcd = objBaggageFee[j].fee_rcd;
                                                f.fee_category_rcd = objBaggageFee[j].fee_category_rcd;
                                                f.currency_rcd = objBaggageFee[j].currency_rcd;
                                                f.display_name = objBaggageFee[j].display_name;
                                                f.number_of_units = objBaggageFee[j].number_of_units;
                                                f.fee_amount = objBaggageFee[j].total_amount;
                                                f.fee_amount_incl = objBaggageFee[j].total_amount_incl;
                                                f.total_amount = 0;
                                                f.total_amount_incl = 0;
                                                f.vat_percentage = objBaggageFee[j].vat_percentage;
                                                f.agency_code = strAgencyCode;

                                                fees.Add(f);
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (bNewFeesCreate == true)
                                {
                                    Session["Fees"] = fees;
                                }
                            }
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                            AddBaggageResult = "100";
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                        AddBaggageResult = "100";
                    }

                    if (bSuccess == false)
                    {
                      //  Utils.SaveLog("AddBaggageFee", dtStart, dtEnd, strResult, "");
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("000", "Success Request Transaction");
                        AddBaggageResult = "000";
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                    AddBaggageResult = "106";
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("AddBaggageFee", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
                AddBaggageResult = "103";
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("AddBaggageFee", dtStart, DateTime.Now, XmlHelper.Serialize(baggageRequest, false) + "\n" + "AddBaggageResult: " + AddBaggageResult + "\n" + "booking_id:" + booking_id);
            }
            return strResult;
        }
        [WebMethod(EnableSession = true)]
        public string RemoveBaggageFee(System.Collections.Generic.List<BaggageRequest> baggageRequest)
        {
            Agents objAgents = (Agents)Session["Agents"];
            string strAgencyCode = string.Empty;
            string strResult = string.Empty;

            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;

            BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];
            string booking_id = bookingHeader.booking_id.ToString();

            bool bSuccess = true;

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.BaggageFee) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        strAgencyCode = objAgents[0].agency_code;
                        if (string.IsNullOrEmpty(strAgencyCode) == false)
                        {
                            Fees fees = (Fees)Session["Fees"];
                            Fees Baggage = (Fees)Session["BaggageOutBound"];

                            //Read bagggae fee from one of the available segment.
                            if (Baggage == null)
                            {
                                Baggage = (Fees)Session["BaggageInBound"];
                            }

                            if (Baggage != null && Baggage.Count > 0)
                            {
                                for (int i = 0; i < baggageRequest.Count; i++)
                                {
                                    //Read baggage fee list from session.
                                    for (int j = 0; j < fees.Count; j++)
                                    {
                                        if (fees[j].booking_segment_id.Equals(baggageRequest[i].BookingSegmentId) &&
                                            fees[j].passenger_id.Equals(baggageRequest[i].PassengerId) &&
                                            fees[j].fee_rcd.Equals(Baggage[0].fee_rcd) &&
                                            fees[j].number_of_units.Equals(baggageRequest[i].NumberOfUnit))
                                        {
                                            fees.RemoveAt(j);
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                strResult = Utils.ErrorXml("1002", "GetBaggageFee have to be successfully call before we can process this call");
                                dtEnd = DateTime.Now;
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                        }

                        if (bSuccess == false)
                        {
                            Utils.SaveLog("RemoveBaggageFee", dtStart, dtEnd, strResult, "");
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("000", "Success Request Transaction");
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("RemoveBaggageFee", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("RemoveBaggageFee", dtStart, DateTime.Now, XmlHelper.Serialize(baggageRequest, false) + "\n" + "booking_id:" + booking_id);
            }
            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string SelectSeat(System.Collections.Generic.List<TikAeroWebAPI.Classes.SeatRequest> request)
        {
            string strResult = string.Empty;
            bool bSuccess = true;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            Agents objAgents = (Agents)Session["Agents"];
            string booking_id = "";

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.SeatMap) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        if (request != null && request.Count > 0)
                        {
                            //Validate input
                            BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];
                            Fees fees = (Fees)Session["Fees"];
                            Mappings mappings = (Mappings)Session["Mappings"];
                            Itinerary itinerary = (Itinerary)Session["Itinerary"];
                            Passengers passengers = (Passengers)Session["Passengers"];
                            Services services = (Services)Session["Services"];
                            Remarks remarks = (Remarks)Session["Remarks"];

                            string agencyCode = objAgents[0].agency_code;
                            string languageCode = bookingHeader.language_rcd;
                            string currencyCode = bookingHeader.currency_rcd;
                            Guid bookingId = bookingHeader.booking_id;
                            booking_id = bookingId.ToString();

                            for (int i = 0; i < request.Count; i++)
                            {
                                if (request[i].BookingSegmentId.Equals(Guid.Empty))
                                {
                                    strResult = Utils.ErrorXml("1200", "One of the request supply an invalid BookingSegmentId");
                                    dtEnd = DateTime.Now;
                                    bSuccess = false;
                                    break;
                                }
                                else if (request[i].PassengerId.Equals(Guid.Empty))
                                {
                                    strResult = Utils.ErrorXml("1201", "One of the request supply an invalid PassengerId");
                                    dtEnd = DateTime.Now;
                                    bSuccess = false;
                                    break;
                                }
                            }

                            //When all validation pass do the seat assignment.
                            if (bSuccess == true)
                            {
                                #region Fill Seat Information to Mapping
                                //Fill Seat Information.
                                for (int i = 0; i < request.Count; i++)
                                {
                                    for (int j = 0; j < mappings.Count; j++)
                                    {
                                        if (request[i].PassengerId.Equals(mappings[j].passenger_id) &&
                                            request[i].BookingSegmentId.Equals(mappings[j].booking_segment_id))
                                        {
                                            mappings[j].seat_column = request[i].SeatColumn;
                                            mappings[j].seat_row = request[i].SeatRow;
                                            if (request[i].SeatRow == 0 && string.IsNullOrEmpty(request[i].SeatColumn))
                                            {
                                                mappings[j].seat_number = string.Empty;
                                            }
                                            else
                                            {
                                                mappings[j].seat_number = request[i].SeatRow.ToString() + request[i].SeatColumn;
                                            }
                                            mappings[j].seat_fee_rcd = request[i].SeatFeeRcd;
                                        }
                                    }
                                }
                                #endregion
                                if (fees == null)
                                {
                                    fees = new Fees();
                                    Session["Fees"] = fees;
                                }
                                //Calculate special service fee
                                if (fees.CalculateSeatFee(agencyCode, currencyCode, bookingHeader.booking_id.ToString(), bookingHeader, itinerary, passengers, services, remarks, mappings, languageCode, Convert.ToBoolean(bookingHeader.no_vat_flag)) == true)
                                {
                                    bSuccess = true;
                                }
                                else
                                {
                                    strResult = Utils.ErrorXml("1203", "Seat assignment failed");
                                    dtEnd = DateTime.Now;
                                    bSuccess = false;
                                }
                            }
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("1204", "No request information found");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                        }

                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }

                    //If failed write to log file.
                    if (bSuccess == false)
                    {
                        Utils.SaveLog("SelectSeat", dtStart, dtEnd, strResult, "");
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("000", "Success Request Transaction");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("SelectSeat", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }
            string seatInput = "";
            for(int i=0;i< request.Count;i++)
            {
                seatInput += request[i].BookingSegmentId + "," + request[i].PassengerId + "," + request[i].SeatRow + "," + request[i].SeatColumn + "\n";
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("SelectSeat", dtStart, DateTime.Now, seatInput + "\n" + "booking_id:" + booking_id);
            }

            return strResult;
        }
        [WebMethod(EnableSession = true)]
        public string AddSpecialService(System.Collections.Generic.List<SpecialServiceRequest> request)
        {
            string strResult = string.Empty;
            bool bSuccess = true;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            Agents objAgents = (Agents)Session["Agents"];
            string booking_id = "";

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.SpecialService) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        if (request != null && request.Count > 0)
                        {
                            //Validate input
                            BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];
                            Fees fees = (Fees)Session["Fees"];
                            Mappings mappings = (Mappings)Session["Mappings"];
                            Services services = (Services)Session["Services"];
                            Itinerary itinerary = (Itinerary)Session["Itinerary"];
                            Remarks remarks = (Remarks)Session["Remarks"];

                            string agencyCode = objAgents[0].agency_code;
                            string languageCode = bookingHeader.language_rcd;
                            string currencyCode = bookingHeader.currency_rcd;
                            Guid bookingId = bookingHeader.booking_id;
                            booking_id = bookingId.ToString();

                            for (int i = 0; i < request.Count; i++)
                            {
                                if (request[i].PassengerSegmentServiceId.Equals(Guid.Empty))
                                {
                                    strResult = Utils.ErrorXml("1300", "One of the request supply an invalid PassengerSegmentServiceId");
                                    dtEnd = DateTime.Now;
                                    bSuccess = false;
                                    break;
                                }
                                else if (request[i].PassengerId.Equals(Guid.Empty))
                                {
                                    strResult = Utils.ErrorXml("1302", "One of the request supply an invalid PassengerId");
                                    dtEnd = DateTime.Now;
                                    bSuccess = false;
                                    break;
                                }
                                else if (request[i].BookingSegmentId.Equals(Guid.Empty))
                                {
                                    strResult = Utils.ErrorXml("1303", "One of the request supply an invalid BookingSegmentId");
                                    dtEnd = DateTime.Now;
                                    bSuccess = false;
                                    break;
                                }
                                else if (string.IsNullOrEmpty(request[i].OriginRcd))
                                {
                                    strResult = Utils.ErrorXml("1304", "One of the request supply an invalid OriginRcd");
                                    dtEnd = DateTime.Now;
                                    bSuccess = false;
                                    break;
                                }
                                else if (string.IsNullOrEmpty(request[i].DestinationRcd))
                                {
                                    strResult = Utils.ErrorXml("1305", "One of the request supply an invalid DestinationRcd");
                                    dtEnd = DateTime.Now;
                                    bSuccess = false;
                                    break;
                                }
                                else if (string.IsNullOrEmpty(request[i].SpecialServiceRcd))
                                {
                                    strResult = Utils.ErrorXml("1306", "One of the request supply an invalid SpecialServiceRcd");
                                    dtEnd = DateTime.Now;
                                    bSuccess = false;
                                    break;
                                }
                                else if (string.IsNullOrEmpty(request[i].ServiceText))
                                {
                                    strResult = Utils.ErrorXml("1307", "One of the request supply an invalid ServiceText");
                                    dtEnd = DateTime.Now;
                                    bSuccess = false;
                                    break;
                                }
                                else if (SpecialServiceExist(services, request[i].SpecialServiceRcd, request[i].BookingSegmentId, request[i].PassengerId) == true)
                                {
                                    strResult = Utils.ErrorXml("1308", "Duplicate special service selected.");
                                    dtEnd = DateTime.Now;
                                    bSuccess = false;
                                    break;
                                }
                            }

                            //When all validation pass do the seat assignment.
                            if (bSuccess == true)
                            {
                                #region Fill Service
                                //Fill service object.
                                Service sService;
                                for (int i = 0; i < request.Count; i++)
                                {
                                    sService = new Service();
                                    sService.passenger_segment_service_id = request[i].PassengerSegmentServiceId;
                                    sService.passenger_id = request[i].PassengerId;
                                    sService.booking_segment_id = request[i].BookingSegmentId;
                                    sService.origin_rcd = request[i].OriginRcd;
                                    sService.destination_rcd = request[i].DestinationRcd;
                                    if (request[i].ServiceOnRequestFlag == true)
                                    {
                                        sService.special_service_status_rcd = "RQ";
                                    }
                                    else
                                    {
                                        //Pass segment status to service object.
                                        for (int j = 0; j < itinerary.Count; j++)
                                        {
                                            if (itinerary[j].booking_segment_id.Equals(sService.booking_segment_id))
                                            {
                                                sService.special_service_status_rcd = itinerary[j].segment_status_rcd;
                                                break;
                                            }
                                        }
                                    }
                                    sService.special_service_rcd = request[i].SpecialServiceRcd;
                                    sService.service_text = request[i].ServiceText;
                                    sService.number_of_units = request[i].NumberOfUnit;

                                    services.Add(sService);
                                }

                                #endregion

                                if (fees == null)
                                {
                                    fees = new Fees();
                                    Session["Fees"] = fees;
                                }
                                //Calculate special service fee
                                fees.CalculateSpecialServiceFee(agencyCode, currencyCode, bookingHeader, services, remarks, mappings, languageCode, Convert.ToBoolean(bookingHeader.no_vat_flag));
                                bSuccess = true;
                            }
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("1309", "No request information found");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                        }

                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }

                    //Return Message to client.
                    if (bSuccess == false)
                    {
                        Utils.SaveLog("AddSpecialService", dtStart, dtEnd, strResult, "");
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("000", "Success Request Transaction");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("AddSpecialService", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }
            string ssrInput = "";
            for (int i = 0; i < request.Count; i++)
            {
                ssrInput += request[i].SpecialServiceRcd + ",";
            }
                //Save Process Log
                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {

                Utils.SaveProcessLog("AddSpecialService", dtStart, DateTime.Now, ssrInput+ "\n" + "booking_id:" + booking_id);
            }

            return strResult;
        }
        [WebMethod(EnableSession = true)]
        public string RemoveSpecialService(Guid[] passengerSegmentServiceId)
        {
            Agents objAgents = (Agents)Session["Agents"];

            string strResult = string.Empty;
            bool bSuccess = true;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            int iCount = 0;
            BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];
            string booking_id = bookingHeader.booking_id.ToString();

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.SpecialService) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        if (passengerSegmentServiceId.Length > 0)
                        {
                            //Validate input
                            Fees fees = (Fees)Session["Fees"];
                            Services services = (Services)Session["Services"];

                            //When all validation pass do the seat assignment.
                            //Filed Local Fee to SOA Fee object.
                            #region Delete Service
                            for (int i = 0; i < passengerSegmentServiceId.Length; i++)
                            {
                                for (int j = 0; j < services.Count; j++)
                                {
                                    if (passengerSegmentServiceId[i].Equals(services[j].passenger_segment_service_id))
                                    {
                                        iCount = ++iCount;
                                        services.RemoveAt(j);
                                        break;
                                    }
                                }
                            }
                            #endregion

                            #region Delete Fee
                            for (int i = 0; i < passengerSegmentServiceId.Length; i++)
                            {
                                for (int j = 0; j < fees.Count; j++)
                                {
                                    if (passengerSegmentServiceId[i].Equals(fees[j].passenger_segment_service_id))
                                    {
                                        fees.RemoveAt(j);
                                        break;
                                    }
                                }
                            }
                            #endregion

                            if (iCount > 0)
                            {
                                bSuccess = true;
                            }
                            else
                            {
                                bSuccess = false;
                                strResult = Utils.ErrorXml("1400", "No passengerSegmentServiceId match the remove");
                            }
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("1401", "No Passenger Segment service found");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                        }

                        if (bSuccess == false)
                        {
                            Utils.SaveLog("RemoveSpecialService", dtStart, dtEnd, strResult, "");
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("000", "Success Request Transaction");
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }

            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("RemoveSpecialService", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Library objLi = new Library();
                Utils.SaveProcessLog("RemoveSpecialService", dtStart, DateTime.Now, passengerSegmentServiceId.ToString() + "\n" + "booking_id:" + booking_id);
            }

            return strResult;
        }
        [WebMethod(EnableSession = true)]
        public string ClientLogon(string clientNumber, string password)
        {
            string strResult = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            bool bSuccess = true;
            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.ClientProfile) == true)
                {
                    if (string.IsNullOrEmpty(clientNumber))
                    {
                        strResult = Utils.ErrorXml("1500", "Client number is required");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }
                    else if (string.IsNullOrEmpty(password))
                    {
                        strResult = Utils.ErrorXml("1501", "Password is required");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }
                    else
                    {
                        Clients objClient = new Clients();
                        Client client = objClient.ClientLogon(clientNumber, password);
                        if (client != null)
                        {
                            //Save client information to session
                            Session["ClientProfile"] = client;
                            bSuccess = true;
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("1503", "Login Failed");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                        }
                    }

                    if (bSuccess == false)
                    {
                        Utils.SaveLog("ClientLogon", dtStart, dtEnd, strResult, "");
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("000", "Success Request Transaction");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("ClientLogon", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                string strParam = "clientNumber    : " + clientNumber + Environment.NewLine +
                                  "password        : " + password + Environment.NewLine;
                Utils.SaveProcessLog("ClientLogon", dtStart, DateTime.Now, strParam);
            }

            return strResult;
        }
        [WebMethod(EnableSession = true)]
        public string GetClientPassenger()
        {
            string strResult = string.Empty;
            bool bSuccess = true;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.ClientProfile) == true)
                {
                    Client sClient = (Client)Session["ClientProfile"];
                    if (sClient != null)
                    {
                        Clients objClients = new Clients();
                       
                        objClients.ReadClientPassenger(string.Empty, string.Empty, sClient.client_number.ToString());
                       
                        if (objClients != null && objClients.Count > 0)
                        {
                            strResult = objClients.GetPassengerProfileXml(sClient);
                            bSuccess = true;
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("1601", "Login Failed");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("1600", "Client Logon is required");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }

                    //Return Message to client.
                    if (bSuccess == false)
                    {
                        Utils.SaveLog("GetClientPassenger", dtStart, dtEnd, strResult, "");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetClientPassenger", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("GetClientPassenger", dtStart, DateTime.Now, string.Empty);
            }

            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string GetPointHistory(string strClientProfileId,
                                         string strTransactionFrom,
                                         string strTransactionTo,
                                         string strAllPoint)
        {
            string strResult = string.Empty;
            bool bSuccess = true;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.ClientProfile) == true)
                {
                    Client sClient = (Client)Session["ClientProfile"];
                    if (sClient != null)
                    {
                        string strOrigin = string.Empty,
                            strDestination = string.Empty,
                            strAirline = string.Empty,
                            strFlight = string.Empty,
                            strSegmentType = string.Empty,
                            strPassengerProfileId = string.Empty,
                            strVendor = string.Empty,
                            strCreditDebit = string.Empty;
                        DateTime dateFlightFrom = DateTime.MinValue;
                        DateTime dateFlightTo = DateTime.MinValue;
                        DateTime dateTransactionFrom = DateTime.MinValue;
                        DateTime dateTransactionTo = DateTime.MinValue;
                        DateTime dateExpiryFrom = DateTime.MinValue;
                        DateTime dateExpiryTo = DateTime.MinValue;
                        DateTime dateVoidFrom = DateTime.MinValue;
                        DateTime dateVoidTo = DateTime.MinValue;
                        int iBatch = 0;
                        bool bAllVoid = false;
                        bool bAllExpired = false;
                        bool bAuto = false;
                        bool bManual = false;
                        bool bAllPoint = false;

                        int intDateFrom = 0;
                        int intDateTo = 0;

                        if (strTransactionFrom.Length == 8 && int.TryParse(strTransactionFrom, out intDateFrom))
                        {
                            string a = strTransactionFrom.Substring(0, 4);
                            string b = strTransactionFrom.Substring(4, 2);
                            string c = strTransactionFrom.Substring(6, 2);
                            strTransactionFrom = string.Join("-", new string[] { a, b, c });
                        }

                        if (strTransactionTo.Length == 8 && int.TryParse(strTransactionTo, out intDateTo))
                        {
                            string a = strTransactionTo.Substring(0, 4);
                            string b = strTransactionTo.Substring(4, 2);
                            string c = strTransactionTo.Substring(6, 2);
                            strTransactionTo = string.Join("-", new string[] { a, b, c });
                        }

                        //DateTime.TryParse(strFlightFrom, out dateFlightFrom);
                        //DateTime.TryParse(strFlightTo, out dateFlightTo);
                        DateTime.TryParse(strTransactionFrom, out dateTransactionFrom);
                        DateTime.TryParse(strTransactionTo, out dateTransactionTo);
                        //DateTime.TryParse(strExpiryFrom, out dateExpiryFrom);
                        //DateTime.TryParse(strExpiryTo, out dateExpiryTo);
                        //DateTime.TryParse(strVoidFrom, out dateVoidFrom);
                        //DateTime.TryParse(strVoidTo, out dateVoidTo);
                        //int.TryParse(strBatch, out iBatch);
                        //bool.TryParse(strAllVoid, out bAllVoid);
                        //bool.TryParse(strAllExpired, out bAllExpired);
                        //bool.TryParse(strAuto, out bAuto);
                        //bool.TryParse(strManual, out bManual);
                        bool.TryParse(strAllPoint, out bAllPoint);

                        if (dateTransactionFrom.Equals(DateTime.MinValue))
                        {
                            strResult = Utils.ErrorXml("1701", "Transaction Empty.");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                        }
                        else
                        {
                        Clients objClients = new Clients();
                        DataSet ds = objClients.GetTransaction(strOrigin, strDestination, strAirline, strFlight, strSegmentType, strClientProfileId, strPassengerProfileId, strVendor, strCreditDebit, dateFlightFrom, dateFlightTo, dateTransactionFrom, dateTransactionTo, dateExpiryFrom, dateExpiryTo, dateVoidFrom, dateVoidTo, iBatch, bAllVoid, bAllExpired, bAuto, bManual, bAllPoint);
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            strResult = ds.GetXml();
                            bSuccess = true;
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("1701", "Transaction Empty.");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                        }
                    }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("1600", "Client Logon is required");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }

                    //Return Message to client.
                    if (bSuccess == false)
                    {
                        Utils.SaveLog("GetTransaction", dtStart, dtEnd, strResult, "");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetTransaction", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("GetClientPassenger", dtStart, DateTime.Now, string.Empty);
            }

            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string GetInfantCapacity(string flightId, string originRcd, string destinationRcd, string boardingClassRcd)
        {
            //Int32 infantCapacity = 0;
            string strResult = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            string strLanguageCode = string.Empty;

            ServiceClient obj = new ServiceClient();

            try
            {
                if (string.IsNullOrEmpty(flightId))
                {
                    strResult = Utils.ErrorXml("9110", "FlightId is required");
                    dtEnd = DateTime.Now;
                    Utils.SaveLog("GetInfantCapacity", dtStart, dtEnd, strResult, string.Empty);
                }
                else if (string.IsNullOrEmpty(originRcd))
                {
                    strResult = Utils.ErrorXml("9112", "OriginRcd is required");
                    dtEnd = DateTime.Now;
                    Utils.SaveLog("GetInfantCapacity", dtStart, dtEnd, strResult, string.Empty);
                }
                else if (string.IsNullOrEmpty(destinationRcd))
                {
                    strResult = Utils.ErrorXml("9113", "DestinationRcd is required");
                    dtEnd = DateTime.Now;
                    Utils.SaveLog("GetInfantCapacity", dtStart, dtEnd, strResult, string.Empty);
                }
                else if (string.IsNullOrEmpty(boardingClassRcd))
                {
                    strResult = Utils.ErrorXml("9120", "BoardingClassRcd is required");
                    dtEnd = DateTime.Now;
                    Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, strResult, string.Empty);
                }
                else
                {
                    strResult = obj.GetInfantCapacity(flightId, originRcd, destinationRcd, boardingClassRcd).ToString();
                }

            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetClientPassenger", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                //bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("GetInfantCapacity", dtStart, DateTime.Now, string.Empty);
            }

            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string ClientLogout()
        {
            string strResult = string.Empty;
            bool bSuccess = true;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.ClientProfile) == true)
                {
                    Client sClient = (Client)Session["ClientProfile"];
                    if (sClient != null)
                    {
                        Session.Remove("ClientProfile");
                        bSuccess = true;
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("1600", "Client Logon is required");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }

                    //Return Message to client.
                    if (bSuccess == true)
                    {
                        strResult = Utils.ErrorXml("000", "Success Request Transaction");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }

            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("ClientLogout", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("ClientLogout", dtStart, DateTime.Now, string.Empty);
            }
            return strResult;
        }
        [WebMethod(EnableSession = true)]
        public string GetFlownBookings()
        {
            string strResult = string.Empty;
            bool bSuccess = true;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.ClientProfile) == true)
                {
                    Client sClient = (Client)Session["ClientProfile"];
                    if (sClient != null)
                    {
                        Booking objBooking = new Booking();
                        strResult = objBooking.GetFlownBookings(sClient.client_profile_id);
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("1600", "Client Logon is required");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }

                    //Return Message to client.
                    if (bSuccess == false)
                    {
                        Utils.SaveLog("GetFlownBookings", dtStart, dtEnd, strResult, "");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }

            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetFlownBookings", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("GetFlownBookings", dtStart, DateTime.Now, string.Empty);
            }

            return strResult;
        }
        [WebMethod(EnableSession = true)]
        public string GetActiveBookings()
        {
            string strResult = string.Empty;
            bool bSuccess = true;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.ClientProfile) == true)
                {
                    Client sClient = (Client)Session["ClientProfile"];
                    if (sClient != null)
                    {
                        Booking objBooking = new Booking();
                        strResult = objBooking.GetActiveBookings(sClient.client_profile_id);
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("1600", "Client Logon is required");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }

                    //Return Message to client.
                    if (bSuccess == false)
                    {
                        Utils.SaveLog("GetActiveBookings", dtStart, dtEnd, strResult, "");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetActiveBookings", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("GetActiveBookings", dtStart, DateTime.Now, string.Empty);
            }
            return strResult;
        }
        [WebMethod(EnableSession = true)]
        public string GetTitles()
        {
            Agents objAgents = (Agents)Session["Agents"];
            string strAgencyCode = string.Empty;
            string strResult = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.GetTitles) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        strAgencyCode = objAgents[0].agency_code;
                        if (string.IsNullOrEmpty(strAgencyCode) == false)
                        {
                            string strLanguageCode = string.Empty;
                            if (Session["LanguageCode"] != null)
                            {
                                strLanguageCode = Session["LanguageCode"].ToString();
                            }
                            else
                            {
                                strLanguageCode = "EN";
                            }

                            ServiceClient srvClient = new ServiceClient();
                            Titles titles = srvClient.GetPassengerTitles(strLanguageCode);

                            if (titles != null && titles.Count > 0)
                            {
                                strResult = titles.GetXml();
                            }
                            else
                            {
                                strResult = string.Empty;
                            }
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                            dtEnd = DateTime.Now;
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetTitles", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("GetTitles", dtStart, DateTime.Now, string.Empty);
            }

            return strResult;
        }
        [WebMethod(EnableSession = true)]
        public string GetCountry()
        {
            Agents objAgents = (Agents)Session["Agents"];
            string strResult = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.GetCountry) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        string strLanguageCode = string.Empty;
                        if (Session["LanguageCode"] != null)
                        {
                            strLanguageCode = Session["LanguageCode"].ToString();
                        }
                        else
                        {
                            strLanguageCode = "EN";
                        }

                        ServiceClient srvClient = new ServiceClient();
                        Countries countries = srvClient.GetCountry(strLanguageCode);

                        if (countries != null && countries.Count > 0)
                        {
                            strResult = countries.GetXml();
                        }
                        else
                        {
                            strResult = string.Empty;
                        }

                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }

            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetCountry", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("GetCountry", dtStart, DateTime.Now, string.Empty);
            }

            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string GetCurrency()
        {
            Agents objAgents = (Agents)Session["Agents"];
            string strResult = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.GetCurrency) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        string strLanguageCode = string.Empty;
                        if (Session["LanguageCode"] != null)
                        {
                            strLanguageCode = Session["LanguageCode"].ToString();
                        }
                        else
                        {
                            strLanguageCode = "EN";
                        }

                        ServiceClient srvClient = new ServiceClient();
                        Currencies currencies = srvClient.GetCurrencies(strLanguageCode);

                        if (currencies != null && currencies.Count > 0)
                        {

                            strResult = currencies.GetXml();
                        }
                        else
                        {
                            strResult = string.Empty;
                        }

                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }

            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetCurrency", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("GetCurrency", dtStart, DateTime.Now, string.Empty);
            }

            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string GetLanguage()
        {
            Agents objAgents = (Agents)Session["Agents"];
            string strResult = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.GetLanguage) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        string strLanguageCode = string.Empty;
                        if (Session["LanguageCode"] != null)
                        {
                            strLanguageCode = Session["LanguageCode"].ToString();
                        }
                        else
                        {
                            strLanguageCode = "EN";
                        }

                        ServiceClient srvClient = new ServiceClient();
                        Languages languages = srvClient.GetLanguages(strLanguageCode);
                        if (languages != null && languages.Count > 0)
                        {

                            strResult = languages.GetXml();
                        }
                        else
                        {
                            strResult = string.Empty;
                        }

                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }

            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetLanguage", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("GetLanguage", dtStart, DateTime.Now, string.Empty);
            }

            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string GetCreditCardType()
        {
            Agents objAgents = (Agents)Session["Agents"];
            string strResult = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.GetCreditCardType) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        string strLanguageCode = string.Empty;
                        if (Session["LanguageCode"] != null)
                        {
                            strLanguageCode = Session["LanguageCode"].ToString();
                        }
                        else
                        {
                            strLanguageCode = "EN";
                        }
                        FormOfPaymentSubTypes formOfPaymentSubTypes = new FormOfPaymentSubTypes();
                        formOfPaymentSubTypes.Read("CC", strLanguageCode);
                        if (formOfPaymentSubTypes.Count > 0)
                        {
                            strResult = formOfPaymentSubTypes.GetXml();
                        }
                        else
                        {
                            strResult = string.Empty;
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetCCType", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("GetCreditCardType", dtStart, DateTime.Now, string.Empty);
            }

            return strResult;
        }
        [WebMethod(EnableSession = true)]
        public string GetCreditCardFee()
        {
            Agents objAgents = (Agents)Session["Agents"];
            BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];
            string strResult = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.GetCreditCardFee) == true)
                {
                    if (objAgents == null && objAgents.Count == 0)
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                    }
                    else if (bookingHeader == null)
                    {
                        strResult = Utils.ErrorXml("1700", "No Booking Information Found");
                        dtEnd = DateTime.Now;
                        Utils.SaveLog("GetCreditCardFee", dtStart, dtEnd, strResult, string.Empty);
                    }
                    else
                    {
                        Fees objFees = new Fees();
                        objFees.GetFormOfPaymentSubTypeFee("CC", string.Empty, bookingHeader.currency_rcd, bookingHeader.agency_code, DateTime.MinValue);
                        if (objFees != null && objFees.Count > 0)
                        {
                            strResult = objFees.GetXml();
                        }
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetCreditCardFee", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("GetCreditCardFee", dtStart, DateTime.Now, string.Empty);
            }

            return strResult;
        }
        [WebMethod(EnableSession = true)]
        public string GetFeeDefinition(string feeRcd, string bookingClass, string fareCode, string originRcd, string destinationRcd, string flightNumber)
        {
            Agents objAgents = (Agents)Session["Agents"];
            BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];
            string strResult = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.GetFeeDefinition) == true)
                {
                    if (objAgents == null || objAgents.Count == 0)
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                    }
                    else if (bookingHeader == null)
                    {
                        strResult = Utils.ErrorXml("1800", "No Booking Information Found");
                        dtEnd = DateTime.Now;
                        Utils.SaveLog("GetFeeDefinition", dtStart, dtEnd, strResult, string.Empty);
                    }
                    else
                    {
                        string strLanguageCode = string.Empty;
                        if (Session["LanguageCode"] != null)
                        {
                            strLanguageCode = Session["LanguageCode"].ToString();
                        }
                        else
                        {
                            strLanguageCode = "EN";
                        }
                        Fees objFees = new Fees();
                        objFees.GetFeeDefinition(feeRcd,
                                                bookingHeader.currency_rcd,
                                                bookingHeader.agency_code,
                                                bookingClass,
                                                fareCode,
                                                originRcd,
                                                destinationRcd,
                                                flightNumber,
                                                DateTime.MinValue,
                                                strLanguageCode,
                                                Convert.ToBoolean(bookingHeader.no_vat_flag));

                        if (objFees != null && objFees.Count > 0)
                        {
                            strResult = objFees.GetXml();
                        }
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetFeeDefinition", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                string strParam = "feeRcd           : " + feeRcd + Environment.NewLine +
                                  "bookingClass     : " + bookingClass + Environment.NewLine +
                                  "fareCode         : " + fareCode + Environment.NewLine +
                                  "originRcd        : " + originRcd + Environment.NewLine +
                                  "flightNumber     : " + flightNumber + Environment.NewLine;
                Utils.SaveProcessLog("GetFeeDefinition", dtStart, DateTime.Now, strParam);
            }
            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string AddFee(System.Collections.Generic.List<CustomFeeRequest> customFeeRequest)
        {
            Agents objAgents = (Agents)Session["Agents"];
            BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];
            Fees fees = (Fees)Session["Fees"];

            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;

            string strResult = string.Empty;
            bool bSuccess = true;
            string booking_id = bookingHeader.booking_id.ToString();

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.AddCustomFee) == true)
                {
                    if (objAgents == null || objAgents.Count == 0)
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                    }
                    else if (bookingHeader == null)
                    {
                        strResult = Utils.ErrorXml("1800", "No Booking Information Found");
                        dtEnd = DateTime.Now;
                        Utils.SaveLog("GetFeeDefinition", dtStart, dtEnd, strResult, string.Empty);
                    }
                    else
                    {

                        if (customFeeRequest == null && customFeeRequest.Count > 0)
                        {
                            strResult = Utils.ErrorXml("1000", "Invalid input parameter");
                        }
                        else
                        {
                            //Loop to validate all field.
                            for (int i = 0; i < customFeeRequest.Count; i++)
                            {
                                if (customFeeRequest[i].PassengerId.Equals(Guid.Empty))
                                {
                                    strResult = Utils.ErrorXml("1900", "PassengerId is required");
                                    bSuccess = false;
                                    break;
                                }
                                else if (customFeeRequest[i].BookingSegmentId.Equals(Guid.Empty))
                                {
                                    strResult = Utils.ErrorXml("1901", "BookingSegmentId is required");
                                    bSuccess = false;
                                    break;
                                }
                                else if (customFeeRequest[i].FeeId.Equals(Guid.Empty))
                                {
                                    strResult = Utils.ErrorXml("1902", "FeeId is required");
                                    bSuccess = false;
                                    break;
                                }
                                else if (string.IsNullOrEmpty(customFeeRequest[i].FeeCategoryRcd))
                                {
                                    strResult = Utils.ErrorXml("1903", "FeeCategoryRcd is required");
                                    bSuccess = false;
                                    break;
                                }
                                else if (string.IsNullOrEmpty(customFeeRequest[i].FeeRcd))
                                {
                                    strResult = Utils.ErrorXml("1904", "FeeRcd is required");
                                    bSuccess = false;
                                    break;
                                }
                                else if (string.IsNullOrEmpty(customFeeRequest[i].CurrencyRcd))
                                {
                                    strResult = Utils.ErrorXml("1905", "CurrencyRcd is required");
                                    bSuccess = false;
                                    break;
                                }
                                else if (string.IsNullOrEmpty(customFeeRequest[i].ChargeCurrencyRcd))
                                {
                                    strResult = Utils.ErrorXml("1906", "ChargeCurrencyRcd is required");
                                    bSuccess = false;
                                    break;
                                }
                            }

                            //Fill Success fee information.
                            if (bSuccess == true)
                            {
                                for (int i = 0; i < customFeeRequest.Count; i++)
                                {
                                    string strLanguageCode = string.Empty;
                                    if (Session["LanguageCode"] != null)
                                    {
                                        strLanguageCode = Session["LanguageCode"].ToString();
                                    }
                                    else
                                    {
                                        strLanguageCode = "EN";
                                    }

                                    //Fill in fee information to booking fee
                                    Fee fee = new Fee();
                                    if (fees == null)
                                    {
                                        fees = new Fees();
                                        Session["Fees"] = fees;

                                    }

                                    fee.booking_id = bookingHeader.booking_id;
                                    fee.booking_fee_id = Guid.NewGuid();
                                    fee.passenger_id = customFeeRequest[i].PassengerId;
                                    fee.booking_segment_id = customFeeRequest[i].BookingSegmentId;
                                    fee.fee_id = customFeeRequest[i].FeeId;
                                    fee.fee_category_rcd = customFeeRequest[i].FeeCategoryRcd;
                                    fee.fee_rcd = customFeeRequest[i].FeeRcd;
                                    fee.agency_code = objAgents[0].agency_code;
                                    fee.currency_rcd = customFeeRequest[i].CurrencyRcd;
                                    fee.fee_amount = customFeeRequest[i].FeeAmount;
                                    fee.fee_amount_incl = customFeeRequest[i].FeeAmountIncl;
                                    fee.vat_percentage = customFeeRequest[i].VatPercentage;
                                    fee.charge_currency_rcd = customFeeRequest[i].ChargeCurrencyRcd;
                                    fee.charge_amount = customFeeRequest[i].ChargeAmount;
                                    fee.charge_amount_incl = customFeeRequest[i].ChargeAmountIncl;
                                    if (customFeeRequest[i].NumberOfUnits == 0)
                                    {
                                        fee.number_of_units = 1;
                                    }
                                    else
                                    {
                                        fee.number_of_units = customFeeRequest[i].NumberOfUnits;
                                    }
                                    if (string.IsNullOrEmpty(customFeeRequest[i].Units))
                                    {
                                        fee.units = "1";
                                    }
                                    else
                                    {
                                        fee.units = customFeeRequest[i].Units;
                                    }
                                    //optional
                                    fee.document_number = customFeeRequest[i].DocumentNumber;
                                    fee.document_date_time = customFeeRequest[i].DocumentDateTime;
                                    fee.comment = customFeeRequest[i].Comment;
                                    fee.external_reference = customFeeRequest[i].ExternalReference;
                                    fee.vendor_rcd = customFeeRequest[i].VendorRcd;
                                    fee.od_origin_rcd = customFeeRequest[i].OdOriginRcd;
                                    fee.od_destination_rcd = customFeeRequest[i].OdDestinationRcd;

                                    fee.new_record = true;
                                    fees.Add(fee);
                                    fee = null;
                                }

                                strResult = Utils.ErrorXml("000", "Success Request Transaction");
                            }

                        }
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("AddFee", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, XmlHelper.Serialize(customFeeRequest, false));
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                string strParam = string.Empty;
                foreach(Fee f in fees)
                {
                    strParam += f.fee_rcd + ",";
                }
                //if (customFeeRequest != null)
                //{
                //    strParam = XmlHelper.Serialize(customFeeRequest, false);
                //}

                Utils.SaveProcessLog("AddFee", dtStart, DateTime.Now, strParam + "\n" + "booking_id:" + booking_id);
            }
            return strResult;
        }
        [WebMethod(EnableSession = true)]
        public string RemoveFee(System.Collections.Generic.List<RemoveFeeRequest> removeFeeRequest)
        {
            Agents objAgents = (Agents)Session["Agents"];
            string strAgencyCode = string.Empty;
            string strResult = string.Empty;

            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;

            BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];
            string booking_id = bookingHeader.booking_id.ToString();

            bool bSuccess = true;

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.AddCustomFee) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        strAgencyCode = objAgents[0].agency_code;
                        if (string.IsNullOrEmpty(strAgencyCode) == false)
                        {
                            if (removeFeeRequest != null && removeFeeRequest.Count > 0)
                            {
                                Fees fees = (Fees)Session["Fees"];

                                //Remove fee.
                                for (int i = 0; i < removeFeeRequest.Count; i++)
                                {
                                    for (int j = 0; j < fees.Count; j++)
                                    {
                                        if (fees[j].booking_fee_id.Equals(Guid.Empty) == false &&
                                            fees[j].booking_fee_id.Equals(removeFeeRequest[i].BookingFeeId))// &&fees[j].new_record == true)
                                        {
                                            fees.RemoveAt(j);
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                strResult = Utils.ErrorXml("2000", "Remove Fee request parameter is required.");
                                dtEnd = DateTime.Now;
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                        }

                        if (bSuccess == false)
                        {
                            Utils.SaveLog("RemoveFee", dtStart, dtEnd, strResult, XmlHelper.Serialize(removeFeeRequest, false));
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("000", "Success Request Transaction");
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("RemoveFee", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("RemoveFee", dtStart, DateTime.Now, XmlHelper.Serialize(removeFeeRequest, false) + "\n" + "booking_id:" + booking_id);
            }
            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string AddRemark(System.Collections.Generic.List<RemarkRequest> remarkRequest)
        {
            Agents objAgents = (Agents)Session["Agents"];
            BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];
            Remarks remarks = (Remarks)Session["Remarks"];

            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;

            string strResult = string.Empty;
            bool bSuccess = true;

            string booking_id = bookingHeader.booking_id.ToString();

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.AddRemark) == true)
                {
                    if (objAgents == null || objAgents.Count == 0)
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                    }
                    else if (bookingHeader == null)
                    {
                        strResult = Utils.ErrorXml("1800", "No Booking Information Found");
                        dtEnd = DateTime.Now;
                        Utils.SaveLog("GetRemarkDefinition", dtStart, dtEnd, strResult, string.Empty);
                    }
                    else
                    {

                        if (remarkRequest == null && remarkRequest.Count > 0)
                        {
                            strResult = Utils.ErrorXml("1000", "Invalid input parameter");
                        }
                        else
                        {
                            //Loop to validate all field.
                            for (int i = 0; i < remarkRequest.Count; i++)
                            {
                                if (string.IsNullOrEmpty(remarkRequest[i].RemarkTypeRcd))
                                {
                                    strResult = Utils.ErrorXml("3000", "Remark Type Rcd is required");
                                    bSuccess = false;
                                    break;
                                }
                                else if (string.IsNullOrEmpty(remarkRequest[i].RemarkText))
                                {
                                    strResult = Utils.ErrorXml("3001", "Remark Text is required");
                                    bSuccess = false;
                                    break;
                                }

                            }

                            //Fill Success fee information.
                            if (bSuccess == true)
                            {
                                for (int i = 0; i < remarkRequest.Count; i++)
                                {
                                    string strLanguageCode = string.Empty;
                                    if (Session["LanguageCode"] != null)
                                    {
                                        strLanguageCode = Session["LanguageCode"].ToString();
                                    }
                                    else
                                    {
                                        strLanguageCode = "EN";
                                    }

                                    //Fill in fee information to booking fee
                                    Guid userID = new Guid(Session["UserId"].ToString());
                                    Remark remark = new Remark();
                                    if (remarks == null)
                                    {
                                        remarks = new Remarks();
                                        Session["Remarks"] = remarks;

                                    }

                                    //System pass parameter.
                                    remark.booking_remark_id = Guid.NewGuid();
                                    remark.booking_id = bookingHeader.booking_id;
                                    remark.agency_code = objAgents[0].agency_code;

                                    //User pass parameter.
                                    remark.client_profile_id = remarkRequest[i].ClientProfileId;
                                    remark.remark_type_rcd = remarkRequest[i].RemarkTypeRcd;
                                    remark.remark_text = remarkRequest[i].RemarkText;
                                    remark.update_by = userID;
                                    remark.update_date_time = DateTime.Now;
                                    remark.create_by = userID;
                                    remark.create_date_time = DateTime.Now;
                                    remark.nickname = remarkRequest[i].NickName;
                                    remark.protected_flag = Convert.ToByte(remarkRequest[i].ProtectedFlag);
                                    remark.warning_flag = Convert.ToByte(remarkRequest[i].WarningFlag);
                                    remark.process_message_flag = Convert.ToByte(remarkRequest[i].ProcessMessageFlag);

                                    remarks.Add(remark);
                                    remark = null;
                                }

                                strResult = Utils.ErrorXml("000", "Success Request Transaction");
                            }

                        }
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("AddRemark", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, XmlHelper.Serialize(remarkRequest, false));
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                string strParam = string.Empty;
                foreach(RemarkRequest r in remarkRequest)
                {
                    strParam += r.RemarkTypeRcd + ",";
                }
                //if (remarkRequest == null)
                //{
                //    strParam = XmlHelper.Serialize(remarkRequest, false);
                //}

                Utils.SaveProcessLog("AddRemark", dtStart, DateTime.Now, strParam + "\n" + "booking_id:" + booking_id);
            }
            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string RemoveRemark(System.Collections.Generic.List<RemoveRemarkRequest> removeRemarkRequest)
        {
            Agents objAgents = (Agents)Session["Agents"];
            string strAgencyCode = string.Empty;
            string strResult = string.Empty;

            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;

            bool bSuccess = true;

            BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];
            string booking_id = bookingHeader.booking_id.ToString();

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.AddRemark) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        strAgencyCode = objAgents[0].agency_code;
                        if (string.IsNullOrEmpty(strAgencyCode) == false)
                        {
                            if (removeRemarkRequest != null && removeRemarkRequest.Count > 0)
                            {
                                Remarks remarks = (Remarks)Session["Remarks"];

                                //Remove fee.
                                for (int i = 0; i < removeRemarkRequest.Count; i++)
                                {
                                    for (int j = 0; j < remarks.Count; j++)
                                    {
                                        if (remarks[j].booking_remark_id.Equals(Guid.Empty) == false &&
                                            remarks[j].booking_remark_id.Equals(removeRemarkRequest[i].BookingRemarkId))// &&fees[j].new_record == true)
                                        {
                                            remarks.RemoveAt(j);
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                strResult = Utils.ErrorXml("4000", "Remove Remark request parameter is required.");
                                dtEnd = DateTime.Now;
                                bSuccess = false;
                            }
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                        }

                        if (bSuccess == false)
                        {
                            Utils.SaveLog("RemoveRemark", dtStart, dtEnd, strResult, XmlHelper.Serialize(removeRemarkRequest, false));
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("000", "Success Request Transaction");
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("RemoveRemark", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("RemoveRemark", dtStart, DateTime.Now, XmlHelper.Serialize(removeRemarkRequest, false) + "\n" + "booking_id:" + booking_id);
            }
            return strResult;
        }


        [WebMethod(EnableSession = true)] //System.Collections.Generic.List<VoucherTemplateListRequest>
        public string VoucherTemplateList(VoucherTemplateListRequest voucherTemplateListRequest)
        {
            string strResult = string.Empty;
            bool bSuccess = true;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            Agents objAgents = (Agents)Session["Agents"];

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.Voucher) == true)
                {

                    if (objAgents != null)
                    {

                        VoucherTemplates objVoucherTemplates = new VoucherTemplates();
                        string strLanguageCode = string.Empty;
                        if (Session["LanguageCode"] != null)
                        {
                            strLanguageCode = Session["LanguageCode"].ToString();
                        }
                        else
                        {
                            strLanguageCode = "EN";
                        }
                        strResult = objVoucherTemplates.VoucherTemplateList(voucherTemplateListRequest.VoucherTemplateId, voucherTemplateListRequest.VoucherTemplate, voucherTemplateListRequest.FromDate, voucherTemplateListRequest.ToDate, false, voucherTemplateListRequest.Status, strLanguageCode);
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }

                    //Return Message to client.
                    if (bSuccess == false)
                    {
                        Utils.SaveLog("VoucherTemplateList", dtStart, dtEnd, strResult, "");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("VoucherTemplateList", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("VoucherTemplateList", dtStart, DateTime.Now, string.Empty);
            }
            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string ReadVoucher(VoucherReadRequest voucherReadRequest)
        {

            string strResult = string.Empty;
            bool bSuccess = true;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            Agents objAgents = (Agents)Session["Agents"];

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.Voucher) == true)
                {

                    if (objAgents != null)
                    {

                        Vouchers objVouchers = new Vouchers();
                        string strLanguageCode = string.Empty;
                        if (Session["LanguageCode"] != null)
                        {
                            strLanguageCode = Session["LanguageCode"].ToString();
                        }
                        else
                        {
                            strLanguageCode = "EN";
                        }
                        strResult = objVouchers.ReadVoucher(voucherReadRequest.VoucherId, voucherReadRequest.VoucherNumber);

                        if (!string.IsNullOrEmpty(strResult))
                        {
                            XPathDocument xmlDoc = new XPathDocument(new StringReader(strResult));
                            XPathNavigator nv = xmlDoc.CreateNavigator();
                            if (nv.Select("Voucher/Details").Count == 0)
                            {
                                strResult = Utils.ErrorXml("207", "Invalid Voucher");
                                dtEnd = DateTime.Now;
                                bSuccess = false;
                            }
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }

                    //Return Message to client.
                    if (bSuccess == false)
                    {
                        Utils.SaveLog("VoucherReadRequest", dtStart, dtEnd, strResult, "");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("VoucherReadRequest", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("VoucherReadRequest", dtStart, DateTime.Now, string.Empty);
            }
            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string EmailVoucher(VoucherReadRequest voucherReadRequest)
        {

            string strResult = string.Empty;
            bool bSuccess = true;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            Agents objAgents = (Agents)Session["Agents"];

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.Voucher) == true)
                {

                    if (objAgents != null)
                    {

                        Vouchers objVouchers = new Vouchers();
                        string strLanguageCode = string.Empty;
                        if (Session["LanguageCode"] != null)
                        {
                            strLanguageCode = Session["LanguageCode"].ToString();
                        }
                        else
                        {
                            strLanguageCode = "EN";
                        }
                        strResult = objVouchers.ReadVoucher(voucherReadRequest.VoucherId, voucherReadRequest.VoucherNumber);
                        ServiceClient objClient = new ServiceClient();


                        //**************************Transform and send mail ************************************

                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }

                    //Return Message to client.
                    if (bSuccess == false)
                    {
                        Utils.SaveLog("VoucherTemplateList", dtStart, dtEnd, strResult, "");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("VoucherReadRequest", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("VoucherReadRequest", dtStart, DateTime.Now, string.Empty);
            }
            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string VoidVoucher(VoucherVoidRequest voucherVoidRequest)
        {

            string strResult = string.Empty;
            bool bSuccess = true;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            Agents objAgents = (Agents)Session["Agents"];

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.Voucher) == true)
                {

                    if (objAgents != null)
                    {

                        Vouchers objVouchers = new Vouchers();
                        Guid UserID = Guid.Empty;
                        if (Session["UserId"] != null)
                        {
                            UserID = new Guid(Convert.ToString(Session["UserId"]));
                            strResult = objVouchers.Void(voucherVoidRequest.VoucherId, UserID, dtStart) ? Utils.ErrorXml("000", "Voucher valid") : Utils.ErrorXml("6000", "Voucher invalid");
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                        }

                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }

                    //Return Message to client.
                    if (bSuccess == false)
                    {
                        Utils.SaveLog("VoucherTemplateList", dtStart, dtEnd, strResult, "");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("VoucherReadRequest", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("VoucherReadRequest", dtStart, DateTime.Now, string.Empty);
            }
            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string SaveVoucher(SaveVoucherRequest saveVoucherRequest)
        {
            string strResult = string.Empty;

            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            Agents objAgents = (Agents)Session["Agents"];



            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.Voucher) == true)
                {
                    if (objAgents == null || objAgents.Count == 0)
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                    }
                    else
                    {

                        if (saveVoucherRequest.Voucher == null)  //bookingRequest.Payment == null
                        {
                            strResult = Utils.ErrorXml("1000", "Invalid input parameter");
                        }
                        else
                        {
                            Payments pmsClass;

                            Guid userId = new Guid(Session["UserId"].ToString());
                            Guid voucherId = Guid.NewGuid();
                            if (saveVoucherRequest.Payment != null)
                            {
                                VoucherPaymentRequest pm = saveVoucherRequest.Payment;

                                pmsClass = new Payments();
                                Payment pmClass = new Payment();
                                if (objAgents[0].b2b_credit_card_payment_flag == 1)
                                {
                                    string strFOP = string.Empty;
                                    strFOP = pm.form_of_payment_rcd.Trim();
                                    if (strFOP == "CC")
                                    {
                                        //Credit Card Payment.

                                        ////Start Input Section
                                        pmClass.booking_payment_id = pm.booking_payment_id;
                                        pmClass.agency_code = objAgents[0].agency_code;
                                        pmClass.payment_amount = pm.payment_amount;
                                        pmClass.currency_rcd = pm.currency_rcd;
                                        pmClass.receive_payment_amount = pm.payment_amount;
                                        pmClass.receive_currency_rcd = pm.currency_rcd;
                                        pmClass.form_of_payment_rcd = pm.form_of_payment_rcd;
                                        pmClass.form_of_payment_subtype_rcd = pm.form_of_payment_subtype_rcd;
                                        pmClass.name_on_card = pm.NameOnCard;
                                        pmClass.document_number = pm.CreditCardNumber;
                                        pmClass.cvv_code = pm.CCV;
                                        if (!String.IsNullOrEmpty(pm.IssueMonth))
                                        {
                                            pmClass.issue_month = Convert.ToInt32(pm.IssueMonth);
                                        }
                                        if (!String.IsNullOrEmpty(pm.IssueYear))
                                        {
                                            pmClass.issue_year = Convert.ToInt32(pm.IssueYear);
                                        }
                                        pmClass.issue_number = pm.IssueNumber.Trim();
                                        pmClass.expiry_month = Convert.ToInt32(pm.ExpiryMonth);
                                        pmClass.expiry_year = Convert.ToInt32(pm.ExpiryYear);
                                        pmClass.address_line1 = pm.Addr1.Trim();
                                        pmClass.street = pm.Street.Trim();
                                        pmClass.state = pm.State.Trim();
                                        pmClass.city = pm.City.Trim();
                                        pmClass.country_rcd = pm.Country.Trim();
                                        pmClass.zip_code = pm.ZipCode.Trim();
                                        pmClass.payment_by = userId;
                                        pmClass.ip_address = DataHelper.GetClientIpAddress();
                                        pmClass.payment_type_rcd = "SALES";
                                        pmClass.receive_payment_amount = pm.payment_amount;

                                        pmsClass.Add(pmClass);

                                        //Address2
                                        //County

                                    }
                                    else
                                    {
                                        pmsClass = null;
                                    }
                                }
                                else
                                {
                                    pmsClass = null;
                                }
                            }
                            else
                            {
                                pmsClass = null;
                            }


                            VoucherTemplateRequest vc = saveVoucherRequest.Voucher;
                            Vouchers objVouchers = new Vouchers();
                            Voucher objVoucher = new Voucher();

                            objVoucher.voucher_template_id = vc.voucher_template_id;
                            objVoucher.form_of_payment_rcd = vc.form_of_payment_rcd;
                            objVoucher.form_of_payment_subtype_rcd = vc.form_of_payment_subtype_rcd;
                            objVoucher.currency_rcd = vc.currency_rcd;
                            objVoucher.valid_to_date = vc.valid_to_date;
                            objVoucher.valid_days = vc.valid_days;
                            objVoucher.valid_from_date = vc.valid_from_date;
                            objVoucher.display_name = vc.display_name;
                            objVoucher.status_code = vc.status_code;
                            objVoucher.origins = vc.origins;
                            objVoucher.destinations = vc.destinations;
                            objVoucher.passenger_segments = vc.passenger_segments;
                            objVoucher.voucher_use_code = vc.voucher_use_code;
                            objVoucher.recipient_only_flag = vc.recipient_only_flag;
                            objVoucher.discount_percentage = vc.discount_percentage;
                            objVoucher.valid_for_class = vc.valid_for_class;
                            objVoucher.ticket_flag = vc.ticket_flag;
                            objVoucher.seat_fee_flag = vc.seat_fee_flag;
                            objVoucher.other_fee_flag = vc.other_fee_flag;
                            objVoucher.b2c_flag = vc.b2c_flag;
                            objVoucher.b2b_flag = vc.b2b_flag;
                            objVoucher.b2e_flag = vc.b2e_flag;
                            objVoucher.airline_flag = vc.airline_flag;
                            objVoucher.fare_only_flag = vc.fare_only_flag;
                            objVoucher.voucher_value = vc.voucher_value;
                            objVoucher.charge_amount = vc.charge_amount;
                            objVoucher.multiple_payment_flag = vc.multiple_payment_flag;

                            objVoucher.voucher_id = voucherId;


                            objVoucher.voucher_status_rcd = "OPEN";
                            //objVoucher.create_date_time = DateTime.Now;
                            // objVoucher.update_date_time = vc.update_date_time;
                            objVoucher.refundable_flag = vc.refundable_flag;
                            objVoucher.percentage_flag = vc.percentage_flag;
                            objVoucher.single_flight_flag = vc.single_flight_flag;
                            objVoucher.single_passenger_flag = vc.single_passenger_flag;
                            objVoucher.recipient_name = vc.recipient_name;
                            objVoucher.agency_code = objAgents[0].agency_code;
                            objVoucher.currency_rcd = vc.currency_rcd;
                            objVoucher.voucher_password = vc.voucher_password;
                            objVoucher.expiry_date_time = vc.expiry_date_time;
                            objVouchers.Add(objVoucher);


                            SetCreateUpdateInformation(userId, null, null, null, null, null, null, null, pmsClass, null, objVouchers);
                            if (pmsClass != null)
                            {
                                if (pmsClass[0].currency_rcd == objVouchers[0].currency_rcd)
                                {

                                    strResult = objVouchers.PaymentCreditCard(pmsClass);
                                    if (strResult.Length > 0)
                                    {

                                        XPathDocument xmlDoc = new XPathDocument(new StringReader(strResult));
                                        XPathNavigator nv = xmlDoc.CreateNavigator();
                                        Library objLi = new Library();
                                        if (nv.Select("NewDataSet/Payments").Count > 0)
                                        {
                                            XPathNavigator n = nv.SelectSingleNode("NewDataSet/Payments");

                                            string retCode = objLi.getXPathNodevalue(n, "ResponseCode", Library.xmlReturnType.value);
                                            if (retCode == "APPROVED")
                                            {
                                                strResult = Utils.ErrorXml("000", "Success Request Transaction", voucherId.ToString());
                                            }
                                            else
                                            {
                                                strResult = Utils.ErrorXml("1000", "Save fail");
                                            }

                                        }
                                        else
                                        {
                                            strResult = Utils.ErrorXml("1000", "Save fail");
                                        }
                                    }
                                    else
                                    {
                                        strResult = Utils.ErrorXml("1000", "Save fail");
                                    }
                                }
                                else
                                {
                                    strResult = Utils.ErrorXml("1000", "Payment's currency and voucher's currency aren't match");
                                }

                            }
                            else
                            {
                                if (objVouchers.Save())
                                {
                                    strResult = Utils.ErrorXml("000", "Success Request Transaction", voucherId.ToString());
                                }
                                else
                                {
                                    strResult = Utils.ErrorXml("1000", "Save fail");
                                }
                            }



                        }
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("VoucherSave", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, XmlHelper.Serialize(saveVoucherRequest, false));
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                //string strParam = string.Empty;
                //if (voucherSaveRequest == null)
                //{
                //    strParam = XmlHelper.Serialize(voucherSaveRequest, false);
                //}

                Utils.SaveProcessLog("VoucherSave", dtStart, DateTime.Now, XmlHelper.Serialize(saveVoucherRequest, false));
            }
            return strResult;
        }

        private string ValidateVoucher(List<ValidateVoucherRequest> validateVoucherRequests, int iPaymentCount, ref Payments objPaymentsInput)
        {
            string strResult = string.Empty;
            string strVoucherXml = string.Empty;
            string strVoucherPassword = string.Empty;
            string strVoucherCurrency = string.Empty;
            string strVoucherStatus = string.Empty;
            string strVoucherNumber = string.Empty;
            string strVoucherNumbers = string.Empty;
            decimal dVoucherBalance = 0;
            //decimal dVoucherValue = 0;
            //decimal dVoucherPaymentTotal = 0;
            bool bDuplicate = false;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            Library objLi = new Library();
            Vouchers vouchers = new Vouchers();
            Voucher objVoucher;
            Payment objPaymentInput;

            Agents objAgents = (Agents)Session["Agents"];
            BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];
            Mappings mappings = (Mappings)Session["Mappings"];
            Payments payments = (Payments)Session["Payments"];
            Fees fees = (Fees)Session["Fees"];
            Quotes quotes = (Quotes)Session["Quotes"];

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.Voucher) == true)
                {
                    if (objAgents != null)
                    {
                        if (bookingHeader != null && mappings != null && fees != null && payments != null & quotes != null)
                        {
                            foreach (ValidateVoucherRequest objValidate in validateVoucherRequests)
                            {
                                if (objValidate.VoucherNumber.Length == 10)
                                {
                                    bDuplicate = false;
                                    Vouchers objVouchers = new Vouchers();
                                    strVoucherXml = objVouchers.ReadVoucher(Guid.Empty, Convert.ToDouble(objValidate.VoucherNumber));

                                    if (!string.IsNullOrEmpty(strVoucherXml))
                                    {
                                        XPathDocument xmlDoc = new XPathDocument(new StringReader(strVoucherXml));
                                        XPathNavigator nv = xmlDoc.CreateNavigator();

                                        if (nv.Select("Voucher/Details").Count > 0)
                                        {
                                            foreach (XPathNavigator n in nv.Select("Voucher/Details"))
                                            {
                                                strVoucherNumber = XmlHelper.XpathValueNullToEmpty(n, "voucher_number");
                                                strVoucherPassword = XmlHelper.XpathValueNullToEmpty(n, "voucher_password");
                                                strVoucherCurrency = XmlHelper.XpathValueNullToEmpty(n, "currency_rcd");
                                                strVoucherStatus = XmlHelper.XpathValueNullToEmpty(n, "voucher_status_rcd");

                                                if (strVoucherPassword != objValidate.VoucherPassword)
                                                {
                                                    //invalid voucher
                                                    strResult = Utils.ErrorXml("7002", "Voucher password not matches.");
                                                    dtEnd = DateTime.Now;
                                                }
                                                else if (strVoucherCurrency != bookingHeader.currency_rcd)
                                                {
                                                    //invalid currency
                                                    strResult = Utils.ErrorXml("7003", "Voucher currency not matches.");
                                                    dtEnd = DateTime.Now;
                                                }
                                                else if (strVoucherStatus.ToUpper() != "OPEN")
                                                {
                                                    //invalid voucher status
                                                    strResult = Utils.ErrorXml("7004", "Voucher status is not open.");
                                                    dtEnd = DateTime.Now;
                                                }
                                                else if (strVoucherNumbers.IndexOf(strVoucherNumber) >= 0)
                                                {
                                                    //duplicate voucher
                                                    strResult = Utils.ErrorXml("7005", "Duplicate vouchers.");
                                                    dtEnd = DateTime.Now;
                                                }
                                                else
                                                {
                                                    objVoucher = (Voucher)XmlHelper.Deserialize(n.OuterXml.Replace("Details", "Voucher"), typeof(Voucher));
                                                    vouchers.Add(objVoucher);

                                                    objPaymentInput = new Payment();
                                                    objPaymentInput.voucher_payment_id = XmlHelper.XpathValueNullToGUID(n, "voucher_id");
                                                    objPaymentInput.document_number = XmlHelper.XpathValueNullToEmpty(n, "voucher_number");
                                                    objPaymentInput.form_of_payment_rcd = XmlHelper.XpathValueNullToEmpty(n, "form_of_payment_rcd");
                                                    objPaymentInput.form_of_payment_subtype_rcd = XmlHelper.XpathValueNullToEmpty(n, "form_of_payment_subtype_rcd");
                                                    objPaymentInput.payment_amount = objLi.ReadVoucherAmount(vouchers, objPaymentInput.voucher_payment_id);

                                                    dVoucherBalance += objPaymentInput.payment_amount;
                                                    strVoucherNumbers += strVoucherNumber + "|";

                                                    if (objPaymentsInput.Count > 0)
                                                    {
                                                        foreach (Payment input in objPaymentsInput)
                                                        {
                                                            if (input.document_number.Equals(objPaymentInput.document_number))
                                                            {
                                                                bDuplicate = true;
                                                            }
                                                        }
                                                        if (!bDuplicate)
                                                        {
                                                            objPaymentsInput.Add(objPaymentInput);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        objPaymentsInput.Add(objPaymentInput);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            strResult = Utils.ErrorXml("514", "Voucher not found.");
                                            dtEnd = DateTime.Now;
                                        }
                                    }
                                    else
                                    {
                                        strResult = Utils.ErrorXml("514", "Voucher not found.");
                                        dtEnd = DateTime.Now;
                                    }
                                }
                                else
                                {
                                    strResult = Utils.ErrorXml("514", "Voucher not found.");
                                    dtEnd = DateTime.Now;
                                }
                            }

                            if (strResult.Equals(string.Empty))
                            {
                                decimal dOutStandingBalance = objLi.CalOutStandingBalance(quotes, fees, payments);
                                if (dVoucherBalance - dOutStandingBalance < 0 && ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.MultipleFOP) == false)
                                {
                                    //Voucher amount is not enough.
                                    strResult = Utils.ErrorXml("7006", "Voucher amount is not enough.");
                                    dtEnd = DateTime.Now;
                                }
                                else if (dVoucherBalance - dOutStandingBalance < 0 && validateVoucherRequests.Count == iPaymentCount)
                                {
                                    //Voucher amount is not enough.
                                    strResult = Utils.ErrorXml("7006", "Voucher amount is not enough.");
                                    dtEnd = DateTime.Now;
                                }
                                //else
                                //{
                                //    //First Sort voucher payment from less amont to greater amount before Add credit payment.
                                //    objPaymentsInput.Sort("payment_amount", GenericComparer.SortOrderEnum.Ascending);
                                //}
                            }
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                            dtEnd = DateTime.Now;
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                    }

                    //Return Message to client.
                    if (!string.IsNullOrEmpty(strResult))
                    {
                        Utils.SaveLog("ValidateVoucher", dtStart, dtEnd, strResult, "");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This function call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("ValidateVoucher", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                //bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("ValidateVoucher", dtStart, DateTime.Now, string.Empty);
            }
            return strResult;
        }

        // Add remark to existing booking
        [WebMethod(EnableSession = true)]
        public string AddRemarkToBooking(RemarkRequest remarkRequest)
        {
            Agents objAgents = (Agents)Session["Agents"];
            // BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];

            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;

            string strResult = string.Empty;

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.AddRemark) == true)
                {
                    if (objAgents == null || objAgents.Count == 0 || Session["UserId"] == null)
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                    }
                    else
                    {

                        if (remarkRequest == null)
                        {
                            strResult = Utils.ErrorXml("1000", "Invalid input parameter");
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(remarkRequest.RemarkTypeRcd))
                            {
                                strResult = Utils.ErrorXml("3000", "Remark Type Rcd is required");
                            }
                            else
                            {

                                if (string.IsNullOrEmpty(remarkRequest.RemarkText))
                                {
                                    strResult = Utils.ErrorXml("3001", "Remark Text is required");
                                }
                                else
                                {
                                    if ((remarkRequest.BookingId == null) || (Guid.Empty == remarkRequest.BookingId))
                                    {
                                        strResult = Utils.ErrorXml("3003", "Booking id is required");
                                    }
                                    else
                                    {
                                        string strLanguageCode = string.Empty;
                                        if (Session["LanguageCode"] != null)
                                        {
                                            strLanguageCode = Session["LanguageCode"].ToString();
                                        }
                                        else
                                        {
                                            strLanguageCode = "EN";
                                        }

                                        //Fill in fee information to booking fee
                                        Remarks remarks = new Remarks();
                                        Remark remark = new Remark();
                                        Guid userID = new Guid(Session["UserId"].ToString());
                                        string strRemarkId = string.Empty;

                                        //System pass parameter.
                                        if (remarkRequest.BookingRemarkId.Equals(Guid.Empty))
                                        {
                                            remark.booking_remark_id = Guid.NewGuid();
                                        }
                                        else
                                        {
                                            remark.booking_remark_id = remarkRequest.BookingRemarkId;
                                        }
                                        remark.booking_id = remarkRequest.BookingId;
                                        remark.agency_code = objAgents[0].agency_code;

                                        //User pass parameter.
                                        remark.client_profile_id = remarkRequest.ClientProfileId;
                                        remark.remark_type_rcd = remarkRequest.RemarkTypeRcd;
                                        remark.remark_text = remarkRequest.RemarkText;
                                        remark.update_by = userID;
                                        remark.update_date_time = DateTime.Now;
                                        remark.create_by = userID;
                                        remark.create_date_time = DateTime.Now;
                                        remark.nickname = remarkRequest.NickName;
                                        remark.protected_flag = Convert.ToByte(remarkRequest.ProtectedFlag);
                                        remark.warning_flag = Convert.ToByte(remarkRequest.WarningFlag);
                                        remark.process_message_flag = Convert.ToByte(remarkRequest.ProcessMessageFlag);

                                        strRemarkId = remarks.RemarkAdd(remark, userID);

                                        remark = null;
                                        if (string.IsNullOrEmpty(strRemarkId) == false)
                                        {
                                            strResult = Utils.ErrorXml("000", "Success Request Transaction", strRemarkId);
                                        }
                                        else
                                        {
                                            strResult = Utils.ErrorXml("3004", "Failed adding remark.", strRemarkId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This function call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("AddRemarkToBooking", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, XmlHelper.Serialize(remarkRequest, false));
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                string strParam = string.Empty;
                if (remarkRequest == null)
                {
                    strParam = XmlHelper.Serialize(remarkRequest, false);
                }

                Utils.SaveProcessLog("AddRemarkToBooking", dtStart, DateTime.Now, strParam);
            }
            return strResult;
        }
        // Delete remark from existing booking
        [WebMethod(EnableSession = true)]
        public string DeleteRemarkFromBooking(RemoveRemarkRequest removeRemarkRequest)
        {
            Agents objAgents = (Agents)Session["Agents"];
            string strAgencyCode = string.Empty;
            string strResult = string.Empty;

            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.AddRemark) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        strAgencyCode = objAgents[0].agency_code;
                        if (string.IsNullOrEmpty(strAgencyCode) == false)
                        {
                            if (removeRemarkRequest != null && removeRemarkRequest.BookingRemarkId != Guid.Empty)
                            {
                                Remarks remarks = new Remarks();
                                bool bSuccess = remarks.RemarkDelete(removeRemarkRequest.BookingRemarkId);
                                if (bSuccess == false)
                                {
                                    strResult = Utils.ErrorXml("4001", "Delete remark failed.");
                                    Utils.SaveLog("RemarkDelete", dtStart, dtEnd, strResult, XmlHelper.Serialize(removeRemarkRequest, false));
                                }
                                else
                                {
                                    strResult = Utils.ErrorXml("000", "Success Request Transaction");
                                }
                            }
                            else
                            {
                                strResult = Utils.ErrorXml("4000", "Remove Remark request parameter is required.");
                                dtEnd = DateTime.Now;
                            }
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                            dtEnd = DateTime.Now;
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("DeleteRemarkFromBooking", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, XmlHelper.Serialize(removeRemarkRequest, false));
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("DeleteRemarkFromBooking", dtStart, DateTime.Now, XmlHelper.Serialize(removeRemarkRequest, false));
            }
            return strResult;
        }
        // Save and edit existing remark
        [WebMethod(EnableSession = true)]
        public string UpdateBookingRemark(updateRemarkRequest remarkRequest)
        {
            Agents objAgents = (Agents)Session["Agents"];
            Remarks remarks;

            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;

            string strResult = string.Empty;
            bool bSuccess = true;

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.AddRemark) == true)
                {
                    if (objAgents == null || objAgents.Count == 0 || Session["UserId"] == null)
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                    }
                    else
                    {
                        Guid userID = new Guid(Session["UserId"].ToString());
                        if (remarkRequest == null && remarkRequest.remarkUpdateRequest.Count > 0)
                        {
                            strResult = Utils.ErrorXml("1000", "Invalid input parameter");
                        }
                        else
                        {
                            //Loop to validate all field.

                            foreach (RemarkBase updateRemark in remarkRequest.remarkUpdateRequest)
                            {

                                if (string.IsNullOrEmpty(updateRemark.RemarkTypeRcd))
                                {
                                    strResult = Utils.ErrorXml("3000", "Remark Type Rcd is required");
                                    bSuccess = false;
                                    break;
                                }
                                else if (string.IsNullOrEmpty(updateRemark.RemarkText))
                                {
                                    strResult = Utils.ErrorXml("3001", "Remark Text is required");
                                    bSuccess = false;
                                    break;
                                }
                                else if (updateRemark.BookingRemarkId.Equals(Guid.Empty))
                                {
                                    strResult = Utils.ErrorXml("3002", "Booking Remark Id is required");
                                    bSuccess = false;
                                    break;
                                }
                                else if (remarkRequest.BookingId.Equals(Guid.Empty))
                                {
                                    strResult = Utils.ErrorXml("3003", "Booking Id is required");
                                    bSuccess = false;
                                    break;
                                }

                            }

                            //Fill Success fee information.
                            if (bSuccess == true)
                            {
                                remarks = new Remarks();
                                foreach (RemarkBase updateRemark in remarkRequest.remarkUpdateRequest)
                                {
                                    //Fill in fee information to booking fee
                                    Remark remark = new Remark();

                                    //string remarkStr = remarks.RemarkRead(String.Empty, remarkRequest.BookingId.ToString(), String.Empty, Double.MinValue, false);
                                    //remarks.Fill(remarkStr);
                                    //remark = remarks[0];


                                    //System pass parameter.
                                    remark.booking_remark_id = updateRemark.BookingRemarkId;
                                    remark.booking_id = remarkRequest.BookingId;
                                    remark.agency_code = objAgents[0].agency_code;

                                    //User pass parameter.
                                    remark.client_profile_id = updateRemark.ClientProfileId;
                                    remark.remark_type_rcd = updateRemark.RemarkTypeRcd;
                                    remark.remark_text = updateRemark.RemarkText;
                                    if (string.IsNullOrEmpty(updateRemark.NickName))
                                    {
                                        remark.nickname = string.Empty;
                                    }
                                    else
                                    {
                                        remark.nickname = updateRemark.NickName;
                                    }

                                    remark.protected_flag = Convert.ToByte(updateRemark.ProtectedFlag);
                                    remark.warning_flag = Convert.ToByte(updateRemark.WarningFlag);
                                    remark.process_message_flag = Convert.ToByte(updateRemark.ProcessMessageFlag);
                                    remark.update_by = userID;
                                    remark.update_date_time = DateTime.Now;
                                    remarks.Add(remark);
                                    bSuccess = remarks.RemarkSave();
                                    remark = null;
                                }


                                if (bSuccess == false)
                                {
                                    Utils.SaveLog("UpdateRemark", dtStart, dtEnd, strResult, XmlHelper.Serialize(remarkRequest, false));
                                }
                                else
                                {
                                    strResult = Utils.ErrorXml("000", "Success Request Transaction");
                                }
                            }

                        }
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("UpdateBookingRemark", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, XmlHelper.Serialize(remarkRequest, false));
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                string strParam = string.Empty;
                if (remarkRequest == null)
                {
                    strParam = XmlHelper.Serialize(remarkRequest, false);
                }

                Utils.SaveProcessLog("UpdateBookingRemark", dtStart, DateTime.Now, strParam);
            }
            return strResult;
        }
        // Complete existing remark
        [WebMethod(EnableSession = true)]
        public string CompleteRemark(CompleteRemarkRequest completeRemarkRequest)
        {
            Agents objAgents = (Agents)Session["Agents"];
            BookingHeader bookingHeader = (BookingHeader)Session["BookingHeader"];


            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            string strResult = string.Empty;
            bool bSuccess = true;
            Guid userID;
            Remarks remarks = null;

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.AddRemark) == true)
                {
                    if (objAgents == null || objAgents.Count == 0 || Session["UserId"] == null)
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }
                    else
                    {
                        userID = new Guid(Session["UserId"].ToString());
                        if (completeRemarkRequest != null)
                        {
                            remarks = new Remarks();
                            bSuccess = remarks.RemarkComplete(completeRemarkRequest.BookingRemarkId, userID);
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("4000", "Remove Remark request parameter is required.");
                            dtEnd = DateTime.Now;
                            bSuccess = false;
                        }
                    }

                    if (bSuccess == false)
                    {
                        Utils.SaveLog("CompleteRemark", dtStart, dtEnd, strResult, XmlHelper.Serialize(completeRemarkRequest, false));
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("000", "Success Request Transaction");
                    }


                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("CompleteRemark", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, XmlHelper.Serialize(completeRemarkRequest, false));
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {


                Utils.SaveProcessLog("CompleteRemark", dtStart, DateTime.Now, XmlHelper.Serialize(completeRemarkRequest, false));
            }
            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string GetQueueCount(bool unassigned)
        {
            Agents objAgents = (Agents)Session["Agents"];
            RemarksQueue remarksQueue;
            string strAgencyCode = string.Empty;
            string strResult = string.Empty;


            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;

            bool bSuccess = true;

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.AddRemark) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        strAgencyCode = objAgents[0].agency_code;

                        remarksQueue = new RemarksQueue();
                        strResult = remarksQueue.GetQueueCount(strAgencyCode, unassigned);
                        if (bSuccess == false)
                        {
                            Utils.SaveLog("GetQueueCount", dtStart, dtEnd, strResult, strAgencyCode);
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetQueueCount", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("GetQueueCount", dtStart, DateTime.Now, strAgencyCode);
            }
            return strResult;
        }
        [WebMethod(EnableSession = true)]
        public string GetQuoteSummary(GetQuoteSummaryRequest getQuoteRequest)
        {
            clstikAeroWebService objService = new clstikAeroWebService();
            Passengers passengers = new Passengers();
            Flights flights = new Flights();
            string strResult = string.Empty;
            string strGetQuote = string.Empty;
            string strTransitFlightId = string.Empty;
            Guid gFlightConnectionId = Guid.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd;
            try
            {
                if (getQuoteRequest.Adult == 0 && getQuoteRequest.Child == 0 && getQuoteRequest.Infant == 0 && getQuoteRequest.Other == 0)
                {
                    strResult = Utils.ErrorXml("9103", "Passengers are required");
                    dtEnd = DateTime.Now;
                    Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, strResult, strGetQuote);
                }
                else if (getQuoteRequest.Infant > getQuoteRequest.Adult)
                {
                    strResult = Utils.ErrorXml("9104", "Number of infants is greater than number of adults.");
                    dtEnd = DateTime.Now;
                    Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, strResult, strGetQuote);
                }

                else if (getQuoteRequest.AgencyCode.Length == 0)
                {
                    strResult = Utils.ErrorXml("9105", "AgencyCode is required");
                    dtEnd = DateTime.Now;
                    Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, strResult, strGetQuote);
                }
                else if (getQuoteRequest.Language.Length == 0)
                {
                    strResult = Utils.ErrorXml("9106", "Language is required");
                    dtEnd = DateTime.Now;
                    Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, strResult, strGetQuote);
                }
                else if (getQuoteRequest.CurrencyCode.Length == 0)
                {
                    strResult = Utils.ErrorXml("9107", "CurrencyCode is required");
                    dtEnd = DateTime.Now;
                    Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, strResult, strGetQuote);
                }
                else if (getQuoteRequest.Password.Length == 0)
                {
                    strResult = Utils.ErrorXml("9108", "Password is required");
                    dtEnd = DateTime.Now;
                    Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, strResult, strGetQuote);
                }

                else if (getQuoteRequest.FlightSegmentRequest == null)
                {
                    strResult = Utils.ErrorXml("9109", "FlightSegment is required");
                    dtEnd = DateTime.Now;
                    Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, strResult, strGetQuote);
                }
                else if (getQuoteRequest.FlightSegmentRequest.FlightId.ToString().Length == 0 || getQuoteRequest.FlightSegmentRequest.FlightId.Equals(Guid.Empty))
                {
                    strResult = Utils.ErrorXml("9110", "FlightId is required");
                    dtEnd = DateTime.Now;
                    Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, strResult, strGetQuote);
                }
                else if (getQuoteRequest.FlightSegmentRequest.FareId.ToString().Length == 0 || getQuoteRequest.FlightSegmentRequest.FareId.Equals(Guid.Empty))
                {
                    strResult = Utils.ErrorXml("9111", "FareId is required");
                    dtEnd = DateTime.Now;
                    Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, strResult, strGetQuote);
                }
                else if (getQuoteRequest.FlightSegmentRequest.OriginRcd.Length == 0)
                {
                    strResult = Utils.ErrorXml("9112", "OriginRcd is required");
                    dtEnd = DateTime.Now;
                    Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, strResult, strGetQuote);
                }
                else if (getQuoteRequest.FlightSegmentRequest.DestinationRcd.Length == 0)
                {
                    strResult = Utils.ErrorXml("9113", "DestinationRcd is required");
                    dtEnd = DateTime.Now;
                    Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, strResult, strGetQuote);
                }
                else if (!getQuoteRequest.NoVat.ToString().ToUpper().Equals("TRUE") && !getQuoteRequest.NoVat.ToString().ToUpper().Equals("FALSE"))
                {
                    strResult = Utils.ErrorXml("9114", "NoVat is not valid");
                    dtEnd = DateTime.Now;
                    Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, strResult, strGetQuote);
                }
                else if (getQuoteRequest.FlightSegmentRequest.BookingClassRcd.Length == 0)
                {
                    strResult = Utils.ErrorXml("9115", "BookingClassRcd is required");
                    dtEnd = DateTime.Now;
                    Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, strResult, strGetQuote);
                }
                else if (!getQuoteRequest.FlightSegmentRequest.TransitFlightId.Equals(Guid.Empty))
                {
                    if (getQuoteRequest.FlightSegmentRequest.TransitAirportRcd.Length == 0)
                    {
                        strResult = Utils.ErrorXml("9116", "TransitAirportRcd is required");
                        dtEnd = DateTime.Now;
                        Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, strResult, strGetQuote);
                    }
                    else if (getQuoteRequest.FlightSegmentRequest.TransitBoardingClassRcd.Length == 0)
                    {
                        strResult = Utils.ErrorXml("9117", "TransitBoardingClassRcd is required");
                        dtEnd = DateTime.Now;
                        Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, strResult, strGetQuote);
                    }
                    else if (getQuoteRequest.FlightSegmentRequest.TransitBookingClassRcd.Length == 0)
                    {
                        strResult = Utils.ErrorXml("9118", "TransitBookingClassRcd is required");
                        dtEnd = DateTime.Now;
                        Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, strResult, strGetQuote);
                    }
                    else if (getQuoteRequest.FlightSegmentRequest.TransitDepartureDate.Equals(DateTime.MinValue))
                    {
                        strResult = Utils.ErrorXml("9119", "TransitDepartureDate is required");
                        dtEnd = DateTime.Now;
                        Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, strResult, strGetQuote);
                    }
                }

                if (strResult.Equals(string.Empty))
                {
                    FlightSegmentRequest flight = getQuoteRequest.FlightSegmentRequest;
                    Flight f = new Flight();
                    f.flight_id = flight.FlightId;
                    f.fare_id = flight.FareId;
                    f.origin_rcd = flight.OriginRcd;
                    f.destination_rcd = flight.DestinationRcd;
                    strTransitFlightId = flight.TransitFlightId.ToString();
                    f.boarding_class_rcd = flight.BoardingClassRcd;
                    f.booking_class_rcd = flight.BookingClassRcd;
                    f.departure_date = flight.DepartureDate;
                    f.eticket_flag = 1;

                    if (!Guid.Empty.Equals(new Guid(strTransitFlightId)))
                    {
                        gFlightConnectionId = Guid.NewGuid();
                        f.od_origin_rcd = flight.OriginRcd;
                        f.od_destination_rcd = flight.DestinationRcd;
                        f.flight_connection_id = gFlightConnectionId;
                        f.destination_rcd = flight.TransitAirportRcd;
                    }

                    flights.Add(f);
                    f = null;

                    //Transit flight information
                    if (!Guid.Empty.Equals(new Guid(strTransitFlightId)))
                    {
                        Flight transitFlight = new Flight();
                        transitFlight.destination_rcd = flight.DestinationRcd;
                        transitFlight.flight_id = new Guid(strTransitFlightId);
                        transitFlight.origin_rcd = flight.TransitAirportRcd;
                        transitFlight.fare_id = flight.FareId;
                        transitFlight.eticket_flag = 1;
                        transitFlight.od_origin_rcd = flight.OriginRcd;
                        transitFlight.od_destination_rcd = flight.DestinationRcd;
                        transitFlight.flight_connection_id = gFlightConnectionId;
                        transitFlight.boarding_class_rcd = flight.TransitBoardingClassRcd;
                        transitFlight.booking_class_rcd = flight.TransitBookingClassRcd;
                        transitFlight.departure_date = flight.TransitDepartureDate;
                        flights.Add(transitFlight);
                        transitFlight = null;
                    }


                    for (int i = 0; i < getQuoteRequest.Adult; i++)
                    {
                        Passenger pax = new Passenger();
                        pax.passenger_type_rcd = "ADULT";
                        pax.passenger_id = Guid.NewGuid();
                        passengers.Add(pax);
                    }
                    for (int i = 0; i < getQuoteRequest.Child; i++)
                    {
                        Passenger pax = new Passenger();
                        pax.passenger_type_rcd = "CHD";
                        pax.passenger_id = Guid.NewGuid();
                        passengers.Add(pax);
                    }
                    for (int i = 0; i < getQuoteRequest.Infant; i++)
                    {
                        Passenger pax = new Passenger();
                        pax.passenger_type_rcd = "INF";
                        pax.passenger_id = Guid.NewGuid();
                        passengers.Add(pax);
                    }
                    for (int i = 0; i < getQuoteRequest.Other; i++)
                    {
                        Passenger pax = new Passenger();
                        pax.passenger_type_rcd = String.IsNullOrEmpty(getQuoteRequest.OtherPassengerType) ? "STAFF" : getQuoteRequest.OtherPassengerType;
                        pax.passenger_id = Guid.NewGuid();
                        passengers.Add(pax);
                    }

                    strGetQuote += "Passengers.Adult : " + getQuoteRequest.Adult.ToString() + Environment.NewLine +
                                   "Passengers.Child : " + getQuoteRequest.Child.ToString() + Environment.NewLine +
                                   "Passengers.Infant : " + getQuoteRequest.Infant.ToString() + Environment.NewLine +
                                   "Passengers.Other : " + getQuoteRequest.Other.ToString() + Environment.NewLine;

                    strGetQuote += "Flight.FlightId : " + flight.FlightId + Environment.NewLine +
                                   "Flight.FareId : " + flight.FareId + Environment.NewLine +
                                   "Flight.OriginRcd : " + flight.OriginRcd + Environment.NewLine +
                                   "Flight.DestinationRcd : " + flight.DestinationRcd + Environment.NewLine +
                                   "Flight.DepartureDate : " + flight.DepartureDate.ToString("dd/MM/yyyy") + Environment.NewLine +
                                   "Flight.BoardingClassRcd : " + flight.BoardingClassRcd + Environment.NewLine +
                                   "Flight.BookingClassRcd : " + flight.BookingClassRcd + Environment.NewLine +
                                   "Flight.TransitFlightId : " + flight.TransitFlightId + Environment.NewLine +
                                   "Flight.TransitAirportRcd : " + flight.TransitAirportRcd + Environment.NewLine +
                                   "Flight.TransitDepartureDate : " + flight.TransitDepartureDate.ToString("dd/MM/yyyy") + Environment.NewLine +
                                   "Flight.TransitBoardingClassRcd : " + flight.TransitBoardingClassRcd + Environment.NewLine +
                                   "Flight.TransitBookingClassRcd : " + flight.TransitBookingClassRcd + Environment.NewLine;

                    strGetQuote += "AgencyCode : " + getQuoteRequest.AgencyCode + Environment.NewLine +
                                   "Language : " + getQuoteRequest.Language + Environment.NewLine +
                                   "CurrencyCode : " + getQuoteRequest.CurrencyCode + Environment.NewLine +
                                   "BNoVat : " + getQuoteRequest.NoVat + Environment.NewLine +
                                   "Password : " + getQuoteRequest.Password + Environment.NewLine;

                    strResult = objService.GetFlightSummaryAgencyLogin(passengers,
                                                                    flights,
                                                                    getQuoteRequest.AgencyCode.ToUpper(),
                                                                    getQuoteRequest.Language,
                                                                    getQuoteRequest.CurrencyCode,
                                                                    getQuoteRequest.NoVat,
                                                                    getQuoteRequest.Password);
                }
                else
                {
                    strGetQuote += "ErrorCode : " + strResult + Environment.NewLine;
                }

            }
            catch (Exception ex)
            {
                strResult = Utils.ErrorXml("103", "General Error");

                dtEnd = DateTime.Now;
                Utils.SaveLog("GetQuoteSummary", dtStart, dtEnd, ex.Message, strGetQuote);
            }
            finally
            {
                objService = null;

                //Save Process Log
                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                    Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
                {

                    Utils.SaveProcessLog("GetQuoteSummary", dtStart, DateTime.Now, strGetQuote);
                }
            }

            return strResult;
        }
        [WebMethod(EnableSession = true)]
        public string GetPassenger(PassengerManifestRequest paxManifestRequest)
        {

            string strResult = string.Empty;
            bool bSuccess = true;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            Agents objAgents = (Agents)Session["Agents"];
            PassengersManifest paxManifest;

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.Passenger) == true)
                {

                    if (objAgents != null && objAgents.Count > 0)
                    {

                        //Passengers objPax = new Passengers();
                        string strLanguageCode = string.Empty;
                        if (string.IsNullOrEmpty(paxManifestRequest.language))
                        {
                            paxManifestRequest.language = "EN";
                        }

                        paxManifest = new PassengersManifest();
                        strResult = paxManifest.GetPassenger(paxManifestRequest.airline,
                                                            paxManifestRequest.flightNumber,
                                                            paxManifestRequest.flightID,
                                                            paxManifestRequest.flightFrom,
                                                            paxManifestRequest.flightTo,
                                                            paxManifestRequest.recordLocator,
                                                            paxManifestRequest.origin,
                                                            paxManifestRequest.destination,
                                                            paxManifestRequest.passengerName,
                                                            paxManifestRequest.seatNumber,
                                                            paxManifestRequest.ticketNumber,
                                                            paxManifestRequest.phoneNumber,
                                                            paxManifestRequest.passengerStatus,
                                                            paxManifestRequest.checkInStatus,
                                                            paxManifestRequest.clientNumber,
                                                            paxManifestRequest.memberNumber,
                                                            paxManifestRequest.clientID,
                                                            paxManifestRequest.passengerId,
                                                            paxManifestRequest.booked,
                                                            paxManifestRequest.listed,
                                                            paxManifestRequest.eTicketOnly,
                                                            paxManifestRequest.includeCancelled,
                                                            paxManifestRequest.openSegments,
                                                            paxManifestRequest.showHistory,
                                                            paxManifestRequest.language);
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }

                    //Return Message to client.
                    if (bSuccess == false)
                    {
                        Utils.SaveLog("GetPassenger", dtStart, dtEnd, strResult, "");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetPassenger", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("GetPassenger", dtStart, DateTime.Now, string.Empty);
            }
            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string GetBookings(BookingGetRequest bookingGetRequest)
        {

            string strResult = string.Empty;
            bool bSuccess = true;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            Agents objAgents = (Agents)Session["Agents"];
            Booking booking;

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.Booking) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        string strLanguageCode = string.Empty;
                        if (string.IsNullOrEmpty(bookingGetRequest.language))
                        {
                            bookingGetRequest.language = "EN";
                        }

                        bool valid = true;

                        if (!string.IsNullOrEmpty(bookingGetRequest.flightFrom) && !IsValidDateTime(bookingGetRequest.flightFrom))
                        {
                            valid = false;
                        }
                        if (!string.IsNullOrEmpty(bookingGetRequest.flightTo) && !IsValidDateTime(bookingGetRequest.flightTo))
                        {
                            valid = false;
                        }

                        if (!string.IsNullOrEmpty(bookingGetRequest.createFrom) && !IsValidDateTime(bookingGetRequest.createFrom))
                        {
                            valid = false;
                        }

                        if (!string.IsNullOrEmpty(bookingGetRequest.createTo) && !IsValidDateTime(bookingGetRequest.createTo))
                        {
                            valid = false;
                        }

                        if (valid)
                        {
                            booking = new Booking();
                            strResult = booking.GetBookings(bookingGetRequest.airline,
                                                            bookingGetRequest.flightNumber,
                                                            bookingGetRequest.flightId,
                                                            bookingGetRequest.flightFrom,
                                                            bookingGetRequest.flightTo,
                                                            bookingGetRequest.recordLocator,
                                                            bookingGetRequest.origin,
                                                            bookingGetRequest.destination,
                                                            bookingGetRequest.passengerName,
                                                            bookingGetRequest.seatNumber,
                                                            bookingGetRequest.ticketNumber,
                                                            bookingGetRequest.phoneNumber,
                                                            bookingGetRequest.agencyCode,
                                                            bookingGetRequest.clientNumber,
                                                            bookingGetRequest.memberNumber,
                                                            bookingGetRequest.clientId,
                                                            bookingGetRequest.showHistory,
                                                            bookingGetRequest.language,
                                                            bookingGetRequest.bIndividual,
                                                            bookingGetRequest.bGroup,
                                                            bookingGetRequest.createFrom,
                                                            bookingGetRequest.createTo);
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("9120", "Invalid Datetime, Datetime should be yyyy-MM-dd");
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }

                    //Return Message to client.
                    if (bSuccess == false)
                    {
                        Utils.SaveLog("GetBookings", dtStart, dtEnd, strResult, "");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetBookings", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("GetBookings", dtStart, DateTime.Now, string.Empty);
            }
            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string GetActivities(ActivityGetRequestMessage activityGetMessage)
        {
            string strAgencyCode = string.Empty;
            string strResult = string.Empty;
            string agencyCode = string.Empty;
            bool bSuccess = true;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            Agents objAgents = (Agents)Session["Agents"];
            ServiceClient srvClient;

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.Booking) == true)
                {

                    if (objAgents != null && objAgents.Count > 0)
                    {
                        // don't use anymore
                        strAgencyCode = objAgents[0].agency_code;

                        if (!string.IsNullOrEmpty(activityGetMessage.AgencyCode))
                            agencyCode = activityGetMessage.AgencyCode;

                        srvClient = new ServiceClient();
                        strResult = (srvClient.GetActivities(agencyCode,
                                                            activityGetMessage.RemarkType,
                                                            activityGetMessage.Nickname,
                                                            activityGetMessage.TimelimitFrom,
                                                            activityGetMessage.TimelimitTo,
                                                            activityGetMessage.PendingOnly,
                                                            activityGetMessage.IncompleteOnly,
                                                            activityGetMessage.IncludeRemarks,
                                                            activityGetMessage.showUnassigned)).GetXml();
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                        dtEnd = DateTime.Now;
                        bSuccess = false;
                    }
                    if (strResult.Length > 30)
                    {

                    }
                    else
                    {
                        strResult = Utils.ErrorXml("107", "Activity not found.");
                    }
                    //Return Message to client.
                    if (bSuccess == false)
                    {
                        Utils.SaveLog("GetActivities", dtStart, dtEnd, strResult, "");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("GetActivities", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
                bSuccess = false;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("GetActivities", dtStart, DateTime.Now, string.Empty);
            }
            return strResult;
        }


        [WebMethod(EnableSession = true)]
        public string CreateClientProfile(CreateClientProfileRequest ClientRequest)
        {
            string strResult = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd;
            Agents objAgents = (Agents)Session["Agents"];
            Guid clientProfileId = Guid.NewGuid();
            string strClientProfile = string.Empty;

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.ClientProfile) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        if (ClientValidation(ClientRequest, ref strResult, clientProfileId.ToString(), true))
                        {
                            Client client = new Client();
                            client.client_profile_id = clientProfileId;
                            client.title_rcd = ClientRequest.title_rcd;
                            client.lastname = ClientRequest.Lastname;
                            client.firstname = ClientRequest.Firstname;
                            client.middlename = ClientRequest.Middlename;
                            client.language_rcd = ClientRequest.language_rcd;
                            client.nationality_rcd = ClientRequest.nationality_rcd;

                            if (!string.IsNullOrEmpty(ClientRequest.passenger_weight))
                                client.passenger_weight = Convert.ToDecimal(ClientRequest.passenger_weight);
                            else
                                client.passenger_weight = 0;

                            client.gender_type_rcd = ClientRequest.gender_type_rcd;
                            client.passenger_type_rcd = ClientRequest.passenger_type_rcd;
                            client.address_line1 = ClientRequest.address_line1;
                            client.address_line2 = ClientRequest.address_line2;
                            client.state = ClientRequest.State;
                            client.zip_code = ClientRequest.zip_code;
                            client.country_rcd = ClientRequest.country_rcd;
                            client.street = ClientRequest.Street;
                            client.city = ClientRequest.City;
                            client.document_type_rcd = ClientRequest.document_type_rcd;
                            client.passport_number = ClientRequest.passport_number;
                            client.passport_issue_date = Convert.ToDateTime(ClientRequest.passport_issue_date);
                            client.passport_expiry_date = Convert.ToDateTime(ClientRequest.passport_expiry_date);
                            client.passport_issue_place = ClientRequest.passport_issue_place;
                            client.passport_birth_place = ClientRequest.passport_birth_place;
                            client.date_of_birth = Convert.ToDateTime(ClientRequest.date_of_birth);
                            client.passport_issue_country_rcd = ClientRequest.passport_issue_country_rcd;
                            client.contact_email = ClientRequest.contact_email;
                            client.phone_mobile = ClientRequest.phone_mobile;
                            client.phone_home = ClientRequest.phone_home;
                            client.phone_business = ClientRequest.phone_business;
                            client.create_by = objAgents[0].user_account_id;
                            client.create_date_time = DateTime.Now;
                            client.update_by = objAgents[0].user_account_id;
                            client.update_date_time = DateTime.Now;
                            client.wheelchair_flag = Convert.ToByte(ClientRequest.wheelchair_flag);
                            client.vip_flag = Convert.ToByte(ClientRequest.vip_flag);
                            client.member_level_rcd = ClientRequest.member_level_rcd;
                            client.window_seat_flag = Convert.ToByte(ClientRequest.window_seat_flag);
                            client.redress_number = ClientRequest.redress_number;
                            client.status_code = ClientRequest.status_code;
                            client.client_password = ClientRequest.client_password;
                            client.company_flag = Convert.ToByte(ClientRequest.company_flag);
                            client.profile_on_hold_date_time = DateTime.Now;
                            client.client_type_rcd = ClientRequest.client_type_rcd;
                            client.member_since_date = Convert.ToDateTime(ClientRequest.member_since_date);
                            client.contact_name = ClientRequest.contact_name;
                            client.mobile_email = ClientRequest.mobile_email;
                            client.member_number = ClientRequest.member_number;
                            client.member_level_display_name = ClientRequest.member_level_rcd;
                            client.po_box = ClientRequest.po_box;
                            client.phone_fax = ClientRequest.phone_fax;
                            client.province = ClientRequest.Province;
                            client.district = ClientRequest.District;
                            client.employee_number = ClientRequest.employee_number;
                            // news letter
                            client.subscribe_newsletter_flag = ClientRequest.newsletter_flag;

                            if (ClientRequest.airport_rcd == null )
                                client.airport_rcd = string.Empty;
                            else
                                client.airport_rcd = ClientRequest.airport_rcd;

                            Passenger pax = new Passenger();
                            pax.passenger_profile_id = Guid.NewGuid();
                            pax.client_profile_id = clientProfileId;
                            pax.title_rcd = ClientRequest.title_rcd;
                            pax.lastname = ClientRequest.Lastname;
                            pax.firstname = ClientRequest.Firstname;
                            pax.middlename = ClientRequest.Middlename;
                            pax.nationality_rcd = ClientRequest.nationality_rcd;

                            if (!string.IsNullOrEmpty(ClientRequest.passenger_weight))
                                pax.passenger_weight = Convert.ToDecimal(ClientRequest.passenger_weight);
                            else
                                pax.passenger_weight = 0;

                            pax.gender_type_rcd = ClientRequest.gender_type_rcd;
                            pax.passenger_type_rcd = ClientRequest.passenger_type_rcd;
                            pax.document_type_rcd = ClientRequest.document_type_rcd;
                            pax.passport_number = ClientRequest.passport_number;
                            pax.passport_issue_date = Convert.ToDateTime(ClientRequest.passport_issue_date);
                            pax.passport_expiry_date = Convert.ToDateTime(ClientRequest.passport_expiry_date);
                            pax.passport_issue_place = ClientRequest.passport_issue_place;
                            pax.passport_birth_place = ClientRequest.passport_birth_place;
                            pax.date_of_birth = Convert.ToDateTime(ClientRequest.date_of_birth);
                            pax.passport_issue_country_rcd = ClientRequest.passport_issue_country_rcd;
                            pax.create_by = objAgents[0].user_account_id;
                            pax.create_date_time = DateTime.Now;
                            pax.update_by = objAgents[0].user_account_id;
                            pax.update_date_time = DateTime.Now;
                            pax.wheelchair_flag = Convert.ToByte(ClientRequest.wheelchair_flag);
                            pax.vip_flag = Convert.ToByte(ClientRequest.vip_flag);
                            pax.passenger_role_rcd = "MYSELF";
                            pax.window_seat_flag = Convert.ToByte(ClientRequest.window_seat_flag);
                            pax.redress_number = ClientRequest.redress_number;

                            //add KnownTravelerNumber when create profile
                            //pax.known_traveler_number = ClientRequest.known_traveler_number;
                            //pax.known_traveler_number = "test";

                            Passengers paxs = new Passengers();
                            paxs.Add(pax);

                            tikSystem.Web.Library.ServiceClient srvClient = new tikSystem.Web.Library.ServiceClient();

                            if (srvClient.AddClientProfile(client, paxs, null))
                            {
                                //read client
                                strClientProfile = srvClient.ReadClientProfile(clientProfileId.ToString());
                                strResult = Utils.ErrorXmlAddClient("000", "Successful Transaction Request", strClientProfile);
                                //strResult = Utils.ErrorXml("000", "Successful Transaction Request", clientProfileId.ToString());
                            }
                            else
                            {
                                strResult = Utils.ErrorXml("7000", "Save client profile failed");
                            }
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                }
            }
            catch (Exception ex)
            {
                strResult = Utils.ErrorXml("103", "General Error");
                dtEnd = DateTime.Now;
                Utils.SaveLog("CreateClientProfile", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, strResult);
            }

            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string EditClientProfile(EditClientProfileRequest ClientRequest)
        {
            string strResult = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd;
            Agents objAgents = (Agents)Session["Agents"];

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.ClientProfile) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        if (ClientValidation(ClientRequest, ref strResult, ClientRequest.client_profile_id.ToString(), false))
                            // LA phase2
                           // if (ClientValidationEdit(ClientRequest, ref strResult, ClientRequest.client_profile_id.ToString(), false))
                        {
                            Client client = new Client();
                            client.client_profile_id = ClientRequest.client_profile_id;
                            client.title_rcd = ClientRequest.title_rcd;
                            client.lastname = ClientRequest.Lastname;
                            client.firstname = ClientRequest.Firstname;
                            client.middlename = ClientRequest.Middlename;
                            client.language_rcd = ClientRequest.language_rcd;
                            client.nationality_rcd = ClientRequest.nationality_rcd;

                            if(string.IsNullOrEmpty(ClientRequest.passenger_weight))
                                client.passenger_weight = 0;
                            else
                                client.passenger_weight = Convert.ToDecimal(ClientRequest.passenger_weight);


                            client.gender_type_rcd = ClientRequest.gender_type_rcd;
                            client.passenger_type_rcd = ClientRequest.passenger_type_rcd;
                            client.address_line1 = ClientRequest.address_line1;
                            client.address_line2 = ClientRequest.address_line2;
                            client.state = ClientRequest.State;
                            client.zip_code = ClientRequest.zip_code;
                            client.country_rcd = ClientRequest.country_rcd;
                            client.street = ClientRequest.Street;
                            client.city = ClientRequest.City;
                            client.document_type_rcd = ClientRequest.document_type_rcd;
                            client.passport_number = ClientRequest.passport_number;
                            client.passport_issue_date = Convert.ToDateTime(ClientRequest.passport_issue_date);
                            client.passport_expiry_date = Convert.ToDateTime(ClientRequest.passport_expiry_date);
                            client.passport_issue_place = ClientRequest.passport_issue_place;
                            client.passport_birth_place = ClientRequest.passport_birth_place;
                            client.date_of_birth = Convert.ToDateTime(ClientRequest.date_of_birth);
                            client.passport_issue_country_rcd = ClientRequest.passport_issue_country_rcd;
                            client.contact_email = ClientRequest.contact_email;
                            client.phone_mobile = ClientRequest.phone_mobile;
                            client.phone_home = ClientRequest.phone_home;
                            client.phone_business = ClientRequest.phone_business;
                            client.update_by = objAgents[0].user_account_id;
                            client.update_date_time = DateTime.Now;
                            client.wheelchair_flag = Convert.ToByte(ClientRequest.wheelchair_flag);
                            client.vip_flag = Convert.ToByte(ClientRequest.vip_flag);
                            client.member_level_rcd = ClientRequest.member_level_rcd;
                            client.window_seat_flag = Convert.ToByte(ClientRequest.window_seat_flag);
                            client.redress_number = ClientRequest.redress_number;
                            client.status_code = ClientRequest.status_code;
                            client.client_password = ClientRequest.client_password;
                            client.company_flag = Convert.ToByte(ClientRequest.company_flag);
                            //client.profile_on_hold_date_time = DateTime.Now;
                            client.company_client_profile_id = ClientRequest.client_profile_id;
                            client.ffp_total = 0;
                            client.ffp_period = 0;
                            client.ffp_balance = 0;
                            client.client_type_rcd = ClientRequest.client_type_rcd;
                            client.member_since_date = Convert.ToDateTime(ClientRequest.member_since_date);
                            client.keep_point = 0;
                            client.district = ClientRequest.District;
                            client.province = ClientRequest.Province;
                            client.po_box = ClientRequest.po_box;
                            client.phone_fax = ClientRequest.phone_fax;

                            // new feature for LA
                            client.subscribe_newsletter_flag = ClientRequest.newsletter_flag;
                            client.airport_rcd = ClientRequest.airport_rcd;

                            tikSystem.Web.Library.ServiceClient srvClient = new tikSystem.Web.Library.ServiceClient();

                            if (srvClient.EditClientProfile(client, null, null))
                            {
                                strResult = Utils.ErrorXml("000", "Successful Transaction Request");
                            }
                            else
                            {
                                strResult = Utils.ErrorXml("7000", "Save client profile failed.");
                            }
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                }
            }
            catch (Exception ex)
            {
                strResult = Utils.ErrorXml("103", "General Error");
                dtEnd = DateTime.Now;
                Utils.SaveLog("EditClientProfile", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, strResult);
            }

            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string AddPassengerProfile(AddPassengersProfileRequest PassenegerRequest)
        {
            string strResult = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd;
            Agents objAgents = (Agents)Session["Agents"];

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.ClientProfile) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        if (PassenegerRequest != null &&
                            PassenegerRequest.PassengerProfile != null &&
                            PassenegerRequest.PassengerProfile.Count > 0)
                        {
                            Passengers paxs = new Passengers();
                            for (int i = 0; i < PassenegerRequest.PassengerProfile.Count; i++)
                            {
                                if (PassengerClientValidation(PassenegerRequest.PassengerProfile[i], ref strResult))
                                {
                                    Passenger pax = new Passenger();
                                    pax.passenger_profile_id = Guid.NewGuid();
                                    pax.client_profile_id = PassenegerRequest.client_profile_id;
                                    pax.document_type_rcd = PassenegerRequest.PassengerProfile[i].document_type_rcd;
                                    pax.title_rcd = PassenegerRequest.PassengerProfile[i].title_rcd;
                                    pax.lastname = PassenegerRequest.PassengerProfile[i].Lastname;
                                    pax.firstname = PassenegerRequest.PassengerProfile[i].Firstname;
                                    pax.middlename = PassenegerRequest.PassengerProfile[i].Middlename;
                                    pax.language_rcd = PassenegerRequest.PassengerProfile[i].language_rcd;
                                    pax.nationality_rcd = PassenegerRequest.PassengerProfile[i].nationality_rcd;
                                    pax.passenger_weight = Convert.ToDecimal(PassenegerRequest.PassengerProfile[i].passenger_weight);
                                    pax.gender_type_rcd = PassenegerRequest.PassengerProfile[i].gender_type_rcd;
                                    pax.passenger_type_rcd = PassenegerRequest.PassengerProfile[i].passenger_type_rcd;
                                    pax.passport_number = PassenegerRequest.PassengerProfile[i].passport_number;
                                    pax.passport_issue_date = Convert.ToDateTime(PassenegerRequest.PassengerProfile[i].passport_issue_date);
                                    pax.passport_expiry_date = Convert.ToDateTime(PassenegerRequest.PassengerProfile[i].passport_expiry_date);
                                    pax.passport_issue_place = PassenegerRequest.PassengerProfile[i].passport_issue_place;
                                    pax.passport_birth_place = PassenegerRequest.PassengerProfile[i].passport_birth_place;
                                    pax.date_of_birth = Convert.ToDateTime(PassenegerRequest.PassengerProfile[i].date_of_birth);
                                    pax.passport_issue_country_rcd = PassenegerRequest.PassengerProfile[i].passport_issue_country_rcd;
                                    pax.employee_number = PassenegerRequest.PassengerProfile[i].employee_number;
                                    pax.wheelchair_flag = Convert.ToByte(PassenegerRequest.PassengerProfile[i].wheelchair_flag);
                                    pax.vip_flag = Convert.ToByte(PassenegerRequest.PassengerProfile[i].vip_flag);
                                    pax.member_level_rcd = PassenegerRequest.PassengerProfile[i].member_level_rcd;
                                    pax.member_number = PassenegerRequest.PassengerProfile[i].member_number;
                                    pax.window_seat_flag = Convert.ToByte(PassenegerRequest.PassengerProfile[i].window_seat_flag);
                                    pax.redress_number = PassenegerRequest.PassengerProfile[i].redress_number;
                                    pax.passenger_role_rcd = PassenegerRequest.PassengerProfile[i].passenger_role_rcd;
                                    pax.comment = PassenegerRequest.PassengerProfile[i].comment;
                                    pax.medical_conditions = PassenegerRequest.PassengerProfile[i].medical_conditions;
                                    //add KnownTravelerNumber
                                    pax.known_traveler_number = PassenegerRequest.PassengerProfile[i].known_traveler_number;

                                    paxs.Add(pax);
                                    pax = null;
                                }
                                else
                                {
                                    paxs.Clear();
                                    break;
                                }

                                if (paxs.Count > 0)
                                {
                                    tikSystem.Web.Library.ServiceClient srvClient = new tikSystem.Web.Library.ServiceClient();
                                    if (srvClient.AddClientProfile(null, paxs, null))
                                    {
                                        strResult = Utils.ErrorXml("000", "Successful Transaction Request");
                                    }
                                    else
                                    {
                                        strResult = Utils.ErrorXml("8000", "Save passenger profile failed.");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                }
            }
            catch (Exception ex)
            {
                strResult = Utils.ErrorXml("103", "General Error");
                dtEnd = DateTime.Now;
                Utils.SaveLog("AddPassengerProfile", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, strResult);
            }

            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string EditPassengerProfile(EditPassengerProfileRequest PassenegerRequest)
        {
            string strResult = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd;
            Agents objAgents = (Agents)Session["Agents"];

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.ClientProfile) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        if (PassenegerRequest != null &&
                            PassenegerRequest.PassengerProfile != null &&
                            PassenegerRequest.PassengerProfile.Count > 0)
                        {
                            Passengers paxs = new Passengers();
                            for (int i = 0; i < PassenegerRequest.PassengerProfile.Count; i++)
                            {
                                if (PassengerClientValidation(PassenegerRequest.PassengerProfile[i], ref strResult))
                                {
                                    Passenger pax = new Passenger();
                                    pax.client_profile_id = PassenegerRequest.client_profile_id;
                                    pax.passenger_profile_id = PassenegerRequest.PassengerProfile[i].passenger_profile_id;
                                    pax.document_type_rcd = PassenegerRequest.PassengerProfile[i].document_type_rcd;
                                    pax.title_rcd = PassenegerRequest.PassengerProfile[i].title_rcd;
                                    pax.lastname = PassenegerRequest.PassengerProfile[i].Lastname;
                                    pax.firstname = PassenegerRequest.PassengerProfile[i].Firstname;
                                    pax.middlename = PassenegerRequest.PassengerProfile[i].Middlename;
                                    pax.language_rcd = PassenegerRequest.PassengerProfile[i].language_rcd;
                                    pax.nationality_rcd = PassenegerRequest.PassengerProfile[i].nationality_rcd;
                                    pax.passenger_weight = Convert.ToDecimal(PassenegerRequest.PassengerProfile[i].passenger_weight);
                                    pax.gender_type_rcd = PassenegerRequest.PassengerProfile[i].gender_type_rcd;
                                    pax.passenger_type_rcd = PassenegerRequest.PassengerProfile[i].passenger_type_rcd;
                                    pax.passport_number = PassenegerRequest.PassengerProfile[i].passport_number;
                                    pax.passport_issue_date = Convert.ToDateTime(PassenegerRequest.PassengerProfile[i].passport_issue_date);
                                    pax.passport_expiry_date = Convert.ToDateTime(PassenegerRequest.PassengerProfile[i].passport_expiry_date);
                                    pax.passport_issue_place = PassenegerRequest.PassengerProfile[i].passport_issue_place;
                                    pax.passport_birth_place = PassenegerRequest.PassengerProfile[i].passport_birth_place;
                                    pax.date_of_birth = Convert.ToDateTime(PassenegerRequest.PassengerProfile[i].date_of_birth);
                                    pax.passport_issue_country_rcd = PassenegerRequest.PassengerProfile[i].passport_issue_country_rcd;
                                    pax.employee_number = PassenegerRequest.PassengerProfile[i].employee_number;
                                    pax.wheelchair_flag = Convert.ToByte(PassenegerRequest.PassengerProfile[i].wheelchair_flag);
                                    pax.vip_flag = Convert.ToByte(PassenegerRequest.PassengerProfile[i].vip_flag);
                                    pax.member_level_rcd = PassenegerRequest.PassengerProfile[i].member_level_rcd;
                                    pax.member_number = PassenegerRequest.PassengerProfile[i].member_number;
                                    pax.window_seat_flag = Convert.ToByte(PassenegerRequest.PassengerProfile[i].window_seat_flag);
                                    pax.redress_number = PassenegerRequest.PassengerProfile[i].redress_number;
                                    pax.passenger_role_rcd = PassenegerRequest.PassengerProfile[i].passenger_role_rcd;

                                    pax.comment = PassenegerRequest.PassengerProfile[i].comment;
                                    pax.medical_conditions = PassenegerRequest.PassengerProfile[i].medical_conditions;
                                    //add KnownTravelerNumber
                                    pax.known_traveler_number = PassenegerRequest.PassengerProfile[i].known_traveler_number;

                                    paxs.Add(pax);
                                    pax = null;
                                }
                                else
                                {
                                    paxs.Clear();
                                    break;
                                }

                                if (paxs.Count > 0)
                                {
                                    tikSystem.Web.Library.ServiceClient srvClient = new tikSystem.Web.Library.ServiceClient();

                                    if (srvClient.EditClientProfile(null, paxs, null))
                                    {
                                        strResult = Utils.ErrorXml("000", "Successful Transaction Request");
                                    }
                                    else
                                    {
                                        strResult = Utils.ErrorXml("8000", "Save passenger profile failed.");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                }
            }
            catch (Exception ex)
            {
                strResult = Utils.ErrorXml("103", "General Error");
                dtEnd = DateTime.Now;
                Utils.SaveLog("EditPassengerProfile", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, strResult);
            }

            return strResult;
        }

        [WebMethod(EnableSession = true)]
        public string PassengerProfileRead(ClientProfileReadRequest ClientReadRequest)
        {
            string strResult = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            Agents objAgents = (Agents)Session["Agents"];

            try
            {
                if (ConfigurationUtil.GetFunctionalSetting(ConfigurationUtil.ConfigName.ClientProfile) == true)
                {
                    if (objAgents != null && objAgents.Count > 0)
                    {
                        Clients objMainProfile = new Clients();
                        //Read main profile.
                        objMainProfile.Read(ClientReadRequest.client_profile_id.ToString(),
                                            string.Empty,
                                            string.Empty,
                                            false);

                        if (objMainProfile.Count > 0)
                        {
                            Clients objClients = new Clients();
                           
                            objClients.ReadClientPassenger(string.Empty, ClientReadRequest.client_profile_id.ToString(), string.Empty);
                           
                            if (objClients != null && objClients.Count > 0)
                            {
                                strResult = objClients.GetPassengerProfileXml(objMainProfile[0]);
                            }
                            else
                            {
                                strResult = Utils.ErrorXml("9001", "Passenger profile not found.");
                                dtEnd = DateTime.Now;
                            }
                        }
                        else
                        {
                            strResult = Utils.ErrorXml("9000", "Client profile not found.");
                            dtEnd = DateTime.Now;
                        }

                    }
                    else
                    {
                        strResult = Utils.ErrorXml("100", "Fail To Initialize API Service");
                    }
                }
                else
                {
                    strResult = Utils.ErrorXml("106", "This funation call is not allowed.");
                    dtEnd = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                strResult = Utils.ErrorXml("103", ex.Message);
                Utils.SaveLog("PassengerProfileRead", dtStart, dtEnd, ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
            {
                Utils.SaveProcessLog("PassengerProfileRead", dtStart, DateTime.Now, string.Empty);
            }

            return strResult;
        }

        #endregion

        #region Helper
        public string WrapCCNumber(string strXml,string document_number)
        {
            string startCC = string.Empty;
            string endCC = string.Empty;

            try
            {
                //replace CC Number
                startCC = document_number.Trim().Substring(0, 3);
                endCC = document_number.Trim().Substring(document_number.Trim().Length - 3, 3);
                string newCC = startCC + new string('*', 12) + endCC;
                strXml = strXml.Replace(document_number, newCC);

                //replace cvv
                string strCvv = @"cvv";
                Match m = Regex.Match(strXml, @"\b" + strCvv + @"\b(.+?)\>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                string value = m.Groups[1].Value;

                if (value.Contains("cvv"))
                    strXml = strXml.Replace(value, ">***</cvv");
                else
                    strXml = strXml.Replace(value, ">***</CVV");

            }
            catch (System.Exception ex) { }

            return strXml;
        }
        public bool IsValidDateTime(string DateTime)
        {
            Regex checktime =
             new Regex(@"^\d{4}\-(0[1-9]|1[012])\-(0[1-9]|[12][0-9]|3[01])$");

            return checktime.IsMatch(DateTime);
        }
        public bool IsValidTime(string thetime)
        {
            //^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$
            Regex checktime =
             new Regex(@"^(?:0?[0-9]|1[0-9]|2[0-3])[0-5][0-9]$");

            return checktime.IsMatch(thetime);
        } 
        public string GroupXML(XPathDocument xmlDoc, string strPath, string currencyCode)
        {
            XPathNavigator nv = xmlDoc.CreateNavigator();

            StringWriter objWriter = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(objWriter);
            Library objLi = new Library();

            string[] fields = new string[]{ "airline_rcd",
                                            "flight_number",
                                            "booking_class_rcd",
                                            "boarding_class_rcd",
                                            "flight_id",
                                            "origin_rcd",
                                            "destination_rcd",
                                            "origin_name",
                                            "destination_name",
                                            "flight_status_rcd",
                                            "departure_date",
                                            "planned_departure_time",
                                            "planned_arrival_time",
                                            "fare_id",
                                            "fare_code",
                                            "fare_type_rcd",
                                            "redemption_points",
                                            "transit_points",
                                            "transit_points_name",
                                            "transit_flight_id",
                                            "transit_booking_class_rcd",
                                            "transit_boarding_class_rcd",
                                            "transit_airport_rcd",
                                            "transit_departure_date",
                                            "transit_planned_departure_time",
                                            "transit_planned_arrival_time",
                                            "transit_fare_id",
                                            "transit_redemption_points",
                                            "transit_name",
                                            "transit_waitlist_open_flag",
                                            "transit_airline_rcd",
                                            "transit_flight_number",
                                            "transit_flight_status_rcd",
                                            "transit_flight_duration",
                                            "transit_class_open_flag",
                                            "transit_nesting_string",
                                            "nesting_string",
                                            "nested_book_available",
                                            "full_flight_flag",
                                            "class_open_flag",
                                            "close_web_sales",
                                            "total_adult_fare",
                                            "total_child_fare",
                                            "total_infant_fare",
                                            "total_other_fare",
                                            "fare_column",
                                            "flight_comment",
                                            "filter_logic_flag",
                                            "restriction_text",
                                            "flight_duration",
                                            "class_capacity",
                                            "waitlist_open_flag",
                                            "refundable_flag",
                                            "currency_rcd",
                                            "aircraft_type_rcd",
                                            "transit_aircraft_type_rcd",
                                            "arrival_date",
                                            "transit_arrival_date",
                                            "number_of_stops",
                                            "eticket_flag",
                                            "nest_seat_available",
                                            "corporate_fare_flag",
                                            "endorsement_text"};

            if (!string.IsNullOrEmpty(currencyCode))
            {
                foreach (XPathNavigator n in nv.Select(strPath))
                {
                    xmlWriter.WriteStartElement("AvailabilityFlight");
                    {
                        for (int i = 0; i < fields.Length; i++)
                        {
                            if (fields[i] == "currency_rcd")
                            {
                                xmlWriter.WriteStartElement(fields[i]);
                                xmlWriter.WriteValue(currencyCode);
                                xmlWriter.WriteEndElement();
                            }
                            else
                            {
                                xmlWriter.WriteStartElement(fields[i]);
                                xmlWriter.WriteValue(objLi.getXPathNodevalue(n, fields[i], Library.xmlReturnType.value));
                                xmlWriter.WriteEndElement();
                            }


                        }
                    }
                    xmlWriter.WriteEndElement();
                }
            }
            else
            {
                return string.Empty;
            }

            xmlWriter.Flush();
            xmlWriter.Close();
            xmlWriter = null;

            objLi = null;
            return objWriter.ToString();
        }

        private void SetCreateUpdateInformation(Guid guUserId,
                                                BookingHeader bookingHeader,
                                                Itinerary itinerary,
                                                Passengers passengers,
                                                Fees fees,
                                                Mappings mappings,
                                                Services services,
                                                Remarks remarks,
                                                Payments payments,
                                                Taxes taxes,
                                                Vouchers vouchers)
        {

            if (bookingHeader != null)
            {
                //Booking header
                if (bookingHeader.create_by == Guid.Empty)
                {
                    bookingHeader.create_by = guUserId;
                    bookingHeader.create_date_time = DateTime.Now;
                }
                bookingHeader.update_by = guUserId;
                bookingHeader.update_date_time = DateTime.Now;
            }

            if (itinerary != null)
            {
                //Flight Segment
                foreach (FlightSegment f in itinerary)
                {
                    if (f.create_by == Guid.Empty)
                    {
                        f.create_by = guUserId;
                        f.create_date_time = DateTime.Now;
                    }
                    f.update_by = guUserId;
                    f.update_date_time = DateTime.Now;
                }
            }

            if (passengers != null)
            {
                //Passenger
                foreach (Passenger p in passengers)
                {
                    if (p.create_by == Guid.Empty)
                    {
                        p.create_by = guUserId;
                        p.create_date_time = DateTime.Now;
                    }
                    p.update_by = guUserId;
                    p.update_date_time = DateTime.Now;
                }
            }

            if (fees != null)
            {
                foreach (Fee f in fees)
                {
                    if (f.create_by == Guid.Empty)
                    {
                        f.create_by = guUserId;
                        f.create_date_time = DateTime.Now;
                    }
                    f.update_by = guUserId;
                    f.update_date_time = DateTime.Now;
                }
            }

            if (mappings != null)
            {

                foreach (Mapping mp in mappings)
                {
                    if (mp.create_by == Guid.Empty)
                    {
                        mp.create_by = guUserId;
                        mp.create_date_time = DateTime.Now;
                    }
                    mp.update_by = guUserId;
                    mp.update_date_time = DateTime.Now;
                }
            }

            if (services != null)
            {

                foreach (Service s in services)
                {
                    if (s.create_by == Guid.Empty)
                    {
                        s.create_by = guUserId;
                        s.create_date_time = DateTime.Now;
                    }
                    s.update_by = guUserId;
                    s.update_date_time = DateTime.Now;
                }
            }

            if (remarks != null)
            {
                foreach (Remark r in remarks)
                {
                    if (r.create_by == Guid.Empty)
                    {
                        r.create_by = guUserId;
                        r.create_date_time = DateTime.Now;
                    }
                    r.update_by = guUserId;
                    r.update_date_time = DateTime.Now;
                }
            }

            if (payments != null)
            {
                foreach (Payment p in payments)
                {
                    if (p.create_by == Guid.Empty)
                    {
                        p.create_by = guUserId;
                        p.create_date_time = DateTime.Now;
                    }
                    p.update_by = guUserId;
                    p.update_date_time = DateTime.Now;
                }
            }
            if (taxes != null)
            {
                foreach (Tax t in taxes)
                {
                    if (t.create_by == Guid.Empty)
                    {
                        t.create_by = guUserId;
                        t.create_date_time = DateTime.Now;
                    }
                    t.update_by = guUserId;
                    t.update_date_time = DateTime.Now;
                }
            }

            if (vouchers != null)
            {
                foreach (Voucher t in vouchers)
                {
                    if (t.create_by == Guid.Empty)
                    {
                        t.create_by = guUserId;
                        t.create_date_time = DateTime.Now;
                    }
                    t.update_by = guUserId;
                    t.update_date_time = DateTime.Now;
                }
            }
        }

        private void ClearBagggaeSession(bool outBoundFlight)
        {
            if (outBoundFlight == true)
            {
                Session.Remove("BaggageOutBound");
            }
            else
            {
                Session.Remove("BaggageInBound");
            }
        }

        private void FillClientCo(string client_profile_id,Clients clients)
        {
            ServiceClient objClient = new ServiceClient();
            DataSet dsClientList = null;

            // get client list call web service clientid  lastname
            dsClientList = objClient.GetCoporateSessionProfile(client_profile_id, string.Empty);

            // add to client session
            XPathDocument xmlDoc = new XPathDocument(new StringReader(dsClientList.GetXml()));
            XPathNavigator nv = xmlDoc.CreateNavigator();
            foreach (XPathNavigator n in nv.Select("NewDataSet/Corporate"))
            {
                Client client = new Client();
                client.client_number = long.Parse(XmlHelper.XpathValueNullToEmpty(n, "client_number"));
                client.client_profile_id = new Guid(XmlHelper.XpathValueNullToEmpty(n, "client_profile_id"));
                client.title_rcd = XmlHelper.XpathValueNullToEmpty(n, "title_rcd");
                client.gender_type_rcd = XmlHelper.XpathValueNullToEmpty(n, "gender_type_rcd");
                client.passenger_type_rcd = XmlHelper.XpathValueNullToEmpty(n, "passenger_type_rcd");
                client.lastname = XmlHelper.XpathValueNullToEmpty(n, "lastname");
                client.firstname = XmlHelper.XpathValueNullToEmpty(n, "firstname");
                client.middlename = XmlHelper.XpathValueNullToEmpty(n, "middlename");

                client.contact_name = XmlHelper.XpathValueNullToEmpty(n, "contact_name");
                client.contact_email = XmlHelper.XpathValueNullToEmpty(n, "contact_email");
                client.address_line1 = XmlHelper.XpathValueNullToEmpty(n, "address_line1");
                client.address_line2 = XmlHelper.XpathValueNullToEmpty(n, "address_line2");
                client.city = XmlHelper.XpathValueNullToEmpty(n, "city");
                client.zip_code = XmlHelper.XpathValueNullToEmpty(n, "zip_code");
                client.country_rcd = XmlHelper.XpathValueNullToEmpty(n, "country_rcd");
                client.phone_mobile = XmlHelper.XpathValueNullToEmpty(n, "phone_mobile");
                client.phone_home = XmlHelper.XpathValueNullToEmpty(n, "phone_home");
                client.phone_business = XmlHelper.XpathValueNullToEmpty(n, "phone_business");
                client.state = XmlHelper.XpathValueNullToEmpty(n, "state");
                client.passenger_profile_id = new Guid(XmlHelper.XpathValueNullToEmpty(n, "passenger_profile_id"));
                client.employee_number = XmlHelper.XpathValueNullToEmpty(n, "employee_number");
                client.member_level_rcd = XmlHelper.XpathValueNullToEmpty(n, "member_level_rcd");
                client.date_of_birth = XmlHelper.XpathValueNullToDateTime(n, "date_of_birth");
                client.nationality_rcd = XmlHelper.XpathValueNullToEmpty(n, "nationality_rcd");
                client.document_type_rcd = XmlHelper.XpathValueNullToEmpty(n, "document_type_rcd");
                client.passport_number = XmlHelper.XpathValueNullToEmpty(n, "passport_number");
                client.passport_issue_place = XmlHelper.XpathValueNullToEmpty(n, "passport_issue_place");
                client.passport_issue_date = XmlHelper.XpathValueNullToDateTime(n, "passport_issue_date");
                client.passport_expiry_date = XmlHelper.XpathValueNullToDateTime(n, "passport_expiry_date");
                client.passport_issue_country_rcd = XmlHelper.XpathValueNullToEmpty(n, "passport_issue_country_rcd");
                client.passport_issue_place = XmlHelper.XpathValueNullToEmpty(n, "passport_issue_place");
                client.redress_number = XmlHelper.XpathValueNullToEmpty(n, "redress_number");

                clients.Add(client);
            }

        }
        private void FillClientInformation(Clients clientPassenger, Passenger passenger)
        {
            if (clientPassenger != null && clientPassenger.Count > 0)
            {
                if (passenger != null)
                {
                    for (int i = 0; i < clientPassenger.Count; i++)
                    {
                        if (clientPassenger[i].passenger_profile_id.Equals(passenger.passenger_profile_id))
                        {
                            passenger.lastname = clientPassenger[i].lastname.ToUpper();
                            passenger.firstname = clientPassenger[i].firstname.ToUpper();
                            passenger.title_rcd = clientPassenger[i].title_rcd.ToUpper();
                            passenger.date_of_birth = clientPassenger[i].date_of_birth;
                            passenger.gender_type_rcd = clientPassenger[i].gender_type_rcd;
                            passenger.nationality_rcd = clientPassenger[i].nationality_rcd;
                            passenger.passport_number = clientPassenger[i].passport_number;
                            passenger.passport_issue_date = clientPassenger[i].passport_issue_date;
                            passenger.passport_expiry_date = clientPassenger[i].passport_expiry_date;
                            passenger.passport_issue_place = clientPassenger[i].passport_issue_place;
                            passenger.passport_birth_place = clientPassenger[i].passport_birth_place;
                            passenger.passport_issue_country_rcd = clientPassenger[i].passport_issue_country_rcd;
                            passenger.vip_flag = clientPassenger[i].vip_flag;
                            passenger.passenger_weight = clientPassenger[i].passenger_weight;
                            passenger.document_type_rcd = clientPassenger[i].document_type_rcd;
                            passenger.passenger_role_rcd = clientPassenger[i].passenger_role_rcd;
                            passenger.member_number = clientPassenger[i].member_number;
                            passenger.client_number = clientPassenger[i].client_number;

                            // set member_level
                            passenger.member_level_rcd = clientPassenger[i].member_level_rcd;
                            passenger.known_traveler_number = clientPassenger[i].known_traveler_number;
                            passenger.medical_conditions = clientPassenger[i].medical_conditions;
                            passenger.comment = clientPassenger[i].comment;
                            passenger.middlename = clientPassenger[i].middlename;

                            break;
                        }
                    }
                }
            }
        }
        private bool SpecialServiceExist(Services services, string feeRcd, Guid bookingSegmentId, Guid passengerId)
        {
            if (services != null)
            {
                for (int i = 0; i < services.Count; i++)
                {
                    if (services[i].special_service_rcd.ToUpper().Equals(feeRcd.ToUpper()) == true &&
                        services[i].booking_segment_id.Equals(bookingSegmentId) == true &&
                        services[i].passenger_id.Equals(passengerId) == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private string GetAvailability(string strAgencyCode,
                                       string strPassword,
                                       string strOrigin,
                                       string strDestination,
                                       string strDepartFrom,
                                       string strDepartTo,
                                       string strReturnFrom,
                                       string strReturnTo,
                                       short iAdult,
                                       short iChild,
                                       short iInfant,
                                       short iOther,
                                       string strBookingClass,
                                       string strBoardingClass,
                                       string strPromoCode,
                                       string searchTypeCode,
                                       string strLanguageCode,
                                       string strOtherPassengerType,
                                       string strCurrency)
        {
            ServiceClient obj = new ServiceClient();
            string result = null;
            string AvaiString = string.Empty;
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd;
            string strCurreny = strCurrency;
            int iMaximumDateRange = 0;

            string strSearchType = "FARE";
            bool bLowestGroup = false;
            bool bLowest = false;
            bool bLowestClass = false;
            bool bSort = false;
            bool bClose = false;
            bool bDelete = false;
            bool bGroupFare = false;

           // bool applyFareLogic = false;

            string AvaiErrorResult = "";
            try
            {
                if (strOrigin.Length == 0 || strDestination.Length == 0)
                {
                    result = Utils.ErrorXml("200", "Origin and Destination are Required");

                    dtEnd = DateTime.Now;
                   // Utils.SaveLog("FlightAvailability", dtStart, dtEnd, result, AvaiString);
                    AvaiErrorResult = "200";

                }
                else if (strDepartFrom.Length == 0 || strDepartTo.Length == 0)
                {
                    result = Utils.ErrorXml("201", "Departure From and Departure To are Required");
                    dtEnd = DateTime.Now;
                   // Utils.SaveLog("FlightAvailability", dtStart, dtEnd, result, AvaiString);
                    AvaiErrorResult = "201";
                }
                //else if (iAdult == 0 && iChild == 0 )
                else if (iAdult == 0 && iChild == 0 && iOther == 0)
                {
                    result = Utils.ErrorXml("202", "At least one Adult is required");
                    dtEnd = DateTime.Now;
                    // Utils.SaveLog("FlightAvailability", dtStart, dtEnd, result, AvaiString);
                    AvaiErrorResult = "202";
                }
                else if (strAgencyCode.Length == 0)
                {
                    result = Utils.ErrorXml("203", "Agency Code is required");
                    dtEnd = DateTime.Now;
                    // Utils.SaveLog("FlightAvailability", dtStart, dtEnd, result, AvaiString);
                    AvaiErrorResult = "203";
                }
                else if (strPassword.Length == 0)
                {
                    result = Utils.ErrorXml("204", "Agency Password is required");
                    dtEnd = DateTime.Now;
                  //  Utils.SaveLog("FlightAvailability", dtStart, dtEnd, result, AvaiString);
                    AvaiErrorResult = "204";
                }
                else
                {
                    string newline = Environment.NewLine;
                    AvaiString = "Agency           : " + strAgencyCode + newline +
                                 "Password         : " + strPassword + newline +
                                 "Origin           : " + strOrigin + newline +
                                 "Destination      : " + strDestination + newline +
                                 "Departure From   : " + strDepartFrom + newline +
                                 "Departure To     : " + strDepartTo + newline +
                                 "Arrival From     : " + strReturnFrom + newline +
                                 "Arrival To       : " + strReturnTo + newline +
                                 "Adult            : " + iAdult + newline +
                                 "Child            : " + iChild + newline +
                                 "Infant           : " + iInfant + newline +
                                 "Other            : " + iOther + newline +
                                 "Booking Class    : " + strBookingClass + newline +
                                 "Boarding Class   : " + strBoardingClass + newline +
                                 "Promocode        : " + strPromoCode + newline +
                                 "Farelogic        : " + "true" + newline +
                                 "OtherPassengerType : " + strOtherPassengerType + newline;


                    DateTime dtDepartFrom = DateTime.MinValue;
                    DateTime dtDepartTo = DateTime.MinValue;
                    DateTime dtReturnFrom = DateTime.MinValue;
                    DateTime dtReturnTo = DateTime.MinValue;

                    if (string.IsNullOrEmpty(strLanguageCode))
                    {
                        strLanguageCode = "EN";
                    }

                    if (System.Configuration.ConfigurationManager.AppSettings["MaximumSearchDayRange"] != null)
                    {
                        //Maximun search day ranger in day.
                        iMaximumDateRange = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["MaximumSearchDayRange"]);
                    }

                    int intDepartFrom = 0;
                    if (strDepartFrom.Length == 8 && int.TryParse(strDepartFrom, out intDepartFrom))
                    {
                        string a = strDepartFrom.Substring(0, 4);
                        string b = strDepartFrom.Substring(4, 2);
                        string c = strDepartFrom.Substring(6, 2);
                        strDepartFrom = string.Join("-", new string[] { a, b, c });
                        DateTime.TryParse(strDepartFrom, out dtDepartFrom);
                    }

                    int intDepartTo = 0;
                    if (strDepartTo.Length == 8 && int.TryParse(strDepartTo, out intDepartTo))
                    {
                        string a = strDepartTo.Substring(0, 4);
                        string b = strDepartTo.Substring(4, 2);
                        string c = strDepartTo.Substring(6, 2);
                        strDepartTo = string.Join("-", new string[] { a, b, c });
                        DateTime.TryParse(strDepartTo, out dtDepartTo);
                        if (iMaximumDateRange != 0 && DataHelper.DateDifferent(dtDepartFrom, dtDepartTo).Days > iMaximumDateRange)
                        {
                            dtDepartTo = dtDepartFrom.AddDays(iMaximumDateRange);
                        }
                    }

                    int intReturnFrom = 0;
                    if (strReturnFrom.Length == 8 && int.TryParse(strReturnFrom, out intReturnFrom))
                    {
                        string a = strReturnFrom.Substring(0, 4);
                        string b = strReturnFrom.Substring(4, 2);
                        string c = strReturnFrom.Substring(6, 2);
                        strReturnFrom = string.Join("-", new string[] { a, b, c });
                        DateTime.TryParse(strReturnFrom, out dtReturnFrom);
                    }

                    int intReturnTo = 0;
                    if (strReturnTo.Length == 8 && int.TryParse(strReturnTo, out intReturnTo))
                    {
                        string a = strReturnTo.Substring(0, 4);
                        string b = strReturnTo.Substring(4, 2);
                        string c = strReturnTo.Substring(6, 2);
                        strReturnTo = string.Join("-", new string[] { a, b, c });
                        DateTime.TryParse(strReturnTo, out dtReturnTo);

                        if (iMaximumDateRange != 0 && DataHelper.DateDifferent(dtReturnFrom, dtReturnTo).Days > iMaximumDateRange)
                        {
                            dtReturnTo = dtReturnFrom.AddDays(iMaximumDateRange);
                        }
                    }


                    if (searchTypeCode == "AF")
                    {
                        bLowestGroup = true;
                        bLowest = true;
                        bLowestClass = true;
                        bSort = false;
                        bClose = false;
                        bDelete = true;
                        bGroupFare = false;
                    }
                    else if (searchTypeCode == "LF")
                    {
                        // bLowestGroup = true;
                        bLowestGroup = false;
                        bLowest = true;
                        bLowestClass = false;
                        //bSort = false;
                        bSort = true;
                        bClose = false;
                        bDelete = true;
                        bGroupFare = false;
                    }
                    else if (searchTypeCode == "GF")
                    {
                        bLowestGroup = true;
                        bLowest = true;
                        bLowestClass = true;
                        bSort = false;
                        bClose = false;
                        bDelete = true;
                        bGroupFare = true;
                    }
                    else if (searchTypeCode == "SF")
                    {
                        bLowestGroup = true;
                        bLowest = true;
                        bLowestClass = true;
                        bSort = false;
                        bClose = false;
                        bDelete = true;
                        bGroupFare = false;
                        strSearchType = "SF";
                    }

                    if (searchTypeCode == "PF")
                    {
                        bLowestGroup = true;
                        bLowest = true;
                        bLowestClass = true;
                        bSort = false;
                        bClose = false;
                        strSearchType = "POINT";
                    }

                    result = obj.GetAvailabilityAgencyLogin(strOrigin.ToUpper(),
                                                            strDestination.ToUpper(),
                                                            dtDepartFrom,
                                                            dtDepartTo,
                                                            dtReturnFrom,
                                                            dtReturnTo,
                                                            DateTime.MinValue,
                                                            iAdult,
                                                            iChild,
                                                            iInfant,
                                                            iOther,
                                                            strOtherPassengerType,
                                                            strBoardingClass,
                                                            strBookingClass,
                                                            string.Empty,
                                                            strAgencyCode.ToUpper(),
                                                            strPassword,
                                                            string.Empty,
                                                            string.Empty, 0.0,
                                                            false,
                                                            false,
                                                            false,
                                                            true,
                                                            true,
                                                            false,
                                                            bGroupFare,
                                                            false,
                                                            false,
                                                            true,  //apply fare logic
                                                            false,
                                                            string.Empty,
                                                            dtDepartFrom,
                                                            dtReturnFrom,
                                                            string.Empty,
                                                            true,
                                                            false,
                                                            string.Empty,
                                                            strPromoCode,
                                                            1, // farelogic
                                                            strSearchType,
                                                            bLowest,
                                                            bLowestClass,
                                                            bLowestGroup,
                                                            bClose,
                                                            bSort,
                                                            bDelete,
                                                            false,//skipfarelogic
                                                            strLanguageCode,
                                                            DataHelper.GetClientIpAddress(),
                                                            ref strCurreny,
                                                            false);

                    if (result == "205")
                    {
                        result = Utils.ErrorXml("205", "Agency Information not match.");
                        AvaiErrorResult = "205";
                        dtEnd = DateTime.Now;
                    }
                    else
                    {
                        if (result.IndexOf("Availability") == -1)
                            result = tikSystem.Web.Library.SecurityHelper.DecompressString(result);

                        XPathDocument xmlDoc = new XPathDocument(new StringReader(result));
                        Library objLi = new Library();

                        result = "<Availability>" +
                                    "<AvailabilityOutbound>" +
                                     GroupXML(xmlDoc, "Availability/AvailabilityOutbound/AvailabilityFlight", strCurreny).Replace("\r\n", "") +
                                    "</AvailabilityOutbound>" +
                                    "<AvailabilityReturn>" +
                                     GroupXML(xmlDoc, "Availability/AvailabilityReturn/AvailabilityFlight", strCurreny).Replace("\r\n", "") +
                                    "</AvailabilityReturn>" +
                                 "</Availability>";

                        if (!string.IsNullOrEmpty(result) && result.Length > 100) AvaiErrorResult = "000";
                    }
                }
            }
            catch (Exception e)
            {
                result = Utils.ErrorXml("103", "General Error");

                dtEnd = DateTime.Now;
                // Utils.SaveLog("FlightAvailability", dtStart, dtEnd, e.Message, AvaiString);
                AvaiErrorResult = "103";
            }
            finally
            {
                obj = null;
            }

            //Save Process Log
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == false &&
                    Convert.ToByte(System.Configuration.ConfigurationManager.AppSettings["LogProcess"]) == 1)
                {
                    Utils.SaveProcessLog("GetAvailability", dtStart, DateTime.Now, strAgencyCode + "|"+ strPassword + "\n" + strOrigin + "|" + strDestination + "|" + strDepartFrom + "|" + strDepartTo + "\n" + "AvaiErrorResult: " + AvaiErrorResult );
                }

            return result;
        }
        private string InternalPaymentErrorMapping(XPathNavigator nv)
        {
            string strErrorCode = XmlHelper.XpathValueNullToEmpty(nv, "ErrorCode");
            if (strErrorCode == "202")
            {
                return Utils.ErrorXml("507", "Internal inventory lock is timeout.");
            }
            else if (strErrorCode == "203")
            {
                return Utils.ErrorXml("508", "Duplicate seat.");
            }
            else if (strErrorCode == "204")
            {
                return Utils.ErrorXml("509", "Data access failed.");
            }
            else if (strErrorCode == "205")
            {
                return Utils.ErrorXml("510", "Booking in-use.");
            }
            else if (strErrorCode == "206")
            {
                return Utils.ErrorXml("511", "Error read booking");
            }
            else if (strErrorCode == "200")
            {
                return Utils.ErrorXml("107", "Infant over limit");
            }
            else if (strErrorCode == string.Empty)
            {
                return Utils.ErrorXml("512", "Payment request timeout.");
            }
            else
            {
                return Utils.ErrorXml(strErrorCode, XmlHelper.XpathValueNullToEmpty(nv, "ResponseText"));
            }
        }

        private bool ClientValidation(BaseClientMessage ClientRequest, ref string errMessage, string clientProfileId, bool isCreateClient)
        {
            tikSystem.Web.Library.ServiceClient srvClient = new tikSystem.Web.Library.ServiceClient();
            if (ClientRequest != null)
            {
                if (string.IsNullOrEmpty(ClientRequest.title_rcd))
                {
                    errMessage = Utils.ErrorXml("6102", "TitleRcd Required");
                    return false;
                }
                else if (string.IsNullOrEmpty(ClientRequest.Firstname))
                {
                    errMessage = Utils.ErrorXml("6103", "Firstname Required");
                    return false;
                }
                else if (string.IsNullOrEmpty(ClientRequest.Lastname))
                {
                    errMessage = Utils.ErrorXml("6104", "Lastname Required");
                    return false;
                }
                else if (string.IsNullOrEmpty(ClientRequest.date_of_birth) && ClientRequest.passenger_type_rcd.Equals("CHD"))
                {
                    errMessage = Utils.ErrorXml("6105", "DateOfBirth Required");
                    return false;
                }
                else if (tikSystem.Web.Library.DataHelper.DateValid(ClientRequest.date_of_birth) == false && ClientRequest.passenger_type_rcd.Equals("CHD"))
                {
                    errMessage = Utils.ErrorXml("6106", "Invalid DateOfBirth");
                    return false;
                }
                else if (!ClientRequest.passenger_type_rcd.Equals("ADULT") && !ClientRequest.passenger_type_rcd.Equals("CHD"))
                {
                    errMessage = Utils.ErrorXml("6107", "PassengerTypeRcd Required");
                    return false;
                }
                else if (string.IsNullOrEmpty(ClientRequest.contact_email))
                {
                    errMessage = Utils.ErrorXml("6108", "ContactEmail Required");
                    return false;
                }
                else if (string.IsNullOrEmpty(ClientRequest.client_password))
                {
                    errMessage = Utils.ErrorXml("6109", "ClientPassword Required");
                    return false;
                }
                else if (ClientRequest.client_password.Length > 60)
                {
                    errMessage = Utils.ErrorXml("6112", "ClientPassword is not greater than 60 characters long");
                    return false;
                }
                else if (string.IsNullOrEmpty(ClientRequest.gender_type_rcd))
                {
                    errMessage = Utils.ErrorXml("6110", "GenderTypeRcd Required");
                    return false;
                }
                else if (isCreateClient)
                {
                    if (!srvClient.CheckUniqueMailAddress(ClientRequest.contact_email, clientProfileId))
                    {
                        errMessage = Utils.ErrorXml("6111", "Contact email is already in use");
                        return false;
                    }
                }
                else if (!isCreateClient)
                {
                    Clients objMainProfile = new Clients();
                    //Read main profile.
                    objMainProfile.Read(clientProfileId,
                                        string.Empty,
                                        string.Empty,
                                        false);

                    if (objMainProfile.Count > 0)
                    {
                        if (objMainProfile[0].contact_email != ClientRequest.contact_email)
                        {
                            if (!srvClient.CheckUniqueMailAddress(ClientRequest.contact_email, clientProfileId))
                            {
                                errMessage = Utils.ErrorXml("6111", "Contact email is already in use");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        errMessage = Utils.ErrorXml("9000", "Client profile not found.");
                        return false;
                    }
                }
            }
            else
            {
                errMessage = Utils.ErrorXml("6101", "Object ClientRequest is null");
                return false;
            }
            return true;
        }

        private bool ClientValidationEdit(BaseClientMessage ClientRequest, ref string errMessage, string clientProfileId, bool isCreateClient)
        {
            tikSystem.Web.Library.ServiceClient srvClient = new tikSystem.Web.Library.ServiceClient();
            bool result = false;

            if (ClientRequest != null)
            {
                if (string.IsNullOrEmpty(clientProfileId) || clientProfileId == "00000000-0000-0000-0000-000000000000")
                {
                    errMessage = Utils.ErrorXml("6113", "clientProfileId Required");
                    result = false;
                }
                else
                {
                    // check client is exist
                    Clients objMainProfile = new Clients();
                    //Read main profile.
                    objMainProfile.Read(clientProfileId,
                                        string.Empty,
                                        string.Empty,
                                        false);

                    if (objMainProfile.Count > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        errMessage = Utils.ErrorXml("9000", "Client profile not found.");
                        result = false;
                    }
                }
            }

            return result;
        }
        private bool PassengerClientValidation(BasePassengerProfileMessage PassengerRequest, ref string errMessage)
        {
            if (PassengerRequest != null)
            {
                if (string.IsNullOrEmpty(PassengerRequest.title_rcd))
                {
                    errMessage = Utils.ErrorXml("6302", "TitleRcd Required");
                    return false;
                }
                else if (string.IsNullOrEmpty(PassengerRequest.Firstname))
                {
                    errMessage = Utils.ErrorXml("6303", "FirstName Required");
                    return false;
                }
                else if (string.IsNullOrEmpty(PassengerRequest.Lastname))
                {
                    errMessage = Utils.ErrorXml("6304", "LastName Required");
                    return false;
                }
                else if (string.IsNullOrEmpty(PassengerRequest.passenger_type_rcd))
                {
                    errMessage = Utils.ErrorXml("6305", "PassengerTypeRcd Required");
                    return false;
                }

                else if (string.IsNullOrEmpty(PassengerRequest.passenger_role_rcd))
                {
                    errMessage = Utils.ErrorXml("6307", "PassengerRoleRcd Required");
                    return false;
                }
                else if (string.IsNullOrEmpty(PassengerRequest.date_of_birth) && PassengerRequest.passenger_type_rcd.Equals("CHD"))
                {
                    errMessage = Utils.ErrorXml("6308", "DateOfBirth Required");
                    return false;
                }
                else if (!tikSystem.Web.Library.DataHelper.DateValid(PassengerRequest.date_of_birth) && PassengerRequest.passenger_type_rcd.Equals("CHD"))
                {
                    errMessage = Utils.ErrorXml("6309", "Invalid DateOfBirth");
                    return false;
                }
                else if (string.IsNullOrEmpty(PassengerRequest.gender_type_rcd))
                {
                    errMessage = Utils.ErrorXml("6310", "GenderTypeRcd Required");
                    return false;
                }
            }
            else
            {
                errMessage = Utils.ErrorXml("6301", "Object PassengerRequest is null");
                return false;
            }
            return true;
        }

        private string GetErrorPayment(string strResult, string strXml, DateTime dtStart, DateTime dtEnd)
        {
            if (string.IsNullOrEmpty(strResult) == false)
            {
                XPathDocument xmlCr = new XPathDocument(new StringReader(strResult));
                XPathNavigator nvCr = xmlCr.CreateNavigator();
                string strErrorCode = string.Empty;
                if (nvCr.Select("PaymentError/PaymentError").Count > 0)
                {
                    foreach (XPathNavigator nCr in nvCr.Select("PaymentError/PaymentError"))
                    {
                        strResult = InternalPaymentErrorMapping(nCr);
                        dtEnd = DateTime.Now;
                        Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, strXml);
                    }
                }
                else if (nvCr.Select("NewDataSet/Payments").Count > 0)
                {
                    foreach (XPathNavigator nCr in nvCr.Select("NewDataSet/Payments"))
                    {
                        strResult = InternalPaymentErrorMapping(nCr);
                        dtEnd = DateTime.Now;
                        Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, strXml);
                    }

                }
                else
                {
                    //Clear user cache baggage fee.
                    ClearBagggaeSession(true);
                    ClearBagggaeSession(false);
                    strResult = Utils.ErrorXml("000", "Success Request Transaction");
                }
            }
            else
            {
                strResult = Utils.ErrorXml("501", "Save Booking Failed");

                dtEnd = DateTime.Now;
                Utils.SaveLog("BookingSave", dtStart, dtEnd, strResult, strXml);
            }

            return strResult;
        }
        #endregion

    }
}
