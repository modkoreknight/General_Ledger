using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public interface IZoneProvider
    {
        Int32 GetZonePageCount();
        Zone GetZone(Int32 id);
        GenericList<Zone> GetAllZone();
        GenericList<Zone> GetAllZone(Int32 pageNo, SortByZone sortBy, SortingOrder sortOrder);
        Zone InsertZone(Zone zone);
        Boolean UpdateZone(Zone zone);
        Boolean DeleteZone(Zone zone);
    }
}
