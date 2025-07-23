using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scanquire.Clients.NYP
{
    public partial class SendoutRecordDialog : Form
    {
        public bool UsePatchCards
        {
            get { return chkUsePatchCards.Checked; }
            set { chkUsePatchCards.Checked = value; }
        }

        public string PerformingLab
        {
            get { return (string)cmbPerformingLab.Text; }
            set { cmbPerformingLab.Text = value; }
        }

        public string AccessionNumber
        {
            get { return txtAccessionNumber.Text; }
            set { txtAccessionNumber.Text = value; }
        }

        public string FinancialNumber
        {
            get { return txtFinancialNumber.Text; }
            set { txtFinancialNumber.Text = value; }
        }

        public string MedicalRecordNumber
        {
            get { return txtMedicalRecordNumber.Text; }
            set { txtMedicalRecordNumber.Text = value; }
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

        public DateTime? DateOfService
        {
            get { return dpDateOfService.Value; }
            set { dpDateOfService.Value = value; }
        }

        public SendoutRecordDialog()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            errorProvider.Clear();
            AccessionNumber = null;
            FinancialNumber = null;
            MedicalRecordNumber = null;
            FirstName = null;
            LastName = null;
            dpDateOfService.Clear();
        }

        private void SendoutRecordDialog_Load(object sender, EventArgs e)
        {

        }

        private void chkUsePatchCards_CheckedChanged(object sender, EventArgs e)
        {
            grpRecordSettings.Enabled = (chkUsePatchCards.Checked == false);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool errors = false;
            errorProvider.Clear();
            if (chkUsePatchCards.Checked == false)
            {
                if (string.IsNullOrWhiteSpace(this.PerformingLab))
                { 
                    errorProvider.SetError(cmbPerformingLab, "Required");
                    errors = true;
                }
                if (string.IsNullOrWhiteSpace(this.AccessionNumber))
                { 
                    errorProvider.SetError(txtAccessionNumber, "Required");
                    errors = true;
                }
                if (string.IsNullOrWhiteSpace(this.FinancialNumber))
                { 
                    errorProvider.SetError(txtFinancialNumber, "Required");
                    errors = true;
                }
                if (string.IsNullOrWhiteSpace(this.MedicalRecordNumber))
                { 
                    errorProvider.SetError(txtMedicalRecordNumber, "Required");
                    errors = true;
                }
                if (string.IsNullOrWhiteSpace(this.LastName))
                { 
                    errorProvider.SetError(txtLastName, "Required");
                    errors = true;
                }
                if (string.IsNullOrWhiteSpace(this.FirstName))
                { 
                    errorProvider.SetError(txtFirstName, "Required");
                    errors = true;
                }
                if (dpDateOfService.HasValue == false)
                {
                    errorProvider.SetError(dpDateOfService, "Required");
                    errors = true;
                }
                
            }
            if (errors == false)
            { this.DialogResult = DialogResult.OK; }
        }
    }
}
