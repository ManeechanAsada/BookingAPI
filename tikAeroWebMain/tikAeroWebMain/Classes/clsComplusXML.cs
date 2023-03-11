using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Data;

namespace tikAeroWebMain
{
    public class ComplusXML : RunComplus
    {
        public ComplusXML() : base() { }

        public string GetItinerary(Guid bookingId, string language, string passengerId, string agencyCode)
        {
            tikAeroProcess.XML objXml = null;
            string result = string.Empty;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.XML", _server);
                    objXml = (tikAeroProcess.XML)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objXml = new tikAeroProcess.XML(); };

                if (language.Length == 0)
                { language = "EN"; }
                result = objXml.GetItinerary(bookingId.ToString(), language, ref passengerId, ref agencyCode);
            }
            catch
            { }
            finally
            {
                if (objXml != null)
                { Marshal.FinalReleaseComObject(objXml); }
                objXml = null;
            }

            return result;
        }
        public string GetItinerary(string strRecordlocator, string language, string passengerId, string agencyCode)
        {
            tikAeroProcess.Booking objBooking = null;

            string result = string.Empty;
            string userId = string.Empty;
            string releaseBookingId = string.Empty;
            string CompleteRemarkId = string.Empty;

            bool bReadonly = false;
            bool bSeatLock = false;
            bool bReadHeader = true;
            bool bReadSegment = false;
            bool bReadPassenger = false;
            bool bReadRemark = false;
            bool bReadPayment = false;
            bool bReadMapping = false;
            bool bReadService = false;
            bool bReadTax = false;
            bool bReadFee = false;
            bool bReadOd = false;

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

                if (objBooking.Read(string.Empty,
                                    strRecordlocator,
                                    0,
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
                    if (rsHeader != null)
                    {
                        if (rsHeader.RecordCount > 0)
                        {
                            result = GetItinerary(new Guid(rsHeader.Fields["booking_id"].Value.ToString()), 
                                                language, 
                                                passengerId, 
                                                agencyCode);
                        }
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

            return result;
        }
    }
}