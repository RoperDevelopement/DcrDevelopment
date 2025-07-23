using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EdocsUSA.Utilities.Logging;
namespace Edocs.Columbia_River_Gorge.Archiver
{
    public partial class CRGAArchiverDialog : Form
    {
        public string FileName
        { get { return txtBoxFileNumber.Text; }
        set { txtBoxFileNumber.Text = value; }
        }
        public string ImagesScanned
        {
            get { return txtBoxPagesScanned.Text; }
            set { txtBoxPagesScanned.Text = value; }
        }
        public string ParcelNumLot
        {
            get { return txtBoxParcelTax.Text; }
            set { txtBoxParcelTax.Text = value; }
        }
        public CRGAArchiverDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if((string.IsNullOrWhiteSpace(txtBoxFileNumber.Text)) && (string.IsNullOrWhiteSpace(txtBoxParcelTax.Text)))
            {
                MessageBox.Show("Need File Number or Parcel Number Tax Lot", "Empty Strings", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                    return;
                
            }
            if (!(string.IsNullOrWhiteSpace(txtBoxFileNumber.Text)) && !(string.IsNullOrWhiteSpace(txtBoxParcelTax.Text)))
            {
                MessageBox.Show("Can only have File Number or Parcel Number Tax Lot", "Empty Strings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void CRGAArchiverDialog_Shown(object sender, EventArgs e)
        {
            txtBoxFileNumber.Text = string.Empty;
            txtBoxParcelTax.Text = string.Empty;
            txtBoxPagesScanned.Text = TraceLogger.TraceLoggerInstance.GetTotalImagesScannedString(false);
        }
    }
}
