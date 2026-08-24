using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class EndingReport
    {
        #region Fields
        private Branch _branch;
        private DateTime _startDate;
        private DateTime _cutoff;
        private Int32 _dayNo;
        private Decimal _dueOverdueTotal;
        private Decimal _customerTotal;
        private Decimal _receivableTotal;
        private Decimal _paymentTotal;
        private Decimal _brandNewTotal;
        private Decimal _secondHandTotal;
        private String _rate;
        private Decimal _repoTotal;
        private Decimal _repoBalanceTotal;
        private Decimal _companyServiceTotal;
        private Int32 _auditID;
        #endregion

        #region Properties
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

        [Description("StartDate")]
        public DateTime StartDate
        {
            get
            {
                return this._startDate;
            }
            set
            {
                if (this._startDate != value)
                {
                    this._startDate = value;
                }
            }
        }

        [Description("Cutoff")]
        public DateTime Cutoff
        {
            get
            {
                return this._cutoff;
            }
            set
            {
                if (this._cutoff != value)
                {
                    this._cutoff = value;
                }
            }
        }

        [Description("Day no.")]
        public Int32 DayNo
        {
            get
            {
                return this._dayNo;
            }
            set
            {
                if (this._dayNo != value)
                {
                    this._dayNo = value;
                }
            }
        }

        /// <summary>
        /// Due 30
        /// </summary>
        [Description("DueOverdueTotal")]
        public Decimal DueOverdueTotal
        {
            get
            {
                return this._dueOverdueTotal;
            }
            set
            {
                if (this._dueOverdueTotal != value)
                {
                    this._dueOverdueTotal = value;
                }
            }
        }

        /// <summary>
        /// Due 60 and 90 onwards
        /// </summary>
        [Description("CustomerTotal")]
        public Decimal CustomerTotal
        {
            get
            {
                return this._customerTotal;
            }
            set
            {
                if (this._customerTotal != value)
                {
                    this._customerTotal = value;
                }
            }
        }

        /// <summary>
        /// Due 30, 60 and 90 onwards
        /// </summary>
        [Description("ReceivableTotal")]
        public Decimal ReceivableTotal
        {
            get
            {
                return this._receivableTotal;
            }
            set
            {
                if (this._receivableTotal != value)
                {
                    this._receivableTotal = value;
                }
            }
        }

        [Description("PaymentTotal")]
        public Decimal PaymentTotal
        {
            get
            {
                return this._paymentTotal;
            }
            set
            {
                if (this._paymentTotal != value)
                {
                    this._paymentTotal = value;
                }
            }
        }

        [Description("BrandNewTotal")]
        public Decimal BrandNewTotal
        {
            get
            {
                return this._brandNewTotal;
            }
            set
            {
                if (this._brandNewTotal != value)
                {
                    this._brandNewTotal = value;
                }
            }
        }

        [Description("SecondHandTotal")]
        public Decimal SecondHandTotal
        {
            get
            {
                return this._secondHandTotal;
            }
            set
            {
                if (this._secondHandTotal != value)
                {
                    this._secondHandTotal = value;
                }
            }
        }

        [Description("Rate")]
        public String Rate
        {
            get
            {
                return this._rate;
            }
            set
            {
                if (this._rate != value)
                {
                    this._rate = value;
                }
            }
        }

        [Description("RepoTotal")]
        public Decimal RepoTotal
        {
            get
            {
                return this._repoTotal;
            }
            set
            {
                if (this._repoTotal != value)
                {
                    this._repoTotal = value;
                }
            }
        }

        [Description("RepoBalanceTotal")]
        public Decimal RepoBalanceTotal
        {
            get
            {
                return this._repoBalanceTotal;
            }
            set
            {
                if (this._repoBalanceTotal != value)
                {
                    this._repoBalanceTotal = value;
                }
            }
        }

        [Description("CompanyServiceTotal")]
        public Decimal CompanyServiceTotal
        {
            get
            {
                return this._companyServiceTotal;
            }
            set
            {
                if (this._companyServiceTotal != value)
                {
                    this._companyServiceTotal = value;
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
        private EndingReport()
        {
        }

        public static EndingReport CreateEndingReport()
        {
            EndingReport endingReport = new EndingReport();
            return endingReport; 
        }
        #endregion

        #region Methods
        #endregion

        #region Overrides
        public override String ToString()
        {
            return this._dayNo.ToString();
        }
        #endregion
    }
}
