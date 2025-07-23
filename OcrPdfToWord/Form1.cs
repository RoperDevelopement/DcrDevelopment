using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Edocs.Pdf.Converter;
using System.Threading;
 
namespace OcrPdfToWord
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            userControlPB1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(@"C:\Archives\TestImage\66f708e0-f59a-4e99-94ea-89c008efbd85.TIF");
            Pdf.ConvertBitMapToPdf(bitmap,ImageFormat.Tiff).ConfigureAwait(false).GetAwaiter().GetResult(); ;
            //Task t1 = Task.Factory.StartNew(() =>

            //{

            //    Pdf.OpenPdfFIle(bitmap);

            //});
           // Pdf.OpenPdfFIle(bitmap);
            //userControlPB1.Visible = true;
            ////  t1.Start();
            int n = 10;
            //while (!(t1.IsCompleted))
            //{
            //    int percent = (userControlPB1.proBar.Value / userControlPB1.proBar.Maximum) * 100;
            //    Thread.Sleep(50);
            //        userControlPB1.proBar.Value = n++;
            //    if (n > 50) n = 10;
                
            //    Console.WriteLine();
            //}
           // userControlPB1.Visible = false;
            //// Start the task

            //t1.Start();


            //

         //   richTextBox1.Dock = DockStyle.Top;
         //  richTextBox1.LoadFile(@"C:\Archives\TestImage\test.rtf");
           // this.Refresh();


            //userControlWord1.OpenWordDocument(@"D:\pse\2020-08-25\7bf05140-5ee6-4de5-92fb-c6a73effb64c\912e1431-fad8-46d9-844a-ae62e38b1f71.pdf");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Bitmap b = ConvertTextToImage(richTextBox1.Rtf, "Arial", 12, Color.White, Color.Black, richTextBox1.Width, richTextBox1.Height);
            // b.Save(@"C:\Archives\TestImage\0efa4ab9c5ca.TIF",ImageFormat.Tiff);
            try
            {

                Byte[] bytesToCompress = ASCIIEncoding.Default.GetBytes(richTextBox1.Rtf);
             //   using (var stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(richTextBox1.Rtf))) 
                //using (var stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(richTextBox1.Rtf)))
                ////  using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(richTextBox1.Text)))
              //  {
                        MemoryStream ms2 = new MemoryStream();
                        ms2.Write(bytesToCompress, 0, bytesToCompress.Length);

                    //    Bitmap b1 = new Bitmap(richTextBox1.Width,richTextBox1.Height);
                    //    b1.Save(ms2, ImageFormat.Tiff);
                    ////b1.Save(@"C:\Archives\TestImage\t2.tif", ImageFormat.Tiff);
                    //     Bitmap b2 = new Bitmap(b1);
                    //b2.Save(@"C:\Archives\TestImage\t2.tif", ImageFormat.Tiff);
                    //    richTextBox1.SaveFile(@"C:\Archives\TestImage\tdd2.rtf", RichTextBoxStreamType.RichText);
                //         Image img = Image.FromStream(ms2);
                 //        img.Save(@"C:\Archives\TestImage\t2.tif", ImageFormat.Tiff);

               // }
                 richTextBox1.SaveFile(@"C:\Archives\TestImage\tdd2.rtf", RichTextBoxStreamType.RichText);
                Pdf.SavePdfFIle(@"C:\Archives\TestImage\tdd2.rtf");
                //byte[] bb = File.ReadAllBytes(@"C:\Archives\TestImage\tdd2.rtf");
                // using (var stream = new MemoryStream(bb))
                // {
                //     MemoryStream ms2 = new MemoryStream();
                //      ms2.Write(bb, 0, bb.Length);
                //     Image img = Image.FromStream(stream, true, true);
                //     img.Save(@"C:\Archives\TestImage\t2.tif", ImageFormat.Tiff);
                // }


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        

    }
        public Bitmap ConvertTextToImage(string txt, string fontname, int fontsize, Color bgcolor, Color fcolor, int width, int Height)
        {
            Bitmap bmp = new Bitmap(width, Height);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {

                Font font = new Font(fontname, fontsize);
                graphics.FillRectangle(new SolidBrush(bgcolor), 0, 0, bmp.Width, bmp.Height);
                graphics.DrawString(txt, font, new SolidBrush(fcolor), 0, 0);
                graphics.Flush();
                font.Dispose();
                graphics.Dispose();


            }
            return bmp;
        }

    }
}
