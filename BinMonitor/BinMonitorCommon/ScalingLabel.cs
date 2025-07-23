using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common
{
    public class ScalingLabel : UserControl
    {
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        protected Font GetScaledFont(Graphics graphics, Rectangle rectangle, Font originalFont, string value)
        {
            SizeF originalFontSize = graphics.MeasureString(this.Text, originalFont);

            float widthRatio = (rectangle.Width) / (originalFontSize.Width);
            float heightRatio = (rectangle.Height) / (originalFontSize.Height);

            float minRatio = Math.Min(widthRatio, heightRatio);

            float newFontSize = originalFont.SizeInPoints * minRatio;

            return new Font(originalFont.FontFamily, newFontSize, originalFont.Style);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(base.BackColor);

            //If there is nothing to draw, just return;
            if ((this.Width <= 0)
                || (this.Height <= 0)
                || (string.IsNullOrWhiteSpace(this.Text)))
            { return; }

            //Account for padding
            Rectangle textRectangle = new Rectangle(
                base.ClientRectangle.X
                , base.ClientRectangle.Y
                , (base.ClientRectangle.Width - base.Padding.Horizontal)
                , (base.ClientRectangle.Height - base.Padding.Vertical));

            Font scaledFont = GetScaledFont(e.Graphics, textRectangle, base.Font, this.Text);

            e.Graphics.DrawString(this.Text, scaledFont, new SolidBrush(base.ForeColor), textRectangle.Location);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.Invalidate();
            base.OnTextChanged(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.Invalidate();
            base.OnResize(e);
        }

        public ScalingLabel()
            : base()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }
    }
}
