using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class CustomerManager
    {
        #region Fields
        private ICustomerProvider _provider;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public CustomerManager(ICustomerProvider provider)
        {
            this._provider = provider;
        }
        #endregion

        #region Methods
        public Int32 GetCustomerPageCount()
        {
            return this._provider.GetCustomerPageCount();
        }

        public Customer GetCustomer(Int32 id)
        {
            return this._provider.GetCustomer(id);
        }
        
        public GenericList<Customer> GetAllCustomer()
        {
            return this._provider.GetAllCustomer();
        }

        public GenericList<Customer> GetAllCustomer(Int32 pageNo, SortByCustomer sortBy, SortingOrder sortOrder)
        {
            return this._provider.GetAllCustomer(pageNo, sortBy, sortOrder);
        }

        public Customer InsertCustomer(Customer customer)
        {
            return this._provider.InsertCustomer(customer);
        }

        public Boolean UpdateCustomer(Customer customer)
        {
            return this._provider.UpdateCustomer(customer);
        }

        public Boolean DeleteCustomer(Customer customer)
        {
            return this._provider.DeleteCustomer(customer);
        }
        #endregion
    }
}
