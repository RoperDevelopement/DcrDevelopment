using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common
{
    public partial class AdminOverrideDialog : Form
    {
        static string EncodedPassword
        { get { return Settings.AdminOverride.Default.Password; } }

        static string EncodePassword(string value)
        { return Md5Util.ComputeHash(value); }

        public AdminOverrideDialog()
        {
            InitializeComponent();
        }

        public bool ValidateInput()
        {
            bool valid = true;
            ErrorProvider.Clear();
            if (cmbUsers.HasSelectedKey == false)
            {
                ErrorProvider.SetError(cmbUsers, "Required");
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ErrorProvider.SetError(txtPassword, "Required");
                valid = false;
            }

            if (EncodePassword(txtPassword.Text) != EncodedPassword)
            {
                ErrorProvider.SetError(txtPassword, "Wrong Password");
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(txtReason.Text))
            {
                ErrorProvider.SetError(txtReason, "Required");
                valid = false;
            }

            return valid;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateInput() == true)
            { this.DialogResult = DialogResult.OK; }
        }

    }
}
