using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace BinMonitor.Common
{
    public partial class SpecimenLookupControl : UserControl
    {
        public SpecimenLookupControl()
        {
            InitializeComponent();
        }

        private void btnLookup_Click(object sender, EventArgs e)
        {
            try
            {
                string SpecimenId = txtSpecimenId.Text;
                if (string.IsNullOrWhiteSpace(SpecimenId))
                { throw new InvalidOperationException("Specimen Id is required"); }

                Bin[] bins = Bins.Instance.GetBinsContainingSpecimenWithWildcard(SpecimenId).ToArray();
                
                lbBins.DataSource = bins;
                if (bins.Count() == 0)
                { MessageBox.Show("Specimen not found"); }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }
    }
}
