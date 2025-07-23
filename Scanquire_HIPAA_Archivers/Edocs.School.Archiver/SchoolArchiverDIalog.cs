using EdocsUSA.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EdocsUSA.Utilities.Logging;
namespace Edocs.School.Archiver
{
    public partial class SchoolArchiverDIalog : Form
    {
        public string TrackingID
        {
            get { return txtBoxTrackingID.Text; }
            set { value = txtBoxTrackingID.Text; }
        }
        public string EmpFName
        {
            get { return txtBoxEmpFname.Text; }
            set { value = txtBoxEmpFname.Text; }
        }
        public string EmpLName
        {
            get { return txtBoxEmpLName.Text; }
            set { value = txtBoxEmpLName.Text; }
        }
        public string EmpID
        {
            get { return txtBoxEmpID.Text; }
            set { value = txtBoxEmpID.Text; }
        }
         
        public bool ShowTotalRecordsScanned
        { get; set; }
         
        public string TotalScanned
        {
            get { return txtTRecScanned.Text; }
            set { value = txtTRecScanned.Text; }
        }
        public string[] CmbBoxItems
        { get; set; }
        public string TxtDialog
        { get; set; }
        public SchoolArchiverDIalog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBoxTrackingID.Text))
            {
                MessageBox.Show("Tracking ID Required", "Tracking ID:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.DialogResult = DialogResult.OK;
        }
        private void Clear()
        {
            txtBoxEmpFname.Text = string.Empty;
            txtBoxEmpID.Text = string.Empty;
            txtBoxEmpLName.Text = string.Empty;
             
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void SchoolArchiverDIalog_Shown(object sender, EventArgs e)
        {
             
            txtTRecScanned.Text = TraceLogger.TraceLoggerInstance.GetTotalImagesScannedString(false);
            Clear();
            if (string.IsNullOrWhiteSpace(txtBoxTrackingID.Text))
                txtBoxTrackingID.Focus();
            else
            txtBoxEmpFname.Focus();

        }

        
    }
}
