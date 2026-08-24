using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public interface IRepossessBalanceProvider
    {
        GenericList<Receivable> GetAllReceivable(DateTime cutoff);
        Receivable GetReceivable(DateTime cutoff, Sales sales);
    }
}
