using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class ReceivableManager
    {
        #region Fields
        private IReceivableProvider _provider;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public ReceivableManager(IReceivableProvider provider)
        {
            this._provider = provider;
        }
        #endregion

        #region Methods
        public Receivable GetReceivable()
        {
            return this._provider.GetReceivable();
        }

        public Receivable GetReceivable(Int32 monthsToPay)
        {
            return this._provider.GetReceivable(monthsToPay);
        }

        public Receivable GetReceivable(DateTime cutoff)
        {
            return this._provider.GetReceivable(cutoff);
        }
        #endregion
    }
}
