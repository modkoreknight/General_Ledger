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
    public class ReceivableProvider : IReceivableProvider
    {
        #region Fields
        private SqlConnection _conn;
        private Sales _sales;
        private GenericList<Payment> _allPayment;
        private Receivable _receivable = Receivable.CreateReceivable();
        private Int32 _monthsPaid;
        private Int32 _receivableMonth;
        private Int32 _receivableYear;
        #endregion

        #region Properties
        public Sales Sales
        {
            get
            {
                return this._sales;
            }
            set
            {
                if (this._sales != value)
                {
                    this._sales = value;
                }
            }
        }

        public GenericList<Payment> AllPayment
        {
            get
            {
                return this._allPayment;
            }
            set
            {
                if (this._allPayment != value)
                {
                    this._allPayment = value;
                }
            }
        }
        #endregion

        #region Constructors
        public ReceivableProvider(SqlConnection conn)
        {
            this._conn = conn;
        }

        public ReceivableProvider(SqlConnection conn, Sales sales, GenericList<Payment> allPayment)
        {
            this._conn = conn;
            this._sales = sales;
            this._allPayment = allPayment;
        }
        #endregion

        #region Methods
        private void ComputeTotalPayment()
        {
            this._receivable.TotalPayment = this._allPayment.Where(q => q.InstNo != 0).Sum(q => q.PaymentAmount) + this._allPayment.Where(q => q.InstNo != 0).Sum(q => q.Rebate);
        }

        private void ComputeTotalMonthsPaid()
        {
            this._monthsPaid = (Int32)Math.Floor(this._receivable.TotalPayment / this._sales.AmortAmount);
        }

        private void ComputeInstNo()
        {
            //if (this._monthsPaid == 0)
            //{
            //    this._receivable.InstNo = 0;
            //}
            //else
            //{
                this._receivable.InstNo = this._monthsPaid + 1;
            //}
        }

        private void ComputeMonthApplied()
        {
            this._receivableMonth = this._sales.AmortStartDate.Month + this._monthsPaid;
            this._receivableYear = this._sales.AmortStartDate.Year + (this._receivableMonth / 12);
            //this._receivableYear = (this._sales.AmortStartDate.Year + (this._receivableMonth / 12)) + (DateTime.Today.Year - this._sales.AmortStartDate.Year);
            this._receivableMonth = this._receivableMonth % 12;
            if (this._receivableMonth == 0)
            {
                this._receivableMonth = 12;
                this._receivableYear--;
            }
            //if (this._receivable.InstNo == 0)
            //{
            //    this._receivable.MonthApplied = String.Empty;
            //}
            //else
            //{
                this._receivable.MonthApplied = this._receivableMonth.ToString("0#") + "/" + this._receivableYear.ToString();
            //}
        }

        private void ComputeDue()
        {
            this._receivable.Due = (this._receivable.InstNo * this._sales.AmortAmount) - this._receivable.TotalPayment;
        }

        private void ComputeOverdue()
        {
            Int32 currentMonth = DateTime.Today.Month;
            if (DateTime.Today.Day >= this._sales.AmortStartDate.Day)
            {
                if (this._receivableYear > DateTime.Today.Year)
                {
                    currentMonth = 0;
                }
                else
                {
                    currentMonth = currentMonth - this._receivableMonth;
                }
            }
            else
            {
                currentMonth = currentMonth - this._receivableMonth - 1;
            }
            this._receivable.Overdue = currentMonth * this._sales.AmortAmount;
            if (this._receivable.Overdue < 0)
            {
                this._receivable.Overdue = 0;
            }
            Decimal overdue = this._receivable.Due + this._receivable.Overdue;
            if (overdue > this._receivable.Sales.AmortAmount)
            {
                this._receivable.Overdue30 = this._receivable.Sales.AmortAmount;
                overdue -= this._receivable.Sales.AmortAmount;
                if (overdue > this._receivable.Sales.AmortAmount)
                {
                    this._receivable.Overdue60 = this._receivable.Sales.AmortAmount;
                    overdue -= this._receivable.Sales.AmortAmount;
                    this._receivable.Overdue90 = overdue;
                }
                else
                {
                    this._receivable.Overdue60 = overdue;
                    this._receivable.Overdue90 = 0;
                }
            }
            else
            {
                this._receivable.Overdue30 = overdue;
                this._receivable.Overdue60 = 0;
                this._receivable.Overdue90 = 0;
            }
        }

        private void ComputeBalance()
        {
            this._receivable.RemainingBalance = this._sales.SaleAmount - this._receivable.TotalPayment;
        }

        private void ComputeDueDate()
        {
            this._receivable.DueDate = this._sales.AmortStartDate.AddMonths(this._monthsPaid);
        }

        public Receivable GetReceivable()
        {
            this._receivable.Sales = this._sales;
            this.ComputeTotalPayment();
            this.ComputeTotalMonthsPaid();
            this.ComputeInstNo();
            this.ComputeMonthApplied();
            this.ComputeDue();
            this.ComputeOverdue();
            this.ComputeBalance();
            this.ComputeDueDate();
            this._receivable.Branch = this._sales.Branch;
            this._receivable.AuditID = this._sales.AuditID;
            return this._receivable;
        }

        public Receivable GetReceivable(Int32 monthsToPay)
        {
            this._receivable.Sales = this._sales; 
            this._monthsPaid = monthsToPay;
            this.ComputeTotalPayment();
            this.ComputeInstNo();
            this.ComputeMonthApplied();
            this.ComputeDue();
            this.ComputeOverdue();
            this.ComputeBalance();
            this.ComputeDueDate();
            this._receivable.Branch = this._sales.Branch;
            this._receivable.AuditID = this._sales.AuditID;
            return this._receivable;
        }

        public Receivable GetReceivable(DateTime cutoff)
        {
            Receivable receivable = Receivable.CreateReceivable();

            Int32 monthDiff = 0;
            if (this._allPayment.Count != 0)
            {
                //Implementation changes due to issue on July 18, 2011
                //Int32 maxID = this._allPayment.Max(q => q.ID);
                //Payment lastPayment = this._allPayment.Single(q => q.ID == maxID);
                Int32 maxID = this._allPayment.Max(q => q.InstNo);
                Payment lastPayment = this._allPayment.First(q => q.InstNo == maxID);
                this._receivable.LastPaymentDate = lastPayment.PaymentDate;
                this._receivable.LastMonthApplied = lastPayment.MonthApplied;
                if (!String.IsNullOrEmpty(this._receivable.LastMonthApplied))
                {
                    DateTime dateApplied = DateTime.Parse(lastPayment.MonthApplied);
                    //if (this._sales.ID == 443 || this._sales.ID == 79 || this._sales.ID == 4302)
                    //{
                    //    String x = String.Empty;
                    //}
                    for (Int32 i = 1; i <= 100; i++)
                    {
                        if (this._sales.AmortStartDate.AddMonths(i) > cutoff)
                        {
                            monthDiff = i - 1;
                            receivable = this.GetReceivable(monthDiff);
                            break;
                        }
                    }
                    ////monthDiff is the difference of last MonthApplied and cutoff month
                    //monthDiff = 12 * (cutoff.Year - dateApplied.Year) + (cutoff.Month - dateApplied.Month);
                    //monthDiff = monthDiff + (12 * (dateApplied.Year - this._sales.AmortStartDate.Year));
                    //////while (this._sales.AmortStartDate.Month + monthDiff < cutoff.Month)
                    ////while (this._sales.AmortStartDate.Month + monthDiff < cutoff.Month + (12 * (this._sales.AmortStartDate.Year - dateApplied.Year)))
                    ////{
                    ////    monthDiff++;
                    ////}
                    ////while (this._sales.AmortStartDate.Month + monthDiff > cutoff.Month)
                    ////{
                    ////    monthDiff--;
                    ////}
                    //////monthDiff = monthDiff + (12 * (dateApplied.Year - this._sales.AmortStartDate.Year));
                }
            }
            else
            {
                receivable.LastMonthApplied = "No payment yet";
            }
            ////if (this._sales.AmortStartDate.AddMonths(monthDiff).Day > cutoff.Day)
            //if (this._sales.AmortStartDate.AddMonths(monthDiff) > cutoff)
            //{
            //    //if (this._sales.AmortStartDate.AddMonths(monthDiff).Year > cutoff.Year)
            //    //{
            //    //    Int32 diffYear = this._sales.AmortStartDate.AddMonths(monthDiff).Year - cutoff.Year;
            //    //}
            //    //if (this._sales.AmortStartDate.AddMonths(monthDiff).Month > cutoff.Month)
            //    //{
            //    //    Int32 diffMonth = this._sales.AmortStartDate.AddMonths(monthDiff).Month - cutoff.Month;
            //    //    monthDiff--;
            //    //}
            //    TimeSpan diff = this._sales.AmortStartDate.AddMonths(monthDiff).Subtract(cutoff);
            //    monthDiff = monthDiff - (diff.Days / 30);
            //}
            //if (this._sales.AmortStartDate.AddMonths(monthDiff).Day > cutoff.Day)
            //{
            //    monthDiff--;
            //}
            //if (monthDiff < 0)
            //{
            //    monthDiff = 12 + monthDiff;
            //}
            receivable = this.GetReceivable(monthDiff);
            //if (receivable.DueDate.Month == cutoff.Month && receivable.DueDate.Day > cutoff.Day && receivable.DueDate.Year == cutoff.Year)
            //{
            //    receivable.DueDate = receivable.DueDate.AddMonths(-1);
            //    receivable.Due = receivable.Due - receivable.AmortAmount;
            //}

            this._receivable.Branch = this._sales.Branch;
            this._receivable.AuditID = this._sales.AuditID;
            return receivable;
        }
        #endregion
    }
}
