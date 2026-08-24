using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Interact.BusinessLogic
{
    public class PropertyComparer<T> : IComparer<T>
    {
        // This contains code implemented by Rockford Lhotka:
        // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnadvnet/html/vbnet01272004.asp
        // Can also use the EntityPropertyComparer of NetTiers...

        private PropertyDescriptor property;
        private ListSortDirection direction;

        public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
        {
            this.property = property;
            this.direction = direction;
        }

        private object GetPropertyValue(T value, string property)
        {
            PropertyInfo propertyInfo = value.GetType().GetProperty(property);
            return propertyInfo.GetValue(value, null);
        }

        private int CompareAscending(object xValue, object yValue)
        {
            int result;
            if (xValue is IComparable)
            {
                result = ((IComparable)xValue).CompareTo(yValue);
            }
            else if (xValue.Equals(yValue))
            {
                result = 0;
            }
            else
            {
                result = xValue.ToString().CompareTo(yValue.ToString());
            }
            return result;
        }

        private int CompareDescending(object xValue, object yValue)
        {
            return CompareAscending(xValue, yValue) * -1;
        }

        #region IComparer<T>
        public int Compare(T xWord, T yWord)
        {
            object xValue = GetPropertyValue(xWord, property.Name);
            object yValue = GetPropertyValue(yWord, property.Name);
            if (xValue == null || yValue == null)
            {
                return 1;
            }
            if (direction == ListSortDirection.Ascending)
            {
                return CompareAscending(xValue, yValue);
            }
            else
            {
                return CompareDescending(xValue, yValue);
            }
        }

        public bool Equals(T xWord, T yWord)
        {
            return xWord.Equals(yWord);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
        #endregion
    }
}
