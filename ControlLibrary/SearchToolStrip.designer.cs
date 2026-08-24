using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Interact.Control
{
    partial class SearchToolStrip
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchToolStrip));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.ttbSearchFor = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tcbSearchIn = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSearch = new System.Windows.Forms.ToolStripButton();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(100, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(63, 22);
            this.toolStripLabel1.Text = "Search for:";
            // 
            // ttbSearchFor
            // 
            this.ttbSearchFor.Name = "ttbSearchFor";
            this.ttbSearchFor.Size = new System.Drawing.Size(100, 23);
            this.ttbSearchFor.ToolTipText = "Text to search";
            this.ttbSearchFor.TextChanged += new System.EventHandler(this.ttbSearchFor_TextChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 6);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(58, 15);
            this.toolStripLabel2.Text = "Search in:";
            // 
            // tcbSearchIn
            // 
            this.tcbSearchIn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tcbSearchIn.Name = "tcbSearchIn";
            this.tcbSearchIn.Size = new System.Drawing.Size(121, 23);
            this.tcbSearchIn.Sorted = true;
            this.tcbSearchIn.ToolTipText = "Column to search";
            this.tcbSearchIn.GotFocus += new System.EventHandler(this.tcbSearchIn_GotFocus);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 6);
            // 
            // tsbSearch
            // 
            this.tsbSearch.Image = ((System.Drawing.Image)(resources.GetObject("tsbSearch.Image")));
            this.tsbSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSearch.Name = "tsbSearch";
            this.tsbSearch.Size = new System.Drawing.Size(62, 20);
            this.tsbSearch.Text = "Search";
            this.tsbSearch.Click += new System.EventHandler(this.tsbSearch_Click);
            // 
            // SearchToolStrip
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.ttbSearchFor,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.tcbSearchIn,
            this.toolStripSeparator2,
            this.tsbSearch});
            this.ResumeLayout(false);
        }

        #endregion

        private ToolStrip toolStrip1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripTextBox ttbSearchFor;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel toolStripLabel2;
        private ToolStripComboBox tcbSearchIn;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton tsbSearch;
    }
}
