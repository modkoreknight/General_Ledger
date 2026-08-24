using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public static class Utility
    {
        #region Fields
        private static Byte _pageSize = 20;
        public static String DefaultCustomerPicture;
        public static String DefaultCustomerPictureAlt;
        #endregion

        #region Properties
        public static Byte PageSize
        {
            get
            {
                return Utility._pageSize;
            }
            set
            {
                Utility._pageSize = value;
            }
        }
        #endregion

        #region Constructors
        static Utility()
        {
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ImagePath"]))
            {
                Utility.DefaultCustomerPicture = ConfigurationManager.AppSettings["ImagePath"].ToString() + "NewCustomer.png";
            }
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ImagePathAlt"]))
            {
                Utility.DefaultCustomerPictureAlt = ConfigurationManager.AppSettings["ImagePathAlt"].ToString() + "NewCustomer.png";
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Converts a string into an SQL-encoded string.
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static String SqlEncode(String strSql)
        {
            strSql = strSql.Replace("'", "''");
            strSql = strSql.Replace("[", "[[]");
            strSql = strSql.Replace("%", "[%]");
            strSql = strSql.Replace("_", "[_]");
            strSql = strSql.Replace(";", "[;]");
            strSql = strSql.Replace("--", "[--]");
            strSql = strSql.Replace("/*", "[/*]");
            return strSql;
        }

        /// <summary>
        /// Converts a SQL-encoded string into a decoded string.
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static String SqlDecode(String strSql)
        {
            strSql = strSql.Replace("''", "'");
            strSql = strSql.Replace("[[]", "[");
            strSql = strSql.Replace("[%]", "%");
            strSql = strSql.Replace("[_]", "_");
            strSql = strSql.Replace("[;]", ";");
            strSql = strSql.Replace("[--]", "--");
            strSql = strSql.Replace("[/*]", "/*");
            return strSql;
        }

        /// <summary>
        /// Checks whether a bitwise value is a member of another bitwise value or not.
        /// </summary>
        /// <param name="operandX"></param>
        /// <param name="operandY"></param>
        /// <returns></returns>
        public static Boolean IsBitwiseMember(Int32 operandX, Int32 operandY)
        {
            Int32 i = operandX & operandY;
            if (i == operandY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //TODO: Get description of these 2 methods...
        public static String GetAlternateTextID(String alternateText)
        {
            String[] strArray = alternateText.Split(new Char[] { ':', ':' });
            return strArray.GetValue(0).ToString();
        }

        public static String GetAlternateTextName(String alternateText)
        {
            String[] strArray = alternateText.Split(new Char[] { ':', ':' });
            return strArray.GetValue(2).ToString();
        }

        /// <summary>
        /// Converts a readable string into an Enum string.
        /// </summary>
        /// <param name="strEnum"></param>
        /// <returns></returns>
        public static String EnumEncode(String strEnum)
        {
            strEnum = strEnum.Replace(" - ", "___");                    //space dash space -- 3
            strEnum = strEnum.Replace(": (", "___________________");    //collon-open(  -- 19
            strEnum = strEnum.Replace("., ", "______________");         //period comma-space -- 14
            strEnum = strEnum.Replace(" (", "____");                    //space open(  -- 4
            strEnum = strEnum.Replace(", ", "________");                //comma,space -- 8
            strEnum = strEnum.Replace(") [", "_______________________");// close)space[ -- 23
            strEnum = strEnum.Replace(") ", "_____");                   // close)space -- 5
            strEnum = strEnum.Replace(": ", "___________");             //colon-space  -- 10
            strEnum = strEnum.Replace("; ", "____________");            //semicolon-space -- 14
            strEnum = strEnum.Replace(". ", "_______________");         //period space -- 15
            strEnum = strEnum.Replace(");", "________________");        //close) semicolon -- 16
            strEnum = strEnum.Replace(").", "_________________");       //close) period -- 17
            strEnum = strEnum.Replace("),", "__________________");      //close) comma -- 18
            strEnum = strEnum.Replace("+", "_________");                //plus + -- 11
            strEnum = strEnum.Replace("/", "_______");                  //slash/ -- 7
            strEnum = strEnum.Replace("'", "______");                   //apostrophe' -- 6
            strEnum = strEnum.Replace("(", "____________________");     //open( -- 8
            strEnum = strEnum.Replace("-", "__");                       //dash  -- 2
            strEnum = strEnum.Replace("?", "_____________________");    //questionmark -- 19
            strEnum = strEnum.Replace(".", "__________");               //period -- 20
            strEnum = strEnum.Replace(" ", "_");                        //space  -- 21
            strEnum = strEnum.Replace("\"", "______________________");    //slash" -- 22
            strEnum = strEnum.Replace("]", "________________________");    //closing bracket -- 24
            return strEnum;
        }

        /// <summary>
        /// Converts an Enum-encoded string into a readable string.
        /// </summary>
        /// <param name="strEnum"></param>
        /// <returns></returns>
        public static String EnumDecode(String strEnum)
        {
            strEnum = strEnum.Replace("________________________", "]");
            strEnum = strEnum.Replace("_______________________", ") [");
            strEnum = strEnum.Replace("______________________", "\"");
            strEnum = strEnum.Replace("_____________________", "?");
            strEnum = strEnum.Replace("____________________", "(");
            strEnum = strEnum.Replace("___________________", ": (");
            strEnum = strEnum.Replace("__________________", "),");
            strEnum = strEnum.Replace("_________________", ").");
            strEnum = strEnum.Replace("________________", ");");
            strEnum = strEnum.Replace("_______________", ". ");
            strEnum = strEnum.Replace("______________", "., ");
            strEnum = strEnum.Replace("____________", "; ");
            strEnum = strEnum.Replace("___________", ": ");
            strEnum = strEnum.Replace("__________", ".");
            strEnum = strEnum.Replace("_________", "+");
            strEnum = strEnum.Replace("________", ", ");
            strEnum = strEnum.Replace("_______", "/");
            strEnum = strEnum.Replace("______", "'");
            strEnum = strEnum.Replace("_____", ") ");
            strEnum = strEnum.Replace("____", " (");
            strEnum = strEnum.Replace("___", " - ");
            strEnum = strEnum.Replace("__", "-");
            strEnum = strEnum.Replace("_", " ");
            return strEnum;
        }
        #endregion
    }
}
