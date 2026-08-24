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
    public class LedgerProvider : ILedgerProvider
    {
        #region Fields
        private SqlConnection _conn;
        private Boolean _isLocal;
        private Branch _branch;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public LedgerProvider(SqlConnection conn)
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
                //if (this._branch == Branch.Audit_)
                //{
                //    this._isLocal = false;
                //}
                //else
                //{
                //    this._isLocal = true;
                //}
                this._isLocal = true;
            }
        }
        #endregion

        #region Methods
        private GenericList<Ledger> GetItems(Sales sales)
        {
            GenericList<Ledger> allLedger = new GenericList<Ledger>();

            //Sales
            ISalesProvider salesProvider;
            //TODO:  Test:  If Audit then use Audit database
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
            GenericList<Sales> allSales = salesManager.GetAllSales(sales.Vehicle);
            foreach (Sales s in allSales)
            {
                Ledger salesLedger = Ledger.CreateLedger();
                salesLedger.Date = s.SaleDate;
                salesLedger.Record = s.SaleCode;
                salesLedger.Detail = "Beginning balance";
                salesLedger.InstNo = 0;
                //salesLedger.MonthApplied = salesLedger.Date.ToString("MM/yyyy");
                salesLedger.MonthApplied = salesLedger.MonthApplied;
                salesLedger.Due = 0;
                salesLedger.Overdue = 0;
                salesLedger.Rebate = 0;
                salesLedger.Debit = s.SaleAmount;
                salesLedger.Credit = 0;
                salesLedger.Balance = 0;
                salesLedger.Remarks = s.Remarks;
                salesLedger.Source = LedgerSource.Sales;
                salesLedger.SourceID = s.ID;
                allLedger.Add(salesLedger);
            }

            //Receivables
            //DateTime receivableDate = sales.AmortStartDate;
            //for (Int32 i = 1; i <= sales.TermTotal; i++)
            //{
            //    Ledger receivableLedger = Ledger.CreateLedger();
            //    receivableLedger.Date = receivableDate;
            //    receivableLedger.Record = String.Empty;
            //    receivableLedger.Detail = "Amortization " + i.ToString();
            //    receivableLedger.Overdue = 0;
            //    receivableLedger.Debit = sales.AmortAmount;
            //    receivableLedger.Credit = 0;
            //    receivableLedger.Balance = 0;
            //    receivableLedger.Remarks = String.Empty;
            //    receivableLedger.Source = LedgerSource.Receivables;
            //    receivableLedger.SourceID = 0;
            //    allLedger.Add(receivableLedger);
            //}

            //Payments
            IPaymentProvider paymentProvider;
            if (this._isLocal)
            {
                if (this._branch == Branch.Audit_)
                {
                    paymentProvider = new PaymentProvider(Database.AuditConnection());
                }
                else
                {
                    paymentProvider = new PaymentProvider(Database.GeneralLedger);
                }
            }
            else
            {
                paymentProvider = new PaymentProvider(Database.BranchConnection(this._branch));
            }
            PaymentManager paymentManager = new PaymentManager(paymentProvider);
            foreach (Sales s in allSales)
            {
                GenericList<Payment> allPayment = paymentManager.GetAllPayment(s);
                for (Int32 i = 1; i <= allPayment.Count; i++)
                {
                    Payment payment = allPayment[i - 1];
                    Ledger paymentLedger = Ledger.CreateLedger();
                    paymentLedger.Date = payment.PaymentDate;
                    paymentLedger.Record = payment.PaymentNo;
                    paymentLedger.Detail = "Payment " + i.ToString();
                    paymentLedger.InstNo = payment.InstNo;
                    //paymentLedger.MonthApplied = payment.PaymentDate.ToString("MM/yyyy");
                    paymentLedger.MonthApplied = payment.MonthApplied;
                    paymentLedger.Due = payment.Due;
                    paymentLedger.Overdue = payment.Overdue;
                    Decimal overdue = paymentLedger.Overdue;
                    if (overdue > s.AmortAmount)
                    {
                        paymentLedger.Overdue30 = s.AmortAmount;
                        overdue -= s.AmortAmount;
                        if (overdue > s.AmortAmount)
                        {
                            paymentLedger.Overdue60 = s.AmortAmount;
                            overdue -= s.AmortAmount;
                            paymentLedger.Overdue90 = overdue;
                        }
                        else
                        {
                            paymentLedger.Overdue60 = overdue;
                            paymentLedger.Overdue90 = 0;
                        }
                    }
                    else
                    {
                        paymentLedger.Overdue30 = overdue;
                        paymentLedger.Overdue60 = 0;
                        paymentLedger.Overdue90 = 0;
                    }
                    paymentLedger.Payment = payment.PaymentAmount;
                    paymentLedger.Rebate = payment.Rebate;
                    //paymentLedger.Debit = 0;
                    //paymentLedger.Credit = payment.PaymentAmount;
                    paymentLedger.Debit = payment.Debit;
                    paymentLedger.Credit = payment.Credit;
                    paymentLedger.Balance = 0;
                    paymentLedger.Remarks = payment.Remarks;
                    paymentLedger.Source = LedgerSource.Payments;
                    paymentLedger.SourceID = payment.ID;
                    allLedger.Add(paymentLedger);
                }
            }

            return allLedger;
        }

        private GenericList<Ledger> GetItems(Vehicle vehicle)
        {
            GenericList<Ledger> allLedger = new GenericList<Ledger>();
            return allLedger;
        }

        private GenericList<Ledger> ComputeItems(GenericList<Ledger> allLedger)
        {
            Decimal balance = 0;
            for (Int32 i = 0; i < allLedger.Count; i++)
            {
                switch (allLedger[i].Source)
                {
                    case LedgerSource.Sales:
                        balance = allLedger[i].Debit;
                        break;
                    case LedgerSource.Payments:
                        balance = (balance + allLedger[i].Debit) - allLedger[i].Credit;
                        break;
                }
                allLedger[i].Balance = balance;
                allLedger[i].BalanceTotal = balance;
            }

            return allLedger;
        }

        public GenericList<Ledger> GetLedger(Sales sales)
        {
            GenericList<Ledger> allLedger = this.GetItems(sales);

            System.ComponentModel.PropertyDescriptorCollection properties = System.ComponentModel.TypeDescriptor.GetProperties(typeof(Ledger));
            System.ComponentModel.ListSortDescription[] sortDescs = new System.ComponentModel.ListSortDescription[2];
            //Sort items by Ledger.Date and Ledger.Source
            sortDescs[0] = new System.ComponentModel.ListSortDescription(properties[0], System.ComponentModel.ListSortDirection.Ascending);
            sortDescs[1] = new System.ComponentModel.ListSortDescription(properties[16], System.ComponentModel.ListSortDirection.Ascending);
            allLedger.ApplySort(new System.ComponentModel.ListSortDescriptionCollection(sortDescs));

            this.ComputeItems(allLedger);

            return allLedger;
        }
        #endregion
    }
}
