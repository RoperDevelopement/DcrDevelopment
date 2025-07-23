using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Edocs.ITS.Archiver
{
    public partial class ITSArchiverDialog : Form
    {
        public ITSArchiverDialog()
        {
            InitializeComponent();
        }
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
        public DateTime StYear
        {
            get { return dtpSFinYear.Value; }
            set { dtpSFinYear.Value = value; }
        }
        public DateTime EndYear
        {
            get { return dtpEFinYear.Value; }
            set { dtpEFinYear.Value = value; }
        }
        public string FinIdentifier
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
