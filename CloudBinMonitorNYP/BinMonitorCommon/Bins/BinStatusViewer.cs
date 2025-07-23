using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics;

namespace BinMonitor.Common
{
    public class BinStatusViewer : UserControl
    {
        #region Static Properties

        public static Timer RefreshTimer = new Timer() 
        { Interval = (int)(TimeSpan.FromSeconds(10).TotalMilliseconds) };

        #endregion Static Properties

        #region Static Constructors

        static BinStatusViewer()
        { RefreshTimer.Start(); }

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

        public BinStatusViewer()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            BinStatusViewer.RefreshTimer.Tick += RefreshTimer_Tick;
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
            {return;}
            //Draw the background
            Rectangle topPanelRegion = new Rectangle
            (
                0 + this.Margin.Left,
                0 + this.Margin.Top,
                this.Width - this.Margin.Horizontal,
                (int)(decimal.Multiply(this.Height, .75M)) - this.Margin.Vertical
            );
            //e.Graphics.DrawRectangle(SystemPens.ActiveBorder, topPanelRegion);
            
            Color urgencyColor = SystemColors.Control;
            if (this.Bin.HasBatch)
            {
                if (this.Bin.Batch.CheckPoint1.Elapsed() == false)
                { urgencyColor = Color.LightGreen; }
                else if (this.Bin.Batch.CheckPoint2.Elapsed() == false)
                { urgencyColor = Color.Yellow; }
                else if (this.Bin.Batch.CheckPoint3.Elapsed() == false)
                { urgencyColor = Color.DarkOrange; }
                else if (this.Bin.Batch.CheckPoint4.Elapsed() == false)
                { urgencyColor = Color.Crimson; }
                else
                {urgencyColor = Color.Red;}
            }
            e.Graphics.FillRectangle(new SolidBrush(urgencyColor), topPanelRegion);

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
                
            Rectangle bottomPanelRegion = new Rectangle
            (
                topPanelRegion.Left,
                topPanelRegion.Bottom,
                this.ClientSize.Width - this.Margin.Horizontal,
                this.ClientSize.Height - this.Margin.Vertical - topPanelRegion.Height
            );

            int workflowRectangleWidth = bottomPanelRegion.Width / 2;
            int workflowRectangleHeight = bottomPanelRegion.Height;
            Size workflowRectangleSize = new System.Drawing.Size
            (
                workflowRectangleWidth,
                workflowRectangleHeight
            );

            Rectangle registrationRectangle = new Rectangle
            ( 
                bottomPanelRegion.Left,
                bottomPanelRegion.Top,
                workflowRectangleWidth,
                workflowRectangleHeight                    
            );

            Rectangle processingRectangle = new Rectangle
            (
                registrationRectangle.Right,
                registrationRectangle.Top,
                workflowRectangleWidth,
                workflowRectangleHeight
            );

            //If there is a Batch, fill in the status rectangles
            if (this.Bin.HasBatch)
            {
                Brush workflowStepCompleteBrush = new SolidBrush(this.Bin.Batch.Category.Color.Value);
                Brush workflowStepStartedBrush = new SolidBrush(SystemColors.ControlLightLight);

                if (this.Bin.Batch.Registration.HasCompleted)
                { e.Graphics.FillRectangle(workflowStepCompleteBrush, registrationRectangle); }
                else if (this.Bin.Batch.Registration.HasStarted)
                { e.Graphics.FillRectangle(workflowStepStartedBrush, registrationRectangle); }
                if (this.Bin.Batch.Processing.HasCompleted)
                { e.Graphics.FillRectangle(workflowStepCompleteBrush, processingRectangle); }
                else if (this.Bin.Batch.Processing.HasStarted)
                { e.Graphics.FillRectangle(workflowStepStartedBrush, processingRectangle); }
            }
            
            //Paint the status rectangle borders
            e.Graphics.DrawRectangle(SystemPens.ActiveBorder, registrationRectangle);
            e.Graphics.DrawRectangle(SystemPens.ActiveBorder, processingRectangle);


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
    }
}
