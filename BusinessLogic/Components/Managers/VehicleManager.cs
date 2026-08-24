using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class VehicleManager
    {
        #region Fields
        private IVehicleProvider _provider;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public VehicleManager(IVehicleProvider provider)
        {
            this._provider = provider;
        }
        #endregion

        #region Methods
        public Int32 GetVehiclePageCount()
        {
            return this._provider.GetVehiclePageCount();
        }

        public Vehicle GetVehicle(Int32 id)
        {
            return this._provider.GetVehicle(id);
        }
        
        public GenericList<Vehicle> GetAllVehicle()
        {
            return this._provider.GetAllVehicle();
        }

        public GenericList<Vehicle> GetAllVehicle(Int32 pageNo, SortByVehicle sortBy, SortingOrder sortOrder)
        {
            return this._provider.GetAllVehicle(pageNo, sortBy, sortOrder);
        }

        public Vehicle InsertVehicle(Vehicle vehicle)
        {
            return this._provider.InsertVehicle(vehicle);
        }

        public Boolean UpdateVehicle(Vehicle vehicle)
        {
            return this._provider.UpdateVehicle(vehicle);
        }

        public Boolean DeleteVehicle(Vehicle vehicle)
        {
            return this._provider.DeleteVehicle(vehicle);
        }
        #endregion
    }
}
