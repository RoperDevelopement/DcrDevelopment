using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EdocsUSA.Controls;
using EdocsUSA.Utilities.Logging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Edocs.BSB.Planning.Dep.Archiver
{
    public partial class BSBPlanDepArchiverDialog : Form
    {
        int x = 0;
        int y = 0;

        public bool ShowTotalDocsScanned
        { get; set; }
        public bool IncludeBlankDocs
        { get; set; }
        public string ImageFolder
        { get; set; }
        public BSBPlanDepArchiverDialog()
        {
            InitializeComponent();
        }


        public string PermitNum
        {
            get { return txtBoxPerNumber.Text; }
            set { txtBoxPerNumber.Text = value; }
        }
        public string ParcelNum
        {
            get { return txtBoxParNumber.Text; }
            set { txtBoxParNumber.Text = value; }
        }
        public string ExePermitNumber
        { get { return txtBoxExcNumber.Text; }
            set { txtBoxExcNumber.Text = value; }
        }
        public string ZoneNumber
        {
            get { return txtBoxZoneNumber.Text; }
            set { value = txtBoxZoneNumber.Text; }
        }
        public string ZoneNum
        {
            get; set;
        }
        public string PNumber
        {
            get; set;
        }
        public string GCode
        {
            get; set;
        }
        public string GoCode
        {
            get { return txtBoxGoCode.Text; }
            set { value = txtBoxGoCode.Text; }
        }

        public string OwnerLot
        {
            get { return txtBoxOwner.Text; }
            set { value = txtBoxOwner.Text; }
        }
        public string CoAddress
        { get; set; }
        public string CopOwner
        { get; set; }
        public string Constructionco
        { get; set; }
        public string Address
        {
            get { return txtBoxAddress.Text; }
            set { value = txtBoxAddress.Text; }
        }
        public string AddressComp
        {
            get; set;
        }
        public string TextLCROCR
        { get; set; }
        public string LCROCRText
        {
            get { return rTextBox.Text; }
            set { value = rTextBox.Text; }
        }
        public DateTime DateIssue
        {
            get { return dateIssued.Value; }
            set { value = dateIssued.Value; }
        }
        public DateTime DocIssueDate
        {
            get; set;

        }
        public DateTime DateExp
        {
            get { return dateExp.Value; }
            set { value = dateExp.Value; }
        }
        public string ConstCo
        {
            get { return txtBoxConstCo.Text; }
            set { value = txtBoxConstCo.Text; }
        }
        public string TotalScanned
        {
            get { return txtBoxTotalScanned.Text; }
            set { value = txtBoxTotalScanned.Text; }
        }


        private void btnOk_Click(object sender, EventArgs e)
        {

            //    if (!(int.TryParse(txtBoxPerNumber.Text, out int resultsPer)))
            //{
            //    MessageBox.Show("Permit Number required");
            //    return;
            //}
            if (!(int.TryParse(txtBoxParNumber.Text, out int results)))
            {
                txtBoxParNumber.Text = "0";
            }
            if (!(int.TryParse(txtBoxZoneNumber.Text, out int resultsZone)))
            {
                txtBoxZoneNumber.Text = "0";
            }

            if (!(int.TryParse(txtBoxExcNumber.Text, out int resultsExe)))
            {
                txtBoxExcNumber.Text = "0";
            }
            this.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

        }
        private void SetScrollBars()
        {
            hScrollBar1.Width = pBox.Width;
            hScrollBar1.Left = pBox.Left;
            hScrollBar1.Top = pBox.Bottom;
            hScrollBar1.Maximum = pBox.Image.Width - pBox.Width;
            vScrollBar1.Height = pBox.Height;
            vScrollBar1.Left = pBox.Left + pBox.Width;
            vScrollBar1.Top = pBox.Top;
            vScrollBar1.Maximum = pBox.Image.Height - pBox.Height;
        }
   
    private void BSBPlanDepArchiverDialog_Shown(object sender, EventArgs e)
    {
          txtBoxPerNumber.Text = Guid.NewGuid().ToString("N").Substring(0, 16); 
            txtBoxPerNumber.Focus();
        txtBoxAddress.Text = Guid.NewGuid().ToString("N").Substring(0, 16); ;
            dateIssued.Value = DateTime.Now;
        dateExp.Value = dateExp.Value.AddYears(1);
        txtBoxOwner.Text = Guid.NewGuid().ToString("N").Substring(0, 16);
            txtBoxConstCo.Text = Guid.NewGuid().ToString("N").Substring(0, 16);
            txtBoxExcNumber.Text = Guid.NewGuid().ToString("N").Substring(0, 16);
            txtBoxZoneNumber.Text = Guid.NewGuid().ToString("N").Substring(0, 16);
            txtBoxGoCode.Text = Guid.NewGuid().ToString("N").Substring(0, 16);
            txtBoxParNumber.Text = Guid.NewGuid().ToString("N").Substring(0, 16);
            txtBoxTotalScanned.Text = "1";//TraceLogger.TraceLoggerInstance.GetTotalImagesScannedString(IncludeBlankDocs);

        pBox.Image = System.Drawing.Image.FromFile(ImageFolder);
            pBox.SizeMode = PictureBoxSizeMode.Normal;

        SetScrollBars();
        if (!(string.IsNullOrWhiteSpace(TextLCROCR)))
        {
            rTextBox.Text = TextLCROCR;
        }
        else
            rTextBox.Text = "No Text Found";
            System.Threading.Thread.Sleep(2000);
            btnOk.PerformClick();
    }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            y = (sender as VScrollBar).Value;
            pBox.Refresh();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            x = (sender as HScrollBar).Value;
            pBox.Refresh();
        }

        private void pBox_Paint(object sender, PaintEventArgs e)
        {
            pBox = sender as PictureBox;
            e.Graphics.DrawImage(pBox.Image, e.ClipRectangle, x, y, e.ClipRectangle.Width,
              e.ClipRectangle.Height, GraphicsUnit.Pixel);
        }

        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pBox.SizeMode = PictureBoxSizeMode.Normal;
            pBox.Refresh();
        }

        private void zoomImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pBox.SizeMode = PictureBoxSizeMode.Zoom;
            pBox.Refresh();
        }

        private void autoSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pBox.SizeMode = PictureBoxSizeMode.AutoSize;
            pBox.Refresh();
        }

        private void centerImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pBox.SizeMode = PictureBoxSizeMode.CenterImage;
            pBox.Refresh();
        }

        private void strechImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pBox.Refresh();
        }
    }
    }
 
 
