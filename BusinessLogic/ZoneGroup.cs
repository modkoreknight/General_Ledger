using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class ZoneGroup
    {
        #region Fields
        private Int32 _id;
        private String _name;
        private String _zones;
        private List<Zone> _allZones;
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

        [Description("Zones")]
        public String Zones
        {
            get
            {
                return this._zones;
            }
            set
            {
                if (this._zones != value)
                {
                    this._zones = value;
                }
            }
        }

        [Description("AllZones")]
        public List<Zone> AllZones
        {
            get
            {
                return this._allZones;
            }
            set
            {
                if (this._allZones != value)
                {
                    this._allZones = value;
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
        private ZoneGroup()
        {
        }

        public static ZoneGroup CreateZoneGroup()
        {
            ZoneGroup zoneGroup = new ZoneGroup();
            return zoneGroup;
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
