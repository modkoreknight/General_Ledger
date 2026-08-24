using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public interface IVehicleProvider
    {
        Int32 GetVehiclePageCount();
        Vehicle GetVehicle(Int32 id);
        GenericList<Vehicle> GetAllVehicle();
        GenericList<Vehicle> GetAllVehicle(Int32 pageNo, SortByVehicle sortBy, SortingOrder sortOrder);
        Vehicle InsertVehicle(Vehicle vehicle);
        Boolean UpdateVehicle(Vehicle vehicle);
        Boolean DeleteVehicle(Vehicle vehicle);
    }
}
