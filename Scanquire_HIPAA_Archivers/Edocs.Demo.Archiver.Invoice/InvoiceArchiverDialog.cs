using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Edocs.Demo.Archiver.Invoice
{
    public partial class InvoiceArchiverDialog : Form
    {
        public InvoiceArchiverDialog()
        {
            InitializeComponent();
        }
        public DateTime  InvoiceDateDate
        {
            get { return this.dtPickerInvDate.Value; }
            set { this.dtPickerInvDate.Value = value; }
        }
        public string InvoiceCustomerNumber
        {
            get { return this.txtBoxInvCusNum.Text; }
            set { this.txtBoxInvCusNum.Text = value; }
        }
        public string InvoicePONumber
        {
            get { return this.txtBoxPONum.Text; }
            set { this.txtBoxPONum.Text = value; }
        }
        public string InvoiceNumber
        {
            get { return this.txtBoxInvNum.Text; }
            set { this.txtBoxInvNum.Text = value; }
        }
        public string InvoiceTotal
        {
            get { return this.txtBoxInvTotal.Text; }
            set { this.txtBoxInvTotal.Text = value; }
        }

        public DateTime InvoiceDueDate
        {
            get { return this.dtPickerInvDueDate.Value; }
            set { this.dtPickerInvDueDate.Value = value; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void dtPickerInvDate_Leave(object sender, EventArgs e)
        {
            dtPickerInvDueDate.Value = dtPickerInvDate.Value.AddDays(30);
        }

        private void txtBoxInvTotal_TextChanged(object sender, EventArgs e)
        {
            
             if (!(string.IsNullOrWhiteSpace(txtBoxInvTotal.Text)))
            {
                //string s = txtBoxInvTotal.Text.Substring(txtBoxInvTotal.Text.Length - 1);
                char c = char.Parse(txtBoxInvTotal.Text.Substring(txtBoxInvTotal.Text.Length-1,1));
                if(c != '.')
                {
                    if(!(char.IsDigit(c)))
                    {
                        errorProvider1.SetError(txtBoxInvTotal, $"Invalid Digit {txtBoxInvTotal.Text}");
                      //  txtBoxInvTotal.Text = txtBoxInvTotal.Text.Remove(txtBoxInvTotal.Text.Length - 1);
                      //  txtBoxInvTotal.Focus();
                    }
                     else
                         errorProvider1.Clear();
                }
                else
                    errorProvider1.Clear();
            }
            //else
            //   errorProvider1.Clear();
        }

        private void InvoiceArchiverDialog_Load(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }
    }
}
