using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EdocsUsa.Scanquire.Clients.Navajo.PD
{
    public partial class NavajoPDTrafficHistoryArchiverDialog : Form
    {
        public string BarCode
        { 
            get { return txtBoxBarCode.Text; }
            set { txtBoxBarCode.Text = value; }
        }
        public bool ShowBarCode
        {
            get;set;
        }
        public string SSN
        {
            get { return mTxtBoxSSnumber.Text; }
            set { mTxtBoxSSnumber.Text = value; }
        }
        public string Fname
        {
            get { return txtBoxFN.Text; }
            set { txtBoxFN.Text = value; }
        }
        public string Lname
        {
            get { return txtBoxLN.Text; }
            set { txtBoxLN.Text = value; }
        }
        public string NACN
        {
            get { return txtBoxNACN.Text; }
            set { txtBoxNACN.Text = value; }
        }
        
        public NavajoPDTrafficHistoryArchiverDialog()
        {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void NavajoPDTrafficHistoryArchiverDialog_Shown(object sender, EventArgs e)
        {
            labBarCode.Visible = ShowBarCode;
            txtBoxBarCode.Visible = ShowBarCode;
        }

        

        private void labBarCode_Click(object sender, EventArgs e)
        {

        }

        private void txtBoxBarCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBoxFN_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtBoxLN_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBoxNACN_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void mTxtBoxSSnumber_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
