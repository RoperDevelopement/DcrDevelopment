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
    public partial class ArchiveBatchLookupControl : UserControl
    {

        private IUserSource _CredentialHost = null;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IUserSource CredentialHost
        {
            get { return _CredentialHost; }
            set 
            { 
                _CredentialHost = value;
                OnCredentialHostChanged();
            }
        }

        protected void OnCredentialHostChanged()
        {
            ManageBinControl.CredentialHost = this.CredentialHost;
            this.Enabled = CredentialHost != null;
        }

        public ArchiveBatchLookupControl()
        {
            InitializeComponent();
        }

        private void BatchLookup_SelectedBatchChanged(object sender, SelectedBatchChangedEventArgs e)
        {
            ManageBinControl.ActiveBatch = e.SelectedBatch;

        }

    }
}
