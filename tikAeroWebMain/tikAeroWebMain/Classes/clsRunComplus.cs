using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.OleDb;
using ADODB;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Security.Principal;
using System.Security.Permissions;

namespace tikAeroWebMain
{
    public class RunComplus : IDisposable
    {
        #region Impersonate Function
        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_PROVIDER_DEFAULT = 0;

        WindowsImpersonationContext impersonationContext;

        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);

        public RunComplus()
        {
            string strUserName = ConfigurationManager.AppSettings["ComUser"];
            string strPassword = ConfigurationManager.AppSettings["ComPassword"];
            string strDomain = ConfigurationManager.AppSettings["ComDomain"];
            
            if (strUserName.Length > 0 && strPassword.Length > 0 && strDomain.Length > 0)
            {
                impersonateValidUser(strUserName, strDomain, strPassword);
            }
        }
        private bool impersonateValidUser(String userName, String domain, String password)
        {
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                    LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        if (impersonationContext != null)
                        {
                            using (impersonationContext = tempWindowsIdentity.Impersonate())
                            {
                                CloseHandle(token);
                                CloseHandle(tokenDuplicate);
                                return true;
                            }
                        }
                    }
                }
            }
            if (token != IntPtr.Zero)
                CloseHandle(token);
            if (tokenDuplicate != IntPtr.Zero)
                CloseHandle(tokenDuplicate);
            return false;
        }

        private void undoImpersonation()
        {
            if (impersonationContext != null)
            {
                using (impersonationContext)
                {
                    impersonationContext.Undo();
                }
            }
        }

        #endregion
        #region Property
        protected string _server = ConfigurationManager.AppSettings["ComServer"];
        #endregion

        #region Helper Function
        public DataSet RecordsetToDataset(Recordset rs, string tableName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable(tableName);
            
            try
            {
                foreach (ADODB.Field fld in rs.Fields)
                {
                    Type ttype = fld.GetType();
                    DataColumn dc_column = new DataColumn("column");

                    switch (fld.Type)
                    {
                        case ADODB.DataTypeEnum.adNumeric:
                            dc_column.DataType = System.Type.GetType("System.Double");
                            break;
                        case ADODB.DataTypeEnum.adDouble:
                            dc_column.DataType = System.Type.GetType("System.Double");
                            break;
                        case ADODB.DataTypeEnum.adCurrency:
                            dc_column.DataType = System.Type.GetType("System.Decimal");
                            break;
                        case ADODB.DataTypeEnum.adChar:
                            dc_column.DataType = System.Type.GetType("System.Char");
                            break;
                        case ADODB.DataTypeEnum.adWChar:
                            dc_column.DataType = System.Type.GetType("System.String");
                            break;
                        case ADODB.DataTypeEnum.adVarChar:
                            dc_column.DataType = System.Type.GetType("System.String");
                            break;
                        case ADODB.DataTypeEnum.adLongVarChar:
                            dc_column.DataType = System.Type.GetType("System.String");
                            break;
                        case ADODB.DataTypeEnum.adVarWChar:
                            dc_column.DataType = System.Type.GetType("System.String");
                            break;
                        case ADODB.DataTypeEnum.adLongVarWChar:
                            dc_column.DataType = System.Type.GetType("System.String");
                            break;
                        case ADODB.DataTypeEnum.adBigInt:
                            dc_column.DataType = System.Type.GetType("System.Int64");
                            break;
                        case ADODB.DataTypeEnum.adInteger:
                            dc_column.DataType = System.Type.GetType("System.Int32");
                            break;
                        case ADODB.DataTypeEnum.adSmallInt:
                            dc_column.DataType = System.Type.GetType("System.Int16");
                            break;
                        case ADODB.DataTypeEnum.adUnsignedTinyInt:
                            dc_column.DataType = System.Type.GetType("System.Byte");
                            break;
                        case ADODB.DataTypeEnum.adTinyInt:
                            dc_column.DataType = System.Type.GetType("System.SByte");
                            break;
                        case ADODB.DataTypeEnum.adDBTimeStamp:
                            dc_column.DataType = System.Type.GetType("System.DateTime");
                            break;
                        case ADODB.DataTypeEnum.adDBDate:
                            dc_column.DataType = System.Type.GetType("System.DateTime");
                            break;
                        case ADODB.DataTypeEnum.adDate:
                            dc_column.DataType = System.Type.GetType("System.DateTime");
                            break;
                        case ADODB.DataTypeEnum.adGUID:
                            dc_column.DataType = System.Type.GetType("System.Guid");
                            break;
                        case ADODB.DataTypeEnum.adBoolean:
                            dc_column.DataType = System.Type.GetType("System.Boolean");
                            break;
                        case ADODB.DataTypeEnum.adSingle:
                            dc_column.DataType = System.Type.GetType("System.Single");
                            break;
                        default: 
                            System.Diagnostics.Debug.Assert(false);
                            break;
                    }
                    if (fld.Name == "passenger_role_rcd")
                        dt.Columns.Add(new DataColumn(fld.Name, dc_column.DataType));
                    else if (fld.Name == "vip_flag")
                        dt.Columns.Add(new DataColumn(fld.Name, dc_column.DataType));
                    else
                        dt.Columns.Add(new DataColumn(fld.Name, dc_column.DataType));

                    ttype = null;
                    if (dc_column != null)
                    {
                        dc_column.Dispose();
                        dc_column = null;
                    }
                }

                object[] vals = new object[rs.Fields.Count];

                rs.MoveFirst();
                while (!rs.EOF)
                {
                    int nFldCnt = 0;

                    foreach (ADODB.Field fld in rs.Fields)
                    {
                        vals[nFldCnt] = fld.Value;
                        nFldCnt += 1;
                    }

                    dt.Rows.Add(vals);

                    rs.MoveNext();
                }
                ds.Tables.Add(dt);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
            return ds;

        }
        public void RecordsetToDataset(DataSet ds, Recordset rs, string tableName)
        {
            DataTable dt = new DataTable(tableName);

            try
            {
                foreach (ADODB.Field fld in rs.Fields)
                {
                    Type ttype = fld.GetType();
                    DataColumn dc_column = new DataColumn("column");

                    switch (fld.Type)
                    {
                        case ADODB.DataTypeEnum.adNumeric:
                            dc_column.DataType = System.Type.GetType("System.Double");
                            break;
                        case ADODB.DataTypeEnum.adDouble:
                            dc_column.DataType = System.Type.GetType("System.Double");
                            break;
                        case ADODB.DataTypeEnum.adCurrency:
                            dc_column.DataType = System.Type.GetType("System.Decimal");
                            break;
                        case ADODB.DataTypeEnum.adChar:
                            dc_column.DataType = System.Type.GetType("System.Char");
                            break;
                        case ADODB.DataTypeEnum.adWChar:
                            dc_column.DataType = System.Type.GetType("System.String");
                            break;
                        case ADODB.DataTypeEnum.adVarChar:
                            dc_column.DataType = System.Type.GetType("System.String");
                            break;
                        case ADODB.DataTypeEnum.adLongVarChar:
                            dc_column.DataType = System.Type.GetType("System.String");
                            break;
                        case ADODB.DataTypeEnum.adVarWChar:
                            dc_column.DataType = System.Type.GetType("System.String");
                            break;
                        case ADODB.DataTypeEnum.adBigInt:
                            dc_column.DataType = System.Type.GetType("System.Int64");
                            break;
                        case ADODB.DataTypeEnum.adInteger:
                            dc_column.DataType = System.Type.GetType("System.Int32");
                            break;
                        case ADODB.DataTypeEnum.adSmallInt:
                            dc_column.DataType = System.Type.GetType("System.Int16");
                            break;
                        case ADODB.DataTypeEnum.adUnsignedTinyInt:
                            dc_column.DataType = System.Type.GetType("System.Byte");
                            break;
                        case ADODB.DataTypeEnum.adTinyInt:
                            dc_column.DataType = System.Type.GetType("System.SByte");
                            break;
                        case ADODB.DataTypeEnum.adDBTimeStamp:
                            dc_column.DataType = System.Type.GetType("System.DateTime");
                            break;
                        case ADODB.DataTypeEnum.adDBDate:
                            dc_column.DataType = System.Type.GetType("System.DateTime");
                            break;
                        case ADODB.DataTypeEnum.adDate:
                            dc_column.DataType = System.Type.GetType("System.DateTime");
                            break;
                        case ADODB.DataTypeEnum.adGUID:
                            dc_column.DataType = System.Type.GetType("System.Guid");
                            break;
                        case ADODB.DataTypeEnum.adBoolean:
                            dc_column.DataType = System.Type.GetType("System.Boolean");
                            break;
                        case ADODB.DataTypeEnum.adSingle:
                            dc_column.DataType = System.Type.GetType("System.Single");
                            break;
                        case ADODB.DataTypeEnum.adLongVarWChar:
                            dc_column.DataType = System.Type.GetType("System.String");
                            break;
                        default:
                            System.Diagnostics.Debug.Assert(false);
                            break;
                    }

                    dt.Columns.Add(new DataColumn(fld.Name, dc_column.DataType));

                    ttype = null;
                    if (dc_column != null)
                    {
                        dc_column.Dispose();
                        dc_column = null;
                    }
                }

                object[] vals = new object[rs.Fields.Count];
                rs.MoveFirst();
                while (!rs.EOF)
                {
                    int nFldCnt = 0;

                    foreach (ADODB.Field fld in rs.Fields)
                    {
                        vals[nFldCnt] = fld.Value;
                        nFldCnt += 1;
                    }

                    dt.Rows.Add(vals);

                    rs.MoveNext();
                }
                ds.Tables.Add(dt);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }
        public Recordset DatasetToRecordset(string xml)
        {
            if (xml.Length != 0)
            {
                DataSet ds = new DataSet();
                ds.ReadXml(new StringReader(xml));

                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    ADODB.Recordset result = new ADODB.Recordset();
                    result.CursorLocation = ADODB.CursorLocationEnum.adUseClient;

                    ADODB.Fields resultFields = result.Fields;
                    System.Data.DataColumnCollection inColumns = dt.Columns;

                    foreach (DataColumn inColumn in inColumns)
                    {
                        resultFields.Append(inColumn.ColumnName,
                                            TranslateType(inColumn.DataType),
                                            inColumn.MaxLength,
                                            inColumn.AllowDBNull ? ADODB.FieldAttributeEnum.adFldIsNullable :
                                                                   ADODB.FieldAttributeEnum.adFldUnspecified,
                                            null);
                    }

                    result.Open(System.Reflection.Missing.Value,
                                System.Reflection.Missing.Value,
                                ADODB.CursorTypeEnum.adOpenStatic,
                                ADODB.LockTypeEnum.adLockOptimistic, 0);

                    foreach (DataRow dr in dt.Rows)
                    {
                        result.AddNew(System.Reflection.Missing.Value,
                                      System.Reflection.Missing.Value);

                        for (int columnIndex = 0; columnIndex < inColumns.Count; columnIndex++)
                        {
                            resultFields[columnIndex].Value = dr[columnIndex];
                        }
                    }

                    inColumns = null;

                    if (dt != null)
                    {
                        dt.Dispose();
                    }

                    ds.Dispose();
                    resultFields = null;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
        public Recordset DatasetToRecordset(DataTable dt)
        {
            if (dt != null)
            {
                ADODB.Recordset result = new ADODB.Recordset();
                result.CursorLocation = ADODB.CursorLocationEnum.adUseClient;

                ADODB.Fields resultFields = result.Fields;
                System.Data.DataColumnCollection inColumns = dt.Columns;

                foreach (DataColumn inColumn in inColumns)
                {
                    resultFields.Append(inColumn.ColumnName,
                                        TranslateType(inColumn.DataType),
                                        inColumn.MaxLength,
                                        inColumn.AllowDBNull ? ADODB.FieldAttributeEnum.adFldIsNullable :
                                                               ADODB.FieldAttributeEnum.adFldUnspecified,
                                        null);
                }

                result.Open(System.Reflection.Missing.Value,
                            System.Reflection.Missing.Value,
                            ADODB.CursorTypeEnum.adOpenStatic,
                            ADODB.LockTypeEnum.adLockOptimistic, 0);

                foreach (DataRow dr in dt.Rows)
                {
                    result.AddNew(System.Reflection.Missing.Value,
                                  System.Reflection.Missing.Value);

                    for (int columnIndex = 0; columnIndex < inColumns.Count; columnIndex++)
                    {
                        resultFields[columnIndex].Value = dr[columnIndex];
                    }
                }

                inColumns = null;

                Marshal.FinalReleaseComObject(resultFields);
                resultFields = null;
                return result;
            }
            else
            {
                return null;
            }
            
        }
        public void DatasetToRecordset(DataTable dt, ref Recordset rs)
        {
            if (dt != null)
            {
                System.Data.DataColumnCollection inColumns = dt.Columns;
                
                foreach (DataRow dr in dt.Rows)
                {
                    rs.AddNew(System.Reflection.Missing.Value,
                                  System.Reflection.Missing.Value);

                    foreach (DataColumn inColumn in inColumns)
                    {
                        try
                        {
                            if (rs.Fields[inColumn.ColumnName].Type == DataTypeEnum.adGUID)
                            {
                                if (dr[inColumn.ColumnName].Equals(Guid.Empty.ToString()) == false)
                                {
                                    rs.Fields[inColumn.ColumnName].Value = "{" + dr[inColumn.ColumnName].ToString() + "}";
                                }
                            }
                            else if (rs.Fields[inColumn.ColumnName].Type == DataTypeEnum.adDBTimeStamp)
                            {
                                if (Convert.ToDateTime(dr[inColumn.ColumnName]) != DateTime.MinValue)
                                {
                                    rs.Fields[inColumn.ColumnName].Value = Convert.ToDateTime(dr[inColumn.ColumnName]);
                                }
                            }
                            else if (rs.Fields[inColumn.ColumnName].Type == DataTypeEnum.adUnsignedTinyInt)
                            {
                                if (dr[inColumn.ColumnName].ToString() != "1")
                                {
                                    rs.Fields[inColumn.ColumnName].Value = false;
                                }
                                else
                                {
                                    rs.Fields[inColumn.ColumnName].Value = dr[inColumn.ColumnName];
                                }

                            }
                            else
                            {
                                if (dr[inColumn.ColumnName].ToString() != string.Empty)
                                {
                                    rs.Fields[inColumn.ColumnName].Value = dr[inColumn.ColumnName];
                                }

                            }
                        }
                        catch
                        {
                            
                        }
                    }
                }

                inColumns = null;
            }
        }
        protected ADODB.DataTypeEnum TranslateType(Type columnType)
        {
            switch (columnType.UnderlyingSystemType.ToString())
            {
                case "System.Boolean":
                    return ADODB.DataTypeEnum.adBoolean;

                case "System.Byte":
                    return ADODB.DataTypeEnum.adUnsignedTinyInt;

                case "System.Char":
                    return ADODB.DataTypeEnum.adChar;

                case "System.DateTime":
                    return ADODB.DataTypeEnum.adDate;

                case "System.Decimal":
                    return ADODB.DataTypeEnum.adCurrency;

                case "System.Double":
                    return ADODB.DataTypeEnum.adDouble;

                case "System.Int16":
                    return ADODB.DataTypeEnum.adSmallInt;

                case "System.Int32":
                    return ADODB.DataTypeEnum.adInteger;

                case "System.Int64":
                    return ADODB.DataTypeEnum.adBigInt;

                case "System.SByte":
                    return ADODB.DataTypeEnum.adTinyInt;

                case "System.Single":
                    return ADODB.DataTypeEnum.adSingle;

                case "System.UInt16":
                    return ADODB.DataTypeEnum.adUnsignedSmallInt;

                case "System.UInt32":
                    return ADODB.DataTypeEnum.adUnsignedInt;
                case "System.UInt64":
                    return ADODB.DataTypeEnum.adUnsignedBigInt;
                case "System.String":
                default:
                    return ADODB.DataTypeEnum.adVarChar;
            }
        }
        protected bool IsDate(string strValue)
        {
            DateTime tmp;
            return DateTime.TryParse(strValue, out tmp);
        }
        protected bool IsBoolean(string strValue)
        {
            Boolean tmp;
            return Boolean.TryParse(strValue, out tmp);
        }
        protected DataTable GetAllocation(DataTable dtMappings, DataTable dtFees, DataTable dtPaymentFee, string gUserId)
        {
            DataTable dt = new DataTable("Allocation");
            decimal dPaid = 0;
            decimal dCharge = 0;
            decimal dNet = 0;
            decimal dOutstanding = 0;

            //Create Column
            dt.Columns.Add("passenger_id", typeof(string));
            dt.Columns.Add("booking_segment_id", typeof(string));
            dt.Columns.Add("booking_fee_id", typeof(string));
            dt.Columns.Add("fee_id", typeof(string));
            dt.Columns.Add("user_id", typeof(string));
            dt.Columns.Add("currency_rcd", typeof(string));
            dt.Columns.Add("sales_amount", typeof(decimal));

            DataRow drAllocation = null;

            if (dtMappings != null && dtMappings.Rows.Count > 0)
            {
                
                foreach (DataRow dr in dtMappings.Rows)
                {
                    dNet = Convert.ToDecimal(dr["net_total"]);
                    if (Convert.ToByte(dr["exclude_pricing_flag"]) == 1)
                    {
                        dCharge = Convert.ToDecimal(dr["payment_amount"]);
                    }
                    else if (Convert.ToDateTime(dr["refund_date_time"]) != DateTime.MinValue)
                    {
                        dCharge = Convert.ToDecimal(dr["refund_charge"]);
                    }
                    else
                    {
                        decimal dExchangePaid = 0;
                        foreach (DataRow sDr in dtMappings.Rows)
                        {
                            if (Convert.ToDateTime(dr["exchanged_date_time"]) != DateTime.MinValue && dr["booking_segment_id"].Equals(sDr["exchanged_segment_id"]) && dr["passenger_id"].Equals(sDr["passenger_id"]))
                            {
                                dExchangePaid = Convert.ToDecimal(sDr["payment_amount"]);
                                break;
                            }
                        }
                        dCharge = Convert.ToDecimal(dr["net_total"]) - dExchangePaid;
                    }
                    dPaid = Convert.ToDecimal(dr["payment_amount"]);
                    dOutstanding = dCharge - dPaid;
                    if (dOutstanding != 0)
                    {
                        drAllocation = dt.NewRow();
                        drAllocation["passenger_id"] = dr["passenger_id"];
                        drAllocation["booking_segment_id"] = dr["booking_segment_id"];
                        drAllocation["user_id"] = gUserId;
                        drAllocation["currency_rcd"] = dr["currency_rcd"];
                        drAllocation["sales_amount"] = Convert.ToDouble(dOutstanding);
                        dt.Rows.Add(drAllocation);
                        drAllocation = null;
                    }
                }
            }
            if (dtFees != null && dtFees.Rows.Count > 0)
            {
                foreach (DataRow dr in dtFees.Rows)
                {
                    if (Convert.ToDateTime(dr["void_date_time"]) == DateTime.MinValue)
                    {
                        dCharge = Convert.ToDecimal(dr["fee_amount_incl"]);
                    }
                    else
                    {
                        dCharge = 0;
                    }
                    dPaid = Convert.ToDecimal(dr["payment_amount"]);
                    dOutstanding = dCharge - dPaid;

                    if (dOutstanding != 0)
                    {
                        drAllocation = dt.NewRow();
                        
                        drAllocation["booking_fee_id"] = dr["booking_fee_id"];
                        drAllocation["user_id"] = gUserId;
                        drAllocation["currency_rcd"] = dr["currency_rcd"];
                        drAllocation["sales_amount"] = Convert.ToDouble(dOutstanding);
                        drAllocation["passenger_id"] = dr["passenger_id"];
                        drAllocation["booking_segment_id"] = dr["booking_segment_id"];

                        dt.Rows.Add(drAllocation);
                        drAllocation = null;
                    }
                }
            }
            if (dtPaymentFee != null && dtPaymentFee.Rows.Count > 0)
            {
                foreach (DataRow dr in dtPaymentFee.Rows)
                {
                    if (Convert.ToDateTime(dr["void_date_time"]) == DateTime.MinValue)
                    {
                        dCharge = Convert.ToDecimal(dr["fee_amount_incl"]);
                    }
                    else
                    {
                        dCharge = 0;
                    }
                    dPaid = Convert.ToDecimal(dr["payment_amount"]);
                    dOutstanding = dCharge - dPaid;

                    if (dOutstanding != 0)
                    {
                        drAllocation = dt.NewRow();

                        drAllocation["booking_fee_id"] = dr["booking_fee_id"];
                        drAllocation["fee_id"] = dr["fee_id"];
                        drAllocation["user_id"] = gUserId;
                        drAllocation["currency_rcd"] = dr["currency_rcd"];
                        drAllocation["sales_amount"] = Convert.ToDouble(dOutstanding);
                        drAllocation["passenger_id"] = dr["passenger_id"];
                        drAllocation["booking_segment_id"] = dr["booking_segment_id"];

                        dt.Rows.Add(drAllocation);
                        drAllocation = null;
                    }
                }
            }
            return dt;
        }
        protected DataTable GetAllocation(DataTable dtPayments, DataTable dtMappings, DataTable dtFees, DataTable dtPaymentFees, string gUserId)
        {
            DataTable dt = new DataTable("Allocation");
            decimal dPaid = 0;
            decimal dCharge = 0;
            decimal dTotalPayment = 0;
            decimal dOutstanding = 0;
            decimal dAllocatedAmount = 0;

            //Create Column
            dt.Columns.Add("passenger_id", typeof(string));
            dt.Columns.Add("booking_segment_id", typeof(string));
            dt.Columns.Add("booking_payment_id", typeof(string));
            dt.Columns.Add("booking_fee_id", typeof(string));
            dt.Columns.Add("voucher_id", typeof(string));
            dt.Columns.Add("user_id", typeof(string));
            dt.Columns.Add("currency_rcd", typeof(string));
            dt.Columns.Add("sales_amount", typeof(decimal));

            DataRow drMapping = null;
            DataRow drExchangeMapping = null;
            DataRow drPaymentFee = null;
            DataRow drFee = null;
            DataRow drAllocation = null;

            if (dtPayments != null && dtPayments.Rows.Count > 0)
            {
                foreach (DataRow drPayment in dtPayments.Rows)
                {
                    if (Convert.ToDecimal(drPayment["payment_amount"]) > Convert.ToDecimal(drPayment["allocated_amount"]))
                    {
                        dTotalPayment = Convert.ToDecimal(drPayment["payment_amount"]);
                        if (dTotalPayment > 0)
                        {
                            //allocate mapping
                            if (dtMappings != null && dtMappings.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtMappings.Rows.Count; i++)
                                {
                                    drMapping = dtMappings.Rows[i];
                                    if (Convert.ToDecimal(drMapping["exclude_pricing_flag"]) != 0)
                                    {
                                        dCharge = Convert.ToDecimal(drMapping["payment_amount"]);
                                    }
                                    else if (Convert.ToDateTime(drMapping["refund_date_time"]) != DateTime.MinValue)
                                    {
                                        dCharge = Convert.ToDecimal(drMapping["refund_charge"]);
                                    }
                                    else
                                    {
                                        decimal dExchangePaid = 0;
                                        for (int m = 0; m < dtMappings.Rows.Count; m++)
                                        {
                                            drExchangeMapping = dtMappings.Rows[m];
                                            if (Convert.ToDateTime(drExchangeMapping["exchanged_date_time"]) != DateTime.MinValue && Convert.ToString(drMapping["booking_segment_id"]).Equals(drExchangeMapping["exchanged_segment_id"].ToString()) && Convert.ToString(drMapping["passenger_id"]).Equals(drExchangeMapping["passenger_id"].ToString()))
                                            {
                                                dExchangePaid = Convert.ToDecimal(drExchangeMapping["payment_amount"]);
                                                break;
                                            }
                                        }

                                        dCharge = Convert.ToDecimal(drMapping["net_total"]) - dExchangePaid;
                                        //dPaid = Convert.ToDecimal(drMapping["payment_amount"]);
                                        DataRow[] drs = dt.Select("passenger_id = '" + drMapping["passenger_id"].ToString() + "' AND booking_segment_id = '" + drMapping["booking_segment_id"].ToString() + "'");
                                        if (drs.Length > 0)
                                        {
                                            dPaid = 0;
                                            for (int ii = 0; ii < drs.Length; ii++)
                                            {
                                                dPaid += Convert.ToDecimal(drs[ii]["sales_amount"].ToString());
                                            }
                                            //dPaid = Convert.ToDecimal(drs[drs.Length - 1]["sales_amount"].ToString());
                                        }
                                        else
                                        {
                                            dPaid = Convert.ToDecimal(drMapping["payment_amount"]);
                                        }

                                        dAllocatedAmount = 0;//
                                        dOutstanding = (dCharge - dPaid) - dAllocatedAmount;
                                    }

                                    if (dOutstanding != 0)
                                    {
                                        if (dTotalPayment == 0)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            if (dAllocatedAmount != dOutstanding)
                                            {
                                                drAllocation = dt.NewRow();

                                                drAllocation["passenger_id"] = drMapping["passenger_id"];

                                                drAllocation["booking_segment_id"] = drMapping["booking_segment_id"];
                                                drAllocation["user_id"] = gUserId;
                                                drAllocation["currency_rcd"] = drMapping["currency_rcd"];
                                                drAllocation["booking_payment_id"] = drPayment["booking_payment_id"];
                                                drAllocation["voucher_id"] = drPayment["voucher_payment_id"];
                                                if (dTotalPayment >= dOutstanding)
                                                {
                                                    drAllocation["sales_amount"] = dOutstanding;
                                                    dAllocatedAmount = dAllocatedAmount + dOutstanding;
                                                    dTotalPayment = dTotalPayment - dOutstanding;
                                                }
                                                else
                                                {
                                                    drAllocation["sales_amount"] = dTotalPayment;
                                                    dAllocatedAmount = dAllocatedAmount + dTotalPayment;
                                                    dTotalPayment = 0;
                                                }
                                                dt.Rows.Add(drAllocation);
                                                drAllocation = null;
                                            }
                                        }
                                    }
                                }
                                //isMappingPaid = true;
                            }

                            //allocate payment fee
                            if (dtPaymentFees != null && dtPaymentFees.Rows.Count > 0)
                            {
                                for (int j = 0; j < dtPaymentFees.Rows.Count; j++)
                                {
                                    drPaymentFee = dtPaymentFees.Rows[j];
                                    if (drPaymentFee["void_date_time"].Equals(DateTime.MinValue))
                                    {
                                        dCharge = Convert.ToDecimal(drPaymentFee["fee_amount_incl"]);
                                    }
                                    else
                                    {
                                        dCharge = 0;
                                    }

                                    //dPaid = Convert.ToDecimal(drPaymentFee["payment_amount"]);
                                    DataRow[] drs = dt.Select("booking_fee_id = '" + drPaymentFee["booking_fee_id"].ToString() + "'");
                                    if (drs.Length > 0)
                                    {
                                        dPaid = 0;
                                        for (int i = 0; i < drs.Length; i++)
                                        {
                                            dPaid += Convert.ToDecimal(drs[i]["sales_amount"].ToString());
                                        }
                                        //dPaid = Convert.ToDecimal(drs[drs.Length - 1]["sales_amount"].ToString());
                                    }
                                    else
                                    {
                                        dPaid = Convert.ToDecimal(drPaymentFee["payment_amount"]);
                                    }

                                    dAllocatedAmount = 0;
                                    dOutstanding = (dCharge - dPaid) - dAllocatedAmount;

                                    if (dOutstanding != 0)
                                    {
                                        if (dTotalPayment == 0)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            if (dAllocatedAmount != dOutstanding)
                                            {
                                                drAllocation = dt.NewRow();

                                                drAllocation["booking_fee_id"] = drPaymentFee["booking_fee_id"];
                                                drAllocation["fee_id"] = drPaymentFee["fee_id"];
                                                drAllocation["user_id"] = gUserId;
                                                drAllocation["currency_rcd"] = drPaymentFee["currency_rcd"];
                                                drAllocation["booking_payment_id"] = drPaymentFee["booking_payment_id"];
                                                drAllocation["voucher_id"] = drPaymentFee["voucher_payment_id"];
                                                if (dTotalPayment >= dOutstanding)
                                                {
                                                    drAllocation["sales_amount"] = dOutstanding;
                                                    dAllocatedAmount = dAllocatedAmount + dOutstanding;
                                                    dTotalPayment = dTotalPayment - dOutstanding;
                                                }
                                                else
                                                {
                                                    drAllocation["sales_amount"] = dTotalPayment;
                                                    dAllocatedAmount = dAllocatedAmount + dTotalPayment;
                                                    dTotalPayment = 0;
                                                }
                                                dt.Rows.Add(drAllocation);
                                                drAllocation = null;
                                            }
                                        }
                                    }
                                }
                            }

                            //allocate other fee
                            if (dtFees != null && dtFees.Rows.Count > 0)
                            {
                                for (int k = 0; k < dtFees.Rows.Count; k++)
                                {
                                    drFee = dtFees.Rows[k];
                                    if (Convert.ToDateTime(drFee["void_date_time"]).Equals(DateTime.MinValue))
                                    {
                                        dCharge = Convert.ToDecimal(drFee["fee_amount_incl"]);
                                    }
                                    else
                                    {
                                        dCharge = 0;
                                    }

                                    //dPaid = Convert.ToDecimal(drFee["payment_amount"]);
                                    DataRow[] drs = dt.Select("booking_fee_id = '" + drFee["booking_fee_id"].ToString() + "'");
                                    if (drs.Length > 0)
                                    {
                                        dPaid = 0;
                                        for (int i = 0; i < drs.Length; i++)
                                        {
                                            dPaid += Convert.ToDecimal(drs[i]["sales_amount"].ToString());
                                        }
                                        //dPaid = Convert.ToDecimal(drs[drs.Length - 1]["sales_amount"].ToString());
                                    }
                                    else
                                    {
                                        dPaid = Convert.ToDecimal(drFee["payment_amount"]);
                                    }

                                    dAllocatedAmount = 0;
                                    dOutstanding = (dCharge - dPaid) - dAllocatedAmount;

                                    if (dOutstanding != 0)
                                    {
                                        if (dTotalPayment == 0)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            if (dAllocatedAmount != dOutstanding)
                                            {
                                                drAllocation = dt.NewRow();

                                                drAllocation["booking_fee_id"] = drFee["booking_fee_id"];
                                                drAllocation["user_id"] = gUserId;
                                                drAllocation["currency_rcd"] = drFee["currency_rcd"];
                                                drAllocation["booking_payment_id"] = drPayment["booking_payment_id"];
                                                drAllocation["voucher_id"] = drPayment["voucher_payment_id"];
                                                if (dTotalPayment >= dOutstanding)
                                                {
                                                    drAllocation["sales_amount"] = dOutstanding;
                                                    dAllocatedAmount = dAllocatedAmount + dOutstanding;
                                                    dTotalPayment = dTotalPayment - dOutstanding;
                                                }
                                                else
                                                {
                                                    drAllocation["sales_amount"] = dTotalPayment;
                                                    dAllocatedAmount = dAllocatedAmount + dTotalPayment;
                                                    dTotalPayment = 0;
                                                }
                                                dt.Rows.Add(drAllocation);
                                                drAllocation = null;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return dt;
        }
        protected bool ValidateSave(ADODB.Recordset rsBookingHeaderUpdate, ADODB.Recordset rsItinerary, ADODB.Recordset rsPassenger, ADODB.Recordset rsMapping)
        {
            if ((rsBookingHeaderUpdate == null) == true)
            {
                return false;
            }
            else if ((rsItinerary == null) == true)
            {
                return false;
            }
            else if ((rsPassenger == null) == true)
            {
                return false;
            }
            else if ((rsMapping == null) == true)
            {
                return false;
            }
            else
            {
                if (rsItinerary.RecordCount == 0)
                {
                    return false;
                }
                else if (rsPassenger.RecordCount == 0)
                {
                    return false;
                }
                else if (rsMapping.RecordCount == 0)
                {
                    return false;
                }
                else if (rsBookingHeaderUpdate.RecordCount == 0)
                {
                    return false;

                }
                else
                {
                    //Check for empty lastname
                    {
                        rsPassenger.MoveFirst();
                        while (!rsPassenger.EOF)
                        {
                            if (rsPassenger.Fields["lastname"].Value == System.DBNull.Value)
                            {
                                rsPassenger.MoveFirst();
                                rsPassenger.Filter = 0;
                                return false;
                            }
                            else if (rsPassenger.Fields["lastname"].Value.ToString().Length == 0)
                            {
                                rsPassenger.MoveFirst();
                                rsPassenger.Filter = 0;
                                return false;
                            }
                            rsPassenger.MoveNext();
                        }
                        rsPassenger.MoveFirst();
                        rsPassenger.Filter = 0;
                    }

                    {
                        rsMapping.MoveFirst();
                        while (!rsMapping.EOF)
                        {
                            if (rsMapping.Fields["lastname"].Value == System.DBNull.Value)
                            {
                                rsMapping.MoveFirst();
                                rsMapping.Filter = 0;
                                return false;
                            }
                            else if (rsMapping.Fields["lastname"].Value.ToString().Length == 0)
                            {
                                rsMapping.MoveFirst();
                                rsMapping.Filter = 0;
                                return false;
                            }
                            rsMapping.MoveNext();
                        }
                        rsMapping.MoveFirst();
                        rsMapping.Filter = 0;
                    }
                }
            }

            return true;
        }
        protected void ClearRecordset(ref Recordset rs)
        {
            if (rs != null)
            {
                if (rs.State == 1)
                {
                    try
                    {
                        rs.Close();
                    }
                    catch
                    { }
                }
                Marshal.FinalReleaseComObject(rs);
                rs = null;
            }
        }
        #endregion
        #region Fabricate Recordset
        protected Recordset FabricateFlightRecordset()
        {
            Recordset rs = new Recordset();

            rs.Fields.Append("flight_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fare_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_segment_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("origin_rcd", ADODB.DataTypeEnum.adVarChar, 5, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("destination_rcd", ADODB.DataTypeEnum.adVarChar, 5, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("airline_rcd", ADODB.DataTypeEnum.adVarChar, 3, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("flight_number", ADODB.DataTypeEnum.adVarChar, 5, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("boarding_class_rcd", ADODB.DataTypeEnum.adVarChar, 2, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_class_rcd", ADODB.DataTypeEnum.adVarChar, 2, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("departure_date", ADODB.DataTypeEnum.adDate, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fare_reduction", ADODB.DataTypeEnum.adInteger, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("waitlist_flag", ADODB.DataTypeEnum.adBoolean, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("refundable_flag", ADODB.DataTypeEnum.adBoolean, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("non_revenue_flag", ADODB.DataTypeEnum.adBoolean, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("eticket_flag", ADODB.DataTypeEnum.adBoolean, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("group_flag", ADODB.DataTypeEnum.adBoolean, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("overbook_flag", ADODB.DataTypeEnum.adBoolean, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("exchanged_segment_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("flight_connection_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("od_origin_rcd", ADODB.DataTypeEnum.adVarChar, 5, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("od_destination_rcd", ADODB.DataTypeEnum.adVarChar, 5, ADODB.FieldAttributeEnum.adFldMayBeNull, null);

            rs.Fields.Append("segment_status_rcd", ADODB.DataTypeEnum.adVarChar, 3, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("exclude_quote_flag", ADODB.DataTypeEnum.adUnsignedTinyInt, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("advanced_purchase_flag", ADODB.DataTypeEnum.adBoolean, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_by", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_date_time", ADODB.DataTypeEnum.adDBTimeStamp, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_by", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_date_time", ADODB.DataTypeEnum.adDBTimeStamp, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);

            rs.Fields.Append("transit_airport_rcd", ADODB.DataTypeEnum.adVarChar, 3, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("transit_boarding_class_rcd", ADODB.DataTypeEnum.adVarChar, 2, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("transit_booking_class_rcd", ADODB.DataTypeEnum.adVarChar, 2, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("transit_flight_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("transit_fare_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("transit_departure_date", ADODB.DataTypeEnum.adDate, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("transit_points", ADODB.DataTypeEnum.adVarChar, 5, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("priority_rcd", ADODB.DataTypeEnum.adVarChar, 20, ADODB.FieldAttributeEnum.adFldIsNullable);
            //Open Recordset
            rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient;
            rs.Open(System.Reflection.Missing.Value,
                    System.Reflection.Missing.Value,
                    CursorTypeEnum.adOpenStatic,
                    LockTypeEnum.adLockOptimistic, 0);

            return rs;
        }
        protected Recordset FabricateHeaderRecordset()
        {
            Recordset rs = new Recordset();

            rs.Fields.Append("booking_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_source_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("currency_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("client_profile_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_number", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("record_locator", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("sender_locator", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("number_of_adults", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("number_of_children", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("number_of_infants", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("language_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("agency_code", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("contact_name", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("contact_email", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("phone_mobile", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("phone_home", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("received_from", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("phone_fax", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("phone_search", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("comment", DataTypeEnum.adVarChar, 500, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("notify_by_email_flag", DataTypeEnum.adBoolean, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("notify_by_sms_flag", DataTypeEnum.adBoolean, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("group_name", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("group_booking_flag", DataTypeEnum.adBoolean, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("agency_name", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("own_agency_flag", DataTypeEnum.adBoolean, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("web_agency_flag", DataTypeEnum.adBoolean, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("client_number", DataTypeEnum.adBigInt, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("lastname", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("firstname", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("city", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_name", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("street", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("lock_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("middlename", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("address_line1", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("address_line2", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("state", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("district", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("province", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("zip_code", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("po_box", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("country_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("title_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("cost_center", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("purchase_order", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("project_number", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("lock_name", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);

            //Open Recordset
            rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient;
            rs.Open(System.Reflection.Missing.Value,
                    System.Reflection.Missing.Value,
                    CursorTypeEnum.adOpenStatic,
                    LockTypeEnum.adLockOptimistic, 0);

            return rs;
        }
        protected Recordset FabricateSegmentRecordset()
        {
            Recordset rs = new Recordset();

            rs.Fields.Append("booking_segment_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("flight_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("flight_connection_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("airline_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("flight_number", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_class_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("boarding_class_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("segment_status_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("number_of_units", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("departure_date", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("departure_time", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("journey_time", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("arrival_time", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("origin_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("destination_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("segment_change_status_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("info_segment_flag", DataTypeEnum.adBoolean, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("high_priority_waitlist_flag", DataTypeEnum.adBoolean, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("marketing_airline_rcd", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("marketing_flight_number", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("od_origin_rcd", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("flight_check_in_status_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("od_destination_rcd", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("origin_name", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("destination_name", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("segment_status_name", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("seatmap_flag", DataTypeEnum.adBoolean, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("temp_seatmap_flag", DataTypeEnum.adBoolean, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("allow_web_checkin_flag", DataTypeEnum.adBoolean, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("close_web_sales_flag", DataTypeEnum.adBoolean, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);

            //Open Recordset
            rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient;
            rs.Open(System.Reflection.Missing.Value,
                    System.Reflection.Missing.Value,
                    CursorTypeEnum.adOpenStatic,
                    LockTypeEnum.adLockOptimistic, 0);

            return rs;
        }
        protected Recordset FabricatePassengerRecordset()
        {
            Recordset rs = new Recordset();

            rs.Fields.Append("passenger_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("client_number", ADODB.DataTypeEnum.adBigInt, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passenger_profile_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passenger_status_rcd", ADODB.DataTypeEnum.adVarChar, 20, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passenger_type_rcd", ADODB.DataTypeEnum.adVarChar, 20, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("lastname", ADODB.DataTypeEnum.adVarChar, 60, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("firstname", ADODB.DataTypeEnum.adVarChar, 60, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("employee_number", ADODB.DataTypeEnum.adVarChar, 60, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("date_of_birth", ADODB.DataTypeEnum.adDBTimeStamp, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("title_rcd", ADODB.DataTypeEnum.adVarChar, 20, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("gender_type_rcd", ADODB.DataTypeEnum.adVarChar, 20, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("nationality_rcd", ADODB.DataTypeEnum.adVarChar, 20, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passport_number", ADODB.DataTypeEnum.adVarChar, 60, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passport_issue_date", ADODB.DataTypeEnum.adDBTimeStamp, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passport_expiry_date", ADODB.DataTypeEnum.adDBTimeStamp, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passport_issue_place", ADODB.DataTypeEnum.adVarWChar, 60, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passport_birth_place", ADODB.DataTypeEnum.adVarWChar, 60, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("document_type_rcd", ADODB.DataTypeEnum.adVarChar, 20, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("wheelchair_flag", ADODB.DataTypeEnum.adUnsignedTinyInt, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("vip_flag", ADODB.DataTypeEnum.adUnsignedTinyInt, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_by", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_date_time", ADODB.DataTypeEnum.adDBTimeStamp, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_by", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_date_time", ADODB.DataTypeEnum.adDBTimeStamp, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passenger_weight", ADODB.DataTypeEnum.adDouble, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("client_profile_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);

            rs.Fields.Append("flight_id", ADODB.DataTypeEnum.adGUID, 0, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("origin_rcd", ADODB.DataTypeEnum.adVarChar, 60, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("destination_rcd", ADODB.DataTypeEnum.adVarChar, 60, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passenger_check_in_status_rcd", ADODB.DataTypeEnum.adVarChar, 60, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("class_rcd", ADODB.DataTypeEnum.adVarChar, 60, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("action", ADODB.DataTypeEnum.adVarChar, 60, ADODB.FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("record_locator", ADODB.DataTypeEnum.adVarChar, 60, ADODB.FieldAttributeEnum.adFldMayBeNull, null);

            //Open Recordset
            rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient;
            rs.Open(System.Reflection.Missing.Value,
                    System.Reflection.Missing.Value,
                    CursorTypeEnum.adOpenStatic,
                    LockTypeEnum.adLockOptimistic, 0);

            return rs;
        }
        protected Recordset FabricateRemarkRecordset()
        {
            Recordset rs = new Recordset();

            rs.Fields.Append("booking_remark_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("client_profile_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("added_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("remark_type_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("agency_code", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("timelimit_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("remark_text", DataTypeEnum.adVarWChar, 450, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("complete_flag", DataTypeEnum.adUnsignedTinyInt, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);

            //Open Recordset
            rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient;
            rs.Open(System.Reflection.Missing.Value,
                    System.Reflection.Missing.Value,
                    CursorTypeEnum.adOpenStatic,
                    LockTypeEnum.adLockOptimistic, 0);

            return rs;
        }
        protected Recordset FabricatePaymentRecordset()
        {
            Recordset rs = new Recordset();

            rs.Fields.Append("booking_payment_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_segment_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("voucher_payment_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("form_of_payment_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("currency_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("receive_currency_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("agency_code", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("payment_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("receive_payment_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("acct_payment_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("payment_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("payment_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("payment_due_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("document_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("void_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("void_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("expiry_month", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("expiry_year", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("cvv_code", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("name_on_card", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("document_number", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("form_of_payment_subtype_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("city", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("state", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("street", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("address_line1", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("address_line2", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("zip_code", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("country_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("pos_indicator", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("issue_month", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("issue_year", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("issue_number", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_reference", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("payment_reference", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("transaction_reference", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("approval_code", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("manual_creditcard_transaction_flag", DataTypeEnum.adBoolean, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("processed_authorization_flag", DataTypeEnum.adBoolean, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("processed_authorization_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            
            //Open Recordset
            rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient;
            rs.Open(System.Reflection.Missing.Value,
                    System.Reflection.Missing.Value,
                    CursorTypeEnum.adOpenStatic,
                    LockTypeEnum.adLockOptimistic, 0);

            return rs;
        }
        protected Recordset FabricateMappingRecordset()
        {
            Recordset rs = new Recordset();

            rs.Fields.Append("exchanged_segment_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passenger_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_segment_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("origin_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("destination_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("lastname", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("firstname", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("gender_type_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("title_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passenger_type_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("date_of_birth", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("flight_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("airline_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("flight_number", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("departure_date", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_class_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("boarding_class_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("inventory_class_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("record_locator", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("seat_number", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("seat_row", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("seat_column", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("currency_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("net_total", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("tax_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fare_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("yq_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("base_ticketing_fee_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("ticketing_fee_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("reservation_fee_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("commission_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fare_vat", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("tax_vat", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("yq_vat", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("ticketing_fee_vat", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("reservation_fee_vat", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fare_amount_incl", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("tax_amount_incl", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("yq_amount_incl", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("public_fare_amount_incl", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("public_fare_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("commission_amount_incl", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("commission_percentage", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("ticketing_fee_amount_incl", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("reservation_fee_amount_incl", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("acct_net_total", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("acct_tax_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("acct_fare_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("acct_yq_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("acct_ticketing_fee_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("acct_reservation_fee_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("acct_commission_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("acct_fare_amount_incl", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("acct_tax_amount_incl", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("acct_yq_amount_incl", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("acct_commission_amount_incl", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("acct_ticketing_fee_amount_incl", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("acct_reservation_fee_amount_incl", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fare_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fare_code", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fare_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("refundable_flag", DataTypeEnum.adUnsignedTinyInt, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("advanced_seating_flag", DataTypeEnum.adUnsignedTinyInt, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("e_ticket_flag", DataTypeEnum.adUnsignedTinyInt, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("standby_flag", DataTypeEnum.adUnsignedTinyInt, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("transferable_fare_flag", DataTypeEnum.adUnsignedTinyInt, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passenger_check_in_status_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("priority_code", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("agency_code", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passenger_status_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("ticket_number", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("ticketing_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("ticketing_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("boarding_pass_number", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("check_in_sequence", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("group_sequence", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("unload_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("unload_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("unload_comment", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("number_of_pieces", DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("baggage_weight", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("excess_baggage_weight", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("check_in_baggage_weight", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("check_in_user_code", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("check_in_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("cancel_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("exchanged_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("cancel_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fare_description", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("restriction_text", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fare_line", DataTypeEnum.adVarWChar, 450, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("form_of_payment_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("master_passenger_type_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("endorsement_text", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("exclude_pricing_flag", DataTypeEnum.adUnsignedTinyInt, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fare_note", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("ticketing_name", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_name", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_name", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("cancel_name", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("void_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("refund_charge", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("refundable_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("refund_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("payment_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("check_in_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("onward_airline_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("onward_flight_number", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("onward_departure_date", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("onward_destination_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("onward_booking_class_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("e_ticket_status", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);

            //Open Recordset
            rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient;
            rs.Open(System.Reflection.Missing.Value,
                    System.Reflection.Missing.Value,
                    CursorTypeEnum.adOpenStatic,
                    LockTypeEnum.adLockOptimistic, 0);

            return rs;
        }
        protected Recordset FabricateServiceRecordset()
        {
            Recordset rs = new Recordset();

            rs.Fields.Append("passenger_segment_service_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passenger_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_segment_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("special_service_status_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("special_service_change_status_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("special_service_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("service_text", DataTypeEnum.adVarWChar, 500, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);

            //Open Recordset
            rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient;
            rs.Open(System.Reflection.Missing.Value,
                    System.Reflection.Missing.Value,
                    CursorTypeEnum.adOpenStatic,
                    LockTypeEnum.adLockOptimistic, 0);

            return rs;
        }
        protected Recordset FabricateTaxRecordset()
        {
            Recordset rs = new Recordset();

            rs.Fields.Append("passenger_segment_tax_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("tax_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("tax_amount_incl", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("acct_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("acct_amount_incl", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("sales_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("sales_amount_incl", DataTypeEnum.adDouble, 00, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("tax_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passenger_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("tax_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_segment_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("tax_currency_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("sales_currency_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("display_name", DataTypeEnum.adVarChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("summarize_up", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("coverage_type", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);

            //Open Recordset
            rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient;
            rs.Open(System.Reflection.Missing.Value,
                    System.Reflection.Missing.Value,
                    CursorTypeEnum.adOpenStatic,
                    LockTypeEnum.adLockOptimistic, 0);

            return rs;
        }
        protected Recordset FabricateFeeRecordset()
        {
            Recordset rs = new Recordset();

            rs.Fields.Append("booking_fee_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fee_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passenger_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("currency_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("acct_fee_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fee_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("vat_percentage", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fee_amount_incl", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("acct_fee_amount_incl", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fee_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("display_name", DataTypeEnum.adVarWChar, 60, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("account_fee_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("account_fee_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("void_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("void_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("payment_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_segment_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("agency_code", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("create_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("update_date_time", DataTypeEnum.adDBTimeStamp, 0, FieldAttributeEnum.adFldMayBeNull, null);

            //Open Recordset
            rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient;
            rs.Open(System.Reflection.Missing.Value,
                    System.Reflection.Missing.Value,
                    CursorTypeEnum.adOpenStatic,
                    LockTypeEnum.adLockOptimistic, 0);

            return rs;
        }
        protected Recordset FabricatePaymentAllocationRecordset()
        {
            Recordset rs = new Recordset();

            rs.Fields.Append("booking_payment_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("passenger_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_segment_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("currency_rcd", DataTypeEnum.adVarChar, 20, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("booking_fee_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("voucher_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("fee_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("user_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("sales_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("payment_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);
            rs.Fields.Append("account_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldMayBeNull, null);

            rs.Fields.Append("passenger_segment_service_id", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("od_origin_rcd", DataTypeEnum.adVarChar, 5, FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("od_destination_rcd", DataTypeEnum.adVarChar, 5, FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("charge_amount", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("charge_amount_incl", DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("charge_currency_rcd", DataTypeEnum.adVarChar, 10, FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("fee_category_rcd", DataTypeEnum.adVarChar, 5, FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("external_reference", DataTypeEnum.adVarChar, 50, FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("vendor_rcd", DataTypeEnum.adVarChar, 50, FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("weight_lbs", DataTypeEnum.adNumeric, 0, FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("weight_kgs", DataTypeEnum.adNumeric, 0, FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("units", DataTypeEnum.adNumeric, 0, FieldAttributeEnum.adFldIsNullable, null);
            rs.Fields.Append("create_by", DataTypeEnum.adGUID, 0, FieldAttributeEnum.adFldIsNullable, null);

            //Open Recordset
            rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient;
            rs.Open(System.Reflection.Missing.Value,
                    System.Reflection.Missing.Value,
                    CursorTypeEnum.adOpenStatic,
                    LockTypeEnum.adLockOptimistic, 0);

            return rs;
        }
        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            string strUserName = ConfigurationManager.AppSettings["ComUser"];
            string strPassword = ConfigurationManager.AppSettings["ComPassword"];
            string strDomain = ConfigurationManager.AppSettings["ComDomain"];

            if (string.IsNullOrEmpty(strUserName) == false && string.IsNullOrEmpty(strPassword)== false && string.IsNullOrEmpty(strDomain) == false)
            {
                undoImpersonation();
            }
        }

        #endregion
    }
}
