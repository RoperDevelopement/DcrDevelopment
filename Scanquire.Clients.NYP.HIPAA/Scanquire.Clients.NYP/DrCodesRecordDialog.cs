using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scanquire.Clients.NYP
{
    public partial class DrCodesRecordDialog : Form
    {
        public bool UsePatchCards
        {
            get { return chkUsePatchCards.Checked; }
            set { chkUsePatchCards.Checked = value; }
        }

        public string DrCode
        {
            get { return txtDrCode.Text; }
            set { txtDrCode.Text = value; }
        }

        public string LastName
        {
            get { return txtLastName.Text; }
            set { txtLastName.Text = value; }
        }

        public string FirstName
        {
            get { return txtFirstName.Text; }
            set { txtFirstName.Text = value; }
        }

        public DrCodesRecordDialog()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool errors = false;
            errorProvider.Clear();
            if (chkUsePatchCards.Checked == false)
            {
                if (string.IsNullOrWhiteSpace(DrCode))
                {
                    errorProvider.SetError(txtDrCode, "Dr Code is required");
                    errors = true;
                }

                if (string.IsNullOrWhiteSpace(LastName))
                {
                    errorProvider.SetError(txtLastName, "Last Name is required");
                    errors = true;
                }

                if (string.IsNullOrWhiteSpace(FirstName))
                {
                    errorProvider.SetError(txtFirstName, "First Name is required");
                    errors = true;
                }
            }
            if (errors == false)
            { this.DialogResult = DialogResult.OK; }
        }

        public void Clear()
        {
            errorProvider.Clear();
            txtDrCode.Clear();
            txtLastName.Clear();
            txtFirstName.Clear();
        }

        private void chkUsePatchCards_CheckedChanged(object sender, EventArgs e)
        {
            grpRecordSettings.Enabled = (chkUsePatchCards.Checked == false);
        }
    }
}
