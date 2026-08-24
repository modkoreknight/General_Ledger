using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public interface ICustomerProvider
    {
        Int32 GetCustomerPageCount();
        Customer GetCustomer(Int32 id);
        GenericList<Customer> GetAllCustomer();
        GenericList<Customer> GetAllCustomer(Int32 pageNo, SortByCustomer sortBy, SortingOrder sortOrder);
        Customer InsertCustomer(Customer customer);
        Boolean UpdateCustomer(Customer customer);
        Boolean DeleteCustomer(Customer customer);
    }
}
