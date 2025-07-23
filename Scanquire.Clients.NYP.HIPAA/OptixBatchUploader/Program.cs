using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EdocsUSA.OptixTools;
using Scanquire.Clients.NYP;
using System.Diagnostics;
using System.IO;
using EdocsUSA.Utilities.Interop;

namespace OptixBatchUploader
{
    class Program
    {
        static Settings.Batch BatchSettings = Settings.Batch.Default;

        static void Main(string[] args)
        {
            
            try
            {
                Trace.Listeners.Add(new ConsoleTraceListener());

                //Process the command line arguments
                string batchDir = null;

                foreach (string arg in args)
                {
                    if (arg.StartsWith("/batchdir:", StringComparison.OrdinalIgnoreCase))
                    { batchDir = arg.Substring("/batchdir:".Length); }
                }

                //If any command line args were ommited, flash the window, then prompt for entry
                if (string.IsNullOrWhiteSpace(batchDir))
                { FlashWindow.Flash(FlashWindow.FLASHW_TIMERNOFG); }

                if (string.IsNullOrWhiteSpace(batchDir))
                {
                    Console.WriteLine("Batch Directory:");
                    batchDir = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(batchDir))
                    { throw new ArgumentNullException("BatchId", "Batch ID is required"); }
                }

                Trace.TraceInformation("Processing input dir " + batchDir);
                if (Directory.Exists(batchDir) == false)
                { throw new DirectoryNotFoundException("Cannot find batch directory at " + batchDir); }

                Trace.TraceInformation("Reading batch settings");
                OptixBatchSettings batchSettings = OptixBatchHelper.ReadSettings(batchDir);

                Trace.TraceInformation("Reading batch records");
                OptixBatchRecord[] batchRecords = OptixBatchHelper.ReadRecords(batchDir).ToArray();
                Trace.TraceInformation("Processing " + batchRecords.Length + " batch records");

                ExtendedOptixInstance optix = new ExtendedOptixInstance();
                optix.SetFilesystemScreen(batchSettings.ScreenName, OptixEnums.OptixScreenType.sql.ToString());
                foreach (OptixBatchRecord batchRecord in batchRecords)
                {
                    string batchRecordFilePath = Path.Combine(batchDir, batchRecord.FileName);
                    Trace.TraceInformation("Uploading " + batchRecord.FileName);
                    optix.OpenFile(batchRecordFilePath);
                    optix.ConvertColl();
                    
                    //Clear the modified flag of the primary collection so we can close it without affecting anything else.
                    optix.SetModifiedFlag(0);
                    string windowName = optix.GetActiveWindowName();
                    OptixFile newCollectionFile = optix.StoreActiveObject2(batchSettings.ScreenName, OptixEnums.OptixScreenType.sql);
                    Dictionary<string, string> indexFields = new Dictionary<string, string>();
                    //Apply the common fields metadata
                    foreach (KeyValuePair<string, string> field in batchSettings.CommonFields)
                    { indexFields.Add(field.Key, field.Value); }

                    //Apply the record specific metadata
                    foreach (KeyValuePair<string, string> field in batchRecord.Fields)
                    { indexFields.Add(field.Key, field.Value); }

                    optix.AddScreenRecord(batchSettings.ScreenName, OptixEnums.OptixScreenType.sql, 1, indexFields, newCollectionFile, false, false);
                    optix.ActivateWindowName(windowName);
                    optix.CloseActiveWindow();
                }

                if (BatchSettings.DeleteOnSuccess == true)
                {
                    Trace.TraceInformation("Deleting {0}", batchDir);
                    Directory.Delete(batchDir, true);
                }
                else
                { Trace.TraceInformation("Not deleting batch folder"); }

                Environment.ExitCode = 0;
                Trace.TraceInformation("Process Complete");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                FlashWindow.Flash(FlashWindow.FLASHW_TIMERNOFG);
                Console.WriteLine("Press any key to continue");
                Console.ReadLine();
                Environment.ExitCode = -1;
            }
        }
    }
}
