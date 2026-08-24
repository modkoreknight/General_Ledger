using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public interface ISalesProvider
    {
        Int32 GetSalesPageCount();
        Sales GetSales(Int32 id);
        GenericList<Sales> GetAllSales();
        GenericList<Sales> GetAllSales(Customer customer);
        GenericList<Sales> GetAllSales(SalesStatus status);
        GenericList<Sales> GetAllSales(Vehicle vehicle);
        GenericList<Sales> GetAllSales(Int32 pageNo, SortBySales sortBy, SortingOrder sortOrder);
        Sales InsertSales(Sales sales);
        Boolean UpdateSales(Sales sales);
        Boolean DeleteSales(Sales sales);
    }
}
