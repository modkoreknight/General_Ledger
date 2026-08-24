using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Interact.BusinessLogic
{
    public class GenericList<T> : BindingList<T>, IBindingListView
    {
        private string filterString = null;
        private List<T> excludedItems = new List<T>();
        private ListSortDescriptionCollection sortDescriptions = new ListSortDescriptionCollection();
        private List<T> OriginalList = new List<T>();

        #region BindingList<T> Overrides
        private bool isSorted;
        private int findIndex = -1;
        private PropertyDescriptor sortProperty;
        private ListSortDirection sortDirection;

        #region Sorting
        protected override bool SupportsSortingCore
        {
            get
            {
                return true;
            }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get
            {
                return sortDirection;
            }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get
            {
                return sortProperty;
            }
        }

        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
        {
            //DataGridView sorting
            List<T> items = this.Items as List<T>;
            if (items != null)
            {
                PropertyComparer<T> pc = new PropertyComparer<T>(property, direction);
                items.Sort(pc);
                isSorted = true;
            }
            else
            {
                isSorted = false;
            }
            sortProperty = property;
            sortDirection = direction;
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override bool IsSortedCore
        {
            get { return isSorted; }
        }

        protected override void RemoveSortCore()
        {
            isSorted = false;
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
        #endregion

        #region Searching
        protected override bool SupportsSearchingCore
        {
            get
            {
                return true;
            }
        }

        protected override int FindCore(PropertyDescriptor property, object key)
        {
            if (property == null)
            {
                return -1;
            }
            List<T> items = this.Items as List<T>;
            foreach (T item in items)
            {
                string value = (string)property.GetValue(item);
                if ((string)key == value)
                {
                    if (this.IndexOf(item) > findIndex)
                    {
                        findIndex = this.IndexOf(item);
                        return findIndex;
                    }
                }
            }
            findIndex = -1;
            return findIndex;
        }
        #endregion
        #endregion

        #region IBindingListView Members
        //BindingSource sorting
        public void ApplySort(ListSortDescriptionCollection sorts)
        {
            sortProperty = null;
            sortDescriptions = sorts;
            SortComparer<T> comparer = new SortComparer<T>(sorts);
            ApplySortInternal(comparer);
        }

        public string Filter
        {
            get
            {
                return this.filterString;
            }
            set
            {
                if (value == this.filterString) return;
                this.filterString = value;
                this.ApplyFilter();
            }
        }

        public void RemoveFilter()
        {
            this.Filter = string.Empty;
        }

        public ListSortDescriptionCollection SortDescriptions
        {
            get { return sortDescriptions; }
        }

        public bool SupportsAdvancedSorting
        {
            get { return true; }
        }

        public bool SupportsFiltering
        {
            get { return true; }
        }

        public void ApplyFilter()
        {
            for (int i = 0; i < this.excludedItems.Count; i++)
            {
                this.Add(this.excludedItems[i]);
            }
            this.excludedItems.Clear();
            if (this.filterString != null & this.filterString.Length > 0)
            {
                Filter MyFilter = new Filter(this, this.excludedItems, typeof(T), this.filterString);
            }
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0, 0));
        }

        public void ApplyFilter(Predicate<T> match)
        {
            this.filterString = string.Empty;
            for (int i = 0; i < this.excludedItems.Count; i++)
            {
                this.Add(this.excludedItems[i]);
            }
            this.excludedItems.Clear();
            IList iList = (IList)this;
            for (int i = this.Items.Count - 1; i >= 0; i--)
            {
                if (!match(this.Items[i]))
                {
                    this.excludedItems.Add(this.Items[i]);
                    iList.RemoveAt(i);
                }
            }
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0, 0));
        }

        /// <summary>
        /// Sorts the elements in the entire list using the specified <see cref="System.Comparison{T}"/>.
        /// </summary>
        /// <param name="comparison">The <see cref="System.Comparison{T}"/> to use when comparing elements.</param>
        /// <exception cref="ArgumentNullException">comparison is a null reference.</exception>
        private void ApplySortInternal(Comparison<T> comparison)
        {
            if (comparison == null)
                throw new ArgumentNullException("The comparison parameter must be a valid object instance.");

            if (OriginalList.Count == 0)
            {
                OriginalList.AddRange(this);
            }

            List<T> listRef = this.Items as List<T>;

            if (listRef == null)
                return;

            listRef.Sort(comparison);
            isSorted = true;
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        /// <summary>
        /// Sorts the elements in the entire list using the specified comparer. 
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing elements, or a null reference (Nothing in Visual Basic) to use the default comparer <see cref="Comparer.Default"/>.</param>
        private void ApplySortInternal(IComparer<T> comparer)
        {
            if (comparer == null)
                comparer = Comparer<T>.Default;

            ApplySortInternal(comparer.Compare);
        }

        //TODO: Sort - new implementations in new templates...
        // But this already works

        //TODO: User controls - abstraction of methods from UI
        #endregion

        //#region IComparable Members
        //public int CompareTo(object obj)
        //{
        //    throw new NotImplementedException();
        //}
        //#endregion
    }
}
