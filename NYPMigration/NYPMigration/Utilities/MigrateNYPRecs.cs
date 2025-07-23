using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using NYPMigration.ProcessRecords;
using edl = EdocsUSA.Utilities.Logging;
namespace NYPMigration.Utilities
{
    public class MigrateNYPRecs
    {
        public async Task GetInputArgs(string[] args)
        {
            Thread keyListener = new Thread(() =>
            {
                Console.ReadKey(true); // Wait for a key press
                Environment.Exit(0); // Exit the application
            });

            keyListener.Start();

            string processNYPRecs = string.Empty;
            EdocsUSA.Utilities.Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(PropertiesConst.PropertiesConstInstance.LabReqsJasonFolder);
            PropertiesConst.PropertiesConstInstance.SQLServer = PropertiesConst.PropertiesConstInstance.AzureSqlServer;
            foreach (string inputArgs in args)
            {
                if (inputArgs.StartsWith(PropertiesConst.PropertiesConstInstance.ArgMerg, StringComparison.OrdinalIgnoreCase))
                {
                    processNYPRecs = inputArgs.Substring(PropertiesConst.PropertiesConstInstance.ArgMerg.Length);
                }
                else if (inputArgs.StartsWith(PropertiesConst.PropertiesConstInstance.ArgLocalSqlServer, StringComparison.OrdinalIgnoreCase))
                {
                    PropertiesConst.PropertiesConstInstance.SQLServer = PropertiesConst.PropertiesConstInstance.LocalSqlSerever;
                }
                else
                    throw new Exception($"Invalid arg {inputArgs}");
            }
            if (string.Compare(processNYPRecs.ToLower(), PropertiesConst.PropertiesConstInstance.ArgLabRecs.ToLower()) == 0)
            {
                ProcessLabReqs().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else if (string.Compare(processNYPRecs.ToLower(), PropertiesConst.PropertiesConstInstance.ArgPatID.ToLower()) == 0)
            {
                NYPRecords.NYPRecordsInstance.GetLabReqsPatientID().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else if (string.Compare(processNYPRecs, PropertiesConst.PropertiesConstInstance.ArgCovLr, true) == 0)
            {
                ProcessCovidLabReqs().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else if (string.Compare(processNYPRecs, PropertiesConst.PropertiesConstInstance.ArgML, true) == 0)
            {
                NYPRecords.NYPRecordsInstance.GetMaintenanceLogs().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else if (string.Compare(processNYPRecs, PropertiesConst.PropertiesConstInstance.ArgMLS, true) == 0)
            {
                processNYPRecs = PropertiesConst.PropertiesConstInstance.ArgMLS;
            }

            else if (string.Compare(processNYPRecs, PropertiesConst.PropertiesConstInstance.ArgMPF, true) == 0)
            {
                processNYPRecs = PropertiesConst.PropertiesConstInstance.ArgMPF;
            }
            else if (string.Compare(processNYPRecs, PropertiesConst.PropertiesConstInstance.ArgMPL, true) == 0)
            {
                Console.WriteLine();
            }
            else if (string.Compare(processNYPRecs, PropertiesConst.PropertiesConstInstance.ArgSOPS, true) == 0)
            {
                processNYPRecs = PropertiesConst.PropertiesConstInstance.ArgSOPS;
            }
            else if (string.Compare(processNYPRecs, PropertiesConst.PropertiesConstInstance.ArgSOPLC, true) == 0)
            {
                processNYPRecs = PropertiesConst.PropertiesConstInstance.ArgSOPLC;
            }
            else if (string.Compare(processNYPRecs, PropertiesConst.PropertiesConstInstance.ArgSOR, true) == 0)
            {
                processNYPRecs = PropertiesConst.PropertiesConstInstance.SOR;
            }
            else if (string.Compare(processNYPRecs, PropertiesConst.PropertiesConstInstance.SR, true) == 0)
            {
                processNYPRecs = PropertiesConst.PropertiesConstInstance.SR;
            }
            else if (string.Compare(processNYPRecs, PropertiesConst.PropertiesConstInstance.Doh, true) == 0)
            {
                NYPRecords.NYPRecordsInstance.GetDOH().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else
                throw new Exception($"Method not defined");

        }
        private async Task ProcessLabReqs()
        {
            int totalErrorsFound = 0;
            bool done = false;
            while (!(done))
            {

                totalErrorsFound = totalErrorsFound + PropertiesConst.PropertiesConstInstance.TotalErrors;
                done = NYPRecords.NYPRecordsInstance.GetNypLabReqs().ConfigureAwait(false).GetAwaiter().GetResult();
                if ((!(done)))
                {
                    Console.Clear();
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total labrecs processed {PropertiesConst.PropertiesConstInstance.TotalLabRecsProcessed} Total Errors Found {totalErrorsFound}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Thread sleeping for {PropertiesConst.PropertiesConstInstance.ThreadBeforeLabReqs} secs Press any key to stop process..");
                    System.Threading.Thread.Sleep(PropertiesConst.PropertiesConstInstance.ThreadBeforeLabReqs);
                    Console.Clear();
                }

            }
        }
        private async Task ProcessCovidLabReqs()
        {
            int totalErrorsFound = 0;
            bool done = false;
            while (!(done))
            {

                totalErrorsFound = totalErrorsFound + PropertiesConst.PropertiesConstInstance.TotalErrors;
                done = NYPRecords.NYPRecordsInstance.GetCovidLabReqs().ConfigureAwait(false).GetAwaiter().GetResult();
                if ((!(done)))
                {
                    Console.Clear();
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total CovidLR processed {PropertiesConst.PropertiesConstInstance.TotalLabRecsProcessed} Total Errors Found {totalErrorsFound}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Thread sleeping for {PropertiesConst.PropertiesConstInstance.ThreadBeforeLabReqs} secs Press any key to stop process..");
                    System.Threading.Thread.Sleep(PropertiesConst.PropertiesConstInstance.ThreadBeforeLabReqs);
                    Console.Clear();
                }

            }
        }
    }
}