using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Edocs.HelperUtilities;
using Edocs.Convert.Viedos.Images.PropConst;
using System.Diagnostics;
using System.IO;
using Edocs.Convert.Viedos.Images.Converters;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Net.WebRequestMethods;
using Edocs.Convert.Viedos.Images.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Net.NetworkInformation;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;

namespace Edocs.Convert.Viedos.Images
{
    public partial class MainForm : Form
    {
        //Dictionary<int, Image> dicImgLoc = new Dictionary<int, Image>();
        IDictionary<int, string> dicImgLoc = new Dictionary<int, string>();
        string ImgaeFileExt
        { get; set; }
        bool ImagesSaved
        { get; set; }


        public MainForm()
        {
            InitializeComponent();
        }



        private void MainForm_Load(object sender, EventArgs e)
        {
            Init().ConfigureAwait(false).GetAwaiter().GetResult();
            string args = ConstProp.GetInputArgs(ConstProp.Args).ConfigureAwait(false).GetAwaiter().GetResult();
            if (!(string.IsNullOrWhiteSpace(args)))
            {
                if (string.Compare(args, ConstProp.ArgsParmaImage, true) == 0)
                {
                    ImportImagesScanQuire().ConfigureAwait(false).GetAwaiter().GetResult();
                }
                else
                    tsmpng.PerformClick();
            }

        }
        async Task Init()
        {
            this.Text = "Convert Videos or Images V1.0";
            UpdateProBar(0, "Select Viedo or Images").ConfigureAwait(false).GetAwaiter().GetResult();
            tsmPlayViedo.Visible = false;
            imgViedoImages.ImageSize = new Size(200, 125);
            tsmSaveImages.Visible = false;
            tsmSaveClose.Visible = false;
            imgViedoImages.TransparentColor = Color.Blue;
            lViewVImgs.View = View.LargeIcon;
            tsmDeleteImage.Visible = true;
            //msDelete.Visible = false;
            ImagesSaved = true;
            tsmcreate.Visible = false;
            pBar.Step = 0;

            tsmDeleteImage.Visible = false;
            openFDialog.Multiselect = false;
            this.WindowState = FormWindowState.Maximized;
            pBar.Visible = false;
            //     pBar.Location = new Point(0,412);
            // pBar.Location = new Point(MainForm.Location.X , this.Location.Y);
            pBar.Dock = DockStyle.Bottom;
            Utilities.CreateDirectory(ConstProp.ViewFolder);
            Utilities.CreateDirectory(ConstProp.ImageFolder);
            Utilities.CreateDirectory(ConstProp.ImageWFFolder);
            Utilities.CreateDirectory(ConstProp.ViedoCreateFolder);
            Utilities.DeleteFiles(ConstProp.ViedoCreateFolder);
            Utilities.DeleteFiles(ConstProp.ViewFolder);
            Utilities.DeleteFiles(ConstProp.ImageFolder);

            Utilities.DeleteFiles(ConstProp.ViedoCreateFolder);

            UpDateLabel("Create Images or Viedo", System.Drawing.Color.Azure);
        }






        private async Task GetViedoFile()
        {
            openFDialog.Filter = "MP4|*.Mp4|MP3|*.Mp3|Ogv|*.Ogv|TS|*.Ts|Webmd|*.WebM";

            if (openFDialog.ShowDialog() == DialogResult.OK)
            {

                pBar.Visible = true;
                pBar.Update();
                
                UpDateLabel($"Creating Images from Viedo {openFDialog.FileName}", System.Drawing.Color.Aqua);
                UpdateProBar(50).ConfigureAwait(false).GetAwaiter().GetResult();
                dicImgLoc = ConvertViedoToImage.ConvertViedoToImageInstance.LoadImages(true, ConstProp.ImageWFFolder, imgViedoImages, lViewVImgs, openFDialog.FileName, ImgaeFileExt, true).ConfigureAwait(false).GetAwaiter().GetResult();
                ConvertViedoToImage.ConvertViedoToImageInstance.AddImagesToListView(0, imgView, dicImgLoc).ConfigureAwait(false).GetAwaiter().GetResult();

                UpdateProBar(100).ConfigureAwait(false).GetAwaiter().GetResult();
                tsmDeleteImage.Visible = true;
                ImagesSaved = false;
                tsmcreate.Visible = false;
                tsmIimportImages.Visible = true;
                tsmSaveImages.Visible = true;
                tsmSaveClose.Visible = true;
                ImagesSaved = false;
                tsmcreate.Visible = false;
                tsmIimportImages.Visible = false;
                UpDateLabel($"Done tuning Viedo to Images", System.Drawing.Color.Green);
                
                //LoadImages(true, ConstProp.ImageWFFolder).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            pBar.Visible = false;
        }






        private void lViewVImgs_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (lViewVImgs.SelectedItems.Count > 0)
            {
                UpDateLabel($"Adding Images to View", System.Drawing.Color.Brown);
                pBar.Visible = true;
                int i = 10;
                UpdateProBar(10);
                foreach (ListViewItem item in lViewVImgs.SelectedItems)
                {
                    i = i + 5;
                    UpdateProBar(i).ConfigureAwait(false).GetAwaiter().GetResult();
                    //  AddImagesToListView(item.Index).ConfigureAwait(false).GetAwaiter().GetResult();
                    ConvertViedoToImage.ConvertViedoToImageInstance.AddImagesToListView(item.Index, imgView, dicImgLoc).ConfigureAwait(false).GetAwaiter().GetResult();
                    //    pBoxImages.Image = imgViedoImages.Images[item.Index];
                    ////do something with item.text or whatever
                }
                pBar.Visible = false;
                UpDateLabel($"Done Adding Images to View", System.Drawing.Color.Green);
            }
        }

        private void pngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImgaeFileExt = ".png";

            GetViedoFile().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private void bmpToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ImgaeFileExt = ".bmp";

            GetViedoFile().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private void jpgToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ImgaeFileExt = ".jpeg";

            GetViedoFile().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private void gifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                GetViedoFile().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        async Task SaveandReview()
        {
            if (lViewVImgs.SelectedItems.Count > 0)
            {
                pBar.Visible = true;
                ImagesSaved = true;


                UpDateLabel($"Saving Images for review", System.Drawing.Color.Magenta);
                UpdateProBar(50).ConfigureAwait(false).GetAwaiter().GetResult();
                ConvertViedoToImage.ConvertViedoToImageInstance.Save(lViewVImgs, dicImgLoc).ConfigureAwait(false).GetAwaiter().GetResult();
                // imgView.Image.Dispose();

                UpdateProBar(75).ConfigureAwait(false).GetAwaiter().GetResult();
                dicImgLoc = ConvertViedoToImage.ConvertViedoToImageInstance.LoadImages(false, ConstProp.ImageWFFolder, imgViedoImages, lViewVImgs, openFDialog.FileName, ImgaeFileExt, true).ConfigureAwait(false).GetAwaiter().GetResult();

                UpdateProBar(90).ConfigureAwait(false).GetAwaiter().GetResult();
                ConvertViedoToImage.ConvertViedoToImageInstance.AddImagesToListView(0, imgView, dicImgLoc).ConfigureAwait(false).GetAwaiter().GetResult();
                UpDateLabel($"Complete", System.Drawing.Color.Green);
                
            }
            else
            {
                UpDateLabel($"No Images Selected to Save", System.Drawing.Color.Red);
                MessageBox.Show("No Images Selected to Save", "No Images", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                
            pBar.Visible = false;
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveandReview().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {



            if (lViewVImgs.SelectedItems.Count > 0)
            {
                ImagesSaved = true;
                pBar.Visible = true;
                labInfo.Text = $"Saving images";
                UpDateLabel($"Saving images", System.Drawing.Color.Azure);
                UpdateProBar(50).ConfigureAwait(false).GetAwaiter().GetResult();
                ConvertViedoToImage.ConvertViedoToImageInstance.Save(lViewVImgs, dicImgLoc).ConfigureAwait(false).GetAwaiter().GetResult();
                pBar.Visible = false;
                UpDateLabel($"Complete", System.Drawing.Color.Green);

            }
            else
            {
                if (!(ImagesSaved))
                {
                    if ((MessageBox.Show("Save Images", "No Images Saved", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK))
                    {
                        pBar.Visible = true;
                       
                        UpDateLabel($"Saving Images", System.Drawing.Color.Azure);
                        UpdateProBar(50).ConfigureAwait(false).GetAwaiter().GetResult();

                        ConvertViedoToImage.ConvertViedoToImageInstance.Save(lViewVImgs, dicImgLoc).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    else
                        Utilities.DeleteFiles(ConstProp.ImageFolder);
                }

                this.Close();
            }


        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ImagesSaved)
                this.Close();
            else
            {
                if (MessageBox.Show("Saved Images", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    
                    UpDateLabel($"Saving Images", System.Drawing.Color.Azure);
                    pBar.Visible = true;
                    ConvertViedoToImage.ConvertViedoToImageInstance.Save(lViewVImgs, dicImgLoc).ConfigureAwait(false).GetAwaiter().GetResult();
                }

                this.Close();
            }
        }

        private async Task DelImages()
        {
            int itemsSel = lViewVImgs.SelectedItems.Count;
            if (itemsSel > 0)
            {
                
                UpDateLabel($"Deleing Images", System.Drawing.Color.Azure);
                pBar.Visible = true;

                itemsSel = (int)(Math.Round(itemsSel * 0.25) + itemsSel);
                UpdateProBar(itemsSel).ConfigureAwait(false).GetAwaiter().GetResult();
                foreach (ListViewItem item in lViewVImgs.SelectedItems)
                {
                    itemsSel = itemsSel + 25;
                    string value = ConstProp.GetImageFolder(item.Index, dicImgLoc).ConfigureAwait(false).GetAwaiter().GetResult();
                    UpdateProBar(itemsSel).ConfigureAwait(false).GetAwaiter().GetResult();
                    //lViewVImgs.BeginUpdate();
                    //lViewVImgs.Items.RemoveAt(item.Index);
                    //lViewVImgs.EndUpdate();
                    // lViewVImgs.Update();

                    if (!(string.IsNullOrWhiteSpace(value)))
                    {
                        if (Utilities.CheckFileExists(value))
                        {
                            
                            UpDateLabel($"Deleting File {value}", System.Drawing.Color.Green);

                            Utilities.DeleteFile(value);

                        }
                        else
                        {
                         
                            UpDateLabel($"File not found {value}", System.Drawing.Color.Red);
                        }
                    }
                }
                ImagesSaved = false;
                //Dictionary<int, string> dicTemp = new Dictionary<int, string>();
                //foreach (KeyValuePair<int, string> keyValuePair in dicImgLoc)
                //{
                //    dicTemp.Add(keyValuePair.Key - 1, keyValuePair.Value);
                //}
                //dicImgLoc.Clear();
                //dicImgLoc = dicTemp;
                
                UpDateLabel($"UpDating Images", System.Drawing.Color.Azure);
            
            UpdateProBar(75).ConfigureAwait(false).GetAwaiter().GetResult();
                dicImgLoc = ConvertViedoToImage.ConvertViedoToImageInstance.LoadImages(false, ConstProp.ImageWFFolder, imgViedoImages, lViewVImgs, openFDialog.FileName, ImgaeFileExt, true).ConfigureAwait(false).GetAwaiter().GetResult();
                if (dicImgLoc.Count > 0)
                    ConvertViedoToImage.ConvertViedoToImageInstance.AddImagesToListView(0, imgView, dicImgLoc).ConfigureAwait(false).GetAwaiter().GetResult();
                UpdateProBar(100).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            pBar.Visible = false;

            UpDateLabel($"Complete", System.Drawing.Color.Green);
            //    pBoxImages.Image = imgViedoImages.Images[item.Index];
            ////do something with item.text or whatever


        }
        private async Task SetImportImagesTSM()
        {
            ImagesSaved = true;
            tsmIimportImages.Visible = false;
            tsmcreate.Visible = true;
            tsmConvertVedio.Visible = false;
            tsmSaveImages.Visible = true;
            tsmDeleteImage.Visible = true;
            pBar.Visible = true;
            ImagesSaved = false;
            ImgaeFileExt = "*.*";
            
            UpDateLabel($"Importing Images From ScanQuire", System.Drawing.Color.Azure);
            UpdateProBar(25).ConfigureAwait(false).GetAwaiter().GetResult();
            pBar.Visible = false;
         
            UpDateLabel($"Done Importing Images From ScanQuire", System.Drawing.Color.Green);
        }
        private async Task ImortImages()
        {

            openFDialog.Filter = "PNG image|*.png|GIF image|*.gif|Bitmap image|*.bmp|JPEG image|*.jpg|JPEG image|*.jpeg|TIFF image|*.tif|TIFF image|*.tiff";
            openFDialog.Multiselect = true;
            if (openFDialog.ShowDialog() == DialogResult.OK)
            {
                
                UpDateLabel($"Importing Images", System.Drawing.Color.Azure);
                pBar.Visible = true;
                SetImportImagesTSM().ConfigureAwait(false).GetAwaiter().GetResult();


                ImgaeFileExt = "*.*";
                Utilities.DeleteFiles(ConstProp.ImageWFFolder);
                UpdateProBar(25).ConfigureAwait(false).GetAwaiter().GetResult();

                foreach (string fName in openFDialog.FileNames)
                {


                    string destFoler = $"{Path.Combine(ConstProp.ImageWFFolder, Path.GetFileName(fName))}";
                    Utilities.CopyFile(fName, destFoler, true);
                }

                UpdateProBar(50).ConfigureAwait(false).GetAwaiter().GetResult();
                dicImgLoc = ConvertViedoToImage.ConvertViedoToImageInstance.LoadImages(false, ConstProp.ImageWFFolder, imgViedoImages, lViewVImgs, openFDialog.FileName, ImgaeFileExt, true).ConfigureAwait(false).GetAwaiter().GetResult();
                if (dicImgLoc.Count > 0)
                {
                    UpdateProBar(75).ConfigureAwait(false).GetAwaiter().GetResult();
                    ConvertViedoToImage.ConvertViedoToImageInstance.AddImagesToListView(0, imgView, dicImgLoc).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                pBar.Visible = false;
                UpDateLabel($"Done Importing Images", System.Drawing.Color.Green);
                
            }
        }
        private async Task UpdateProBar(int step)
        {
            pBar.Value = step;
            //pBar.Step = step;
               pBar.PerformStep();
            pBar.CreateGraphics().DrawString(pBar.Value.ToString() + "%",
       new Font("Arial", (float)8.25, FontStyle.Regular),
       Brushes.Azure, new PointF(pBar.Width / 2 - 10,
           pBar.Height / 2 - 7));
        }
        private async Task UpdateProBar(int step, string text)
        {
            if (step > 0)
            {
                pBar.Value = step;
                text = $"{text} {pBar.Value.ToString()}+%";
            }
            //  pBar.BackColor = System.Drawing.Color.Green;
            //pBar.Step = step;
            // pBar.PerformStep();
            pBar.Value = 0;
            pBar.PerformStep();
            pBar.CreateGraphics().DrawString(step.ToString().Trim() + "%",
                  new Font("Arial", (float)8.25, FontStyle.Regular),
       Brushes.Azure, new PointF(pBar.Width,
           pBar.Height));
            //     new Font("Arial", (float)8.25, FontStyle.Regular),
            //Brushes.Azure, new PointF(pBar.Width / 2 - 10,
            //    pBar.Height / 2 - 7));  
        }
        private void deleteImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lViewVImgs.SelectedItems.Count > 0)
            {
                
                UpDateLabel($"Deleting Images", System.Drawing.Color.Red);
                DelImages().ConfigureAwait(false).GetAwaiter().GetResult();
                UpDateLabel($"Done Deleting Images", System.Drawing.Color.Green);
                

            }

            else
            {
                MessageBox.Show("No Images Selected to Delete", "No Images", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
                UpDateLabel($"No Images Selected to Deleting", System.Drawing.Color.DarkRed);

            }

        }

        private void importImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //pBar.Value = 0;
            //pBar.Maximum = 0;
            //pBar.Maximum = 100;
            //pBar.Step = 100;
            // IProgress<int> progress = new Progress<int>();

            //  progress.Report(100);
            //var progress = new Progress<int>(percent =>
            //{
            //    pBar.Value = percent;

            //});
            // Task.Run(() => DoSomething(progress));
            //  pBar.Step = 25;
            UpDateLabel($"Getting Images", System.Drawing.Color.Azure);

         

            ImortImages().ConfigureAwait(false).GetAwaiter().GetResult();


        }

        private void createViedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sFileDialog.Filter = "MP4 Viedo|.mp4";
            if (sFileDialog.ShowDialog() == DialogResult.OK)
            {
              
                UpDateLabel($"Creating Viedo", System.Drawing.Color.Azure);
                pBar.Visible = true;
                tsmIimportImages.Visible = false;

                tsmConvertVedio.Visible = true;
                UpdateProBar(0, $"Converting Images To Viedo").ConfigureAwait(false).GetAwaiter().GetResult();

                Utilities.DeleteFile(sFileDialog.FileName);
                UpdateProBar(10).ConfigureAwait(false).GetAwaiter().GetResult();
                ConvertViedoToImage.ConvertViedoToImageInstance.Save(lViewVImgs, dicImgLoc).ConfigureAwait(false).GetAwaiter().GetResult();
                // ConvertViedoToImage.ConvertViedoToImageInstance.CopyImages(ConstProp.ImageWFFolder, ConstProp.ImageFolder);
                UpdateProBar(0, $"Loading Images to Convert").ConfigureAwait(false).GetAwaiter().GetResult();


                UpdateProBar(25).ConfigureAwait(false).GetAwaiter().GetResult();
                dicImgLoc = ConvertViedoToImage.ConvertViedoToImageInstance.LoadImages(false, ConstProp.ImageWFFolder, imgViedoImages, lViewVImgs, openFDialog.FileName, ImgaeFileExt, true).ConfigureAwait(false).GetAwaiter().GetResult();
                //labInfo.Text = $"Checking Images to Convert";
                // labInfo.Update();
                UpdateProBar(50).ConfigureAwait(false).GetAwaiter().GetResult();
                ConvertImagesToViedo.ConvertImagesToViedoInstance.SaveImageAsPing(dicImgLoc).ConfigureAwait(false).GetAwaiter().GetResult();
                //labInfo.Text = $"Running Process to Convert Images to Viedo";
                //labInfo.Update();
                UpdateProBar(75).ConfigureAwait(false).GetAwaiter().GetResult();
                ConvertImagesToViedo.ConvertImagesToViedoInstance.CreateViedo(sFileDialog.FileName).ConfigureAwait(false).GetAwaiter().GetResult();
                //  dicImgLoc = ConvertViedoToImage.ConvertViedoToImageInstance.LoadImages(false, ConstProp.ImageWFFolder, imgViedoImages, lViewVImgs, openFDialog.FileName, ImgaeFileExt, true).ConfigureAwait(false).GetAwaiter().GetResult();
                // pBar.Visible = false;
                if (MessageBox.Show("Play Saved Viedo", "Play", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    UpdateProBar(0, $"Play viedo {sFileDialog.FileName}").ConfigureAwait(false).GetAwaiter().GetResult();


                    VIewViedoForm vIewViedo = new VIewViedoForm();
                    vIewViedo.ViedoFileName = sFileDialog.FileName;
                    vIewViedo.ShowDialog();
                }
                tsmPlayViedo.Visible = true;
                UpdateProBar(0, $"Complete").ConfigureAwait(false).GetAwaiter().GetResult();
                UpDateLabel($"Donge Creating Viedo", System.Drawing.Color.Green);
               
                pBar.Visible = false;
            }
        }

        private void tsmPlayViedo_Click(object sender, EventArgs e)
        {
            openFDialog.Filter = "MP4 Viedo |*.mp4";
            //4GIF image|*.gif|Bitmap image|*.bmp|JPEG image|*.jpg|JPEG image|*.jpeg|TIFF image|*.tif|TIFF image|*.tiff";
            if (openFDialog.ShowDialog() == DialogResult.OK)
            {
                UpDateLabel($"Playing Viedo", System.Drawing.Color.Green);
                labInfo.Text = $"";
                //   labInfo.Text = $"Play viedo {sFileDialog.FileName}";
                // labInfo.Update();
                
                VIewViedoForm vIewViedo = new VIewViedoForm();
                vIewViedo.ViedoFileName = sFileDialog.FileName;
                vIewViedo.ShowDialog();
                // labInfo.Text = $"Select Option";
                // labInfo.Update();
            }
            UpDateLabel($"Done Playing Viedo", System.Drawing.Color.Green);

        }
        public void DoSomething(IProgress<int> progress)
        {
            for (int i = 1; i <= 100; i++)
            {
                pBar.PerformStep();
                Thread.Sleep(100);
                if (progress != null)
                    progress.Report(i);

            }
        }
        async void UpDateLabel(string text, System.Drawing.Color color)
        {
            labInfo.Text = text;
            labInfo.BackColor = color;
        }
        async Task ImportImagesScanQuire()
        {
            
            SetImportImagesTSM().ConfigureAwait(false).GetAwaiter().GetResult();
            UpdateProBar(50).ConfigureAwait(false).GetAwaiter().GetResult();
            dicImgLoc = ConvertViedoToImage.ConvertViedoToImageInstance.LoadImages(false, ConstProp.ImageWFFolder, imgViedoImages, lViewVImgs, openFDialog.FileName, ImgaeFileExt, true).ConfigureAwait(false).GetAwaiter().GetResult();
            if (dicImgLoc.Count > 0)
            {
                UpdateProBar(75).ConfigureAwait(false).GetAwaiter().GetResult();
                ConvertViedoToImage.ConvertViedoToImageInstance.AddImagesToListView(0, imgView, dicImgLoc).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }

        
    }
}

