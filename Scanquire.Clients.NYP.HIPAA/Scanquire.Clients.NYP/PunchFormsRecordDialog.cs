using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDL = EdocsUSA.Utilities.Logging;

namespace Scanquire.Clients.NYP
{
    public partial class PunchFormsRecordDialog : Form
    {
        public string LabLocation
        {
            get { return (string)cmbLocation.Text; }
            set { cmbLocation.Text = value; }
        }

        public DateTime? LogDate
        {
            get { return dpLogDate.Value; }
            set { dpLogDate.Value = value; }
        }

        public PunchFormsRecordDialog()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool errors = false;
            errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(LabLocation))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("LabLocation is required for missing punch forms archiver");
                errorProvider.SetError(cmbLocation, "Location is required");
                errors = true;
            }

            if (LogDate.HasValue == false)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("Logdate is required for missing punch forms archiver");
                errorProvider.SetError(dpLogDate, "Value Required");
                errors = true;
            }

            if (errors == false)
            { this.DialogResult = DialogResult.OK; }
        }

        public void Clear()
        {
            errorProvider.Clear();
            cmbLocation.SelectedIndex = -1;
            dpLogDate.Clear();
        }
    }
}
