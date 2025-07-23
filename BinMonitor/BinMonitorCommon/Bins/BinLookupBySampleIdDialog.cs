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
    public partial class BinLookupBySpecimenIdDialog : Form
    {
        public BinLookupBySpecimenIdDialog()
        {
            InitializeComponent();
        }

        private void Lookup()
        {
            ErrorProvider.Clear();
            try
            {
                lbBins.SelectedIndex = -1;
                lbBins.DataSource = null;
                string SpecimenId = txtSpecimenId.Text;
                if (string.IsNullOrWhiteSpace(SpecimenId))
                { throw new InvalidOperationException("Specimen Id is required"); }
                Bin[] bins = Bins.Instance.GetBinsContainingSpecimenWithWildcard(SpecimenId).ToArray();
                if (bins.Count() == 0)
                {
                    throw new KeyNotFoundException("Specimen not found");
                        
                }

                List<string> displayValues = new List<string>();
                foreach (Bin bin in bins)
                {
                    string binId = bin.Id;
                    string msg = string.Empty;

                    SpecimenBatch batch = bin.Batch;
                    if (batch != null)
                    {
                        if (batch.IsClosed)
                        { msg = "Closed by " + batch.ClosedBy; }
                        if (batch.Processing.HasCompleted)
                        { msg = "Processing completed by " + batch.Processing.CompletedBy; }
                        else if (batch.Processing.HasStarted)
                        { msg = "Processing started by " + batch.Processing.AssignedTo; }
                        else if (batch.Registration.HasCompleted)
                        { msg = "Registration completed by " + batch.Registration.CompletedBy; }
                        else if (batch.Registration.HasStarted)
                        { msg = "Registration started by " + batch.Registration.AssignedTo; }
                        else
                        { msg = "Created by " + batch.CreatedBy; }
                    }

                    displayValues.Add(string.Format("{0} - {1}", binId, msg));
                }

                lbBins.DataSource = displayValues;
                lbBins.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                ErrorProvider.SetError(txtSpecimenId, ex.Message);
            }
        }

        private void btnLookupBinBySpecimenId_Click(object sender, EventArgs e)
        {
            Lookup();
        }

        private void txtSpecimenId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            { Lookup(); }
        }

        
    }
}
