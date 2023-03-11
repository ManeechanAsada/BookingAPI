using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Data;
using Avantik.Web.Service.COMHelper;
using Avantik.Web.Service.Helpers.Database;

namespace tikAeroWebMain
{
    public class ComplusInventory : RunComplus
    {
        public ComplusInventory() : base() { }
        public DataSet GetAirlines(string strLanguage)
        {
            tikAeroProcess.Inventory objInventory = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objInventory = new tikAeroProcess.Inventory(); }

                rs = objInventory.GetAirlines(ref strLanguage);

                //Convert Recordset to Dataset
                ds = RecordsetToDataset(rs, "Airline");
            }
            catch
            { }
            finally
            {
                if (objInventory != null)
                {
                    Marshal.FinalReleaseComObject(objInventory);
                    objInventory = null;
                }

                ClearRecordset(ref rs);
            }
            return ds;
        }
        public tikSystem.Web.Library.Routes GetOrigins(string strLanguage, bool b2cFlag, bool b2bFlag, bool b2EFlag, bool b2SFlag, bool APIFlag)
        {
            tikAeroProcess.Inventory objInventory = null;
            ADODB.Recordset rs = null;
            try
            {
                tikSystem.Web.Library.Routes routes = new tikSystem.Web.Library.Routes();
                if (string.IsNullOrEmpty(_server) == false)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objInventory = new tikAeroProcess.Inventory(); }

                rs = objInventory.GetOrigins(ref strLanguage, ref b2cFlag, ref b2bFlag, ref b2EFlag, ref b2SFlag, ref APIFlag);

                //convert Recordset to Object
                if (rs != null && rs.RecordCount > 0)
                {
                    tikSystem.Web.Library.Route r;
                    rs.MoveFirst();
                    while (!rs.EOF)
                    {
                        r = new tikSystem.Web.Library.Route();
                        r.origin_rcd = RecordsetHelper.ToString(rs, "origin_rcd");
                        r.display_name = RecordsetHelper.ToString(rs, "display_name");
                        r.country_rcd = RecordsetHelper.ToString(rs, "country_rcd");
                        r.currency_rcd = RecordsetHelper.ToString(rs, "currency_rcd");
                        r.routes_tot = RecordsetHelper.ToByte(rs, "routes_tot");
                        r.routes_avl = RecordsetHelper.ToByte(rs, "routes_avl");
                        r.routes_b2c = RecordsetHelper.ToByte(rs, "routes_b2c");
                        r.routes_b2b = RecordsetHelper.ToByte(rs, "routes_b2b");
                        r.routes_b2s = RecordsetHelper.ToByte(rs, "routes_b2s");
                        r.routes_api = RecordsetHelper.ToByte(rs, "routes_api");
                        r.routes_b2t = RecordsetHelper.ToByte(rs, "routes_b2t");
                        routes.Add(r);
                        r = null;
                        rs.MoveNext();
                    }
                }

                return routes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objInventory != null)
                {
                    Marshal.FinalReleaseComObject(objInventory);
                    objInventory = null;
                }
                RecordsetHelper.ClearRecordset(ref rs);
                base.Dispose();
            }
        }
        public tikSystem.Web.Library.Routes GetDestination(string strLanguage, bool b2cFlag, bool b2bFlag, bool b2EFlag, bool b2SFlag, bool APIFlag)
        {
            tikAeroProcess.Inventory objInventory = null;
            ADODB.Recordset rs = null;
            try
            {
                tikSystem.Web.Library.Routes routes = new tikSystem.Web.Library.Routes();
                if (string.IsNullOrEmpty(_server) == false)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objInventory = new tikAeroProcess.Inventory(); }

                rs = objInventory.GetDestinations(ref strLanguage, ref b2cFlag, ref b2bFlag, ref b2EFlag, ref b2SFlag, ref APIFlag);
                //convert Recordset to Object
                if (rs != null && rs.RecordCount > 0)
                {
                    tikSystem.Web.Library.Route r;
                    rs.MoveFirst();
                    while (!rs.EOF)
                    {
                        r = new tikSystem.Web.Library.Route();
                        r.origin_rcd = RecordsetHelper.ToString(rs, "origin_rcd");
                        r.destination_rcd = RecordsetHelper.ToString(rs, "destination_rcd");
                        r.segment_change_fee_flag = RecordsetHelper.ToBoolean(rs, "segment_change_fee_flag");
                        r.transit_flag = RecordsetHelper.ToBoolean(rs, "transit_flag");
                        r.direct_flag = RecordsetHelper.ToBoolean(rs, "direct_flag");
                        r.avl_flag = RecordsetHelper.ToBoolean(rs, "avl_flag");
                        r.b2c_flag = RecordsetHelper.ToBoolean(rs, "b2c_flag");
                        r.b2b_flag = RecordsetHelper.ToBoolean(rs, "b2b_flag");
                        r.b2t_flag = RecordsetHelper.ToBoolean(rs, "b2t_flag");
                        r.day_range = RecordsetHelper.ToInt16(rs, "day_range");
                        r.show_redress_number_flag = RecordsetHelper.ToBoolean(rs, "show_redress_number_flag");
                        r.require_passenger_title_flag = RecordsetHelper.ToBoolean(rs, "require_passenger_title_flag");
                        r.require_passenger_gender_flag = RecordsetHelper.ToBoolean(rs, "require_passenger_gender_flag");
                        r.require_date_of_birth_flag = RecordsetHelper.ToBoolean(rs, "require_date_of_birth_flag");
                        r.require_document_details_flag = RecordsetHelper.ToBoolean(rs, "require_document_details_flag");
                        r.require_passenger_weight_flag = RecordsetHelper.ToBoolean(rs, "require_passenger_weight_flag");
                        r.special_service_fee_flag = RecordsetHelper.ToBoolean(rs, "special_service_fee_flag");
                        r.show_insurance_on_web_flag = RecordsetHelper.ToBoolean(rs, "show_insurance_on_web_flag");
                        r.display_name = RecordsetHelper.ToString(rs, "display_name");
                        r.country_rcd = RecordsetHelper.ToString(rs, "destination_country_rcd");
                        r.currency_rcd = RecordsetHelper.ToString(rs, "currency_rcd");

                        routes.Add(r);
                        r = null;
                        rs.MoveNext();
                    }
                }

                return routes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objInventory != null)
                {
                    Marshal.FinalReleaseComObject(objInventory);
                    objInventory = null;
                }

                RecordsetHelper.ClearRecordset(ref rs);
                base.Dispose();
            }
        }
        public void GetFilterAvailability(string Origin,
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
                                          bool bSkipFarelogic,
                                          string strLanguage,
                                          string strIpAddress,
                                          ref string xmlOutward,
                                          ref string xmlReturn)
        {

            bool bReturn = false;
            tikAeroProcess.Inventory objInventory = null;

            ADODB.Recordset rsOutward = null;
            ADODB.Recordset rsReturn = null;
            ADODB.Recordset rsLogic = null;

            try
            {
                DateTime dtBookingDate = DateTime.MinValue;

                
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objInventory = new tikAeroProcess.Inventory(); }

                if (objInventory != null)
                {
                    rsOutward = objInventory.GetAvailability(Origin,
                                                             Destination,
                                                             DepartDateFrom,
                                                             DepartDateTo,
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
                                                             rsReturn,
                                                             bMapWithFares,
                                                             bReturnRefundable,
                                                             strReturnDayTimeIndicator,
                                                             PromotionCode,
                                                             strFareType,
                                                             strLanguage,
                                                             strIpAddress);

                    //If have return flight
                    if (ReturnDateFrom != DateTime.MinValue)
                    {
                        bReturn = true;
                        rsReturn = objInventory.GetAvailability(Destination,
                                                                Origin,
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
                                                              rsReturn,
                                                              bMapWithFares,
                                                              bReturnRefundable,
                                                              strReturnDayTimeIndicator,
                                                              PromotionCode,
                                                              strFareType,
                                                              strLanguage,
                                                              strIpAddress);
                    }


                    objInventory.FilterAvailability(ref rsOutward,
                                                    Adult,
                                                    Child,
                                                    Infant,
                                                    Other,
                                                    ref rsReturn,
                                                    ref rsLogic,
                                                    Origin,
                                                    Destination,
                                                    DepartDateFrom,
                                                    ReturnDateFrom,
                                                    dtBookingDate,
                                                    bReturn,
                                                    bLowest,
                                                    bLowestClass,
                                                    bLowestGroup,
                                                    bShowClosed,
                                                    bSort,
                                                    bDelet,
                                                    bSkipFarelogic);


                }

                if (rsOutward != null && rsOutward.RecordCount > 0)
                {
                    xmlOutward = RecordsetToDataset(rsOutward, "AvailabilityFlight").GetXml();
                }
                if (rsReturn != null && rsReturn.RecordCount > 0)
                {
                    xmlReturn = RecordsetToDataset(rsReturn, "AvailabilityFlight").GetXml();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Marshal.FinalReleaseComObject(objInventory);
                objInventory = null;

                ClearRecordset(ref rsOutward);
                ClearRecordset(ref rsReturn);
                ClearRecordset(ref rsLogic);
            }
        }
        public bool ReleaseFlightInventorySession(string sessionId,
                                                string flightId,
                                                string bookingClasss,
                                                string bookingId,
                                                bool releaseTimeOut,
                                                bool ReleaseInventory,
                                                bool ReleaseBookingLock)
        {
            tikAeroProcess.Inventory objInventory = null;
            bool result = false;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objInventory = new tikAeroProcess.Inventory(); }

                result = objInventory.ReleaseFlightInventorySession(ref sessionId,
                                                                    ref flightId,
                                                                    ref bookingClasss,
                                                                    ref bookingId,
                                                                    ref releaseTimeOut,
                                                                    ref ReleaseInventory,
                                                                    ref ReleaseBookingLock);

            }
            catch
            { }
            finally
            {
                if (objInventory != null)
                {
                    Marshal.FinalReleaseComObject(objInventory);
                    objInventory = null;
                }
            }

            return result;
        }
        public tikSystem.Web.Library.Documents GetDocumentType(string language)
        {
            tikAeroProcess.Inventory objInventory = null;
            ADODB.Recordset rs = null;
            tikSystem.Web.Library.Documents documents = new tikSystem.Web.Library.Documents();

            try
            {
                if (string.IsNullOrEmpty(_server) == false)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objInventory = new tikAeroProcess.Inventory(); }

                if (string.IsNullOrEmpty(language) == true)
                { language = "EN"; }
                rs = objInventory.GetDocumentType(ref language);

                //convert Recordset to Object
                if (rs != null && rs.RecordCount > 0)
                {
                    tikSystem.Web.Library.Document d;
                    rs.MoveFirst();
                    while (!rs.EOF)
                    {
                        d = new tikSystem.Web.Library.Document();

                        d.document_type_rcd = RecordsetHelper.ToString(rs, "document_type_rcd");
                        d.display_name = RecordsetHelper.ToString(rs, "display_name");

                        documents.Add(d);
                        d = null;
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
                if (objInventory != null)
                {
                    Marshal.FinalReleaseComObject(objInventory);
                    objInventory = null;
                }

                RecordsetHelper.ClearRecordset(ref rs);
                base.Dispose();
            }

            return documents;
        }
        public tikSystem.Web.Library.Countries GetCountry(string language)
        {
            tikAeroProcess.Inventory objInventory = null;
            ADODB.Recordset rs = null;
            tikSystem.Web.Library.Countries countries = new tikSystem.Web.Library.Countries();
            try
            {
                if (string.IsNullOrEmpty(_server) == false)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objInventory = new tikAeroProcess.Inventory(); }

                //set default language to EN
                if (language.Length == 0)
                { language = "EN"; }

                rs = objInventory.GetCountry(ref language);
                //convert Recordset to Object
                if (rs != null && rs.RecordCount > 0)
                {
                    tikSystem.Web.Library.Country c;
                    rs.MoveFirst();
                    while (!rs.EOF)
                    {
                        c = new tikSystem.Web.Library.Country();

                        c.country_rcd = RecordsetHelper.ToString(rs, "country_rcd");
                        c.display_name = RecordsetHelper.ToString(rs, "display_name");
                        c.status_code = RecordsetHelper.ToString(rs, "status_code");
                        c.currency_rcd = RecordsetHelper.ToString(rs, "currency_rcd");
                        c.vat_display = RecordsetHelper.ToString(rs, "vat_display");
                        c.country_code_long = RecordsetHelper.ToString(rs, "country_code_long");
                        c.country_number = RecordsetHelper.ToString(rs, "country_number");
                        c.phone_prefix = RecordsetHelper.ToString(rs, "phone_prefix");
                        c.issue_country_flag = RecordsetHelper.ToByte(rs, "issue_country_flag");
                        c.residence_country_flag = RecordsetHelper.ToByte(rs, "residence_country_flag");
                        c.nationality_country_flag = RecordsetHelper.ToByte(rs, "nationality_country_flag");
                        c.address_country_flag = RecordsetHelper.ToByte(rs, "address_country_flag");
                        c.agency_country_flag = RecordsetHelper.ToByte(rs, "agency_country_flag");

                        countries.Add(c);
                        c = null;
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
                if (objInventory != null)
                {
                    Marshal.FinalReleaseComObject(objInventory);
                    objInventory = null;
                }
                RecordsetHelper.ClearRecordset(ref rs);
                base.Dispose();
            }

            return countries;
        }

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
                                    bool bDelet,
                                    string strLanguage,
                                    string strIpAddress,
                                    bool bLowFareFinder,
                                    bool bNoVat)
        {
            System.Text.StringBuilder stb;
            bool bReturn = false;
            tikAeroProcess.Inventory objInventory = null;

            ADODB.Recordset rsOutward = null;
            ADODB.Recordset rsReturn = null;
            ADODB.Recordset rsLogic = null;

            bool bSkipFarelogic = false;

            try
            {
                DateTime dtBookingDate = DateTime.MinValue;


                if (string.IsNullOrEmpty(_server) == false)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objInventory = new tikAeroProcess.Inventory(); }


                // add staff Avai
                if (strFareType == "SF")
                {
                    bLowest = true; bLowestClass = true; bLowestGroup = true;
                    bStaffFares = true; strFareType = "FARE";
                }


                if (objInventory != null)
                {
                    rsOutward = objInventory.GetAvailability(Origin,
                                                             Destination,
                                                             DepartDateFrom,
                                                             DepartDateTo,
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
                                                             ReturnDateFrom,
                                                             ReturnDateTo,
                                                             ref rsReturn,
                                                             bMapWithFares,
                                                             bReturnRefundable,
                                                             strReturnDayTimeIndicator,
                                                             PromotionCode,
                                                             strFareType,
                                                             strLanguage,
                                                             strIpAddress,
                                                             bNoVat);




                    //Check if it is search by date range. If yes skip fare logic.
                    //if (DepartDateFrom != DepartDateTo && ReturnDateFrom != ReturnDateTo)
                    //{
                    //    bApplyFareLogic = false;
                    //}

                    if (ReturnDateFrom.Equals(DateTime.MinValue) == false && ReturnDateTo.Equals(DateTime.MinValue) == false)
                    {
                        bReturn = true;
                    }

                    //for  lowest fare
                    if (bLowestGroup == false && bLowest == true && bLowestClass == false && GroupFares == false)
                    {
                        //identify exactly select date
                        dtSelectedDateFrom = dtSelectedDateFrom.AddDays(3);

                        if (bReturn)
                            dtSelectedDateTo = dtSelectedDateTo.AddDays(3);

                        // always bApplyFareLogic == false
                        bApplyFareLogic = false;
                    }
                    else
                    {
                        // for other fare except lowest fare
                        if (bApplyFareLogic)
                        {
                            bSkipFarelogic = false;
                        }
                        else
                        {
                            bSkipFarelogic = true;
                        }
                    }


                        // filter Avai
                        objInventory.FilterAvailability(ref rsOutward,
                                                        Adult,
                                                        Child,
                                                        Infant,
                                                        Other,
                                                        ref rsReturn,
                                                        ref rsLogic,
                                                        Origin,
                                                        Destination,
                                                        dtSelectedDateFrom,
                                                        dtSelectedDateTo,
                                                        dtBookingDate,
                                                        false,  //bReturn
                                                        bLowest,
                                                        bLowestClass,
                                                        bLowestGroup, //bLowestGroup
                                                        bShowClosed,
                                                        bSort, //bSort
                                                        bDelet,
                                                        bSkipFarelogic,
                                                        bReturnRefundable);

                }

                stb = new System.Text.StringBuilder();
                using (System.IO.StringWriter stw = new System.IO.StringWriter(stb))
                {
                    System.Xml.XmlWriterSettings writerSettings = new System.Xml.XmlWriterSettings();
                    writerSettings.OmitXmlDeclaration = true;
                    
                    using (System.Xml.XmlWriter xtw = System.Xml.XmlWriter.Create(stw, writerSettings))
                    {
                        //Get Array of Field.
                        //Get Array of data.
                        Array arrOutBound = null;
                        Array arrReturn = null;
                        
                        xtw.WriteStartElement("Availability");
                        {
                            System.Collections.Specialized.StringDictionary srrFieldList = GetAvailabilityColumn(rsOutward);
                            if (srrFieldList != null)
                            {
                                //Start Outward******************************************************
                                xtw.WriteStartElement("AvailabilityOutbound");
                                if ((rsOutward == null) == false && rsOutward.RecordCount > 0)
                                {
                                    //Get Field Array
                                    arrOutBound = (Array)rsOutward.GetRows();
                                    CreateAvailabilityXml(xtw, srrFieldList, arrOutBound);
                                }
                                xtw.WriteEndElement();
                                //AvailabilityOutbound

                                //Start Return******************************************************
                                xtw.WriteStartElement("AvailabilityReturn");
                                if ((rsReturn == null) == false && rsReturn.RecordCount > 0)
                                {
                                    arrReturn = (Array)rsReturn.GetRows();
                                    CreateAvailabilityXml(xtw, srrFieldList, arrReturn);
                                }
                                xtw.WriteEndElement();
                                //AvailabilityReturn
                            }
                        }
                        xtw.WriteEndElement();
                        //Availability
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Marshal.FinalReleaseComObject(objInventory);
                objInventory = null;

                ClearRecordset(ref rsOutward);
                ClearRecordset(ref rsReturn);
                ClearRecordset(ref rsLogic);
            }

            if (stb != null && stb.Length > 0)
            {
               return tikSystem.Web.Library.SecurityHelper.CompressString(stb.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

        public void FilterAvailability(ref string xmlOutbound,
                                        ref short iAdult,
                                        ref short iChild,
                                        ref short iInfant,
                                        ref short iOther,
                                        ref string xmlReturn,
                                        ref string xmlLogic,
                                        ref string strOrigin,
                                        ref string strDestination,
                                        ref DateTime dteOutbound,
                                        ref DateTime dteReturn,
                                        ref DateTime dteBooking,
                                        ref bool bReturn,
                                        ref bool bLowest,
                                        ref bool bLowestClass,
                                        ref bool bLowestGroup,
                                        ref bool bShowClosed,
                                        ref bool bSort,
                                        ref bool bDelet)
        {
            //Convert Datatable to recordset
            ADODB.Recordset rsOutbound = DatasetToRecordset(xmlOutbound);
            ADODB.Recordset rsReturn = DatasetToRecordset(xmlReturn);
            ADODB.Recordset rsLogic = DatasetToRecordset(xmlLogic);

            tikAeroProcess.Inventory objInventory = null;
            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objInventory = new tikAeroProcess.Inventory(); }

                if (objInventory != null)
                {
                    objInventory.FilterAvailability(ref rsOutbound,
                                                    ref iAdult,
                                                    ref iChild,
                                                    ref iInfant,
                                                    ref iOther,
                                                    ref rsReturn,
                                                    ref rsLogic,
                                                    ref strOrigin,
                                                    ref strDestination,
                                                    ref dteOutbound,
                                                    ref dteReturn,
                                                    ref dteBooking,
                                                    ref bReturn,
                                                    ref bLowest,
                                                    ref bLowestClass,
                                                    ref bLowestGroup,
                                                    ref bShowClosed,
                                                    ref bSort,
                                                    ref bDelet);

                    if (rsOutbound == null)
                    { xmlOutbound = null; }
                    else
                    {
                        //Convert Recordset to DataTable
                        xmlOutbound = RecordsetToDataset(rsOutbound, "AvailabilityFlight").GetXml();
                    }

                    if (rsReturn == null)
                    { xmlReturn = null; }
                    else
                    {
                        //Convert Recordset to DataTable
                        xmlReturn = RecordsetToDataset(rsReturn, "AvailabilityFlight").GetXml();
                    }

                    if (rsLogic == null)
                    { xmlLogic = null; }
                    else
                    {
                        //Convert Recordset to DataTable
                        xmlLogic = RecordsetToDataset(rsLogic, "Logic").GetXml();
                    }
                }

                
            }
            catch
            { }
            finally
            {
                Marshal.FinalReleaseComObject(objInventory);
                objInventory = null;

                ClearRecordset(ref rsOutbound);
                ClearRecordset(ref rsReturn);
                ClearRecordset(ref rsLogic);
            }
        }

        public DataSet GetFlights(string strOrigin,
                                  string strDestination,
                                  string strAirline,
                                  string strFlightNumber,
                                  string strFlightId,
                                  DateTime dteFlightFrom,
                                  DateTime dteFlightTo,
                                  string strFlightStatus,
                                  string strControllingAgency,
                                  string strCheckInStatus,
                                  string strLanguage,
                                  short iMon, short iTue, short iWed, short iThu, short iFri, short iSat, short iSun,
                                  string strScheduleId)
        {

            tikAeroProcess.Inventory objInventory = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objInventory = new tikAeroProcess.Inventory(); }

                rs = objInventory.GetFlights(ref strOrigin,
                                            ref strDestination,
                                            ref strAirline,
                                            ref strFlightNumber,
                                            ref strFlightId,
                                            ref dteFlightFrom,
                                            ref dteFlightTo,
                                            ref strFlightStatus,
                                            ref strControllingAgency,
                                            ref strLanguage,
                                            ref iMon, ref iTue, ref iWed, ref iThu, ref iFri, ref iSat, ref iSun,
                                            ref strScheduleId);

                //Convert Recordset to Dataset
                ds = RecordsetToDataset(rs, "Flights");
            }
            catch
            { }
            finally
            {
                if (objInventory != null)
                { Marshal.FinalReleaseComObject(objInventory); }
                objInventory = null;

                ClearRecordset(ref rs);
            }
            return ds;
        }

        public DataSet GetFlightSummary(string strOrigin,
                                        string strDestination,
                                        string strFlightId,
                                        string strAirline,
                                        string strFlightNumber,
                                        DateTime dteFlight,
                                        string strScheduleId,
                                        bool bEdit)
        {
            tikAeroProcess.Inventory objInventory = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objInventory = new tikAeroProcess.Inventory(); }

                rs = objInventory.GetFlightSummary(ref strOrigin,
                                                   ref strDestination,
                                                   ref strFlightId,
                                                   ref strAirline,
                                                   ref strFlightNumber,
                                                   ref dteFlight,
                                                   ref strScheduleId,
                                                   ref bEdit);

                //convert Recordset to dataset
                ds = RecordsetToDataset(rs, "FlightSummary");
            }
            catch
            { }
            finally
            {
                if (objInventory != null)
                { Marshal.FinalReleaseComObject(objInventory); }
                objInventory = null;

                ClearRecordset(ref rs);
            }
            return ds;
        }
        public DataSet GetPassengerTypes(string language)
        {
            tikAeroProcess.Inventory objInventory = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objInventory = new tikAeroProcess.Inventory(); }

                if (language.Length == 0)
                { language = "EN"; }
                rs = objInventory.GetPassengerTypes(ref language);
                ds = RecordsetToDataset(rs, "PassengerTypes");
            }
            catch
            { }
            finally
            {
                if (objInventory != null)
                { Marshal.FinalReleaseComObject(objInventory); }
                objInventory = null;

                ClearRecordset(ref rs);
            }

            return ds;
        }

        public bool CheckInfantOverLimit(DataTable dtSegment, int numberOfInfant)
        {
            tikAeroProcess.Inventory objInventory = null;
            ADODB.Recordset rsInfantLimit = null;
            string flightId;
            string originRcd;
            string destinationRcd;
            string boardingClass;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objInventory = new tikAeroProcess.Inventory(); }

                if (objInventory != null)
                {
                    if (dtSegment != null)
                    {
                        foreach (DataRow dr in dtSegment.Rows)
                        {
                            //Get infant limitation information.
                            flightId = dr.DBToString("flight_id");
                            originRcd = dr.DBToString("origin_rcd");
                            destinationRcd = dr.DBToString("destination_rcd");
                            boardingClass = dr.DBToString("boarding_class_rcd");

                            rsInfantLimit = objInventory.GetFlightLegInfants(ref flightId,
                                                                            ref originRcd,
                                                                            ref destinationRcd,
                                                                            ref boardingClass);

                            if (rsInfantLimit != null && rsInfantLimit.RecordCount > 0)
                            {
                                if (rsInfantLimit.Fields["infant_capacity"].Value != System.DBNull.Value)
                                {
                                    if (Convert.ToInt32(rsInfantLimit.Fields["infant_capacity"].Value) > 0 && numberOfInfant > 0)
                                    {
                                        if (Convert.ToInt32(rsInfantLimit.Fields["available_infant"].Value) < numberOfInfant)
                                        {
                                            ClearRecordset(ref rsInfantLimit);
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (objInventory != null)
                { Marshal.FinalReleaseComObject(objInventory); }
                objInventory = null;

                ClearRecordset(ref rsInfantLimit);
            }

            return false;
        }

        public Int32 GetInfantCapacity(string flightId, string originRcd, string destinationRcd, string boardingClass)
        {
            tikAeroProcess.Inventory objInventory = null;
            ADODB.Recordset rsInfantLimit = null;
            Int32 infantCapacity = 0;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objInventory = new tikAeroProcess.Inventory(); }

                if (objInventory != null)
                {
                    if (!flightId.Equals(string.Empty))
                    {
                        rsInfantLimit = objInventory.GetFlightLegInfants(ref flightId,
                                                                        ref originRcd,
                                                                        ref destinationRcd,
                                                                        ref boardingClass);

                        if (rsInfantLimit != null && rsInfantLimit.RecordCount > 0)
                        {
                            if (rsInfantLimit.Fields["infant_capacity"].Value != System.DBNull.Value)
                            {

                                infantCapacity = Convert.ToInt32(rsInfantLimit.Fields["available_infant"].Value);
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (objInventory != null)
                { Marshal.FinalReleaseComObject(objInventory); }
                objInventory = null;

                ClearRecordset(ref rsInfantLimit);
            }

            return infantCapacity;
        }

        #region "Helper"
        private void CreateAvailabilityXml(System.Xml.XmlWriter xtw, System.Collections.Specialized.StringDictionary stdFieldList, Array arrField)
        {
            if (stdFieldList != null && stdFieldList.Count > 0 && arrField != null && stdFieldList.Count > 0)
            {
                //Construct the availability xml and get the lowest fare.
                int iNumberOfField = arrField.GetUpperBound(0);
                int iNumberOfRows = arrField.GetUpperBound(1);
                string strFieldName = string.Empty;

                try
                {
                    for (int i = 0; i <= iNumberOfRows; i++)
                    {
                        xtw.WriteStartElement("AvailabilityFlight");
                        {
                            for (int j = 0; j <= iNumberOfField; j++)
                            {
                                strFieldName = stdFieldList[j.ToString()];
                                if (string.IsNullOrEmpty(strFieldName) == false)
                                {
                                    xtw.WriteStartElement(strFieldName);
                                    if ((arrField.GetValue(j, i) is DBNull) == false)
                                    {
                                        xtw.WriteValue(arrField.GetValue(j, i));
                                    }
                                    xtw.WriteEndElement();
                                }

                            }
                        }
                        xtw.WriteEndElement();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private void GetAvailabilityField(System.Xml.XmlWriter xtw, ADODB.Recordset rs)
        {
            xtw.WriteStartElement("airline_rcd");
            if (object.ReferenceEquals(rs.Fields["airline_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["airline_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("flight_number");
            if (object.ReferenceEquals(rs.Fields["flight_number"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["flight_number"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("booking_class_rcd");
            if (object.ReferenceEquals(rs.Fields["booking_class_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["booking_class_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("boarding_class_rcd");
            if (object.ReferenceEquals(rs.Fields["boarding_class_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["boarding_class_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("flight_id");
            if (object.ReferenceEquals(rs.Fields["flight_id"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["flight_id"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("origin_rcd");
            if (object.ReferenceEquals(rs.Fields["origin_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["origin_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("destination_rcd");
            if (object.ReferenceEquals(rs.Fields["destination_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["destination_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("origin_name");
            if (object.ReferenceEquals(rs.Fields["origin_name"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["origin_name"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("destination_name");
            if (object.ReferenceEquals(rs.Fields["destination_name"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["destination_name"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("departure_date");
            if (object.ReferenceEquals(rs.Fields["departure_date"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["departure_date"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("planned_departure_time");
            if (object.ReferenceEquals(rs.Fields["planned_departure_time"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["planned_departure_time"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("planned_arrival_time");
            if (object.ReferenceEquals(rs.Fields["planned_arrival_time"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["planned_arrival_time"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("fare_id");
            if (object.ReferenceEquals(rs.Fields["fare_id"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["fare_id"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("fare_code");
            if (object.ReferenceEquals(rs.Fields["fare_code"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["fare_code"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_points");
            if (object.ReferenceEquals(rs.Fields["transit_points"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_points"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_points_name");
            if (object.ReferenceEquals(rs.Fields["transit_points_name"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_points_name"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_flight_id");
            if (object.ReferenceEquals(rs.Fields["transit_flight_id"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_flight_id"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_booking_class_rcd");
            if (object.ReferenceEquals(rs.Fields["transit_booking_class_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_booking_class_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_boarding_class_rcd");
            if (object.ReferenceEquals(rs.Fields["transit_boarding_class_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_boarding_class_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_airport_rcd");
            if (object.ReferenceEquals(rs.Fields["transit_airport_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_airport_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_departure_date");
            if (object.ReferenceEquals(rs.Fields["transit_departure_date"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_departure_date"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_planned_departure_time");
            if (object.ReferenceEquals(rs.Fields["transit_planned_departure_time"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_planned_departure_time"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_planned_arrival_time");
            if (object.ReferenceEquals(rs.Fields["transit_planned_arrival_time"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_planned_arrival_time"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_fare_id");
            if (object.ReferenceEquals(rs.Fields["transit_fare_id"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_fare_id"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_name");
            if (object.ReferenceEquals(rs.Fields["transit_name"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_name"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("nesting_string");
            if (object.ReferenceEquals(rs.Fields["nesting_string"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["nesting_string"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("full_flight_flag");
            if (object.ReferenceEquals(rs.Fields["full_flight_flag"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["full_flight_flag"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("class_open_flag");
            if (object.ReferenceEquals(rs.Fields["class_open_flag"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["class_open_flag"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("close_web_sales");
            if (object.ReferenceEquals(rs.Fields["close_web_sales"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["close_web_sales"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("waitlist_open_flag");
            if (object.ReferenceEquals(rs.Fields["waitlist_open_flag"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["waitlist_open_flag"].Value);
            }
            xtw.WriteEndElement();


            xtw.WriteStartElement("adult_fare");
            if (object.ReferenceEquals(rs.Fields["adult_fare"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["adult_fare"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("child_fare");
            if (object.ReferenceEquals(rs.Fields["child_fare"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["child_fare"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("infant_fare");
            if (object.ReferenceEquals(rs.Fields["infant_fare"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["infant_fare"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("total_adult_fare");
            if (object.ReferenceEquals(rs.Fields["total_adult_fare"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["total_adult_fare"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("total_child_fare");
            if (object.ReferenceEquals(rs.Fields["total_child_fare"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["total_child_fare"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("total_infant_fare");
            if (object.ReferenceEquals(rs.Fields["total_infant_fare"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["total_infant_fare"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("fare_column");
            if (object.ReferenceEquals(rs.Fields["fare_column"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["fare_column"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("flight_comment");
            if (object.ReferenceEquals(rs.Fields["flight_comment"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["flight_comment"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_flight_comment");
            if (object.ReferenceEquals(rs.Fields["transit_flight_comment"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_flight_comment"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("filter_logic_flag");
            if (object.ReferenceEquals(rs.Fields["filter_logic_flag"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["filter_logic_flag"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("restriction_text");
            if (object.ReferenceEquals(rs.Fields["restriction_text"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["restriction_text"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("endorsement_text");
            if (object.ReferenceEquals(rs.Fields["endorsement_text"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["endorsement_text"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("fare_type_rcd");
            if (object.ReferenceEquals(rs.Fields["fare_type_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["fare_type_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("redemption_points");
            if (object.ReferenceEquals(rs.Fields["redemption_points"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["redemption_points"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_redemption_points");
            if (object.ReferenceEquals(rs.Fields["transit_redemption_points"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_redemption_points"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("flight_duration");
            if (object.ReferenceEquals(rs.Fields["flight_duration"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["flight_duration"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("promotion_code");
            if (object.ReferenceEquals(rs.Fields["promotion_code"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["promotion_code"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("nested_book_available");
            if (object.ReferenceEquals(rs.Fields["nested_book_available"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["nested_book_available"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("flight_status_rcd");
            if (object.ReferenceEquals(rs.Fields["flight_status_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["flight_status_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_airline_rcd");
            if (object.ReferenceEquals(rs.Fields["transit_airline_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_airline_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_flight_number");
            if (object.ReferenceEquals(rs.Fields["transit_flight_number"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_flight_number"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_flight_status_rcd");
            if (object.ReferenceEquals(rs.Fields["transit_flight_status_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_flight_status_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_flight_duration");
            if (object.ReferenceEquals(rs.Fields["transit_flight_duration"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_flight_duration"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_aircraft_type_rcd");
            if (object.ReferenceEquals(rs.Fields["transit_aircraft_type_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_aircraft_type_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_nested_book_available");
            if (object.ReferenceEquals(rs.Fields["transit_nested_book_available"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_nested_book_available"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_waitlist_open_flag");
            if (object.ReferenceEquals(rs.Fields["transit_waitlist_open_flag"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_waitlist_open_flag"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_adult_fare");
            if (object.ReferenceEquals(rs.Fields["transit_adult_fare"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_adult_fare"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_class_open_flag");
            if (object.ReferenceEquals(rs.Fields["transit_class_open_flag"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_class_open_flag"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("flight_information_1");
            if (object.ReferenceEquals(rs.Fields["flight_information_1"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["flight_information_1"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("corporate_fare_flag");
            if (object.ReferenceEquals(rs.Fields["corporate_fare_flag"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["corporate_fare_flag"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("refundable_flag");
            if (object.ReferenceEquals(rs.Fields["refundable_flag"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["refundable_flag"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_flight_duration");
            if (object.ReferenceEquals(rs.Fields["transit_flight_duration"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_flight_duration"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("aircraft_type_rcd");
            if (object.ReferenceEquals(rs.Fields["aircraft_type_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["aircraft_type_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("class_capacity");
            if (object.ReferenceEquals(rs.Fields["class_capacity"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["class_capacity"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("arrival_date");
            if (object.ReferenceEquals(rs.Fields["arrival_date"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["arrival_date"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_arrival_date");
            if (object.ReferenceEquals(rs.Fields["transit_arrival_date"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_arrival_date"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("number_of_stops");
            if (object.ReferenceEquals(rs.Fields["number_of_stops"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["number_of_stops"].Value);
            }
            xtw.WriteEndElement();
        }
        private void GetLowFareFinderField(System.Xml.XmlWriter xtw, ADODB.Recordset rs)
        {
            xtw.WriteStartElement("airline_rcd");
            if (object.ReferenceEquals(rs.Fields["airline_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["airline_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("flight_number");
            if (object.ReferenceEquals(rs.Fields["flight_number"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["flight_number"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("booking_class_rcd");
            if (object.ReferenceEquals(rs.Fields["booking_class_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["booking_class_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("boarding_class_rcd");
            if (object.ReferenceEquals(rs.Fields["boarding_class_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["boarding_class_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("flight_id");
            if (object.ReferenceEquals(rs.Fields["flight_id"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["flight_id"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("origin_rcd");
            if (object.ReferenceEquals(rs.Fields["origin_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["origin_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("destination_rcd");
            if (object.ReferenceEquals(rs.Fields["destination_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["destination_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("origin_name");
            if (object.ReferenceEquals(rs.Fields["origin_name"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["origin_name"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("destination_name");
            if (object.ReferenceEquals(rs.Fields["destination_name"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["destination_name"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("departure_date");
            if (object.ReferenceEquals(rs.Fields["departure_date"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["departure_date"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("full_flight_flag");
            if (object.ReferenceEquals(rs.Fields["full_flight_flag"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["full_flight_flag"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("class_open_flag");
            if (object.ReferenceEquals(rs.Fields["class_open_flag"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["class_open_flag"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("close_web_sales");
            if (object.ReferenceEquals(rs.Fields["close_web_sales"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["close_web_sales"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("adult_fare");
            if (object.ReferenceEquals(rs.Fields["adult_fare"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["adult_fare"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("child_fare");
            if (object.ReferenceEquals(rs.Fields["child_fare"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["child_fare"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("infant_fare");
            if (object.ReferenceEquals(rs.Fields["infant_fare"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["infant_fare"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("total_adult_fare");
            if (object.ReferenceEquals(rs.Fields["total_adult_fare"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["total_adult_fare"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("total_child_fare");
            if (object.ReferenceEquals(rs.Fields["total_child_fare"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["total_child_fare"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("total_infant_fare");
            if (object.ReferenceEquals(rs.Fields["total_infant_fare"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["total_infant_fare"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_flight_id");
            if (object.ReferenceEquals(rs.Fields["transit_flight_id"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_flight_id"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("planned_departure_time");
            if (object.ReferenceEquals(rs.Fields["planned_departure_time"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["planned_departure_time"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("planned_arrival_time");
            if (object.ReferenceEquals(rs.Fields["planned_arrival_time"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["planned_arrival_time"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_departure_date");
            if (object.ReferenceEquals(rs.Fields["transit_departure_date"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_departure_date"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_airport_rcd");
            if (object.ReferenceEquals(rs.Fields["transit_airport_rcd"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_airport_rcd"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("transit_fare_id");
            if (object.ReferenceEquals(rs.Fields["transit_fare_id"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["transit_fare_id"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("fare_id");
            if (object.ReferenceEquals(rs.Fields["fare_id"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["fare_id"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("full_flight_flag");
            if (object.ReferenceEquals(rs.Fields["full_flight_flag"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["full_flight_flag"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("class_open_flag");
            if (object.ReferenceEquals(rs.Fields["class_open_flag"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["class_open_flag"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("close_web_sales");
            if (object.ReferenceEquals(rs.Fields["close_web_sales"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["close_web_sales"].Value);
            }
            xtw.WriteEndElement();

            xtw.WriteStartElement("waitlist_open_flag");
            if (object.ReferenceEquals(rs.Fields["waitlist_open_flag"].Value, System.DBNull.Value))
            {
                xtw.WriteValue(string.Empty);
            }
            else
            {
                xtw.WriteValue(rs.Fields["waitlist_open_flag"].Value);
            }
            xtw.WriteEndElement();
        }
        private System.Collections.Specialized.StringDictionary GetAvailabilityColumn(ADODB.Recordset rs)
        {
            System.Collections.Specialized.StringDictionary std = null;
            if (rs != null && rs.Fields.Count > 0)
            {
                std = new System.Collections.Specialized.StringDictionary();

                for (int i = 0; i < rs.Fields.Count; i++)
                {
                    switch (rs.Fields[i].Name)
                    {
                        case "fee_rcd":
                            std.Add(i.ToString(), "fee_rcd");
                            break;
                        case "airline_rcd":
                            std.Add(i.ToString(), "airline_rcd");
                            break;
                        case "flight_number":
                            std.Add(i.ToString(), "flight_number");
                            break;
                        case "booking_class_rcd":
                            std.Add(i.ToString(), "booking_class_rcd");
                            break;
                        case "boarding_class_rcd":
                            std.Add(i.ToString(), "boarding_class_rcd");
                            break;
                        case "flight_id":
                            std.Add(i.ToString(), "flight_id");
                            break;
                        case "origin_rcd":
                            std.Add(i.ToString(), "origin_rcd");
                            break;
                        case "destination_rcd":
                            std.Add(i.ToString(), "destination_rcd");
                            break;
                        case "origin_name":
                            std.Add(i.ToString(), "origin_name");
                            break;
                        case "destination_name":
                            std.Add(i.ToString(), "destination_name");
                            break;
                        case "departure_date":
                            std.Add(i.ToString(), "departure_date");
                            break;
                        case "planned_departure_time":
                            std.Add(i.ToString(), "planned_departure_time");
                            break;
                        case "planned_arrival_time":
                            std.Add(i.ToString(), "planned_arrival_time");
                            break;
                        case "fare_id":
                            std.Add(i.ToString(), "fare_id");
                            break;
                        case "fare_code":
                            std.Add(i.ToString(), "fare_code");
                            break;
                        case "transit_points":
                            std.Add(i.ToString(), "transit_points");
                            break;
                        case "transit_points_name":
                            std.Add(i.ToString(), "transit_points_name");
                            break;
                        case "transit_flight_id":
                            std.Add(i.ToString(), "transit_flight_id");
                            break;
                        case "transit_booking_class_rcd":
                            std.Add(i.ToString(), "transit_booking_class_rcd");
                            break;
                        case "transit_boarding_class_rcd":
                            std.Add(i.ToString(), "transit_boarding_class_rcd");
                            break;
                        case "transit_airport_rcd":
                            std.Add(i.ToString(), "transit_airport_rcd");
                            break;
                        case "transit_departure_date":
                            std.Add(i.ToString(), "transit_departure_date");
                            break;
                        case "transit_planned_departure_time":
                            std.Add(i.ToString(), "transit_planned_departure_time");
                            break;
                        case "transit_planned_arrival_time":
                            std.Add(i.ToString(), "transit_planned_arrival_time");
                            break;
                        case "transit_fare_id":
                            std.Add(i.ToString(), "transit_fare_id");
                            break;
                        case "transit_name":
                            std.Add(i.ToString(), "transit_name");
                            break;
                        case "nesting_string":
                            std.Add(i.ToString(), "nesting_string");
                            break;
                        case "full_flight_flag":
                            std.Add(i.ToString(), "full_flight_flag");
                            break;
                        case "class_open_flag":
                            std.Add(i.ToString(), "class_open_flag");
                            break;
                        case "close_web_sales":
                            std.Add(i.ToString(), "close_web_sales");
                            break;
                        case "waitlist_open_flag":
                            std.Add(i.ToString(), "waitlist_open_flag");
                            break;
                        case "adult_fare":
                            std.Add(i.ToString(), "adult_fare");
                            break;
                        case "child_fare":
                            std.Add(i.ToString(), "child_fare");
                            break;
                        case "infant_fare":
                            std.Add(i.ToString(), "infant_fare");
                            break;
                        case "other_fare":
                            std.Add(i.ToString(), "other_fare");
                            break;
                        case "total_adult_fare":
                            std.Add(i.ToString(), "total_adult_fare");
                            break;
                        case "total_child_fare":
                            std.Add(i.ToString(), "total_child_fare");
                            break;
                        case "total_infant_fare":
                            std.Add(i.ToString(), "total_infant_fare");
                            break;
                        case "total_other_fare":
                            std.Add(i.ToString(), "total_other_fare");
                            break;
                        case "fare_column":
                            std.Add(i.ToString(), "fare_column");
                            break;
                        case "flight_comment":
                            std.Add(i.ToString(), "flight_comment");
                            break;
                        case "transit_flight_comment":
                            std.Add(i.ToString(), "transit_flight_comment");
                            break;
                        case "filter_logic_flag":
                            std.Add(i.ToString(), "filter_logic_flag");
                            break;
                        case "restriction_text":
                            std.Add(i.ToString(), "restriction_text");
                            break;
                        case "endorsement_text":
                            std.Add(i.ToString(), "endorsement_text");
                            break;
                        case "fare_type_rcd":
                            std.Add(i.ToString(), "fare_type_rcd");
                            break;
                        case "redemption_points":
                            std.Add(i.ToString(), "redemption_points");
                            break;
                        case "transit_redemption_points":
                            std.Add(i.ToString(), "transit_redemption_points");
                            break;
                        case "flight_duration":
                            std.Add(i.ToString(), "flight_duration");
                            break;
                        case "promotion_code":
                            std.Add(i.ToString(), "promotion_code");
                            break;
                        case "nested_book_available":
                            std.Add(i.ToString(), "nested_book_available");
                            break;
                        case "flight_status_rcd":
                            std.Add(i.ToString(), "flight_status_rcd");
                            break;
                        case "transit_airline_rcd":
                            std.Add(i.ToString(), "transit_airline_rcd");
                            break;
                        case "transit_flight_number":
                            std.Add(i.ToString(), "transit_flight_number");
                            break;
                        case "transit_flight_status_rcd":
                            std.Add(i.ToString(), "transit_flight_status_rcd");
                            break;
                        case "transit_flight_duration":
                            std.Add(i.ToString(), "transit_flight_duration");
                            break;
                        case "transit_aircraft_type_rcd":
                            std.Add(i.ToString(), "transit_aircraft_type_rcd");
                            break;
                        case "transit_nested_book_available":
                            std.Add(i.ToString(), "transit_nested_book_available");
                            break;
                        case "transit_waitlist_open_flag":
                            std.Add(i.ToString(), "transit_waitlist_open_flag");
                            break;
                        case "transit_adult_fare":
                            std.Add(i.ToString(), "transit_adult_fare");
                            break;
                        case "transit_class_open_flag":
                            std.Add(i.ToString(), "transit_class_open_flag");
                            break;
                        case "flight_information_1":
                            std.Add(i.ToString(), "flight_information_1");
                            break;
                        case "corporate_fare_flag":
                            std.Add(i.ToString(), "corporate_fare_flag");
                            break;
                        case "refundable_flag":
                            std.Add(i.ToString(), "refundable_flag");
                            break;
                        case "aircraft_type_rcd":
                            std.Add(i.ToString(), "aircraft_type_rcd");
                            break;
                        case "class_capacity":
                            std.Add(i.ToString(), "class_capacity");
                            break;
                        case "arrival_date":
                            std.Add(i.ToString(), "arrival_date");
                            break;
                        case "transit_arrival_date":
                            std.Add(i.ToString(), "transit_arrival_date");
                            break;
                        case "number_of_stops":
                            std.Add(i.ToString(), "number_of_stops");
                            break;

                    }
                }

                return std;
            }
            return null;
        }
        #endregion
    }
}