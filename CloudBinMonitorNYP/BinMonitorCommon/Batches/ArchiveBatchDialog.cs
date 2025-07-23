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
    public partial class ArchiveBatchDialog : Form
    {
        public ArchiveBatchDialog()
        {
            InitializeComponent();

         
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            try
            {
                ErrorProvider.Clear();
      
                
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
    
            }
        }

 
    }
}
