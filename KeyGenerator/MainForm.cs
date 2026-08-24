using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interact.BusinessLogic;

namespace KeyGenerator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            foreach (String str in Enum.GetNames(typeof(Branch)))
            {
                cmbBranch.Items.Add(Utility.EnumDecode(str));
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name)
            {
                case "btnGenerate":
                    Branch branch = (Branch)Enum.Parse(typeof(Branch), Utility.EnumEncode(cmbBranch.SelectedItem.ToString()));
                    String machineCode = txtMachineCode.Text;
                    //Machine code should be at least 8 characters long...
                    String salt = branch.ToString() + "1n+3r@c+" + machineCode.Substring((machineCode.Length / 2) - 4, 8);
                    String applicationKey = AESCryptography.CreatePasswordHash(machineCode, salt);
                    applicationKey = applicationKey.Insert(10, ((Int32)branch).ToString("D3"));
                    if (branch == Branch.Area_Supervisor_ || branch == Branch.Verifier_ || branch == Branch.HO_)
                    {
                        applicationKey = applicationKey.Insert(23, "X");
                    }
                    else
                    {
                        applicationKey = applicationKey.Insert(23, "Y");
                    }
                    txtApplicationKey.Text = applicationKey;
                    break;
                case "btnClose":
                    DialogResult drExit = MessageBox.Show("Are you sure you want to quit?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (drExit == DialogResult.Yes)
                    {
                        this.Close();
                        this.Dispose();
                    }
                    break;
            }
        }
    }
}
