//icons from <a href="https://www.flaticon.com/free-icons/exit" title="exit icons">Exit icons created by Freepik - Flaticon</a>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using Microsoft;
using System.Diagnostics;
using Tesseract;
using System.IO;
using System.Configuration;
using System.Reflection;
using Scanquire.Public.Extensions;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using ETL = EdocsUSA.Utilities.Logging;
using EdocsUSA.Utilities.Controls;
using Scanquire.Public.UserControls;
using System.Text.RegularExpressions;
using static Scanquire.Public.EmailImagesForm;

namespace Scanquire.Public
{
    public partial class EmailAttachmentsForm : Form
    {
        public EmaiImageTypes ImageType
        { get; set; }
        public SQImageListViewerItem[] EmailImages
        { get; set; }
        public List<string> EmailAttachmentName
        { get; set; }
        private int CurrentImage
        { get; set; }
        private string ImageFolder
        { get; set; }
        public double EmailAttSize
        { get; set; }
        public EmailAttachmentsForm()
        {
            InitializeComponent();
            CurrentImage = 0;
            EmailAttachmentName = new List<string>();
            EmailAttSize = 0.0;
        }
        private async void GetCurrentImage()
        {

            for (int i = CurrentImage; i < EmailImages.Count(); i++)
            {
                if (EmailImages[i].Selected)
                {

                    SQImage image = EmailImages[i].Value;
                    SQImageEditLock image_Lock = image.BeginEdit();
                    PicBoxImage.Image = (Bitmap)image.WorkingCopy;
                    //    Edocs_Utilities.EdocsUtilitiesInstance.PrintBitMap = (Bitmap)image.WorkingCopy;
                    image.DiscardEdit(image_Lock);
                    CurrentImage = i;

                    break;
                }
                CurrentImage = int.MaxValue;
            }
        }

        private void EmailAttachmentsForm_Shown(object sender, EventArgs e)
        {
            ImageFolder = Path.Combine(SettingsManager.TempDirectoryPath, "EmailImageAtt");
            //   Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(imageFolder);
            Edocs_Utilities.EdocsUtilitiesInstance.DelFolder(ImageFolder, true);
            Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(ImageFolder);
            GetCurrentImage();
            TxtBoxAttachmentName.Focus();
        }

        private async Task PDFAttchement(string pdfSaveName)
        {
            SQDocument qDocument = new SQDocument();
            List<SQPage> sQPages = new List<SQPage>();
            CurrentImage = 0;
            for (int i = CurrentImage; i < EmailImages.Count(); i++)
            {
                if (EmailImages[i].Selected)
                {

                    SQImage image = EmailImages[i].Value;
                    SQImageEditLock image_Lock = image.BeginEdit();
                    SQPage page = new SQPage(image);
                    //    Edocs_Utilities.EdocsUtilitiesInstance.PrintBitMap = (Bitmap)image.WorkingCopy;
                    image.DiscardEdit(image_Lock);
                    CurrentImage = i;
                    qDocument.Pages.Add(page);
                   
                }
              
            }
            CancellationTokenSource cTokenSource = new CancellationTokenSource();
            CancellationToken cToken = cTokenSource.Token;
            Progress<ProgressEventArgs> progress = new Progress<ProgressEventArgs>();
            //ProgressMonitor.StartMonitoring(progress, cTokenSource);
            //ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessRunningAutoQa);
            //ProgressMonitor.StartMonitoring(progress, cTokenSource);
            SQFileWriter_Pdf sQFileWriter_Pdf = new SQFileWriter_Pdf();

            SQFile sQFile = await sQFileWriter_Pdf.Write(qDocument, progress, cToken);
            File.WriteAllBytes(pdfSaveName, sQFile.Data);
            
             CurrentImage = int.MaxValue;

        }
        private async void BtnOk_Click(object sender, EventArgs e)
        {
            if (!(string.IsNullOrWhiteSpace(TxtBoxAttachmentName.Text)))
            {
                await SaveImage();
                GetCurrentImage();
                if (CurrentImage >= EmailImages.Count())
                {

                    this.Close();
                }
            }
            else
                MessageBox.Show("Need an attachment name", "Error Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async Task SaveImage()
        {
            //  SQImage image = EmailImages[CurrentImage].Value;
            // SQImageEditLock image_Lock = image.BeginEdit();
            // Image img= (Bitmap)image.WorkingCopy;
            //  image.DiscardEdit(image_Lock);
            CurrentImage++;
            string imgSaveName = string.Empty;
            switch (ImageType)
            {
                case EmaiImageTypes.BMP:
                    imgSaveName = Path.Combine(ImageFolder, $"{TxtBoxAttachmentName.Text}.bmp");

                    PicBoxImage.Image.Save(imgSaveName, System.Drawing.Imaging.ImageFormat.Bmp);
                    //   img.Save(imgBmp, System.Drawing.Imaging.ImageFormat.Bmp);
                    //  img.Dispose();
                    break;
                case EmaiImageTypes.GIF:
                    imgSaveName = Path.Combine(ImageFolder, $"{TxtBoxAttachmentName.Text}.gif");

                    PicBoxImage.Image.Save(imgSaveName, System.Drawing.Imaging.ImageFormat.Gif);
                    //   img.Save(imgBmp, System.Drawing.Imaging.ImageFormat.Bmp);
                    //  img.Dispose();
                    break;
                case EmaiImageTypes.JPG:
                    imgSaveName = Path.Combine(ImageFolder, $"{TxtBoxAttachmentName.Text}.jpg");

                    PicBoxImage.Image.Save(imgSaveName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //   img.Save(imgBmp, System.Drawing.Imaging.ImageFormat.Bmp);
                    //  img.Dispose();
                    break;

                case EmaiImageTypes.PNG:
                    imgSaveName = Path.Combine(ImageFolder, $"{TxtBoxAttachmentName.Text}.png");

                    PicBoxImage.Image.Save(imgSaveName, System.Drawing.Imaging.ImageFormat.Png);
                    //   img.Save(imgBmp, System.Drawing.Imaging.ImageFormat.Bmp);
                    //  img.Dispose();
                    break;
                case EmaiImageTypes.TIF:
                    imgSaveName = Path.Combine(ImageFolder, $"{TxtBoxAttachmentName.Text}.tif");

                    PicBoxImage.Image.Save(imgSaveName, System.Drawing.Imaging.ImageFormat.Tiff);
                    //   img.Save(imgBmp, System.Drawing.Imaging.ImageFormat.Bmp);
                    //  img.Dispose();
                    break;
                case EmaiImageTypes.PDF:
                    CurrentImage = int.MaxValue;
                    imgSaveName = Path.Combine(ImageFolder, $"{TxtBoxAttachmentName.Text}.pdf");
                    EmailAttachmentName.Add(imgSaveName);
                    await PDFAttchement(imgSaveName);
                   
                    // imgSaveName = Path.Combine(ImageFolder, $"{TxtBoxAttachmentName.Text}.tif");
                    // EmailAttachmentName.Add(imgSaveName);
                    // PicBoxImage.Image.Save(imgSaveName, System.Drawing.Imaging.ImageFormat.Tiff);
                    //   img.Save(imgBmp, System.Drawing.Imaging.ImageFormat.Bmp);
                    //  img.Dispose();
                    break;
            }
          if(ImageType != EmaiImageTypes.PDF)
            { 
            EmailAttSize = EmailAttSize + Edocs_Utilities.EdocsUtilitiesInstance.FileSize(imgSaveName);
            if (EmailAttSize < 10f)
                EmailAttachmentName.Add(imgSaveName);
            else
            {
                MessageBox.Show($"Email Attachments has to be less then 10 MB. Attachment size {EmailAttSize.ToString()}MB", "Error AttachmentSize", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentImage = int.MaxValue;
            }
            TxtBoxAttachmentName.Text = string.Empty;
            TxtBoxAttachmentName.Focus();
            }
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
