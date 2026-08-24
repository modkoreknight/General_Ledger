using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class SettingsManager
    {
        #region Fields
        private ISettingsProvider _provider;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public SettingsManager(ISettingsProvider provider)
        {
            this._provider = provider;
        }
        #endregion

        #region Methods
        public Settings GetSettings(Settings settings)
        {
            return this._provider.GetSettings(settings);
        }
        #endregion
    }
}
