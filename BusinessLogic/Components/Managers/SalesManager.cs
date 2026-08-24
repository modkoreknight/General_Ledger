using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class SalesManager
    {
        #region Fields
        private ISalesProvider _provider;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public SalesManager(ISalesProvider provider)
        {
            this._provider = provider;
        }
        #endregion

        #region Methods
        public Int32 GetSalesPageCount()
        {
            return this._provider.GetSalesPageCount();
        }

        public Sales GetSales(Int32 id)
        {
            return this._provider.GetSales(id);
        }
        
        public GenericList<Sales> GetAllSales()
        {
            return this._provider.GetAllSales();
        }

        public GenericList<Sales> GetAllSales(Customer customer)
        {
            return this._provider.GetAllSales(customer);
        }

        public GenericList<Sales> GetAllSales(SalesStatus status)
        {
            return this._provider.GetAllSales(status);
        }

        public GenericList<Sales> GetAllSales(Vehicle vehicle)
        {
            return this._provider.GetAllSales(vehicle);
        }

        public GenericList<Sales> GetAllSales(Int32 pageNo, SortBySales sortBy, SortingOrder sortOrder)
        {
            return this._provider.GetAllSales(pageNo, sortBy, sortOrder);
        }

        public Sales InsertSales(Sales sales)
        {
            return this._provider.InsertSales(sales);
        }

        public Boolean UpdateSales(Sales sales)
        {
            return this._provider.UpdateSales(sales);
        }

        public Boolean DeleteSales(Sales sales)
        {
            return this._provider.DeleteSales(sales);
        }
        #endregion
    }
}
