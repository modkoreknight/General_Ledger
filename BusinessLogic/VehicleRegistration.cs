using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class VehicleRegistration : Vehicle
    {
        #region Fields
        private String _referenceNo;
        private String _confirmLTO;
        private String _confirmationFieldOffice;
        private Decimal _confirmationAmount;
        private String _sop;
        private Decimal _sopAmount;
        private String _registered;
        private String _insured;
        private String _orNo;
        private String _crNo;
        private Decimal _registrationAmount;
        private String _fileNo;
        private String _siNo;
        private String _districtOffice;
        private String _clearance;
        private String _clearanceNo;
        private DateTime _dateRegistration;
        private String _registrationFieldOffice;
        private String _insuranceNo;
        private Decimal _insuranceAmount;
        private DateTime _dateRegistered;
        private String _fieldOffice;
        private String _mvFileNo;
        private Branch _branch;
        private Int32 _auditID;
        #endregion

        #region Properties
        [Description("ReferenceNo")]
        public String ReferenceNo
        {
            get
            {
                return this._referenceNo;
            }
            set
            {
                if (this._referenceNo != value)
                {
                    this._referenceNo = value;
                }
            }
        }

        [Description("ConfirmLTO")]
        public String ConfirmLTO
        {
            get
            {
                return this._confirmLTO;
            }
            set
            {
                if (this._confirmLTO != value)
                {
                    this._confirmLTO = value;
                }
            }
        }

        [Description("ConfirmationFieldOffice")]
        public String ConfirmationFieldOffice
        {
            get
            {
                return this._confirmationFieldOffice;
            }
            set
            {
                if (this._confirmationFieldOffice != value)
                {
                    this._confirmationFieldOffice = value;
                }
            }
        }

        [Description("ConfirmationAmount")]
        public Decimal ConfirmationAmount
        {
            get
            {
                return this._confirmationAmount;
            }
            set
            {
                if (this._confirmationAmount != value)
                {
                    this._confirmationAmount = value;
                }
            }
        }

        [Description("SOP")]
        public String SOP
        {
            get
            {
                return this._sop;
            }
            set
            {
                if (this._sop != value)
                {
                    this._sop = value;
                }
            }
        }

        [Description("SOPAmount")]
        public Decimal SOPAmount
        {
            get
            {
                return this._sopAmount;
            }
            set
            {
                if (this._sopAmount != value)
                {
                    this._sopAmount = value;
                }
            }
        }

        [Description("Registered")]
        public String Registered
        {
            get
            {
                return this._registered;
            }
            set
            {
                if (this._registered != value)
                {
                    this._registered = value;
                }
            }
        }

        [Description("Insured")]
        public String Insured
        {
            get
            {
                return this._insured;
            }
            set
            {
                if (this._insured != value)
                {
                    this._insured = value;
                }
            }
        }

        [Description("ORNo")]
        public String ORNo
        {
            get
            {
                return this._orNo;
            }
            set
            {
                if (this._orNo != value)
                {
                    this._orNo = value;
                }
            }
        }

        [Description("CRNo")]
        public String CRNo
        {
            get
            {
                return this._crNo;
            }
            set
            {
                if (this._crNo != value)
                {
                    this._crNo = value;
                }
            }
        }

        [Description("RegistrationAmount")]
        public Decimal RegistrationAmount
        {
            get
            {
                return this._registrationAmount;
            }
            set
            {
                if (this._registrationAmount != value)
                {
                    this._registrationAmount = value;
                }
            }
        }

        [Description("FileNo")]
        public String FileNo
        {
            get
            {
                return this._fileNo;
            }
            set
            {
                if (this._fileNo != value)
                {
                    this._fileNo = value;
                }
            }
        }

        [Description("SINo")]
        public String SINo
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

        [Description("DistrictOffice")]
        public String DistrictOffice
        {
            get
            {
                return this._districtOffice;
            }
            set
            {
                if (this._districtOffice != value)
                {
                    this._districtOffice = value;
                }
            }
        }

        [Description("Clearance")]
        public String Clearance
        {
            get
            {
                return this._clearance;
            }
            set
            {
                if (this._clearance != value)
                {
                    this._clearance = value;
                }
            }
        }

        [Description("ClearanceNo")]
        public String ClearanceNo
        {
            get
            {
                return this._clearanceNo;
            }
            set
            {
                if (this._clearanceNo != value)
                {
                    this._clearanceNo = value;
                }
            }
        }

        [Description("DateRegistration")]
        public DateTime DateRegistration
        {
            get
            {
                return this._dateRegistration;
            }
            set
            {
                if (this._dateRegistration != value)
                {
                    this._dateRegistration = value;
                }
            }
        }

        [Description("RegistrationFieldOffice")]
        public String RegistrationFieldOffice
        {
            get
            {
                return this._registrationFieldOffice;
            }
            set
            {
                if (this._registrationFieldOffice != value)
                {
                    this._registrationFieldOffice = value;
                }
            }
        }

        [Description("InsuranceNo")]
        public String InsuranceNo
        {
            get
            {
                return this._insuranceNo;
            }
            set
            {
                if (this._insuranceNo != value)
                {
                    this._insuranceNo = value;
                }
            }
        }

        [Description("InsuranceAmount")]
        public Decimal InsuranceAmount
        {
            get
            {
                return this._insuranceAmount;
            }
            set
            {
                if (this._insuranceAmount != value)
                {
                    this._insuranceAmount = value;
                }
            }
        }

        [Description("DateRegistered")]
        public DateTime DateRegistered
        {
            get
            {
                return this._dateRegistered;
            }
            set
            {
                if (this._dateRegistered != value)
                {
                    this._dateRegistered = value;
                }
            }
        }

        [Description("FieldOffice")]
        public String FieldOffice
        {
            get
            {
                return this._fieldOffice;
            }
            set
            {
                if (this._fieldOffice != value)
                {
                    this._fieldOffice = value;
                }
            }
        }

        [Description("MVFileNo")]
        public String MVFileNo
        {
            get
            {
                return this._mvFileNo;
            }
            set
            {
                if (this._mvFileNo != value)
                {
                    this._mvFileNo = value;
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
        private VehicleRegistration()
        {
        }

        public static VehicleRegistration CreateVehicleRegistration()
        {
            VehicleRegistration vehicleRegistration = new VehicleRegistration();
            //vehicleRegistration.Status = VehicleStatus.Brand_new;
            return vehicleRegistration; 
        }
        #endregion

        #region Methods
        #endregion

        #region Overrides
        public override String ToString()
        {
            return this.ID.ToString();
        }
        #endregion
    }
}
