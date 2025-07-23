using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ED = EdocsUSA.Utilities;
using Edocs.Send.Emails;
namespace Edocs.DOH.Upload.Report
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            try
            { 
            GetInputArgs(args);
            }
            catch(Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail(Send_Emails.EmailInstance.EmailAddressError, $"Error Running DOH Upload Report {ex.Message}", "Error Running DOH Report", string.Empty, false, "Error Running DOH Report");
            }
        }
        private static void GetInputArgs(string[] args)
        {
            
           Utilities_Const.UtilityConstInstance.SbErrors = new StringBuilder();
         //   Send_Emails.EmailInstance.SendEmail(Send_Emails.EmailInstance.EmailAddressError, $"test", "Error Running DOH Report", string.Empty, false, "Error Running DOH Report");
            try
            {
                if (args.Length > 5)
                {
                    string invArgs = "Invalid args entered";
                    foreach (string invaild in args)
                    {
                        invArgs = $"{invArgs} {invaild}";
                    }
                    invArgs = $"{invArgs} run Edocs.LabReqs.Reports.exe /? for usage";
                    Console.WriteLine(invArgs);
                    throw new Exception(invArgs);
                }


                foreach (string inputArgs in args)
                {
                    // edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Looking for input arg:{inputArgs}");
                    if (inputArgs.StartsWith(Utilities_Const.UtilityConstInstance.ArgsDateAdd, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Utilities_Const.UtilityConstInstance.DateAdd =int.Parse(inputArgs.Substring(Utilities_Const.UtilityConstInstance.ArgsDateAdd.Length));
                       


                    }
                    
                        else if (inputArgs.StartsWith(Utilities_Const.UtilityConstInstance.ArgsRunUpLoadReport, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Utilities_Const.UtilityConstInstance.RunUploadReport = bool.Parse(inputArgs.Substring(Utilities_Const.UtilityConstInstance.ArgsRunUpLoadReport.Length));
                    }
                    else if (inputArgs.StartsWith(Utilities_Const.UtilityConstInstance.ArgsRunDate, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Utilities_Const.UtilityConstInstance.ReportDate = DateTime.Parse(inputArgs.Substring(Utilities_Const.UtilityConstInstance.ArgsRunDate.Length));
                    }
                    else if (inputArgs.StartsWith(Utilities_Const.UtilityConstInstance.ArgsReportFolder, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Utilities_Const.UtilityConstInstance.ReportFolder = inputArgs.Substring(Utilities_Const.UtilityConstInstance.ArgsReportFolder.Length);
                    }

                    else if (inputArgs.StartsWith(Utilities_Const.UtilityConstInstance.ArgsReportName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Utilities_Const.UtilityConstInstance.ReportName = inputArgs.Substring(Utilities_Const.UtilityConstInstance.ArgsReportName.Length);
                        


                    }
                    else if (inputArgs.StartsWith(Utilities_Const.UtilityConstInstance.ArgsEmailReport, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Utilities_Const.UtilityConstInstance.EmailReport = bool.Parse(inputArgs.Substring(Utilities_Const.UtilityConstInstance.ArgsEmailReport.Length));



                    }
                    else if (inputArgs.StartsWith(Utilities_Const.UtilityConstInstance.ArgsLocalSqlServer, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Utilities_Const.UtilityConstInstance.AzureSqlServer = bool.Parse(inputArgs.Substring(Utilities_Const.UtilityConstInstance.ArgsLocalSqlServer.Length));



                    }
                    else
                    {
                        throw new Exception($"Invlaid arg {inputArgs}");

                    }
                    
                }
               
                Utilities_Const.UtilityConstInstance.SqlServerConnStr = Utilities_Const.UtilityConstInstance.GetServerConnection(Utilities_Const.UtilityConstInstance.AzureSqlServer).ConfigureAwait(false).GetAwaiter().GetResult();
            if(Utilities_Const.UtilityConstInstance.RunUploadReport)
                {
                       UpladedDOHDocuments upladedDOHDocuments = new UpladedDOHDocuments();
                       upladedDOHDocuments.GetUploadedDocuments().ConfigureAwait(false).GetAwaiter().GetResult();
                }

                if (Utilities_Const.UtilityConstInstance.RunJsonCompare)
                       DohCompareJsaonFiles.CompJsonInstance.CompareJsonFiles().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                string message = $"Error Running DOH Upload Report {ex.Message}";
                Send_Emails.EmailInstance.SendEmail(Send_Emails.EmailInstance.EmailAddressError, $"{message}", "Error Running DOH Report", string.Empty, false, "Error Running DOH Report");
                // SEmails(message, true, string.Empty, false).ConfigureAwait(false).GetAwaiter().GetResult();


            }



        }
    }
}
