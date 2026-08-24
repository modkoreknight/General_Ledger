using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Interact.BusinessLogic
{
    public static class AppConfigHelper
    {
        #region Fields
        private static Boolean _isAuthorized;
        #endregion

        #region Properties
        public static Boolean IsAuthorized
        {
            get
            {
                return _isAuthorized;
            }
            set
            {
                if (_isAuthorized != value)
                {
                    _isAuthorized = value;
                }
            }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public static Boolean GetApplicationKey()
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                ConfigurationSectionGroup section = config.GetSectionGroup("userSettings");
                String stringXml = section.Sections[0].SectionInformation.GetRawXml();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(stringXml);
                XmlNode node = doc.SelectSingleNode("/Interact.UserInterface.Properties.Settings/setting[@name='ApplicationKey']");

                Settings settings = Settings.CreateSettings(node.InnerText);
                _isAuthorized = settings.IsAuthorized;
            }
            catch
            {
                _isAuthorized = false;
            }

            return _isAuthorized;
        }
        #endregion

        #region Overrides
        #endregion
    }
}
