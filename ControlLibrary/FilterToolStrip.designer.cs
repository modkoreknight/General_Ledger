using System.Windows.Forms;

namespace Interact.Control
{
    partial class FilterToolStrip
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterToolStrip));
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.ttbValueToFilter = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tcbColumnToFilter = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbFilter = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbRemoveFilter = new System.Windows.Forms.ToolStripButton();
            this.SuspendLayout();
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.AutoSize = false;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(61, 22);
            this.toolStripLabel1.Text = "Filter for:";
            this.toolStripLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ttbValueToFilter
            // 
            this.ttbValueToFilter.Name = "ttbValueToFilter";
            this.ttbValueToFilter.Size = new System.Drawing.Size(100, 21);
            this.ttbValueToFilter.ToolTipText = "Text to filter";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 6);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.AutoSize = false;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(55, 13);
            this.toolStripLabel2.Text = "Filter in:";
            this.toolStripLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tcbColumnToFilter
            // 
            this.tcbColumnToFilter.Name = "tcbColumnToFilter";
            this.tcbColumnToFilter.Size = new System.Drawing.Size(121, 21);
            this.tcbColumnToFilter.ToolTipText = "Column to filter";
            this.tcbColumnToFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.tcbColumnToFilter.Sorted = true;
            this.tcbColumnToFilter.GotFocus += new System.EventHandler(this.tcbColumnToFilter_GotFocus);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 6);
            // 
            // tsbFilter
            // 
            this.tsbFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsbFilter.Image")));
            this.tsbFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFilter.Name = "tsbFilter";
            this.tsbFilter.Size = new System.Drawing.Size(51, 20);
            this.tsbFilter.Text = "Filter";
            this.tsbFilter.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 6);
            // 
            // tsbRemoveFilter
            // 
            this.tsbRemoveFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsbRemoveFilter.Image")));
            this.tsbRemoveFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRemoveFilter.Name = "tsbRemoveFilter";
            this.tsbRemoveFilter.Size = new System.Drawing.Size(91, 20);
            this.tsbRemoveFilter.Text = "Remove filter";
            this.tsbRemoveFilter.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // FilterToolStrip
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.ttbValueToFilter,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.tcbColumnToFilter,
            this.toolStripSeparator2,
            this.tsbFilter,
            this.toolStripSeparator3,
            this.tsbRemoveFilter});
            this.ResumeLayout(false);

        }

        #endregion

        private ToolStripLabel toolStripLabel1;
        private ToolStripTextBox ttbValueToFilter;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel toolStripLabel2;
        private ToolStripComboBox tcbColumnToFilter;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton tsbFilter;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton tsbRemoveFilter;
    }
}
