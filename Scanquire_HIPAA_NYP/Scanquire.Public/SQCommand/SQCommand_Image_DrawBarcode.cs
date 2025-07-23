using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using EdocsUSA.Utilities.Logging;
namespace Scanquire.Public
{
    /// <summary>A command requesting a barcode to be drawn on the specified image.</summary>
    public class SQCommand_Image_DrawBarcode : ISQCommand_Image
    {
        private readonly string _Value;
        /// <summary>The encoded barcode text to apply.</summary>
        public string Value 
        { get { return _Value; } }

        private readonly string _Caption;
        /// <summary>The caption to apply to the drawn barcode.</summary>
        public string Caption
        { get { return _Caption; } }

        public SQCommand_Image_DrawBarcode(string value, string caption)
        {
            TraceLogger.TraceLoggerInstance.TraceInformation($"Draw barcode value:{value} caption:{caption}");
            this._Value = value;
            this._Caption = caption;
        }
    }
}
