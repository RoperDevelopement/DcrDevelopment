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
    public partial class DOHRecordDialogOLD : Form
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

        public string MedicalRecordNumber
        {
            get { return txtMedicalRecordNumber.Text; }
            set { txtMedicalRecordNumber.Text = value; }
        }

        public DateTime? DateOfService
        {
            get { return dpDateOfService.Value; }
            set { dpDateOfService.Value = value; }
        }


        public DOHRecordDialogOLD()
        {
            InitializeComponent();
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
                    this.cmbPerformingLab.Text = "NONE DOH OLD";
                }
                if (string.IsNullOrWhiteSpace(this.AccessionNumber))
                {
                    this.txtAccessionNumber.Text = "NONE DOH OLD";
                }
                if (string.IsNullOrWhiteSpace(this.MedicalRecordNumber))
                {
                    this.txtMedicalRecordNumber.Text = "NONE DOH OLD";
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

        public void Clear()
        {
            dpDateOfService.Clear();
            errorProvider.Clear();
            AccessionNumber = null;
            MedicalRecordNumber = null;
        }
    }
}
