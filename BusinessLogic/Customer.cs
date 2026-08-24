using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class Customer
    {
        #region Fields
        private Int32 _id;
        private String _customerNo;
        private String _lastName;
        private String _firstName;
        private String _middleName;
        private String _address;
        private Zone _zone;
        private String _phone;
        private DateTime _birthDate;
        private String _pictureFile;
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

        [Description("CustomerNo")]
        public String CustomerNo
        {
            get
            {
                return this._customerNo;
            }
            set
            {
                if (this._customerNo != value)
                {
                    this._customerNo = value;
                }
            }
        }

        [Description("LastName")]
        public String LastName
        {
            get
            {
                return this._lastName;
            }
            set
            {
                if (this._lastName != value)
                {
                    this._lastName = value;
                }
            }
        }

        [Description("FirstName")]
        public String FirstName
        {
            get
            {
                return this._firstName;
            }
            set
            {
                if (this._firstName != value)
                {
                    this._firstName = value;
                }
            }
        }

        [Description("MiddleName")]
        public String MiddleName
        {
            get
            {
                return this._middleName;
            }
            set
            {
                if (this._middleName != value)
                {
                    this._middleName = value;
                }
            }
        }

        [Description("FullNameLNF")]
        public String FullNameLNF
        {
            get
            {
                return this._lastName + ", " + this._firstName + " " + this._middleName;
            }
        }

        [Description("FullNameFNF")]
        public String FullNameFNF
        {
            get
            {
                return this._firstName + " " + this._lastName;
            }
        }

        [Description("Address")]
        public String Address
        {
            get
            {
                return this._address;
            }
            set
            {
                if (this._address != value)
                {
                    this._address = value;
                }
            }
        }

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

        [Description("Phone")]
        public String Phone
        {
            get
            {
                return this._phone;
            }
            set
            {
                if (this._phone != value)
                {
                    this._phone = value;
                }
            }
        }

        [Description("BirthDate")]
        public DateTime BirthDate
        {
            get
            {
                return this._birthDate;
            }
            set
            {
                if (this._birthDate != value)
                {
                    this._birthDate = value;
                }
            }
        }

        [Description("PictureFile")]
        public String PictureFile
        {
            get
            {
                return this._pictureFile;
            }
            set
            {
                if (this._pictureFile != value)
                {
                    this._pictureFile = value;
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
        private Customer()
        {
            if (!AppConfigHelper.IsAuthorized)
            {
                if (!AppConfigHelper.GetApplicationKey())
                {
                    throw new Exception("Unable to validate the application.");
                }
            }
        }

        public static Customer CreateCustomer()
        {
            Customer customer = new Customer();
            return customer; 
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
