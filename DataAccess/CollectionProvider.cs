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
    public class CollectionProvider : ICollectionProvider
    {
        #region Fields
        private SqlConnection _conn;
        private Boolean _isLocal;
        private Branch _branch;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public CollectionProvider(SqlConnection conn)
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
        public GenericList<Receivable> GetAllReceivable(DateTime cutoff)
        {
            GenericList<Receivable> allReceivable = new GenericList<Receivable>();

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

            GenericList<Payment> allPayment = new GenericList<Payment>();
            GenericList<Sales> allSales = salesManager.GetAllSales(SalesStatus.Current);
            foreach (Sales sales in allSales)
            {
                allPayment = paymentManager.GetAllPayment(sales);
                Receivable receivable = Receivable.CreateReceivable();
                IReceivableProvider receivableProvider;
                if (this._isLocal)
                {
                    if (this._branch == Branch.Audit_)
                    {
                        receivableProvider = new ReceivableProvider(Database.AuditConnection());
                    }
                    else
                    {
                        receivableProvider = new ReceivableProvider(Database.GeneralLedger);
                    }
                }
                else
                {
                    receivableProvider = new ReceivableProvider(Database.BranchConnection(this._branch));
                }
                ReceivableManager receivableManager = new ReceivableManager(receivableProvider);
                receivableProvider.Sales = sales;
                receivableProvider.AllPayment = allPayment;
                receivable = receivableManager.GetReceivable(cutoff);
                if (receivable.Due == 0)
                {
                    String x = String.Empty;
                }
                if (receivable.Due > 0 || receivable.Overdue > 0)
                {
                    allReceivable.Add(receivable);
                }
            }

            return allReceivable;
        }

        //public GenericList<Receivable> GetAllReceivable(Zone zone, DateTime cutoff)
        //{
        //    GenericList<Receivable> allReceivable = new GenericList<Receivable>();

        //    ISalesProvider salesProvider;
        //    if (this._isLocal)
        //    {
        //        salesProvider = new SalesProvider(Database.GeneralLedger);
        //    }
        //    else
        //    {
        //        salesProvider = new SalesProvider(Database.BranchConnection(this._branch));
        //    }
        //    SalesManager salesManager = new SalesManager(salesProvider);
        //    IPaymentProvider paymentProvider;
        //    if (this._isLocal)
        //    {
        //        paymentProvider = new PaymentProvider(Database.GeneralLedger);
        //    }
        //    else
        //    {
        //        paymentProvider = new PaymentProvider(Database.BranchConnection(this._branch));
        //    }
        //    PaymentManager paymentManager = new PaymentManager(paymentProvider);

        //    GenericList<Payment> allPayment = new GenericList<Payment>();
        //    GenericList<Sales> allSales = salesManager.GetAllSales(SalesStatus.Current);
        //    foreach (Sales sales in allSales)
        //    {
        //        allPayment = paymentManager.GetAllPayment(sales);
        //        Receivable receivable = Receivable.CreateReceivable();
        //        IReceivableProvider receivableProvider;
        //        if (this._isLocal)
        //        {
        //            receivableProvider = new ReceivableProvider(Database.GeneralLedger);
        //        }
        //        else
        //        {
        //            receivableProvider = new ReceivableProvider(Database.BranchConnection(this._branch));
        //        }
        //        ReceivableManager receivableManager = new ReceivableManager(receivableProvider);
        //        receivableProvider.Sales = sales;
        //        receivableProvider.AllPayment = allPayment;
        //        receivable = receivableManager.GetReceivable(cutoff);
        //        if (receivable.Due > 0 || receivable.Overdue > 0)
        //        {
        //            allReceivable.Add(receivable);
        //        }
        //    }

        //    return allReceivable;
        //}

        public Receivable GetReceivable(DateTime cutoff, Sales sales)
        {
            Receivable receivable = Receivable.CreateReceivable();

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

            GenericList<Payment> allPayment = new GenericList<Payment>();
            allPayment = paymentManager.GetAllPayment(sales);
            IReceivableProvider receivableProvider;
            if (this._isLocal)
            {
                if (this._branch == Branch.Audit_)
                {
                    receivableProvider = new ReceivableProvider(Database.AuditConnection());
                }
                else
                {
                    receivableProvider = new ReceivableProvider(Database.GeneralLedger);
                }
            }
            else
            {
                receivableProvider = new ReceivableProvider(Database.BranchConnection(this._branch));
            }
            ReceivableManager receivableManager = new ReceivableManager(receivableProvider);
            receivableProvider.Sales = sales;
            receivableProvider.AllPayment = allPayment;
            receivable = receivableManager.GetReceivable(cutoff);

            return receivable;
        }
        #endregion
    }
}
