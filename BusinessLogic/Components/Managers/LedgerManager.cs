using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class LedgerManager
    {
        #region Fields
        private ILedgerProvider _provider;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public LedgerManager(ILedgerProvider provider)
        {
            this._provider = provider;
        }
        #endregion

        #region Methods
        public GenericList<Ledger> GetLedger(Sales sales)
        {
            return this._provider.GetLedger(sales);
        }
        #endregion
    }
}
