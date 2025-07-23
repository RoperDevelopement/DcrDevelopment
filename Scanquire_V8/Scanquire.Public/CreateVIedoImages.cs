using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using Microsoft;
using System.Diagnostics;
using Tesseract;
using System.IO;
using System.Configuration;
using System.Reflection;
using Scanquire.Public.Extensions;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using ETL = EdocsUSA.Utilities.Logging;
using EdocsUSA.Utilities.Controls;

namespace Scanquire.Public
{
   public class CreateVIedoImages
    {
        CreateVIedoImages() { }
        private static readonly object lockCheck = new object();
        private static CreateVIedoImages instance = null;
        public static CreateVIedoImages ConvertImagesToViedoInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockCheck)
                    {
                        if (instance == null)
                        {
                            instance = new CreateVIedoImages();
                        }
                    }
                }
                return instance;
            }
        }

        public async Task CreateViedo(IEnumerable<SQImage> images, CancellationToken cToken,string imageFolder,string viedosImagesExe)
        {
            Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(imageFolder);
            Edocs_Utilities.EdocsUtilitiesInstance.DeleteFiles(imageFolder);
            
            
            foreach(SQImage sQImage in images)
            {
                //string saveFolder = Path.Combine(imageFolder,$"{Path.GetFileName(sQImage.WorkingCopy)}")
                sQImage.BeginEdit();
              using(Bitmap bitmap = sQImage.LatestRevision.GetOriginalImageBitmap())
                {
                    string saveFolder = Path.Combine(imageFolder, $"{Path.GetFileNameWithoutExtension(sQImage.LatestRevision.OriginalImageFilePath)}.png");
                    bitmap.Save(saveFolder, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            string convertApp = Path.Combine(SettingsManager.ApplicationDirectory,viedosImagesExe);

            Edocs_Utilities.EdocsUtilitiesInstance.StartProcess(convertApp, "-iv images", true);
        }
    }
}
