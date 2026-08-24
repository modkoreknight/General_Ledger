using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class ZoneManager
    {
        #region Fields
        private IZoneProvider _provider;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public ZoneManager(IZoneProvider provider)
        {
            this._provider = provider;
        }
        #endregion

        #region Methods
        public Int32 GetZonePageCount()
        {
            return this._provider.GetZonePageCount();
        }

        public Zone GetZone(Int32 id)
        {
            return this._provider.GetZone(id);
        }
        
        public GenericList<Zone> GetAllZone()
        {
            return this._provider.GetAllZone();
        }

        public GenericList<Zone> GetAllZone(Int32 pageNo, SortByZone sortBy, SortingOrder sortOrder)
        {
            return this._provider.GetAllZone(pageNo, sortBy, sortOrder);
        }

        public Zone InsertZone(Zone zone)
        {
            return this._provider.InsertZone(zone);
        }

        public Boolean UpdateZone(Zone zone)
        {
            return this._provider.UpdateZone(zone);
        }

        public Boolean DeleteZone(Zone zone)
        {
            return this._provider.DeleteZone(zone);
        }
        #endregion
    }
}
