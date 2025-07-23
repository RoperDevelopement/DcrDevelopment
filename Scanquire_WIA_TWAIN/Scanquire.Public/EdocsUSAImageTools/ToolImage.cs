using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace Scanquire.Public.EdocsUSAImageTools
{
    public partial class ToolImage : Form
    {

        BrightnessForm bFrm = null;
        ContrastForm cFrm = null;
        public ImageHandler imageHandler = new ImageHandler();
        double zoomFactor = 1.0;
        public string SaveFormat
        { get; set; }
        Point SelPoint
        { get; set; }
        Rectangle Rect
        { get; set; }
        int MouseX
        { get; set; }
        int MouseY
        { get; set; }
        bool DrawRect
        { get; set; }

        Point MousePoint
        { get; set; }
        bool CropInsert
        { get; set; }
        public int BitMapw
        { get; set; }
        public ToolImage()
        {
            InitializeComponent();
            //Init();
        }
        private async Task Init()
        {
            this.Cursor = Cursors.WaitCursor;
            SaveFormat = string.Empty;
            this.AutoScroll = true;
            //this.AutoScrollMinSize = new Size(Convert.ToInt32(imageHandler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32(imageHandler.CurrentBitmap.Height * zoomFactor));


            zoomPercentToolStripMenuItem.Text = "100%";

            if (imageHandler.CurrentBitmap.Width > 6000)
            {
                // imageHandler.Resize(imageHandler.CurrentBitmap.Width / 2, imageHandler.CurrentBitmap.Height / 2).ConfigureAwait(false).GetAwaiter().GetResult();
                int newSizeW = imageHandler.CurrentBitmap.Width / 2;
                int newSizeH = imageHandler.CurrentBitmap.Height / 2;
                imageHandler.CurrentBitmap = imageHandler.ResizeImage(imageHandler.CurrentBitmap, new Size(newSizeW, newSizeH)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            imageHandler.BitmapBeforeProcessing = imageHandler.CurrentBitmap;
            CropInsert = false;
            InitChildForm();
            this.Invalidate();
            this.Cursor = Cursors.Default;
            //  imageHandler.CurrentBitmap = imageHandler.BitmapBeforeProcessing;


        }
        private void InitChildForm()
        {

            bFrm = new BrightnessForm();
            bFrm.ChangeBrightness += ChangeBrightness;
            bFrm.RestoreImageBFRM += RestoreImageBFRM;




            cFrm = new ContrastForm();
            cFrm.UpdateContrast += UpdateContrast;
            cFrm.RestoreImageCont += RestoreImageCont;

        }

        private void RestoreImageCont()
        {
            this.Cursor = Cursors.WaitCursor;
            imageHandler.RestorePrevious().ConfigureAwait(false).GetAwaiter().GetResult();
            this.Invalidate();
            this.Cursor = Cursors.Default;
        }
        private void RestoreImageBFRM()
        {
            this.Cursor = Cursors.WaitCursor;
            imageHandler.RestorePrevious().ConfigureAwait(false).GetAwaiter().GetResult();
            this.Invalidate();
            this.Cursor = Cursors.Default;
        }
        private void ChangeBrightness(int brightness)
        {
            this.Cursor = Cursors.WaitCursor;
            // imageHandler.RestorePrevious().ConfigureAwait(false).GetAwaiter().GetResult();
            imageHandler.SetBrightness(brightness).ConfigureAwait(false).GetAwaiter().GetResult();
            this.Invalidate();
            this.Cursor = Cursors.Default;
        }
        private void UpdateContrast(double contrast)
        {
            this.Cursor = Cursors.WaitCursor;
            // imageHandler.RestorePrevious().ConfigureAwait(false).GetAwaiter().GetResult();
            imageHandler.SetContrast(contrast).ConfigureAwait(false).GetAwaiter().GetResult();
            this.Invalidate();
            this.Cursor = Cursors.Default;
        }
        private void ToolImage_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawImage(imageHandler.CurrentBitmap, new Rectangle(this.AutoScrollPosition.X, this.AutoScrollPosition.Y, Convert.ToInt32(imageHandler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32(imageHandler.CurrentBitmap.Height * zoomFactor)));
            if (DrawRect)
            {
                Brush brush = new SolidBrush(Color.Black);
                Pen pen = new Pen(brush, (float)5);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                e.Graphics.DrawRectangle(pen, Rect);
                pen.Dispose();
                brush.Dispose();
                System.Threading.Thread.Sleep(30);
                //   Size size1 = new Size(Rect.X, Rect.Y);
                // Size size2 = new Size(Rect.Width, Rect.Height);
                //    Point p1 = new Point(Rect.X,0);
                //  Point p2 = new Point(Rect.Width, Rect.Height);
                //  e.Graphics.DrawLine(pen,p1,p2);
                //  g.DrawImage(imageHandler.CurrentBitmap, new Rectangle(this.AutoScrollPosition.X, this.AutoScrollPosition.Y, Convert.ToInt32(imageHandler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32////(imageHandler.CurrentBitmap.Height * zoomFactor)));

            }


            this.AutoScrollMinSize = new Size(Convert.ToInt32(imageHandler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32(imageHandler.CurrentBitmap.Height * zoomFactor));

        }
        async void ResizeImage(int w, int h)
        {
            try
            {
                CropInsert = false;
                using (InsertTextForm1 rFrm = new InsertTextForm1())
                {

                    if (w > 0)
                    {
                        rFrm.NewWidth = w;
                        rFrm.NewHeight = h;
                    }
                    else
                    {
                        rFrm.NewWidth = imageHandler.CurrentBitmap.Width;
                        rFrm.NewHeight = imageHandler.CurrentBitmap.Height;
                    }
                    if (rFrm.ShowDialog() == DialogResult.OK)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        //  imageHandler.RestorePrevious();
                        imageHandler.Resize(rFrm.NewWidth, rFrm.NewHeight).ConfigureAwait(false).GetAwaiter().GetResult();
                        // this.AutoScrollMinSize = new Size(Convert.ToInt32(imageHandler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32(imageHandler.CurrentBitmap.Height * zoomFactor));
                        this.Invalidate();
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error ReSizeing Image {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.Refresh();
            }
        }
        //private void rmenuItemResize_Click(object sender, EventArgs e)
        //{
        //    using (InsertTextForm1 rFrm = new InsertTextForm1())
        //    {


        //        rFrm.NewWidth = imageHandler.CurrentBitmap.Width;
        //        rFrm.NewHeight = imageHandler.CurrentBitmap.Height;
        //        if (rFrm.ShowDialog() == DialogResult.OK)
        //        {
        //            this.Cursor = Cursors.WaitCursor;
        //            //  imageHandler.RestorePrevious();
        //            imageHandler.Resize(rFrm.NewWidth, rFrm.NewHeight).ConfigureAwait(false).GetAwaiter().GetResult();
        //            // this.AutoScrollMinSize = new Size(Convert.ToInt32(imageHandler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32(imageHandler.CurrentBitmap.Height * zoomFactor));
        //            this.Invalidate();
        //            this.Cursor = Cursors.Default;
        //        }
        //    }
        //}

        private void zoomPercentToolStripMenuItem_SelectedIndexChanged(object sender, EventArgs e)
        {

            zoomFactor = imageHandler.ConvertStrToPercent(zoomPercentToolStripMenuItem.Text);
            //  cZoom.Checked = false;
            //  menuItemZoom50.Checked = true;
            //  cZoom = menuItemZoom50;
            this.AutoScrollMinSize = new Size(Convert.ToInt32(imageHandler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32(imageHandler.CurrentBitmap.Height * zoomFactor));
            this.Invalidate();
        }

        private void redFillerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {


                // imageHandler.RestorePrevious();
                //  imageHandler.CurrentBitmap = imageHandler.BitmapBeforeProcessing;
                imageHandler.RestorePrevious().ConfigureAwait(false).GetAwaiter().GetResult(); ;
                imageHandler.SetColorFilter(ImageHandler.ColorFilterTypes.Red).ConfigureAwait(false).GetAwaiter().GetResult();
                this.Invalidate();
                this.Cursor = Cursors.Default;
                redFillerToolStripMenuItem.Checked = true;
                blueFillerToolStripMenuItem.Checked = false;
                greenFillerToolStripMenuItem.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Setting Red Color Filter Image {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.Refresh();
            }
        }

        private void blueFillerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // imageHandler.RestorePrevious();
                imageHandler.RestorePrevious().ConfigureAwait(false).GetAwaiter().GetResult();
                imageHandler.SetColorFilter(ImageHandler.ColorFilterTypes.Blue).ConfigureAwait(false).GetAwaiter().GetResult();
                this.Invalidate();
                this.Cursor = Cursors.Default;

                blueFillerToolStripMenuItem.Checked = true;
                redFillerToolStripMenuItem.Checked = false;
                greenFillerToolStripMenuItem.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Setting Blue Color Filter Image {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.Refresh();
            }
        }

        private void greenFillerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // imageHandler.CurrentBitmap = imageHandler.BitmapBeforeProcessing;
                imageHandler.RestorePrevious().ConfigureAwait(false).GetAwaiter().GetResult();
                imageHandler.SetColorFilter(ImageHandler.ColorFilterTypes.Green).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Setting Red Color Filter Image {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.Refresh();
            }
            this.Invalidate();
            this.Cursor = Cursors.Default;
            blueFillerToolStripMenuItem.Checked = false;
            redFillerToolStripMenuItem.Checked = false;
            greenFillerToolStripMenuItem.Checked = true;
        }

        private void ToolImage_Load(object sender, EventArgs e)
        {
            Init().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private void brightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                //  using (BrightnessForm bFrm = new BrightnessForm())
                //{
                //     CheckOpened("ContrastForm").ConfigureAwait(false).GetAwaiter().GetResult();

                // bFrm.BrightnessValue = 0;

                if (bFrm.ShowDialog() == DialogResult.Cancel)
                {
                    if (MessageBox.Show("Restore Image", "Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        imageHandler.RestorePrevious().ConfigureAwait(false).GetAwaiter().GetResult();
                        bFrm.BrightnessValue = 0;
                        this.Cursor = Cursors.WaitCursor;

                        this.Cursor = Cursors.Default;
                    }
                }
                this.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Brightness {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.Refresh();
            }
        }

        private void contrastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // CheckOpened(string.Empty).ConfigureAwait(false).GetAwaiter().GetResult();
                //InitChildForms();

                // cFrm.ContrastValue = 0;
                if (cFrm.ShowDialog() == DialogResult.Cancel)
                {
                    if (MessageBox.Show("Restore Image", "Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        imageHandler.RestorePrevious().ConfigureAwait(false).GetAwaiter().GetResult();
                        this.Invalidate();
                        cFrm.ContrastValue = 0;
                    }

                }
                this.Cursor = Cursors.Default;


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Setting Contrast {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.Refresh();
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageHandler.RestorePrevious().ConfigureAwait(false).GetAwaiter().GetResult();
            this.Refresh();
        }

        private void gammaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (GammaForm gFrm = new GammaForm())
                {
                    gFrm.RedComponent = gFrm.GreenComponent = gFrm.BlueComponent = 0;
                    if (gFrm.ShowDialog() == DialogResult.OK)
                    {

                        if ((gFrm.RedComponent > 0) && (gFrm.GreenComponent > 0) && (gFrm.BlueComponent > 0))
                        {
                            this.Cursor = Cursors.WaitCursor;
                            imageHandler.RestorePrevious().ConfigureAwait(false).GetAwaiter().GetResult();
                            imageHandler.SetGamma(gFrm.RedComponent, gFrm.GreenComponent, gFrm.BlueComponent).ConfigureAwait(false).GetAwaiter().GetResult();
                            this.Invalidate();
                            this.Cursor = Cursors.Default;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Setting Gamma {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.Refresh();
            }
        }

        private void grayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {


                this.Cursor = Cursors.WaitCursor;
                // imageHandler.RestorePrevious();
                imageHandler.SetGrayscale().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Setting Gray Scale {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.Refresh();
            }
            this.Invalidate();
            this.Cursor = Cursors.Default;
        }

        private void invertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {


                this.Cursor = Cursors.WaitCursor;
                imageHandler.RestorePrevious().ConfigureAwait(false).GetAwaiter().GetResult();
                imageHandler.SetInvert().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Invert Image {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.Refresh();
            }
            this.Invalidate();
            this.Cursor = Cursors.Default;
        }

        private void roate90ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                imageHandler.RotateFlip(RotateFlipType.Rotate90FlipNone).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Rotate Image 90 {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.Refresh();
            }

            this.Invalidate();

        }

        private void roate180ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                imageHandler.RotateFlip(RotateFlipType.Rotate180FlipNone).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Roate Image 180 {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.Refresh();
            }

            this.Invalidate();
        }

        private void roate270ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {


                imageHandler.RotateFlip(RotateFlipType.Rotate270FlipNone).ConfigureAwait(false).GetAwaiter().GetResult(); ;
                this.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Roate Image 270 {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.Refresh();
            }
        }
        private async void CropImageXY(int xPos, int yPos, int recW, int rech)
        {
            drawRectualToolStripMenuItem.Checked = false;
            manuelToolStripMenuItem.Checked = false;
            DrawRect = false;
            CropInsert = false;
            try
            {


                using (CropForm cpFrm = new CropForm())
                {
                    cpFrm.CropXPosition = xPos;
                    cpFrm.CropYPosition = yPos;
                    if (recW > 0)
                    {
                        cpFrm.CropWidth = recW;
                        cpFrm.CropHeight = rech;
                    }
                    else
                    {
                        cpFrm.CropWidth = imageHandler.CurrentBitmap.Width;
                        cpFrm.CropHeight = imageHandler.CurrentBitmap.Height;
                    }
                    if (cpFrm.ShowDialog() == DialogResult.OK)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        imageHandler.RestorePrevious().ConfigureAwait(false).GetAwaiter().GetResult();
                        imageHandler.DrawOutCropArea(cpFrm.CropXPosition, cpFrm.CropYPosition, cpFrm.CropWidth, cpFrm.CropHeight).ConfigureAwait(false).GetAwaiter().GetResult(); ;
                        this.Invalidate();
                        if (MessageBox.Show("Do u want to crop this area?", "ImageProcessing", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            imageHandler.Crop(cpFrm.CropXPosition, cpFrm.CropYPosition, cpFrm.CropWidth, cpFrm.CropHeight).ConfigureAwait(false).GetAwaiter().GetResult(); ;
                        }
                        else
                        {
                            imageHandler.RemoveCropAreaDraw().ConfigureAwait(false).GetAwaiter().GetResult(); ;
                        }
                        this.Invalidate();
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Cropping Image {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.Refresh();
            }
        }

        async void InsetText(int x, int y)
        {
            DrawRect = false;
            CropInsert = false;
            manualInsertToolStripMenuItem.Checked = false;
            drawLocationToolStripMenuItem.Checked = false;
            try
            {


                using (InsertTextForm itFrm = new InsertTextForm())
                {
                    itFrm.XPosition = x;
                    itFrm.YPosition = y;
                    if (itFrm.ShowDialog() == DialogResult.OK)
                    {
                        //    imageHandler.RestorePrevious();
                        imageHandler.InsertText(itFrm.DisplayText, itFrm.XPosition, itFrm.YPosition, itFrm.DisplayTextFont, itFrm.DisplayTextFontSize, itFrm.DisplayTextFontStyle, itFrm.DisplayTextForeColor1, itFrm.DisplayTextForeColor2).ConfigureAwait(false).GetAwaiter().GetResult();
                        this.Invalidate();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Insert Text {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.Refresh();
            }
        }
        async void InsertImage(int xpos, int ypos, int w = 0, int h = 0)
        {
            DrawRect = false;
            CropInsert = false;
            manuToolStripMenuItem.Checked = false;
            drawLoactionToolStripMenuItem.Checked = false;
            try
            {


                using (InsertImageForm iiFrm = new InsertImageForm())
                {
                    iiFrm.ImageWidth = w;
                    iiFrm.ImageHeight = h;
                    iiFrm.XPosition = xpos;
                    iiFrm.YPosition = ypos;
                    if (iiFrm.ShowDialog() == DialogResult.OK)
                    {
                        if ((iiFrm.ImageWidth > 0) || (iiFrm.Height > 0))
                        {
                            imageHandler.RestorePrevious().ConfigureAwait(false).GetAwaiter().GetResult();
                            imageHandler.InsertImage(iiFrm.DisplayImagePath, iiFrm.XPosition, iiFrm.YPosition,w,h).ConfigureAwait(false).GetAwaiter().GetResult();
                        }
                        else
                        { 
                            imageHandler.RestorePrevious().ConfigureAwait(false).GetAwaiter().GetResult();
                        imageHandler.InsertImage(iiFrm.DisplayImagePath, iiFrm.XPosition, iiFrm.YPosition).ConfigureAwait(false).GetAwaiter().GetResult();
                        }
                        this.Invalidate();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Inserting Image {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.Refresh();
            }
        }
        //private void iamgeToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    using (InsertImageForm iiFrm = new InsertImageForm())
        //    {
        //        iiFrm.XPosition = iiFrm.YPosition = 0;
        //        if (iiFrm.ShowDialog() == DialogResult.OK)
        //        {
        //            imageHandler.RestorePrevious().ConfigureAwait(false).GetAwaiter().GetResult();
        //            imageHandler.InsertImage(iiFrm.DisplayImagePath, iiFrm.XPosition, iiFrm.YPosition).ConfigureAwait(false).GetAwaiter().GetResult();
        //            this.Invalidate();
        //        }
        //    }
        //}

        private void flipHorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {


                imageHandler.RotateFlip(RotateFlipType.RotateNoneFlipX).ConfigureAwait(false).GetAwaiter().GetResult();
                this.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Flip Image Hor {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.Refresh();
            }
        }

        private void flipVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                imageHandler.RotateFlip(RotateFlipType.RotateNoneFlipY).ConfigureAwait(false).GetAwaiter().GetResult();
                this.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Flip Image Ver {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.Refresh();
            }
        }

        private void DisposeImageToolForms()
        {

            if (bFrm != null)
                bFrm.Dispose();

            cFrm.Dispose();
        }

        private void saveCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFormat = "CopySave";
            DisposeImageToolForms();
            this.Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFormat = "Save";
            DisposeImageToolForms();
            this.Close();
        }

        private void ToolImage_MouseDown(object sender, MouseEventArgs e)
        {

            SelPoint = e.Location;
            MouseX = SelPoint.X;
            MouseY = SelPoint.Y;
            MouseButtons mouse = e.Button;
            if (e.Button == MouseButtons.None)
            { Console.WriteLine(); }

        }

        private void ToolImage_MouseMove(object sender, MouseEventArgs e)
        {

            if ((e.Button == MouseButtons.Left) && (CropInsert))
            {
                Point p = e.Location;
                MousePoint = p;
                int x = Math.Min(SelPoint.X, p.X);
                MouseY = p.Y;
                int y = Math.Min(SelPoint.Y, p.Y);
                int w = Math.Abs(p.X - SelPoint.X);
                int h = Math.Abs(p.Y - SelPoint.Y);
                Rect = new Rectangle(x, y, w, h);
                MouseY = y;
                MouseX = x;
                DrawRect = true;


                this.Invalidate();
            }
        }

        private void ToolImage_MouseUp(object sender, MouseEventArgs e)
        {
            // if (DrawRect)
            if ((CropInsert))
            {
                try
                {


                    DrawRect = true;
                    this.Invalidate();
                    if (drawRectualToolStripMenuItem.Checked)
                    {
                        if (MessageBox.Show("Crop Image", "Crop", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            CropImageXY(Rect.X, Rect.Y, Rect.Width, Rect.Height);

                    }
                    else if (drawLocationToolStripMenuItem.Checked)
                    {
                        if (MessageBox.Show("Add Text", "Text", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)

                            InsetText(Rect.X, Rect.Y);
                    }
                    else if (drawShapInsertLocationToolStripMenuItem.Checked)
                    {
                        if (MessageBox.Show("Insert Shape", "Shape", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)

                            InsertShape(Rect.X, Rect.Y, Rect.Width, Rect.Height);
                    }
                    else if (drawNewImageSizeToolStripMenuItem.Checked)
                    {
                        if (MessageBox.Show("ReSize Image", "ReSize", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)

                            ResizeImage(Rect.Width, Rect.Height);
                    }
                    else
                    {
                        if (MessageBox.Show($"New Image Size Width:{Rect.Width} Height:{Rect.Height} at Location X:{Rect.X} Y:{Rect.Y}", "Insert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)

                            InsertImage(Rect.X, Rect.Y, 0, 0);
                        else
                            InsertImage(Rect.X, Rect.Y, Rect.Width, Rect.Height);
                    }
                    //drawLoactionToolStripMenuItem.Checked




                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error Processing Image {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Cursor = Cursors.Default;
                    this.Refresh();
                }
                drawNewImageSizeToolStripMenuItem.Checked = false;
                drawShapInsertLocationToolStripMenuItem.Checked = false;
                drawRectualToolStripMenuItem.Checked = false;
                drawRectualToolStripMenuItem.Checked = false;
                drawLoactionToolStripMenuItem.Checked = false;
                this.Invalidate();
                DrawRect = false;
                CropInsert = false;
            }


        }

        private void manuelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawRectualToolStripMenuItem.Checked = false;
            manuelToolStripMenuItem.Checked = true;
            CropInsert = false;
            CropImageXY(0, 0, 0, 0);
        }

        private void drawRectualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawRectualToolStripMenuItem.Checked = true;
            manuelToolStripMenuItem.Checked = false;
            CropInsert = true;
        }

        private void manualInsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manualInsertToolStripMenuItem.Checked = true;
            drawLocationToolStripMenuItem.Checked = false;
            CropInsert = false;
            InsetText(0, 0);
        }

        private void drawLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manualInsertToolStripMenuItem.Checked = false;
            drawLocationToolStripMenuItem.Checked = true;
            CropInsert = true;
        }

        private void manuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manuToolStripMenuItem.Checked = true;
            CropInsert = false;
            InsertImage(0, 0);
        }

        private void drawLoactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manuToolStripMenuItem.Checked = false;
            drawLoactionToolStripMenuItem.Checked = true;
            CropInsert = true;
        }
        async void InsertShape(int xPos, int yPos, int w, int h)
        {
            DrawRect = false;
            CropInsert = false;
            manualInsertShapeToolStripMenuItem.Checked = false;

            drawShapInsertLocationToolStripMenuItem.Checked = false;
            try
            {


                using (InsertShapeForm isFrm = new InsertShapeForm())
                {
                    isFrm.XPosition = xPos;
                    isFrm.YPosition = yPos;
                    isFrm.Width = w;
                    isFrm.Height = h;
                    if (isFrm.ShowDialog() == DialogResult.OK)
                    {
                        imageHandler.RestorePrevious().ConfigureAwait(false).GetAwaiter().GetResult(); ;
                        imageHandler.InsertShape(isFrm.ShapeType, isFrm.XPosition, isFrm.YPosition, isFrm.ShapeWidth, isFrm.ShapeHeight, isFrm.ShapeColor).ConfigureAwait(false).GetAwaiter().GetResult(); ;
                        this.Invalidate();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Inserting Image {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.Refresh();
            }
        }
        private void manualInsertShapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manualInsertShapeToolStripMenuItem.Checked = true;

            drawShapInsertLocationToolStripMenuItem.Checked = false;
            InsertShape(0, 0, 0, 0);
            CropInsert = false;
        }

        private void drawShapInsertLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manualInsertShapeToolStripMenuItem.Checked = false;
            drawShapInsertLocationToolStripMenuItem.Checked = true;
            CropInsert = true;

        }

        private void manualResiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manualResiseToolStripMenuItem.Checked = true;
            ResizeImage(0, 0);
        }

        private void drawNewImageSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manualResiseToolStripMenuItem.Checked = false;
            drawNewImageSizeToolStripMenuItem.Checked = true;
            CropInsert = true;

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisposeImageToolForms();
            this.Close();
        }
        private async Task CheckOpenedByName(string name)
        {
            FormCollection fc = Application.OpenForms;
            // TL.TraceLoggerInstance.TraceInformation($"Method CheckOpened form: {name}");
            int numOpenForms = fc.Count;
            for (int i = 0; i < numOpenForms; i++)
            {
                if (string.Compare(fc[i].Name, name, true) == 0)
                {
                    fc[i].Close();
                    break;
                }
            }

        }
        private async Task CheckOpened(string name)
        {
            FormCollection fc = Application.OpenForms;
            // TL.TraceLoggerInstance.TraceInformation($"Method CheckOpened form: {name}");
            int numOpenForms = fc.Count;

            foreach (var fName in name.Split(','))
            {
                for (int i = 0; i < numOpenForms; i++)
                {
                    if (string.Compare(fc[i].Name, fName, true) == 0)
                    {
                        // fc[i];
                        break;
                    }
                }
            }

        }
    }
}
