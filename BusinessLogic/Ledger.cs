using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class Ledger
    {
        #region Fields
        private DateTime _date = DateTime.Today;
        private String _record = String.Empty;
        private String _detail = String.Empty;
        private Int32 _instNo = 0;
        private String _monthApplied = String.Empty;
        private Decimal _due = 0.0M;
        private Decimal _overdue = 0.0M;
        private Decimal _overdue30 = 0.0M;
        private Decimal _overdue60 = 0.0M;
        private Decimal _overdue90 = 0.0M;
        private Decimal _payment = 0.0M;
        private Decimal _rebate = 0.0M;
        private Decimal _debit = 0.0M;
        private Decimal _credit = 0.0M;
        private Decimal _balance = 0.0M;
        private String _remarks;
        private LedgerSource _source;
        private Int32 _sourceID;
        private Decimal _debitTotal = 0.0M;
        private Decimal _creditTotal = 0.0M;
        private Decimal _balanceTotal = 0.0M;
        private Branch _branch;
        private Int32 _auditID;
        #endregion

        #region Properties
        [Description("Date")]
        public DateTime Date
        {
            get
            {
                return this._date;
            }
            set
            {
                if (this._date != value)
                {
                    this._date = value;
                }
            }
        }

        [Description("Record")]
        public String Record
        {
            get
            {
                return this._record;
            }
            set
            {
                if (this._record != value)
                {
                    this._record = value;
                }
            }
        }

        [Description("Detail")]
        public String Detail
        {
            get
            {
                return this._detail;
            }
            set
            {
                if (this._detail != value)
                {
                    this._detail = value;
                }
            }
        }

        [Description("Installment no.")]
        public Int32 InstNo
        {
            get
            {
                return this._instNo;
            }
            set
            {
                if (this._instNo != value)
                {
                    this._instNo = value;
                }
            }
        }

        [Description("Month applied")]
        public String MonthApplied
        {
            get
            {
                return this._monthApplied;
            }
            set
            {
                if (this._monthApplied != value)
                {
                    this._monthApplied = value;
                }
            }
        }

        [Description("Due")]
        public Decimal Due
        {
            get
            {
                return this._due;
            }
            set
            {
                if (this._due != value)
                {
                    this._due = value;
                }
            }
        }

        [Description("Overdue")]
        public Decimal Overdue
        {
            get
            {
                return this._overdue;
            }
            set
            {
                if (this._overdue != value)
                {
                    this._overdue = value;
                }
            }
        }

        [Description("Overdue30")]
        public Decimal Overdue30
        {
            get
            {
                return this._overdue30;
            }
            set
            {
                if (this._overdue30 != value)
                {
                    this._overdue30 = value;
                }
            }
        }

        [Description("Overdue60")]
        public Decimal Overdue60
        {
            get
            {
                return this._overdue60;
            }
            set
            {
                if (this._overdue60 != value)
                {
                    this._overdue60 = value;
                }
            }
        }

        [Description("Overdue90")]
        public Decimal Overdue90
        {
            get
            {
                return this._overdue90;
            }
            set
            {
                if (this._overdue90 != value)
                {
                    this._overdue90 = value;
                }
            }
        }

        [Description("Payment")]
        public Decimal Payment
        {
            get
            {
                return this._payment;
            }
            set
            {
                if (this._payment != value)
                {
                    this._payment = value;
                }
            }
        }
        
        [Description("Rebate")]
        public Decimal Rebate
        {
            get
            {
                return this._rebate;
            }
            set
            {
                if (this._rebate != value)
                {
                    this._rebate = value;
                }
            }
        }

        [Description("Debit")]
        public Decimal Debit
        {
            get
            {
                return this._debit;
            }
            set
            {
                if (this._debit != value)
                {
                    this._debit = value;
                }
            }
        }

        [Description("Credit")]
        public Decimal Credit
        {
            get
            {
                return this._credit;
            }
            set
            {
                if (this._credit != value)
                {
                    this._credit = value;
                }
            }
        }

        [Description("Balance")]
        public Decimal Balance
        {
            get
            {
                return this._balance;
            }
            set
            {
                if (this._balance != value)
                {
                    this._balance = value;
                }
            }
        }

        [Description("Remarks")]
        public String Remarks
        {
            get
            {
                return this._remarks;
            }
            set
            {
                if (this._remarks != value)
                {
                    this._remarks = value;
                }
            }
        }

        [Description("Source")]
        public LedgerSource Source
        {
            get
            {
                return this._source;
            }
            set
            {
                if (this._source != value)
                {
                    this._source = value;
                }
            }
        }

        [Description("SourceID")]
        public Int32 SourceID
        {
            get
            {
                return this._sourceID;
            }
            set
            {
                if (this._sourceID != value)
                {
                    this._sourceID = value;
                }
            }
        }

        [Description("Debit total")]
        public Decimal DebitTotal
        {
            get
            {
                return this._debitTotal;
            }
            set
            {
                if (this._debitTotal != value)
                {
                    this._debitTotal = value;
                }
            }
        }

        [Description("Credit total")]
        public Decimal CreditTotal
        {
            get
            {
                return this._creditTotal;
            }
            set
            {
                if (this._creditTotal != value)
                {
                    this._creditTotal = value;
                }
            }
        }

        [Description("Balance total")]
        public Decimal BalanceTotal
        {
            get
            {
                return this._balanceTotal;
            }
            set
            {
                if (this._balanceTotal != value)
                {
                    this._balanceTotal = value;
                }
            }
        }

        [Description("Branch")]
        public Branch Branch
        {
            get
            {
                return this._branch;
            }
            set
            {
                if (this._branch != value)
                {
                    this._branch = value;
                }
            }
        }

        [Description("AuditID")]
        public Int32 AuditID
        {
            get
            {
                return this._auditID;
            }
            set
            {
                if (this._auditID != value)
                {
                    this._auditID = value;
                }
            }
        }
        #endregion

        #region Constructors
        private Ledger()
        {
        }

        public static Ledger CreateLedger()
        {
            Ledger ledger = new Ledger();
            return ledger; 
        }
        #endregion

        #region Methods
        #endregion

        #region Overrides
        public override String ToString()
        {
            return this._sourceID.ToString();
        }
        #endregion
    }
}
