using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Edocs.Convert.Viedos.Images.PropConst;
using Edocs.Convert.Viedos.Images.Controls;
using System.Windows.Forms;
using Edocs.HelperUtilities;
using System.IO;

namespace Edocs.Convert.Viedos.Images.Converters
{
    public sealed class ConvertViedoToImage
    {
        ConvertViedoToImage() { }
        private static readonly object lockCheck = new object();
        private static ConvertViedoToImage instance = null;
        public static ConvertViedoToImage ConvertViedoToImageInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockCheck)
                    {
                        if (instance == null)
                        {
                            instance = new ConvertViedoToImage();
                        }
                    }
                }
                return instance;
            }
        }

        public async Task<IDictionary<int, string>> LoadImages(bool runFFmpeg, string wf, System.Windows.Forms.ImageList imgViedoImages, System.Windows.Forms.ListView lViewVImgs,string fileName,string fileExt, bool updateView = true)
        {

            IDictionary<int, string> dicImgLoc = new Dictionary<int, string>();
            if (imgViedoImages.Images.Count > 0)
            {
                
                imgViedoImages.Images.Clear();
                lViewVImgs.Items.Clear();
                //dicImgLoc.Clear();

            }

            lViewVImgs.LargeImageList = imgViedoImages;
            //imgViedoImages.ImageSize = new Size(256, 125);


            int totalImages = 0;
            string imgWF = Utilities.CheckFolderPath(wf);
            if (runFFmpeg)
            {
                string imagesParams = Utilities.ReplaceString(ConstProp.FfmpegExeParamViedoToImages, ConstProp.RepStrViedoName,fileName);
                //   labInfo.Text = $"Processing Viedo File {openFDialog.FileName}";
                Utilities.CreateDirectory(imgWF);
                Utilities.DeleteFiles(imgWF);
                imagesParams = $"{Utilities.ReplaceString(imagesParams, ConstProp.RepStrImageFolder, imgWF)}";
                imagesParams = $"{Utilities.ReplaceString(imagesParams, ConstProp.RepStrImageName, Path.GetFileNameWithoutExtension(fileName))}";
                imagesParams = $"{Utilities.ReplaceString(imagesParams, ConstProp.RepStrImageExten, fileExt)}";
                ConstProp.RunFFmpeg(imagesParams).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            Utilities.DeleteFiles(ConstProp.ViewFolder);
            foreach (var file in Utilities.GetDirectoryFiles(imgWF, $"*{fileExt}", SearchOption.TopDirectoryOnly).ConfigureAwait(false).GetAwaiter().GetResult())

            {
                byte[] fByte = System.IO.File.ReadAllBytes(file);
                using (MemoryStream ms = new MemoryStream(fByte))
                {
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                    dicImgLoc.Add(totalImages, file);
                    imgViedoImages.Images.Add(image);


                }
                AddImagesToView(totalImages, updateView, lViewVImgs).ConfigureAwait(false).GetAwaiter().GetResult();
                totalImages++;
                // labInfo.Text = $"Total Images processed {totalImages}";

            }
           
            return dicImgLoc;
        }
         async Task AddImagesToView(int index, bool updateView, System.Windows.Forms.ListView lViewVImgs)
        {
            int totalImages = index + 1;
            lViewVImgs.BeginUpdate();
            ListViewItem lvi = new ListViewItem();

            lvi.ImageIndex = index;

            lvi.Text = totalImages.ToString();
            totalImages++;
            lViewVImgs.Items.Add(lvi);
           // labInfo.BackColor = System.Drawing.Color.Blue;
            //labInfo.Text = $"Total Images Loaded {totalImages}";

            lViewVImgs.EndUpdate();
            if (updateView)
                lViewVImgs.Update();
        }
        public async Task AddImagesToListView(int index, ImageViewer imgView, IDictionary<int, string> dicImgLoc)
        {
            imgView.ClearSelection();
            string value = ConstProp.GetImageFolder(index, dicImgLoc).ConfigureAwait(false).GetAwaiter().GetResult();
            if (!(string.IsNullOrWhiteSpace(value)))
            {
                string destFile = Path.Combine(ConstProp.ViewFolder, $"ViewFile_{DateTime.Now.ToString("MM_dd_yyy_HH_mm_ss_ff")}_{Path.GetFileName(value)}");
                Utilities.CopyFile(value, destFile, true);

                imgView.Image = System.Drawing.Image.FromFile(destFile);
                //imgView.Image = imgViedoImages.Images[index];
                //  imgView.ScalingMode = Images.Controls.ImageViewer.ImageScalingMode.Fit;
                imgView.ScaleToFit();

            }

        }
    public async Task Save(System.Windows.Forms.ListView lViewVImgs, IDictionary<int, string> dicImgLoc)
        {
            string destFoler = ConstProp.ImageFolder;
            Utilities.CreateDirectory(destFoler);
            Utilities.DeleteFiles(destFoler);
            if (lViewVImgs.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in lViewVImgs.SelectedItems)
                {
                    CopyImages(item.Index, destFoler, dicImgLoc).ConfigureAwait(false).GetAwaiter().GetResult();


                }
            }
            else
            {
                for (int i = 0; i < lViewVImgs.Items.Count; i++)
                {
                    CopyImages(i, destFoler,dicImgLoc).ConfigureAwait(false).GetAwaiter().GetResult();
                }

            }
            CopyImagesToWF().ConfigureAwait(false).GetAwaiter().GetResult();
        }
       public async Task CopyImagesToWF()
        {
            string destFoler = ConstProp.ImageWFFolder;
          //  Utilities.CreateDirectory(destFoler);
        
            Utilities.DeleteFiles(destFoler);
            foreach(string convertFolder in Utilities.GetDirectoryFiles(ConstProp.ImageFolder,"*.*",SearchOption.TopDirectoryOnly).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                string dFolder = $"{Path.Combine(destFoler, Path.GetFileName(convertFolder))}";
                Utilities.CopyFile(convertFolder, dFolder, true);
            }

        }
        public async Task CopyImages(string sFolder,string destFolder,bool delDestFolder=false)
        {
            //string destFoler = ConstProp.ImageWFFolder;
            if(delDestFolder)
             Utilities.CreateDirectory(destFolder);

            
            foreach (string convertFolder in Utilities.GetDirectoryFiles(sFolder, "*.*", SearchOption.TopDirectoryOnly).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                string dFolder = $"{Path.Combine(destFolder, Path.GetFileName(convertFolder))}";
                Utilities.CopyFile(convertFolder, dFolder, true);
            }

        }
        public  async Task CopyImages(int index, string destFoler, IDictionary<int, string> dicImgLoc)
        {
            string value = ConstProp.GetImageFolder(index,dicImgLoc).ConfigureAwait(false).GetAwaiter().GetResult();
            if (!(string.IsNullOrWhiteSpace(value)))
            {
                if (Utilities.CheckFileExists(value))
                {
                    string destFile = Path.Combine(destFoler, Path.GetFileName(value));
                   // labInfo.Text = $"Saving File {value} for File {destFile}";
                    Utilities.CopyFile(value, destFile, true);

                }
                else
                {
                    //labInfo.Text = $"File not found {value}";
                }
            }
        }
     
    }
}
