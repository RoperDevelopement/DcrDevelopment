using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common.Batches
{
    public partial class TransferBatchControl : UserControl
    {
        public TransferBatchControl()
        {
            InitializeComponent();
        }

        private void txtLookupBinId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            { btnLookupBinById.PerformClick(); }
        }
    }
}
