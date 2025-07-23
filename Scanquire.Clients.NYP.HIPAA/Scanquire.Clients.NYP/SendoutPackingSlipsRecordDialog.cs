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
    public partial class SendoutPackingSlipsRecordDialog : Form
    {
        public DateTime? Date
        {
            get { return dpDate.Value; }
            set { dpDate.Value = value; }
        }

        public SendoutPackingSlipsRecordDialog()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool errors =false;
            errorProvider.Clear();
             
            if (dpDate.HasValue == false)
            {
                errorProvider.SetError(dpDate, "Date is required");
                errors = true;
            }

            if (errors == false)
            { this.DialogResult = DialogResult.OK; }
        }

        public void Clear()
        { dpDate.Clear(); }
    }
}
