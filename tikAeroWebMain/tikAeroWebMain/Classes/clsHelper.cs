using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Data;
using System.Reflection;
using tikSystem.Web.Library;
using Avantik.Web.Service.COMHelper;

namespace tikAeroWebMain
{
    public class Helper
    {
        public enum xmlReturnType
        {
            value = 0,
            OuterXml = 1,
            InnerXml = 2,
        }
        public string getXPathNodevalue(XPathNavigator n, string SelectExpression, xmlReturnType rType)
        {
            XPathNodeIterator ni = n.Select(SelectExpression);
            string result = string.Empty;

            while (ni.MoveNext())
            {
                if (rType == xmlReturnType.value)
                { result = ni.Current.Value; }
                else if (rType == xmlReturnType.InnerXml)
                { result = ni.Current.InnerXml; }
                else
                { result = ni.Current.OuterXml; }
            }
            ni = null;
            return result;
        }
        public string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            MemoryStream ms = new MemoryStream();
            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }

            ms.Position = 0;
            MemoryStream outStream = new MemoryStream();

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];
            System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return Convert.ToBase64String(gzBuffer);
        }
        public void SaveLog(string strFunctionName, DateTime dtStart, DateTime dtEnd, string strMessage, string strInput)
        {
            if (strMessage.Length > 0)
            {
                //string strPath = HttpContext.Current.Server.MapPath("~") + @"\Log\WebMain_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
                string strPath = HttpContext.Current.Server.MapPath("~") + @"\Log\WebMain_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
                StringBuilder stb = new StringBuilder();
                StreamWriter stw = null;
                try
                {
                    using (stw = new StreamWriter(strPath, true))
                    {
                        stb.Append("------------------------------(" + strFunctionName + ")" + Environment.NewLine);
                        stb.Append("******Start " + String.Format("{0:dd/MM/yyyy hh:mm:ss}", dtStart) + Environment.NewLine);
                        stb.Append("******End " + String.Format("{0:dd/MM/yyyy hh:mm:ss}", dtEnd) + Environment.NewLine);
                        stb.Append("******Message" + Environment.NewLine);
                        stb.Append(strMessage + Environment.NewLine);
                        stb.Append("Input Data" + Environment.NewLine);
                        stb.Append(strInput + Environment.NewLine);

                        stw.WriteLine(stb.ToString());
                        stw.Flush();
                    }
                }
                catch
                {
                    if (stw != null)
                    {
                        stw.Close();
                    }
                }
            }
        }
        public string DecompressString(string compressedText)
        {
            byte[] gzBuffer = Convert.FromBase64String(compressedText);
            using (MemoryStream ms = new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                byte[] buffer = new byte[msgLength];

                ms.Position = 0;
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        public string EncryptString(string inputString, string filePath)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string fileString = null;
            fileString = streamReader.ReadToEnd();
            string bitStrengthString = fileString.Substring(0, fileString.IndexOf("</BitStrength>") + 14);
            fileString = fileString.Replace(bitStrengthString, "");
            int bitStrength = Convert.ToInt32(bitStrengthString.Replace("<BitStrength>", "").Replace("</BitStrength>", ""));

            //TODO: Add Proper Exception Handlers
            RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(bitStrength);
            rsaCryptoServiceProvider.FromXmlString(fileString);
            int keySize = bitStrength / 8;
            byte[] bytes = Encoding.UTF32.GetBytes(inputString);
            // The hash function in use by the .NET RSACryptoServiceProvider here is SHA1
            // int maxLength = ( keySize ) - 2 - ( 2 * SHA1.Create().ComputeHash( rawBytes ).Length );
            int maxLength = keySize - 42;
            int dataLength = bytes.Length;
            int iterations = dataLength / maxLength;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i <= iterations; i++)
            {
                byte[] tempBytes = new byte[(dataLength - maxLength * i > maxLength) ? maxLength : dataLength - maxLength * i];
                Buffer.BlockCopy(bytes, maxLength * i, tempBytes, 0, tempBytes.Length);
                byte[] encryptedBytes = rsaCryptoServiceProvider.Encrypt(tempBytes, true);
                // Be aware the RSACryptoServiceProvider reverses the order of encrypted bytes after encryption and before decryption.
                // If you do not require compatibility with Microsoft Cryptographic API (CAPI) and/or other vendors.
                // Comment out the next line and the corresponding one in the DecryptString function.
                Array.Reverse(encryptedBytes);
                // Why convert to base 64?
                // Because it is the largest power-of-two base printable using only ASCII characters
                stringBuilder.Append(Convert.ToBase64String(encryptedBytes));
            }
            return stringBuilder.ToString();
        }

        public string DecryptString(string inputString, string filePath)
        {

            StreamReader streamReader = new StreamReader(filePath);
            try
            {
                string fileString = null;
                fileString = streamReader.ReadToEnd();
                string bitStrengthString = fileString.Substring(0, fileString.IndexOf("</BitStrength>") + 14);
                fileString = fileString.Replace(bitStrengthString, "");
                int bitStrength = Convert.ToInt32(bitStrengthString.Replace("<BitStrength>", "").Replace("</BitStrength>", ""));

                // TODO: Add Proper Exception Handlers
                RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(bitStrength);
                rsaCryptoServiceProvider.FromXmlString(fileString);
                int base64BlockSize = ((bitStrength / 8) % 3 != 0) ? (((bitStrength / 8) / 3) * 4) + 4 : ((bitStrength / 8) / 3) * 4;
                int iterations = inputString.Length / base64BlockSize;
                ArrayList arrayList = new ArrayList();
                for (int i = 0; i < iterations; i++)
                {
                    byte[] encryptedBytes = Convert.FromBase64String(inputString.Substring(base64BlockSize * i, base64BlockSize));
                    // Be aware the RSACryptoServiceProvider reverses the order of encrypted bytes after encryption and before decryption.
                    // If you do not require compatibility with Microsoft Cryptographic API (CAPI) and/or other vendors.
                    // Comment out the next line and the corresponding one in the EncryptString function.
                    Array.Reverse(encryptedBytes);
                    arrayList.AddRange(rsaCryptoServiceProvider.Decrypt(encryptedBytes, true));
                }
                return Encoding.UTF32.GetString(arrayList.ToArray(Type.GetType("System.Byte")) as byte[]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DecryptCCValue(DataSet ds, string filePath)
        {
            if (ds != null && ds.Tables.Count > 0)
            {   
                foreach(DataRow dr in ds.Tables["Payment"].Rows)
                {
                    dr["document_number"] = DecryptString(dr["document_number"].ToString(), filePath);
                    if (dr["cvv_code"].ToString().Length > 0)
                    { dr["cvv_code"] = DecryptString(dr["cvv_code"].ToString(), filePath); }
                    
                }   
            }
        }

        public void CreateErrorDataset(ref DataSet ds, string strCode, string strMessage, string DataSetname, string tableName)
        {
            if (ds == null)
            {
                ds = new DataSet();
            }

            if (((ds == null) == false))
            {
                if (string.IsNullOrEmpty(DataSetname) == false)
                {
                    ds.DataSetName = DataSetname;
                }
                if ((ds.Tables[tableName] == null) == true)
                {
                    DataTable dt = new DataTable();

                    DataColumn dcErrorCode = null;
                    DataColumn dcResponseCode = null;
                    DataColumn dcMessage = null;

                    //Set Table name
                    dt.TableName = tableName;

                    //Initialize the columns
                    dcErrorCode = new DataColumn("ErrorCode");
                    dcResponseCode = new DataColumn("ResponseCode");
                    dcMessage = new DataColumn("ResponseText");

                    //Add them  to your DataTable
                    dt.Columns.Add(dcErrorCode);
                    dt.Columns.Add(dcResponseCode);
                    dt.Columns.Add(dcMessage);

                    //Add the DataTable to your DataSet
                    ds.Tables.Add(dt);

                    dcErrorCode.Dispose();
                    dcMessage.Dispose();
                }

                DataRow drError = ds.Tables[tableName].NewRow();

                drError["ErrorCode"] = strCode;
                drError["ResponseCode"] = strMessage;
                drError["ResponseText"] = strMessage;

                ds.Tables[tableName].Rows.Add(drError);
            }
        }

        #region Recordset Helper
        public ADODB.Recordset FabricatePaymentAllocationRecordset()
        {
            ADODB.Recordset rs = new ADODB.Recordset();

            rs.Fields.Append("booking_payment_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passenger_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_segment_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("currency_rcd", ADODB.DataTypeEnum.adVarChar, 20, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_fee_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("voucher_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fee_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("user_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("sales_amount", ADODB.DataTypeEnum.adDouble, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("payment_amount", ADODB.DataTypeEnum.adDouble, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("account_amount", ADODB.DataTypeEnum.adDouble, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);

            rs.Fields.Append("passenger_segment_service_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("od_origin_rcd", ADODB.DataTypeEnum.adVarChar, 5, ADODB.FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("od_destination_rcd", ADODB.DataTypeEnum.adVarChar, 5, ADODB.FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("charge_amount", ADODB.DataTypeEnum.adDouble, 0, ADODB.FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("charge_amount_incl", ADODB.DataTypeEnum.adDouble, 0, ADODB.FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("charge_currency_rcd", ADODB.DataTypeEnum.adVarChar, 10, ADODB.FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("fee_category_rcd", ADODB.DataTypeEnum.adVarChar, 5, ADODB.FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("external_reference", ADODB.DataTypeEnum.adVarChar, 50, ADODB.FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("vendor_rcd", ADODB.DataTypeEnum.adVarChar, 50, ADODB.FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("weight_lbs", ADODB.DataTypeEnum.adNumeric, 0, ADODB.FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("weight_kgs", ADODB.DataTypeEnum.adNumeric, 0, ADODB.FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("units", ADODB.DataTypeEnum.adNumeric, 0, ADODB.FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("create_by", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldIsNullable, null);

            //Open Recordset
            rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient;
            rs.Open(System.Reflection.Missing.Value,
                    System.Reflection.Missing.Value,
                    ADODB.CursorTypeEnum.adOpenStatic,
                    ADODB.LockTypeEnum.adLockOptimistic, 0);

            return rs;
        }

        public Booking RecordsetToBooking(ADODB.Recordset rsHeader,
                                          ADODB.Recordset rsItinerary,
                                          ADODB.Recordset rsPassengers,
                                          ADODB.Recordset rsMappings,
                                          ADODB.Recordset rsQuotes,
                                          ADODB.Recordset rsServices,
                                          ADODB.Recordset rsRemarks,
                                          ADODB.Recordset rsPayments)
        {
            Booking objBooking = null;

            #region Header
            if (rsHeader != null)
            {
                if (objBooking == null)
                { objBooking = new Booking(); }
            }
            #endregion

            #region Flight Segment
            if (rsItinerary != null && rsItinerary.RecordCount > 0)
            {
                if (objBooking == null)
                { objBooking = new Booking(); }

                objBooking.Itinerary = new Itinerary();
                FlightSegment sm;
                rsItinerary.Filter = 0;
                while (!rsItinerary.EOF)
                {
                    sm = new FlightSegment();

                    sm.flight_id = RecordsetHelper.ToGuid(rsItinerary, "flight_id");
                    sm.fare_id = RecordsetHelper.ToGuid(rsItinerary, "fare_id");
                    sm.origin_rcd = RecordsetHelper.ToString(rsItinerary, "origin_rcd");
                    sm.destination_rcd = RecordsetHelper.ToString(rsItinerary, "destination_rcd");
                    sm.booking_class_rcd = RecordsetHelper.ToString(rsItinerary, "booking_class_rcd");
                    sm.departure_date = RecordsetHelper.ToDateTime(rsItinerary, "departure_date");
                    sm.fare_reduction = RecordsetHelper.ToByte(rsItinerary, "fare_reduction");
                    sm.waitlist_flag = RecordsetHelper.ToByte(rsItinerary, "waitlist_flag");
                    sm.refundable_flag = RecordsetHelper.ToByte(rsItinerary, "refundable_flag");
                    sm.non_revenue_flag = RecordsetHelper.ToByte(rsItinerary, "non_revenue_flag");
                    sm.eticket_flag = RecordsetHelper.ToByte(rsItinerary, "eticket_flag");
                    sm.group_flag = RecordsetHelper.ToByte(rsItinerary, "group_flag");
                    sm.exclude_quote_flag = RecordsetHelper.ToByte(rsItinerary, "exclude_quote_flag");
                    sm.advanced_purchase_flag = RecordsetHelper.ToByte(rsItinerary, "advanced_purchase_flag");
                    sm.od_origin_rcd = RecordsetHelper.ToString(rsItinerary, "od_origin_rcd");
                    sm.od_destination_rcd = RecordsetHelper.ToString(rsItinerary, "od_destination_rcd");
                    sm.create_by = RecordsetHelper.ToGuid(rsItinerary, "create_by");
                    sm.create_date_time = RecordsetHelper.ToDateTime(rsItinerary, "create_date_time");
                    sm.update_by = RecordsetHelper.ToGuid(rsItinerary, "update_by");
                    sm.update_date_time = RecordsetHelper.ToDateTime(rsItinerary, "update_date_time");

                    objBooking.Itinerary.Add(sm);
                    rsItinerary.MoveNext();
                }
            }
            #endregion

            #region Passenger
            if (rsPassengers != null && rsPassengers.RecordCount > 0)
            {
                if (objBooking == null)
                { objBooking = new Booking(); }

                objBooking.Passengers = new Passengers();
                Passenger p;
                rsPassengers.Filter = 0;
                while (!rsPassengers.EOF)
                {
                    p = new Passenger();

                    p.passenger_id = RecordsetHelper.ToGuid(rsPassengers, "passenger_id");
                    p.passenger_type_rcd = RecordsetHelper.ToString(rsPassengers, "passenger_type_rcd");
                    p.create_by = RecordsetHelper.ToGuid(rsPassengers, "create_by");
                    p.create_date_time = RecordsetHelper.ToDateTime(rsPassengers, "create_date_time");
                    p.update_by = RecordsetHelper.ToGuid(rsPassengers, "update_by");
                    p.update_date_time = RecordsetHelper.ToDateTime(rsPassengers, "update_date_time");

                    objBooking.Passengers.Add(p);
                    rsPassengers.MoveNext();
                }
            }
            #endregion

            #region Mappings
            if (rsMappings != null && rsMappings.RecordCount > 0)
            {
                if (objBooking == null)
                { objBooking = new Booking(); }

                objBooking.Mappings = new Mappings();
                Mapping m;
                rsMappings.Filter = 0;
                while (!rsMappings.EOF)
                {
                    m = new Mapping();

                    m.passenger_id = RecordsetHelper.ToGuid(rsMappings, "passenger_id");
                    m.booking_segment_id = RecordsetHelper.ToGuid(rsMappings, "booking_segment_id");
                    m.origin_rcd = RecordsetHelper.ToString(rsMappings, "origin_rcd");
                    m.destination_rcd = RecordsetHelper.ToString(rsMappings, "destination_rcd");
                    m.passenger_type_rcd = RecordsetHelper.ToString(rsMappings, "passenger_type_rcd");
                    m.flight_id = RecordsetHelper.ToGuid(rsMappings, "flight_id");
                    m.departure_date = RecordsetHelper.ToDateTime(rsMappings, "departure_date");
                    m.boarding_class_rcd = RecordsetHelper.ToString(rsMappings, "boarding_class_rcd");
                    m.booking_class_rcd = RecordsetHelper.ToString(rsMappings, "booking_class_rcd");
                    m.currency_rcd = RecordsetHelper.ToString(rsMappings, "currency_rcd");
                    m.net_total = RecordsetHelper.ToDecimal(rsMappings, "net_total");
                    m.tax_amount = RecordsetHelper.ToDecimal(rsMappings, "tax_amount");
                    m.acct_tax_amount_incl = RecordsetHelper.ToDecimal(rsMappings, "acct_tax_amount_incl");
                    m.fare_amount = RecordsetHelper.ToDecimal(rsMappings, "fare_amount");
                    m.fare_amount_incl = RecordsetHelper.ToDecimal(rsMappings, "fare_amount_incl");
                    m.yq_amount = RecordsetHelper.ToDecimal(rsMappings, "yq_amount");
                    m.yq_amount_incl = RecordsetHelper.ToDecimal(rsMappings, "yq_amount_incl");
                    m.ticketing_fee_amount = RecordsetHelper.ToDecimal(rsMappings, "ticketing_fee_amount");
                    m.ticketing_fee_amount_incl = RecordsetHelper.ToDecimal(rsMappings, "ticketing_fee_amount_incl");
                    m.reservation_fee_amount = RecordsetHelper.ToDecimal(rsMappings, "reservation_fee_amount");
                    m.commission_amount = RecordsetHelper.ToDecimal(rsMappings, "commission_amount");
                    m.commission_amount_incl = RecordsetHelper.ToDecimal(rsMappings, "commission_amount_incl");
                    m.commission_percentage = RecordsetHelper.ToDecimal(rsMappings, "commission_percentage");
                    m.refund_charge = RecordsetHelper.ToDecimal(rsMappings, "refund_charge");
                    m.fare_vat = RecordsetHelper.ToDecimal(rsMappings, "fare_vat");
                    m.tax_vat = RecordsetHelper.ToDecimal(rsMappings, "tax_vat");
                    m.yq_vat = RecordsetHelper.ToDecimal(rsMappings, "yq_vat");
                    m.ticketing_fee_vat = RecordsetHelper.ToDecimal(rsMappings, "ticketing_fee_vat");
                    m.reservation_fee_vat = RecordsetHelper.ToDecimal(rsMappings, "reservation_fee_vat");
                    m.od_origin_rcd = RecordsetHelper.ToString(rsMappings, "od_origin_rcd");
                    m.od_destination_rcd = RecordsetHelper.ToString(rsMappings, "od_destination_rcd");
                    m.fare_id = RecordsetHelper.ToGuid(rsMappings, "fare_id");
                    m.fare_code = RecordsetHelper.ToString(rsMappings, "fare_code");
                    m.fare_date_time = RecordsetHelper.ToDateTime(rsMappings, "fare_date_time");
                    m.refundable_flag = RecordsetHelper.ToByte(rsMappings, "refundable_flag");
                    m.transferable_fare_flag = RecordsetHelper.ToByte(rsMappings, "transferable_fare_flag");
                    m.refund_with_charge_hours = RecordsetHelper.ToInt16(rsMappings, "refund_with_charge_hours");
                    m.refund_not_possible_hours = RecordsetHelper.ToInt16(rsMappings, "refund_not_possible_hours");
                    m.duty_travel_flag = RecordsetHelper.ToByte(rsMappings, "duty_travel_flag");
                    m.baggage_weight = RecordsetHelper.ToDecimal(rsMappings, "baggage_weight");
                    m.piece_allowance = RecordsetHelper.ToInt16(rsMappings, "piece_allowance");
                    m.restriction_text = RecordsetHelper.ToString(rsMappings, "restriction_text");
                    m.endorsement_text = RecordsetHelper.ToString(rsMappings, "endorsement_text");
                    m.fare_type_rcd = RecordsetHelper.ToString(rsMappings, "fare_type_rcd");
                    m.redemption_points = RecordsetHelper.ToDouble(rsMappings, "redemption_points");

                    objBooking.Mappings.Add(m);
                    rsMappings.MoveNext();
                }
            }
            #endregion

            #region Quotes
            if (rsQuotes != null && rsQuotes.RecordCount > 0)
            {
                if (objBooking == null)
                { objBooking = new Booking(); }

                objBooking.Quotes = new Quotes();
                Quote q;
                rsQuotes.Filter = 0;
                while (!rsQuotes.EOF)
                {
                    q = new Quote();

                    q.passenger_type_rcd = RecordsetHelper.ToString(rsQuotes, "passenger_type_rcd");
                    q.passenger_count = RecordsetHelper.ToInt32(rsQuotes, "passenger_count");
                    q.currency_rcd = RecordsetHelper.ToString(rsQuotes, "currency_rcd");
                    q.charge_type = RecordsetHelper.ToString(rsQuotes, "charge_type");
                    q.charge_name = RecordsetHelper.ToString(rsQuotes, "charge_name");
                    q.charge_amount = RecordsetHelper.ToDecimal(rsQuotes, "charge_amount");
                    q.charge_amount_incl = RecordsetHelper.ToDecimal(rsQuotes, "charge_amount_incl");
                    q.total_amount = RecordsetHelper.ToDecimal(rsQuotes, "total_amount");
                    q.tax_amount = RecordsetHelper.ToDecimal(rsQuotes, "tax_amount");
                    q.redemption_points = RecordsetHelper.ToDecimal(rsQuotes, "redemption_points");
                    q.sort_sequence = RecordsetHelper.ToInt32(rsQuotes, "sort_sequence");

                    objBooking.Quotes.Add(q);
                    rsQuotes.MoveNext();
                }
            }
            #endregion

            #region Services
            if (rsServices != null)
            {
                if (objBooking == null)
                { objBooking = new Booking(); }
            }
            #endregion

            #region Remarks
            if (rsRemarks != null)
            {
                if (objBooking == null)
                { objBooking = new Booking(); }
            }
            #endregion

            #region Payment
            if (rsPayments != null)
            {
                if (objBooking == null)
                { objBooking = new Booking(); }
            }
            #endregion

            return objBooking;
        }
        public void RecordsetToXML(System.Xml.XmlWriter xtw,
                                   ADODB.Recordset rsHeader,
                                   ADODB.Recordset rsItinerary,
                                   ADODB.Recordset rsPassengers,
                                   ADODB.Recordset rsMappings,
                                   ADODB.Recordset rsQuotes,
                                   ADODB.Recordset rsServices,
                                   ADODB.Recordset rsRemarks,
                                   ADODB.Recordset rsPayments,
                                   ADODB.Recordset rsTaxes)
        {
            string strFieldName = string.Empty;
            string strValue = string.Empty;
            DateTime dtValue = DateTime.MinValue;
            #region Header
            if (rsHeader != null)
            {
            }
            #endregion

            #region Flight Segment
            if (rsItinerary != null && rsItinerary.RecordCount > 0)
            {
                rsItinerary.Filter = 0;

                while (!rsItinerary.EOF)
                {
                    xtw.WriteStartElement("FlightSegment");
                    {
                        WriteXmlElement(xtw, rsItinerary);
                    }
                    xtw.WriteEndElement();//FlightSegment
                    rsItinerary.MoveNext();
                }
            }
            #endregion

            #region Passenger
            if (rsPassengers != null && rsPassengers.RecordCount > 0)
            {
                rsPassengers.Filter = 0;
                while (!rsPassengers.EOF)
                {
                    xtw.WriteStartElement("Passenger");
                    {
                        WriteXmlElement(xtw, rsPassengers);
                    }
                    xtw.WriteEndElement();//Passenger
                    rsPassengers.MoveNext();
                }
            }
            #endregion

            #region Mappings
            if (rsMappings != null && rsMappings.RecordCount > 0)
            {
                rsMappings.Filter = 0;
                while (!rsMappings.EOF)
                {
                    xtw.WriteStartElement("Mapping");
                    {
                        WriteXmlElement(xtw, rsMappings);
                    }
                    xtw.WriteEndElement();//Mapping
                    rsMappings.MoveNext();
                }
            }
            #endregion

            #region Quotes
            if (rsQuotes != null && rsQuotes.RecordCount > 0)
            {
                rsQuotes.Filter = 0;
                while (!rsQuotes.EOF)
                {
                    xtw.WriteStartElement("Quote");
                    {
                        WriteXmlElement(xtw, rsQuotes);
                    }
                    xtw.WriteEndElement();//Quote
                    rsQuotes.MoveNext();
                }
            }
            #endregion

            #region Services
            if (rsServices != null)
            {
            }
            #endregion

            #region Remarks
            if (rsRemarks != null)
            {
            }
            #endregion

            #region Payment
            if (rsPayments != null)
            {
            }
            #endregion

            #region Taxes
            if (rsTaxes != null && rsTaxes.RecordCount > 0)
            {
                rsTaxes.Filter = 0;
                while (!rsTaxes.EOF)
                {
                    xtw.WriteStartElement("Tax");
                    {
                        WriteXmlElement(xtw, rsTaxes);
                    }
                    xtw.WriteEndElement();//Tax
                    rsTaxes.MoveNext();
                }
            }
            #endregion
        }

        private XmlWriter WriteXmlElement(XmlWriter xtw, ADODB.Recordset rs)
        {
            string strFieldName = string.Empty;

            if (rs != null && rs.RecordCount > 0)
            {
                for (int i = 0; i < rs.Fields.Count; i++)
                {
                    if (rs.Fields[i].Value != null)
                    {
                        strFieldName = rs.Fields[i].Name;
                        xtw.WriteStartElement(strFieldName);

                        switch (rs.Fields[i].Type)
                        {
                            case ADODB.DataTypeEnum.adDBTimeStamp:
                                xtw.WriteValue(RecordsetHelper.ToDateTime(rs, strFieldName));
                                break;

                            case ADODB.DataTypeEnum.adDBDate:
                                xtw.WriteValue(RecordsetHelper.ToDateTime(rs, strFieldName));
                                break;

                            case ADODB.DataTypeEnum.adDate:
                                xtw.WriteValue(RecordsetHelper.ToDateTime(rs, strFieldName));
                                break;

                            default:
                                xtw.WriteValue(RecordsetHelper.ToString(rs, strFieldName));
                                break;
                        }

                        xtw.WriteEndElement();
                    }
                }
            }
            return xtw;
        }

        #endregion
    }
}
