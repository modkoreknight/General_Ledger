using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class ZoneGroupManager
    {
        #region Fields
        private IZoneGroupProvider _provider;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public ZoneGroupManager(IZoneGroupProvider provider)
        {
            this._provider = provider;
        }
        #endregion

        #region Methods
        public Int32 GetZoneGroupPageCount()
        {
            return this._provider.GetZoneGroupPageCount();
        }

        public ZoneGroup GetZoneGroup(Int32 id)
        {
            return this._provider.GetZoneGroup(id);
        }

        public GenericList<ZoneGroup> GetAllZoneGroup()
        {
            return this._provider.GetAllZoneGroup();
        }

        public GenericList<ZoneGroup> GetAllZoneGroup(Int32 pageNo, SortByZoneGroup sortBy, SortingOrder sortOrder)
        {
            return this._provider.GetAllZoneGroup(pageNo, sortBy, sortOrder);
        }

        public ZoneGroup InsertZoneGroup(ZoneGroup zoneGroup)
        {
            return this._provider.InsertZoneGroup(zoneGroup);
        }

        public Boolean UpdateZoneGroup(ZoneGroup zoneGroup)
        {
            return this._provider.UpdateZoneGroup(zoneGroup);
        }

        public Boolean DeleteZoneGroup(ZoneGroup zoneGroup)
        {
            return this._provider.DeleteZoneGroup(zoneGroup);
        }
        #endregion
    }
}
