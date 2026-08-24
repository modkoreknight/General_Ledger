using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class Payment
    {
        #region Fields
        private Int32 _id;
        private Sales _paymentSales;
        private String _paymentNo;
        private DateTime _paymentDate;
        private Decimal _paymentAmount;
        private Decimal _rebate;
        private PaymentMode _mode;
        private String _checkNo;
        private PaymentStatus _status;
        private Int32 _instNo;
        private String _monthApplied;
        private Decimal _due;
        private Decimal _overdue;
        private Decimal _debit;
        private Decimal _credit;
        private String _remarks;
        private Branch _branch;
        private Int32 _auditID;
        #endregion

        #region Properties
        [Description("ID")]
        public Int32 ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if (this._id != value)
                {
                    this._id = value;
                }
            }
        }

        [Description("PaymentSales")]
        public Sales PaymentSales
        {
            get
            {
                return this._paymentSales;
            }
            set
            {
                if (this._paymentSales != value)
                {
                    this._paymentSales = value;
                }
            }
        }

        [Description("PaymentNo")]
        public String PaymentNo
        {
            get
            {
                return this._paymentNo;
            }
            set
            {
                if (this._paymentNo != value)
                {
                    this._paymentNo = value;
                }
            }
        }

        [Description("PaymentDate")]
        public DateTime PaymentDate
        {
            get
            {
                return this._paymentDate;
            }
            set
            {
                if (this._paymentDate != value)
                {
                    this._paymentDate = value;
                }
            }
        }

        [Description("PaymentAmount")]
        public Decimal PaymentAmount
        {
            get
            {
                return this._paymentAmount;
            }
            set
            {
                if (this._paymentAmount != value)
                {
                    this._paymentAmount = value;
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

        [Description("Mode")]
        public PaymentMode Mode
        {
            get
            {
                return this._mode;
            }
            set
            {
                if (this._mode != value)
                {
                    this._mode = value;
                }
            }
        }

        [Description("CheckNo")]
        public String CheckNo
        {
            get
            {
                return this._checkNo;
            }
            set
            {
                if (this._checkNo != value)
                {
                    this._checkNo = value;
                }
            }
        }

        [Description("Status")]
        public PaymentStatus Status
        {
            get
            {
                return this._status;
            }
            set
            {
                if (this._status != value)
                {
                    this._status = value;
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

        [Description("CustomerName")]
        public String CustomerName
        {
            get
            {
                return this._paymentSales.Customer.FullNameLNF;
            }
        }

        [Description("VehicleCode")]
        public String VehicleCode
        {
            get
            {
                return this._paymentSales.Vehicle.Code;
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
        private Payment()
        {
            if (!AppConfigHelper.IsAuthorized)
            {
                if (!AppConfigHelper.GetApplicationKey())
                {
                    throw new Exception("Unable to validate the application.");
                }
            }
        }

        public static Payment CreatePayment()
        {
            Payment payment = new Payment();
            payment.PaymentDate = DateTime.Today;
            return payment; 
        }
        #endregion

        #region Methods
        #endregion

        #region Overrides
        public override String ToString()
        {
            return this._id.ToString();
        }
        #endregion
    }
}
