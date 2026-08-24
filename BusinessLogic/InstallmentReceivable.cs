using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class InstallmentReceivable
    {
        #region Fields
        private Zone _zone;
        private DateTime _cutoff;
        private Decimal _balanceTotal;
        private Int32 _customerTotal;
        private Branch _branch;
        private Int32 _auditID;
        #endregion

        #region Properties
        [Description("Zone")]
        public Zone Zone
        {
            get
            {
                return this._zone;
            }
            set
            {
                if (this._zone != value)
                {
                    this._zone = value;
                }
            }
        }

        [Description("ZoneName")]
        public String ZoneName
        {
            get
            {
                return this._zone.Name;
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

        [Description("BalanceTotal")]
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

        [Description("CustomerTotal")]
        public Int32 CustomerTotal
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
        private InstallmentReceivable()
        {
        }

        public static InstallmentReceivable CreateInstallmentReceivable()
        {
            InstallmentReceivable installmentReceivable = new InstallmentReceivable();
            return installmentReceivable; 
        }
        #endregion

        #region Methods
        #endregion

        #region Overrides
        public override String ToString()
        {
            return this._zone.ID.ToString();
        }
        #endregion
    }
}
