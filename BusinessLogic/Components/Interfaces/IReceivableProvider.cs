using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public interface IReceivableProvider
    {
        GenericList<Payment> AllPayment
        {
            get;
            set;
        }
        Sales Sales
        {
            get;
            set;
        }
        Receivable GetReceivable();
        Receivable GetReceivable(Int32 monthsToPay);
        Receivable GetReceivable(DateTime cutoff);
    }
}
