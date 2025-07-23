using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common
{
    public partial class CreateBatchDialog : Form
    {
        public CreateBatchDialog()
        {
            InitializeComponent();

            createBatchControl.CredentialHost = UserAuthenticationControl.UserSource;
            createBatchControl.BatchCreated += CreateBatchControl_BatchCreated;
        }

        private void CreateBatchControl_BatchCreated(object sender, EventArgs e)
        {
            string message = "Would you like to add another?";
            DialogResult r = MessageBox.Show(this, message, "Batch Created", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.No)
            { this.DialogResult = DialogResult.OK; }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            try
            {
                ErrorProvider.Clear();
                UserAuthenticationControl.RequestUserAuthentication();
                createBatchControl.Clear();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                ErrorProvider.SetError(UserAuthenticationControl, ex.Message);
            }
        }
    }
}
