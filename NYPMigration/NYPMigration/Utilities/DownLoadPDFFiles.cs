using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS = Edocs.Azure.Blob.Storage.AzureBlobStorage;
using edl = EdocsUSA.Utilities.Logging;
namespace NYPMigration.Utilities
{
   public class DownLoadPDFFiles
    {
        public DownLoadPDFFiles()
        {
            BS.BlobStorageInstance.AzureBlobAccountKey = PropertiesConst.PropertiesConstInstance.AzureBlobAccountKey;
            BS.BlobStorageInstance.AzureBlobStorageConnectionString = PropertiesConst.PropertiesConstInstance.AzureBlobStorageConnectionString;
            BS.BlobStorageInstance.AzureBlobAccountName = PropertiesConst.PropertiesConstInstance.AzureBlobAccountName;
        }
       public async Task DownloadFile(string url,string fileName,string saveFolder)
        {
            try
            {
            //    EdocsUSA.Utilities.Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(saveFolder);
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Downloading file for url {url} filename {fileName} save folder {saveFolder}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Downloading file for url {url} filename {fileName} save folder {saveFolder}");
            byte[] labReqsFname = BS.BlobStorageInstance.DownloadFileAzureBlob(fileName, url,saveFolder).ConfigureAwait(false).GetAwaiter().GetResult();
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Saving file {saveFolder}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Saving file {saveFolder}");
                System.IO.File.WriteAllBytes(saveFolder, labReqsFname);
            }
            catch(Exception ex)
            {
                PropertiesConst.PropertiesConstInstance.UpdateErrors($"Downloading file for url {url} filename {fileName} save folder {saveFolder} {ex.Message}");
                
            }
            }
    }
}
