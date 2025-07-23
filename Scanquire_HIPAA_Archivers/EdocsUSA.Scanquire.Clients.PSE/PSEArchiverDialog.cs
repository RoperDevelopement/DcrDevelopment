using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EdocsUSA.Scanquire.Clients.PSE
{
    public partial class PSEArchiverDialog : Form
    {
        public PSEArchiverDialog()
        {
            InitializeComponent();
            
            TotalRecordsScanned = "0";
            ShowTotalRecordsScanned = false;
            
        }
        public string ArchiveName
        { get; set; }
        public bool ShowTotalRecordsScanned
        { get; set; }
        public string TotalRecordsScanned
        { get; set; } 
        public string TxtDialog
        { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            ArchiveName = "StudentRec";
            if(rbFinRec.Checked)
            {
                ArchiveName = "FinRecords";
            
            }
            this.DialogResult = DialogResult.OK; 
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void PSEArchiverDialog_Shown(object sender, EventArgs e)
        {
            
                labTRecScanned.Visible = ShowTotalRecordsScanned;
            txtTRec.Visible = ShowTotalRecordsScanned;
            txtTRec.Text = "0";
            this.Text = TxtDialog;
        }

        private void txtTRec_TextChanged(object sender, EventArgs e)
        {

            TotalRecordsScanned = txtTRec.Text;
        }
    }
}
