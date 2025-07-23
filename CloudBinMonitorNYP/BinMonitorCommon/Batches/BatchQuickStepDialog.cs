using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common.Batches
{
    public partial class BatchQuickStepDialog : Form
    {
        public Action<SpecimenBatch, User> BatchAction
        { get; set; }

        public BatchQuickStepDialog()
        {
            InitializeComponent();
        }

        private void txtBinId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            { btnOk.PerformClick(); }
        }

        private void BatchQuickStepDialog_Shown(object sender, EventArgs e)
        {
            try
            {
                ErrorProvider.Clear();
                UserAuthenticationControl.RequestUserAuthentication();
                txtBinId.Focus();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                ErrorProvider.SetError(UserAuthenticationControl, ex.Message);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ErrorProvider.Clear();
            User user = UserAuthenticationControl.UserSource.SelectedUser;

            if (user == null)
            {
                ErrorProvider.SetError(UserAuthenticationControl, "Authenticated user is required");
                return;
            }

            try
            {
                string binId = Bin.NormalizeId(txtBinId.Text);
                if (string.IsNullOrWhiteSpace(binId))
                { ErrorProvider.SetError(txtBinId, "Bin ID is required"); }

                Bin bin = Bins.Instance.EnsureGetValue(binId);
                SpecimenBatch batch = bin.EnsureGetBatch();

                if (BatchAction == null)
                { throw new InvalidOperationException("BatchAction not defined"); }
                
                BatchAction(batch, user);

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                ErrorProvider.SetError(txtBinId, ex.Message);
            }

        }

        public static void PerformBatchQuickStep(IWin32Window parent, string title, Action<SpecimenBatch, User> batchAction)
        {
            BatchQuickStepDialog dlg = new BatchQuickStepDialog() { StartPosition = FormStartPosition.CenterParent };
            dlg.Text = title;
            dlg.BatchAction = batchAction;
            if (dlg.ShowDialog(parent) != DialogResult.OK)
            { throw new OperationCanceledException(); }

            
        }        
    }
}
