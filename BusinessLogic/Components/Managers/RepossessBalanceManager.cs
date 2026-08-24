using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class RepossessBalanceManager
    {
        #region Fields
        private IRepossessBalanceProvider _provider;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public RepossessBalanceManager(IRepossessBalanceProvider provider)
        {
            this._provider = provider;
        }
        #endregion

        #region Methods
        public GenericList<Receivable> GetAllReceivable(DateTime cutoff)
        {
            return this._provider.GetAllReceivable(cutoff);
        }

        public Receivable GetReceivable(DateTime cutoff, Sales sales)
        {
            return this._provider.GetReceivable(cutoff, sales);
        }
        #endregion
    }
}
