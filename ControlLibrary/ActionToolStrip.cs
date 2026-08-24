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
    public partial class ActionToolStrip : ToolStrip
    {
        #region Fields
        private Boolean _isOperationCancelled;
        #endregion

        #region Properties
        public Boolean IsOperationCancelled
        {
            get
            {
                return this._isOperationCancelled;
            }
            set
            {
                this._isOperationCancelled = value;
            }
        }
        #endregion

        #region Constructors
        public ActionToolStrip()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: Add custom paint code here

            // Calling the base class OnPaint
            base.OnPaint(pe);
        }

        private void toolStripButton_Click(object sender, EventArgs e)
        {
            ToolStripButton myButton = (ToolStripButton)sender;
            switch (myButton.Text)
            {
                case "Insert":
                    if (!this._isOperationCancelled)
                    {
                        this.tsbInsert.Enabled = false;
                        this.tsbDelete.Enabled = false;
                        this.tsbSort.Enabled = false;
                        this.tsbSearch.Enabled = false;
                        this.tsbFilter.Enabled = false;
                        this.tsbPrint.Enabled = false;
                    }
                    break;
                default:
                    if (this.tsbInsert.Enabled == false)
                    {
                        this.tsbInsert.Enabled = true;
                        this.tsbDelete.Enabled = true;
                        this.tsbSort.Enabled = true;
                        this.tsbSearch.Enabled = true;
                        this.tsbFilter.Enabled = true;
                        this.tsbPrint.Enabled = true;
                    }
                    break;
            }
        }
        #endregion
    }
}
