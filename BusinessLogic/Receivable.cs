using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class Receivable
    {
        #region Fields
        private Sales _sales = Sales.CreateSales();
        private Int32 _instNo = 0;
        private String _monthApplied = String.Empty;
        private Decimal _due = 0.0M;
        private Decimal _overdue = 0.0M;
        private Decimal _overdue30 = 0.0M;
        private Decimal _overdue60 = 0.0M;
        private Decimal _overdue90 = 0.0M;
        private Decimal _totalPayment = 0.0M;
        private Decimal _remainingBalance = 0.0M;
        private DateTime _lastPaymentDate;
        private String _lastMonthApplied;
        private DateTime _dueDate;
        private String _customerName;
        private Branch _branch;
        private Int32 _auditID;
        #endregion

        #region Properties
        [Description("Sales")]
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

        /// <summary>
        /// Same as Overdue30
        /// </summary>
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

        /// <summary>
        /// Sum of Overdue60 and Overdue90
        /// </summary>
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

        /// <summary>
        /// Same as Due
        /// </summary>
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

        [Description("Total payment")]
        public Decimal TotalPayment
        {
            get
            {
                return this._totalPayment;
            }
            set
            {
                if (this._totalPayment != value)
                {
                    this._totalPayment = value;
                }
            }
        }

        [Description("Remaining balance")]
        public Decimal RemainingBalance
        {
            get
            {
                return this._remainingBalance;
            }
            set
            {
                if (this._remainingBalance != value)
                {
                    this._remainingBalance = value;
                }
            }
        }

        [Description("Customer name")]
        public String CustomerName
        {
            get
            {
                return this._customerName;
            }
            set
            {
                if (this._customerName != value)
                {
                    this._customerName = value;
                }
            }
        }

        [Description("Customer address")]
        public String CustomerAddress
        {
            get
            {
                return this._sales.Customer.Address;
            }
        }

        [Description("Amort amount")]
        public Decimal AmortAmount
        {
            get
            {
                return this._sales.AmortAmount;
            }
        }

        [Description("Amort rebate")]
        public Decimal AmortRebate
        {
            get
            {
                return this._sales.AmortRebate;
            }
        }

        [Description("Last payment date")]
        public DateTime LastPaymentDate
        {
            get
            {
                return this._lastPaymentDate;
            }
            set
            {
                if (this._lastPaymentDate != value)
                {
                    this._lastPaymentDate = value;
                }
            }
        }

        [Description("Last month applied")]
        public String LastMonthApplied
        {
            get
            {
                return this._lastMonthApplied;
            }
            set
            {
                if (this._lastMonthApplied != value)
                {
                    this._lastMonthApplied = value;
                }
            }
        }

        [Description("DueDate")]
        public DateTime DueDate
        {
            get
            {
                return this._dueDate;
            }
            set
            {
                if (this._dueDate != value)
                {
                    this._dueDate = value;
                }
            }
        }

        [Description("VehicleCode")]
        public String VehicleCode
        {
            get
            {
                return this._sales.Vehicle.Code;
            }
        }

        [Description("CustomerNo")]
        public String CustomerNo
        {
            get
            {
                return this._sales.Customer.CustomerNo;
            }
        }

        [Description("CustomerPhone")]
        public String CustomerPhone
        {
            get
            {
                return this._sales.Customer.Phone;
            }
        }

        [Description("CustomerZone")]
        public String CustomerZone
        {
            get
            {
                String customerZone = String.Empty;
                if (this._sales.Customer.Zone != null)
                {
                    customerZone = this._sales.Customer.Zone.Name;
                }
                return customerZone;
            }
        }

        [Description("CustomerRemarks")]
        public String CustomerRemarks
        {
            get
            {
                return this._sales.Customer.Remarks;
            }
        }

        [Description("VehicleBrand")]
        public String VehicleBrand
        {
            get
            {
                return this._sales.Vehicle.Brand;
            }
        }

        [Description("VehicleModel")]
        public String VehicleModel
        {
            get
            {
                return this._sales.Vehicle.Model;
            }
        }

        [Description("VehicleColor")]
        public String VehicleColor
        {
            get
            {
                return this._sales.Vehicle.Color;
            }
        }

        [Description("VehicleEngineNo")]
        public String VehicleEngineNo
        {
            get
            {
                return this._sales.Vehicle.EngineNo;
            }
        }

        [Description("VehicleChassisNo")]
        public String VehicleChassisNo
        {
            get
            {
                return this._sales.Vehicle.ChassisNo;
            }
        }

        [Description("VehiclePlateNo")]
        public String VehiclePlateNo
        {
            get
            {
                return this._sales.Vehicle.PlateNo;
            }
        }

        [Description("VehicleCertReg")]
        public String VehicleCertReg
        {
            get
            {
                return this._sales.Vehicle.CertReg;
            }
        }

        [Description("VehicleRemarks")]
        public String VehicleRemarks
        {
            get
            {
                return this._sales.Vehicle.Remarks;
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
        private Receivable()
        {
        }

        public static Receivable CreateReceivable()
        {
            Receivable receivable = new Receivable();
            return receivable; 
        }
        #endregion

        #region Methods
        #endregion

        #region Overrides
        public override String ToString()
        {
            return this._instNo.ToString();
        }
        #endregion
    }
}
