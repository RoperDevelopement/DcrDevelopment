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
    public partial class InvoiceRecordDialog : Form
    {
        public DateTime? InvoiceDate
        {
            get { return dpInvoiceDate.Value; }
            set { dpInvoiceDate.Value = value; }
        }

        public string Department
        {
            get { return cmbDepartment.Text; }
            set { cmbDepartment.Text = value; }
        }

        public string Category
        {
            get { return cmbCategory.Text; }
            set { cmbCategory.Text = value; }
        }

        public string Account
        {
            get { return cmbAccount.Text; }
            set { cmbAccount.Text = value; }
        }

        public string Reference
        {
            get { return txtReference.Text; }
            set { txtReference.Text = value; }
        }

        public InvoiceRecordDialog()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool errors = false;
            errorProvider.Clear();

            if (dpInvoiceDate.HasValue == false)
            {
                errorProvider.SetError(dpInvoiceDate, "Required");
                errors = true;
            }
            if (string.IsNullOrWhiteSpace(Account))
            {
                errorProvider.SetError(cmbAccount, "Required");
                errors = true;
            }
            if (string.IsNullOrWhiteSpace(Department))
            {
                errorProvider.SetError(cmbDepartment, "Required");
                errors = true;
            }

            if (errors == false)
            { this.DialogResult = DialogResult.OK; }
        }

        public void Clear()
        {
            errorProvider.Clear();
            cmbAccount.Text = "";
            cmbAccount.SelectedIndex = -1;
            cmbCategory.Text = "";
            cmbCategory.SelectedIndex = -1;
            cmbAccount.Text = "";
            cmbAccount.SelectedIndex = -1;
            txtReference.Clear();
        }
    }
}
