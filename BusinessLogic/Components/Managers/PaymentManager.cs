using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class PaymentManager
    {
        #region Fields
        private IPaymentProvider _provider;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public PaymentManager(IPaymentProvider provider)
        {
            this._provider = provider;
        }
        #endregion

        #region Methods
        public Int32 GetPaymentPageCount()
        {
            return this._provider.GetPaymentPageCount();
        }

        public Payment GetPayment(Int32 id)
        {
            return this._provider.GetPayment(id);
        }

        public GenericList<Payment> GetAllPayment()
        {
            return this._provider.GetAllPayment();
        }

        public GenericList<Payment> GetAllPayment(Sales sales)
        {
            return this._provider.GetAllPayment(sales);
        }

        public GenericList<Payment> GetAllPayment(Int32 rangeFrom, Int32 rangeTo)
        {
            return this._provider.GetAllPayment(rangeFrom, rangeTo);
        }

        public GenericList<Payment> GetAllPayment(String monthApplied)
        {
            return this._provider.GetAllPayment(monthApplied);
        }

        public GenericList<Payment> GetAllPayment(DateTime from, DateTime to)
        {
            return this._provider.GetAllPayment(from, to);
        }

        public GenericList<Payment> GetAllPayment(Int32 pageNo, SortByPayment sortBy, SortingOrder sortOrder)
        {
            return this._provider.GetAllPayment(pageNo, sortBy, sortOrder);
        }

        public Payment InsertPayment(Payment payment)
        {
            return this._provider.InsertPayment(payment);
        }

        public Boolean UpdatePayment(Payment payment)
        {
            return this._provider.UpdatePayment(payment);
        }

        public Boolean DeletePayment(Payment payment)
        {
            return this._provider.DeletePayment(payment);
        }

        public Payment FindPayment(String searchString, String searchCriteria)
        {
            return this._provider.FindPayment(searchString, searchCriteria);
        }
        #endregion
    }
}
