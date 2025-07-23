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
    public partial class StudentRecordRecordDialog : Form
    {
        public string LastName
        {
            get { return txtLastName.Text; }
            set { txtLastName.Text = value; }
        }

        public string FirstName
        {
            get { return txtFirstName.Text; }
            set { txtFirstName.Text = value; }
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

        public StudentRecordRecordDialog()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        public void Clear()
        {
            txtLastName.Clear();
            txtFirstName.Clear();
            cmbFormType.SelectedIndex = -1;
            dpFormDate.Clear();
        }
    }
}
