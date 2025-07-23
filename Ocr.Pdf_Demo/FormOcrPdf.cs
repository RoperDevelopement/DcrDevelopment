using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using OP = Edocs.Pdf.Converter;
using System.IO;
using FreeImageAPI;
using static System.Net.WebRequestMethods;

namespace Edocs.Ocr.Pdf
{
    public partial class FormOcrPdf : Form
    {
        private int ProBarStep
        { get; set; }
        public Bitmap ScanedBitMap
        { get; set; }

        public bool? SaveCopy
        { get; set; }
        public FreeImageBitmap FreeImage
        { get; set; }
        public FormOcrPdf()
        {
            InitializeComponent();
            ScanedBitMap = null;
            panel1.Visible = false;
            panel1.Dock = DockStyle.None;
            pBoxImage.Visible = false;
            rTextBoxOcrPdf.Visible = false;
            pBoxImage.Dock = DockStyle.None;
            rTextBoxOcrPdf.Dock = DockStyle.None;
            userControlPB1.Visible = false;
            SaveCopy = null;

        }


        private void loadImgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (pBoxImage.Image != null)
                {
                    pBoxImage.Dispose();

                }
                //  Bitmap bitmap = new Bitmap(openFileDialog1.FileName);
                //pBoxImage.Image = bitmap;
                pBoxImage.Image = Image.FromFile(openFileDialog1.FileName);
                FreeImage = new FreeImageBitmap(openFileDialog1.FileName);


                // resize the image to match the image file

                panel1.Dock = DockStyle.Fill;
                pBoxImage.Size = pBoxImage.Image.Size;
                pBoxImage.Dock = DockStyle.Top;
                pBoxImage.Visible = true;
                panel1.Visible = true;
                pBoxImage.Invalidate();

            }
        }

        private void ocrImageToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void ConvertBitMapToPdf()
        {
            panel1.Visible = false;
            panel1.Dock = DockStyle.None;
            pBoxImage.Visible = false;
            pBoxImage.Dock = DockStyle.None;

            Bitmap bitmap = new Bitmap(pBoxImage.Image);
            Task createPdf = Task.Factory.StartNew(() =>

            {

                OP.Pdf.ConvertBitMapToPdf(FreeImage);



            });
            DisplayPBar(createPdf);
        }
        private void ConvertPdfToRtfFile()
        {
            panel1.Visible = false;
            panel1.Dock = DockStyle.None;
            pBoxImage.Visible = false;
            pBoxImage.Dock = DockStyle.None;


            Task createPdf = Task.Factory.StartNew(() =>

            {

                OP.Pdf.ConvertPdfToRtfFile(OP.Pdf.PDFExtension, OP.Pdf.RTFExtension);


            });
            DisplayPBar(createPdf);
        }
        private void LoadRichText()
        {

            rTextBoxOcrPdf.Dock = DockStyle.Fill;
            rTextBoxOcrPdf.Font = new Font("Arial", 8);
            rTextBoxOcrPdf.PerformLayout();

            rTextBoxOcrPdf.LoadFile($"{OP.Pdf.WorkingFolder}{OP.Pdf.GuidFname}{OP.Pdf.RTFExtension}", RichTextBoxStreamType.RichText);
            // rTextBoxOcrPdf.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(rTextBoxOcrPdf.Text))
            {
                rTextBoxOcrPdf.Font = new Font("Arial", 12);
                userControlPB1.pBar.Value = 90;
                string newText = string.Empty; ;
               // rTextBoxOcrPdf.Text = OP.Pdf.GetImageText(ScanedBitMap).ConfigureAwait(true).GetAwaiter().GetResult();
                foreach (string lines in rTextBoxOcrPdf.Lines)
                {
                    if (!(string.IsNullOrWhiteSpace(lines)))
                    {
                        newText = newText + lines + "\r\n";
                    }
                }
                rTextBoxOcrPdf.Text = newText.Trim();


            }
            if (!(string.IsNullOrWhiteSpace(rTextBoxOcrPdf.Text)))
            {
                rTextBoxOcrPdf.SelectAll();
                rTextBoxOcrPdf.SelectionAlignment = HorizontalAlignment.Left;
            }

            rTextBoxOcrPdf.Visible = true;
            rTextBoxOcrPdf.Update();

            pBoxImage.Invalidate();
        }
        private void EnableDisableTS(bool enabdis)
        {
            ocrImageToolStripMenuItem.Enabled = enabdis;
            saveToolStripMenuItem.Enabled = enabdis;
        }
        private void ocrImageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not Available in Demo Version Contact e-Docs USA https://edocsusa.com/contact-us/", "Not Available", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private void DisplayPBar(Task task)
        {
            if (ProBarStep > 90)
                ProBarStep = 50;


            userControlPB1.pBar.Value = ProBarStep;
            while (!(task.IsCompleted))
            {
                Application.DoEvents();
                Thread.Sleep(2000);
                ProBarStep = ProBarStep + 5;
                userControlPB1.pBar.Value = ProBarStep;
                if (ProBarStep > 90)
                    ProBarStep = 50;
                Application.DoEvents();
            }

        }
        private void SaveRichText()
        {


            if (System.IO.File.Exists($"{OP.Pdf.WorkingFolder}{OP.Pdf.GuidFname}{OP.Pdf.RTFExtension}"))
                System.IO.File.Delete($"{OP.Pdf.WorkingFolder}{OP.Pdf.GuidFname}{OP.Pdf.RTFExtension}");
            rTextBoxOcrPdf.SaveFile($"{OP.Pdf.WorkingFolder}{OP.Pdf.GuidFname}{OP.Pdf.RTFExtension}", RichTextBoxStreamType.RichText);
            rTextBoxOcrPdf.Visible = false;
            rTextBoxOcrPdf.Dock = DockStyle.None;
            pBoxImage.Invalidate();
        }
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            userControlPB1.label1.Text = "Saving Image";
            SaveCopy = false;
            SaveImage();
            this.Close();
        }
        private void SaveImage()
        {

            rTextBoxOcrPdf.Font = new Font("Arial", 8);
            ProBarStep = 5;
            userControlPB1.Visible = true;
            SaveRichText();
            ProBarStep = 50;

            ConvertRtfFiletoPdf();
            userControlPB1.Visible = false;

            FreeImage = new FreeImageBitmap($"{OP.Pdf.WorkingFolder}{OP.Pdf.GuidFname}1{OP.Pdf.TIFExtension}");
        }
        private void ConvertRtfFiletoPdf()
        {

            Task createPdf = Task.Factory.StartNew(() =>

            {

                OP.Pdf.ConvertRtfToPdfFile(OP.Pdf.RTFExtension, OP.Pdf.PDFExtension);


            });
            DisplayPBar(createPdf);
            ProBarStep = 100;
        }

        private void FormOcrPdf_Shown(object sender, EventArgs e)
        {
            if (ScanedBitMap != null)
            {

                pBoxImage.Image = ScanedBitMap;
                pBoxImage.Size = ScanedBitMap.Size;
                ScanedBitMap = new Bitmap(pBoxImage.Image.Width, pBoxImage.Image.Width);
                ScanedBitMap = (Bitmap)pBoxImage.Image;
                panel1.Visible = true;
                panel1.Dock = DockStyle.Fill;
                pBoxImage.Visible = true;
                pBoxImage.Dock = DockStyle.Fill;


            }
        }

        private void saveCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userControlPB1.label1.Text = "Saving Copy of Image";
            SaveCopy = true;
            SaveImage();
            this.Close();
        }
        private void OcrText(bool freeOCr)
        {
            panel1.Visible = false;
            panel1.Dock = DockStyle.None;
            pBoxImage.Visible = false;
            pBoxImage.Dock = DockStyle.None;
            rTextBoxOcrPdf.Clear();
            userControlPB1.label1.Text = "OCR Image Could take some time";
            rTextBoxOcrPdf.Font = new Font("Arial", 12);
            ProBarStep = 20;
            userControlPB1.pBar.Value = ProBarStep;
            userControlPB1.Visible = true;
            userControlPB1.Update();
            Task ocrText = Task.Factory.StartNew(() =>

            {

                string newText = string.Empty;
              //  if (freeOCr)
                 //   rTextBoxOcrPdf.Text = OP.Pdf.GetImageText(ScanedBitMap).ConfigureAwait(true).GetAwaiter().GetResult();
               // else
                    rTextBoxOcrPdf.Text = OP.Pdf.GetImageOcrText(ScanedBitMap).ConfigureAwait(true).GetAwaiter().GetResult();


                //foreach (string lines in rTextBoxOcrPdf.Lines)
                //{
                //    if (!(string.IsNullOrWhiteSpace(lines)))
                //    {
                //        newText = newText + lines + "\r\n";
                //    }
                //}
                //rTextBoxOcrPdf.Text = newText.Trim();


            });
            DisplayPBar(ocrText);
            ProBarStep = 100;
            userControlPB1.pBar.Value = ProBarStep;
            userControlPB1.Update();
            userControlPB1.Visible = false;
            rTextBoxOcrPdf.Dock = DockStyle.Fill;
            if (!(string.IsNullOrWhiteSpace(rTextBoxOcrPdf.Text)))
            {
                rTextBoxOcrPdf.SelectAll();
                rTextBoxOcrPdf.SelectionAlignment = HorizontalAlignment.Center;
            }
            pBoxImage.Dock = DockStyle.None;
            pBoxImage.Visible = false;
            rTextBoxOcrPdf.Visible = true;
            rTextBoxOcrPdf.Update();
        }
        private void tsMenuOcrImageTxt_Click(object sender, EventArgs e)
        {
            EnableDisableTS(false);
            tsExportText.Visible = true;
            OcrText(false);
            EnableDisableTS(true);


        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FreeImage.Dispose();

            this.Close();
        }

        private void freeOcrTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnableDisableTS(false);
            OcrText(true);
            EnableDisableTS(true);

        }

        private void tsExportText_Click(object sender, EventArgs e)
        {
            if (rTextBoxOcrPdf.Text.Length > 0)
            {
                saveFileDIalog.Title = "Export OCR Text";
                saveFileDIalog.Filter = "Text File|*.txt|Excel File|*.csv";
                saveFileDIalog.ShowDialog();
                if (!(string.IsNullOrWhiteSpace(saveFileDIalog.FileName)))
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDIalog.FileName, false, Encoding.ASCII))
                    {
                        foreach (string lines in rTextBoxOcrPdf.Lines)
                        {
                            if (!(string.IsNullOrWhiteSpace(lines)))
                            {
                                sw.WriteLine(lines);
                            }
                        }
                        //string s = rTextBoxOcrPdf.;
                        //..File.WriteAllText(@"C:\Users\mtcha\Documents", s,Encoding.ASCII );
                    }
                    MessageBox.Show($"File Saved Under {saveFileDIalog.FileName}", "File Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
