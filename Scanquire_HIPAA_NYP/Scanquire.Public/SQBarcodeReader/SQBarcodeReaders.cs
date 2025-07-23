using EdocsUSA.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DTKBarReader;
 
namespace Scanquire.Public
{
    /// <summary>
    /// Singleton collection of all avilable ISQBarcodeReaders
    /// </summary>
    public sealed class SQBarcodeReaders : SerializedObjectDictionary<ISQBarcodeReader>
    {
      //  public string JsonFileName
    //    {
     //       get; set;
    //    }
        public override string DirectoryPath
        {
            get
            { return Path.Combine(SettingsManager.ApplicationSettingsDirectoryPath, "Barcode Readers"); }
        }

        static readonly SQBarcodeReaders _Instance = new SQBarcodeReaders();
        public static SQBarcodeReaders Instance
        { get { return _Instance; } }

        static SQBarcodeReaders()
        {
        }
    }
}

