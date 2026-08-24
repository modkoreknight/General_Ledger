using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    /// <summary>
    /// Represents an address' city or town group.
    /// </summary>
    public class Zone
    {
        #region Fields
        private Int32 _id;
        private String _name;
        private String _abbreviation;
        private String _description;
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

        [Description("Name")]
        public String Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (this._name != value)
                {
                    this._name = value;
                }
            }
        }

        [Description("Abbreviation")]
        public String Abbreviation
        {
            get
            {
                return this._abbreviation;
            }
            set
            {
                if (this._abbreviation != value)
                {
                    this._abbreviation = value;
                }
            }
        }

        [Description("Description")]
        public String Description
        {
            get
            {
                return this._description;
            }
            set
            {
                if (this._description != value)
                {
                    this._description = value;
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
        private Zone()
        {
        }

        public static Zone CreateZone()
        {
            Zone zone = new Zone();
            return zone;
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
