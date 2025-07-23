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
    public partial class GrantReceiptsRecordDialog : Form
    {
        public DateTime? DocumentDate
        {
            get { return dpDocumentDate.Value; }
            set { dpDocumentDate.Value = value; }
        }

        public string ClientCode
        {
            get { return txtClientCode.Text; }
            set { txtClientCode.Text = value; }
        }

        public string Comments
        {
            get { return txtComments.Text; }
            set { txtComments.Text = value; }
        }

        public GrantReceiptsRecordDialog()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (DocumentDate.HasValue && string.IsNullOrWhiteSpace(ClientCode) == false)
            { btnOk.Focus(); }
            else
            { dpDocumentDate.Focus(); }
        }

        public void Clear()
        {
            dpDocumentDate.Clear();
            txtClientCode.Clear();
            txtComments.Clear();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (DocumentDate.HasValue == false)
                { throw new Exception("Document Date is required"); }
                if (string.IsNullOrWhiteSpace(ClientCode))
                { throw new Exception("Client Code is required"); }

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
