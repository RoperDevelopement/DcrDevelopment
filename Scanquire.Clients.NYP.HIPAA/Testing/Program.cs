using Scanquire.Clients.NYP;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

using Scanquire.Public;

namespace Testing
{
    class Program
    {
        public void SerializeArchiver(ISQArchiver archiver, string name)
        { SQArchivers.Instance[name] = archiver; }

        public void SerializeArchiver(Type t, string name)
        {
            ISQArchiver archiver = (ISQArchiver)Activator.CreateInstance(t);
            SerializeArchiver(archiver, name);
        }

     

        static void Main(string[] args)
        {
            SendoutArchiver sendoutArchiver = new SendoutArchiver();
            sendoutArchiver.ScanStationId = "CS";
            sendoutArchiver.OptixFileWriterName = "TIFF - G4";
            sendoutArchiver.OptixBatchRootDir = @"C:\Archives\NYP\Sendout\Optix Batches";
            sendoutArchiver.OptixScreenName = "Sendout";
            sendoutArchiver.OptixUploadScriptPath = "";
            sendoutArchiver.EnableOptixUpload = false;
            sendoutArchiver.WaitForOptixUpload = true;
            sendoutArchiver.DeleteOptixBatchOnSuccess = true;
            sendoutArchiver.SharepointFileWriterName = "PDF";
            sendoutArchiver.SharepointBatchRootDir = @"C:\Archives\NYP\Sendout\Sharepoint Batches";
            sendoutArchiver.SharepointSiteUrl = @"";
            sendoutArchiver.SharepointLibraryName = "Sendout";
            sendoutArchiver.SharepointUploadScriptPath = "";
            sendoutArchiver.EnableSharepointUpload = true;
            sendoutArchiver.WaitForSharepointUpload = false;
            sendoutArchiver.DeleteSharepointBatchOnSuccess = false;
            sendoutArchiver.SharepointUserName = "";
            sendoutArchiver.SharepointPassword = "";
            try
            {
                Scanquire.Public.SQArchivers.Instance["Sendout"] = sendoutArchiver;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
          
        }
    }
}
