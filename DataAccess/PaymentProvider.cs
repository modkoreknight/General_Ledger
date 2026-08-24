using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Interact.BusinessLogic;
using Interact.Common;

namespace Interact.DataAccess
{
    public class PaymentProvider : IPaymentProvider
    {
        #region Fields
        private SqlConnection _conn;
        private Boolean _isLocal;
        private Branch _branch;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public PaymentProvider(SqlConnection conn)
        {
            this._conn = conn;
            String[] str = conn.Database.Split('_');
            if (str.Count() > 1)
            {
                if (str[1] == "Audit")
                {
                    this._branch = Branch.Audit_;
                }
                else
                {
                    this._branch = (Branch)Enum.Parse(typeof(Branch), str[1]);
                }
            }

            Int32 branchKey = 0;
            Boolean result = Int32.TryParse(this._conn.Database.Substring(this._conn.Database.Length - 2, 2), out branchKey);
            if (result)
            {
                this._isLocal = false;
                this._branch = (Branch)Enum.Parse(typeof(Branch), branchKey.ToString());
            }
            else
            {
                this._isLocal = true;
            }
        }
        #endregion

        #region Methods
        public Int32 GetPaymentPageCount()
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "PaymentGetPageCount";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@PageSize", SqlDbType.TinyInt);
            myParam1.Value = Utility.PageSize;
            SqlParameter myParam2 = new SqlParameter("@Output", SqlDbType.TinyInt);
            myParam2.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(myParam1);
            myCommand.Parameters.Add(myParam2);
            try
            {
                this._conn.Open();
                try
                {
                    myCommand.ExecuteNonQuery();
                }
                catch
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
            finally
            {
                this._conn.Close();
            }
            return Convert.ToInt32(myParam2.Value);
        }

        public Payment GetPayment(Int32 id)
        {
            Payment payment = Payment.CreatePayment();
            ISalesProvider salesProvider;
            if (this._isLocal)
            {
                if (this._branch == Branch.Audit_)
                {
                    salesProvider = new SalesProvider(Database.AuditConnection());
                }
                else
                {
                    salesProvider = new SalesProvider(Database.GeneralLedger);
                }
            }
            else
            {
                salesProvider = new SalesProvider(Database.BranchConnection(this._branch));
            }
            SalesManager salesManager = new SalesManager(salesProvider);

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "PaymentSelect";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@ID", SqlDbType.Int);
            myParam1.Value = id;
            myCommand.Parameters.Add(myParam1);
            try
            {
                this._conn.Open();
                try
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            myReader.Read();
                            payment.ID = myReader.GetInt32(0);
                            if (myReader.IsDBNull(1))
                            {
                                payment.PaymentSales = null;
                            }
                            else
                            {
                                payment.PaymentSales = salesManager.GetSales(myReader.GetInt32(1));
                            }
                            if (myReader.IsDBNull(2))
                            {
                                payment.PaymentNo = String.Empty;
                            }
                            else
                            {
                                payment.PaymentNo = myReader.GetString(2);
                            }
                            if (myReader.IsDBNull(3))
                            {
                                payment.PaymentDate = DateTime.Today;
                            }
                            else
                            {
                                payment.PaymentDate = myReader.GetDateTime(3);
                            }
                            if (myReader.IsDBNull(4))
                            {
                                payment.PaymentAmount = 0;
                            }
                            else
                            {
                                payment.PaymentAmount = myReader.GetDecimal(4);
                            }
                            if (myReader.IsDBNull(5))
                            {
                                payment.Rebate = 0;
                            }
                            else
                            {
                                payment.Rebate = myReader.GetDecimal(5);
                            }
                            if (myReader.IsDBNull(6))
                            {
                                payment.Mode = PaymentMode.Cash;
                            }
                            else
                            {
                                payment.Mode = (PaymentMode)Enum.Parse(typeof(PaymentMode), myReader.GetInt32(6).ToString());
                            }
                            if (myReader.IsDBNull(7))
                            {
                                payment.CheckNo = String.Empty;
                            }
                            else
                            {
                                payment.CheckNo = myReader.GetString(7).ToString();
                            }
                            if (myReader.IsDBNull(8))
                            {
                                payment.Status = PaymentStatus.Processing;
                            }
                            else
                            {
                                payment.Status = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), myReader.GetInt32(8).ToString());
                            }
                            if (myReader.IsDBNull(9))
                            {
                                payment.InstNo = 0;
                            }
                            else
                            {
                                payment.InstNo = myReader.GetInt32(9);
                            }
                            if (myReader.IsDBNull(10))
                            {
                                payment.MonthApplied = String.Empty;
                            }
                            else
                            {
                                payment.MonthApplied = myReader.GetString(10).ToString();
                            }
                            if (myReader.IsDBNull(11))
                            {
                                payment.Due = 0;
                            }
                            else
                            {
                                payment.Due = myReader.GetDecimal(11);
                            }
                            if (myReader.IsDBNull(12))
                            {
                                payment.Overdue = 0;
                            }
                            else
                            {
                                payment.Overdue = myReader.GetDecimal(12);
                            }
                            if (myReader.IsDBNull(13))
                            {
                                payment.Debit = 0;
                            }
                            else
                            {
                                payment.Debit = myReader.GetDecimal(13);
                            }
                            if (myReader.IsDBNull(14))
                            {
                                payment.Credit = 0;
                            }
                            else
                            {
                                payment.Credit = myReader.GetDecimal(14);
                            }
                            if (myReader.IsDBNull(15))
                            {
                                payment.Remarks = String.Empty;
                            }
                            else
                            {
                                payment.Remarks = myReader.GetString(15);
                            }
                            if (myReader.FieldCount > 16)
                            {
                                if (myReader.IsDBNull(16))
                                {
                                    payment.Branch = 0;
                                }
                                else
                                {
                                    payment.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(16).ToString());
                                }
                            }
                            if (myReader.FieldCount > 17)
                            {
                                if (myReader.IsDBNull(17))
                                {
                                    payment.AuditID = 0;
                                }
                                else
                                {
                                    payment.AuditID = myReader.GetInt32(17);
                                }
                            }
                            payment.Branch = this._branch;
                        }
                    }
                }
                finally
                {
                    this._conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return payment;
        }

        public GenericList<Payment> GetAllPayment()
        {
            GenericList<Payment> allPayment = new GenericList<Payment>();
            ISalesProvider salesProvider;
            if (this._isLocal)
            {
                if (this._branch == Branch.Audit_)
                {
                    salesProvider = new SalesProvider(Database.AuditConnection());
                }
                else
                {
                    salesProvider = new SalesProvider(Database.GeneralLedger);
                }
            }
            else
            {
                salesProvider = new SalesProvider(Database.BranchConnection(this._branch));
            }
            SalesManager salesManager = new SalesManager(salesProvider);

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "PaymentSelect";
            myCommand.Connection = this._conn;
            try
            {
                this._conn.Open();
                try
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            while (myReader.Read())
                            {
                                Payment payment = Payment.CreatePayment();
                                payment.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    payment.PaymentSales = null;
                                }
                                else
                                {
                                    //payment.PaymentSales = salesManager.GetSales(myReader.GetInt32(1));
                                    Sales sales = Sales.CreateSales();
                                    sales.ID = myReader.GetInt32(1);
                                    payment.PaymentSales = sales;
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    payment.PaymentNo = String.Empty;
                                }
                                else
                                {
                                    payment.PaymentNo = myReader.GetString(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    payment.PaymentDate = DateTime.Today;
                                }
                                else
                                {
                                    payment.PaymentDate = myReader.GetDateTime(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    payment.PaymentAmount = 0;
                                }
                                else
                                {
                                    payment.PaymentAmount = myReader.GetDecimal(4);
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    payment.Rebate = 0;
                                }
                                else
                                {
                                    payment.Rebate = myReader.GetDecimal(5);
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    payment.Mode = PaymentMode.Cash;
                                }
                                else
                                {
                                    payment.Mode = (PaymentMode)Enum.Parse(typeof(PaymentMode), myReader.GetInt32(6).ToString());
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    payment.CheckNo = String.Empty;
                                }
                                else
                                {
                                    payment.CheckNo = myReader.GetString(7).ToString();
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    payment.Status = PaymentStatus.Processing;
                                }
                                else
                                {
                                    payment.Status = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), myReader.GetInt32(8).ToString());
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    payment.InstNo = 0;
                                }
                                else
                                {
                                    payment.InstNo = myReader.GetInt32(9);
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    payment.MonthApplied = String.Empty;
                                }
                                else
                                {
                                    payment.MonthApplied = myReader.GetString(10).ToString();
                                }
                                if (myReader.IsDBNull(11))
                                {
                                    payment.Due = 0;
                                }
                                else
                                {
                                    payment.Due = myReader.GetDecimal(11);
                                }
                                if (myReader.IsDBNull(12))
                                {
                                    payment.Overdue = 0;
                                }
                                else
                                {
                                    payment.Overdue = myReader.GetDecimal(12);
                                }
                                if (myReader.IsDBNull(13))
                                {
                                    payment.Debit = 0;
                                }
                                else
                                {
                                    payment.Debit = myReader.GetDecimal(13);
                                }
                                if (myReader.IsDBNull(14))
                                {
                                    payment.Credit = 0;
                                }
                                else
                                {
                                    payment.Credit = myReader.GetDecimal(14);
                                }
                                if (myReader.IsDBNull(15))
                                {
                                    payment.Remarks = String.Empty;
                                }
                                else
                                {
                                    payment.Remarks = myReader.GetString(15);
                                }
                                if (myReader.FieldCount > 16)
                                {
                                    if (myReader.IsDBNull(16))
                                    {
                                        payment.Branch = 0;
                                    }
                                    else
                                    {
                                        payment.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(16).ToString());
                                    }
                                }
                                if (myReader.FieldCount > 17)
                                {
                                    if (myReader.IsDBNull(17))
                                    {
                                        payment.AuditID = 0;
                                    }
                                    else
                                    {
                                        payment.AuditID = myReader.GetInt32(17);
                                    }
                                }
                                allPayment.Add(payment);
                            }
                        }
                    }
                }
                finally
                {
                    this._conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return allPayment;
        }

        public GenericList<Payment> GetAllPayment(Sales sales)
        {
            GenericList<Payment> allPayment = new GenericList<Payment>();
            //ISalesProvider salesProvider;
            //if (this._isLocal)
            //{
            //    salesProvider = new SalesProvider(Database.GeneralLedger);
            //}
            //else
            //{
            //    salesProvider = new SalesProvider(Database.BranchConnection(this._branch));
            //}
            //SalesManager salesManager = new SalesManager(salesProvider);

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "PaymentSelect";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@SalesID", SqlDbType.Int);
            myParam1.Value = sales.ID;
            myCommand.Parameters.Add(myParam1);
            try
            {
                this._conn.Open();
                try
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            while (myReader.Read())
                            {
                                Payment payment = Payment.CreatePayment();
                                payment.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    payment.PaymentSales = null;
                                }
                                else
                                {
                                    //payment.PaymentSales = salesManager.GetSales(myReader.GetInt32(1));
                                    payment.PaymentSales = sales;
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    payment.PaymentNo = String.Empty;
                                }
                                else
                                {
                                    payment.PaymentNo = myReader.GetString(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    payment.PaymentDate = DateTime.Today;
                                }
                                else
                                {
                                    payment.PaymentDate = myReader.GetDateTime(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    payment.PaymentAmount = 0;
                                }
                                else
                                {
                                    payment.PaymentAmount = myReader.GetDecimal(4);
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    payment.Rebate = 0;
                                }
                                else
                                {
                                    payment.Rebate = myReader.GetDecimal(5);
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    payment.Mode = PaymentMode.Cash;
                                }
                                else
                                {
                                    payment.Mode = (PaymentMode)Enum.Parse(typeof(PaymentMode), myReader.GetInt32(6).ToString());
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    payment.CheckNo = String.Empty;
                                }
                                else
                                {
                                    payment.CheckNo = myReader.GetString(7).ToString();
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    payment.Status = PaymentStatus.Processing;
                                }
                                else
                                {
                                    payment.Status = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), myReader.GetInt32(8).ToString());
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    payment.InstNo = 0;
                                }
                                else
                                {
                                    payment.InstNo = myReader.GetInt32(9);
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    payment.MonthApplied = String.Empty;
                                }
                                else
                                {
                                    payment.MonthApplied = myReader.GetString(10).ToString();
                                }
                                if (myReader.IsDBNull(11))
                                {
                                    payment.Due = 0;
                                }
                                else
                                {
                                    payment.Due = myReader.GetDecimal(11);
                                }
                                if (myReader.IsDBNull(12))
                                {
                                    payment.Overdue = 0;
                                }
                                else
                                {
                                    payment.Overdue = myReader.GetDecimal(12);
                                }
                                if (myReader.IsDBNull(13))
                                {
                                    payment.Debit = 0;
                                }
                                else
                                {
                                    payment.Debit = myReader.GetDecimal(13);
                                }
                                if (myReader.IsDBNull(14))
                                {
                                    payment.Credit = 0;
                                }
                                else
                                {
                                    payment.Credit = myReader.GetDecimal(14);
                                }
                                if (myReader.IsDBNull(15))
                                {
                                    payment.Remarks = String.Empty;
                                }
                                else
                                {
                                    payment.Remarks = myReader.GetString(15);
                                }
                                if (myReader.FieldCount > 16)
                                {
                                    if (myReader.IsDBNull(16))
                                    {
                                        payment.Branch = 0;
                                    }
                                    else
                                    {
                                        payment.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(16).ToString());
                                    }
                                }
                                if (myReader.FieldCount > 17)
                                {
                                    if (myReader.IsDBNull(17))
                                    {
                                        payment.AuditID = 0;
                                    }
                                    else
                                    {
                                        payment.AuditID = myReader.GetInt32(17);
                                    }
                                }
                                allPayment.Add(payment);
                            }
                        }
                    }
                }
                finally
                {
                    this._conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return allPayment;
        }

        public GenericList<Payment> GetAllPayment(Int32 rangeFrom, Int32 rangeTo)
        {
            GenericList<Payment> allPayment = new GenericList<Payment>();
            ISalesProvider salesProvider;
            if (this._isLocal)
            {
                if (this._branch == Branch.Audit_)
                {
                    salesProvider = new SalesProvider(Database.AuditConnection());
                }
                else
                {
                    salesProvider = new SalesProvider(Database.GeneralLedger);
                }
            }
            else
            {
                salesProvider = new SalesProvider(Database.BranchConnection(this._branch));
            }
            SalesManager salesManager = new SalesManager(salesProvider);

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "PaymentSelectRangeOR";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@From", SqlDbType.Int);
            myParam1.Value = rangeFrom;
            SqlParameter myParam2 = new SqlParameter("@To", SqlDbType.Int);
            myParam2.Value = rangeTo;
            myCommand.Parameters.Add(myParam1);
            myCommand.Parameters.Add(myParam2);
            try
            {
                this._conn.Open();
                try
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            while (myReader.Read())
                            {
                                Payment payment = Payment.CreatePayment();
                                payment.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    payment.PaymentSales = null;
                                }
                                else
                                {
                                    payment.PaymentSales = salesManager.GetSales(myReader.GetInt32(1));
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    payment.PaymentNo = String.Empty;
                                }
                                else
                                {
                                    payment.PaymentNo = myReader.GetString(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    payment.PaymentDate = DateTime.Today;
                                }
                                else
                                {
                                    payment.PaymentDate = myReader.GetDateTime(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    payment.PaymentAmount = 0;
                                }
                                else
                                {
                                    payment.PaymentAmount = myReader.GetDecimal(4);
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    payment.Rebate = 0;
                                }
                                else
                                {
                                    payment.Rebate = myReader.GetDecimal(5);
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    payment.Mode = PaymentMode.Cash;
                                }
                                else
                                {
                                    payment.Mode = (PaymentMode)Enum.Parse(typeof(PaymentMode), myReader.GetInt32(6).ToString());
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    payment.CheckNo = String.Empty;
                                }
                                else
                                {
                                    payment.CheckNo = myReader.GetString(7).ToString();
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    payment.Status = PaymentStatus.Processing;
                                }
                                else
                                {
                                    payment.Status = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), myReader.GetInt32(8).ToString());
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    payment.InstNo = 0;
                                }
                                else
                                {
                                    payment.InstNo = myReader.GetInt32(9);
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    payment.MonthApplied = String.Empty;
                                }
                                else
                                {
                                    payment.MonthApplied = myReader.GetString(10).ToString();
                                }
                                if (myReader.IsDBNull(11))
                                {
                                    payment.Due = 0;
                                }
                                else
                                {
                                    payment.Due = myReader.GetDecimal(11);
                                }
                                if (myReader.IsDBNull(12))
                                {
                                    payment.Overdue = 0;
                                }
                                else
                                {
                                    payment.Overdue = myReader.GetDecimal(12);
                                }
                                if (myReader.IsDBNull(13))
                                {
                                    payment.Debit = 0;
                                }
                                else
                                {
                                    payment.Debit = myReader.GetDecimal(13);
                                }
                                if (myReader.IsDBNull(14))
                                {
                                    payment.Credit = 0;
                                }
                                else
                                {
                                    payment.Credit = myReader.GetDecimal(14);
                                }
                                if (myReader.IsDBNull(15))
                                {
                                    payment.Remarks = String.Empty;
                                }
                                else
                                {
                                    payment.Remarks = myReader.GetString(15);
                                }
                                if (myReader.FieldCount > 16)
                                {
                                    if (myReader.IsDBNull(16))
                                    {
                                        payment.Branch = 0;
                                    }
                                    else
                                    {
                                        payment.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(16).ToString());
                                    }
                                }
                                if (myReader.FieldCount > 17)
                                {
                                    if (myReader.IsDBNull(17))
                                    {
                                        payment.AuditID = 0;
                                    }
                                    else
                                    {
                                        payment.AuditID = myReader.GetInt32(17);
                                    }
                                }
                                allPayment.Add(payment);
                            }
                        }
                    }
                }
                finally
                {
                    this._conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return allPayment;
        }

        public GenericList<Payment> GetAllPayment(String monthApplied)
        {
            GenericList<Payment> allPayment = new GenericList<Payment>();
            ISalesProvider salesProvider;
            if (this._isLocal)
            {
                if (this._branch == Branch.Audit_)
                {
                    salesProvider = new SalesProvider(Database.AuditConnection());
                }
                else
                {
                    salesProvider = new SalesProvider(Database.GeneralLedger);
                }
            }
            else
            {
                salesProvider = new SalesProvider(Database.BranchConnection(this._branch));
            }
            SalesManager salesManager = new SalesManager(salesProvider);

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "PaymentSelectRangeMonth";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@MonthApplied", SqlDbType.VarChar, 16);
            myParam1.Value = monthApplied;
            myCommand.Parameters.Add(myParam1);
            try
            {
                this._conn.Open();
                try
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            while (myReader.Read())
                            {
                                Payment payment = Payment.CreatePayment();
                                payment.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    payment.PaymentSales = null;
                                }
                                else
                                {
                                    payment.PaymentSales = salesManager.GetSales(myReader.GetInt32(1));
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    payment.PaymentNo = String.Empty;
                                }
                                else
                                {
                                    payment.PaymentNo = myReader.GetString(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    payment.PaymentDate = DateTime.Today;
                                }
                                else
                                {
                                    payment.PaymentDate = myReader.GetDateTime(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    payment.PaymentAmount = 0;
                                }
                                else
                                {
                                    payment.PaymentAmount = myReader.GetDecimal(4);
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    payment.Rebate = 0;
                                }
                                else
                                {
                                    payment.Rebate = myReader.GetDecimal(5);
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    payment.Mode = PaymentMode.Cash;
                                }
                                else
                                {
                                    payment.Mode = (PaymentMode)Enum.Parse(typeof(PaymentMode), myReader.GetInt32(6).ToString());
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    payment.CheckNo = String.Empty;
                                }
                                else
                                {
                                    payment.CheckNo = myReader.GetString(7).ToString();
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    payment.Status = PaymentStatus.Processing;
                                }
                                else
                                {
                                    payment.Status = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), myReader.GetInt32(8).ToString());
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    payment.InstNo = 0;
                                }
                                else
                                {
                                    payment.InstNo = myReader.GetInt32(9);
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    payment.MonthApplied = String.Empty;
                                }
                                else
                                {
                                    payment.MonthApplied = myReader.GetString(10).ToString();
                                }
                                if (myReader.IsDBNull(11))
                                {
                                    payment.Due = 0;
                                }
                                else
                                {
                                    payment.Due = myReader.GetDecimal(11);
                                }
                                if (myReader.IsDBNull(12))
                                {
                                    payment.Overdue = 0;
                                }
                                else
                                {
                                    payment.Overdue = myReader.GetDecimal(12);
                                }
                                if (myReader.IsDBNull(13))
                                {
                                    payment.Debit = 0;
                                }
                                else
                                {
                                    payment.Debit = myReader.GetDecimal(13);
                                }
                                if (myReader.IsDBNull(14))
                                {
                                    payment.Credit = 0;
                                }
                                else
                                {
                                    payment.Credit = myReader.GetDecimal(14);
                                }
                                if (myReader.IsDBNull(15))
                                {
                                    payment.Remarks = String.Empty;
                                }
                                else
                                {
                                    payment.Remarks = myReader.GetString(15);
                                }
                                if (myReader.FieldCount > 16)
                                {
                                    if (myReader.IsDBNull(16))
                                    {
                                        payment.Branch = 0;
                                    }
                                    else
                                    {
                                        payment.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(16).ToString());
                                    }
                                }
                                if (myReader.FieldCount > 17)
                                {
                                    if (myReader.IsDBNull(17))
                                    {
                                        payment.AuditID = 0;
                                    }
                                    else
                                    {
                                        payment.AuditID = myReader.GetInt32(17);
                                    }
                                }
                                allPayment.Add(payment);
                            }
                        }
                    }
                }
                finally
                {
                    this._conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return allPayment;
        }

        public GenericList<Payment> GetAllPayment(DateTime from, DateTime to)
        {
            GenericList<Payment> allPayment = new GenericList<Payment>();
            ISalesProvider salesProvider;
            if (this._isLocal)
            {
                if (this._branch == Branch.Audit_)
                {
                    salesProvider = new SalesProvider(Database.AuditConnection());
                }
                else
                {
                    salesProvider = new SalesProvider(Database.GeneralLedger);
                }
            }
            else
            {
                salesProvider = new SalesProvider(Database.BranchConnection(this._branch));
            }
            SalesManager salesManager = new SalesManager(salesProvider);

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "PaymentSelectRangeDate";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@From", SqlDbType.DateTime);
            myParam1.Value = from;
            SqlParameter myParam2 = new SqlParameter("@To", SqlDbType.DateTime);
            myParam2.Value = to;
            myCommand.Parameters.Add(myParam1);
            myCommand.Parameters.Add(myParam2);
            try
            {
                this._conn.Open();
                try
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            while (myReader.Read())
                            {
                                Payment payment = Payment.CreatePayment();
                                payment.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    payment.PaymentSales = null;
                                }
                                else
                                {
                                    payment.PaymentSales = salesManager.GetSales(myReader.GetInt32(1));
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    payment.PaymentNo = String.Empty;
                                }
                                else
                                {
                                    payment.PaymentNo = myReader.GetString(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    payment.PaymentDate = DateTime.Today;
                                }
                                else
                                {
                                    payment.PaymentDate = myReader.GetDateTime(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    payment.PaymentAmount = 0;
                                }
                                else
                                {
                                    payment.PaymentAmount = myReader.GetDecimal(4);
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    payment.Rebate = 0;
                                }
                                else
                                {
                                    payment.Rebate = myReader.GetDecimal(5);
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    payment.Mode = PaymentMode.Cash;
                                }
                                else
                                {
                                    payment.Mode = (PaymentMode)Enum.Parse(typeof(PaymentMode), myReader.GetInt32(6).ToString());
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    payment.CheckNo = String.Empty;
                                }
                                else
                                {
                                    payment.CheckNo = myReader.GetString(7).ToString();
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    payment.Status = PaymentStatus.Processing;
                                }
                                else
                                {
                                    payment.Status = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), myReader.GetInt32(8).ToString());
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    payment.InstNo = 0;
                                }
                                else
                                {
                                    payment.InstNo = myReader.GetInt32(9);
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    payment.MonthApplied = String.Empty;
                                }
                                else
                                {
                                    payment.MonthApplied = myReader.GetString(10).ToString();
                                }
                                if (myReader.IsDBNull(11))
                                {
                                    payment.Due = 0;
                                }
                                else
                                {
                                    payment.Due = myReader.GetDecimal(11);
                                }
                                if (myReader.IsDBNull(12))
                                {
                                    payment.Overdue = 0;
                                }
                                else
                                {
                                    payment.Overdue = myReader.GetDecimal(12);
                                }
                                if (myReader.IsDBNull(13))
                                {
                                    payment.Debit = 0;
                                }
                                else
                                {
                                    payment.Debit = myReader.GetDecimal(13);
                                }
                                if (myReader.IsDBNull(14))
                                {
                                    payment.Credit = 0;
                                }
                                else
                                {
                                    payment.Credit = myReader.GetDecimal(14);
                                }
                                if (myReader.IsDBNull(15))
                                {
                                    payment.Remarks = String.Empty;
                                }
                                else
                                {
                                    payment.Remarks = myReader.GetString(15);
                                }
                                if (myReader.FieldCount > 16)
                                {
                                    if (myReader.IsDBNull(16))
                                    {
                                        payment.Branch = 0;
                                    }
                                    else
                                    {
                                        payment.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(16).ToString());
                                    }
                                }
                                if (myReader.FieldCount > 17)
                                {
                                    if (myReader.IsDBNull(17))
                                    {
                                        payment.AuditID = 0;
                                    }
                                    else
                                    {
                                        payment.AuditID = myReader.GetInt32(17);
                                    }
                                }
                                allPayment.Add(payment);
                            }
                        }
                    }
                }
                finally
                {
                    this._conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return allPayment;
        }

        public GenericList<Payment> GetAllPayment(Int32 pageNo, SortByPayment sortBy, SortingOrder sortOrder)
        {
            GenericList<Payment> allPayment = new GenericList<Payment>();
            ISalesProvider salesProvider;
            if (this._isLocal)
            {
                if (this._branch == Branch.Audit_)
                {
                    salesProvider = new SalesProvider(Database.AuditConnection());
                }
                else
                {
                    salesProvider = new SalesProvider(Database.GeneralLedger);
                }
            }
            else
            {
                salesProvider = new SalesProvider(Database.BranchConnection(this._branch));
            }
            SalesManager salesManager = new SalesManager(salesProvider);

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "PaymentSelect";
            myCommand.Connection = this._conn;

            if (pageNo >= 0)
            {
                SqlParameter myParam1 = new SqlParameter("@PageNo", SqlDbType.TinyInt);
                myParam1.Value = pageNo;
                SqlParameter myParam2 = new SqlParameter("@PageSize", SqlDbType.TinyInt);
                myParam2.Value = Utility.PageSize;
                SqlParameter myParam3 = new SqlParameter("@SortBy", SqlDbType.TinyInt);
                myParam3.Value = (Byte)sortBy;
                SqlParameter myParam4 = new SqlParameter("@SortOrder", SqlDbType.Bit);
                myParam4.Value = (Byte)sortOrder;
                myCommand.Parameters.Add(myParam1);
                myCommand.Parameters.Add(myParam2);
                myCommand.Parameters.Add(myParam3);
                myCommand.Parameters.Add(myParam4);
            }
            try
            {
                this._conn.Open();
                try
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            while (myReader.Read())
                            {
                                Payment payment = Payment.CreatePayment();
                                payment.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    payment.PaymentSales = null;
                                }
                                else
                                {
                                    payment.PaymentSales = salesManager.GetSales(myReader.GetInt32(1));
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    payment.PaymentNo = String.Empty;
                                }
                                else
                                {
                                    payment.PaymentNo = myReader.GetString(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    payment.PaymentDate = DateTime.Today;
                                }
                                else
                                {
                                    payment.PaymentDate = myReader.GetDateTime(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    payment.PaymentAmount = 0;
                                }
                                else
                                {
                                    payment.PaymentAmount = myReader.GetDecimal(4);
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    payment.Rebate = 0;
                                }
                                else
                                {
                                    payment.Rebate = myReader.GetDecimal(5);
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    payment.Mode = PaymentMode.Cash;
                                }
                                else
                                {
                                    payment.Mode = (PaymentMode)Enum.Parse(typeof(PaymentMode), myReader.GetInt32(6).ToString());
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    payment.CheckNo = String.Empty;
                                }
                                else
                                {
                                    payment.CheckNo = myReader.GetString(7).ToString();
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    payment.Status = PaymentStatus.Processing;
                                }
                                else
                                {
                                    payment.Status = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), myReader.GetInt32(8).ToString());
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    payment.InstNo = 0;
                                }
                                else
                                {
                                    payment.InstNo = myReader.GetInt32(9);
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    payment.MonthApplied = String.Empty;
                                }
                                else
                                {
                                    payment.MonthApplied = myReader.GetString(10).ToString();
                                }
                                if (myReader.IsDBNull(11))
                                {
                                    payment.Due = 0;
                                }
                                else
                                {
                                    payment.Due = myReader.GetDecimal(11);
                                }
                                if (myReader.IsDBNull(12))
                                {
                                    payment.Overdue = 0;
                                }
                                else
                                {
                                    payment.Overdue = myReader.GetDecimal(12);
                                }
                                if (myReader.IsDBNull(13))
                                {
                                    payment.Debit = 0;
                                }
                                else
                                {
                                    payment.Debit = myReader.GetDecimal(13);
                                }
                                if (myReader.IsDBNull(14))
                                {
                                    payment.Credit = 0;
                                }
                                else
                                {
                                    payment.Credit = myReader.GetDecimal(14);
                                }
                                if (myReader.IsDBNull(15))
                                {
                                    payment.Remarks = String.Empty;
                                }
                                else
                                {
                                    payment.Remarks = myReader.GetString(15);
                                }
                                if (myReader.FieldCount > 16)
                                {
                                    if (myReader.IsDBNull(16))
                                    {
                                        payment.Branch = 0;
                                    }
                                    else
                                    {
                                        payment.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(16).ToString());
                                    }
                                }
                                if (myReader.FieldCount > 17)
                                {
                                    if (myReader.IsDBNull(17))
                                    {
                                        payment.AuditID = 0;
                                    }
                                    else
                                    {
                                        payment.AuditID = myReader.GetInt32(17);
                                    }
                                }
                                allPayment.Add(payment);
                            }
                        }
                    }
                }
                finally
                {
                    this._conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return allPayment;
        }

        public Payment InsertPayment(Payment payment)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "PaymentInsert";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@SalesID", SqlDbType.Int);
            if (payment.PaymentSales == null)
            {
                myParam1.Value = DBNull.Value;
            }
            else
            {
                myParam1.Value = payment.PaymentSales.ID;
            }
            SqlParameter myParam2 = new SqlParameter("@PaymentNo", SqlDbType.VarChar, 16);
            if (String.IsNullOrEmpty(payment.PaymentNo))
            {
                myParam2.Value = DBNull.Value;
            }
            else
            {
                myParam2.Value = payment.PaymentNo;
            }
            SqlParameter myParam3 = new SqlParameter("@PaymentDate", SqlDbType.DateTime);
            myParam3.Value = payment.PaymentDate;
            SqlParameter myParam4 = new SqlParameter("@PaymentAmount", SqlDbType.Decimal);
            if (payment.PaymentAmount == 0)
            {
                myParam4.Value = DBNull.Value;
            }
            else
            {
                myParam4.Value = payment.PaymentAmount;
            }
            SqlParameter myParam5 = new SqlParameter("@Rebate", SqlDbType.Decimal);
            if (payment.Rebate == 0)
            {
                myParam5.Value = DBNull.Value;
            }
            else
            {
                myParam5.Value = payment.Rebate;
            }
            SqlParameter myParam6 = new SqlParameter("@Mode", SqlDbType.Int);
            if (payment.Mode == 0)
            {
                myParam6.Value = DBNull.Value;
            }
            else
            {
                myParam6.Value = payment.Mode;
            }
            SqlParameter myParam7 = new SqlParameter("@CheckNo", SqlDbType.VarChar, 16);
            if (String.IsNullOrEmpty(payment.CheckNo))
            {
                myParam7.Value = DBNull.Value;
            }
            else
            {
                myParam7.Value = payment.CheckNo;
            }
            SqlParameter myParam8 = new SqlParameter("@Status", SqlDbType.Int);
            if (payment.Status == 0)
            {
                myParam8.Value = DBNull.Value;
            }
            else
            {
                myParam8.Value = payment.Status;
            }
            SqlParameter myParam9 = new SqlParameter("@InstNo", SqlDbType.Int);
            if (payment.InstNo == 0)
            {
                myParam9.Value = DBNull.Value;
            }
            else
            {
                myParam9.Value = payment.InstNo;
            }
            SqlParameter myParam10 = new SqlParameter("@MonthApplied", SqlDbType.VarChar, 16);
            if (String.IsNullOrEmpty(payment.MonthApplied))
            {
                myParam10.Value = DBNull.Value;
            }
            else
            {
                myParam10.Value = payment.MonthApplied;
            }
            SqlParameter myParam11 = new SqlParameter("@Due", SqlDbType.Decimal);
            if (payment.Due == 0)
            {
                myParam11.Value = DBNull.Value;
            }
            else
            {
                myParam11.Value = payment.Due;
            }
            SqlParameter myParam12 = new SqlParameter("@Overdue", SqlDbType.Decimal);
            if (payment.Overdue == 0)
            {
                myParam12.Value = DBNull.Value;
            }
            else
            {
                myParam12.Value = payment.Overdue;
            }
            SqlParameter myParam13 = new SqlParameter("@Debit", SqlDbType.Decimal);
            if (payment.Debit == 0)
            {
                myParam13.Value = DBNull.Value;
            }
            else
            {
                myParam13.Value = payment.Debit;
            }
            SqlParameter myParam14 = new SqlParameter("@Credit", SqlDbType.Decimal);
            if (payment.Credit == 0)
            {
                myParam14.Value = DBNull.Value;
            }
            else
            {
                myParam14.Value = payment.Credit;
            }
            SqlParameter myParam15 = new SqlParameter("@Remarks", SqlDbType.VarChar, 1024);
            if (String.IsNullOrEmpty(payment.Remarks))
            {
                myParam15.Value = DBNull.Value;
            }
            else
            {
                myParam15.Value = payment.Remarks;
            }
            SqlParameter myParam16 = new SqlParameter("@BranchID", SqlDbType.Int);
            myParam16.Value = (Int32)payment.Branch;
            SqlParameter myParam17 = new SqlParameter("@AuditID", SqlDbType.Int);
            myParam17.Value = payment.AuditID;
            SqlParameter myParam18 = new SqlParameter("@Output", SqlDbType.Int);
            myParam18.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(myParam1);
            myCommand.Parameters.Add(myParam2);
            myCommand.Parameters.Add(myParam3);
            myCommand.Parameters.Add(myParam4);
            myCommand.Parameters.Add(myParam5);
            myCommand.Parameters.Add(myParam6);
            myCommand.Parameters.Add(myParam7);
            myCommand.Parameters.Add(myParam8);
            myCommand.Parameters.Add(myParam9);
            myCommand.Parameters.Add(myParam10);
            myCommand.Parameters.Add(myParam11);
            myCommand.Parameters.Add(myParam12);
            myCommand.Parameters.Add(myParam13);
            myCommand.Parameters.Add(myParam14);
            myCommand.Parameters.Add(myParam15);
            myCommand.Parameters.Add(myParam16);
            myCommand.Parameters.Add(myParam17);
            myCommand.Parameters.Add(myParam18);
            try
            {
                this._conn.Open();
                try
                {
                    myCommand.ExecuteNonQuery();
                    payment.ID = Convert.ToInt32(myParam18.Value);
                }
                finally
                {
                    this._conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return payment;
        }

        public Boolean UpdatePayment(Payment payment)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "PaymentUpdate";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@ID", SqlDbType.Int);
            myParam1.Value = payment.ID;
            SqlParameter myParam2 = new SqlParameter("@SalesID", SqlDbType.Int);
            if (payment.PaymentSales == null)
            {
                myParam2.Value = DBNull.Value;
            }
            else
            {
                myParam2.Value = payment.PaymentSales.ID;
            }
            SqlParameter myParam3 = new SqlParameter("@PaymentNo", SqlDbType.VarChar, 16);
            if (String.IsNullOrEmpty(payment.PaymentNo))
            {
                myParam3.Value = DBNull.Value;
            }
            else
            {
                myParam3.Value = payment.PaymentNo;
            }
            SqlParameter myParam4 = new SqlParameter("@PaymentDate", SqlDbType.DateTime);
            myParam4.Value = payment.PaymentDate;
            SqlParameter myParam5 = new SqlParameter("@PaymentAmount", SqlDbType.Decimal);
            if (payment.PaymentAmount == 0)
            {
                myParam5.Value = DBNull.Value;
            }
            else
            {
                myParam5.Value = payment.PaymentAmount;
            }
            SqlParameter myParam6 = new SqlParameter("@Rebate", SqlDbType.Decimal);
            if (payment.Rebate == 0)
            {
                myParam6.Value = DBNull.Value;
            }
            else
            {
                myParam6.Value = payment.Rebate;
            }
            SqlParameter myParam7 = new SqlParameter("@Mode", SqlDbType.Int);
            if (payment.Mode == 0)
            {
                myParam7.Value = DBNull.Value;
            }
            else
            {
                myParam7.Value = payment.Mode;
            }
            SqlParameter myParam8 = new SqlParameter("@CheckNo", SqlDbType.VarChar, 16);
            if (String.IsNullOrEmpty(payment.CheckNo))
            {
                myParam8.Value = DBNull.Value;
            }
            else
            {
                myParam8.Value = payment.CheckNo;
            }
            SqlParameter myParam9 = new SqlParameter("@Status", SqlDbType.Int);
            if (payment.Status == 0)
            {
                myParam9.Value = DBNull.Value;
            }
            else
            {
                myParam9.Value = payment.Status;
            }
            SqlParameter myParam10 = new SqlParameter("@InstNo", SqlDbType.Int);
            if (payment.InstNo == 0)
            {
                myParam10.Value = DBNull.Value;
            }
            else
            {
                myParam10.Value = payment.InstNo;
            }
            SqlParameter myParam11 = new SqlParameter("@MonthApplied", SqlDbType.VarChar, 16);
            if (String.IsNullOrEmpty(payment.MonthApplied))
            {
                myParam11.Value = DBNull.Value;
            }
            else
            {
                myParam11.Value = payment.MonthApplied;
            }
            SqlParameter myParam12 = new SqlParameter("@Due", SqlDbType.Decimal);
            if (payment.Due == 0)
            {
                myParam12.Value = DBNull.Value;
            }
            else
            {
                myParam12.Value = payment.Due;
            }
            SqlParameter myParam13 = new SqlParameter("@Overdue", SqlDbType.Decimal);
            if (payment.Overdue == 0)
            {
                myParam13.Value = DBNull.Value;
            }
            else
            {
                myParam13.Value = payment.Overdue;
            }
            SqlParameter myParam14 = new SqlParameter("@Debit", SqlDbType.Decimal);
            if (payment.Debit == 0)
            {
                myParam14.Value = DBNull.Value;
            }
            else
            {
                myParam14.Value = payment.Debit;
            }
            SqlParameter myParam15 = new SqlParameter("@Credit", SqlDbType.Decimal);
            if (payment.Credit == 0)
            {
                myParam15.Value = DBNull.Value;
            }
            else
            {
                myParam15.Value = payment.Credit;
            }
            SqlParameter myParam16 = new SqlParameter("@Remarks", SqlDbType.VarChar, 1024);
            if (String.IsNullOrEmpty(payment.Remarks))
            {
                myParam16.Value = DBNull.Value;
            }
            else
            {
                myParam16.Value = payment.Remarks;
            }
            SqlParameter myParam17 = new SqlParameter("@ReturnValue", SqlDbType.Int);
            myParam17.Direction = ParameterDirection.ReturnValue;
            myCommand.Parameters.Add(myParam1);
            myCommand.Parameters.Add(myParam2);
            myCommand.Parameters.Add(myParam3);
            myCommand.Parameters.Add(myParam4);
            myCommand.Parameters.Add(myParam5);
            myCommand.Parameters.Add(myParam6);
            myCommand.Parameters.Add(myParam7);
            myCommand.Parameters.Add(myParam8);
            myCommand.Parameters.Add(myParam9);
            myCommand.Parameters.Add(myParam10);
            myCommand.Parameters.Add(myParam11);
            myCommand.Parameters.Add(myParam12);
            myCommand.Parameters.Add(myParam13);
            myCommand.Parameters.Add(myParam14);
            myCommand.Parameters.Add(myParam15);
            myCommand.Parameters.Add(myParam16);
            myCommand.Parameters.Add(myParam17);
            try
            {
                this._conn.Open();
                myCommand.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            finally
            {
                this._conn.Close();
            }
            if (Convert.ToInt32(myParam17.Value) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean DeletePayment(Payment payment)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "PaymentDelete";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@ID", SqlDbType.Int);
            myParam1.Value = payment.ID;
            SqlParameter myParam2 = new SqlParameter("@ReturnValue", SqlDbType.Int);
            myParam2.Direction = ParameterDirection.ReturnValue;
            myCommand.Parameters.Add(myParam1);
            myCommand.Parameters.Add(myParam2);
            try
            {
                this._conn.Open();
                myCommand.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            finally
            {
                this._conn.Close();
            }
            if (Convert.ToInt32(myParam2.Value) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Payment FindPayment(String searchString, String searchCriteria)
        {
            Payment payment = Payment.CreatePayment();
            ISalesProvider salesProvider;
            if (this._isLocal)
            {
                if (this._branch == Branch.Audit_)
                {
                    salesProvider = new SalesProvider(Database.AuditConnection());
                }
                else
                {
                    salesProvider = new SalesProvider(Database.GeneralLedger);
                }
            }
            else
            {
                salesProvider = new SalesProvider(Database.BranchConnection(this._branch));
            }
            SalesManager salesManager = new SalesManager(salesProvider);

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "PaymentFind";
            myCommand.Connection = this._conn;
            SqlParameter myParam1;
            if (searchCriteria == "Payment no.")
            {
                myParam1 = new SqlParameter("@PaymentNo", SqlDbType.VarChar, 16);
            }
            else
            {
                myParam1 = new SqlParameter("@CheckNo", SqlDbType.VarChar, 16);
            }
            myParam1.Value = searchString;
            myCommand.Parameters.Add(myParam1);
            try
            {
                this._conn.Open();
                try
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            myReader.Read();
                            payment.ID = myReader.GetInt32(0);
                            if (myReader.IsDBNull(1))
                            {
                                payment.PaymentSales = null;
                            }
                            else
                            {
                                payment.PaymentSales = salesManager.GetSales(myReader.GetInt32(1));
                            }
                            if (myReader.IsDBNull(2))
                            {
                                payment.PaymentNo = String.Empty;
                            }
                            else
                            {
                                payment.PaymentNo = myReader.GetString(2);
                            }
                            if (myReader.IsDBNull(3))
                            {
                                payment.PaymentDate = DateTime.Today;
                            }
                            else
                            {
                                payment.PaymentDate = myReader.GetDateTime(3);
                            }
                            if (myReader.IsDBNull(4))
                            {
                                payment.PaymentAmount = 0;
                            }
                            else
                            {
                                payment.PaymentAmount = myReader.GetDecimal(4);
                            }
                            if (myReader.IsDBNull(5))
                            {
                                payment.Rebate = 0;
                            }
                            else
                            {
                                payment.Rebate = myReader.GetDecimal(5);
                            }
                            if (myReader.IsDBNull(6))
                            {
                                payment.Mode = PaymentMode.Cash;
                            }
                            else
                            {
                                payment.Mode = (PaymentMode)Enum.Parse(typeof(PaymentMode), myReader.GetInt32(6).ToString());
                            }
                            if (myReader.IsDBNull(7))
                            {
                                payment.CheckNo = String.Empty;
                            }
                            else
                            {
                                payment.CheckNo = myReader.GetString(7).ToString();
                            }
                            if (myReader.IsDBNull(8))
                            {
                                payment.Status = PaymentStatus.Processing;
                            }
                            else
                            {
                                payment.Status = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), myReader.GetInt32(8).ToString());
                            }
                            if (myReader.IsDBNull(9))
                            {
                                payment.InstNo = 0;
                            }
                            else
                            {
                                payment.InstNo = myReader.GetInt32(9);
                            }
                            if (myReader.IsDBNull(10))
                            {
                                payment.MonthApplied = String.Empty;
                            }
                            else
                            {
                                payment.MonthApplied = myReader.GetString(10).ToString();
                            }
                            if (myReader.IsDBNull(11))
                            {
                                payment.Due = 0;
                            }
                            else
                            {
                                payment.Due = myReader.GetDecimal(11);
                            }
                            if (myReader.IsDBNull(12))
                            {
                                payment.Overdue = 0;
                            }
                            else
                            {
                                payment.Overdue = myReader.GetDecimal(12);
                            }
                            if (myReader.IsDBNull(13))
                            {
                                payment.Debit = 0;
                            }
                            else
                            {
                                payment.Debit = myReader.GetDecimal(13);
                            }
                            if (myReader.IsDBNull(14))
                            {
                                payment.Credit = 0;
                            }
                            else
                            {
                                payment.Credit = myReader.GetDecimal(14);
                            }
                            if (myReader.IsDBNull(15))
                            {
                                payment.Remarks = String.Empty;
                            }
                            else
                            {
                                payment.Remarks = myReader.GetString(15);
                            }
                        }
                    }
                }
                finally
                {
                    this._conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return payment;
        }
        #endregion
    }
}
