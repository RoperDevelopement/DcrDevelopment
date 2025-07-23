using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

using DTKBarReader;
using System.Threading.Tasks;
using System.Threading;
using Polenter.Serialization;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Scanquire.Public
{
    /// <summary>ISQCommandReader used for processing barcode base command images.</summary>
    public class SQCommandReader_Barcode : ISQCommandReader
    {
        private string _BarcodeEncoderName = "DEFAULT";
      
        /// <summary>Name of the ISQBarcodeEncoder to use for encoding/decoding.</summary>
        public virtual  string BarcodeEncoderName
        {
            get { return _BarcodeEncoderName; }
            set { _BarcodeEncoderName = value; }
        }

        private ISQBarcodeEncoder _BarcodeEncoder = null;
        /// <summary>ISQBarcodeEncoder specified by BarcodeEncoderName</summary>
        [ExcludeFromSerialization]
        public ISQBarcodeEncoder BarcodeEncoder
        {
            get 
            {
                if (_BarcodeEncoder == null)
                { _BarcodeEncoder = SQBarcodeEncoders.Instance[BarcodeEncoderName]; }
                return _BarcodeEncoder; 
            }
            set { _BarcodeEncoder = value; }
        }

        private string _BarcodeReaderName = "DEFAULT";
        /// <summary>Name of the ISQBarcodeReader to read the barcode values from the image.</summary>
        public virtual string BarcodeReaderName
        {
            get { return _BarcodeReaderName; }
            set { _BarcodeReaderName = value; }
        }

        private ISQBarcodeReader _BarcodeReader = null;
      
        /// <summary>ISQBarcodeReader specified by BarcodeReaderName</summary>
        [ExcludeFromSerialization]
        public ISQBarcodeReader BarcodeReader
        {
            get 
            {
                if (_BarcodeReader == null)
                { _BarcodeReader = SQBarcodeReaders.Instance[BarcodeReaderName]; }
                return _BarcodeReader; 
            }
            set { _BarcodeReader = value; }
        }

       
        public Task<IList<ISQCommand>> Read(SQImage image, int documentNumber, int pageNumber, CancellationToken cToken)
        {
            ETL.TraceLoggerInstance.TraceInformation($"Reading barcodes for image:{image.LatestRevision.OriginalImageFilePath}");
            return Task.Factory.StartNew<IList<ISQCommand>>(() =>
            {
                //Start a new list of commands
                List<ISQCommand> commands = new List<ISQCommand>();
                using (SQImageEditLock editLock = image.BeginEdit())
                {   
                    string[] barcodeValues = BarcodeReader.ReadText(image, cToken).Result;
                    ETL.TraceLoggerInstance.TraceInformation("Found " + barcodeValues.Length + " barcodes");

                    //For each barcode, try to parse it and add all parsed commands
                    // to the commands list.
                    foreach (string barcodeValue in barcodeValues)
                    {
                        ETL.TraceLoggerInstance.TraceInformation("Processing " + barcodeValue);
                        IEnumerable<ISQCommand> currentBarcodeCommands;
                        if (BarcodeEncoder.TryParse(barcodeValue, out currentBarcodeCommands))
                        { commands.AddRange(currentBarcodeCommands); }
                        else
                        { ETL.TraceLoggerInstance.TraceInformation("Unable to parse barcode " + barcodeValue); }
                    }
                }
                return commands;
            });
        }
    }
   
}
