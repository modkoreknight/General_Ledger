using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class Sales
    {
        #region Fields
        private Int32 _id;
        private String _saleCode;
        private DateTime _saleDate;
        private Decimal _saleAmount;
        private Customer _customer;
        private Vehicle _vehicle;
        private PaymentTerm _term;
        private Int32 _termTotal;
        private DateTime _amortStartDate;
        private Decimal _amortAmount;
        private Decimal _amortRebate;
        private SalesStatus _status;
        private Decimal _cashPrice;
        private Decimal _lcPrice;
        private String _siNo;
        private DateTime _siDate;
        private Decimal _interestRate;
        private String _remarks;
        private Decimal _balanceFwd;
        private DateTime _dueDate01;
        private DateTime _dueDate02;
        private DateTime _repoDate;
        private String _repoExplain;
        private Decimal _repoBalance;
        private String _vehicleCurrentOwnerName;
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

        [Description("SaleCode")]
        public String SaleCode
        {
            get
            {
                return this._saleCode;
            }
            set
            {
                if (this._saleCode != value)
                {
                    this._saleCode = value;
                }
            }
        }

        [Description("SaleDate")]
        public DateTime SaleDate
        {
            get
            {
                return this._saleDate;
            }
            set
            {
                if (this._saleDate != value)
                {
                    this._saleDate = value;
                }
            }
        }

        [Description("SaleAmount")]
        public Decimal SaleAmount
        {
            get
            {
                return this._saleAmount;
            }
            set
            {
                if (this._saleAmount != value)
                {
                    this._saleAmount = value;
                }
            }
        }

        [Description("Customer")]
        public Customer Customer
        {
            get
            {
                return this._customer;
            }
            set
            {
                if (this._customer != value)
                {
                    this._customer = value;
                }
            }
        }

        [Description("Vehicle")]
        public Vehicle Vehicle
        {
            get
            {
                return this._vehicle;
            }
            set
            {
                if (this._vehicle != value)
                {
                    this._vehicle = value;
                }
            }
        }

        [Description("PaymentTerm")]
        public PaymentTerm Term
        {
            get
            {
                return this._term;
            }
            set
            {
                if (this._term != value)
                {
                    this._term = value;
                }
            }
        }

        [Description("TermTotal")]
        public Int32 TermTotal
        {
            get
            {
                return this._termTotal;
            }
            set
            {
                if (this._termTotal != value)
                {
                    this._termTotal = value;
                }
            }
        }

        [Description("AmortStartDate")]
        public DateTime AmortStartDate
        {
            get
            {
                return this._amortStartDate;
            }
            set
            {
                if (this._amortStartDate != value)
                {
                    this._amortStartDate = value;
                }
            }
        }

        [Description("AmortAmount")]
        public Decimal AmortAmount
        {
            get
            {
                return this._amortAmount;
            }
            set
            {
                if (this._amortAmount != value)
                {
                    this._amortAmount = value;
                }
            }
        }

        [Description("AmortRebate")]
        public Decimal AmortRebate
        {
            get
            {
                return this._amortRebate;
            }
            set
            {
                if (this._amortRebate != value)
                {
                    this._amortRebate = value;
                }
            }
        }

        [Description("Status")]
        public SalesStatus Status
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

        [Description("CashPrice")]
        public Decimal CashPrice
        {
            get
            {
                return this._cashPrice;
            }
            set
            {
                if (this._cashPrice != value)
                {
                    this._cashPrice = value;
                }
            }
        }

        [Description("LCP")]
        public Decimal LCP
        {
            get
            {
                return this._lcPrice;
            }
            set
            {
                if (this._lcPrice != value)
                {
                    this._lcPrice = value;
                }
            }
        }

        [Description("InvoiceNo")]
        public String InvoiceNo
        {
            get
            {
                return this._siNo;
            }
            set
            {
                if (this._siNo != value)
                {
                    this._siNo = value;
                }
            }
        }

        [Description("InvoiceDate")]
        public DateTime InvoiceDate
        {
            get
            {
                return this._siDate;
            }
            set
            {
                if (this._siDate != value)
                {
                    this._siDate = value;
                }
            }
        }

        [Description("InterestRate")]
        public Decimal InterestRate
        {
            get
            {
                return this._interestRate;
            }
            set
            {
                if (this._interestRate != value)
                {
                    this._interestRate = value;
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

        [Description("SaleTitle")]
        public String SaleTitle
        {
            get
            {
                String str = String.Empty;
                if (this._vehicle != null)
                {
                    str = this._vehicle.Name + " " + this._saleDate.ToString("MM/dd/yyyy");
                }
                else
                {
                    str = "[NoVehicle] " + this._saleDate.ToString("MM/dd/yyyy");
                }
                return str;
            }
        }

        [Description("CustomerName")]
        public String CustomerName
        {
            get
            {
                String name = String.Empty;
                if (this._customer != null)
                {
                    name = this._customer.FullNameLNF;
                }
                return name;
            }
        }

        [Description("VehicleName")]
        public String VehicleName
        {
            get
            {
                String name = String.Empty;
                if (this._vehicle != null)
                {
                    name = this._vehicle.Name;
                }
                return name;
            }
        }

        [Description("BalanceForwarded")]
        public Decimal BalanceFwd
        {
            get
            {
                return this._balanceFwd;
            }
            set
            {
                if (this._balanceFwd != value)
                {
                    this._balanceFwd = value;
                }
            }
        }

        [Description("DueDate01")]
        public DateTime DueDate01
        {
            get
            {
                return this._dueDate01;
            }
            set
            {
                if (this._dueDate01 != value)
                {
                    this._dueDate01 = value;
                }
            }
        }

        [Description("DueDate02")]
        public DateTime DueDate02
        {
            get
            {
                return this._dueDate02;
            }
            set
            {
                if (this._dueDate02 != value)
                {
                    this._dueDate02 = value;
                }
            }
        }

        [Description("RepoDate")]
        public DateTime RepoDate
        {
            get
            {
                return this._repoDate;
            }
            set
            {
                if (this._repoDate != value)
                {
                    this._repoDate = value;
                }
            }
        }

        [Description("RepoExplain")]
        public String RepoExplain
        {
            get
            {
                return this._repoExplain;
            }
            set
            {
                if (this._repoExplain != value)
                {
                    this._repoExplain = value;
                }
            }
        }

        [Description("RepoBalance")]
        public Decimal RepoBalance
        {
            get
            {
                return this._repoBalance;
            }
            set
            {
                if (this._repoBalance != value)
                {
                    this._repoBalance = value;
                }
            }
        }

        [Description("VehicleCode")]
        public String VehicleCode
        {
            get
            {
                String name = String.Empty;
                if (this._vehicle != null)
                {
                    name = this._vehicle.Code;
                }
                return name;
            }
        }

        [Description("VehicleModel")]
        public String VehicleModel
        {
            get
            {
                String name = String.Empty;
                if (this._vehicle != null)
                {
                    name = this._vehicle.Model;
                }
                return name;
            }
        }

        [Description("VehicleCondition")]
        public String VehicleCondition
        {
            get
            {
                String name = String.Empty;
                if (this._vehicle != null)
                {
                    name = this._vehicle.Status.ToString();
                }
                return Utility.EnumDecode(name);
            }
        }

        [Description("VehicleRegOwner")]
        public String VehicleRegOwner
        {
            get
            {
                String name = String.Empty;
                if (this._vehicle != null)
                {
                    if (this._vehicle.OwnerReg != null)
                    {
                        name = this._vehicle.OwnerReg.FullNameLNF;
                    }
                }
                return name;
            }
        }

        [Description("VehicleCuurentOwnerName")]
        public String VehicleCurrentOwnerName
        {
            get
            {
                return this._vehicleCurrentOwnerName;
            }
            set
            {
                if (this._vehicleCurrentOwnerName != value)
                {
                    this._vehicleCurrentOwnerName = value;
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
        private Sales()
        {
            if (!AppConfigHelper.IsAuthorized)
            {
                if (!AppConfigHelper.GetApplicationKey())
                {
                    throw new Exception("Unable to validate the application.");
                }
            }
        }

        public static Sales CreateSales()
        {
            Sales sales = new Sales();
            sales.SaleDate = DateTime.Today;
            sales.AmortStartDate = DateTime.Today.AddMonths(1);
            sales.InvoiceDate = DateTime.Today;
            sales.DueDate01 = DateTime.Today.AddMonths(1).AddDays(1);
            sales.DueDate02 = DateTime.Today.AddMonths(1).AddDays(2);
            return sales; 
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
