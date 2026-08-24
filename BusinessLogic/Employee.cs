using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class Employee
    {
        #region Fields
        private Int32 _id;
        private String _employeeNo;
        private String _lastName;
        private String _firstName;
        private String _middleName;
        private String _address;
        private String _phone;
        private DateTime _birthDate;
        private String _userName;
        private String _password;
        private String _salt;
        private String _pictureFile;
        private String _remarks;
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

        [Description("EmployeeNo")]
        public String EmployeeNo
        {
            get
            {
                return this._employeeNo;
            }
            set
            {
                if (this._employeeNo != value)
                {
                    this._employeeNo = value;
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
                return this._lastName + ", " + this._firstName;
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

        [Description("UserName")]
        public String UserName
        {
            get
            {
                return this._userName;
            }
            set
            {
                if (this._userName != value)
                {
                    this._userName = value;
                }
            }
        }

        [Description("Password")]
        public String Password
        {
            get
            {
                return this._password;
            }
            set
            {
                if (this._password != value)
                {
                    this._password = value;
                }
            }
        }

        [Description("Salt")]
        public String Salt
        {
            get
            {
                return this._salt;
            }
            set
            {
                if (this._salt != value)
                {
                    this._salt = value;
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
        #endregion

        #region Constructors
        private Employee()
        {
        }

        public static Employee CreateEmployee()
        {
            Employee employee = new Employee();
            return employee; 
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
