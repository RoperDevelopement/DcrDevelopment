using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using   EdocsUSA.Utilities.Logging;
namespace Edocs.ITS.Archiver
{
    public partial class ITSArchiverDialog : Form
    {
        public ITSArchiverDialog()
        {
            InitializeComponent();
        }
        public bool AddTitle
        { get { return chkBoxAddTitle.Checked; }
            set { chkBoxAddTitle.Checked = value; }
        }
        public bool IncludeBlank
        { get; set; }
        public string TrackingID
        { get; set; }
        public bool ShowTotalRecordsScanned
        { get; set; }
        public string TotalRecordsScanned
        { get; set; }
        public string TxtDialog
        { get; set; }
        public string[] CmbBoxItems
        { get; set; }
        
        public string ArchiveDate
        {
            get { return txtBoxDate.Text; }
            set { txtBoxDate.Text = value; }
        }
        public string Title
        { get { return txtBoxTitle.Text; } 
        set {  txtBoxTitle.Text = value;}
}
        public string Description
        {
            get { return tichTxtBoxDesc.Text; }
            set { tichTxtBoxDesc.Text = value; }
        }
        public string Collection
        {
            get { return cmbBoxIdentifer.Text; }
            set { cmbBoxIdentifer.Text = value; }
        }
        private void txtBoxTrackingID_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)(Keys.Return):
                  //  btnOk.PerformClick();
                    e.Handled = false;
                    break;
                case '+':
                    //btnNavigateNext.PerformClick();
                    e.Handled = true;
                    break;
                case '-':
                   // btnNavigatePrevious.PerformClick();
                    e.Handled = true;
                    break;
                case '*':
                   // btnNavigateReset.PerformClick();
                    e.Handled = true;
                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }

        private void ITSArchiverDialog_Shown(object sender, EventArgs e)
        {
            
            this.Text = TxtDialog;
            if (!(ShowTotalRecordsScanned))
                txtTRec.Visible = false;
            if(cmbBoxIdentifer.Items.Count == 0)
            {
                cmbBoxIdentifer.BeginUpdate();
                cmbBoxIdentifer.Items.Clear();
                foreach (var item in CmbBoxItems)
                    cmbBoxIdentifer.Items.Add(item);
                cmbBoxIdentifer.EndUpdate();
            }
            txtTRec.Text = TraceLogger.TraceLoggerInstance.GetTotalImagesScannedString(IncludeBlank) ;
        }
        


        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtBoxTrackingID.Text))
            {
                MessageBox.Show("Tracking ID Required", "Tracking ID:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtBoxTrackingID.Text.Length < 6)
                MessageBox.Show($"Invalid Tracking ID {txtBoxTrackingID.Text}", "Tracking ID:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                TrackingID = txtBoxTrackingID.Text;
                if (string.IsNullOrWhiteSpace(txtTRec.Text))
                {
                    TotalRecordsScanned  = "0";
                }
                else
                {
                    if (int.TryParse(txtTRec.Text, out int results))
                    {
                        TotalRecordsScanned = results.ToString();;
                        
                    }
                    else
                    {
                        MessageBox.Show($"Invalid Total Records Scanned {txtTRec.Text}", "Records Scanned:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                        
                }

                    this.DialogResult = DialogResult.OK;
            }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
