using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class VehicleRegistrationManager
    {
        #region Fields
        private IVehicleRegistrationProvider _provider;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public VehicleRegistrationManager(IVehicleRegistrationProvider provider)
        {
            this._provider = provider;
        }
        #endregion

        #region Methods
        public Int32 GetVehicleRegistrationPageCount()
        {
            return this._provider.GetVehicleRegistrationPageCount();
        }

        public VehicleRegistration GetVehicleRegistration(Int32 id)
        {
            return this._provider.GetVehicleRegistration(id);
        }

        public GenericList<VehicleRegistration> GetAllVehicleRegistration()
        {
            return this._provider.GetAllVehicleRegistration();
        }

        public GenericList<VehicleRegistration> GetAllVehicleRegistration(Int32 pageNo, SortByVehicle sortBy, SortingOrder sortOrder)
        {
            return this._provider.GetAllVehicleRegistration(pageNo, sortBy, sortOrder);
        }

        public VehicleRegistration InsertVehicleRegistration(VehicleRegistration vehicleRegistration)
        {
            return this._provider.InsertVehicleRegistration(vehicleRegistration);
        }

        public Boolean UpdateVehicleRegistration(VehicleRegistration vehicleRegistration)
        {
            return this._provider.UpdateVehicleRegistration(vehicleRegistration);
        }

        public Boolean DeleteVehicleRegistration(VehicleRegistration vehicleRegistration)
        {
            return this._provider.DeleteVehicleRegistration(vehicleRegistration);
        }
        #endregion
    }
}
