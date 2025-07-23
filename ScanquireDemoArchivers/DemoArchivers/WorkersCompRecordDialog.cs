using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DemoArchivers
{
    public partial class WorkersCompRecordDialog : Form
    {

        public string ClientName
        {
            get { return txtClientName.Text; }
            set { txtClientName.Text = value; }
        }

        public string CaseNumber
        {
            get { return txtCaseNumber.Text; }
            set { txtCaseNumber.Text = value; }
        }

        public string FormType
        {
            get { return (string)cmbFormType.SelectedItem; }
            set { cmbFormType.SelectedText = value; }
        }

        public DateTime? FormDate
        {
            get { return dpFormDate.Value; }
            set { dpFormDate.Value = value; }
        }


        private void SubmitButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        public void Clear()
        {
            //txtClientName.Clear();
            //txtCaseNumber.Clear();
            cmbFormType.SelectedIndex = -1;
            dpFormDate.Clear();

            cmbFormType.Focus();
        }

        public WorkersCompRecordDialog()
        {
            InitializeComponent();
            txtClientName.Focus();
        }

        private void SubmitButton_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CaseNumber))
                { throw new Exception("Case Number is required"); }

                if (string.IsNullOrEmpty(ClientName))
                { throw new Exception("Client Name is required"); }

                if (string.IsNullOrEmpty(FormType))
                { throw new Exception("Form Type is required"); }

                if (FormDate.HasValue == false)
                { throw new Exception("Form Date is required"); }

                base.DialogResult = DialogResult.OK;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
