using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class PaymentFrequency
    {
        #region Fields
        private Branch _branch;
        private DateTime _cutoff;
        private Int32 _dayNo;
        private Decimal _dueTotal;
        private Decimal _overdueTotal;
        private Decimal _receivableTotal;
        private Decimal _paymentTotal;
        private Decimal _rebateTotal;
        private String _frequency;
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
        [Description("DueTotal")]
        public Decimal DueTotal
        {
            get
            {
                return this._dueTotal;
            }
            set
            {
                if (this._dueTotal != value)
                {
                    this._dueTotal = value;
                }
            }
        }

        /// <summary>
        /// Due 60 and 90 onwards
        /// </summary>
        [Description("OverdueTotal")]
        public Decimal OverdueTotal
        {
            get
            {
                return this._overdueTotal;
            }
            set
            {
                if (this._overdueTotal != value)
                {
                    this._overdueTotal = value;
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

        [Description("RebateTotal")]
        public Decimal RebateTotal
        {
            get
            {
                return this._rebateTotal;
            }
            set
            {
                if (this._rebateTotal != value)
                {
                    this._rebateTotal = value;
                }
            }
        }

        [Description("Frequency")]
        public String Frequency
        {
            get
            {
                return this._frequency;
            }
            set
            {
                if (this._frequency != value)
                {
                    this._frequency = value;
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
        private PaymentFrequency()
        {
        }

        public static PaymentFrequency CreatePaymentFrequency()
        {
            PaymentFrequency paymentFrequency = new PaymentFrequency();
            return paymentFrequency; 
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
