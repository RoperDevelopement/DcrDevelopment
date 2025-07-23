using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Edocs.WorkFlow.Archiver.ArchiverForms
{
    public partial class DialogArchiver : Form
    {
        public DateTime? EmpStDate
        {
            get { return empStDate.Value; }
            set { this.empStDate.Value = value; }
        }
        public string EmpFName
        {
            get { return txtBoxEFirstName.Text; }
            set { txtBoxEFirstName.Text = value; }
        }
        public string EmpLName
        {
            get { return txtBoxELastName.Text; }
            set { txtBoxELastName.Text = value; }
        }
        public string EmpAddress
        {
            get { return txtBoxEAddress.Text; }
            set { txtBoxEAddress.Text = value; }
        }
        public string EmpCity
        {
            get { return txtBoxECity.Text; }
            set { txtBoxECity.Text = value; }
        }

       public string MidName
        {
            get { return txtBoxMiddleName.Text; }
            set { txtBoxMiddleName.Text = value; }
            
        }
        public bool USCit
        {
            get;
            set;
            
        }
        public bool ConvFel
        {
            get;
            set;

        }
        public bool DrugTest
        {
            get;
            set;

        }
        public string EmpState
        {
            get { return cmbBoxEState.Text; }
            set { cmbBoxEState.Text = value; }
        }
        public string EmpZip
        {
            get { return txtBoxEZip.Text; }
            set { txtBoxEZip.Text = value; }
        }
        public string PosApply
        {
            get { return txtBoxPosApply.Text; }
            set { txtBoxPosApply.Text = value; }
        }
        public string EmpHomePhone
        {
            get { return TxtBoxHomePhone.Text; }
            set { TxtBoxHomePhone.Text = value; }
        }
        public string EmpCellPhone
        {
            get { return txtBoxCellNumber.Text; }
            set { txtBoxCellNumber.Text = value; }
        }
        public string EmpPay
        {
            get { return txtBoxEPay.Text; }
            set { txtBoxEPay.Text = value; }
        }
      
        public string EmpSSN
        {
            get { return txtBoxEmpSSN.Text; }
            set { txtBoxEmpSSN.Text = value; }
        }
        //public string EmpID
        //{
        //    get { return txtBoxEmpID.Text; }
        //    set { txtBoxEmpID.Text = value; }
        //}
        public string Comments
        {
            get { return txtBoxComments.Text; }
            set { txtBoxComments.Text = value; }
        }
        public string EmpEmail        {
            get { return txtBoxEEmail.Text; }
            set { txtBoxEEmail.Text = value; }
        }
        public DialogArchiver()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void checkBoxes_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox senderChkbx = (CheckBox)sender;
            switch (senderChkbx.Name)
            {

            }
        }

        private void chkBoxYes_Click(object sender, EventArgs e)
        {
             
            chkBoxNo.Checked = false;
            if (chkBoxYes.Checked)
                USCit = true;
            else
                USCit = false;
        }

        private void chkBoxNo_Click(object sender, EventArgs e)
        {
            chkBoxYes.Checked = false;
            if (chkBoxNo.Checked)
                USCit = false;
            else
                USCit = false;
        }

        private void chkBoxFelYes_CheckedChanged(object sender, EventArgs e)
        {
            chkBoxFelNo.Checked = false;
            if (chkBoxFelYes.Checked)
                ConvFel = true;
            else
                ConvFel = false;
        }

        private void chkBoxFelNo_Click(object sender, EventArgs e)
        {
            chkBoxFelYes.Checked = false;
            
                ConvFel = false;
            
        }

        private void chkBoxDrugYes_Click(object sender, EventArgs e)
        {
            chkBoxDrugNo.Checked = false;
            if (chkBoxDrugYes.Checked)
                DrugTest = true;
            else
                DrugTest = false;
        }

        private void chkBoxDrugNo_Click(object sender, EventArgs e)
        {
            chkBoxDrugYes.Checked = false;
            if (chkBoxDrugNo.Checked)
                DrugTest = false;
            else
                DrugTest = true;
        }
    }
}
