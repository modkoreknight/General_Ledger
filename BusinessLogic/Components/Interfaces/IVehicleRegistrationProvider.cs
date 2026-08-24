using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public interface IVehicleRegistrationProvider
    {
        Int32 GetVehicleRegistrationPageCount();
        VehicleRegistration GetVehicleRegistration(Int32 id);
        GenericList<VehicleRegistration> GetAllVehicleRegistration();
        GenericList<VehicleRegistration> GetAllVehicleRegistration(Int32 pageNo, SortByVehicle sortBy, SortingOrder sortOrder);
        VehicleRegistration InsertVehicleRegistration(VehicleRegistration vehicleRegistration);
        Boolean UpdateVehicleRegistration(VehicleRegistration vehicleRegistration);
        Boolean DeleteVehicleRegistration(VehicleRegistration vehicleRegistration);
    }
}
