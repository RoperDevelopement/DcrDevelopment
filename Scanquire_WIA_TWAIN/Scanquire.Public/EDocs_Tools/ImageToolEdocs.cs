//using code from https://www.codeproject.com https://www.codeproject.com/Articles/33838/Image-Processing-using-C and per The Code Project Open License (CPOL) https://www.codeproject.com/info/cpol10.aspx can use code
//icons come from <a href="https://www.flaticon.com/free-icons/undo" title="undo icons">Undo icons created by Md Tanvirul Haque - Flaticon</a>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scanquire.Public.EDocs_Tools
{
    public partial class ImageToolEdocs : Form
    {
     public   ImageHandler imageHandler = new ImageHandler();
        double zoomFactor = 1.0;
        public ImageToolEdocs()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(Convert.ToInt32(imageHandler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32(imageHandler.CurrentBitmap.Height * zoomFactor));
            this.Invalidate();
            this.Refresh();
        }
        private void ImageToolEdocs_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(imageHandler.CurrentBitmap, new Rectangle(this.AutoScrollPosition.X, this.AutoScrollPosition.Y, Convert.ToInt32(imageHandler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32(imageHandler.CurrentBitmap.Height * zoomFactor)));
            this.AutoScrollMinSize = new Size(Convert.ToInt32(imageHandler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32(imageHandler.CurrentBitmap.Height * zoomFactor));
        }

        private void menuItemReSize_Click(object sender, System.EventArgs e)
        {

        }
    }
}
