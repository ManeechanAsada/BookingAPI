using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Data;

namespace tikAeroWebMain
{
    public class ComplusMiscellaneous : RunComplus
    {
        public ComplusMiscellaneous() : base() { }

        public DataSet GetVouchers(string recordLocator,
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
                                    string strAgencyCode)
        {
            tikAeroProcess.Miscellaneous objMiscellaneous = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            DateTime dtFrom = DateTime.MinValue;
            DateTime dtTo = DateTime.MinValue;
            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Miscellaneous", _server);
                    objMiscellaneous = (tikAeroProcess.Miscellaneous)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objMiscellaneous = new tikAeroProcess.Miscellaneous(); }

                rs = objMiscellaneous.GetVouchers(recordLocator,
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
                                                ref write, ref dtFrom, ref dtTo, ref strAgencyCode);

                ds = RecordsetToDataset(rs, "Vouchers");
            }
            catch
            { }
            finally
            {
                if (objMiscellaneous != null)
                {
                    Marshal.FinalReleaseComObject(objMiscellaneous);
                    objMiscellaneous = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet GetTicketsIssued(DateTime reportFrom,
                                     DateTime reportTo,
                                     DateTime flightFrom,
                                     DateTime flightTo,
                                     string origin,
                                     string destination,
                                     string agency,
                                     string airline,
                                     string flight)
        {
            tikAeroProcess.Miscellaneous objMiscellaneous = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Miscellaneous", _server);
                    objMiscellaneous = (tikAeroProcess.Miscellaneous)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objMiscellaneous = new tikAeroProcess.Miscellaneous(); }

                rs = objMiscellaneous.GetTicketsIssued(ref origin,
                                                        ref destination,
                                                        ref agency,
                                                        ref airline,
                                                        ref flight,
                                                        ref reportFrom,
                                                        ref reportTo,
                                                        ref flightFrom,
                                                        ref flightTo);

                ds = RecordsetToDataset(rs, "Tickets");
            }
            catch
            { }
            finally
            {
                if (objMiscellaneous != null)
                {
                    Marshal.FinalReleaseComObject(objMiscellaneous);
                    objMiscellaneous = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet GetTicketsUsed(DateTime reportFrom,
                                     DateTime reportTo,
                                     DateTime flightFrom,
                                     DateTime flightTo,
                                     string origin,
                                     string destination,
                                     string agency,
                                     string airline,
                                     string flight)
        {
            tikAeroProcess.Miscellaneous objMiscellaneous = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Miscellaneous", _server);
                    objMiscellaneous = (tikAeroProcess.Miscellaneous)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objMiscellaneous = new tikAeroProcess.Miscellaneous(); }

                rs = objMiscellaneous.GetTicketsUsed(ref origin,
                                                    ref destination,
                                                    ref agency,
                                                    ref airline,
                                                    ref flight,
                                                    ref reportFrom,
                                                    ref reportTo,
                                                    ref flightFrom,
                                                    ref flightTo);

                ds = RecordsetToDataset(rs, "Tickets");
            }
            catch
            { }
            finally
            {
                if (objMiscellaneous != null)
                {
                    Marshal.FinalReleaseComObject(objMiscellaneous);
                    objMiscellaneous = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet GetTicketsRefunded(DateTime reportFrom,
                                 DateTime reportTo,
                                 DateTime flightFrom,
                                 DateTime flightTo,
                                 string origin,
                                 string destination,
                                 string agency,
                                 string airline,
                                 string flight)
        {
            tikAeroProcess.Miscellaneous objMiscellaneous = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Miscellaneous", _server);
                    objMiscellaneous = (tikAeroProcess.Miscellaneous)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objMiscellaneous = new tikAeroProcess.Miscellaneous(); }

                rs = objMiscellaneous.GetTicketsRefunded(ref origin,
                                                        ref destination,
                                                        ref agency,
                                                        ref airline,
                                                        ref flight,
                                                        ref reportFrom,
                                                        ref reportTo,
                                                        ref flightFrom,
                                                        ref flightTo);

                ds = RecordsetToDataset(rs, "Tickets");
            }
            catch
            { }
            finally
            {
                if (objMiscellaneous != null)
                {
                    Marshal.FinalReleaseComObject(objMiscellaneous);
                    objMiscellaneous = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }

        public DataSet GetTicketsExpired(DateTime reportFrom,
                                         DateTime reportTo,
                                         DateTime flightFrom,
                                         DateTime flightTo,
                                         string origin,
                                         string destination,
                                         string agency,
                                         string airline,
                                         string flight)
        {
            tikAeroProcess.Miscellaneous objMiscellaneous = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Miscellaneous", _server);
                    objMiscellaneous = (tikAeroProcess.Miscellaneous)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objMiscellaneous = new tikAeroProcess.Miscellaneous(); }

                rs = objMiscellaneous.GetTicketsExpired(ref origin,
                                                        ref destination,
                                                        ref agency,
                                                        ref airline,
                                                        ref flight,
                                                        ref reportFrom,
                                                        ref reportTo,
                                                        ref flightFrom,
                                                        ref flightTo);

                ds = RecordsetToDataset(rs, "Tickets");
            }
            catch
            { }
            finally
            {
                if (objMiscellaneous != null)
                {
                    Marshal.FinalReleaseComObject(objMiscellaneous);
                    objMiscellaneous = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }

        public DataSet GetTicketsCancelled(string origin,
                                           string destination,
                                           string agency,
                                           string airline,
                                           string flight,
                                           DateTime reportFrom,
                                           DateTime reportTo,
                                           DateTime flightFrom,
                                           DateTime flightTo,
                                           short ticketonly,
                                           short refundable,
                                           string profileID,
                                           string ticketNumber,
                                           string firstName,
                                           string lastName,
                                           string passengerId,
                                           string bookingSegmentID)
        {
            tikAeroProcess.Miscellaneous objMiscellaneous = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Miscellaneous", _server);
                    objMiscellaneous = (tikAeroProcess.Miscellaneous)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objMiscellaneous = new tikAeroProcess.Miscellaneous(); }

                rs = objMiscellaneous.GetTicketsCancelled(ref origin,
                                                            ref destination,
                                                            ref agency,
                                                            ref airline,
                                                            ref flight,
                                                            ref reportFrom,
                                                            ref reportTo,
                                                            ref flightFrom,
                                                            ref flightTo,
                                                            ref ticketonly,
                                                            ref refundable,
                                                            ref profileID,
                                                            ref ticketNumber,
                                                            ref firstName,
                                                            ref lastName,
                                                            ref passengerId,
                                                            ref bookingSegmentID);
                ds = RecordsetToDataset(rs, "Tickets");
            }
            catch
            { }
            finally
            {
                if (objMiscellaneous != null)
                {
                    Marshal.FinalReleaseComObject(objMiscellaneous);
                    objMiscellaneous = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet GetTicketsNotFlown(string origin,
                                          string destination,
                                          string agency,
                                          string airline,
                                          string fligh,
                                          DateTime reportFrom,
                                          DateTime reportTo,
                                          DateTime flightFrom,
                                          DateTime flightTo,
                                          bool unflown,
                                          bool noShow)
        {
            tikAeroProcess.Miscellaneous objMiscellaneous = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Miscellaneous", _server);
                    objMiscellaneous = (tikAeroProcess.Miscellaneous)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objMiscellaneous = new tikAeroProcess.Miscellaneous(); }

                rs = objMiscellaneous.GetTicketsNotFlown(ref origin,
                                                        ref destination,
                                                        ref agency,
                                                        ref airline,
                                                        ref fligh,
                                                        ref reportFrom,
                                                        ref reportTo,
                                                        ref flightFrom,
                                                        ref flightTo,
                                                        ref unflown,
                                                        ref noShow);

                ds = RecordsetToDataset(rs, "Tickets");
            }
            catch
            { }
            finally
            {
                if (objMiscellaneous != null)
                {
                    Marshal.FinalReleaseComObject(objMiscellaneous);
                    objMiscellaneous = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet GetBookingFeeAccounted(string agencyCode,
                                      string userId,
                                      string fee,
                                      DateTime from,
                                      DateTime to)
        {
            tikAeroProcess.Miscellaneous objMiscellaneous = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Miscellaneous", _server);
                    objMiscellaneous = (tikAeroProcess.Miscellaneous)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objMiscellaneous = new tikAeroProcess.Miscellaneous(); }

                rs = objMiscellaneous.GetBookingFeeAccounted(ref agencyCode, ref userId, ref fee, ref from, ref to);

                ds = RecordsetToDataset(rs, "BookingFee");
            }
            catch
            { }
            finally
            {
                if (objMiscellaneous != null)
                {
                    Marshal.FinalReleaseComObject(objMiscellaneous);
                    objMiscellaneous = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }

        public DataSet GetBookingFeeBooked(string agencyCode,
                                          string userId,
                                          string fee,
                                          DateTime from,
                                          DateTime to)
        {
            tikAeroProcess.Miscellaneous objMiscellaneous = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Miscellaneous", _server);
                    objMiscellaneous = (tikAeroProcess.Miscellaneous)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objMiscellaneous = new tikAeroProcess.Miscellaneous(); }

                rs = objMiscellaneous.GetBookingFeeBooked(ref agencyCode, ref userId, ref fee, ref from, ref to);

                ds = RecordsetToDataset(rs, "BookingFee");
            }
            catch
            { }
            finally
            {
                if (objMiscellaneous != null)
                {
                    Marshal.FinalReleaseComObject(objMiscellaneous);
                    objMiscellaneous = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet GetBookingFeeVoided(string agencyCode,
                                          string userId,
                                          string fee,
                                          DateTime from,
                                          DateTime to)
        {
            tikAeroProcess.Miscellaneous objMiscellaneous = null;
            ADODB.Recordset rs = null;

            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Miscellaneous", _server);
                    objMiscellaneous = (tikAeroProcess.Miscellaneous)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objMiscellaneous = new tikAeroProcess.Miscellaneous(); }

                rs = objMiscellaneous.GetBookingFeeVoided(ref agencyCode, ref userId, ref fee, ref from, ref to);

                ds = RecordsetToDataset(rs, "BookingFee");
            }
            catch
            { }
            finally
            {
                if (objMiscellaneous != null)
                {
                    Marshal.FinalReleaseComObject(objMiscellaneous);
                    objMiscellaneous = null;
                }

                ClearRecordset(ref rs);
            }

            return ds;
        }

        public DataSet CreditCardPayment(string ccNumber,
                                         string transType,
                                         string transStatus,
                                         DateTime from,
                                         DateTime to,
                                         string ccType,
                                         string agency)
        {
            tikAeroProcess.Payment objPayment = null;
            ADODB.Recordset rs = null;
            DataSet ds = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Payment", _server);
                    objPayment = (tikAeroProcess.Payment)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objPayment = new tikAeroProcess.Payment(); }

                rs = objPayment.CreditCardPayment(ref ccNumber, ref transType, ref transStatus, ref from, ref to, ref ccType, ref agency);

                ds = RecordsetToDataset(rs, "CreditCardPayment");
            }
            catch
            { }
            finally
            {
                if (objPayment != null)
                {
                    Marshal.FinalReleaseComObject(objPayment);
                    objPayment = null;
                }

                ClearRecordset(ref rs);

            }

            return ds;
        }

        public bool SaveVoucher(DataTable voucher)
        {
            tikAeroProcess.Miscellaneous objMiscellaneous = null;
            ADODB.Recordset rs = null;
            ADODB.Recordset rsRefundVoucher = null;

            bool bResult = false;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Miscellaneous", _server);
                    objMiscellaneous = (tikAeroProcess.Miscellaneous)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objMiscellaneous = new tikAeroProcess.Miscellaneous(); }

                if (voucher != null && voucher.Rows.Count > 0)
                {
                    ComplusMiscellaneous objComplus = new ComplusMiscellaneous();
                    double dblVoucherNumber = 0;

                    //Get Empty recordset.
                    rsRefundVoucher = ADODBReadVoucher("", ref dblVoucherNumber);

                    //Fill datatable into recordset,
                    DatasetToRecordset(voucher, ref rsRefundVoucher);

                    //Call save function after convert record type.
                    bResult = objMiscellaneous.SaveVoucher(rsRefundVoucher);
                }
            }
            catch
            { }
            finally
            {
                if (objMiscellaneous != null)
                {
                    Marshal.FinalReleaseComObject(objMiscellaneous);
                    objMiscellaneous = null;
                }

                ClearRecordset(ref rs);
                ClearRecordset(ref rsRefundVoucher);
            }

            return bResult;
        }

        public ADODB.Recordset ADODBReadVoucher(string voucherId, ref double voucherNumber)
        {
            tikAeroProcess.Miscellaneous objMiscellaneous = null;
            ADODB.Recordset rs = null;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Miscellaneous", _server);
                    objMiscellaneous = (tikAeroProcess.Miscellaneous)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objMiscellaneous = new tikAeroProcess.Miscellaneous(); }

                //Read voucher information
                rs = objMiscellaneous.ReadVoucher(voucherId, ref voucherNumber);
            }
            catch
            {
                ClearRecordset(ref rs);
            }
            finally
            {
                if (objMiscellaneous != null)
                {
                    Marshal.FinalReleaseComObject(objMiscellaneous);
                    objMiscellaneous = null;
                }
            }

            return rs;
        }

        public DataSet ReadVoucher(string voucherId, ref double voucherNumber)
        {
            DataSet ds = null;
            ADODB.Recordset rs = null;
            try
            {
                //Read voucher information
                rs = ADODBReadVoucher(voucherId, ref voucherNumber);
                ds = RecordsetToDataset(rs, "Details");
                if (ds != null && ds.Tables.Count > 0)
                {
                    ds.DataSetName = "Voucher";
                }
            }
            catch
            { }
            finally
            {
                ClearRecordset(ref rs);
            }

            return ds;
        }

        public bool VoidVoucher(Guid voucherId, Guid userId, DateTime voidDate)
        {
            tikAeroProcess.Miscellaneous objMiscellaneous = null;

            bool bResult = false;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Miscellaneous", _server);
                    objMiscellaneous = (tikAeroProcess.Miscellaneous)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objMiscellaneous = new tikAeroProcess.Miscellaneous(); }

                if (voucherId.Equals(Guid.Empty) == false && userId.Equals(Guid.Empty) == false && voidDate.Equals(DateTime.MinValue) == false)
                {
                    objMiscellaneous.VoidVoucher(voucherId.ToString(), userId.ToString(), voidDate);
                    bResult = true;
                }
            }
            catch
            { }
            finally
            {
                if (objMiscellaneous != null)
                {
                    Marshal.FinalReleaseComObject(objMiscellaneous);
                    objMiscellaneous = null;
                }
            }

            return bResult;
        }
    }
}