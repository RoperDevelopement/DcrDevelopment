using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics;
using BinMonitor.Common;
using BinMonitor;
namespace BinMonitor.Common
{
    public class RoomStatusViewer : UserControl
    {
        #region Static Properties

        public static Timer RefreshTimer = new Timer()
        { Interval = (int)(TimeSpan.FromSeconds(10).TotalMilliseconds) };

        #endregion Static Properties

        #region Static Constructors

        static RoomStatusViewer()
        { //RefreshTimer.Start(); 
        }

        #endregion Static Constructors

        #region Properties

        private Bin _Bin = null;
        public Bin Bin
        {
            get { return _Bin; }
            set
            {
                //If there was an existing Batch, unregister the events
                if (_Bin != null)
                {
                    _Bin.BatchChanged -= this.OnBinChanged;
                    _Bin.PropertyChanged -= this.OnBinChanged;
                }

                //If there is a new value, register the events
                if (value != null)
                {
                    value.BatchChanged += this.OnBinChanged;
                    value.PropertyChanged += this.OnBinChanged;
                }

                _Bin = value;
                OnBinChanged();
            }
        }

        #endregion Properties

        #region Constructors

        public RoomStatusViewer()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            RoomStatusViewer.RefreshTimer.Tick += RefreshTimer_Tick;
        }

        #endregion Constructors

        #region Event Handlers

        void RefreshTimer_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        protected void OnBinChanged()
        { this.Invalidate(); }

        protected void OnBinChanged(object sender, EventArgs e)
        { OnBinChanged(); }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(SystemColors.Control);
            if (this.Bin == null || this.Width == 0 || this.Height == 0)
            { return; }

            //Draw the background
            Rectangle topPanelRegion = new Rectangle
            (
                0 + this.Margin.Left,
                0 + this.Margin.Top,
                this.Width - this.Margin.Horizontal,
                (int)(decimal.Multiply(this.Height, .75M)) - this.Margin.Vertical
            );
            //e.Graphics.DrawRectangle(SystemPens.ActiveBorder, topPanelRegion);




            string binIdText = Bin.Id;
            Font binIdTextFont = GetScaledFont(e.Graphics, topPanelRegion, SystemFonts.DefaultFont, binIdText);
            SizeF binIdTextSize = e.Graphics.MeasureString(binIdText, binIdTextFont);
            Rectangle binIdTextRegion = new Rectangle
            (
                topPanelRegion.X + ((topPanelRegion.Width - (int)binIdTextSize.Width) / 2)
                , topPanelRegion.Y + ((topPanelRegion.Height - (int)binIdTextSize.Height) / 2)
                , (int)binIdTextSize.Width
                , (int)binIdTextSize.Height
            );
            e.Graphics.DrawString(binIdText, binIdTextFont, SystemBrushes.ActiveCaptionText, binIdTextRegion.Location);




            //Paint the status rectangle borders
            //e.Graphics.DrawRectangle(SystemPens.ActiveBorder, registrationRectangle);
            //e.Graphics.DrawRectangle(SystemPens.ActiveBorder, processingRectangle);
            InitializeComponent();
        }

        #endregion Event Handlers

        protected Font GetScaledFont(Graphics graphics, Rectangle rectangle, Font originalFont, string value)
        {
            SizeF originalFontSize = graphics.MeasureString(value, originalFont);

            float widthRatio = (rectangle.Width) / (originalFontSize.Width);
            float heightRatio = (rectangle.Height) / (originalFontSize.Height);

            float minRatio = Math.Min(widthRatio, heightRatio);

            float newFontSize = originalFont.SizeInPoints * minRatio;

            return new Font(originalFont.FontFamily, newFontSize, originalFont.Style);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // RoomStatusViewer
            // 
            this.Name = "RoomStatusViewer";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RoomStatusViewer_Click);
            this.ResumeLayout(false);

        }
        BinMonitorForm MonitorForm = new BinMonitorForm();
        private void RoomStatusViewer_Click(object sender, MouseEventArgs e)
        {


            if (MonitorForm.IsDisposed == true)
            {
                MonitorForm = new BinMonitorForm();

            }
            MonitorForm.setId(int.Parse(Bin.Id));
            MonitorForm.Show();

            MonitorForm.Text = String.Concat("BinMonitor, Room Number: ", Bin.Id, "- e-Docs USA, Inc.");

            // this.MouseClick -= new System.Windows.Forms.MouseEventHandler(this.RoomStatusViewer_Click);
            //MessageBox.Show("TEST");
        }
    }
}
