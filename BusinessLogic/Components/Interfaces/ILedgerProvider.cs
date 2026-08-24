using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public interface ILedgerProvider
    {
        GenericList<Ledger> GetLedger(Sales sales);
    }
}
