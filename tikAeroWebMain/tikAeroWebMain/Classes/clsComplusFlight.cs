using System;
using System.Collections.Generic;
using System.Web;
using System.Runtime.InteropServices;
using System.Data;
using System.IO;
using System.Xml;
using tikSystem.Web.Library;
using Avantik.Web.Service.COMHelper;

namespace tikAeroWebMain
{
    public class ComplusFlight : RunComplus
    {
        public ComplusFlight() : base() { }
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
                                      bool bSingle)

        {
            tikAeroProcess.Flights objFlight = null;
            ADODB.Recordset rs = null;
            bool bResult = false;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Flights", _server);
                    objFlight = (tikAeroProcess.Flights)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objFlight = new tikAeroProcess.Flights(); }

                rs = objFlight.GetSingleFlight(ref strFlightId,
                                                ref strAirline,
                                                ref strFlightNumber,
                                                ref dtFlightFrom,
                                                ref dtFlightTo,
                                                ref strLanguage,
                                                ref strOrigin,
                                                ref strDestination,
                                                ref bWrite,
                                                ref bEmptyRs,
                                                ref strScheduleId,
                                                ref bSingle);

                if (rs != null && rs.RecordCount > 0)
                {
                    if (strOrigin == rs.Fields["origin_rcd"].Value.ToString() && strDestination == rs.Fields["destination_rcd"].Value.ToString())
                    {
                        bResult = true;
                    }
                }
            }
            catch
            { }
            finally
            {
                if (objFlight != null)
                { Marshal.FinalReleaseComObject(objFlight); }
                objFlight = null;

                ClearRecordset(ref rs);
            }

            return bResult;
        }


        public bool GetFlight(string strFlightId)
        {
            tikAeroProcess.Flights objFlight = null;
            bool bResult = false;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Flights", _server);
                    objFlight = (tikAeroProcess.Flights)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objFlight = new tikAeroProcess.Flights(); }

                bResult = objFlight.GetFlight(strFlightId);
            }
            catch
            { }
            finally
            {
                if (objFlight != null)
                { Marshal.FinalReleaseComObject(objFlight); }
                objFlight = null;
            }

            return bResult;
        }


        public DataSet GetSeatMapLayout(string origin, string destination, string flightId, string boardingClass, string bookingClass)
        {
            tikAeroProcess.Flights objFlight = null;
            string language = string.Empty;
            string strConfiguration = string.Empty;
            ADODB.Recordset rsCompartment = null;
            ADODB.Recordset rsSeatmap = null;
            ADODB.Recordset rsAttribute = null;
            bool result = false;

            if(string.IsNullOrEmpty(boardingClass))
            {
                boardingClass = "Y";
            }
            
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.CheckIn", _server);
                    objFlight = (tikAeroProcess.Flights)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objFlight = new tikAeroProcess.Flights(); }

                result = objFlight.GetSeatmapLayout(ref flightId, ref origin, ref destination, ref boardingClass, ref strConfiguration,ref language, ref rsCompartment, ref rsSeatmap, ref rsAttribute);
                ds = RecordsetToDataset(rsSeatmap, "Seat");
            }
            catch
            { }
            finally
            {
                if (objFlight != null)
                { Marshal.FinalReleaseComObject(objFlight); }
                objFlight = null;

                ClearRecordset(ref rsCompartment);
                ClearRecordset(ref rsSeatmap);
                ClearRecordset(ref rsAttribute);

            }

            return ds;
        }

    }
}