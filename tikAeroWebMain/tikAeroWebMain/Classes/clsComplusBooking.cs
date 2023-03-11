using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Data;
using Avantik.Web.Service.COMHelper;
using tikSystem.Web.Library;

namespace tikAeroWebMain
{
    public class ComplusBooking : RunComplus
    {
        public ComplusBooking() : base() { }
        
        public DataSet GetClient(string clientId,
                                 string clientNumber,
                                 string passengerId,
                                 bool bShowRemark)
        {

            bool bResult = false;

            DataSet ds = new DataSet();

            //Convert Datatable to Recordset
            ADODB.Recordset rsClient = new ADODB.Recordset();
            ADODB.Recordset rsRemarks = new ADODB.Recordset();

            if (rsClient != null && rsRemarks != null)
            {
                tikAeroProcess.Booking objBooking = null;
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Booking(); }

                if (objBooking != null)
                {
                    //Call Complus function
                    bResult = objBooking.GetClient(ref clientId, ref clientNumber, ref passengerId, ref rsClient, ref rsRemarks, ref bShowRemark);
                    Marshal.FinalReleaseComObject(objBooking);

                    DataSet TempDt;
                    //Convert Recordset to Datatable and Release com object
                    TempDt = RecordsetToDataset(rsClient, "Client");
                    if (TempDt.Tables.Count > 0)
                    { ds.Tables.Add(TempDt.Tables[0].Copy()); }
                    TempDt.Dispose();
                    TempDt = null;

                    if (rsClient.State == 1)
                    {
                        rsClient.Close();
                    }

                    Marshal.FinalReleaseComObject(rsClient);
                    rsClient = null;

                    TempDt = RecordsetToDataset(rsRemarks, "Remarks");
                    if (TempDt.Tables.Count > 0)
                    { ds.Tables.Add(TempDt.Tables[0].Copy()); }
                    TempDt.Dispose();
                    TempDt = null;
                    if (rsRemarks.State == 1)
                    {
                        rsRemarks.Close();
                    }
                    Marshal.FinalReleaseComObject(rsRemarks);
                    rsRemarks = null;
                }
                objBooking = null;
                ds.DataSetName = "Booking";
            }

            return ds;
        }


        public bool GetEmpty(string agencyCode,
                              string currency,
                              ref DataTable header,
                              ref DataTable segments,
                              ref DataTable passengers,
                              ref DataTable remarks,
                              ref DataTable payments,
                              ref DataTable mappings,
                              ref DataTable services,
                              ref DataTable taxes,
                              ref DataTable fees,
                              DataTable flights,
                              ref DataTable quotes,
                              string bookingID,
                              short adults,
                              short children,
                              short infants,
                              short others,
                              string strOthers,
                              string userId,
                              string strIpAddress,
                              string strLanguageCode,
                              bool bNoVat)
        {
            tikAeroProcess.Booking objBooking = null;

            ADODB.Recordset rsHeader = null;
            ADODB.Recordset rsSegments = null;
            ADODB.Recordset rsPassengers = null;
            ADODB.Recordset rsRemarks = null;
            ADODB.Recordset rsPayments = null;
            ADODB.Recordset rsMappings = null;
            ADODB.Recordset rsServices = null;
            ADODB.Recordset rsTaxes = null;
            ADODB.Recordset rsFees = null;

            ADODB.Recordset rsFlights = null;
            ADODB.Recordset rsQuotes = null;


            DataSet ds = null;
            bool result = false;
            string res = "";

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Booking(); }


                    rsHeader = new ADODB.Recordset();
                    rsSegments = new ADODB.Recordset();
                    rsPassengers = new ADODB.Recordset();
                    rsRemarks = new ADODB.Recordset();
                    rsPayments = new ADODB.Recordset();
                    rsMappings = new ADODB.Recordset();
                    rsServices = new ADODB.Recordset();
                    rsTaxes = new ADODB.Recordset();
                    rsFees = new ADODB.Recordset();
                    rsQuotes = new ADODB.Recordset();
                    rsFlights = FabricateFlightRecordset();


                    DatasetToRecordset(flights, ref rsFlights);

                    res = objBooking.GetEmpty(ref rsHeader,
                                               ref rsSegments,
                                               ref rsPassengers,
                                               ref rsRemarks,
                                               ref rsPayments,
                                               ref rsMappings,
                                               ref rsServices,
                                               ref rsTaxes,
                                               ref rsFees,
                                               ref rsFlights,
                                               ref rsQuotes,
                                               ref bookingID,
                                               ref agencyCode,
                                               ref currency,
                                               ref adults,
                                               ref children,
                                               ref infants,
                                               ref others,
                                               ref strOthers,
                                               ref userId,
                                               ref strIpAddress,
                                               ref strLanguageCode,
                                               ref bNoVat);

                ds = RecordsetToDataset(rsHeader, "BookingHeader");
                if (ds.Tables.Count > 0)
                { header = ds.Tables[0]; }

                ds = RecordsetToDataset(rsSegments, "FlightSegment");
                if (ds.Tables.Count > 0)
                { segments = ds.Tables[0]; }

                ds = RecordsetToDataset(rsPassengers, "Passenger");
                if (ds.Tables.Count > 0)
                { passengers = ds.Tables[0]; }

                ds = RecordsetToDataset(rsRemarks, "Remark");
                if (ds.Tables.Count > 0)
                { remarks = ds.Tables[0]; }

                ds = RecordsetToDataset(rsPayments, "Payment");
                if (ds.Tables.Count > 0)
                { payments = ds.Tables[0]; }

                ds = RecordsetToDataset(rsMappings, "Mapping");
                if (ds.Tables.Count > 0)
                { mappings = ds.Tables[0]; }

                ds = RecordsetToDataset(rsServices, "Service");
                if (ds.Tables.Count > 0)
                { services = ds.Tables[0]; }

                ds = RecordsetToDataset(rsTaxes, "Tax");
                if (ds.Tables.Count > 0)
                { taxes = ds.Tables[0]; }

                ds = RecordsetToDataset(rsFees, "Fee");
                if (ds.Tables.Count > 0)
                { fees = ds.Tables[0]; }

                ds = RecordsetToDataset(rsQuotes, "Quote");
                if (ds.Tables.Count > 0)
                { quotes = ds.Tables[0]; }

                if (header != null && segments != null && segments.Rows.Count > 0 && passengers != null && passengers.Rows.Count > 0 && mappings != null && mappings.Rows.Count > 0)
                {
                    string strRecordLocator = string.Empty;
                    System.Int32 iBookingNumber = 0;

                    bool bResult = objBooking.GetRecordLocator(ref strRecordLocator, ref iBookingNumber);

                    if (bResult)
                    {
                        header.Rows[0]["record_locator"] = strRecordLocator;
                        header.Rows[0]["booking_number"] = iBookingNumber;
                    }

                    //get empty success
                    result = true;
                }

                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            }
            catch (Exception e)
            {
                Helper objHelper = new Helper();
                objHelper.SaveLog("GetEmpty", DateTime.Now, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);
            }
            finally
            {
                //Release Com Object

                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }
                ClearRecordset(ref rsFlights);
                ClearRecordset(ref rsHeader);
                ClearRecordset(ref rsSegments);
                ClearRecordset(ref rsPassengers);
                ClearRecordset(ref rsRemarks);
                ClearRecordset(ref rsPayments);
                ClearRecordset(ref rsMappings);
                ClearRecordset(ref rsServices);
                ClearRecordset(ref rsTaxes);
                ClearRecordset(ref rsFees);
                ClearRecordset(ref rsQuotes);
            }

            return result;
        }


        public bool GetEmptySubload(string agencyCode,
                              string currency,
                              ref DataTable header,
                              ref DataTable segments,
                              ref DataTable passengers,
                              ref DataTable remarks,
                              ref DataTable payments,
                              ref DataTable mappings,
                              ref DataTable services,
                              ref DataTable taxes,
                              ref DataTable fees,
                              DataTable flights,
                              ref DataTable quotes,
                              string bookingID,
                              short adults,
                              short children,
                              short infants,
                              short others,
                              string strOthers,
                              string userId,
                              string strIpAddress,
                              string strLanguageCode,
                              bool bNoVat, DataTable p)
        {
            tikAeroProcess.Booking objBooking = null;

            ADODB.Recordset rsHeader = null;
            ADODB.Recordset rsSegments = null;
            ADODB.Recordset rsPassengers = null;
            ADODB.Recordset rsRemarks = null;
            ADODB.Recordset rsPayments = null;
            ADODB.Recordset rsMappings = null;
            ADODB.Recordset rsServices = null;
            ADODB.Recordset rsTaxes = null;
            ADODB.Recordset rsFees = null;

            ADODB.Recordset rsFlights = null;
            ADODB.Recordset rsQuotes = null;


            DataSet ds = null;
            bool result = false;
            string res = "";

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Booking(); }


                    rsFlights = FabricateFlightRecordset();


                    DatasetToRecordset(flights, ref rsFlights);

                    rsPassengers = FabricatePassengerRecordset();


                    DatasetToRecordset(p, ref rsPassengers);

                  
                    DateTime dt = new DateTime();
                    string boardpoint = string.Empty;
                    string Offpoint = string.Empty;
                    string flightId = string.Empty;
                    string fareId = string.Empty;
                    string airline = string.Empty;
                    string flight = string.Empty;
                    string boardingClass = string.Empty;
                    string bookingClass = string.Empty;
                    string language = string.Empty;
                    string currencyCode = string.Empty;
                    bool eTicket = false;
                    bool refundable = false;
                    bool groupBooking = false;
                    
                    bool waitlist = true;
                    bool quoteOnly = true;

                    bool nonRevenue = true;
                    bool subjectToAvaliability = false;
                    short idReduction = 0;
                    bool excludeQuote = true;

                    string marketingAirline = string.Empty;
                    string marketingFlight = string.Empty;

                    
                    string segmentId = string.Empty;

                    bool overbook = false;


                    result = objBooking.AddFlight(ref rsPassengers,
                            ref rsSegments,
                            ref rsMappings,
                            ref rsTaxes,
                            ref rsServices,
                            agencyCode,
                            ref rsQuotes,
                            ref rsRemarks,
                            ref rsFlights,
                            ref dt,
                            ref bookingID,
                            ref boardpoint,
                            ref Offpoint,
                            ref flightId,
                            ref fareId,
                            ref airline,
                            ref flight,
                            ref boardingClass,
                            ref bookingClass,
                            ref language,
                            ref currency,
                            ref eTicket,
                            ref refundable,
                            ref groupBooking,
                            ref waitlist,
                            ref quoteOnly,
                            ref nonRevenue,
                            ref subjectToAvaliability,
                            ref marketingAirline,
                            ref marketingFlight,
                            ref segmentId,
                            ref idReduction,
                            ref userId,
                            ref excludeQuote,
                            ref overbook);


                    ds = RecordsetToDataset(rsHeader, "BookingHeader");
                    if (ds.Tables.Count > 0)
                    { header = ds.Tables[0]; }

                    ds = RecordsetToDataset(rsSegments, "FlightSegment");
                    if (ds.Tables.Count > 0)
                    { segments = ds.Tables[0]; }

                    ds = RecordsetToDataset(rsPassengers, "Passenger");
                    if (ds.Tables.Count > 0)
                    { passengers = ds.Tables[0]; }

                    ds = RecordsetToDataset(rsRemarks, "Remark");
                    if (ds.Tables.Count > 0)
                    { remarks = ds.Tables[0]; }

                    ds = RecordsetToDataset(rsPayments, "Payment");
                    if (ds.Tables.Count > 0)
                    { payments = ds.Tables[0]; }

                    ds = RecordsetToDataset(rsMappings, "Mapping");
                    if (ds.Tables.Count > 0)
                    { mappings = ds.Tables[0]; }

                    ds = RecordsetToDataset(rsServices, "Service");
                    if (ds.Tables.Count > 0)
                    { services = ds.Tables[0]; }

                    ds = RecordsetToDataset(rsTaxes, "Tax");
                    if (ds.Tables.Count > 0)
                    { taxes = ds.Tables[0]; }

                    ds = RecordsetToDataset(rsFees, "Fee");
                    if (ds.Tables.Count > 0)
                    { fees = ds.Tables[0]; }

                    ds = RecordsetToDataset(rsQuotes, "Quote");
                    if (ds.Tables.Count > 0)
                    { quotes = ds.Tables[0]; }

                    ds = RecordsetToDataset(rsFlights, "flights");
                    flights = ds.Tables[0];

                if (header != null && segments != null && segments.Rows.Count > 0 && passengers != null && passengers.Rows.Count > 0 && mappings != null && mappings.Rows.Count > 0)
                {
                    string strRecordLocator = string.Empty;
                    System.Int32 iBookingNumber = 0;

                    bool bResult = objBooking.GetRecordLocator(ref strRecordLocator, ref iBookingNumber);

                    if (bResult)
                    {
                        header.Rows[0]["record_locator"] = strRecordLocator;
                        header.Rows[0]["booking_number"] = iBookingNumber;
                    }

                    //get empty success
                    result = true;
                }
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            }
            catch (Exception e)
            {
                Helper objHelper = new Helper();
                objHelper.SaveLog("GetEmpty", DateTime.Now, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);
            }
            finally
            {
                //Release Com Object

                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }
                ClearRecordset(ref rsFlights);
                ClearRecordset(ref rsHeader);
                ClearRecordset(ref rsSegments);
                ClearRecordset(ref rsPassengers);
                ClearRecordset(ref rsRemarks);
                ClearRecordset(ref rsPayments);
                ClearRecordset(ref rsMappings);
                ClearRecordset(ref rsServices);
                ClearRecordset(ref rsTaxes);
                ClearRecordset(ref rsFees);
                ClearRecordset(ref rsQuotes);
            }

            string IETVersion = string.Empty;
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["bIET"]) == false)
            {
                IETVersion = System.Configuration.ConfigurationManager.AppSettings["bIET"];
            }

            if (!IETVersion.Equals("IET"))
            {
                if (res == "00000")
                    result = true;
            }

            //if (res == "00000")
            //    result = true;

            return result;
        }

        //public bool GetEmpty(string agencyCode,
        //              string currency,
        //              ref DataTable header,
        //              ref DataTable segments,
        //              ref DataTable passengers,
        //              ref DataTable remarks,
        //              ref DataTable payments,
        //              ref DataTable mappings,
        //              ref DataTable services,
        //              ref DataTable taxes,
        //              ref DataTable fees,
        //              DataTable flights,
        //              ref DataTable quotes,
        //              string bookingID,
        //              short adults,
        //              short children,
        //              short infants,
        //              short others,
        //              string strOthers,
        //              string userId,
        //              string strIpAddress,
        //              string strLanguageCode,
        //              bool bNoVat)
        //{
        //    tikAeroProcess.Booking objBooking = null;

        //    ADODB.Recordset rsHeader = new ADODB.Recordset();
        //    ADODB.Recordset rsSegments = new ADODB.Recordset();
        //    ADODB.Recordset rsPassengers = new ADODB.Recordset();
        //    ADODB.Recordset rsRemarks = new ADODB.Recordset();
        //    ADODB.Recordset rsPayments = new ADODB.Recordset();
        //    ADODB.Recordset rsMappings = new ADODB.Recordset();
        //    ADODB.Recordset rsServices = new ADODB.Recordset();
        //    ADODB.Recordset rsTaxes = new ADODB.Recordset();
        //    ADODB.Recordset rsFees = new ADODB.Recordset();
        //    ADODB.Recordset rsQuotes = new ADODB.Recordset();
        //    ADODB.Recordset rsFlights = FabricateFlightRecordset();

        //    DataSet ds = null;
        //    bool result = false;
        //    try
        //    {
        //        if (_server.Length > 0)
        //        {
        //            Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
        //            objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
        //            remote = null;
        //        }
        //        else
        //        { objBooking = new tikAeroProcess.Booking(); }

        //        DatasetToRecordset(flights, ref rsFlights);

        //        result = objBooking.GetEmpty(ref rsHeader,
        //                                    ref rsSegments,
        //                                    ref rsPassengers,
        //                                    ref rsRemarks,
        //                                    ref rsPayments,
        //                                    ref rsMappings,
        //                                    ref rsServices,
        //                                    ref rsTaxes,
        //                                    ref rsFees,
        //                                    ref rsFlights,
        //                                    ref rsQuotes,
        //                                    ref bookingID,
        //                                    ref agencyCode,
        //                                    ref currency,
        //                                    ref adults,
        //                                    ref children,
        //                                    ref infants,
        //                                    ref others,
        //                                    ref strOthers,
        //                                    ref userId,
        //                                    ref strIpAddress,
        //                                    ref strLanguageCode,
        //                                    ref bNoVat);

        //        ds = RecordsetToDataset(rsHeader, "BookingHeader");
        //        if (ds.Tables.Count > 0)
        //        { header = ds.Tables[0]; }

        //        ds = RecordsetToDataset(rsSegments, "FlightSegment");
        //        if (ds.Tables.Count > 0)
        //        { segments = ds.Tables[0]; }

        //        ds = RecordsetToDataset(rsPassengers, "Passenger");
        //        if (ds.Tables.Count > 0)
        //        { passengers = ds.Tables[0]; }

        //        ds = RecordsetToDataset(rsRemarks, "Remark");
        //        if (ds.Tables.Count > 0)
        //        { remarks = ds.Tables[0]; }

        //        ds = RecordsetToDataset(rsPayments, "Payment");
        //        if (ds.Tables.Count > 0)
        //        { payments = ds.Tables[0]; }

        //        ds = RecordsetToDataset(rsMappings, "Mapping");
        //        if (ds.Tables.Count > 0)
        //        { mappings = ds.Tables[0]; }

        //        ds = RecordsetToDataset(rsServices, "Service");
        //        if (ds.Tables.Count > 0)
        //        { services = ds.Tables[0]; }

        //        ds = RecordsetToDataset(rsTaxes, "Tax");
        //        if (ds.Tables.Count > 0)
        //        { taxes = ds.Tables[0]; }

        //        ds = RecordsetToDataset(rsFees, "Fee");
        //        if (ds.Tables.Count > 0)
        //        { fees = ds.Tables[0]; }

        //        ds = RecordsetToDataset(rsQuotes, "Quote");
        //        if (ds.Tables.Count > 0)
        //        { quotes = ds.Tables[0]; }

        //        if (ds != null)
        //        {
        //            ds.Dispose();
        //            ds = null;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Helper objHelper = new Helper();
        //        objHelper.SaveLog("GetEmpty", DateTime.Now, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);
        //    }
        //    finally
        //    {
        //        //Release Com Object

        //        if (objBooking != null)
        //        {
        //            Marshal.FinalReleaseComObject(objBooking);
        //            objBooking = null;
        //        }
        //        ClearRecordset(ref rsFlights);
        //        ClearRecordset(ref rsHeader);
        //        ClearRecordset(ref rsSegments);
        //        ClearRecordset(ref rsPassengers);
        //        ClearRecordset(ref rsRemarks);
        //        ClearRecordset(ref rsPayments);
        //        ClearRecordset(ref rsMappings);
        //        ClearRecordset(ref rsServices);
        //        ClearRecordset(ref rsTaxes);
        //        ClearRecordset(ref rsFees);
        //        ClearRecordset(ref rsQuotes);
        //    }

        //    return result;
        //}

        //public bool GetEmpty(string agencyCode,
        //              string currency,
        //              ref DataTable header,
        //              ref DataTable segments,
        //              ref DataTable passengers,
        //              ref DataTable remarks,
        //              ref DataTable payments,
        //              ref DataTable mappings,
        //              ref DataTable services,
        //              ref DataTable taxes,
        //              ref DataTable fees,
        //              DataTable flights,
        //              ref DataTable quotes,
        //              string bookingID,
        //              short adults,
        //              short children,
        //              short infants,
        //              short others,
        //              string strOthers,
        //              string userId,
        //              string strIpAddress,
        //              string strLanguageCode,
        //              bool bNoVat)
        //{
        //    tikAeroProcess.Booking objBooking = null;

        //    ADODB.Recordset rsHeader = new ADODB.Recordset();
        //    ADODB.Recordset rsSegments = new ADODB.Recordset();
        //    ADODB.Recordset rsPassengers = new ADODB.Recordset();
        //    ADODB.Recordset rsRemarks = new ADODB.Recordset();
        //    ADODB.Recordset rsPayments = new ADODB.Recordset();
        //    ADODB.Recordset rsMappings = new ADODB.Recordset();
        //    ADODB.Recordset rsServices = new ADODB.Recordset();
        //    ADODB.Recordset rsTaxes = new ADODB.Recordset();
        //    ADODB.Recordset rsFees = new ADODB.Recordset();
        //    ADODB.Recordset rsQuotes = new ADODB.Recordset();
        //    ADODB.Recordset rsFlights = FabricateFlightRecordset();

        //    DataSet ds = null;
        //    bool result = false;
        //    try
        //    {
        //        if (_server.Length > 0)
        //        {
        //            Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
        //            objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
        //            remote = null;
        //        }
        //        else
        //        { objBooking = new tikAeroProcess.Booking(); }

        //        //Convert Datatable to recordset. 
        //        DatasetToRecordset(flights, ref rsFlights);

        //        result = objBooking.GetEmpty(ref rsHeader,
        //                                    ref rsSegments,
        //                                    ref rsPassengers,
        //                                    ref rsRemarks,
        //                                    ref rsPayments,
        //                                    ref rsMappings,
        //                                    ref rsServices,
        //                                    ref rsTaxes,
        //                                    ref rsFees,
        //                                    ref rsFlights,
        //                                    ref rsQuotes,
        //                                    ref bookingID,
        //                                    ref agencyCode,
        //                                    ref currency,
        //                                    ref adults,
        //                                    ref children,
        //                                    ref infants,
        //                                    ref others,
        //                                    ref strOthers,
        //                                    ref userId,
        //                                    ref strIpAddress,
        //                                    ref strLanguageCode,
        //                                    ref bNoVat);

        //        ds = RecordsetToDataset(rsHeader, "BookingHeader");
        //        if (ds.Tables.Count > 0)
        //        { header = ds.Tables[0]; }

        //        ds = RecordsetToDataset(rsSegments, "FlightSegment");
        //        if (ds.Tables.Count > 0)
        //        { segments = ds.Tables[0]; }

        //        ds = RecordsetToDataset(rsPassengers, "Passenger");
        //        if (ds.Tables.Count > 0)
        //        { passengers = ds.Tables[0]; }

        //        ds = RecordsetToDataset(rsRemarks, "Remark");
        //        if (ds.Tables.Count > 0)
        //        { remarks = ds.Tables[0]; }

        //        ds = RecordsetToDataset(rsPayments, "Payment");
        //        if (ds.Tables.Count > 0)
        //        { payments = ds.Tables[0]; }

        //        ds = RecordsetToDataset(rsMappings, "Mapping");
        //        if (ds.Tables.Count > 0)
        //        { mappings = ds.Tables[0]; }

        //        ds = RecordsetToDataset(rsServices, "Service");
        //        if (ds.Tables.Count > 0)
        //        { services = ds.Tables[0]; }

        //        ds = RecordsetToDataset(rsTaxes, "Tax");
        //        if (ds.Tables.Count > 0)
        //        { taxes = ds.Tables[0]; }

        //        ds = RecordsetToDataset(rsFees, "Fee");
        //        if (ds.Tables.Count > 0)
        //        { fees = ds.Tables[0]; }

        //        ds = RecordsetToDataset(rsQuotes, "Quote");
        //        if (ds.Tables.Count > 0)
        //        { quotes = ds.Tables[0]; }

        //        if (ds != null)
        //        {
        //            ds.Dispose();
        //            ds = null;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Helper objHelper = new Helper();
        //        objHelper.SaveLog("GetEmpty", DateTime.Now, DateTime.Now, e.Message + "\n" + e.StackTrace.ToString(), string.Empty);
        //    }
        //    finally
        //    {
        //        //Release Com Object

        //        if (objBooking != null)
        //        {
        //            Marshal.FinalReleaseComObject(objBooking);
        //            objBooking = null;
        //        }
        //        ClearRecordset(ref rsFlights);
        //        ClearRecordset(ref rsHeader);
        //        ClearRecordset(ref rsSegments);
        //        ClearRecordset(ref rsPassengers);
        //        ClearRecordset(ref rsRemarks);
        //        ClearRecordset(ref rsPayments);
        //        ClearRecordset(ref rsMappings);
        //        ClearRecordset(ref rsServices);
        //        ClearRecordset(ref rsTaxes);
        //        ClearRecordset(ref rsFees);
        //        ClearRecordset(ref rsQuotes);
        //    }

        //    return result;
        //}

       
        public DataSet SaveBooking(DataTable header,
                                DataTable segment,
                                DataTable passenger,
                                DataTable remark,
                                DataTable payment,
                                DataTable mapping,
                                DataTable service,
                                DataTable tax,
                                DataTable fee,
                                bool createTickets,
                                bool readBooking,
                                bool readOnly,
                                bool bSetLock,
                                bool bCheckSeatAssignment,
                                bool bCheckSessionTimeOut)
        {
            tikAeroProcess.Booking objBooking = null;
            tikAeroProcess.Inventory objInventory = null;

            ADODB.Recordset rsHeader = null;
            ADODB.Recordset rsSegment = null;
            ADODB.Recordset rsPassenger = null;
            ADODB.Recordset rsRemark = null;
            ADODB.Recordset rsPayment = null;
            ADODB.Recordset rsMapping = null;
            ADODB.Recordset rsService = null;
            ADODB.Recordset rsTax = null;
            ADODB.Recordset rsFees = null;

            ADODB.Recordset rsFlight = null;
            ADODB.Recordset rsQuote = null;

            string strBookingId = string.Empty;
            string strOther = string.Empty;
            string strUserId = string.Empty;
            string strAgencyCode = string.Empty;
            string strCurrency = string.Empty;

            short iAdult = 0;
            short iChild = 0;
            short iInfant = 0;
            short iOther = 0;
            DataSet ds = null;
            Helper objHelper = new Helper();
            tikAeroProcess.BookingSaveError enumSaveError = tikAeroProcess.BookingSaveError.OK;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;

                    remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { 
                    objBooking = new tikAeroProcess.Booking();
                    objInventory = new tikAeroProcess.Inventory();
                }

                objBooking.GetEmpty(ref rsHeader,
                                    ref rsSegment,
                                    ref rsPassenger,
                                    ref rsRemark,
                                    ref rsPayment,
                                    ref rsMapping,
                                    ref rsService,
                                    ref rsTax,
                                    ref rsFees,
                                    ref rsFlight,
                                    ref rsQuote,
                                    ref strBookingId,
                                    ref strAgencyCode,
                                    ref strCurrency,
                                    ref iAdult,
                                    ref iChild,
                                    ref iInfant,
                                    ref iOther,
                                    ref strOther,
                                    ref strUserId);


                DatasetToRecordset(header, ref rsHeader);
                DatasetToRecordset(segment, ref rsSegment);
                DatasetToRecordset(passenger, ref rsPassenger);
                DatasetToRecordset(fee, ref rsFees);
                DatasetToRecordset(remark, ref rsRemark);
                DatasetToRecordset(payment, ref rsPayment);
                DatasetToRecordset(mapping, ref rsMapping);
                DatasetToRecordset(service, ref rsService);
                DatasetToRecordset(tax, ref rsTax);

                // check mappint type MM
                //SetMMToMapping(ref rsMapping, ref rsSegment);
               
                // check SSR type RQ
                if(service != null)
                    SetServiceToRQ(ref rsMapping, ref rsService);
                                    

                if (ValidateSave(rsHeader, rsSegment, rsPassenger, rsMapping) == true)
                {
                    rsHeader.MoveFirst();
                    if (CheckInfantOverLimit(objInventory, rsSegment, Convert.ToInt32(rsHeader.Fields["number_of_infants"].Value)) == false)
                    {
                        bCheckSessionTimeOut = false;

                        enumSaveError = objBooking.Save(ref rsHeader,
                                                  ref rsSegment,
                                                  ref rsPassenger,
                                                  ref rsRemark,
                                                  ref rsPayment,
                                                  ref rsMapping,
                                                  ref rsService,
                                                  ref rsTax,
                                                  ref rsFees,
                                                  ref createTickets,
                                                  ref readBooking,
                                                  ref readOnly,
                                                  ref bSetLock,
                                                  ref bCheckSeatAssignment,
                                                  ref bCheckSessionTimeOut);

                        if (enumSaveError != tikAeroProcess.BookingSaveError.OK)
                        {
                            if (ds != null)
                            { ds.Clear(); }
                            if (enumSaveError != tikAeroProcess.BookingSaveError.SESSIONTIMEOUT)
                            {
                                //Sent Error log Email
                                Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" ,
                                                                                                                                      "SESSIONTIMEOUT",
                                                                                                                                      "",
                                                                                                                                      "SaveBookingCreditCard",
                                                                                                                                      "TikAeroWebMain-ComplusBooking",
                                                                                                                                      "");
                                objHelper.CreateErrorDataset(ref ds, "202", "SESSIONTIMEOUT", string.Empty, "Payments");
                            }
                            else if (enumSaveError != tikAeroProcess.BookingSaveError.DUPLICATESEAT)
                            {
                                //Sent Error log Email
                                Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" ,
                                                                                                                                      "DUPLICATESEAT",
                                                                                                                                      "",
                                                                                                                                      "SaveBookingCreditCard",
                                                                                                                                      "TikAeroWebMain-ComplusBooking",
                                                                                                                                      "");
                                objHelper.CreateErrorDataset(ref ds, "203", "DUPLICATESEAT", string.Empty, "Payments");
                            }
                            else if (enumSaveError != tikAeroProcess.BookingSaveError.DATABASEACCESS)
                            {
                                //Sent Error log Email
                                Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" ,
                                                                                                                                      "DATABASEACCESS",
                                                                                                                                      "",
                                                                                                                                      "SaveBookingCreditCard",
                                                                                                                                      "TikAeroWebMain-ComplusBooking",
                                                                                                                                      "");
                                objHelper.CreateErrorDataset(ref ds, "204", "DATABASEACCESS", string.Empty, "Payments");
                            }
                            else if (enumSaveError != tikAeroProcess.BookingSaveError.BOOKINGINUSE)
                            {
                                //Sent Error log Email
                                Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" ,
                                                                                                                                      "BOOKINGINUSE",
                                                                                                                                      "",
                                                                                                                                      "SaveBookingCreditCard",
                                                                                                                                      "TikAeroWebMain-ComplusBooking",
                                                                                                                                      "");
                                objHelper.CreateErrorDataset(ref ds, "205", "BOOKINGINUSE", string.Empty, "Payments");
                            }
                            else if (enumSaveError != tikAeroProcess.BookingSaveError.BOOKINGREADERROR)
                            {
                                //Sent Error log Email
                                Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" ,
                                                                                                                                      "BOOKINGREADERROR",
                                                                                                                                      "",
                                                                                                                                      "SaveBookingCreditCard",
                                                                                                                                      "TikAeroWebMain-ComplusBooking",
                                                                                                                                      "");
                                objHelper.CreateErrorDataset(ref ds, "206", "BOOKINGREADERROR", string.Empty, "Payments");
                            }
                        }
                        else
                            objHelper.CreateErrorDataset(ref ds, "000", "APPROVED", string.Empty, "Payments");
                    }
                    else
                    {
                        //infant over limit error code.
                        objHelper.CreateErrorDataset(ref ds, "200", "Infant over limit", string.Empty, "Payments");
                    }
                }
            }
            catch(Exception ex)
            {
                ds.Clear();
                throw;
            }
            finally
            {
                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }

                if (objInventory != null)
                {
                    Marshal.FinalReleaseComObject(objInventory);
                    objInventory = null;
                }

                ClearRecordset(ref rsHeader);
                ClearRecordset(ref rsSegment);
                ClearRecordset(ref rsPassenger);
                ClearRecordset(ref rsRemark);
                ClearRecordset(ref rsPayment);
                ClearRecordset(ref rsMapping);
                ClearRecordset(ref rsService);
                ClearRecordset(ref rsTax);
                ClearRecordset(ref rsFees);
            }

            return ds;
        }

        public DataSet BookingSaveSubLoad(DataTable header,
                        DataTable segment,
                        DataTable passenger,
                        DataTable remark,
                        DataTable payment,
                        DataTable mapping,
                        DataTable service,
                        DataTable tax,
                        DataTable fee,
                        bool createTickets,
                        bool readBooking,
                        bool readOnly,
                        bool bSetLock,
                        bool bCheckSeatAssignment,
                        bool bCheckSessionTimeOut)
        {
            tikAeroProcess.Booking objBooking = null;
            tikAeroProcess.Inventory objInventory = null;

            ADODB.Recordset rsHeader = null;
            ADODB.Recordset rsSegment = null;
            ADODB.Recordset rsPassenger = null;
            ADODB.Recordset rsRemark = null;
            ADODB.Recordset rsPayment = null;
            ADODB.Recordset rsMapping = null;
            ADODB.Recordset rsService = null;
            ADODB.Recordset rsTax = null;
            ADODB.Recordset rsFees = null;

            ADODB.Recordset rsFlight = null;
            ADODB.Recordset rsQuote = null;

            string strBookingId = string.Empty;
            string strOther = string.Empty;
            string strUserId = string.Empty;
            string strAgencyCode = string.Empty;
            string strCurrency = string.Empty;

            short iAdult = 0;
            short iChild = 0;
            short iInfant = 0;
            short iOther = 0;
            DataSet ds = null;
            Helper objHelper = new Helper();
            tikAeroProcess.BookingSaveError enumSaveError = tikAeroProcess.BookingSaveError.OK;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;

                    remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                {
                    objBooking = new tikAeroProcess.Booking();
                    objInventory = new tikAeroProcess.Inventory();
                }

                bCheckSeatAssignment = false;
                bCheckSessionTimeOut = false;

                objBooking.GetEmpty(ref rsHeader,
                                    ref rsSegment,
                                    ref rsPassenger,
                                    ref rsRemark,
                                    ref rsPayment,
                                    ref rsMapping,
                                    ref rsService,
                                    ref rsTax,
                                    ref rsFees,
                                    ref rsFlight,
                                    ref rsQuote,
                                    ref strBookingId,
                                    ref strAgencyCode,
                                    ref strCurrency,
                                    ref iAdult,
                                    ref iChild,
                                    ref iInfant,
                                    ref iOther,
                                    ref strOther,
                                    ref strUserId);


                DatasetToRecordset(header, ref rsHeader);
                DatasetToRecordset(segment, ref rsSegment);
                DatasetToRecordset(passenger, ref rsPassenger);
                DatasetToRecordset(fee, ref rsFees);
                DatasetToRecordset(remark, ref rsRemark);
                DatasetToRecordset(payment, ref rsPayment);
                DatasetToRecordset(mapping, ref rsMapping);
                DatasetToRecordset(service, ref rsService);
                DatasetToRecordset(tax, ref rsTax);

                if (ValidateSave(rsHeader, rsSegment, rsPassenger, rsMapping) == true)
                {
                    rsHeader.MoveFirst();
                    if (CheckInfantOverLimit(objInventory, rsSegment, Convert.ToInt32(rsHeader.Fields["number_of_infants"].Value)) == false)
                    {
                        enumSaveError = objBooking.Save(ref rsHeader,
                                                  ref rsSegment,
                                                  ref rsPassenger,
                                                  ref rsRemark,
                                                  ref rsPayment,
                                                  ref rsMapping,
                                                  ref rsService,
                                                  ref rsTax,
                                                  ref rsFees,
                                                  ref createTickets,
                                                  ref readBooking,
                                                  ref readOnly,
                                                  ref bSetLock,
                                                  ref bCheckSeatAssignment,
                                                  ref bCheckSessionTimeOut);

                        if (enumSaveError != tikAeroProcess.BookingSaveError.OK)
                        {
                            if (ds != null)
                            { ds.Clear(); }
                            if (enumSaveError != tikAeroProcess.BookingSaveError.SESSIONTIMEOUT)
                            {
                                //Sent Error log Email
                                Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>",
                                                                                                                                      "SESSIONTIMEOUT",
                                                                                                                                      "",
                                                                                                                                      "SaveBookingCreditCard",
                                                                                                                                      "TikAeroWebMain-ComplusBooking",
                                                                                                                                      "");
                                objHelper.CreateErrorDataset(ref ds, "202", "SESSIONTIMEOUT", string.Empty, "Payments");
                            }
                            else if (enumSaveError != tikAeroProcess.BookingSaveError.DUPLICATESEAT)
                            {
                                //Sent Error log Email
                                Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>",
                                                                                                                                      "DUPLICATESEAT",
                                                                                                                                      "",
                                                                                                                                      "SaveBookingCreditCard",
                                                                                                                                      "TikAeroWebMain-ComplusBooking",
                                                                                                                                      "");
                                objHelper.CreateErrorDataset(ref ds, "203", "DUPLICATESEAT", string.Empty, "Payments");
                            }
                            else if (enumSaveError != tikAeroProcess.BookingSaveError.DATABASEACCESS)
                            {
                                //Sent Error log Email
                                Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>",
                                                                                                                                      "DATABASEACCESS",
                                                                                                                                      "",
                                                                                                                                      "SaveBookingCreditCard",
                                                                                                                                      "TikAeroWebMain-ComplusBooking",
                                                                                                                                      "");
                                objHelper.CreateErrorDataset(ref ds, "204", "DATABASEACCESS", string.Empty, "Payments");
                            }
                            else if (enumSaveError != tikAeroProcess.BookingSaveError.BOOKINGINUSE)
                            {
                                //Sent Error log Email
                                Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>",
                                                                                                                                      "BOOKINGINUSE",
                                                                                                                                      "",
                                                                                                                                      "SaveBookingCreditCard",
                                                                                                                                      "TikAeroWebMain-ComplusBooking",
                                                                                                                                      "");
                                objHelper.CreateErrorDataset(ref ds, "205", "BOOKINGINUSE", string.Empty, "Payments");
                            }
                            else if (enumSaveError != tikAeroProcess.BookingSaveError.BOOKINGREADERROR)
                            {
                                //Sent Error log Email
                                Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>",
                                                                                                                                      "BOOKINGREADERROR",
                                                                                                                                      "",
                                                                                                                                      "SaveBookingCreditCard",
                                                                                                                                      "TikAeroWebMain-ComplusBooking",
                                                                                                                                      "");
                                objHelper.CreateErrorDataset(ref ds, "206", "BOOKINGREADERROR", string.Empty, "Payments");
                            }
                        }
                        else
                            objHelper.CreateErrorDataset(ref ds, "000", "APPROVED", string.Empty, "Payments");
                    }
                    else
                    {
                        //infant over limit error code.
                        objHelper.CreateErrorDataset(ref ds, "200", "Infant over limit", string.Empty, "Payments");
                    }
                }
            }
            catch
            {
                ds.Clear();
                throw;
            }
            finally
            {
                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }

                if (objInventory != null)
                {
                    Marshal.FinalReleaseComObject(objInventory);
                    objInventory = null;
                }

                ClearRecordset(ref rsHeader);
                ClearRecordset(ref rsSegment);
                ClearRecordset(ref rsPassenger);
                ClearRecordset(ref rsRemark);
                ClearRecordset(ref rsPayment);
                ClearRecordset(ref rsMapping);
                ClearRecordset(ref rsService);
                ClearRecordset(ref rsTax);
                ClearRecordset(ref rsFees);
            }

            return ds;
        }



        public string GetItinerary(string bookingId,
                                    string language,
                                    string recordLocator,
                                    string nameOrPhone,
                                    string agency,
                                    bool getSegment,
                                    bool getPassenger,
                                    bool getRemark,
                                    bool getPayment,
                                    bool getMapping,
                                    bool getService,
                                    bool getTax,
                                    bool getFee)
        {

            DataSet ds = new DataSet();
            string strResult = string.Empty;

            //Convert Datatable to Recordset
            ADODB.Recordset rsHeader = null;
            ADODB.Recordset rsSegment = null;
            ADODB.Recordset rsPassenger = null;
            ADODB.Recordset rsRemark = null;
            ADODB.Recordset rsPayment = null;
            ADODB.Recordset rsMapping = null;
            ADODB.Recordset rsService = null;
            ADODB.Recordset rsTax = null;
            ADODB.Recordset rsFee = null;

            try
            {

                tikAeroProcess.Booking objBooking = null;
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Booking(); }

                if (objBooking != null)
                {
                    //Call Complus function
                    ds.DataSetName = "Booking";
                    if (objBooking.GetItinerary(ref bookingId, ref language, ref recordLocator,
                                                      ref nameOrPhone, ref agency, ref rsHeader,
                                                      ref rsSegment, ref rsPassenger, ref rsRemark,
                                                      ref rsPayment, ref rsMapping, ref rsService,
                                                      ref rsTax, ref rsFee, ref getSegment,
                                                      ref getPassenger, ref getRemark, ref getPayment,
                                                      ref getMapping, ref getService, ref getTax,
                                                      ref getFee) == true)
                    {
                        //Convert Recordset to Datatable and Release com object
                        RecordsetToDataset(ds, rsHeader, "BookingHeader");
                        if (getSegment == true)
                        {
                            RecordsetToDataset(ds, rsSegment, "FlightSegment");
                        }
                        if (getPassenger == true)
                        {
                            RecordsetToDataset(ds, rsPassenger, "Passenger");
                        }
                        if (getRemark == true)
                        {
                            RecordsetToDataset(ds, rsRemark, "Remark");
                        }
                        if (getPayment == true)
                        {
                            RecordsetToDataset(ds, rsPayment, "Payment");
                        }
                        if (getMapping == true)
                        {
                            RecordsetToDataset(ds, rsMapping, "Mapping");
                        }
                        if (getService == true)
                        {
                            RecordsetToDataset(ds, rsService, "Service");
                        }
                        if (getTax == true)
                        {
                            RecordsetToDataset(ds, rsTax, "Tax");
                        }
                        if (getFee == true)
                        {
                            RecordsetToDataset(ds, rsFee, "Fee");
                        }
                    }
                    Marshal.FinalReleaseComObject(objBooking);
                    if (ds.Tables.Count > 0)
                    {
                        strResult = ds.GetXml();
                    }
                }
                objBooking = null;
            }
            catch
            { }
            finally
            {
                ClearRecordset(ref rsHeader);
                ClearRecordset(ref rsSegment);
                ClearRecordset(ref rsPassenger);
                ClearRecordset(ref rsRemark);
                ClearRecordset(ref rsPayment);
                ClearRecordset(ref rsMapping);
                ClearRecordset(ref rsService);
                ClearRecordset(ref rsTax);
                ClearRecordset(ref rsFee);
            }

            return strResult;
        }
        public DataSet SaveBookingCreditCard(DataTable header,
                                           DataTable segment,
                                           DataTable passenger,
                                           DataTable remark,
                                           DataTable payment,
                                           DataTable mapping,
                                           DataTable service,
                                           DataTable tax,
                                           DataTable fee,
                                           DataTable paymentFee,
                                           DataTable voucher,
                                           string securityToken,
                                           string authenticationToken,
                                           string commerceIndicator,
                                           string bookingReference,
                                           string strRequestSource,
                                           bool createTickets,
                                           bool readBooking,
                                           bool readOnly)
        {
            tikAeroProcess.Booking objBooking = null;
            tikAeroProcess.clsCreditCard objCreditCard = null;
            tikAeroProcess.Inventory objInventory = null;
            tikAeroProcess.Reservation objReservation = null;

            ADODB.Recordset rsHeader = null;
            ADODB.Recordset rsSegment = null;
            ADODB.Recordset rsPassenger = null;
            ADODB.Recordset rsRemark = null;
            ADODB.Recordset rsPayment = null;
            ADODB.Recordset rsTempPayment = null;
            ADODB.Recordset rsMapping = null;
            ADODB.Recordset rsService = null;
            ADODB.Recordset rsTax = null;
            ADODB.Recordset rsFees = null;

            ADODB.Recordset rsFlight = null;
            ADODB.Recordset rsQuote = null;
            ADODB.Recordset rsAllocation = FabricatePaymentAllocationRecordset();
            ADODB.Recordset rsVoucher = null;
            tikAeroProcess.BookingSaveError enumSaveError = tikAeroProcess.BookingSaveError.OK;

            string strBookingId = string.Empty;
            string strOther = string.Empty;
            string strUserId = string.Empty;
            string strAgencyCode = string.Empty;
            string strCurrency = string.Empty;
            
            short iAdult = 0;
            short iChild = 0;
            short iInfant = 0;
            short iOther = 0;

            bool bWeb = false;
            bool bDupSeat = false;
            bool bFoundSessionLock = false;
            DataSet ds = null;
            Helper objHelper = new Helper();

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;

                    //Create credit card object
                    remote = Type.GetTypeFromProgID("tikAeroProcess.clsCreditCard", _server);
                    objCreditCard = (tikAeroProcess.clsCreditCard)Activator.CreateInstance(remote);
                    remote = null;

                    remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;

                    remote = Type.GetTypeFromProgID("tikAeroProcess.Reservation", _server);
                    objReservation = (tikAeroProcess.Reservation)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                {
                    objBooking = new tikAeroProcess.Booking();
                    objCreditCard = new tikAeroProcess.clsCreditCard();
                    objInventory = new tikAeroProcess.Inventory();
                    objReservation = new tikAeroProcess.Reservation();
                }

                objBooking.GetEmpty(ref rsHeader,
                                    ref rsSegment,
                                    ref rsPassenger,
                                    ref rsRemark,
                                    ref rsPayment,
                                    ref rsMapping,
                                    ref rsService,
                                    ref rsTax,
                                    ref rsFees,
                                    ref rsFlight,
                                    ref rsQuote,
                                    ref strBookingId,
                                    ref strAgencyCode,
                                    ref strCurrency,
                                    ref iAdult,
                                    ref iChild,
                                    ref iInfant,
                                    ref iOther,
                                    ref strOther,
                                    ref strUserId);

                DatasetToRecordset(header, ref rsHeader);
                DatasetToRecordset(segment, ref rsSegment);
                DatasetToRecordset(passenger, ref rsPassenger);
                DatasetToRecordset(fee, ref rsFees);
                DatasetToRecordset(remark, ref rsRemark);
                DatasetToRecordset(payment, ref rsPayment);
                DatasetToRecordset(mapping, ref rsMapping);
                DatasetToRecordset(service, ref rsService);
                DatasetToRecordset(tax, ref rsTax);

                // check mappint type MM
                //be fill in at web api side
               // SetMMToMapping(ref rsMapping, ref rsSegment);

                if (ValidateSave(rsHeader, rsSegment, rsPassenger, rsMapping) == true)
                {
                    rsHeader.MoveFirst();
                    if (CheckInfantOverLimit(objInventory, rsSegment, Convert.ToInt32(rsHeader.Fields["number_of_infants"].Value)) == false)
                    {
                        //Generate payment allocation recordset.
                        DatasetToRecordset(GetAllocation(mapping, fee, paymentFee, payment.Rows[0]["update_by"].ToString()), ref rsAllocation);

                        //Validate Duplidate seat.
                        ADODB.Recordset rsA = objReservation.VerifySeatAssignment(rsMapping, rsSegment);
                        if (rsA == null)
                        { }
                        else if (rsA.EOF == false)
                        {
                            bDupSeat = true;
                        }
                        ClearRecordset(ref rsA);
                        //Clear COM object.
                        if (objReservation != null)
                        {
                            Marshal.FinalReleaseComObject(objReservation);
                            objReservation = null;
                        }
                        //Validate Session Lock.
                        bFoundSessionLock = objInventory.VerifyFlightInventorySession(rsSegment);
                        if (objInventory != null)
                        {
                            Marshal.FinalReleaseComObject(objInventory);
                            objInventory = null;
                        }
                        if (bFoundSessionLock == false)
                        {
                            objHelper.CreateErrorDataset(ref ds, "202", "SESSIONTIMEOUT", string.Empty, "Payments");
                        }
                        else if (bDupSeat == true)
                        {
                            objHelper.CreateErrorDataset(ref ds, "203", "DUPLICATESEAT", string.Empty, "Payments");
                        }
                        else
                        {
                            //Assign booking id use for logging.
                            if (rsHeader != null && rsHeader.RecordCount > 0)
                            {
                                strBookingId = rsHeader.Fields["booking_id"].Value.ToString();
                            }

                            //Assigning booking reference.
                            if (string.IsNullOrEmpty(bookingReference))
                            {
                                int iBookingNumber = 0;

                                if (rsHeader.Fields["record_locator"].Value is System.DBNull || rsHeader.Fields["record_locator"].Value.ToString().Length == 0)
                                {
                                    objBooking.GetRecordLocator(ref bookingReference, ref iBookingNumber);
                                }
                                else
                                {
                                    bookingReference = rsHeader.Fields["record_locator"].Value.ToString();
                                    iBookingNumber = Convert.ToInt32(rsHeader.Fields["booking_number"].Value);
                                }
                                //Assign value to booking header
                                if (string.IsNullOrEmpty(bookingReference) == false && iBookingNumber > 0)
                                {
                                    rsHeader.Fields["booking_number"].Value = iBookingNumber;
                                    rsHeader.Fields["record_locator"].Value = bookingReference;
                                }
                            }

                            SetRecordToMapping(ref rsMapping,bookingReference);

                            //Call to validate credit card payment.
                            ADODB.Recordset rs = objCreditCard.Authorize(rsPayment,
                                                                        rsSegment,
                                                                        ref rsAllocation,
                                                                        ref rsVoucher,
                                                                        ref rsMapping,
                                                                        ref rsFees,
                                                                        ref rsHeader,
                                                                        ref rsTax,
                                                                        ref securityToken,
                                                                        ref authenticationToken,
                                                                        ref commerceIndicator,
                                                                        ref bookingReference,
                                                                        ref strRequestSource,
                                                                        ref bWeb);

                            ds = RecordsetToDataset(rs, "Payments");

                            if (ds != null && ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows[0]["ResponseCode"].ToString() == "APPROVED")
                                {
                                    enumSaveError = objBooking.Save(ref rsHeader,
                                                                   ref rsSegment,
                                                                   ref rsPassenger,
                                                                   ref rsRemark,
                                                                   ref rsTempPayment,
                                                                   ref rsMapping,
                                                                   ref rsService,
                                                                   ref rsTax,
                                                                   ref rsFees,
                                                                   ref createTickets,
                                                                   ref readBooking,
                                                                   ref readOnly,
                                                                   true);

                                    if (enumSaveError != tikAeroProcess.BookingSaveError.OK)
                                    {
                                        if (ds != null)
                                        { ds.Clear(); }
                                        if (enumSaveError != tikAeroProcess.BookingSaveError.SESSIONTIMEOUT)
                                        {
                                            //Sent Error log Email
                                            Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" +
                                                                                                                                                  "Booking reference : " + bookingReference + "<br/>",
                                                                                                                                                  "SESSIONTIMEOUT",
                                                                                                                                                  "",
                                                                                                                                                  "SaveBookingCreditCard",
                                                                                                                                                  "TikAeroWebMain-ComplusBooking",
                                                                                                                                                  "");
                                            objHelper.CreateErrorDataset(ref ds, "202", "SESSIONTIMEOUT", string.Empty, "Payments");
                                        }
                                        else if (enumSaveError != tikAeroProcess.BookingSaveError.DUPLICATESEAT)
                                        {
                                            //Sent Error log Email
                                            Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" +
                                                                                                                                                  "Booking reference : " + bookingReference + "<br/>",
                                                                                                                                                  "DUPLICATESEAT",
                                                                                                                                                  "",
                                                                                                                                                  "SaveBookingCreditCard",
                                                                                                                                                  "TikAeroWebMain-ComplusBooking",
                                                                                                                                                  "");
                                            objHelper.CreateErrorDataset(ref ds, "203", "DUPLICATESEAT", string.Empty, "Payments");
                                        }
                                        else if (enumSaveError != tikAeroProcess.BookingSaveError.DATABASEACCESS)
                                        {
                                            //Sent Error log Email
                                            Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" +
                                                                                                                                                  "Booking reference : " + bookingReference + "<br/>",
                                                                                                                                                  "DATABASEACCESS",
                                                                                                                                                  "",
                                                                                                                                                  "SaveBookingCreditCard",
                                                                                                                                                  "TikAeroWebMain-ComplusBooking",
                                                                                                                                                  "");
                                            objHelper.CreateErrorDataset(ref ds, "204", "DATABASEACCESS", string.Empty, "Payments");
                                        }
                                        else if (enumSaveError != tikAeroProcess.BookingSaveError.BOOKINGINUSE)
                                        {
                                            //Sent Error log Email
                                            Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" +
                                                                                                                                                  "Booking reference : " + bookingReference + "<br/>",
                                                                                                                                                  "BOOKINGINUSE",
                                                                                                                                                  "",
                                                                                                                                                  "SaveBookingCreditCard",
                                                                                                                                                  "TikAeroWebMain-ComplusBooking",
                                                                                                                                                  "");
                                            objHelper.CreateErrorDataset(ref ds, "205", "BOOKINGINUSE", string.Empty, "Payments");
                                        }
                                        else if (enumSaveError != tikAeroProcess.BookingSaveError.BOOKINGREADERROR)
                                        {
                                            //Sent Error log Email
                                            Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" +
                                                                                                                                                  "Booking reference : " + bookingReference + "<br/>",
                                                                                                                                                  "BOOKINGREADERROR",
                                                                                                                                                  "",
                                                                                                                                                  "SaveBookingCreditCard",
                                                                                                                                                  "TikAeroWebMain-ComplusBooking",
                                                                                                                                                  "");
                                            objHelper.CreateErrorDataset(ref ds, "206", "BOOKINGREADERROR", string.Empty, "Payments");
                                        }
                                    }
                                }
                            }
                            ClearRecordset(ref rsAllocation);
                        }
                    }
                    else
                    {
                        objHelper.CreateErrorDataset(ref ds, "200", "Infant over limit", string.Empty, "Payments");
                    }
                }
            }
            catch
            {
                ds.Clear();
                throw;
            }
            finally
            {
                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }
                if (objCreditCard != null)
                {
                    Marshal.FinalReleaseComObject(objCreditCard);
                    objCreditCard = null;
                }
                
                ClearRecordset(ref rsHeader);
                ClearRecordset(ref rsSegment);
                ClearRecordset(ref rsPassenger);
                ClearRecordset(ref rsRemark);
                ClearRecordset(ref rsPayment);
                ClearRecordset(ref rsTempPayment);
                ClearRecordset(ref rsMapping);
                ClearRecordset(ref rsService);
                ClearRecordset(ref rsTax);
                ClearRecordset(ref rsFees);
            }
            return ds;
        }
        public DataSet SaveBookingPayment(DataTable header,
                                        DataTable segment,
                                        DataTable passenger,
                                        DataTable remark,
                                        DataTable payment,
                                        DataTable mapping,
                                        DataTable service,
                                        DataTable tax,
                                        DataTable fee,
                                        DataTable paymentFee,
                                        DataTable voucher,
                                        DataTable refundVoucher,
                                        bool createTickets,
                                        bool readBooking,
                                        bool readOnly)
        {


            tikAeroProcess.Booking objBooking = null;
            tikAeroProcess.Payment objPayment = null;
            tikAeroProcess.Inventory objInventory = null;
            tikAeroProcess.Reservation objReservation = null;

            ADODB.Recordset rsHeader = null;
            ADODB.Recordset rsSegment = null;
            ADODB.Recordset rsPassenger = null;
            ADODB.Recordset rsRemark = null;
            ADODB.Recordset rsPayment = null;
            ADODB.Recordset rsTempPayment = null;
            ADODB.Recordset rsMapping = null;
            ADODB.Recordset rsService = null;
            ADODB.Recordset rsTax = null;
            ADODB.Recordset rsFees = null;

            ADODB.Recordset rsFlight = null;
            ADODB.Recordset rsQuote = null;
            ADODB.Recordset rsAllocation = FabricatePaymentAllocationRecordset();
            ADODB.Recordset rsRefundVoucher = null;
            tikAeroProcess.BookingSaveError enumSaveError = tikAeroProcess.BookingSaveError.OK;

            string strBookingId = string.Empty;
            string strOther = string.Empty;
            string strUserId = string.Empty;
            string strAgencyCode = string.Empty;
            string strCurrency = string.Empty;

            short iAdult = 0;
            short iChild = 0;
            short iInfant = 0;
            short iOther = 0;

            bool bDupSeat = false;
            bool bFoundSessionLock = false;
            DataSet ds = null;
            Helper objHelper = new Helper();
            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;

                    remote = Type.GetTypeFromProgID("tikAeroProcess.Payment", _server);
                    objPayment = (tikAeroProcess.Payment)Activator.CreateInstance(remote);
                    remote = null;

                    remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;

                    remote = Type.GetTypeFromProgID("tikAeroProcess.Reservation", _server);
                    objReservation = (tikAeroProcess.Reservation)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                {
                    objBooking = new tikAeroProcess.Booking();
                    objPayment = new tikAeroProcess.Payment();

                    objInventory = new tikAeroProcess.Inventory();
                    objReservation = new tikAeroProcess.Reservation();
                }

                objBooking.GetEmpty(ref rsHeader,
                                    ref rsSegment,
                                    ref rsPassenger,
                                    ref rsRemark,
                                    ref rsPayment,
                                    ref rsMapping,
                                    ref rsService,
                                    ref rsTax,
                                    ref rsFees,
                                    ref rsFlight,
                                    ref rsQuote,
                                    ref strBookingId,
                                    ref strAgencyCode,
                                    ref strCurrency,
                                    ref iAdult,
                                    ref iChild,
                                    ref iInfant,
                                    ref iOther,
                                    ref strOther,
                                    ref strUserId);

                //Get Structure of payment recordset

                DatasetToRecordset(header, ref rsHeader);
                DatasetToRecordset(segment, ref rsSegment);
                DatasetToRecordset(passenger, ref rsPassenger);
                DatasetToRecordset(fee, ref rsFees);
                DatasetToRecordset(remark, ref rsRemark);
                DatasetToRecordset(payment, ref rsPayment);
                DatasetToRecordset(mapping, ref rsMapping);
                DatasetToRecordset(service, ref rsService);
                DatasetToRecordset(tax, ref rsTax);

                //Recordset for payment save.
                if (ValidateSave(rsHeader, rsSegment, rsPassenger, rsMapping) == true)
                {
                    rsHeader.MoveFirst();
                    if (CheckInfantOverLimit(objInventory, rsSegment, Convert.ToInt32(rsHeader.Fields["number_of_infants"].Value)) == false)
                    {
                        DatasetToRecordset(GetAllocation(mapping, fee, paymentFee, payment.Rows[0]["update_by"].ToString()),
                                                          ref rsAllocation);

                        //Validate Duplidate seat.
                        ADODB.Recordset rsA = objReservation.VerifySeatAssignment(rsMapping, rsSegment);
                        if (rsA == null)
                        { }
                        else if (rsA.EOF == false)
                        {
                            bDupSeat = true;
                        }
                        ClearRecordset(ref rsA);

                        //Clear COM object.
                        if (objReservation != null)
                        {
                            Marshal.FinalReleaseComObject(objReservation);
                            objReservation = null;
                        }
                        //Validate Session Lock.
                        bFoundSessionLock = objInventory.VerifyFlightInventorySession(rsSegment);
                        if (objInventory != null)
                        {
                            Marshal.FinalReleaseComObject(objInventory);
                            objInventory = null;
                        }
                        if (bFoundSessionLock == false)
                        {
                            //Session timeout
                            objHelper.CreateErrorDataset(ref ds, "202", "SESSIONTIMEOUT", string.Empty, "Payments");
                        }
                        else if (bDupSeat == true)
                        {
                            //Duplicate seat
                            objHelper.CreateErrorDataset(ref ds, "203", "DUPLICATESEAT", string.Empty, "Payments");
                        }
                        else
                        {
                            //Get booking Id from booking header using in loggin.
                            if (rsHeader != null && rsHeader.RecordCount > 0)
                            {
                                strBookingId = rsHeader.Fields["booking_id"].Value.ToString();
                            }

                            if (objPayment.Save(ref rsPayment, ref rsAllocation, ref rsRefundVoucher) == true)
                            {
                                enumSaveError = objBooking.Save(ref rsHeader,
                                                                ref rsSegment,
                                                                ref rsPassenger,
                                                                ref rsRemark,
                                                                ref rsTempPayment,
                                                                ref rsMapping,
                                                                ref rsService,
                                                                ref rsTax,
                                                                ref rsFees,
                                                                ref createTickets,
                                                                ref readBooking,
                                                                ref readOnly);

                                if (enumSaveError != tikAeroProcess.BookingSaveError.OK)
                                {
                                    if (ds != null)
                                    { ds.Clear(); }
                                    if (enumSaveError != tikAeroProcess.BookingSaveError.SESSIONTIMEOUT)
                                    {
                                        //Sent Error log Email
                                        Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" +
                                                                                                                                              "Error Code : " + Convert.ToInt16(enumSaveError).ToString() + "<br/>",
                                                                                                                                              "SESSIONTIMEOUT",
                                                                                                                                              "",
                                                                                                                                              "SaveBookingPayment",
                                                                                                                                              "TikAeroWebMain-ComplusBooking",
                                                                                                                                              "");
                                        objHelper.CreateErrorDataset(ref ds, "202", "SESSIONTIMEOUT", string.Empty, "Payments");
                                    }
                                    else if (enumSaveError != tikAeroProcess.BookingSaveError.DUPLICATESEAT)
                                    {
                                        //Sent Error log Email
                                        Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" +
                                                                                                                                              "Error Code : " + Convert.ToInt16(enumSaveError).ToString() + "<br/>",
                                                                                                                                              "DUPLICATESEAT",
                                                                                                                                              "",
                                                                                                                                              "SaveBookingCreditCard",
                                                                                                                                              "TikAeroWebMain-ComplusBooking",
                                                                                                                                              "");
                                        objHelper.CreateErrorDataset(ref ds, "203", "DUPLICATESEAT", string.Empty, "Payments");
                                    }
                                    else if (enumSaveError != tikAeroProcess.BookingSaveError.DATABASEACCESS)
                                    {
                                        //Sent Error log Email
                                        Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" +
                                                                                                                                              "Error Code : " + Convert.ToInt16(enumSaveError).ToString() + "<br/>",
                                                                                                                                              "DATABASEACCESS",
                                                                                                                                              "",
                                                                                                                                              "SaveBookingCreditCard",
                                                                                                                                              "TikAeroWebMain-ComplusBooking",
                                                                                                                                              "");
                                        objHelper.CreateErrorDataset(ref ds, "204", "DATABASEACCESS", string.Empty, "Payments");
                                    }
                                    else if (enumSaveError != tikAeroProcess.BookingSaveError.BOOKINGINUSE)
                                    {
                                        //Sent Error log Email
                                        Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" +
                                                                                                                                              "Error Code : " + Convert.ToInt16(enumSaveError).ToString() + "<br/>",
                                                                                                                                              "BOOKINGINUSE",
                                                                                                                                              "",
                                                                                                                                              "SaveBookingCreditCard",
                                                                                                                                              "TikAeroWebMain-ComplusBooking",
                                                                                                                                              "");
                                        objHelper.CreateErrorDataset(ref ds, "205", "BOOKINGINUSE", string.Empty, "Payments");
                                    }
                                    else if (enumSaveError != tikAeroProcess.BookingSaveError.BOOKINGREADERROR)
                                    {
                                        //Sent Error log Email
                                        Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" +
                                                                                                                                              "Error Code : " + Convert.ToInt16(enumSaveError).ToString() + "<br/>",
                                                                                                                                              "BOOKINGREADERROR",
                                                                                                                                              "",
                                                                                                                                              "SaveBookingCreditCard",
                                                                                                                                              "TikAeroWebMain-ComplusBooking",
                                                                                                                                              "");
                                        objHelper.CreateErrorDataset(ref ds, "206", "BOOKINGREADERROR", string.Empty, "Payments");
                                    }
                                }
                                else
                                    objHelper.CreateErrorDataset(ref ds, "000", "APPROVED", string.Empty, "Payments");
                            }
                            ClearRecordset(ref rsAllocation);
                        }
                    }
                    else
                    {
                        objHelper.CreateErrorDataset(ref ds, "200", "Infant over limit", string.Empty, "Payments");
                    }
                }
            }
            catch
            {
                ds.Clear();
                throw;
            }
            finally
            {
                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }
                if (objPayment != null)
                {
                    Marshal.FinalReleaseComObject(objPayment);
                    objPayment = null;
                }
                ClearRecordset(ref rsHeader);
                ClearRecordset(ref rsSegment);
                ClearRecordset(ref rsPassenger);
                ClearRecordset(ref rsRemark);
                ClearRecordset(ref rsPayment);
                ClearRecordset(ref rsMapping);
                ClearRecordset(ref rsService);
                ClearRecordset(ref rsTax);
                ClearRecordset(ref rsFees);
                ClearRecordset(ref rsTempPayment);
                ClearRecordset(ref rsRefundVoucher);
            }

            return ds;
        }

        public DataSet SaveBookingMultipleFormOfPayment(DataTable header,
                                                        DataTable segment,
                                                        DataTable passenger,
                                                        DataTable remark,
                                                        DataTable payment,
                                                        DataTable mapping,
                                                        DataTable service,
                                                        DataTable tax,
                                                        DataTable fee,
                                                        DataTable paymentFee,
                                                        string bookingId,
                                                        bool createTickets,
                                                        string securityToken,
                                                        string authenticationToken,
                                                        string commerceIndicator,
                                                        string requestSource,
                                                        string language)
        {
            tikAeroProcess.Booking objBooking = null;
            tikAeroProcess.Payment objPayment = null;
            tikAeroProcess.Inventory objInventory = null;
            tikAeroProcess.Reservation objReservation = null;
            tikAeroProcess.clsCreditCard objCreditCard = null;
            tikAeroProcess.Miscellaneous objMiscellaneous = null;
            tikAeroProcess.System objComSystem = null;

            ADODB.Recordset rsHeader = null;
            ADODB.Recordset rsSegment = null;
            ADODB.Recordset rsPassenger = null;
            ADODB.Recordset rsRemark = null;
            ADODB.Recordset rsPayment = null;
            ADODB.Recordset rsMapping = null;
            ADODB.Recordset rsService = null;
            ADODB.Recordset rsTax = null;
            ADODB.Recordset rsFees = null;

            ADODB.Recordset rsFlight = null;
            ADODB.Recordset rsQuote = null;
            ADODB.Recordset rsAllocation = FabricatePaymentAllocationRecordset();
            ADODB.Recordset rsVoucher = null;
            ADODB.Recordset rsPaymentsVoucherUpdate = null;
            ADODB.Recordset rsPaymentsCCUpdate = null;

            ADODB.Recordset rsV = null;
            ADODB.Recordset raP = null;
            ADODB.Recordset rsC = null;

            tikAeroProcess.BookingSaveError enumSaveError = tikAeroProcess.BookingSaveError.OK;

            string strBookingId = string.Empty;
            string strOther = string.Empty;
            string strUserId = string.Empty;
            string strAgencyCode = string.Empty;
            string strCurrency = string.Empty;
            string strVoucherSessionId = Guid.NewGuid().ToString();
            string strBookingReference = string.Empty;
            bool bDupSeat = false;
            bool bFoundSessionLock = false;
            bool bWeb = false;
            bool bWrite = false;
            bool bCCvalidationPass = true;
            bool bVCValidateionPass = true;
            int iBookingNumber = 0;
            decimal dclVoucherBalance = 0;
            decimal dclPaymentAmount = 0;
            decimal dclAgencyAccountBalance = 0;
            decimal dclVoucherValue = 0;
            decimal dclPaymentTotal = 0;
            short iAdult = 0;
            short iChild = 0;
            short iInfant = 0;
            short iOther = 0;
            DateTime dtFrom = DateTime.MinValue;
            DateTime dtTo = DateTime.MinValue;
            DataSet ds = null;
            Helper objHelper = new Helper();

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;

                    remote = Type.GetTypeFromProgID("tikAeroProcess.Payment", _server);
                    objPayment = (tikAeroProcess.Payment)Activator.CreateInstance(remote);
                    remote = null;

                    remote = Type.GetTypeFromProgID("tikAeroProcess.Inventory", _server);
                    objInventory = (tikAeroProcess.Inventory)Activator.CreateInstance(remote);
                    remote = null;

                    remote = Type.GetTypeFromProgID("tikAeroProcess.Reservation", _server);
                    objReservation = (tikAeroProcess.Reservation)Activator.CreateInstance(remote);
                    remote = null;

                    remote = Type.GetTypeFromProgID("tikAeroProcess.clsCreditCard", _server);
                    objCreditCard = (tikAeroProcess.clsCreditCard)Activator.CreateInstance(remote);
                    remote = null;

                    remote = Type.GetTypeFromProgID("tikAeroProcess.Miscellaneous", _server);
                    objMiscellaneous = (tikAeroProcess.Miscellaneous)Activator.CreateInstance(remote);
                    remote = null;

                    remote = Type.GetTypeFromProgID("tikAeroProcess.System", _server);
                    objComSystem = (tikAeroProcess.System)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                {
                    objBooking = new tikAeroProcess.Booking();
                    objPayment = new tikAeroProcess.Payment();
                    objInventory = new tikAeroProcess.Inventory();
                    objReservation = new tikAeroProcess.Reservation();
                    objCreditCard = new tikAeroProcess.clsCreditCard();
                    objMiscellaneous = new tikAeroProcess.Miscellaneous();
                    objComSystem = new tikAeroProcess.System();
                }

                objBooking.GetEmpty(ref rsHeader,
                                    ref rsSegment,
                                    ref rsPassenger,
                                    ref rsRemark,
                                    ref rsPayment,
                                    ref rsMapping,
                                    ref rsService,
                                    ref rsTax,
                                    ref rsFees,
                                    ref rsFlight,
                                    ref rsQuote,
                                    ref strBookingId,
                                    ref strAgencyCode,
                                    ref strCurrency,
                                    ref iAdult,
                                    ref iChild,
                                    ref iInfant,
                                    ref iOther,
                                    ref strOther,
                                    ref strUserId);

                //Get Structure of payment recordset
                DatasetToRecordset(header, ref rsHeader);
                DatasetToRecordset(segment, ref rsSegment);
                DatasetToRecordset(passenger, ref rsPassenger);
                DatasetToRecordset(fee, ref rsFees);
                DatasetToRecordset(remark, ref rsRemark);
                DatasetToRecordset(payment, ref rsPayment);
                DatasetToRecordset(mapping, ref rsMapping);
                DatasetToRecordset(service, ref rsService);
                DatasetToRecordset(tax, ref rsTax);

                //Split payment object
                if (rsPayment.RecordCount > 0)
                {
                    rsPaymentsCCUpdate = objPayment.GetEmpty();
                    rsPaymentsVoucherUpdate = objPayment.GetEmpty();
                    rsPayment.MoveFirst();
                    while (!rsPayment.EOF)
                    {
                        if (rsPayment.Fields["form_of_payment_rcd"].Value.Equals("CC"))
                        {
                            rsPaymentsCCUpdate.AddNew();
                            for (int i = 0; i < rsPayment.Fields.Count; i++)
                            {
                                switch (rsPayment.Fields[i].Type)
                                {
                                    case ADODB.DataTypeEnum.adGUID:
                                        if (string.IsNullOrEmpty(rsPayment.Fields[i].Value.ToString()))
                                        {
                                            RecordsetHelper.AssignRsGuid(rsPaymentsCCUpdate, rsPayment.Fields[i].Name, Guid.Empty);
                                        }
                                        else
                                        {
                                            RecordsetHelper.AssignRsGuid(rsPaymentsCCUpdate, rsPayment.Fields[i].Name, new Guid(rsPayment.Fields[i].Value.ToString()));
                                        }
                                        break;
                                    case ADODB.DataTypeEnum.adVarChar:
                                        RecordsetHelper.AssignRsString(rsPaymentsCCUpdate, rsPayment.Fields[i].Name, rsPayment.Fields[i].Value.ToString());
                                        break;
                                    case ADODB.DataTypeEnum.adDate:
                                        if (string.IsNullOrEmpty(rsPayment.Fields[i].Value.ToString()))
                                        {
                                            RecordsetHelper.AssignRsDateTime(rsPaymentsCCUpdate, rsPayment.Fields[i].Name, DateTime.MinValue);
                                        }
                                        else
                                        {
                                            RecordsetHelper.AssignRsDateTime(rsPaymentsCCUpdate, rsPayment.Fields[i].Name, Convert.ToDateTime(rsPayment.Fields[i].Value));
                                        }

                                        break;
                                    case ADODB.DataTypeEnum.adDBDate:
                                        if (string.IsNullOrEmpty(rsPayment.Fields[i].Value.ToString()))
                                        {
                                            RecordsetHelper.AssignRsDateTime(rsPaymentsCCUpdate, rsPayment.Fields[i].Name, DateTime.MinValue);
                                        }
                                        else
                                        {
                                            RecordsetHelper.AssignRsDateTime(rsPaymentsCCUpdate, rsPayment.Fields[i].Name, Convert.ToDateTime(rsPayment.Fields[i].Value));
                                        }

                                        break;
                                    case ADODB.DataTypeEnum.adDecimal:
                                        RecordsetHelper.AssignRsDecimal(rsPaymentsCCUpdate, rsPayment.Fields[i].Name, Convert.ToDecimal(rsPayment.Fields[i].Value));
                                        break;
                                    case ADODB.DataTypeEnum.adBoolean:
                                        RecordsetHelper.AssignRsBoolean(rsPaymentsCCUpdate, rsPayment.Fields[i].Name, Convert.ToByte(rsPayment.Fields[i].Value));
                                        break;
                                    case ADODB.DataTypeEnum.adTinyInt:
                                        rsPaymentsCCUpdate.Fields[i].Value = string.IsNullOrEmpty(rsPayment.Fields[i].Value.ToString()) ? 0 : Convert.ToInt16(rsPayment.Fields[i].Value);
                                        break;
                                    case ADODB.DataTypeEnum.adInteger:
                                        rsPaymentsCCUpdate.Fields[i].Value = string.IsNullOrEmpty(rsPayment.Fields[i].Value.ToString()) ? 0 : Convert.ToInt32(rsPayment.Fields[i].Value);
                                        break;
                                    case ADODB.DataTypeEnum.adBigInt:
                                        rsPaymentsCCUpdate.Fields[i].Value = string.IsNullOrEmpty(rsPayment.Fields[i].Value.ToString()) ? 0 : Convert.ToInt64(rsPayment.Fields[i].Value);
                                        break;
                                    default:

                                        RecordsetHelper.AssignRsString(rsPaymentsCCUpdate, rsPayment.Fields[i].Name, rsPayment.Fields[i].Value.ToString());
                                        break;
                                }
                            }

                        }
                        else
                        {
                            rsPaymentsVoucherUpdate.AddNew();
                            for (int i = 0; i < rsPayment.Fields.Count; i++)
                            {
                                switch (rsPayment.Fields[i].Type)
                                {
                                    case ADODB.DataTypeEnum.adGUID:
                                        if (string.IsNullOrEmpty(rsPayment.Fields[i].Value.ToString()))
                                        {
                                            RecordsetHelper.AssignRsGuid(rsPaymentsVoucherUpdate, rsPayment.Fields[i].Name, Guid.Empty);
                                        }
                                        else
                                        {
                                            RecordsetHelper.AssignRsGuid(rsPaymentsVoucherUpdate, rsPayment.Fields[i].Name, new Guid(rsPayment.Fields[i].Value.ToString()));
                                        }
                                        break;
                                    case ADODB.DataTypeEnum.adVarChar:
                                        RecordsetHelper.AssignRsString(rsPaymentsVoucherUpdate, rsPayment.Fields[i].Name, rsPayment.Fields[i].Value.ToString());
                                        break;
                                    case ADODB.DataTypeEnum.adDate:
                                        if (string.IsNullOrEmpty(rsPayment.Fields[i].Value.ToString()))
                                        {
                                            RecordsetHelper.AssignRsDateTime(rsPaymentsVoucherUpdate, rsPayment.Fields[i].Name, DateTime.MinValue);
                                        }
                                        else
                                        {
                                            RecordsetHelper.AssignRsDateTime(rsPaymentsVoucherUpdate, rsPayment.Fields[i].Name, Convert.ToDateTime(rsPayment.Fields[i].Value));
                                        }

                                        break;
                                    case ADODB.DataTypeEnum.adDBDate:
                                        if (string.IsNullOrEmpty(rsPayment.Fields[i].Value.ToString()))
                                        {
                                            RecordsetHelper.AssignRsDateTime(rsPaymentsVoucherUpdate, rsPayment.Fields[i].Name, DateTime.MinValue);
                                        }
                                        else
                                        {
                                            RecordsetHelper.AssignRsDateTime(rsPaymentsVoucherUpdate, rsPayment.Fields[i].Name, Convert.ToDateTime(rsPayment.Fields[i].Value));
                                        }
                                        break;
                                    case ADODB.DataTypeEnum.adDecimal:
                                        RecordsetHelper.AssignRsDecimal(rsPaymentsVoucherUpdate, rsPayment.Fields[i].Name, Convert.ToDecimal(rsPayment.Fields[i].Value));
                                        break;
                                    case ADODB.DataTypeEnum.adBoolean:
                                        RecordsetHelper.AssignRsBoolean(rsPaymentsVoucherUpdate, rsPayment.Fields[i].Name, Convert.ToByte(rsPayment.Fields[i].Value));
                                        break;
                                    case ADODB.DataTypeEnum.adTinyInt:
                                        rsPaymentsVoucherUpdate.Fields[i].Value = string.IsNullOrEmpty(rsPayment.Fields[i].Value.ToString()) ? 0 : Convert.ToInt16(rsPayment.Fields[i].Value);
                                        break;
                                    case ADODB.DataTypeEnum.adInteger:
                                        rsPaymentsVoucherUpdate.Fields[i].Value = string.IsNullOrEmpty(rsPayment.Fields[i].Value.ToString()) ? 0 : Convert.ToInt32(rsPayment.Fields[i].Value);
                                        break;
                                    case ADODB.DataTypeEnum.adBigInt:
                                        rsPaymentsVoucherUpdate.Fields[i].Value = string.IsNullOrEmpty(rsPayment.Fields[i].Value.ToString()) ? 0 : Convert.ToInt64(rsPayment.Fields[i].Value);
                                        break;
                                    default:
                                        RecordsetHelper.AssignRsString(rsPaymentsVoucherUpdate, rsPayment.Fields[i].Name, rsPayment.Fields[i].Value.ToString());
                                        break;
                                }
                            }
                        }
                        rsPayment.MoveNext();
                    }
                }

                //Recordset for payment save.
                if (ValidateSave(rsHeader, rsSegment, rsPassenger, rsMapping) == true)
                {
                    rsHeader.MoveFirst();
                    if (CheckInfantOverLimit(objInventory, rsSegment, Convert.ToInt32(rsHeader.Fields["number_of_infants"].Value)) == false)
                    {
                        DatasetToRecordset(GetAllocation(payment, mapping, fee, paymentFee, payment.Rows[0]["update_by"].ToString()), ref rsAllocation);

                        //Validate Duplidate seat.
                        ADODB.Recordset rsA = objReservation.VerifySeatAssignment(rsMapping, rsSegment);

                        if (rsA == null)
                        { }
                        else if (rsA.EOF == false)
                        {
                            bDupSeat = true;
                        }
                        ClearRecordset(ref rsA);

                        //Validate voucher
                        if (rsPayment.RecordCount > 0)
                        {
                            rsPayment.Filter = "form_of_payment_rcd = 'VOUCHER'";
                            if (rsPayment.RecordCount > 0)
                            {
                                while (!rsPayment.EOF && bVCValidateionPass)
                                {
                                    rsV = objMiscellaneous.GetVouchers("",
                                                                       rsPayment.Fields["document_number"].Value.ToString(),
                                                                       "",
                                                                       "",
                                                                       "",
                                                                       "",
                                                                       "",
                                                                       "",
                                                                       "",
                                                                       true,
                                                                       false,
                                                                       false,
                                                                       false,
                                                                       false,
                                                                       false,
                                                                       ref bWrite,
                                                                       ref dtFrom,
                                                                       ref dtTo,
                                                                       ref strAgencyCode);
                                    if (rsV != null && rsV.RecordCount > 0)
                                    {
                                        //1. Check voucher session.
                                        if (rsV.Fields["voucher_session_id"].Value != DBNull.Value)
                                        {
                                            bVCValidateionPass = false;
                                        }
                                        //2. Validate voucher
                                        if (bVCValidateionPass)
                                        {
                                            ADODB.Recordset rsVoucherAllocation = objPayment.ValidateVoucher(rsV, rsMapping, rsFees, string.Empty);
                                            if (rsVoucherAllocation == null)
                                            {
                                                bVCValidateionPass = false;
                                            }
                                            else
                                            {
                                                if (rsVoucherAllocation.RecordCount == 0)
                                                {
                                                    bVCValidateionPass = false;
                                                }
                                                else
                                                {
                                                    rsVoucherAllocation.Filter = "dif_amount > 0";
                                                    if (rsVoucherAllocation.RecordCount > 0)
                                                    {
                                                        bVCValidateionPass = false;
                                                    }
                                                    rsVoucherAllocation.Filter = 0;
                                                }
                                            }
                                            ClearRecordset(ref rsVoucherAllocation);
                                        }

                                        //3. Insert voucher session and sum total voucher amount.
                                        if (bVCValidateionPass)
                                        {
                                            if (objMiscellaneous.InsertVoucherSession(strVoucherSessionId, rsV.Fields["voucher_id"].Value.ToString(), header.Rows[0]["agency_code"].ToString(), string.Empty, strUserId))
                                            {
                                                if (!object.ReferenceEquals(rsV.Fields["voucher_value"].Value, System.DBNull.Value))
                                                {
                                                    dclVoucherValue = Convert.ToDecimal(rsV.Fields["voucher_value"].Value);
                                                }
                                                if (!object.ReferenceEquals(rsV.Fields["payment_total"].Value, System.DBNull.Value))
                                                {
                                                    dclPaymentTotal = Convert.ToDecimal(rsV.Fields["payment_total"].Value);
                                                }
                                                dclVoucherBalance = dclVoucherBalance + (dclVoucherValue - dclPaymentTotal);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        //no voucher found
                                        bVCValidateionPass = false;
                                    }

                                    dclPaymentAmount = dclPaymentAmount + Convert.ToDecimal(rsPayment.Fields["payment_amount"].Value.ToString());
                                    dclVoucherValue = 0;
                                    dclPaymentTotal = 0;
                                   // ClearRecordset(ref rsV);
                                    rsPayment.MoveNext();
                                }

                                //4. Check if the voucher balance is enough
                                if (bVCValidateionPass)
                                {
                                    if (dclPaymentAmount > dclVoucherBalance)
                                    {
                                        bVCValidateionPass = false;
                                    }
                                }

                                //Check Credit Agency
                                if (bVCValidateionPass)
                                {
                                    rsPayment.Filter = "form_of_payment_rcd = 'INV' OR form_of_payment_rcd = 'CRAGT'";
                                    if (rsPayment.RecordCount > 0)
                                    {
                                        //Read agency information for validate.
                                        raP = objComSystem.GetAgencySessionProfile(strAgencyCode, string.Empty);
                                        if (raP != null)
                                        {
                                            if (raP.RecordCount > 0)
                                            {
                                                dclAgencyAccountBalance = Convert.ToDecimal(raP.Fields["agency_acount"].Value.ToString()) - Convert.ToDecimal(raP.Fields["booking_payment"].Value.ToString());
                                            }
                                            ClearRecordset(ref raP);
                                        }

                                        //Check account balance
                                        while (!rsPayment.EOF)
                                        {
                                            if (dclAgencyAccountBalance < Convert.ToDecimal(rsPayment.Fields["payment_amount"].Value.ToString()))
                                            {
                                                bVCValidateionPass = false;
                                            }
                                            rsPayment.MoveNext();
                                        }
                                    }
                                }
                            }
                        }


                        //Clear COM object.
                        if (objReservation != null)
                        {
                            Marshal.FinalReleaseComObject(objReservation);
                            objReservation = null;
                        }
                        //Validate Session Lock.
                        bFoundSessionLock = objInventory.VerifyFlightInventorySession(rsSegment);
                        if (objInventory != null)
                        {
                            Marshal.FinalReleaseComObject(objInventory);
                            objInventory = null;
                        }

                        if (bFoundSessionLock == false)
                        {
                            //Session timeout
                            objHelper.CreateErrorDataset(ref ds, "202", "SESSIONTIMEOUT", string.Empty, "Payments");
                        }
                        else if (bDupSeat == true)
                        {
                            //Duplicate seat
                            objHelper.CreateErrorDataset(ref ds, "203", "DUPLICATESEAT", string.Empty, "Payments");
                        }
                        else if (bVCValidateionPass == false)
                        {
                            //Invalid voucher
                            objHelper.CreateErrorDataset(ref ds, "207", "INVALID VOUCHER", string.Empty, "Payments");
                        }
                        else
                        {
                            //Get booking Id from booking header using in loggin.
                            if (rsHeader != null && rsHeader.RecordCount > 0)
                            {
                                strBookingId = rsHeader.Fields["booking_id"].Value.ToString();
                            }

                            //Get record locator
                            if (string.IsNullOrEmpty(strBookingReference))
                            {
                                if (rsHeader.Fields["record_locator"].Value is System.DBNull || rsHeader.Fields["record_locator"].Value.ToString().Length == 0)
                                {
                                    objBooking.GetRecordLocator(ref strBookingReference, ref iBookingNumber);
                                }
                                else
                                {
                                    strBookingReference = rsHeader.Fields["record_locator"].Value.ToString();
                                    iBookingNumber = Convert.ToInt32(rsHeader.Fields["booking_number"].Value);
                                }
                                //Assign value to booking header
                                if (string.IsNullOrEmpty(strBookingReference) == false && iBookingNumber > 0)
                                {
                                    rsHeader.Fields["booking_number"].Value = iBookingNumber;
                                    rsHeader.Fields["record_locator"].Value = strBookingReference;
                                }
                            }

                            //CC payment
                            if (rsPaymentsCCUpdate != null && rsPaymentsCCUpdate.RecordCount > 0)
                            {
                                //Call to validate credit card payment.
                                rsC = objCreditCard.Authorize(rsPaymentsCCUpdate,
                                                            rsSegment,
                                                            ref rsAllocation,
                                                            ref rsVoucher,
                                                            ref rsMapping,
                                                            ref rsFees,
                                                            ref rsHeader,
                                                            ref rsTax,
                                                            ref securityToken,
                                                            ref authenticationToken,
                                                            ref commerceIndicator,
                                                            ref strBookingReference,
                                                            ref requestSource,
                                                            ref bWeb);

                                ds = RecordsetToDataset(rsC, "Payments");

                                if (ds != null && ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows[0]["ResponseCode"].ToString() == "APPROVED")
                                    {
                                        bCCvalidationPass = true;
                                    }
                                    else
                                    {
                                        bCCvalidationPass = false;
                                    }
                                }
                                else
                                {
                                    bCCvalidationPass = false;
                                }
                            }

                            if (bCCvalidationPass)
                            {
                                if (rsPaymentsVoucherUpdate.RecordCount > 0)
                                {
                                    DataSet dss = new DataSet();
                                    dss = RecordsetToDataset(rsPaymentsVoucherUpdate, "rsPaymentsVoucherUpdate");
                                    DataSet dsr = new DataSet();
                                    dsr = RecordsetToDataset(rsAllocation, "rsAllocation");
                                    //Voucher payment

                                    bVCValidateionPass = objPayment.Save(ref rsPaymentsVoucherUpdate, ref rsAllocation, null);
                                }

                                if (bCCvalidationPass && bVCValidateionPass)
                                {
                                    enumSaveError = objBooking.Save(ref rsHeader,
                                                                    ref rsSegment,
                                                                    ref rsPassenger,
                                                                    ref rsRemark,
                                                                    null,
                                                                    ref rsMapping,
                                                                    ref rsService,
                                                                    ref rsTax,
                                                                    ref rsFees,
                                                                    ref createTickets,
                                                                    false,
                                                                    false,
                                                                    true,
                                                                    false,
                                                                    false);

                                    if (enumSaveError != tikAeroProcess.BookingSaveError.OK)
                                    {
                                        if (ds != null)
                                        { ds.Clear(); }
                                        if (enumSaveError != tikAeroProcess.BookingSaveError.SESSIONTIMEOUT)
                                        {
                                            //Sent Error log Email
                                            Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" +
                                                                                                                                                  "Error Code : " + Convert.ToInt16(enumSaveError).ToString() + "<br/>",
                                                                                                                                                  "SESSIONTIMEOUT",
                                                                                                                                                  "",
                                                                                                                                                  "SaveBookingMultipleFormOfPayment-Voucher",
                                                                                                                                                  "TikAeroWebMain-ComplusBooking",
                                                                                                                                                  "");
                                            objHelper.CreateErrorDataset(ref ds, "202", "SESSIONTIMEOUT", string.Empty, "Payments");
                                        }
                                        else if (enumSaveError != tikAeroProcess.BookingSaveError.DUPLICATESEAT)
                                        {
                                            //Sent Error log Email
                                            Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" +
                                                                                                                                                  "Error Code : " + Convert.ToInt16(enumSaveError).ToString() + "<br/>",
                                                                                                                                                  "DUPLICATESEAT",
                                                                                                                                                  "",
                                                                                                                                                  "SaveBookingMultipleFormOfPayment-Voucher",
                                                                                                                                                  "TikAeroWebMain-ComplusBooking",
                                                                                                                                                  "");
                                            objHelper.CreateErrorDataset(ref ds, "203", "DUPLICATESEAT", string.Empty, "Payments");
                                        }
                                        else if (enumSaveError != tikAeroProcess.BookingSaveError.DATABASEACCESS)
                                        {
                                            //Sent Error log Email
                                            Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" +
                                                                                                                                                  "Error Code : " + Convert.ToInt16(enumSaveError).ToString() + "<br/>",
                                                                                                                                                  "DATABASEACCESS",
                                                                                                                                                  "",
                                                                                                                                                  "SaveBookingMultipleFormOfPayment-Voucher",
                                                                                                                                                  "TikAeroWebMain-ComplusBooking",
                                                                                                                                                  "");
                                            objHelper.CreateErrorDataset(ref ds, "204", "DATABASEACCESS", string.Empty, "Payments");
                                        }
                                        else if (enumSaveError != tikAeroProcess.BookingSaveError.BOOKINGINUSE)
                                        {
                                            //Sent Error log Email
                                            Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" +
                                                                                                                                                  "Error Code : " + Convert.ToInt16(enumSaveError).ToString() + "<br/>",
                                                                                                                                                  "BOOKINGINUSE",
                                                                                                                                                  "",
                                                                                                                                                  "SaveBookingMultipleFormOfPayment-Voucher",
                                                                                                                                                  "TikAeroWebMain-ComplusBooking",
                                                                                                                                                  "");
                                            objHelper.CreateErrorDataset(ref ds, "205", "BOOKINGINUSE", string.Empty, "Payments");
                                        }
                                        else if (enumSaveError != tikAeroProcess.BookingSaveError.BOOKINGREADERROR)
                                        {
                                            //Sent Error log Email
                                            Avantik.Web.Service.Helpers.Logger.Instance(Avantik.Web.Service.Helpers.Logger.LogType.Mail).WriteLog("Booking Id : " + strBookingId + "<br/>" +
                                                                                                                                                  "Error Code : " + Convert.ToInt16(enumSaveError).ToString() + "<br/>",
                                                                                                                                                  "BOOKINGREADERROR",
                                                                                                                                                  "",
                                                                                                                                                  "SaveBookingMultipleFormOfPayment-Voucher",
                                                                                                                                                  "TikAeroWebMain-ComplusBooking",
                                                                                                                                                  "");
                                            objHelper.CreateErrorDataset(ref ds, "206", "BOOKINGREADERROR", string.Empty, "Payments");
                                        }


                                    }
                                    else // save success
                                    {
                                        objHelper.CreateErrorDataset(ref ds, "000", "APPROVED", string.Empty, "Payments");
                                    }
                                }

                            }
                            else
                            {
                                if (rsPaymentsCCUpdate.RecordCount > 0)
                                {
                                    RecordsetToDataset(ds, rsPaymentsCCUpdate, "Payments");
                                    if (strBookingReference.Length > 0)
                                    {
                                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            DataColumn dcRecordLocator = new DataColumn("record_locator");
                                            ds.Tables[0].Columns.Add(dcRecordLocator);
                                            ds.Tables[0].Rows[0]["record_locator"] = strBookingReference;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        objHelper.CreateErrorDataset(ref ds, "200", "Infant over limit", string.Empty, "Payments");
                    }
                }
            }
            catch
            {
                ds.Clear();
                throw;
            }
            finally
            {
                // clear voucher session
                if (rsV != null && rsV.RecordCount > 0)
                {
                    objMiscellaneous.ReleaseVoucherSession(strVoucherSessionId, rsV.Fields["voucher_id"].Value.ToString());
                }

                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }
                if (objPayment != null)
                {
                    Marshal.FinalReleaseComObject(objPayment);
                    objPayment = null;
                }
                if (objInventory != null)
                {
                    Marshal.FinalReleaseComObject(objInventory);
                    objInventory = null;
                }
                if (objReservation != null)
                {
                    Marshal.FinalReleaseComObject(objReservation);
                    objReservation = null;
                }
                if (objCreditCard != null)
                {
                    Marshal.FinalReleaseComObject(objCreditCard);
                    objCreditCard = null;
                }
                if (objMiscellaneous != null)
                {
                    Marshal.FinalReleaseComObject(objMiscellaneous);
                    objCreditCard = null;
                }
                if (objComSystem != null)
                {
                    Marshal.FinalReleaseComObject(objComSystem);
                    objCreditCard = null;
                }

                ClearRecordset(ref rsHeader);
                ClearRecordset(ref rsSegment);
                ClearRecordset(ref rsPassenger);
                ClearRecordset(ref rsRemark);
                ClearRecordset(ref rsPayment);
                ClearRecordset(ref rsMapping);
                ClearRecordset(ref rsService);
                ClearRecordset(ref rsTax);
                ClearRecordset(ref rsFees);
                ClearRecordset(ref rsFlight);
                ClearRecordset(ref rsQuote);
                ClearRecordset(ref rsAllocation);
                ClearRecordset(ref rsVoucher);
                ClearRecordset(ref rsPaymentsVoucherUpdate);
                ClearRecordset(ref rsPaymentsCCUpdate);
                ClearRecordset(ref rsV);
                ClearRecordset(ref raP);
                ClearRecordset(ref rsC);
            }

            return ds;
        }

        public DataSet GetBooking(string bookingId,
                               string bookingReference,
                               double bookingNumber,
                               bool bReadonly,
                               bool bSeatLock,
                               string userId,
                               bool bReadHeader,
                               bool bReadSegment,
                               bool bReadPassenger,
                               bool bReadRemark,
                               bool bReadPayment,
                               bool bReadMapping,
                               bool bReadService,
                               bool bReadTax,
                               bool bReadFee,
                               bool bReadOd,
                               string releaseBookingId,
                               string CompleteRemarkId)
        {
            tikAeroProcess.Booking objBooking = null;

            ADODB.Recordset rsHeader = null;
            ADODB.Recordset rsSegment = null;
            ADODB.Recordset rsPassenger = null;
            ADODB.Recordset rsRemark = null;
            ADODB.Recordset rsPayment = null;
            ADODB.Recordset rsMapping = null;
            ADODB.Recordset rsService = null;
            ADODB.Recordset rsTax = null;
            ADODB.Recordset rsFees = null;
            ADODB.Recordset rsQuote = null;


            DataSet ds = new DataSet();
            ds.DataSetName = "Booking";
            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Booking(); }

                if (objBooking.Read(bookingId,
                                    bookingReference,
                                    bookingNumber,
                                    ref rsHeader,
                                    ref rsSegment,
                                    ref rsPassenger,
                                    ref rsRemark,
                                    ref rsPayment,
                                    ref rsMapping,
                                    ref rsService,
                                    ref rsTax,
                                    ref rsFees,
                                    ref rsQuote,
                                    ref bReadonly,
                                    ref bSeatLock,
                                    ref userId,
                                    ref bReadHeader,
                                    ref bReadSegment,
                                    ref bReadPassenger,
                                    ref bReadRemark,
                                    ref bReadPayment,
                                    ref bReadMapping,
                                    ref bReadService,
                                    ref bReadTax,
                                    ref bReadFee,
                                    ref bReadOd,
                                    ref releaseBookingId,
                                    ref CompleteRemarkId) == true)
                {
                    if (bReadHeader == true)
                    {
                        RecordsetToDataset(ds, rsHeader, "BookingHeader");
                    }
                    if (bReadSegment == true)
                    {
                        RecordsetToDataset(ds, rsSegment, "FlightSegment");
                    }
                    if (bReadPassenger == true)
                    {
                        RecordsetToDataset(ds, rsPassenger, "Passenger");
                    }
                    if (bReadRemark == true)
                    {
                        RecordsetToDataset(ds, rsRemark, "Remark");
                    }
                    if (bReadPayment == true)
                    {
                        RecordsetToDataset(ds, rsPayment, "Payment");
                    }
                    if (bReadMapping == true)
                    {
                        RecordsetToDataset(ds, rsMapping, "Mapping");
                    }
                    if (bReadService == true)
                    {
                        RecordsetToDataset(ds, rsService, "Service");
                    }
                    if (bReadTax == true)
                    {
                        RecordsetToDataset(ds, rsTax, "Tax");
                    }
                    if (bReadFee == true)
                    {
                        RecordsetToDataset(ds, rsFees, "Fee");
                    }
                    if (bReadOd == true)
                    {
                        RecordsetToDataset(ds, rsQuote, "Quote");
                    }
                }
            }
            catch
            { }
            finally
            {
                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }
                ClearRecordset(ref rsHeader);
                ClearRecordset(ref rsSegment);
                ClearRecordset(ref rsPassenger);
                ClearRecordset(ref rsRemark);
                ClearRecordset(ref rsPayment);
                ClearRecordset(ref rsMapping);
                ClearRecordset(ref rsService);
                ClearRecordset(ref rsTax);
                ClearRecordset(ref rsFees);
                ClearRecordset(ref rsQuote);
            }

            return ds;
        }

        public void UpdatePassengerNames(DataTable passenger,
                                        DataTable mapping,
                                        DataTable header,
                                        DataTable fees,
                                        DataTable segment,
                                        string agency,
                                        string currency,
                                        string bookingID,
                                        bool allowNameChange)
        {
            tikAeroProcess.Booking objBooking = null;

            ADODB.Recordset rsHeader = DatasetToRecordset(header);
            ADODB.Recordset rsSegment = DatasetToRecordset(segment);
            ADODB.Recordset rsPassenger = DatasetToRecordset(passenger);
            ADODB.Recordset rsMapping = DatasetToRecordset(mapping);
            ADODB.Recordset rsFees = DatasetToRecordset(fees);

            DataSet ds = null;
            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Booking(); }

                objBooking.UpdatePassengerNames(ref rsPassenger,
                                                ref rsMapping,
                                                ref rsHeader,
                                                ref rsFees,
                                                ref rsSegment,
                                                ref agency,
                                                ref currency,
                                                ref bookingID,
                                                ref allowNameChange);

                ds = RecordsetToDataset(rsHeader, "header");
                header = ds.Tables[0];

                ds = RecordsetToDataset(rsSegment, "segment");
                segment = ds.Tables[0];

                ds = RecordsetToDataset(rsPassenger, "passenger");
                passenger = ds.Tables[0];

                ds = RecordsetToDataset(rsMapping, "mapping");
                mapping = ds.Tables[0];

                ds = RecordsetToDataset(rsFees, "fees");
                fees = ds.Tables[0];

            }
            catch
            { }
            finally
            {
                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }
                ClearRecordset(ref rsHeader);
                ClearRecordset(ref rsSegment);
                ClearRecordset(ref rsPassenger);
                ClearRecordset(ref rsMapping);
                ClearRecordset(ref rsFees);
            }
        }
        public bool TicketCreate(string agencyCode, string userID, string bookingID, DataTable dt, DataTable ticket, bool readTicket)
        {

            tikAeroProcess.Booking objBooking = null;

            ADODB.Recordset rs = DatasetToRecordset(dt);
            ADODB.Recordset rsTicket = DatasetToRecordset(ticket);

            bool result = false;
            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Booking(); }

                result = objBooking.TicketCreate(ref agencyCode, ref userID, ref bookingID, ref rs, ref rsTicket, ref readTicket);
            }
            catch
            { }
            finally
            {
                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }
                ClearRecordset(ref rs);
                ClearRecordset(ref rsTicket);
            }

            return result;
        }
        public bool AddChangeFlight(DataTable passengers,
                                    DataTable segments,
                                    DataTable mappings,
                                    DataTable taxes,
                                    DataTable services,
                                    string agencyCode,
                                    DataTable quotes,
                                    DataTable remarks,
                                    DataTable flights,
                                    DateTime flightDate,
                                    string bookingID,
                                    string boardpoint,
                                    string Offpoint,
                                    string flightId,
                                    string fareId,
                                    string airline,
                                    string flight,
                                    string boardingClass,
                                    string bookingClass,
                                    string language,
                                    string currencyCode,
                                    bool eTicket,
                                    bool refundable,
                                    bool groupBooking,
                                    bool waitlist,
                                    bool quoteOnly,
                                    bool nonRevenue,
                                    bool subjectToAvaliability,
                                    string marketingAirline,
                                    string marketingFlight,
                                    string segmentId,
                                    short idReduction,
                                    string userId,
                                    bool excludeQuote,
                                    bool overbook)
        {
            tikAeroProcess.Booking objBooking = null;

            ADODB.Recordset rsPassengers = DatasetToRecordset(passengers);
            ADODB.Recordset rsSegments = DatasetToRecordset(segments);
            ADODB.Recordset rsMappings = DatasetToRecordset(mappings);
            ADODB.Recordset rsTaxes = DatasetToRecordset(taxes);
            ADODB.Recordset rsServices = DatasetToRecordset(services);
            ADODB.Recordset rsQuotes = DatasetToRecordset(quotes);
            ADODB.Recordset rsRemarks = DatasetToRecordset(remarks);
            ADODB.Recordset rsFlights = DatasetToRecordset(flights);


            DataSet ds = null;
            bool result = false;
            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Booking(); }

                result = objBooking.AddFlight(ref rsPassengers,
                                            ref rsSegments,
                                            ref rsMappings,
                                            ref rsTaxes,
                                            ref rsServices,
                                            agencyCode,
                                            ref rsQuotes,
                                            ref rsRemarks,
                                            ref rsFlights,
                                            ref flightDate,
                                            ref bookingID,
                                            ref boardpoint,
                                            ref Offpoint,
                                            ref flightId,
                                            ref fareId,
                                            ref airline,
                                            ref flight,
                                            ref boardingClass,
                                            ref bookingClass,
                                            ref language,
                                            ref currencyCode,
                                            ref eTicket,
                                            ref refundable,
                                            ref groupBooking,
                                            ref waitlist,
                                            ref quoteOnly,
                                            ref nonRevenue,
                                            ref subjectToAvaliability,
                                            ref marketingAirline,
                                            ref marketingFlight,
                                            ref segmentId,
                                            ref idReduction,
                                            ref userId,
                                            ref excludeQuote,
                                            ref overbook);

                ds = RecordsetToDataset(rsPassengers, "passengers");
                passengers = ds.Tables[0];

                ds = RecordsetToDataset(rsSegments, "segments");
                segments = ds.Tables[0];

                ds = RecordsetToDataset(rsMappings, "mappings");
                mappings = ds.Tables[0];

                ds = RecordsetToDataset(rsTaxes, "taxes");
                taxes = ds.Tables[0];

                ds = RecordsetToDataset(rsServices, "services");
                services = ds.Tables[0];

                ds = RecordsetToDataset(rsQuotes, "quotes");
                quotes = ds.Tables[0];

                ds = RecordsetToDataset(rsRemarks, "remarks");
                remarks = ds.Tables[0];

                ds = RecordsetToDataset(rsFlights, "flights");
                flights = ds.Tables[0];

            }
            catch
            { }
            finally
            {
                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }
                ClearRecordset(ref rsPassengers);
                ClearRecordset(ref rsSegments);
                ClearRecordset(ref rsMappings);
                ClearRecordset(ref rsTaxes);
                ClearRecordset(ref rsServices);
                ClearRecordset(ref rsQuotes);
                ClearRecordset(ref rsRemarks);
                ClearRecordset(ref rsFlights);

            }

            return result;
        }
        public bool SegmentCancel(string segmentId,
                                DataTable segments,
                                DataTable mappings,
                                DataTable services,
                                DataTable payments,
                                DataTable taxes,
                                DataTable quotes,
                                string userID,
                                string agencyCode,
                                bool waiveFee,
                                bool processTicketRefund,
                                bool includeRefundableOnly,
                                DateTime noShowSegmentDateTime,
                                bool bVoid)
        {
            tikAeroProcess.Booking objBooking = null;

            ADODB.Recordset rsSegment = DatasetToRecordset(segments);
            ADODB.Recordset rsMapping = DatasetToRecordset(mappings);
            ADODB.Recordset rsService = DatasetToRecordset(services);
            ADODB.Recordset rsPayment = DatasetToRecordset(payments);
            ADODB.Recordset rsQuotes = DatasetToRecordset(quotes);
            ADODB.Recordset rsTax = DatasetToRecordset(taxes);

            DataSet ds = null;
            bool result = false;
            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Booking(); }

                result = objBooking.SegmentCancel(segmentId,
                                                ref rsSegment,
                                                ref rsMapping,
                                                ref rsService,
                                                ref rsPayment,
                                                ref rsTax,
                                                ref rsQuotes,
                                                ref userID,
                                                ref agencyCode,
                                                ref waiveFee,
                                                ref processTicketRefund,
                                                ref includeRefundableOnly,
                                                ref noShowSegmentDateTime,
                                                ref bVoid);

                ds = RecordsetToDataset(rsSegment, "segment");
                segments = ds.Tables[0];

                ds = RecordsetToDataset(rsMapping, "mapping");
                mappings = ds.Tables[0];

                ds = RecordsetToDataset(rsService, "service");
                services = ds.Tables[0];

                ds = RecordsetToDataset(rsPayment, "payment");
                payments = ds.Tables[0];

                ds = RecordsetToDataset(rsQuotes, "quotes");
                quotes = ds.Tables[0];

                ds = RecordsetToDataset(rsTax, "tax");
                taxes = ds.Tables[0];

            }
            catch
            { }
            finally
            {
                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }
                ClearRecordset(ref rsSegment);
                ClearRecordset(ref rsPayment);
                ClearRecordset(ref rsMapping);
                ClearRecordset(ref rsService);
                ClearRecordset(ref rsTax);
                ClearRecordset(ref rsQuotes);
            }

            return result;
        }
        public bool AddInfant(DataTable passenger,
                              DataTable segment,
                              DataTable mapping,
                              DataTable tax,
                              DataTable service,
                              DataTable quote,
                              string bookingId,
                              string agencyCode,
                              string passengerType,
                              string language,
                              bool groupBooking)
        {
            tikAeroProcess.Booking objBooking = null;
            bool bResult = false;

            //Convert Datatable to Recordset
            ADODB.Recordset rsPassenger = DatasetToRecordset(passenger);
            ADODB.Recordset rsSegment = DatasetToRecordset(segment);
            ADODB.Recordset rsMapping = DatasetToRecordset(mapping);
            ADODB.Recordset rsTax = DatasetToRecordset(tax);
            ADODB.Recordset rsService = DatasetToRecordset(service);
            ADODB.Recordset rsQuote = DatasetToRecordset(quote);

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Booking(); }

                if (objBooking != null)
                {
                    //set default language to EN
                    if (language.Length == 0)
                    { language = "EN"; }
                    if (passengerType.Length == 0)
                    { passengerType = "INF"; }

                    //Call Complus function
                    bResult = objBooking.AddInfant(ref rsPassenger,
                                                    ref rsSegment,
                                                    ref rsMapping,
                                                    ref rsTax,
                                                    ref rsService,
                                                    bookingId,
                                                    agencyCode,
                                                    ref rsQuote,
                                                    ref passengerType,
                                                    ref language,
                                                    ref groupBooking);

                    //Convert Recordset to Datatable and Release com object

                    passenger = RecordsetToDataset(rsPassenger, "Passenger").Tables[0];
                    segment = RecordsetToDataset(rsSegment, "Segment").Tables[0];
                    mapping = RecordsetToDataset(rsMapping, "Mapping").Tables[0];
                    tax = RecordsetToDataset(rsTax, "Tax").Tables[0];
                    service = RecordsetToDataset(rsService, "Service").Tables[0];
                    quote = RecordsetToDataset(rsQuote, "quote").Tables[0];

                }
                objBooking = null;

            }
            catch
            { bResult = false; }
            finally
            {
                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }
                ClearRecordset(ref rsPassenger);
                ClearRecordset(ref rsSegment);
                ClearRecordset(ref rsMapping);
                ClearRecordset(ref rsTax);
                ClearRecordset(ref rsService);
                ClearRecordset(ref rsQuote);
            }


            return bResult;
        }
        public DataSet TicketRead(string bookingId,
                                   string passengerId,
                                   string segmentId,
                                   string ticketNumber,
                                   DataTable tax,
                                   bool readOnly,
                                   bool returnTax)
        {
            tikAeroProcess.Booking objBooking = null;
            ADODB.Recordset rs = null;
            ADODB.Recordset rsTax = DatasetToRecordset(tax);

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Booking(); }

                rs = objBooking.TicketRead(ref bookingId,
                                            ref passengerId,
                                            ref segmentId,
                                            ref ticketNumber,
                                            ref rsTax,
                                            ref readOnly,
                                            ref returnTax);

                ds = RecordsetToDataset(rs, "Ticket");
            }
            catch
            { }
            finally
            {
                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }

                ClearRecordset(ref rs);
                ClearRecordset(ref rsTax);
            }
            return ds;
        }
        public string SplitBooking(DataTable splitPassenger,
                                string bookingIdOld,
                                string receivedFrom,
                                string userId,
                                string agencyCode,
                                string userCode,
                                bool copyRemark)
        {
            tikAeroProcess.Booking objBooking = null;
            ADODB.Recordset rsSplitPassenger = DatasetToRecordset(splitPassenger);
            string result = string.Empty;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Booking(); }

                result = objBooking.Split(ref rsSplitPassenger,
                                            ref bookingIdOld,
                                            ref receivedFrom,
                                            ref userId,
                                            ref agencyCode,
                                            ref userCode,
                                            ref copyRemark);
            }
            catch
            { }
            finally
            {
                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }

                ClearRecordset(ref rsSplitPassenger);
            }
            return result;
        }

        #region Remark function
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
                                 DateTime timelimitUTC)
        {
            tikAeroProcess.Booking objBooking = null;
            string result = string.Empty;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Booking(); }

                result = objBooking.RemarkAdd(remarkType,
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }
            }
            return result;
        }

        public bool RemarkDelete(Guid bookingRemarkId)
        {
            tikAeroProcess.Booking objBooking = null;
            bool bResult = false;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Booking(); }

                if (bookingRemarkId.Equals(Guid.Empty) == false)
                {
                    bResult = objBooking.RemarkDelete(bookingRemarkId.ToString());
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }
            }
            return bResult;
        }

        public bool RemarkComplete(Guid bookingRemarkId, Guid userId)
        {
            tikAeroProcess.Booking objBooking = null;
            bool bResult = false;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Booking(); }

                if (bookingRemarkId.Equals(Guid.Empty) == false)
                {
                    string strUserId = string.Empty;
                    if (userId.Equals(Guid.Empty) == false)
                    {
                        strUserId = userId.ToString();
                    }
                    bResult = objBooking.RemarkComplete(bookingRemarkId.ToString(), strUserId);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }
            }
            return bResult;
        }

        public DataSet RemarkRead(string remarkId, string bookingId, string bookingReference, double bookingNumber, bool readOnly)
        {
            tikAeroProcess.Booking objBooking = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Booking(); }

                rs = objBooking.RemarkRead(remarkId, bookingId, bookingReference, bookingNumber, readOnly);
                if (rs != null && rs.RecordCount > 0)
                {
                    ds = RecordsetToDataset(rs, "Remarks");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }
                ClearRecordset(ref rs);
            }
            return ds;
        }

        public bool RemarkSave(DataTable remark)
        {
            tikAeroProcess.Booking objBooking = null;
            //ADODB.Recordset rsRemark = FabricateRemarkRecordset();
            ADODB.Recordset rsRemark = null;

            bool bResult = false;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Booking(); }
                string bookId = remark.Rows[0]["booking_id"].ToString();
                rsRemark = objBooking.RemarkRead("", bookId, "", double.MinValue, false);
                foreach (DataRow dr in remark.Rows)
                {
                    Guid bookRemarkIdDT = new Guid( dr["booking_remark_id"].ToString());
                  
                    rsRemark.MoveFirst();
                    while (!rsRemark.EOF)
                    {
                        Guid bookRemarkIdRS = new Guid(rsRemark.Fields["booking_remark_id"].Value.ToString());

                        if (bookRemarkIdRS == bookRemarkIdDT)
                        {
                            rsRemark.Fields["remark_type_rcd"].Value = dr["remark_type_rcd"];
                            rsRemark.Fields["remark_text"].Value = dr["remark_text"];
                            rsRemark.Fields["nickname"].Value = dr["nickname"];
                            rsRemark.Fields["protected_flag"].Value = dr["protected_flag"];
                            rsRemark.Fields["warning_flag"].Value = dr["warning_flag"];
                            rsRemark.Fields["process_message_flag"].Value = dr["process_message_flag"];
                            rsRemark.Fields["update_date_time"].Value = Convert.ToDateTime( dr["update_date_time"]);
                            rsRemark.Fields["update_by"].Value = "{"+dr["update_by"].ToString()+"}";
                           
                        }

                        rsRemark.MoveNext();
                    }
                }

               
                
               
                
                 
                //Convert datatable to recordset.
                //DatasetToRecordset(remark, ref rsRemark);

                if (rsRemark != null && rsRemark.RecordCount > 0)
                {
                    bResult = objBooking.RemarkSave(rsRemark);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }

                if (rsRemark != null)
                {
                    //Clear Recordset
                    ClearRecordset(ref rsRemark);
                }
            }

            return bResult;
        }
        #endregion
 
    
        #region Helper

        private void SetMMToMapping(ref ADODB.Recordset rsMapping, ref ADODB.Recordset rsSegment)
        {
            rsSegment.MoveFirst();
            while (!rsSegment.EOF)
            {
                rsMapping.MoveFirst();
                while (!rsMapping.EOF)
                {
                    if (RecordsetHelper.ToString(rsSegment, "segment_status_rcd") == "MM" && RecordsetHelper.ToGuid(rsSegment, "booking_segment_id") == RecordsetHelper.ToGuid(rsMapping, "booking_segment_id"))
                    {
                        if(RecordsetHelper.ToString(rsMapping, "passenger_status_rcd") != "MM")
                        	rsMapping.Fields["passenger_status_rcd"].Value = "MM";
                    }

                    rsMapping.MoveNext();
                }

                rsSegment.MoveNext();
            }

        }

        private void SetRecordToMapping(ref ADODB.Recordset rsMapping, string recordLocator)
        {
                rsMapping.MoveFirst();
                while (!rsMapping.EOF)
                {
                   rsMapping.Fields["record_locator"].Value = recordLocator;
                   rsMapping.MoveNext();
                }
        }


        private void SetServiceToRQ(ref ADODB.Recordset rsMapping, ref ADODB.Recordset rsService)
        {
            rsMapping.MoveFirst();
            while (!rsMapping.EOF)
            {
                rsService.MoveFirst();
                while (!rsService.EOF)
                {
                    if (RecordsetHelper.ToString(rsMapping, "passenger_status_rcd") == "MM" && RecordsetHelper.ToGuid(rsMapping, "booking_segment_id") == RecordsetHelper.ToGuid(rsService, "booking_segment_id"))
                    {
                        rsService.Fields["special_service_status_rcd"].Value = "RQ";
                    }

                    rsService.MoveNext();
                }

                rsMapping.MoveNext();
            }
        }


        





        private bool CheckInfantOverLimit(tikAeroProcess.Inventory objInventory, ADODB.Recordset rsFlights, int numberOfInfant)
        {
            ADODB.Recordset rsInfantLimit = null;
            string flightId;
            string originRcd;
            string destinationRcd;
            string boardingClass;

            try
            {
                if (objInventory != null)
                {
                    if (rsFlights != null)
                    {
                        rsFlights.Filter = "segment_status_rcd = 'NN' or segment_status_rcd = null";
                        while (!rsFlights.EOF)
                        {
                            if (rsFlights.Fields["boarding_class_rcd"].Value == System.DBNull.Value)
                            {
                                rsFlights.Fields["boarding_class_rcd"].Value = "";
                            }
                            //Get infant limitation information.
                            flightId = rsFlights.Fields["flight_id"].Value.ToString();
                            originRcd = rsFlights.Fields["origin_rcd"].Value.ToString();
                            destinationRcd = rsFlights.Fields["destination_rcd"].Value.ToString();
                            boardingClass = rsFlights.Fields["boarding_class_rcd"].Value.ToString();

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
                            rsFlights.MoveNext();
                        }
                        rsFlights.Filter = 0;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                ClearRecordset(ref rsInfantLimit);
            }

            return false;
        }
        #endregion
    }
}