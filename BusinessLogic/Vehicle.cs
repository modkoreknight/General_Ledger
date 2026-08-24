using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class Vehicle
    {
        #region Fields
        private Int32 _id;
        private String _code;
        private String _brand;
        private String _model;
        private String _color;
        private String _engineNo;
        private String _chassisNo;
        private VehicleStatus _status;
        private String _plateNo;
        private String _certReg;
        private Customer _ownerReg;
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

        [Description("Code")]
        public String Code
        {
            get
            {
                return this._code;
            }
            set
            {
                if (this._code != value)
                {
                    this._code = value;
                }
            }
        }

        [Description("Brand")]
        public String Brand
        {
            get
            {
                return this._brand;
            }
            set
            {
                if (this._brand != value)
                {
                    this._brand = value;
                }
            }
        }

        [Description("Model")]
        public String Model
        {
            get
            {
                return this._model;
            }
            set
            {
                if (this._model != value)
                {
                    this._model = value;
                }
            }
        }

        [Description("Color")]
        public String Color
        {
            get
            {
                return this._color;
            }
            set
            {
                if (this._color != value)
                {
                    this._color = value;
                }
            }
        }

        [Description("Engine number")]
        public String EngineNo
        {
            get
            {
                return this._engineNo;
            }
            set
            {
                if (this._engineNo != value)
                {
                    this._engineNo = value;
                }
            }
        }

        [Description("Name")]
        public String Name
        {
            get
            {
                String name = String.Empty;
                if (!String.IsNullOrEmpty(this._code))
                {
                    name = this._code + " - ";
                }
                name = name + this._brand + " " + this._model;
                if (!String.IsNullOrEmpty(this._color))
                {
                    name = name + " - " + this._color;
                }
                switch (this._status)
                {
                    case VehicleStatus.Brand_new:
                        name = name + " [Available]";
                        break;
                    case VehicleStatus.Repossessed:
                        name = name + " [Repossessed]";
                        break;
                    case VehicleStatus.Sold:
                        name = name + " [Sold]";
                        break;
                }
                return name;
            }
        }

        [Description("Chassis number")]
        public String ChassisNo
        {
            get
            {
                return this._chassisNo;
            }
            set
            {
                if (this._chassisNo != value)
                {
                    this._chassisNo = value;
                }
            }
        }

        [Description("Vehicle status")]
        public VehicleStatus Status
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

        [Description("Plate number")]
        public String PlateNo
        {
            get
            {
                return this._plateNo;
            }
            set
            {
                if (this._plateNo != value)
                {
                    this._plateNo = value;
                }
            }
        }

        [Description("Cert. of reg.")]
        public String CertReg
        {
            get
            {
                return this._certReg;
            }
            set
            {
                if (this._certReg != value)
                {
                    this._certReg = value;
                }
            }
        }

        [Description("Reg. owner")]
        public Customer OwnerReg
        {
            get
            {
                return this._ownerReg;
            }
            set
            {
                if (this._ownerReg != value)
                {
                    this._ownerReg = value;
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
        protected Vehicle()
        {
            if (!AppConfigHelper.IsAuthorized)
            {
                if (!AppConfigHelper.GetApplicationKey())
                {
                    throw new Exception("Unable to validate the application.");
                }
            }
        }

        public static Vehicle CreateVehicle()
        {
            Vehicle vehicle = new Vehicle();
            vehicle.Status = VehicleStatus.Brand_new;
            return vehicle; 
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
