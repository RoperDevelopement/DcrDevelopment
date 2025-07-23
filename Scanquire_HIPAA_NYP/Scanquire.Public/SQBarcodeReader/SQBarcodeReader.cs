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
using System.IO;
using EDL = EdocsUSA.Utilities.Logging;
using EdocsUSA.Utilities;


using Newtonsoft.Json;
namespace Scanquire.Public
{
    /// <summary>Default implementation of an ISQBarcodeReader</summary>
    public class SQBarcodeReader : SQBarcodeReaderBase
    {
        private readonly string DefaultJsonBarcodeFile = "DefaultBarCodeSettting.json";
        private string dtkLicenseKey = "WREXXH7GD92SZLPNFJGK";
        private BarcodeReaderEx _BarcodeReader = null;
        public string JsonFileName
        { get; set; }

        // private string DefaultJsonBarcodeFile
        // { get { return defaultJsonBarcodeFile; }
        //   set { defaultJsonBarcodeFile = value; }
        // }
        private string DtkLicenseKey
        {
            get { return dtkLicenseKey; }
            set { dtkLicenseKey = value; }
        }

        private void GetReaderSettings()
        {
            if (!(string.IsNullOrWhiteSpace(JsonFileName)))
            {
                try
                {
                    if (!(string.IsNullOrWhiteSpace(JsonFileName)))
                    {
                        EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting barcodes settings from json file:{JsonFileName}");
                        JsonFileName = Path.Combine(SettingsManager.ApplicationSettingsDirectoryPath, $"Barcode Readers\\{JsonFileName}");
                        Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists(JsonFileName, false);
                        string barcodeSettings = System.IO.File.ReadAllText(JsonFileName);
                        BarcodeReader barcode = JsonConvert.DeserializeObject<BarcodeReader>(barcodeSettings);
                        _BarcodeReader.BarcodesToRead = barcode.BarcodesToRead;
                        _BarcodeReader.LicenseManager.AddLicenseKey(DtkLicenseKey);
                        _BarcodeReader.ScanInterval = barcode.ScanInterval;
                        _BarcodeReader.Threshold = barcode.Threshold;
                        _BarcodeReader.ThresholdCount = barcode.ThresholdCount;
                        _BarcodeReader.ScanPage = barcode.ScanPage;
                        _BarcodeReader.ThresholdStep = barcode.ThresholdStep;
                        _BarcodeReader.QuietZoneSize = QuietZoneSizeEnum.QZ_Normal;
                        _BarcodeReader.BarcodeOrientation = BarcodeOrientationEnum.BO_All;
                        _BarcodeReader.ThresholdMode = ThresholdModeEnum.TM_Automatic;
                        _BarcodeReader.Code11Checksum = bool.Parse(barcode.Code11Checksum.ToString());
                        _BarcodeReader.Code39Checksum = bool.Parse(barcode.Code39Checksum.ToString());
                        _BarcodeReader.Code39NoStartStop = bool.Parse(barcode.Code39NoStartStop.ToString());
                        _BarcodeReader.Code93Checksum = bool.Parse(barcode.Code93Checksum.ToString());
                        _BarcodeReader.ConvertUPCEtoUPCA = bool.Parse(barcode.ConvertUPCEtoUPCA.ToString());
                        _BarcodeReader.I2of5Checksum = bool.Parse(barcode.I2of5Checksum.ToString());
                        _BarcodeReader.ImageDespeckle = barcode.ImageDespeckle;
                        _BarcodeReader.ImageDilate = barcode.ImageDilate;
                        _BarcodeReader.ImageErode = barcode.ImageErode;
                        _BarcodeReader.ImageInvert = barcode.ImageInvert;
                        _BarcodeReader.ImageSharp = barcode.ImageSharp;
                        _BarcodeReader.QRCodeRequiredECCLevel = barcode.QRCodeRequiredECCLevel;
                        _BarcodeReader.BarcodeTypes = BarcodeTypeEnum.BT_All;
                        
                        if (Enum.TryParse<BarcodeTypeEnum>(barcode.BarcodeTypes.ToString(), true, out BarcodeTypeEnum barcodeTypeEnum))
                        {
                            _BarcodeReader.BarcodeTypes = barcodeTypeEnum;
                        }

                        if (Enum.TryParse<QuietZoneSizeEnum>(barcode.QuietZoneSize.ToString(),true, out QuietZoneSizeEnum quietZoneSizeEnum))
                        {
                            _BarcodeReader.QuietZoneSize = quietZoneSizeEnum;
                        }
                        if (Enum.TryParse<BarcodeOrientationEnum>(barcode.BarcodeOrientation.ToString(),true, out BarcodeOrientationEnum barcodeOrientationEnum))
                        {
                            _BarcodeReader.BarcodeOrientation = barcodeOrientationEnum;
                        }
                        if (Enum.TryParse<ThresholdModeEnum>(barcode.BarcodeOrientation.ToString(),true, out ThresholdModeEnum thresholdModeEnum))
                        {
                            _BarcodeReader.ThresholdMode = thresholdModeEnum;
                        }
                        JsonFileName = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    EDL.TraceLogger.TraceLoggerInstance.TraceError($"Getting barcodes settings from json file:{JsonFileName} {ex.Message}");
                    throw new OperationCanceledException(ex.Message);
                }
            }

        }
        public BarcodeReaderEx BarcodeReader
        {
            get
            {
                if (_BarcodeReader == null)
                {
                    EDL.TraceLogger.TraceLoggerInstance.TraceWarning("Barcode reader is null and should not be");
                    _BarcodeReader = new BarcodeReaderEx();
                    JsonFileName = DefaultJsonBarcodeFile;
                }
                GetReaderSettings();
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



        public override Task<Barcode[]> Read(SQImage image, CancellationToken cToken)
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
