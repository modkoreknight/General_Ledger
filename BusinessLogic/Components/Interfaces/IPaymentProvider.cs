using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public interface IPaymentProvider
    {
        Int32 GetPaymentPageCount();
        Payment GetPayment(Int32 id);
        GenericList<Payment> GetAllPayment();
        GenericList<Payment> GetAllPayment(Sales sales);
        GenericList<Payment> GetAllPayment(Int32 rangeFrom, Int32 rangeTo);
        GenericList<Payment> GetAllPayment(String monthApplied);
        GenericList<Payment> GetAllPayment(DateTime from, DateTime to);
        GenericList<Payment> GetAllPayment(Int32 pageNo, SortByPayment sortBy, SortingOrder sortOrder);
        Payment InsertPayment(Payment payment);
        Boolean UpdatePayment(Payment payment);
        Boolean DeletePayment(Payment payment);
        Payment FindPayment(String searchString, String searchCriteria);
    }
}
