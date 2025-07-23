using EdocsUSA.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Scanquire.Public
{
    public sealed class BarcodeRenderers : SerializedObjectDictionary<Code128BarcodeRenderer>
    {
        public override string DirectoryPath
        {
            get
            { return Path.Combine(SettingsManager.ApplicationSettingsDirectoryPath, "Barcode Renderers"); }
        }

        static readonly BarcodeRenderers _Instance = new BarcodeRenderers();
        public static BarcodeRenderers Instance
        { get { return _Instance; } }

        static BarcodeRenderers()
        {
        }
    }
}