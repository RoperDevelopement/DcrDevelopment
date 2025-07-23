using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OnBarcode.Barcode;
namespace GenerateBarCodes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void GenerateBacode(string _data, string _filename)
        {
            Linear barcode = new Linear();
            barcode.BackColor = Color.White;
            barcode.TextColor = Color.Black;
            barcode.Type = BarcodeType.CODE128;
            barcode.Data = _data;
            barcode.drawBarcode(_filename);
        }
        private void GenerateQrcode(string _data, string _filename)
        {
            QRCode qrcode = new QRCode();
            qrcode.Data = _data;
            qrcode.DataMode = QRCodeDataMode.Byte;
            qrcode.UOM = UnitOfMeasure.PIXEL;
            qrcode.X = 3;
            qrcode.LeftMargin = 0;
            qrcode.RightMargin = 0;
            qrcode.TopMargin = 0;
            qrcode.BottomMargin = 0;
            qrcode.Resolution = 72;
            qrcode.Rotate = Rotate.Rotate0;
            qrcode.ImageFormat = ImageFormat.Jpeg;
            qrcode.drawBarcode(_filename);
        }
        private void AddPath()
        {
            if (!(string.IsNullOrWhiteSpace(txtBarCode.Text)))
            {
                //txtSaveName.Text = $@"I:\BarcodeImages\{txtBarCode.Text}.gif";
                
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            AddPath();
            //txtSaveName.Text = "l:\\d.gif";
            GenerateBacode(txtBarCode.Text, txtSaveName.Text);
          //  txtSaveName.Text = string.Empty;
         ///   txtSaveName.Text = txtSaveName.Text.Replace("gif", "Jpeg");
          //  txtSaveName.Text = "l:\\d.Jpeg";
         //   GenerateQrcode(txtBarCode.Text, txtSaveName.Text);
        }

        private void txtSaveName_Enter(object sender, EventArgs e)
        {
            AddPath();
            
        }
    }
}
