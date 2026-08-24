using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Interact.Control
{
    [ToolboxBitmap(typeof(ToolStrip))]
    public partial class FilterToolStrip : ToolStrip
    {
        private BindingSource bindingSource;

        public BindingSource BindingSource
        {
            get
            {
                return this.bindingSource;
            }
            set
            {
                this.bindingSource = value;
            }
        }

        public FilterToolStrip()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: Add custom paint code here

            // Calling the base class OnPaint
            base.OnPaint(pe);
        }

        private void Filter()
        {
            if (this.bindingSource == null)
            {
                return;
            }
            if (!this.bindingSource.SupportsFiltering)
            {
                return;
            }
            String filterFor = this.ttbValueToFilter.Text;
            if (String.IsNullOrEmpty(filterFor))
            {
                return;
            }
            String filterIn = this.tcbColumnToFilter.Text;
            if (String.IsNullOrEmpty(filterIn))
            {
                return;
            }
            PropertyDescriptorCollection properties = ((ITypedList)this.bindingSource).GetItemProperties(null);
            foreach (PropertyDescriptor property in properties)
            {
                if (property.Description == filterIn)
                {
                    this.bindingSource.Filter = property.Name + " = '" + filterFor + "'";
                    break;
                }
            }
        }

        private void tcbColumnToFilter_GotFocus(object sender, EventArgs e)
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
            this.tcbColumnToFilter.Items.Clear();
            PropertyDescriptorCollection properties = ((ITypedList)this.bindingSource).GetItemProperties(null);
            foreach (PropertyDescriptor property in properties)
            {
                if (property.PropertyType == typeof(string) && property.Name != "EntityTable")
                {
                    this.tcbColumnToFilter.Items.Insert(0, property.Description);
                }
            }
            if (this.tcbColumnToFilter.Items.Count > 0)
            {
                this.tcbColumnToFilter.SelectedIndex = 0;
            }
        }

        private void toolStripButton_Click(object sender, EventArgs e)
        {
            ToolStripButton myAction = (ToolStripButton)sender;
            switch (myAction.ToString())
            {
                case "Filter":
                    this.Filter();
                    break;
                case "Remove filter":
                    this.bindingSource.RemoveFilter();
                    break;
            }
        }
    }
}
