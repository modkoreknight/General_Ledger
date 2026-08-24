using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Interact.Control
{
    public partial class InteractForm : Form
    {
        public InteractForm()
        {
            InitializeComponent();
            MessageBox.Show("From parent form");
        }

        private void InteractForm_Load(object sender, EventArgs e)
        {
            //if (!Program.Settings.IsAuthorized)
            //{
            //    ValidateAppForm validateAppForm = new ValidateAppForm();
            //    if (validateAppForm.ShowDialog(this) == DialogResult.OK)
            //    {
            //        if (Program.Settings.IsAuthorized)
            //        {
            //            //Save new application key...
            //            Properties.Settings.Default.ApplicationKey = Program.Settings.ApplicationKey;
            //            Properties.Settings.Default.Save();

            //            this.AuthenticateUser();
            //            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            //            {
            //                System.Deployment.Application.ApplicationDeployment ad = System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
            //                this.Text = "RMC " + Utility.EnumDecode(Program.Settings.Branch.ToString()) + " - General Ledger v" + ad.CurrentVersion.ToString() + "...";
            //            }
            //            else
            //            {
            //                this.Text = "RMC " + Utility.EnumDecode(Program.Settings.Branch.ToString()) + " - General Ledger v0.0...";
            //            }
            //        }
            //        else
            //        {
            //            MessageBox.Show("Unable to validate the application.", validateAppForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            this.Close();
            //            this.Dispose();
            //        }
            //    }
            //    else
            //    {
            //        this.Close();
            //        this.Dispose();
            //    }
            //}
            //else
            //{
            //    this.AuthenticateUser();
            //    if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            //    {
            //        System.Deployment.Application.ApplicationDeployment ad = System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
            //        this.Text = "RMC " + Utility.EnumDecode(Program.Settings.Branch.ToString()) + " - General Ledger v" + ad.CurrentVersion.ToString() + "...";
            //    }
            //    else
            //    {
            //        this.Text = "RMC " + Utility.EnumDecode(Program.Settings.Branch.ToString()) + " - General Ledger v0.0...";
            //    }
            //}
        }
    }
}
