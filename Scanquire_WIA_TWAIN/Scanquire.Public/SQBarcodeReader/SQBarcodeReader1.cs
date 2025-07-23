using DTKBarReader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EDL = EdocsUSA.Utilities.Logging;
namespace Scanquire.Public
{
    /// <summary>Default implementation of an ISQBarcodeReader</summary>
    public class SQBarcodeReader : SQBarcodeReaderBase
    {
        private BarcodeReaderEx _BarcodeReader = null;
        public BarcodeReaderEx BarcodeReader
        {
            get
            {
                if (_BarcodeReader == null)
                { _BarcodeReader = new BarcodeReaderEx(); }
                return _BarcodeReader;
            }
            set { _BarcodeReader = value; }
        }

        private SQImageRegion _ScanRegion = SQImageRegion.All;
        /// <summary>Region of the image to scan.</summary>
        public SQImageRegion ScanRegion
        {
            get { return _ScanRegion; }
            set { _ScanRegion = value; }
        }

       
       
        public override Task<Barcode[]> ReadDtkBarCode(SQImage image, CancellationToken cToken)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Reading barcodes for image:{image.LatestRevision.OriginalImageFilePath}");
            return Task.Factory.StartNew<Barcode[]>(() =>
            {
                using (SQImageEditLock editLock = image.BeginEdit())
                using (Bitmap bitmap = image.WorkingCopy.ToBitmap())
                {
                    BarcodeReader.ScanRectangle = ScanRegion.ToRectangle(bitmap);
                    return BarcodeReader.ReadFromImage(bitmap);
                }
            });
        }
    }
}
