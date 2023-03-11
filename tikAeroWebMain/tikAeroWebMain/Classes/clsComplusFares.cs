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
    public class ComplusFares : RunComplus
    {
        public ComplusFares() : base() { }
        public string GetFlightSummary(Passengers passengers,
                                       Flights flights,
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
                                       bool bNoVat)
        {
            tikAeroProcess.Fares objFares = null;

            ADODB.Recordset rsPassenger = null;
            ADODB.Recordset rsTaxes = null;
            ADODB.Recordset rsMapping = null;
            ADODB.Recordset rsQuote = null;
            ADODB.Recordset rsFlight = null;

            string strXML = string.Empty;
            try
            {
                bool isComplete = false;

                if (string.IsNullOrEmpty(_server) == false)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Fares", _server);
                    objFares = (tikAeroProcess.Fares)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                {
                    objFares = new tikAeroProcess.Fares();
                }

                //Set default value.
                if (string.IsNullOrEmpty(fareType))
                {
                    fareType = "FARE";
                }
                //Set default value.
                if (string.IsNullOrEmpty(language))
                {
                    language = "EN";
                }

                //Read fare summary.
                rsFlight = FabricateFlightRecordset();
                rsPassenger = FabricatePassengerRecordset();

                //Fill Object Fligt Recordset.
                AddToRecordset(flights, rsFlight);
                //Fill object passenger to Recordset.
                AddToRecordset(passengers, rsPassenger);

                isComplete = objFares.FareQuote(rsPassenger,
                                                strAgencyCode,
                                                ref rsTaxes,
                                                ref rsMapping,
                                                ref rsQuote,
                                                rsFlight,
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
                                                bNoVat);

                //convert Recordset to Object
                if (isComplete == true)
                {
                    Helper objHelper = new Helper();
                    using (StringWriter stw = new StringWriter())
                    {
                        using (XmlWriter xtw = XmlWriter.Create(stw))
                        {
                            xtw.WriteStartElement("Booking");
                            {
                                objHelper.RecordsetToXML(xtw, null, rsFlight, rsPassenger, rsMapping, rsQuote, null, null, null, rsTaxes);
                            }
                            xtw.WriteEndElement();//Booking
                        }
                        strXML = stw.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objFares != null)
                {
                    Marshal.FinalReleaseComObject(objFares);
                    objFares = null;
                }

                //Close and clear unused recordset.
                RecordsetHelper.ClearRecordset(ref rsPassenger);
                RecordsetHelper.ClearRecordset(ref rsTaxes);
                RecordsetHelper.ClearRecordset(ref rsMapping);
                RecordsetHelper.ClearRecordset(ref rsQuote);
                RecordsetHelper.ClearRecordset(ref rsFlight);

                base.Dispose();
            }
            return strXML;
        }

        #region Helper
        private void AddToRecordset(Flights flights, ADODB.Recordset rs)
        {
            for (int i = 0; i < flights.Count; i++)
            {
                rs.AddNew();
                RecordsetHelper.AssignRsGuid(rs, "flight_id", flights[i].flight_id);
                RecordsetHelper.AssignRsGuid(rs, "fare_id", flights[i].fare_id);
                RecordsetHelper.AssignRsString(rs, "booking_class_rcd", flights[i].booking_class_rcd);
                RecordsetHelper.AssignRsString(rs, "origin_rcd", flights[i].origin_rcd);
                RecordsetHelper.AssignRsString(rs, "destination_rcd", flights[i].destination_rcd);
                RecordsetHelper.AssignRsDateTime(rs, "departure_date", flights[i].departure_date);
                RecordsetHelper.AssignRsGuid(rs, "flight_connection_id", flights[i].flight_connection_id);
                RecordsetHelper.AssignRsString(rs, "od_origin_rcd", flights[i].od_origin_rcd);
                RecordsetHelper.AssignRsString(rs, "od_destination_rcd", flights[i].od_destination_rcd);

                RecordsetHelper.AssignRsString(rs, "boarding_class_rcd", flights[i].boarding_class_rcd);
                RecordsetHelper.AssignRsString(rs, "transit_airport_rcd", flights[i].transit_airport_rcd);
                RecordsetHelper.AssignRsString(rs, "transit_boarding_class_rcd", flights[i].transit_boarding_class_rcd);
                RecordsetHelper.AssignRsString(rs, "transit_booking_class_rcd", flights[i].transit_booking_class_rcd);
                RecordsetHelper.AssignRsGuid(rs, "transit_flight_id", flights[i].transit_flight_id);
                RecordsetHelper.AssignRsGuid(rs, "transit_fare_id", flights[i].transit_fare_id);
                RecordsetHelper.AssignRsDateTime(rs, "transit_departure_date", flights[i].transit_departure_date);
                RecordsetHelper.AssignRsString(rs, "transit_points", flights[i].transit_points);
            }
        }
        private void AddToRecordset(Passengers passengers, ADODB.Recordset rs)
        {
            for (int i = 0; i < passengers.Count; i++)
            {
                rs.AddNew();
                RecordsetHelper.AssignRsGuid(rs, "passenger_id", passengers[i].passenger_id);
                RecordsetHelper.AssignRsString(rs, "passenger_type_rcd", passengers[i].passenger_type_rcd);
            }
        }
        #endregion
    }
}