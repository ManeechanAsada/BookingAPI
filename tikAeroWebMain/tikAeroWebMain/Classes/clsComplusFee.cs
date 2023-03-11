using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Data;
using tikSystem.Web.Library;
using Avantik.Web.Service.COMHelper;

namespace tikAeroWebMain
{
    public class ComplusFee : RunComplus
    {
        public ComplusFee() : base() { }

        public string CalculateNewFees(string AgencyCode,
                                        DataTable header,
                                        DataTable segment,
                                        DataTable passenger,
                                        DataTable fees,
                                        string currency,
                                        DataTable remark,
                                        DataTable payment,
                                        DataTable mapping,
                                        DataTable service,
                                        DataTable tax,
                                        bool checkBooking,
                                        bool checkSegment,
                                        bool checkName,
                                        bool checkSeat,
                                        bool checkSpecialService,
                                        string strLanguage ,bool bNovat)
        {
            tikAeroProcess.Fees objFees = null;
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

            DataSet ds = new DataSet();
            ds.DataSetName = "Fees";

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Fees", _server);
                    objFees = (tikAeroProcess.Fees)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objFees = new tikAeroProcess.Fees(); }

                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Booking", _server);
                    objBooking = (tikAeroProcess.Booking)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objBooking = new tikAeroProcess.Booking(); }

                //Get Empty Recordset
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
                if (objBooking != null)
                {
                    Marshal.FinalReleaseComObject(objBooking);
                    objBooking = null;
                }
                DatasetToRecordset(header, ref rsHeader);
                if (segment != null)
                {
                    DatasetToRecordset(segment, ref rsSegment);
                }
                if (passenger != null)
                {
                    DatasetToRecordset(passenger, ref rsPassenger);
                }
                if (fees != null)
                {
                    DatasetToRecordset(fees, ref rsFees);
                }
                if (remark != null)
                {
                    DatasetToRecordset(remark, ref rsRemark);
                }
                if(payment != null)
                {
                    DatasetToRecordset(payment, ref rsPayment);
                }
                if (mapping != null)
                {
                    DatasetToRecordset(mapping, ref rsMapping);
                }
                if (service != null)
                {
                    DatasetToRecordset(service, ref rsService);
                }
                if (tax != null)
                {
                    DatasetToRecordset(tax, ref rsTax);
                }
                
                if (checkBooking == true)
                {
                    objFees.BookingCreate(AgencyCode,
                                         currency,
                                         header.Rows[0]["booking_id"].ToString(),
                                         ref rsHeader,
                                         ref rsSegment,
                                         ref rsPassenger,
                                         ref rsFees,
                                         ref rsRemark,
                                         ref rsMapping,
                                         ref rsService,
                                         ref strLanguage);
                }
                else if (checkSegment == true)
                {
                    objFees.BookingChange(AgencyCode,
                                          currency,
                                          header.Rows[0]["booking_id"].ToString(),
                                          ref rsHeader,
                                          ref rsSegment,
                                          ref rsPassenger,
                                          ref rsFees,
                                          ref rsRemark,
                                          ref rsMapping,
                                          ref rsService,
                                          ref strLanguage);
                }
                else if (checkSeat == true)
                {
                    objFees.SeatAssignment(AgencyCode,
                                           currency,
                                           header.Rows[0]["booking_id"].ToString(),
                                           ref rsHeader,
                                           ref rsSegment,
                                           ref rsPassenger,
                                           ref rsFees,
                                           ref rsRemark,
                                           ref rsMapping,
                                           ref rsService,
                                           ref strLanguage,ref bNovat);
                }
                else if (checkName == true)
                {
                    objFees.NameChange(AgencyCode,
                                      currency,
                                      header.Rows[0]["booking_id"].ToString(),
                                      ref rsHeader,
                                      ref rsSegment,
                                      ref rsPassenger,
                                      ref rsFees,
                                      ref rsRemark,
                                      ref rsMapping,
                                      ref rsService,
                                      ref strLanguage);
                }
                else if (checkSpecialService == true)
                {
                    objFees.SpecialService(AgencyCode,
                                           currency,
                                           header.Rows[0]["booking_id"].ToString(),
                                           ref rsHeader,
                                           ref rsService,
                                           ref rsFees,
                                           ref rsRemark,
                                           ref rsMapping,
                                           ref strLanguage, ref bNovat);
                }
                RecordsetToDataset(ds, rsFees, "Fee");
            }
            catch
            { }
            finally
            {
                if (objFees != null)
                {
                    Marshal.FinalReleaseComObject(objFees);
                    objFees = null;
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

            return ds.GetXml();
        }
        public IList<Fee> GetBaggageFeeOption(IList<Mapping> mapping,
                                                   Guid bookingSegmentId,
                                                   Guid passengerId,
                                                   string agencyCode,
                                                   string languageCode,
                                                   int maxUnits,
                                                   IList<Fee> fees,bool bNovat)
        {
            tikAeroProcess.Fees objFee = null;
            ADODB.Recordset rs = null;
            ADODB.Recordset rsMapping = null;
            ADODB.Recordset rsFee = null;
            IList<Fee> baggageFees = new List<Fee>();

            try
            {
                if (string.IsNullOrEmpty(_server) == false)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Fees", _server);
                    objFee = (tikAeroProcess.Fees)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objFee = new tikAeroProcess.Fees(); }



                if (mapping != null && mapping.Count > 0)
                {
                    string strPassengerId = string.Empty;
                    if (passengerId.Equals(Guid.Empty) == false)
                    {
                        strPassengerId = passengerId.ToString();
                    }
                    rsMapping = mapping.FillRecordset();
                    rs = objFee.GetBaggageFeeOptions(rsMapping,
                                                    bookingSegmentId.ToString(),
                                                    strPassengerId,
                                                    agencyCode,
                                                    languageCode,
                                                    maxUnits,
                                                    rsFee, bNovat);
                    //convert Recordset to Object
                    if (rs != null && rs.RecordCount > 0)
                    {
                        rs.MoveFirst();
                        Fee bf;
                        while (!rs.EOF)
                        {
                            bf = new Fee();

                            bf.baggage_fee_option_id = RecordsetHelper.ToGuid(rs, "baggage_fee_option_id");
                            bf.passenger_id = RecordsetHelper.ToGuid(rs, "passenger_id");
                            bf.booking_segment_id = RecordsetHelper.ToGuid(rs, "booking_segment_id");
                            bf.fee_id = RecordsetHelper.ToGuid(rs, "fee_id");
                            bf.fee_rcd = RecordsetHelper.ToString(rs, "fee_rcd");
                            bf.fee_category_rcd = RecordsetHelper.ToString(rs, "fee_category_rcd");
                            bf.currency_rcd = RecordsetHelper.ToString(rs, "currency_rcd");
                            bf.display_name = RecordsetHelper.ToString(rs, "display_name");
                            bf.number_of_units = RecordsetHelper.ToDecimal(rs, "number_of_units");
                            bf.fee_amount = RecordsetHelper.ToDecimal(rs, "fee_amount");
                            bf.fee_amount_incl = RecordsetHelper.ToDecimal(rs, "fee_amount_incl");
                            bf.total_amount = RecordsetHelper.ToDecimal(rs, "total_amount");
                            bf.total_amount_incl = RecordsetHelper.ToDecimal(rs, "total_amount_incl");
                            bf.vat_percentage = RecordsetHelper.ToDecimal(rs, "vat_percentage");

                            baggageFees.Add(bf);
                            bf = null;
                            rs.MoveNext();
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
                if (objFee != null)
                {
                    Marshal.FinalReleaseComObject(objFee);
                    objFee = null;
                }
                RecordsetHelper.ClearRecordset(ref rs);
                RecordsetHelper.ClearRecordset(ref rsMapping, false);
                RecordsetHelper.ClearRecordset(ref rsFee);
                base.Dispose();
            }
            return baggageFees;
        }
        public IList<Fee> GetFee(string strFeeRcd,
                                   string strCurrencyCode,
                                   string strAgencyCode,
                                   string strClass,
                                   string strFareBasis,
                                   string strOrigin,
                                   string strDestination,
                                   string strFlightNumber,
                                   DateTime dtDate,
                                   string strLanguage,bool bNovat)
        {
            tikAeroProcess.Settings objSettings = null;
            ADODB.Recordset rs = null;
            IList<Fee> fees = new List<Fee>();
            try
            {
                if (string.IsNullOrEmpty(_server) == false)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Settings", _server);
                    objSettings = (tikAeroProcess.Settings)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objSettings = new tikAeroProcess.Settings(); }

                rs = objSettings.GetFee(strFeeRcd,
                                        strCurrencyCode,
                                        strAgencyCode,
                                        strClass,
                                        strFareBasis,
                                        strOrigin,
                                        strDestination,
                                        strFlightNumber,
                                        dtDate,
                                        strLanguage, bNovat);

                //convert Recordset to Object
                if (rs != null && rs.RecordCount > 0)
                {
                    Fee f;
                    rs.MoveFirst();
                    while (!rs.EOF)
                    {
                        f = new Fee();

                        f.fee_id = RecordsetHelper.ToGuid(rs, "fee_id");
                        f.fee_rcd = RecordsetHelper.ToString(rs, "fee_rcd");
                        f.currency_rcd = RecordsetHelper.ToString(rs, "currency_rcd");
                        f.fee_amount = RecordsetHelper.ToDecimal(rs, "fee_amount");
                        f.fee_amount_incl = RecordsetHelper.ToDecimal(rs, "fee_amount_incl");
                        f.vat_percentage = RecordsetHelper.ToDecimal(rs, "vat_percentage");
                        f.display_name = RecordsetHelper.ToString(rs, "display_name");
                        f.fee_category_rcd = RecordsetHelper.ToString(rs, "fee_category_rcd");
                        f.minimum_fee_amount_flag = RecordsetHelper.ToByte(rs, "minimum_fee_amount_flag");
                        f.fee_percentage = RecordsetHelper.ToDecimal(rs, "fee_percentage");
                        f.od_flag = RecordsetHelper.ToByte(rs, "od_flag");

                        fees.Add(f);
                        f = null;
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
                if (objSettings != null)
                {
                    Marshal.FinalReleaseComObject(objSettings);
                    objSettings = null;
                }
                RecordsetHelper.ClearRecordset(ref rs);
                base.Dispose();
            }

            return fees;
        }
        public List<ServiceFee> GetSegmentFee(string agencyCode,
                                               string currencyCode,
                                               string languageCode,
                                               int numberOfPassenger,
                                               int numberOfInfant,
                                               Services services,
                                               IList<SegmentService> segmentService,
                                               bool SpecialService, bool bNovat)
        {
            List<ServiceFee> fees = new List<ServiceFee>();
            tikAeroProcess.Fees objFees = null;
            ADODB.Recordset rs = null;
            try
            {
                if (segmentService != null && segmentService.Count > 0 && services != null && services.Count > 0)
                {
                    bool b = false;
                    //Declaration
                    if (string.IsNullOrEmpty(_server) == false)
                    {
                        Type remote = Type.GetTypeFromProgID("tikAeroProcess.Fees", _server);
                        objFees = (tikAeroProcess.Fees)Activator.CreateInstance(remote);
                        remote = null;
                    }
                    else
                    { objFees = new tikAeroProcess.Fees(); }

                    //Implement Segment Fee function
                    ADODB.Recordset rsSegment;
                    //Fill Segment information
                    for (int f = 0; f < services.Count; f++)
                    {
                        //Create new segmentFee instance.
                        rsSegment = objFees.GetSegmentFeeRecordset();
                        for (int i = 0; i < segmentService.Count; i++)
                        {
                            rsSegment.AddNew();
                            if (segmentService[i].flight_connection_id.Equals(Guid.Empty) == false)
                            {
                                rsSegment.Fields["flight_connection_id"].Value = "{" + segmentService[i].flight_connection_id.ToString() + "}";
                            }
                            rsSegment.Fields["origin_rcd"].Value = segmentService[i].origin_rcd;
                            rsSegment.Fields["destination_rcd"].Value = segmentService[i].destination_rcd;
                            rsSegment.Fields["od_origin_rcd"].Value = segmentService[i].od_origin_rcd;
                            rsSegment.Fields["od_destination_rcd"].Value = segmentService[i].od_destination_rcd;
                            rsSegment.Fields["booking_class_rcd"].Value = segmentService[i].booking_class_rcd;
                            rsSegment.Fields["fare_code"].Value = segmentService[i].fare_code;
                            rsSegment.Fields["airline_rcd"].Value = segmentService[i].airline_rcd;
                            rsSegment.Fields["flight_number"].Value = segmentService[i].flight_number;
                            rsSegment.Fields["departure_date"].Value = segmentService[i].departure_date;
                            rsSegment.Fields["pax_count"].Value = numberOfPassenger;
                            rsSegment.Fields["inf_count"].Value = numberOfInfant;
                        }
                        //Call COM plus function
                        if (SpecialService == true)
                        {
                            b = objFees.SpecialServiceFee(agencyCode, currencyCode, services[f].special_service_rcd, ref rsSegment, languageCode,bNovat);
                        }
                        else
                        {
                            b = objFees.SegmentFee(agencyCode, currencyCode, ref rsSegment, services[f].special_service_rcd, languageCode);
                        }
                        
                        ServiceFee sf;
                        if (b == true)
                        {
                            if (rsSegment != null && rsSegment.RecordCount > 0)
                            {
                                rsSegment.MoveFirst();
                                while (!rsSegment.EOF)
                                {
                                    sf = new ServiceFee();
                                    sf.fee_rcd = services[f].special_service_rcd;
                                    sf.special_service_rcd = services[f].special_service_rcd;
                                    sf.display_name = services[f].display_name;
                                    sf.service_on_request_flag = Convert.ToBoolean(services[f].service_on_request_flag);
                                    sf.cut_off_time = Convert.ToBoolean(services[f].cut_off_time);
                                    sf.origin_rcd = RecordsetHelper.ToString(rsSegment, "origin_rcd");
                                    sf.destination_rcd = RecordsetHelper.ToString(rsSegment, "destination_rcd");
                                    sf.od_origin_rcd = RecordsetHelper.ToString(rsSegment, "od_origin_rcd");
                                    sf.od_destination_rcd = RecordsetHelper.ToString(rsSegment, "od_destination_rcd");
                                    sf.booking_class_rcd = RecordsetHelper.ToString(rsSegment, "booking_class_rcd");
                                    sf.fare_code = RecordsetHelper.ToString(rsSegment, "fare_code");
                                    sf.airline_rcd = RecordsetHelper.ToString(rsSegment, "airline_rcd");
                                    sf.flight_number = RecordsetHelper.ToString(rsSegment, "flight_number");
                                    sf.departure_date = RecordsetHelper.ToDateTime(rsSegment, "departure_date");
                                    sf.fee_amount = RecordsetHelper.ToDecimal(rsSegment, "fee_amount");
                                    sf.fee_amount_incl = RecordsetHelper.ToDecimal(rsSegment, "fee_amount_incl");
                                    sf.total_fee_amount = RecordsetHelper.ToDecimal(rsSegment, "total_fee_amount");
                                    sf.total_fee_amount_incl = RecordsetHelper.ToDecimal(rsSegment, "total_fee_amount_incl");
                                    sf.currency_rcd = RecordsetHelper.ToString(rsSegment, "currency_rcd");
                                    rsSegment.MoveNext();
                                    fees.Add(sf);
                                }

                            }
                        }
                        RecordsetHelper.ClearRecordset(ref rsSegment, true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objFees != null)
                {
                    Marshal.FinalReleaseComObject(objFees);
                    objFees = null;
                }
                RecordsetHelper.ClearRecordset(ref rs, true);
                base.Dispose();
            }

            return fees;
        }
    }
}