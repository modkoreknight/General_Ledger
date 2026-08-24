using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Interact.BusinessLogic;

namespace Interact.Control
{
    [ToolboxBitmap(typeof(ToolStrip))]
    public partial class SearchToolStrip : ToolStrip
    {
        private Boolean isItemFound;
        private BindingSource bindingSource;
        
        private DataTable table;
        private DataView view;
        private Int32 counter = 0;
        
        public BindingSource BindingSource
        {
            get
            {
                return this.bindingSource;
            }
            set
            {
                this.bindingSource = value;
                if (this.bindingSource != null)
                {
                    table = ConvertToDataTable(this.bindingSource);
                    view = new DataView(table);
                }
            }
        }

        public SearchToolStrip()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: Add custom paint code here

            // Calling the base class OnPaint
            base.OnPaint(pe);
        }

        private void Search()
        {
            if (this.bindingSource == null)
            {
                return;
            }
            if (!((IBindingList)this.bindingSource).SupportsSearching)
            {
                return;
            }
            String searchFor = this.ttbSearchFor.Text;
            if (String.IsNullOrEmpty(searchFor))
            {
                return;
            }
            String searchIn = this.tcbSearchIn.Text;
            if (String.IsNullOrEmpty(searchIn))
            {
                return;
            }
            PropertyDescriptorCollection properties = ((ITypedList)this.bindingSource).GetItemProperties(null);

            int index = -1;
            foreach (PropertyDescriptor property in properties)
            {
                if (property.Description == searchIn)
                {
                    if (counter == 0)
                    {
                        index = this.bindingSource.Find(property, searchFor);
                    }
                    if (index != -1)
                    {
                        this.isItemFound = true;
                    }
                    else
                    {
                        view.RowFilter = String.Format(property.Name + " LIKE '*{0}*'", searchFor);
                        if (view.Count != 0)
                        {
                            if (counter < view.Count)
                            {
                                index = this.bindingSource.Find(property, view[counter][property.Name]);
                                counter++;
                                this.isItemFound = true;
                            }
                        }
                    }
                    this.OnItemFound(new ItemFoundEventArgs(index, isItemFound));
                    break;
                }
            }
            if (this.isItemFound && (index == -1))
            {
                this.isItemFound = false;
            }
        }

        protected virtual void OnItemFound(ItemFoundEventArgs e)
        {
            if (this.ItemFound != null)
            {
                this.ItemFound(this, e);
            }
        }

        private void tcbSearchIn_GotFocus(object sender, EventArgs e)
        {
            //TODO: Should be initialized upon load - not everytime the control got focus...
            if (this.bindingSource == null)
            {
                return;
            }
            if (this.bindingSource.DataSource == null)
            {
                return;
            }
            this.tcbSearchIn.Items.Clear();
            PropertyDescriptorCollection properties = ((ITypedList)this.bindingSource).GetItemProperties(null);
            foreach (PropertyDescriptor property in properties)
            {
                if (property.PropertyType == typeof(string) && property.Name != "EntityTable")
                {
                    this.tcbSearchIn.Items.Insert(0, property.Description);
                }
            }
            if (this.tcbSearchIn.Items.Count > 0)
            {
                this.tcbSearchIn.SelectedIndex = 0;
            }
        }

        private void tsbSearch_Click(object sender, EventArgs e)
        {
            this.Search();
        }

        private void ttbSearchFor_TextChanged(object sender, EventArgs e)
        {
            counter = 0;
        }

        public class ItemFoundEventArgs : EventArgs
        {
            private int index;
            private Boolean isItemFound;

            public ItemFoundEventArgs(int index, Boolean isItemFound)
            {
                this.index = index;
                this.isItemFound = isItemFound;
            }

            /// <summary>
            /// Returns the zero-based index of the item.
            /// Returns -1 if no more items are found.
            /// </summary>
            public int Index
            {
                get
                {
                    return this.index;
                }
            }

            /// <summary>
            /// Returns false if no item was found, otherwise true.
            /// </summary>
            public Boolean IsItemFound
            {
                get
                {
                    return this.isItemFound;
                }
            }
        }
                
        public delegate void ItemFoundEventHandler(object sender, ItemFoundEventArgs e);

        public event ItemFoundEventHandler ItemFound;

        private DataTable ConvertToDataTable(BindingSource bs)
        {
            PropertyDescriptorCollection properties = ((ITypedList)bs).GetItemProperties(null);
            DataTable dt = CreateDataTable(properties);
            if (bs.Count != 0)
            {
                foreach (object o in bs)
                    FillData(properties, dt, o);
            }
            return dt;
        }

        private DataTable CreateDataTable(PropertyDescriptorCollection properties)
        {
            DataTable dt = new DataTable();
            DataColumn dc = null;
            foreach (PropertyDescriptor pi in properties)
            {
                dc = new DataColumn();
                dc.ColumnName = pi.Name;
                dc.DataType = pi.PropertyType;
                dt.Columns.Add(dc);
            }
            return dt;
        }

        private void FillData(PropertyDescriptorCollection properties, DataTable dt, Object o)
        {
            DataRow dr = dt.NewRow();
            foreach (PropertyDescriptor pi in properties)
            {
                dr[pi.Name] = pi.GetValue(o);
            }
            dt.Rows.Add(dr);
        }
    }
}
