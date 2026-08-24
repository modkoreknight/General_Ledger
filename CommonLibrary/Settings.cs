using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.Common
{
    public static class SettingsOld
    {
        #region Fields
        private static Byte _schoolYearID;
        private static Int16 _enrolled;
        private static Int16 _notEnrolled;
        #endregion

        #region Properties
        public static Byte SchoolYearID
        {
            get
            {
                return _schoolYearID;
            }
            set
            {
                _schoolYearID = value;
            }
        }

        public static Int16 Enrolled
        {
            get
            {
                return _enrolled;
            }
            set
            {
                _enrolled = value;
            }
        }

        public static Int16 NotEnrolled
        {
            get
            {
                return _notEnrolled;
            }
            set
            {
                _notEnrolled = value;
            }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        #endregion
    }
}
