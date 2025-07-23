using EdocsUSA.Utilities;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using EdocsUSA.Utilities.Logging;
namespace Scanquire.Public
{
    /// <summary>A command specifying a text value to draw onto a specified image.</summary>
	public class SQCommand_Image_DrawText : ISQCommand_Image
	{
        private readonly string _Text = string.Empty;
        /// <summary>The text value to draw.</summary>
        public string Text { get { return _Text; } }

        private readonly Brush _Brush = new SolidBrush(Color.Black);
        /// <summary>The brush to use when drawing the text value.</summary>
        public Brush Brush 
        { get { return _Brush; } }

        private readonly Font _Font = new Font(GenericFontFamilies.SansSerif.ToString(), ImageTools.POINTS_PER_QUARTER_INCH, FontStyle.Regular, GraphicsUnit.Point);
        /// <summary>The font to use when drawing the text value.</summary>
        public Font Font 
        { get { return _Font; } }

        public SQCommand_Image_DrawText(string text, Font font = null, Brush brush = null)
        {
            this._Text = text;
            if (font != null)
            { this._Font = font; }
            if (brush != null)
            { this._Brush = brush; }
            TraceLogger.TraceLoggerInstance.TraceInformation($"Image drawing text:{text}");
        }
	}
}
