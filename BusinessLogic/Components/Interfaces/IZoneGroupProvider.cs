using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public interface IZoneGroupProvider
    {
        Int32 GetZoneGroupPageCount();
        ZoneGroup GetZoneGroup(Int32 id);
        GenericList<ZoneGroup> GetAllZoneGroup();
        GenericList<ZoneGroup> GetAllZoneGroup(Int32 pageNo, SortByZoneGroup sortBy, SortingOrder sortOrder);
        ZoneGroup InsertZoneGroup(ZoneGroup zoneGroup);
        Boolean UpdateZoneGroup(ZoneGroup zoneGroup);
        Boolean DeleteZoneGroup(ZoneGroup zoneGroup);
    }
}
