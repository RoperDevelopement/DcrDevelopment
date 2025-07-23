using EdocsUSA.Utilities;
using Microsoft;
using Scanquire.Public;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EDL = EdocsUSA.Utilities.Logging;
namespace Scanquire.Clients.NYP
{
    public abstract class AzureCloudArchiver : SQArchiverBase
    {
        protected const string SHAREPOINT_DATETIME_FORMAT = "yyyy-MM-ddTHH:mm:ssZ";
        protected const string OPTIX_DATE_FORMAT = "yyyy-MM-dd";
        protected const string OPTIX_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        //  public string ScanStationId { get; set; }
        public string ScanStationMachineName
        {
            get { return Environment.MachineName; }
        }

        public string AzureTableName
        { get; set; }
        public string AzureSPName
        { get; set; }
        public string AzureWebApiController
        { get; set; }

        public string ComboBox
        { get; set; }
        public int DropDownPosition
        { get; set; }
        public string AzureCloudFileWriterName { get; set; }

        public ISQFileWriter SharepointFileWriter
        { get { return SQFileWriters.Instance[AzureCloudFileWriterName]; } }

        public string AzureBatchRootDir { get; set; }

        protected string CurrentSharepointBatchDir
        { get { return BatchHelper.GetBatchDir(AzureBatchRootDir, CurrentBatchId); } }

        public string AzureShareName { get; set; }

        public string AzureDataBaseName { get; set; }

        public string AzureUploadScriptPath { get; set; }

        public string SharepointUserName { get; set; }

        public string SharepointPassword { get; set; }

        public string UnprotectedSharepointPassword
        { get { return SharepointPassword; } }//Non base 64 error used to be from here, if 
                                              //if changing to encrypted password, must change

        public bool EnableAzureBatch { get; set; }

        public bool EnableAzureUpload { get; set; }

        public bool WaitForAzureUpload { get; set; }

        public bool DeleteAzureBatchOnSuccess { get; set; }

        protected string CurrentBatchId = null;



        //   protected virtual string GenerateNewBatchId()
        //   { return this.ScanStationId + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff"); }
        protected virtual string GenerateNewBatchId()
        { return Guid.NewGuid().ToString(); }



        public override async Task Send(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            if (string.IsNullOrWhiteSpace(CurrentBatchId))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("Current batch id is empyt so batch not sent");
                throw new Exception("Current batch id has not been set"); 
            }

           await base.Send(images, progress, cToken);

         

            if (EnableAzureUpload)
            {
                await Task.Factory.StartNew(() =>
                {
                    progress.Report(new ProgressEventArgs(0, 0, "Uploading to Azure Cloud"));
                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Uploading to Azure Cloud");
                  Process sharepointUploadProcess = new Process();
                    sharepointUploadProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                    sharepointUploadProcess.StartInfo.FileName = AzureUploadScriptPath;
                    string archiveFolder = System.IO.Path.GetFullPath(AzureBatchRootDir).TrimEnd(System.IO.Path.DirectorySeparatorChar);
                    archiveFolder = archiveFolder.Split(System.IO.Path.DirectorySeparatorChar).Last();
                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Starting process {AzureUploadScriptPath} with args /batchid:{CurrentBatchId} /archiver:{archiveFolder}");
                    sharepointUploadProcess.StartInfo.Arguments = ($"/batchid:{CurrentBatchId} /archiver:{archiveFolder}");
                    if (!(sharepointUploadProcess.Start()))
                    {
                        EDL.TraceLogger.TraceLoggerInstance.TraceError($"Process {AzureUploadScriptPath} with args /batchid:{CurrentBatchId} /archiver:{archiveFolder} did not start:{sharepointUploadProcess.ExitCode.ToString()}");
                        throw new OperationCanceledException("Azure Cloud upload failed with " + sharepointUploadProcess.ExitCode.ToString());
                    }

                    if (WaitForAzureUpload)
                    {
                        sharepointUploadProcess.WaitForExit();
                        if (sharepointUploadProcess.ExitCode != 0)
                        {
                            EDL.TraceLogger.TraceLoggerInstance.TraceError($"Process {AzureUploadScriptPath} with args /batchid:{CurrentBatchId} /archiver:{archiveFolder} exited with code:{sharepointUploadProcess.ExitCode.ToString()}");
                            throw new OperationCanceledException("Azure Cloud failed with " + sharepointUploadProcess.ExitCode.ToString()); 
                        }
                    }
                });
            }
        }

       

        public override IEnumerable<Task<SQImage>> AcquireFromFile(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceError("This archiver does not support acquiring from file");
            throw new NotImplementedException("This archiver does not support acquiring from file"); 
        }

        public override async Task Send(SQDocument document, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            string spFileWriterProgressCaption = "Writing azurecloud file " + documentNumber;
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"{spFileWriterProgressCaption}");
            Action<ProgressEventArgs> spFileWriterProgressAction = new Action<ProgressEventArgs>(p =>
            { progress.Report(new ProgressEventArgs(p.Current, p.Total, spFileWriterProgressCaption)); });
            Progress<ProgressEventArgs> spFileWriterProgress = new Progress<ProgressEventArgs>(spFileWriterProgressAction);
            SQFile sharepointFile = await SharepointFileWriter.Write(document, spFileWriterProgress, cToken);



            string sendFileProgressCaption = "Sending file " + documentNumber;
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"{sendFileProgressCaption}");
            Action<ProgressEventArgs> sendFileProgressAction = new Action<ProgressEventArgs>(p =>
            { progress.Report(new ProgressEventArgs(p.Current, p.Total, sendFileProgressCaption)); });
            Progress<ProgressEventArgs> sendFileProgress = new Progress<ProgressEventArgs>(sendFileProgressAction);
            await Send(sharepointFile, null, documentNumber, sendFileProgress, cToken);
        }

        public abstract Task Send(SQFile sharepointFile, SQFile optixFile, int fileNumber, IProgress<ProgressEventArgs> progress, CancellationToken cToken);

        public override Task Send(SQFile file, int fileNumber, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceError("Method not NotImplementedException send");
            throw new NotImplementedException();
        }
    }
}
