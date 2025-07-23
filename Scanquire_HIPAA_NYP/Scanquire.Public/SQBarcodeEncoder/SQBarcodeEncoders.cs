using EdocsUSA.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Scanquire.Public
{
    /// <summary>Singleton collection of available SQBarcodeEncoders</summary>
    public sealed class SQBarcodeEncoders : SerializedObjectDictionary<ISQBarcodeEncoder>
    {
        public override string DirectoryPath
        {
            get
            { return Path.Combine(SettingsManager.ApplicationSettingsDirectoryPath, "Barcode Encoders"); }
        }

        static readonly SQBarcodeEncoders _Instance = new SQBarcodeEncoders();
        public static SQBarcodeEncoders Instance
        { get { return _Instance; } }

        static SQBarcodeEncoders()
        {
        }        
    }
}