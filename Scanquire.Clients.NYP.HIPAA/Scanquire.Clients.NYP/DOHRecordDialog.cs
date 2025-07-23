using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Scanquire.Public;
using Scanquire.Public.UserControls;
using Scanquire.Public.ArchivesConstants;
using Scanquire.Public.Extensions;
using System.Windows.Forms;
using EDL = EdocsUSA.Utilities.Logging;
namespace Scanquire.Clients.NYP
{
    public partial class DOHRecordDialog : Form
    {

        public SQImageListViewer DOHImage
        {
            get { return DOHImageViewer; }
            set { DOHImageViewer = value; }
        }
        //public Image CurrentImage
        //{
        //    get { return imageCurrent.Image; }
        //    set
        //    {
        //        if (imageCurrent.Image != null)
        //        { imageCurrent.Image.Dispose(); }
        //        imageCurrent.Image = value;
        //    }
        //}

        public string AccessionNumber
        {
            get { return txtAccessionNumber.Text; }
            set { txtAccessionNumber.Text = value; }
        }

        public string PrevAccessionNumber
        {
            get { return TxtBoxPrevAccNum.Text; }
            set { TxtBoxPrevAccNum.Text = value; }
        }
        public string PrevMRN
        {
            get { return TxtBoxPrevMRN.Text; }
            set { TxtBoxPrevMRN.Text = value; }
        }
        public string MedicalRecordNumber
        {
            get { return txtMedicalRecordNumber.Text; }
            set { txtMedicalRecordNumber.Text = value; }
        }

        public DateTime? DateOfService
        {
            get { return this.dpDateOfService.Value; }
            set { this.dpDateOfService.Value = value; }
        }


        public DOHRecordDialog()
        {
            InitializeComponent();
            DOHImageViewer.SplitContainer.SplitterDistance = 90;
            DOHImageViewer.ThumbnailToolStrip.Visible = false;
            //EdocsUSA.Controls.ImageViewer imageViewer = new EdocsUSA.Controls.ImageViewer();
            //imageViewer.ScaleToFitHeight();
            




        }





        public void Clear()
        {
            dpDateOfService.Value = DateTime.Now;
            errorProvider.Clear();
            AccessionNumber = null;
            MedicalRecordNumber = null;
         
        }

        
   


        private void DOHRecordDialog_Shown(object sender, EventArgs e)
        {
            // imageCurrent.SizeMode = PictureBoxSizeMode.StretchImage;
            //radioButton1.Checked = true;
            
           // imageCurrent.Refresh();
            txtAccessionNumber.Focus();
       //     DOHImageViewer.ActiveImageViewer.ScaleToFitHeight();
         //   DOHImageViewer.ActiveImageViewer.Refresh();


        }

        

        private void DOHRecordDialog_KeyPress(object sender, KeyPressEventArgs e)
        {
           if(e.KeyChar== (char)Keys.Return)
            {
                e.Handled = true;
                btnOk.PerformClick();
            }
           else if (e.KeyChar == (char)Keys.F2)
            {
               
                txtAccessionNumber.Text = TxtBoxPrevAccNum.Text;
                e.Handled = true;
            }
           else if (e.KeyChar == (char)Keys.F3)
            {
                
                txtMedicalRecordNumber.Text = TxtBoxPrevMRN.Text;
                e.Handled = true;
            }
           else
            e.Handled = false;
        }

       

        private void DOHRecordDialog_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    e.Handled = true;
                    btnOk.PerformClick();
                    break;
                case Keys.F3:
                    txtAccessionNumber.Text = TxtBoxPrevAccNum.Text;
                    e.Handled = true;
                    break;
                
                case Keys.F4:
                    txtMedicalRecordNumber.Text = TxtBoxPrevMRN.Text;
                    e.Handled = true;
                    break;

                case Keys.F12:
                    btnCancel.PerformClick();
                    e.Handled = true;
                    break;

                default:
                    e.Handled = false;
                    break;


                    //case Keys.ControlKey:
                    //    rbWC.PerformClick();
                    //    e.Handled = true;
                    //    break;



            }

        }

      

        //private void rBtnNormal_CheckedChanged(object sender, EventArgs e)
        //{

        //    //Bitmap b = new Bitmap(imageCurrent.Image, imageCurrent.Image.Width, imageCurrent.Image.Height / 2);
        //    //imageCurrent.Image = b;
        //   imageCurrent.SizeMode = PictureBoxSizeMode.Normal;

        //}

        //private void rBtnCenter_CheckedChanged(object sender, EventArgs e)
        //{
        //    imageCurrent.SizeMode = PictureBoxSizeMode.CenterImage;
            
        //}

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool errors = false;
            errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(this.AccessionNumber))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("DOH archiver AccessionNumber Required");
                errorProvider.SetError(txtAccessionNumber, "Required");
                errors = true;
            }
            if (string.IsNullOrWhiteSpace(this.MedicalRecordNumber))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("DOH archiver MedicalRecordNumber Required");
                errorProvider.SetError(txtMedicalRecordNumber, "Required");
                errors = true;
            }
            if (dpDateOfService.HasValue == false)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("DOH archiver DateOfService Required");
                errorProvider.SetError(dpDateOfService, "Required");
                errors = true;
            }

            if (errors == false)
            { this.DialogResult = DialogResult.OK; }
        }

        private void BTNUsePrevAccNum_Click(object sender, EventArgs e)
        {
            txtAccessionNumber.Text = TxtBoxPrevAccNum.Text;
        }

        private void BtnUsePrevMRN_Click(object sender, EventArgs e)
        {
            txtMedicalRecordNumber.Text = TxtBoxPrevMRN.Text;
        }
    }
}
