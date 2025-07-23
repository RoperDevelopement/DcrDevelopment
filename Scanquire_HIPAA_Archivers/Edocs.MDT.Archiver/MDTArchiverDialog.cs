using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EdocsUSA.Utilities.Logging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Edocs.MDT.Archiver
{
    public partial class MDTArchiverDialog : Form
    {
        public string ProjectNameNums
        { get; set; }
        public bool ShowTotalDocsScanned
        { get; set; }
        public bool IncludeBlankDocs
        { get; set; }
        public string DocType
        { get; set; }
        public MDTArchiverDialog()
        {
            InitializeComponent();
        }

        public string Project
        {
            get;
            set;
        }
        public string CmboxNumbers
        { get; set; }
        public bool ShowConfirmDialogBox
        { get; set; }
        public string BoxNumber
        {
            get { return cBoxBoxNumber.Text; }
            set { value = cBoxBoxNumber.Text; }
        }
        public string TotalScanned
        {
            get { return txtBoxTotalScanned.Text; }
            set { value = txtBoxTotalScanned.Text; }
        }
        private void MDTArchiverDialog_Shown(object sender, EventArgs e)
        {
             cbDoc.Checked = false;
          //  cbEaseMent.Checked = false;
            //  comBoxProjectNameNum.DropDownStyle = ComboBoxStyle.DropDown;
            txtBoxTotalScanned.Text = string.Empty;
            LoadListBox();
            if (ShowTotalDocsScanned)
                txtBoxTotalScanned.Text = TraceLogger.TraceLoggerInstance.GetTotalImagesScannedString(IncludeBlankDocs);
            else
                txtBoxTotalScanned.Text = "0";

        }
        private void SetPNameNumber()
        {
            if (!(string.IsNullOrWhiteSpace(lBoxProjectName.Text)))
            {
                string[] pNameNum = lBoxProjectName.Text.Split(';');
                txtBoxProjectNumber.Text = pNameNum[1];
                txtBoxPName.Text = pNameNum[0];
            }
        }
        private void LoadListBox()
        {



            cbEaseMent.Checked = false;
            cbDoc.Checked = true;
            chkBoxDocEas.Checked=false;
            txtBoxPName.Text = string.Empty;
            txtBoxProjectNumber.Text = string.Empty;
            DocType = "Document";
            if (lBoxProjectName.Items.Count == 0)
            {
                lBoxProjectName.Items.Clear();
                lBoxProjectName.BeginUpdate();
                string pn = string.Format("{0}{1}", EdocsUSA.Utilities.SettingsManager.ApplicationSettingsDirectoryPath, ProjectNameNums);
                foreach (var str in System.IO.File.ReadLines(pn, Encoding.ASCII))
                {
                    if (!(string.IsNullOrWhiteSpace(str)))
                        lBoxProjectName.Items.Add(str.Trim().Replace(".","_"));
                }
                // System.IO.rea
                //foreach (var lItems in ProjectNameNums.Split('^'))
                //{
                //    if(!(string.IsNullOrWhiteSpace(lItems)))
                //    {

                //    }
                //}
                lBoxProjectName.EndUpdate();
            }
            if (cBoxBoxNumber.Items.Count == 0)
            {
                cBoxBoxNumber.BeginUpdate();
                foreach (string cbStr in CmboxNumbers.Split(';'))
                {
                    if (!(string.IsNullOrWhiteSpace(cbStr)))
                    {
                        cBoxBoxNumber.Items.Add(cbStr.Trim());
                    }
                }
                cBoxBoxNumber.EndUpdate();
            }
            lBoxProjectName.SelectedIndex = 0;
            SetPNameNumber();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cBoxBoxNumber.Text))
            {
                MessageBox.Show("Need Box Number", "Box Number", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!(string.IsNullOrWhiteSpace(txtBoxProjectNumber.Text)) && !((string.IsNullOrWhiteSpace(txtBoxPName.Text))))
            {
              
                    txtBoxPName.Text = txtBoxPName.Text.Replace("/","-").Replace("\\","-").Replace(".","_");
                    txtBoxProjectNumber.Text = txtBoxProjectNumber.Text.Replace("/", "-").Replace(".", "_");
                Project = $"{txtBoxPName.Text.Trim()};{txtBoxProjectNumber.Text.Trim()}";
            }
            else
            {

                //if (!(string.IsNullOrWhiteSpace(lBoxProjectName.Text)))
                //     Project = lBoxProjectName.Text;

                // if ((string.IsNullOrWhiteSpace(Project)))
                //  {
                //  if((string.IsNullOrWhiteSpace(txtBoxProjectNumber.Text)) || (string.IsNullOrWhiteSpace(txtBoxPName.Text)))
                //  { 
                MessageBox.Show("Need Project Number or Name", "Invalid Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                // }
                // Project = $"{txtBoxPName.Text.Trim()};{txtBoxProjectNumber.Text.Trim()}";
            }
            // }
            if (ShowConfirmDialogBox)
            {
                DialogResult dr = MessageBox.Show($"Are You sure this is an {DocType}", "Document or Easement", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == DialogResult.No)
                { return; }
            }
            if(!(string.IsNullOrWhiteSpace(txtBoxTotalScanned.Text)))
            {
                if(!(int.TryParse(txtBoxTotalScanned.Text,out int results)))
                {
                    MessageBox.Show($"Total Scanned Invalid {txtBoxTotalScanned.Text}", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            this.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }



        private void cbDoc_Click(object sender, EventArgs e)
        {
            DocType = "Document";
            if ((cbEaseMent.Checked) || (chkBoxDocEas.Checked))
            {
                cbDoc.Checked = true;
                cbEaseMent.Checked = false;
                chkBoxDocEas.Checked = false;

            }
            else
            {
                cbDoc.Checked = true;
            }
            //if(!(cbDoc.Checked))
            // {
            //     cbDoc.Checked = true;
            //     cbEaseMent.Checked = false;
            // }

        }

        private void cbEaseMent_Click(object sender, EventArgs e)
        {
            DocType = "Easement";
            if ((cbDoc.Checked) || (chkBoxDocEas.Checked))
            {
                cbDoc.Checked = false;
                cbEaseMent.Checked = true;
                chkBoxDocEas.Checked = false;
            }
            else
            {
                cbEaseMent.Checked = false;
            }

            //if(!(cbDoc.Checked))
            // {
            //     cbDoc.Checked = true;
            //     cbEaseMent.Checked = false;
            // }


        }

        private void lBoxProjectName_SelectedValueChanged(object sender, EventArgs e)
        {
            SetPNameNumber();
        }

        private void chkBoxDocEas_Click(object sender, EventArgs e)
        {
            DocType = "Document_Easement";
            if ((cbDoc.Checked) || (chkBoxDocEas.Checked))
            {
                cbDoc.Checked = false;
                cbEaseMent.Checked = false;
            }
            //else
            //{
            //    cbEaseMent.Checked = false;
            //}

        }
    }
}
