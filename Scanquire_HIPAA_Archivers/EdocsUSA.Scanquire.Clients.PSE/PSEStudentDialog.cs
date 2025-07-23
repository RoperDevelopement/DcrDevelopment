using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EdocsUSA.Scanquire.Clients.PSE
{
    public partial class PSEStudentDialog : Form
    {
        public PSEStudentDialog()
        {
            InitializeComponent();
        }

        public string StudentFName
        {
           get { return txtboxStFName.Text; }
            set { txtboxStFName.Text = value; }
        }
        public string StudentLName
        {
            get { return txtBoxStLName.Text; }
            set { txtBoxStLName.Text = value; }
        }
        public DateTime StudentDOB
        {
            get { return dtpStudentDOB.Value; }
            set { dtpStudentDOB.Value = value; }
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
