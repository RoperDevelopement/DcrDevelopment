using System.IO;
using System.Configuration;
using System.Reflection;
using Scanquire.Public.Extensions;
using System.Threading.Tasks;
using System.Drawing;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using ETL = EdocsUSA.Utilities.Logging;
using WIA;
using System.Collections.Generic;

namespace Scanquire.Public
{
   public class PrintImagesWizard
    {

       
        private static readonly object lockCheck = new object();
        private static PrintImagesWizard instance = null;
        PrintImagesWizard() { }
        public static PrintImagesWizard PrintImagesWizardInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockCheck)
                    {
                        if (instance == null)
                        {
                            instance = new PrintImagesWizard();
                        }
                    }
                }
                return instance;
            }
        }
       
        public async Task PrintMulitImages(IEnumerable<SQImage> images,string imageSaveFolder)
        {
            Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(imageSaveFolder);
            Edocs_Utilities.EdocsUtilitiesInstance.DeleteFiles(imageSaveFolder);
            Vector printWizardVector = new Vector();
            foreach (SQImage sQImage in images)
            {
                //string saveFolder = Path.Combine(imageFolder,$"{Path.GetFileName(sQImage.WorkingCopy)}")
                sQImage.BeginEdit();
                using (Bitmap bitmap = sQImage.LatestRevision.GetOriginalImageBitmap())
                {
                    string saveFolder = Path.Combine(imageSaveFolder, $"{Path.GetFileNameWithoutExtension(sQImage.LatestRevision.OriginalImageFilePath)}.png");
                    bitmap.Save(saveFolder, System.Drawing.Imaging.ImageFormat.Png);
                    printWizardVector.Add(saveFolder);
                }
            }
            if(printWizardVector.Count > 0)
            {
                ICommonDialog wiaCommonDialog = new WIA.CommonDialog();
                
                    wiaCommonDialog.ShowPhotoPrintingWizard(printWizardVector);
                printWizardVector.Clear();
                
                Edocs_Utilities.EdocsUtilitiesInstance.DeleteFiles(imageSaveFolder);
            }
            
        }
    }
}
