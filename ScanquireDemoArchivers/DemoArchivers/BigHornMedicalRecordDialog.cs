using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DemoArchivers
{
    public partial class BigHornMedicalRecordDialog : Form
    {
        string Year 
        {
            get { return txtYear.Text; }
            set { txtYear.Text = value; }
        }

        string LastName
        {
            get { return txtLastName.Text; }
            set { txtLastName.Text = value; }
        }

        string FirstName
        {
            get { return txtFirstName.Text; }
            set { txtFirstName.Text = value; }
        }

        public string MRN
        {
            get { return txtMRN.Text; }
            set { txtMRN.Text = value; }
        }

        public BigHornMedicalRecordDialog()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool valid = true;

            ErrorProvider.Clear();

            if (txtYear.MaskCompleted == false)
            {
                ErrorProvider.SetError(txtYear, "Invalid year (YYYY)");
                valid = false;
            }
            if (string.IsNullOrEmpty(LastName))
            {
                ErrorProvider.SetError(txtLastName, "Required");
                valid = false;
            }
            if (string.IsNullOrEmpty(FirstName))
            {
                ErrorProvider.SetError(txtFirstName, "Required");
                valid = false;
            }
            if (string.IsNullOrEmpty(MRN))
            {
                ErrorProvider.SetError(txtMRN, "Required");
            }

            if (valid)
            { this.DialogResult = DialogResult.OK;}
            
        }
    }
}
