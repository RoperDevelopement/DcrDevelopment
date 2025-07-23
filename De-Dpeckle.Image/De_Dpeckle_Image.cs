using ImageMagick;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace E_Docs.De_Dpeckle.Image
{
    public partial class De_Dpeckle_Image : Form
    {
        private Bitmap origImg;
        private Bitmap imgForScanning;
        public De_Dpeckle_Image()
        {
            InitializeComponent();
        }

        private void pictureBoxOrg_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdlg = new OpenFileDialog();
            ofdlg.ShowDialog();
            var name = ofdlg.FileName;
            var origFile = name;
            Stream imageStreamSource = new FileStream(origFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            using (MemoryStream memstream = new MemoryStream())
            {

                memstream.SetLength(imageStreamSource.Length);
                imageStreamSource.Read(memstream.GetBuffer(), 0, (int)imageStreamSource.Length);
                imageStreamSource.Close();
                origImg = new Bitmap(memstream);
                //  MagickImage img = new MagickImage(memstream.ToArray());
                // img.Grayscale(PixelIntensityMethod.Average);
                // MagickImage img = new MagicImage();
                // ImageMagick.MagickImage()

                // pictureBoxOrg = img.ToBitmap();


            }

            pictureBoxOrg.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBoxOrg.BackgroundImage = origImg;
        }

        private void pictureBoxFinal_Click(object sender, EventArgs e)
        {
            Bitmap imgFor = origImg;
            using (MemoryStream memstream = new MemoryStream())
            {
                imgFor.Save(memstream, ImageFormat.Tiff);
                MagickImage img = new MagickImage(memstream.ToArray());
                img.Despeckle();
                byte[] imgb = img.ToByteArray();
                using (MemoryStream memstream1 = new MemoryStream(imgb))
                {
                    Bitmap origImg1 = new Bitmap(memstream1);
                    pictureBoxFinal.BackgroundImage = origImg1;
                }
                // memstream1
            }
        }
    }
}
