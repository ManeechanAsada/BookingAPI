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
    public class ComplusPayment : RunComplus
    {
        public ComplusPayment() : base() { }

        public DataSet GetFormOfPayments(string language)
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
                //set default language to EN
                if (language.Length == 0)
                { language = "EN"; }
                rs = objPayment.GetFormOfPayments(ref language);
                ds = RecordsetToDataset(rs, "GetFormOfPayments");
                ds.DataSetName = "FormOfPayments";
            }
            catch
            { }
            finally
            {
                if (objPayment != null)
                { Marshal.FinalReleaseComObject(objPayment); }
                objPayment = null;

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet GetFormOfPaymentSubTypes(string type, string language)
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

                //set default language to EN
                if (language.Length == 0)
                { language = "EN"; }
                rs = objPayment.GetFormOfPaymentSubTypes(ref type, ref language);
                ds = RecordsetToDataset(rs, "GetFormOfPayments");
                ds.DataSetName = "FormOfPayments";
            }
            catch
            { }
            finally
            {
                if (objPayment != null)
                { Marshal.FinalReleaseComObject(objPayment); }
                objPayment = null;

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public bool PaymentSave(DataTable payment, DataTable allocation, DataTable refundVoucher)
        {
            tikAeroProcess.Payment objPayment = null;

            ADODB.Recordset rsPayment = null;
            ADODB.Recordset rsAllocation = FabricatePaymentAllocationRecordset();
            ADODB.Recordset rsRefundVoucher = null;

            bool result = false;

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

                rsPayment = objPayment.GetEmpty();

                //Get Empty recordset for voucher.
                if (refundVoucher != null && refundVoucher.Rows.Count > 0)
                {
                    ComplusMiscellaneous objComplus = new ComplusMiscellaneous();
                    double dblVoucherNumber = 0;

                    //Get Empty recordset.
                    rsRefundVoucher = objComplus.ADODBReadVoucher("", ref dblVoucherNumber);

                    //Fill datatable into recordset,
                    DatasetToRecordset(refundVoucher, ref rsRefundVoucher);

                }
                DatasetToRecordset(payment, ref rsPayment);
                DatasetToRecordset(allocation, ref rsAllocation);

                result = objPayment.Save(ref rsPayment, ref rsAllocation, ref rsRefundVoucher);
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

                ClearRecordset(ref rsPayment);
                ClearRecordset(ref rsAllocation);
                ClearRecordset(ref rsRefundVoucher);
            }

            return result;
        }
        public DataSet PaymentVoucherCreditCard(DataTable payment, DataTable allocation, DataTable refundVoucher)
        {
            DataSet dsResult = null;
            tikAeroProcess.Payment objPayment = null;
            tikAeroProcess.clsCreditCard objCreditCard = null;

            ADODB.Recordset rsPayment = null;
            ADODB.Recordset rsAllocation = FabricatePaymentAllocationRecordset();
            ADODB.Recordset rsRefundVoucher = null;

            string securityToken = string.Empty;
            string authenticationToken = string.Empty;
            string commerceIndicator = string.Empty;
            string bookingReference = string.Empty;
            string strRequestSource = string.Empty;
            bool bWeb = true;            

            try
            {
                if (_server.Length > 0)
                {
                    //Create credit card object
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.clsCreditCard", _server);
                    objCreditCard = (tikAeroProcess.clsCreditCard)Activator.CreateInstance(remote);
                    remote = null;

                    remote = Type.GetTypeFromProgID("tikAeroProcess.Payment", _server);
                    objPayment = (tikAeroProcess.Payment)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                {
                    objCreditCard = new tikAeroProcess.clsCreditCard();
                    objPayment = new tikAeroProcess.Payment(); 
                }

                rsPayment = objPayment.GetEmpty();

                //Get Empty recordset for voucher.
                if (refundVoucher != null && refundVoucher.Rows.Count > 0)
                {
                    ComplusMiscellaneous objComplus = new ComplusMiscellaneous();
                    double dblVoucherNumber = 0;

                    //Get Empty recordset.
                    rsRefundVoucher = objComplus.ADODBReadVoucher("", ref dblVoucherNumber);

                    //Fill datatable into recordset,
                    DatasetToRecordset(refundVoucher, ref rsRefundVoucher);

                }
                DatasetToRecordset(payment, ref rsPayment);
                DatasetToRecordset(allocation, ref rsAllocation);

                //Call to validate credit card payment.
                ADODB.Recordset rs = objCreditCard.Authorize(rsPayment,
                                                            null,
                                                            ref rsAllocation,
                                                            ref rsRefundVoucher,
                                                            null,
                                                            null,
                                                            null,
                                                            null,
                                                            ref securityToken,
                                                            ref authenticationToken,
                                                            ref commerceIndicator,
                                                            ref bookingReference,
                                                            ref strRequestSource,
                                                            ref bWeb);

                dsResult = RecordsetToDataset(rs, "Payments");
            }
            catch
            { }
            finally
            {
                if (objCreditCard != null)
                {
                    Marshal.FinalReleaseComObject(objCreditCard);
                    objCreditCard = null;
                }

                if (objPayment != null)
                {
                    Marshal.FinalReleaseComObject(objPayment);
                    objPayment = null;
                }

                ClearRecordset(ref rsPayment);
                ClearRecordset(ref rsAllocation);
                ClearRecordset(ref rsRefundVoucher);
            }

            return dsResult;
        }
        public DataSet GetFormOfPaymentSubtypeFees(string formOfPayment,
                                                   string formOfPaymentSubtype,
                                                   string currencyRcd,
                                                   string agency,
                                                   DateTime feeDate)
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

                rs = objPayment.GetFormOfPaymentSubtypeFees(ref formOfPayment, ref formOfPaymentSubtype, ref currencyRcd, ref agency, ref feeDate);
                ds = RecordsetToDataset(rs, "Fee");
                ds.DataSetName = "FormOfPaymentSubtypeFees";
            }
            catch
            {
                throw;
            }
            finally
            {
                if (objPayment != null)
                { Marshal.FinalReleaseComObject(objPayment); }
                objPayment = null;

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public double CalculateExchange(string currencyFrom, string currencyTo, double amount, string systemCurrency, DateTime dateOfExchange, bool reverse)
        {
            tikAeroProcess.Payment objPayment = null;

            double result = 0;
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

                result = objPayment.CalculateExchange(ref currencyFrom,
                                                    ref currencyTo,
                                                    ref amount,
                                                    ref systemCurrency,
                                                    ref dateOfExchange,
                                                    ref reverse);
            }
            catch
            { }
            finally
            {
                if (objPayment != null)
                { Marshal.FinalReleaseComObject(objPayment); }
                objPayment = null;
            }

            return result;
        }
        public DataSet GetPaymentApprovals(DateTime paymentFrom,
                                          DateTime paymentTo,
                                          string documentFirst,
                                          string documentlast,
                                          string approvalCode,
                                          string paymentReference,
                                          string bookingReference,
                                          string nameOnCard,
                                          string status,
                                          string type,
                                          string error,
                                          string subTypes,
                                          int paymentNumber,
                                          double paymentAmount,
                                          bool excludeSubTypes)
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

                rs = objPayment.GetPaymentApprovals(ref paymentFrom,
                                                    ref paymentTo,
                                                    ref documentFirst,
                                                    ref documentlast,
                                                    ref approvalCode,
                                                    ref paymentReference,
                                                    ref bookingReference,
                                                    ref nameOnCard,
                                                    ref status,
                                                    ref type,
                                                    ref  error,
                                                    ref subTypes,
                                                    ref paymentNumber,
                                                    ref paymentAmount,
                                                    ref excludeSubTypes);

                ds = RecordsetToDataset(rs, "PaymentApprovals");
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
        public DataSet GetCashbookPayments(string agencyCode,
                                           string userId,
                                           DateTime paymentFrom,
                                           DateTime paymentTo,
                                           string type,
                                           string documentNumber,
                                           string approvalCode,
                                           string paymentReference,
                                           string bookingReference,
                                           string subTypes,
                                           string currencyCode,
                                           string language,
                                           string cashbookId,
                                           int paymentNumber,
                                           bool allPayments,
                                           bool allForms,
                                           bool excludeSubTypes,
                                           bool timeRange)
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
                //set default language to EN
                if (language.Length == 0)
                { language = "EN"; }
                rs = null;
                //rs = objPayment.GetCashbookPayments(ref agencyCode, 
                //                                    ref userId, 
                //                                    ref paymentFrom, 
                //                                    ref paymentTo, 
                //                                    ref type, 
                //                                    ref documentNumber, 
                //                                    ref approvalCode, 
                //                                    ref paymentReference, 
                //                                    ref bookingReference, 
                //                                    ref subTypes, 
                //                                    ref currencyCode, 
                //                                    ref language, 
                //                                    ref cashbookId, 
                //                                    ref paymentNumber, 
                //                                    ref allPayments, 
                //                                    ref allForms, 
                //                                    ref excludeSubTypes, 
                //                                    ref timeRange);

                ds = RecordsetToDataset(rs, "CashbookPayments");
            }
            catch
            { }
            finally
            {
                if (objPayment != null)
                { Marshal.FinalReleaseComObject(objPayment); }
                objPayment = null;

                ClearRecordset(ref rs);
            }

            return ds;
        }

        public DataSet GetCashbookCharges(DataTable dt, string strCashBookId)
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


                //Convert Datatable to recordset
                DatasetToRecordset(dt, ref rs);
                ADODB.Recordset rsCashBookCharge = objPayment.GetCashbookCharges(ref rs, ref strCashBookId);
                ds = RecordsetToDataset(rsCashBookCharge, "CashbookCharges");

                if (rsCashBookCharge != null)
                {
                    Marshal.FinalReleaseComObject(rsCashBookCharge);
                }

            }
            catch
            { }
            finally
            {
                if (objPayment != null)
                { Marshal.FinalReleaseComObject(objPayment); }

                ClearRecordset(ref rs);
            }

            return ds;
        }
        public DataSet GetServiceFees(string origin,
                                      string destination,
                                      string currency,
                                      string agency,
                                      string serviceGroup,
                                      DateTime dtFee)
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

                rs = objPayment.GetServiceFees(ref origin, ref destination, ref currency, ref agency, ref serviceGroup, ref dtFee);
                ds = RecordsetToDataset(rs, "ServiceFees");
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
        public DataSet GetOutstanding(string agencyCode,
                                   string airline,
                                   string flightNumber,
                                   DateTime flightFrom,
                                   DateTime flightTo,
                                   string origin,
                                   string destination,
                                   bool offices,
                                   bool agencies,
                                   bool lastTwentyFourHours,
                                   bool ticketedOnly,
                                   short olderThanHours,
                                   string language,
                                   bool accountsPayable)
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

                rs = objPayment.GetOutstanding(ref agencyCode,
                                                ref airline,
                                                ref flightNumber,
                                                ref flightFrom,
                                                ref flightTo,
                                                ref origin,
                                                ref destination,
                                                ref offices,
                                                ref agencies,
                                                ref lastTwentyFourHours,
                                                ref ticketedOnly,
                                                ref olderThanHours,
                                                ref language,
                                                ref accountsPayable);

                ds = RecordsetToDataset(rs, "Outstanding");
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

        public Currencies GetCurrencies(string strLanguage)
        {
            
            tikAeroProcess.Payment objPayment = null;
            ADODB.Recordset rs = null;
            Currencies currencies = new Currencies();
            try
            {
                
                if (string.IsNullOrEmpty(_server) == false)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Payment", _server);
                    objPayment = (tikAeroProcess.Payment)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objPayment = new tikAeroProcess.Payment(); }

                rs = objPayment.GetCurrencies(strLanguage);

                //convert Recordset to Object
                if (rs != null && rs.RecordCount > 0)
                {
                    Currency c;
                    rs.MoveFirst();
                    while (!rs.EOF)
                    {
                        c = new Currency();
                        c.currency_rcd = RecordsetHelper.ToString(rs, "currency_rcd");
                        c.currency_number = RecordsetHelper.ToString(rs, "currency_number");
                        c.display_name = RecordsetHelper.ToString(rs, "display_name");
                        c.max_voucher_value = RecordsetHelper.ToDecimal(rs, "max_voucher_value");
                        c.rounding_rule = RecordsetHelper.ToDecimal(rs, "rounding_rule");
                        c.number_of_decimals = RecordsetHelper.ToInt16(rs, "number_of_decimals");
                        currencies.Add(c);
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
                if (objPayment != null)
                {
                    Marshal.FinalReleaseComObject(objPayment);
                    objPayment = null;
                }
                RecordsetHelper.ClearRecordset(ref rs);
                base.Dispose();
            }
            return currencies;
        }
    }
}