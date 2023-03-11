using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Data;

namespace tikAeroWebMain
{
    public class ComplusCheckin : RunComplus
    {
        public ComplusCheckin() : base() { }

        public DataSet GetSeatMap(string origin, string destination, string flightId, string boardingClass, string bookingClass)
        {
            tikAeroProcess.CheckIn objCheckin = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.CheckIn", _server);
                    objCheckin = (tikAeroProcess.CheckIn)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objCheckin = new tikAeroProcess.CheckIn(); }

                rs = objCheckin.GetSeatMap(ref origin, ref destination, ref flightId, ref boardingClass, ref bookingClass);
                ds = RecordsetToDataset(rs, "Seat");
            }
            catch
            { }
            finally
            {
                if (objCheckin != null)
                { Marshal.FinalReleaseComObject(objCheckin); }
                objCheckin = null;

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet GetBookingSegmentCheckIn(string bookingId, string clientId, string language)
        {
            tikAeroProcess.CheckIn objCheckin = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.CheckIn", _server);
                    objCheckin = (tikAeroProcess.CheckIn)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objCheckin = new tikAeroProcess.CheckIn(); }

                if (language.Length == 0)
                { language = "EN"; }
                rs = objCheckin.GetBookingSegmentCheckIn(ref bookingId, ref  clientId, ref language);
                ds = RecordsetToDataset(rs, "BookingSegmentCheckIn");
            }
            catch
            { }
            finally
            {
                if (objCheckin != null)
                { Marshal.FinalReleaseComObject(objCheckin); }
                objCheckin = null;

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet GetPassengerDetails(string passengerId,
                                           string bookingSegmentId,
                                           string flightId,
                                           DataTable idS,
                                           DataTable passenger,
                                           DataTable remarks,
                                           DataTable services,
                                           DataTable baggages,
                                           DataTable segments,
                                           DataTable fees,
                                           bool bPassenger,
                                           bool bRemark,
                                           bool bServices,
                                           bool bBaggages,
                                           bool bSegments,
                                           bool bFees,
                                           bool bBookingDetail,
                                           string language)
        {
            tikAeroProcess.CheckIn objCheckin = null;

            ADODB.Recordset rs = null;
            ADODB.Recordset rsIDs = DatasetToRecordset(idS);
            ADODB.Recordset rsPassenger = DatasetToRecordset(passenger);
            ADODB.Recordset rsRemark = DatasetToRecordset(remarks);
            ADODB.Recordset rsServices = DatasetToRecordset(services);
            ADODB.Recordset rsBaggages = DatasetToRecordset(baggages);
            ADODB.Recordset rsSegments = DatasetToRecordset(segments);
            ADODB.Recordset rsFees = DatasetToRecordset(fees);

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.CheckIn", _server);
                    objCheckin = (tikAeroProcess.CheckIn)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objCheckin = new tikAeroProcess.CheckIn(); }

                if (language.Length == 0)
                { language = "EN"; }
                rs = objCheckin.GetPassengerDetails(ref passengerId,
                                                    ref bookingSegmentId,
                                                    ref flightId,
                                                    ref rsIDs,
                                                    ref rsPassenger,
                                                    ref rsRemark,
                                                    ref rsServices,
                                                    ref rsBaggages,
                                                    ref rsSegments,
                                                    ref rsFees,
                                                    ref bPassenger,
                                                    ref bRemark,
                                                    ref bServices,
                                                    ref bBaggages,
                                                    ref bSegments,
                                                    ref bFees,
                                                    ref bBookingDetail,
                                                    ref language);

                ds = RecordsetToDataset(rs, "PassengerDetails");
            }
            catch
            { }
            finally
            {
                if (objCheckin != null)
                {
                    Marshal.FinalReleaseComObject(objCheckin);
                    objCheckin = null;
                }

                ClearRecordset(ref rsIDs);
                ClearRecordset(ref rsPassenger);
                ClearRecordset(ref rsRemark);
                ClearRecordset(ref rsServices);
                ClearRecordset(ref rsBaggages);
                ClearRecordset(ref rsSegments);
                ClearRecordset(ref rsFees);
            }

            return ds;
        }
        public bool CheckInSave(DataTable mappings,
                                DataTable baggage,
                                DataTable seats,
                                DataTable passengers,
                                DataTable services,
                                DataTable remarks,
                                DataTable segments,
                                DataTable fees)
        {
            tikAeroProcess.CheckIn objCheckin = null;

            ADODB.Recordset rsMappings = DatasetToRecordset(mappings);
            ADODB.Recordset rsBaggage = DatasetToRecordset(baggage);
            ADODB.Recordset rsSeats = DatasetToRecordset(seats);
            ADODB.Recordset rsPassengers = DatasetToRecordset(passengers);
            ADODB.Recordset rsServices = DatasetToRecordset(services);
            ADODB.Recordset rsRemarks = DatasetToRecordset(remarks);
            ADODB.Recordset rsSegments = DatasetToRecordset(segments);
            ADODB.Recordset rsFees = DatasetToRecordset(fees);

            bool result = false;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.CheckIn", _server);
                    objCheckin = (tikAeroProcess.CheckIn)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objCheckin = new tikAeroProcess.CheckIn(); }


                result = objCheckin.Save(ref rsMappings,
                                        ref rsBaggage,
                                        ref rsSeats,
                                        ref rsPassengers,
                                        ref rsServices,
                                        ref rsRemarks,
                                        ref rsSegments,
                                        ref rsFees);
            }
            catch
            { }
            finally
            {
                if (objCheckin != null)
                {
                    Marshal.FinalReleaseComObject(objCheckin);
                    objCheckin = null;
                }
                ClearRecordset(ref rsMappings);
                ClearRecordset(ref rsBaggage);
                ClearRecordset(ref rsSeats);
                ClearRecordset(ref rsPassengers);
                ClearRecordset(ref rsServices);
                ClearRecordset(ref rsRemarks);
                ClearRecordset(ref rsSegments);
                ClearRecordset(ref rsFees);
            }

            return result;
        }
        public DataSet GetPassengersManifest(
            string flightID,
            string strOrigin = "",
            string strDestination = "",
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
            tikAeroProcess.CheckIn objCheckin = null;
            ADODB.Recordset rs = null;
            ADODB.Recordset rsRemarks = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.CheckIn", _server);
                    objCheckin = (tikAeroProcess.CheckIn)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objCheckin = new tikAeroProcess.CheckIn(); }

                rs = objCheckin.GetPassengersManifest(
                    ref flightID,
                    strOrigin,
                    strDestination,
                    rsRemarks,
                    ref bReturnServices,
                    ref bReturnBagTags,
                    ref bReturnRemarks,
                    ref bNotCheckedIn,
                    ref bCheckedIn,
                    ref bOffloaded,
                    ref bNoShow,
                    ref bInfants,
                    ref bConfirmed,
                    ref bWaitlisted,
                    ref bCancelled,
                    ref bStandby,
                    ref bIndividual,
                    ref bGroup,
                    ref bTransit);
                ds = RecordsetToDataset(rs, "PassengersManifest");
            }
            catch
            { }
            finally
            {
                if (objCheckin != null)
                { Marshal.FinalReleaseComObject(objCheckin); }
                objCheckin = null;

                ClearRecordset(ref rs);
            }

            return ds;
        }
    }
}