using Edocs.Report.LabReqs.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Edocs.HelperUtilities;
using SE = Edocs.Send.Emails.Send_Emails;
namespace Edocs.Report.LabReqs
{
    class LabReqsRep
    {
        private static StringBuilder SBHtmlFile
        { get; set; }
        private static StringBuilder SBCsvFile
        { get; set; }
        private static StringBuilder SBErrors
        { get; set; }
        static void Main(string[] args)
        {
            GetInputArgs(args);
        }
        private static void GetInputArgs(string[] args)
        {
            string jsonFolder = string.Empty;
            SBErrors = new StringBuilder();
            int dateAdd = int.MinValue;
            DateTime scanStDate = Utilities.StrToDateTime("1980/01/01");
            DateTime scanEDate = Utilities.StrToDateTime("1980/01/01"); ;
            try
            {


                foreach (string inputArgs in args)
                {
                    // edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Looking for input arg:{inputArgs}");
                    if (inputArgs.StartsWith(LabReqRepConst.ArgsDateAdd, StringComparison.InvariantCultureIgnoreCase))
                    {
                        dateAdd = Utilities.ParseInt(inputArgs.Substring(LabReqRepConst.ArgsDateAdd.Length));
                        //   edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Processing arg:{inputArgs} for folders");
                        //   edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Processing arg:{inputArgs} for folders");
                        //   UploadBatchesByFolder(inputArgs.Substring(ArgFolder.Length)).ConfigureAwait(false).GetAwaiter().GetResult();

                        continue;

                    }

                    else if (inputArgs.StartsWith(LabReqRepConst.ArgsScanStDate, StringComparison.InvariantCultureIgnoreCase))
                    {
                        scanStDate = Utilities.StrToDateTime(inputArgs.Substring(LabReqRepConst.ArgsScanStDate.Length));
                        //   edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Processing arg:{inputArgs} for folders");
                        //   edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Processing arg:{inputArgs} for folders");
                        //   UploadBatchesByFolder(inputArgs.Substring(ArgFolder.Length)).ConfigureAwait(false).GetAwaiter().GetResult();

                        continue;

                    }
                    else if (inputArgs.StartsWith(LabReqRepConst.ArgsScanEndDate, StringComparison.InvariantCultureIgnoreCase))
                    {
                        scanEDate = Utilities.StrToDateTime(inputArgs.Substring(LabReqRepConst.ArgsScanEndDate.Length));
                        //   edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Processing arg:{inputArgs} for folders");
                        //   edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Processing arg:{inputArgs} for folders");
                        //   UploadBatchesByFolder(inputArgs.Substring(ArgFolder.Length)).ConfigureAwait(false).GetAwaiter().GetResult();

                        continue;

                    }
                    else
                    {
                        throw new Exception($"Invald arg {inputArgs}");
                        
                    }

                }
                
                GetScannStEndDate(dateAdd, ref scanStDate, ref scanEDate);
                GenerateReport(scanStDate, scanEDate).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                SBErrors.AppendLine($"Error running Scanned LabReqs report error message {ex.Message}");
                SEmail(null, 0, DateTime.Now.ToString("MM-dd-yyyy"), DateTime.Now.ToString("MM-dd-yyyy")).ConfigureAwait(false).GetAwaiter().GetResult();
            }

        }
        static void GetScannStEndDate(int dateAdd,ref DateTime scanStDate,ref DateTime scanEndDate )
        {
            Console.WriteLine($"Getting date range for date add {dateAdd}");
            if(dateAdd > int.MinValue)
            {
                if ((scanStDate.Year != 1980) && (scanEndDate.Year != 1980))
                    throw new Exception($"Invalid args Cannot have scan start date {scanStDate} and scan end date {scanEndDate}");
                if(dateAdd == 0)
                {
                    scanStDate = DateTime.Now;
                }
                else
                {
                    scanStDate = DateTime.Now.AddDays(dateAdd);

                }
                scanEndDate = scanStDate.AddDays(1);
            }
            else
            {
                if((scanEndDate.Year != 1980) && (scanStDate > scanEndDate))
                    throw new Exception($"Invalid args scan start date {scanStDate} cannot be bigger then scan end date {scanEndDate}");

                if(scanStDate.Year < DateTime.Now.AddYears(LabReqRepConst.LabReqsNumYears).Year)
                    throw new Exception($"Invalid scan start date {scanStDate}");
                if (scanEndDate.Year == 1980)
                    scanEndDate = scanStDate.AddDays(1);
                if (scanStDate == scanEndDate)
                    scanEndDate = scanEndDate.AddDays(1);
            }
        }
        static async Task GenerateReport(DateTime scanStDate,   DateTime scanEndDate)
        {
            Console.WriteLine($"Getting labreq's scanned for date range {scanStDate.ToString("MM-dd-yyyy")} - {scanEndDate.ToString("MM-dd-yyyy")}");
            string wf = LabReqRepConst.WorkingFolder;
            string labReqsHtmlRepName = $"{LabReqRepConst.LabReqsRepName}_{DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss")}.html";
            string labReqsCSVRepName = $"{LabReqRepConst.LabReqsRepName}_{DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss")}.csv";
            wf = Utilities.CheckFolderPath(wf);
            int totalRecProc = 0;
            string[] emailAtt = new string[2];
            try
            {


                HelperUtilities.Utilities.CreateDirectory(wf);

                SqlCmds sqlCmds = new SqlCmds();
                SBCsvFile = new StringBuilder();
                SBHtmlFile = CreateHeder();
                SBCsvFile.AppendLine($"{LabReqRepConst.HeaderBatchID},{LabReqRepConst.HeaderIndexNumber},{LabReqRepConst.HeaderFinanical},{LabReqRepConst.HeaderRequisition},{LabReqRepConst.HeaderPatientID}," +
                    $"{LabReqRepConst.HeaderMRN},{LabReqRepConst.HeaderClientCode},{LabReqRepConst.HeaderDateOfServices},{LabReqRepConst.HeaderScanDate},{LabReqRepConst.HeaderMerged}");

                foreach (var lReq in sqlCmds.GetLabRecs(scanStDate, scanEndDate, LabReqRepConst.SpGenerateLabReqRep))
                {
                    totalRecProc++;
                    Console.WriteLine($"Total LabReq's read {totalRecProc}");
                    AddCsvFile(lReq).ConfigureAwait(false).GetAwaiter().GetResult();
                    AddHtmlFile(lReq).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                SBHtmlFile.AppendLine("</table>");
                SBHtmlFile.AppendLine("<br/>");
                SBHtmlFile.AppendLine("<br/>");
                if (totalRecProc == 0)
                {

                    SBHtmlFile.AppendLine($"<p style={LabReqRepConst.Quote}color:red;text-align:center;{LabReqRepConst.Quote}>No LabReq's Uploaded</p>");
                    SBCsvFile.AppendLine("No LabReq's Uploaded");
                }
                SBHtmlFile.AppendLine("</body>");
                SBHtmlFile.AppendLine("</html>");
                string fileHtml = SBHtmlFile.ToString().Replace("ScanStartDate", scanStDate.ToString("MM-dd-yyyy")).Replace("ScanEndDate", scanEndDate.ToString("MM-dd-yyyy")).Replace("recProc", totalRecProc.ToString());
                HelperUtilities.Utilities.WriteOutPut(System.IO.Path.Combine(wf, labReqsCSVRepName), SBCsvFile.ToString());
                HelperUtilities.Utilities.WriteOutPut(System.IO.Path.Combine(wf, labReqsHtmlRepName), fileHtml);
                

                emailAtt[0] = $"{System.IO.Path.Combine(wf, labReqsCSVRepName)}";
                emailAtt[1] = $"{System.IO.Path.Combine(wf, labReqsHtmlRepName)}";
            }
            catch(Exception ex)
            {
                SBErrors.AppendLine($"Error getting scanned labreqs for scan start date {scanStDate.ToString("MM-dd-yyyy")} scan end date {scanEndDate.ToString("MM-dd-yyyy")} {ex.Message}");
            }
            SEmail(emailAtt, totalRecProc, scanStDate.ToString("MM-dd-yyyy"), scanEndDate.ToString("MM-dd-yyyy")).ConfigureAwait(false).GetAwaiter().GetResult();
             
            
        }
        public static StringBuilder CreateHeder()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine($"<html lang={LabReqRepConst.Quote}en{LabReqRepConst.Quote} xmlns={LabReqRepConst.Quote}http://www.w3.org/1999/xhtml{LabReqRepConst.Quote}>");
            sb.AppendLine("<head>");
            sb.AppendLine($"<meta charset={LabReqRepConst.Quote}utf-8{LabReqRepConst.Quote}/>");
            sb.AppendLine("<style>");
            sb.AppendLine("table {border-collapse:collapse;width:100%;}");

            sb.AppendLine("th, td {padding: 8px;text-align:left;border-bottom:1px solid #ddd;}");
            sb.AppendLine("tr:hover {background-color:#f5f5f5;}");


            sb.AppendLine("</style>");
            sb.AppendLine("<title>LabReqs Report</title>");
            sb.AppendLine("</head>");
            sb.AppendLine($"<body style={LabReqRepConst.Quote}background-color:lightgray{LabReqRepConst.Quote}>");
            sb.AppendLine($"<h1 style={LabReqRepConst.Quote}text-align:center{LabReqRepConst.Quote}>LabReq's Scanned Report Date Range: ScanStartDate - ScanEndDate</h1>");
            sb.AppendLine($"<h2 style={LabReqRepConst.Quote}text-align:center{LabReqRepConst.Quote}>Total Records Scanned recProc</h2>");
            sb.AppendLine("<br/>");
            sb.AppendLine("<br/>");
            sb.AppendLine($"<table border={LabReqRepConst.Quote}1{LabReqRepConst.Quote}  style={LabReqRepConst.Quote}background-color:#CCFFFF;margin-left:auto;margin-right:auto;{LabReqRepConst.Quote}>");
            sb.AppendLine("<tr>");
            sb.AppendLine($"<th>{LabReqRepConst.HeaderBatchID}</th>");
            sb.AppendLine($"<th>{LabReqRepConst.HeaderIndexNumber}</th>");
            sb.AppendLine($"<th>{LabReqRepConst.HeaderFinanical}</th>");
            sb.AppendLine($"<th>{LabReqRepConst.HeaderRequisition}</th>");
            sb.AppendLine($"<th>{LabReqRepConst.HeaderPatientID}</th>");
            sb.AppendLine($"<th>{LabReqRepConst.HeaderMRN}</th>");
            sb.AppendLine($"<th>{LabReqRepConst.HeaderClientCode}</th>");
            sb.AppendLine($"<th>{LabReqRepConst.HeaderDateOfServices}</th>");
            sb.AppendLine($"<th>{LabReqRepConst.HeaderScanDate}</th>");
            sb.AppendLine($"<th>{LabReqRepConst.HeaderMerged}</th>");
            sb.AppendLine("</tr>");
            return sb;
        }
        static async Task AddCsvFile(LabReqModel reqModel)
        {
            SBCsvFile.AppendLine($"{reqModel.BatchID},{reqModel.IndexNumber},{reqModel.FinNumber},{reqModel.ReqNum},{reqModel.PatID}," +
                $"{reqModel.MRN},{reqModel.ClientCode},{reqModel.DateOfServices.ToString("MM-dd-yyyy")},{reqModel.ScanDate},{reqModel.Merged}");
        }
        static async Task AddHtmlFile(LabReqModel reqModel)
        {
            SBHtmlFile.AppendLine("<tr>");
            SBHtmlFile.AppendLine($"<td>{reqModel.BatchID}</td>");
            SBHtmlFile.AppendLine($"<td>{reqModel.IndexNumber}</td>");

            SBHtmlFile.AppendLine($"<td>{reqModel.FinNumber}</td>");
            SBHtmlFile.AppendLine($"<td>{reqModel.ReqNum}</td>");
            SBHtmlFile.AppendLine($"<td>{reqModel.PatID}</td>");
            SBHtmlFile.AppendLine($"<td>{reqModel.MRN}</td>");
            SBHtmlFile.AppendLine($"<td>{reqModel.ClientCode}</td>");
            SBHtmlFile.AppendLine($"<td>{reqModel.DateOfServices.ToString("MM-dd-yyyy")}</td>");
            SBHtmlFile.AppendLine($"<td>{reqModel.ScanDate}</td>");
            SBHtmlFile.AppendLine($"<td>{reqModel.Merged}</td>");
            SBHtmlFile.AppendLine($"</tr>");
        }
        static async Task SEmail(string[] attachments,int totalProcess,string scanStDate,string scanEndDate)
        {
            string emSub = string.Empty;
            string emailB = "test";
            string textMess = string.Empty;
            if (SBErrors.Length != 0)
            { 
                emSub = LabReqRepConst.EmailSubject.Replace(LabReqRepConst.RepStrError, "Error");
                textMess = emSub.Replace(LabReqRepConst.RepStrReportTime, DateTime.Now.ToString());
                emailB = ($"<h1 style={LabReqRepConst.Quote}text-align:center{LabReqRepConst.Quote};{LabReqRepConst.Quote}color:red{LabReqRepConst.Quote}>Errors running LabReq's Scan Report Date Range {scanStDate} -{scanEndDate} </h1>");
                emailB += ($"<h3 style={LabReqRepConst.Quote}text-align:center{LabReqRepConst.Quote}>Total Records Scanned {totalProcess}</h3>");
                emailB += ($"<h3 style={LabReqRepConst.Quote}text-align:center{LabReqRepConst.Quote};{LabReqRepConst.Quote}color:yellow{LabReqRepConst.Quote}>Run on Computer {Environment.MachineName} </h3>");
                emailB += SBErrors.ToString();
            }
            else
            {
                emailB = ($"<h1 style={LabReqRepConst.Quote}text-align:center{LabReqRepConst.Quote}>LabReq's Scan Report Date Range {scanStDate} -{scanEndDate}</h1>");
                emailB += ($"<h3 style={LabReqRepConst.Quote}text-align:center{LabReqRepConst.Quote}>Total Records Scanned {totalProcess}</h3>");
                emailB += ($"<h3 style={LabReqRepConst.Quote}text-align:center{LabReqRepConst.Quote};{LabReqRepConst.Quote}color:yellow{LabReqRepConst.Quote}>Run on Computer {Environment.MachineName} </h3>");
                emSub = LabReqRepConst.EmailSubject.Replace(LabReqRepConst.RepStrError, "Successful");
            }
            emSub = emSub.Replace(LabReqRepConst.RepStrReportTime, DateTime.Now.ToString());
            //SendEmail(string emailTo, string emailCC, string emailMessage, string emailSubject, string[] emailAttachment, bool emailBodyHtml, string textMessage)
            if (SBErrors.Length == 0)
                SE.EmailInstance.SendEmail(LabReqRepConst.EmailTo,LabReqRepConst.EmailCC, emailB, emSub, attachments, true, textMess);
            else
                SE.EmailInstance.SendEmail(LabReqRepConst.EmailTo, LabReqRepConst.EmailCC, emailB, emSub, string.Empty, true, textMess);
        }
    }
}
