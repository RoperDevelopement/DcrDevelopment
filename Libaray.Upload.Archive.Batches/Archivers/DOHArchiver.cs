using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edocs.Libaray.Upload.Archive.Batches.Models;
using EdocsUSA.Utilities;
using edl = EdocsUSA.Utilities.Logging;
namespace Edocs.Libaray.Upload.Archive.Batches.Models
{
    public class DOHArchiver
    {
        public string BatchId
        { get; set; }
        public string BatchDir
        { get; set; }
        public DOHArchiver(string batchID,string archiveFolder)
        {
            BatchId = batchID;
            if (!(archiveFolder.EndsWith(batchID)))
                BatchDir = UploadUtilities.CombPath(archiveFolder, batchID).ConfigureAwait(false).GetAwaiter().GetResult();
            else
                BatchDir = archiveFolder;


        }
        public DOHArchiver()
        {

        }
        public async Task UploadDOHDocuments(string archiverName)
        {
            int totRecodsRead = 0;
            int totRecordsInFile = 0;
            int totdup = 0;
            string message = string.Empty;
            edl.TraceLogger.TraceLoggerInstance.StartStopStopWatch();
          string totalTime =  edl.TraceLogger.TraceLoggerInstance.StopStopWatch();
           // Stopwatch executionTimer = Stopwatch.StartNew();
        }
    }
}
