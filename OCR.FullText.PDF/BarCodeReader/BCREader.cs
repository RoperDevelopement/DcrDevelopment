using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTKBarReader;
using Edocs.HelperUtilities;
using TL = EdocsUSA.Utilities.Logging;
namespace Edocs.OCR.FullText.PDF.BarCodeReader
{
    public sealed class BCREader
    {
        private static BCREader instance = null;
        const string AppKeyBarCodeReaderKey = "BarCodeReaderKey";

        string BarCodeReaderKey
        { get { return Utilities.GetAppConfigSetting(AppKeyBarCodeReaderKey); } }
        public static BCREader InstanceBRader
        {
            get
            {
                if (instance == null)
                {
                    instance = new BCREader();
                }
                return instance;
            }
        }

        private BCREader()
        {

        }
        public async Task<string> GetBarCodes(string bCodeFName)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting barcodes for file {bCodeFName}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Getting barcodes for file {bCodeFName}");
            string retBCodes = string.Empty;
            BarcodeReader reader = new BarcodeReader(BarCodeReaderKey);
            reader.BarcodeOrientation = BarcodeOrientationEnum.BO_All;
            reader.BarcodeTypes = BarcodeTypeEnum.BT_Codabar | BarcodeTypeEnum.BT_Code11 | BarcodeTypeEnum.BT_Code128 | BarcodeTypeEnum.BT_Code39 | BarcodeTypeEnum.BT_Code93;
            reader.ThresholdMode = ThresholdModeEnum.TM_Automatic;

            LicManager manager = reader.LicenseManager;
            reader.ScanPage = 2;
            reader.PDFReadingType = PDFReadingTypeEnum.PDF_Images;
            reader.ScanPage = 0;
            reader.QuietZoneSize = QuietZoneSizeEnum.QZ_Normal;
            //   manager.AddLicenseKey(BarCodeReaderKey);
            if (manager.IsLicensed)
            {
                Barcode[] barcodes = reader.ReadFromFile(bCodeFName);
                if (barcodes.Count() > 0)
                {
                    TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total  barcodes found {barcodes.Count()} for file {bCodeFName}");
                    TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total  barcodes found {barcodes.Count()} for file {bCodeFName}");
                    foreach (Barcode barcode in barcodes)
                    {
                        retBCodes += $"{barcode.BarcodeString} ";
                    }
                    retBCodes = retBCodes.Trim();
                }
                else
                {
                    TL.TraceLogger.TraceLoggerInstance.TraceInformation($"No  barcodes found for file {bCodeFName}");
                    TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"No barcodes found for file {bCodeFName}");
                }
            }
            else
            {
                TL.TraceLogger.TraceLoggerInstance.TraceError($"BarCode reader not License");
                TL.TraceLogger.TraceLoggerInstance.TraceError($"BarCode reader not License");
                throw new Exception("BarCode reader not License");
            }
            return retBCodes;
        }
        public async Task<string> GetBarCodes(System.Drawing.Image bCodeFName)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting barcodes for file {bCodeFName}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Getting barcodes for file {bCodeFName}");
            string retBCodes = string.Empty;
            BarcodeReader reader = new BarcodeReader(BarCodeReaderKey);
            reader.BarcodeOrientation = BarcodeOrientationEnum.BO_All;
            reader.BarcodeTypes = BarcodeTypeEnum.BT_Codabar | BarcodeTypeEnum.BT_Code11 | BarcodeTypeEnum.BT_Code128 | BarcodeTypeEnum.BT_Code39 | BarcodeTypeEnum.BT_Code93;
            reader.ThresholdMode = ThresholdModeEnum.TM_Automatic;

            LicManager manager = reader.LicenseManager;
            reader.ScanPage = 2;
            reader.PDFReadingType = PDFReadingTypeEnum.PDF_Images;
            reader.ScanPage = 0;
            reader.QuietZoneSize = QuietZoneSizeEnum.QZ_Normal;
            //   manager.AddLicenseKey(BarCodeReaderKey);
            if (manager.IsLicensed)
            {
                Barcode[] barcodes = reader.ReadFromImage(bCodeFName);
                if (barcodes.Count() > 0)
                {
                    TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total  barcodes found {barcodes.Count()} for file {bCodeFName}");
                    TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total  barcodes found {barcodes.Count()} for file {bCodeFName}");
                    foreach (Barcode barcode in barcodes)
                    {
                        retBCodes += $"{barcode.BarcodeString} ";
                    }
                    retBCodes = retBCodes.Trim();
                }
                else
                {
                    TL.TraceLogger.TraceLoggerInstance.TraceInformation($"No  barcodes found for file {bCodeFName}");
                    TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"No barcodes found for file {bCodeFName}");
                }
            }
            else
            {
                TL.TraceLogger.TraceLoggerInstance.TraceError($"BarCode reader not License");
                TL.TraceLogger.TraceLoggerInstance.TraceError($"BarCode reader not License");
                throw new Exception("BarCode reader not License");
            }
            return retBCodes;
        }
    }
}
