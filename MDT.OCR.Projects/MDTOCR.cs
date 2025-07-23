using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OCR = Edocs.Ocr.Convert.Libaray;
using UT = MDT.OCT.Projects.ConstProp.MDTUtilities;
using MDT.OCT.Projects.Models;
using MDT.OCT.Projects.WebApis;
using EDU = Edocs.HelperUtilities;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using TL = EdocsUSA.Utilities.Logging;
using System.Xml.Linq;
using System.Security.Policy;

namespace MDT.OCT.Projects
{
    internal class MDTOCR
    {
        static void Main(string[] args)
        {
            UT.TotalSkipped = 0;
            TL.TraceLogger.TraceLoggerInstance.StartWatch();
            OpenTraceLog().Wait();
            try
            {

                UT.SB = new StringBuilder();
                UT.SB.AppendLine(UT.MDTHeader);
                ProcessOcrFiles().GetAwaiter().GetResult();
                // IList<MDTOCTModel> model = WebApis.MDTWebApis.GetRecordsToOCT(ConstProp.ConstProp.WebUri,ConstProp.ConstProp.MDTOCRController).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                UT.SB.AppendLine($"Error processing getting ocr txt,{ex.Message}");
                TL.TraceLogger.TraceLoggerInstance.TraceError($"Error processing getting ocr txt,{ex.Message}");
                TL.TraceLogger.TraceLoggerInstance.CloseTraceFile();
                TL.TraceLogger.TraceLoggerInstance.Dispose();
                Console.WriteLine(ex.Message);
            }

            string tRunTime = TL.TraceLogger.TraceLoggerInstance.StopWatch();
            // Write result.
            UT.SB.AppendLine($"NA,Total Docs To Process,{UT.TotalDocsToProcess}");
            UT.SB.AppendLine($"NA,Total Files Processed,{UT.TotalFilesProcessed}");
            UT.SB.AppendLine($"NA,Total Images OCR,{UT.TotalDocsOCR}");
            UT.SB.AppendLine($"NA,Total Files skipped,{UT.TotalSkipped}");
            UT.SB.AppendLine($"NA,Total Files not found,{UT.TotalDocsNotFound}");
            UT.SB.AppendLine($"NA,Total Errors,{UT.TotalErrors}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Docs to Processed {UT.TotalDocsToProcess}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Files Processed {UT.TotalFilesProcessed}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Images OCR {UT.TotalDocsOCR}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Files skipped {UT.TotalSkipped}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Files not found {UT.TotalDocsNotFound}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Errors {UT.TotalErrors}");
            Console.WriteLine("Time elapsed: {0}", tRunTime);
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total time to ocr records {tRunTime}");
            UT.SB.AppendLine($"NA,Total time to ocr records,{tRunTime}");
            SaveCsvFile(tRunTime).Wait();
        }
        static async Task OpenTraceLog()
        {

            string traceLog = Path.Combine(UT.LogFile.Replace("{ApplicationDir}", EDU.Utilities.GetApplicationDir()), $"{EDU.Utilities.GetAssemblyTitle()}_MDTOCR_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.log");
            TL.TraceLogger.TraceLoggerInstance.CreateDirectory(traceLog);
            TL.TraceLogger.TraceLoggerInstance.RunningAssembley = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe";
            TL.TraceLogger.TraceLoggerInstance.OpenTraceLogFile(traceLog, EDU.Utilities.GetAssemblyTitle(), true);
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Opened trace log file:{traceLog}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Opened trace log file:{traceLog}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyTitle:{EDU.Utilities.GetAssemblyTitle()}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyCopyright:{EDU.Utilities.GetAssemblyCopyright()}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyDescription:{EDU.Utilities.GetAssemblyDescription()}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyVersion:{EDU.Utilities.GetAssemblyVersion()}");
        }
        static async Task SaveCsvFile(string tRunTime)
        {
            string csvFile = UT.CSVFile.Replace("{ApplicationDir}", EDU.Utilities.GetApplicationDir());
            csvFile = csvFile.Replace("{datetime}", DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss"));
            EDU.Utilities.CreateDirectory(csvFile);
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Saving csv file:{csvFile}");
            File.WriteAllText(csvFile, UT.SB.ToString());
           
            string emailMessage = $"<p>Done running OCR MDT on computer {Environment.MachineName}</p>";
            emailMessage += $"<p>Total Docs to Processed {UT.TotalDocsToProcess}</p>";
            emailMessage += $"<p>Total Images OCR {UT.TotalDocsOCR}</p>";
            emailMessage += $"<p>Total Files skipped {UT.TotalSkipped}</p>";
            emailMessage += $"<p>Total Files not found {UT.TotalDocsNotFound}</p>";
            emailMessage += $"<p>Total Errors {UT.TotalErrors}</p>";
            emailMessage += $"<p>RunTime {tRunTime}</p>";
            string emailSubject = string.Empty;
            if(UT.TotalErrors > 0)
            {
                emailSubject = $"Errors found running MDT OCR ";
            }
            else
                emailSubject = $"No Errors found running MDT OCR";
            Console.WriteLine("Sending email");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation("Sending email");
            UT.SendEmail(emailMessage,csvFile,emailSubject).GetAwaiter().GetResult();
            Console.WriteLine("");
            Console.WriteLine("Email Sent");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation("Email Sent");
            TL.TraceLogger.TraceLoggerInstance.CloseTraceFile();
            TL.TraceLogger.TraceLoggerInstance.Dispose();
        }

        static async Task ProcessOcrFiles()
        {

            EDU.Utilities.CreateDirectory(UT.WorkingFolder);
            UT.WorkingPdfFolder = Path.Combine(UT.WorkingFolder, "CopyPdfFolder");
            EDU.Utilities.CreateDirectory(UT.WorkingPdfFolder);
            EDU.Utilities.DeleteFiles(UT.WorkingPdfFolder, 0);
            EDU.Utilities.DeleteFiles(UT.WorkingFolder, 0);
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Method ProcessOcrFiles Working folder {UT.WorkingFolder} Working PDF Folder {UT.WorkingPdfFolder}");
            IList<MDTOCRModel> model = WebApis.MDTWebApis.GetRecordsToOCT(ConstProp.MDTUtilities.WebUri, UT.MDTOCRController).ConfigureAwait(false).GetAwaiter().GetResult();
            if ((model != null) && (model.Count > 0))
            {
                int totalList = UT.TotalDocsToProcess = model.Count;

                foreach (MDTOCRModel mdtMod in model)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Total files left to process {totalList--}");
                    Console.WriteLine($"Processing file {mdtMod.FileName}");
                    Console.WriteLine($"Total Files Processed {UT.TotalFilesProcessed}");
                    Console.WriteLine($"Total Images OCR {UT.TotalDocsOCR}");
                    Console.WriteLine($"Total Files skipped {UT.TotalSkipped}");
                    Console.WriteLine($"Total Files not found {UT.TotalDocsNotFound}");
                    Console.WriteLine($"Total Errors {UT.TotalErrors}");
                    if (EDU.Utilities.CheckFileExists(mdtMod.FileName))
                    {
                        if (!(SkipFile(mdtMod.FileName)))
                        {
                            UT.TotalFilesProcessed++;

                            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Processing file {mdtMod.FileName}");
                            string workingPDFFile = Path.Combine(UT.WorkingPdfFolder, Path.GetFileName(mdtMod.FileName));
                            EDU.Utilities.CopyFile(mdtMod.FileName, workingPDFFile, true);
                            GetMDTOcrText(mdtMod, workingPDFFile).GetAwaiter().GetResult();
                        }
                        else
                        {
                            WebApis.MDTWebApis.UpDateRecordsOCR(UT.WebUri, UT.MDTOCRController, mdtMod.ID, -1).GetAwaiter().GetResult();
                            UT.TotalSkipped++;
                            Console.WriteLine($"Skipping file {mdtMod.FileName}");
                            //  UT.SB.AppendLine($"{mdtMod.ID}, Skipping File {mdtMod.FileName}, File type {UT.Easement}");
                            TL.TraceLogger.TraceLoggerInstance.TraceWarning($"{mdtMod.ID} Skipping File {mdtMod.FileName} File type {UT.Easement}");
                        }
                    }
                    else
                    {
                        UT.TotalDocsNotFound++;
                        TL.TraceLogger.TraceLoggerInstance.TraceInformation($"{mdtMod.ID} File Not found {mdtMod.FileName}");
                        UT.SB.AppendLine($"{mdtMod.ID},File Not found,{mdtMod.FileName}");
                    }
                }

            }
            else
            {

                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"NA No Files to process No files");
                UT.SB.AppendLine($"NA,No Files to process,No files");
            }

        }
        static bool SkipFile(string fname)
        {
            fname = Path.GetFileName(fname);
            if (fname.ToLower().StartsWith(UT.Easement))
                return true;
            return false;
        }
        static async Task GetMDTOcrText(MDTOCRModel mDTOCT, string workingPDFFile)
        {
            UT.DocPages = 0;
            UT.SBOCRResults = new StringBuilder();
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Method GetMDTOcrText for id {mDTOCT.ID} for file {workingPDFFile} ");
            try
            {
                string OCRSavedFName = Path.Combine(Path.GetDirectoryName(mDTOCT.FileName), $"{Path.GetFileNameWithoutExtension(mDTOCT.FileName)}_OCRText.txt");
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Saving OCR Text to {OCRSavedFName}");

                string saveFName = Path.Combine(UT.WorkingFolder, $"{OCR.PdfImageConvert.GetNewGuid()}.png");
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Saving ping file to folder {saveFName}");
                //  EDU.Utilities.DeleteFile(saveFName);
                foreach (var mdtImg in OCR.PdfImageConvert.GetPdfImages(workingPDFFile, string.Empty, saveFName, System.Drawing.Imaging.ImageFormat.Png))
                {
                    UT.DocPages++;
                    UT.TotalDocsOCR++;
                    //  try
                    //  {
                    string resultTxt = OCR.PdfImageConvert.OCRSrace(mdtImg, true, false, UT.OCRAPIKey).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (!(string.IsNullOrWhiteSpace(resultTxt)))
                    {
                        resultTxt += "\r\n Page Number:" + UT.DocPages.ToString() + "\r\n" + "\r\n" + "\r\n";

                    }
                    else
                    {
                        UT.TotalErrors++;
                        resultTxt += "\r\n No Text found \r\n Page Number:" + UT.DocPages.ToString() + "\r\n" + "\r\n" + "\r\n";
                    }
                    UT.SBOCRResults.Append(resultTxt);
                    EDU.Utilities.DeleteFile(saveFName);
                   

                    if (UT.DocPages != mDTOCT.NumberDocsScanned)
                    {
                        Console.WriteLine($"Totals dont match for file {saveFName}");
                        UT.SB.AppendLine($"{mDTOCT.ID} pdf file {mDTOCT.FileName},OCR doc page totals don't match Pages ocr { UT.DocPages} Docs Pages scanned {mDTOCT.NumberDocsScanned} Docs Pages uploaded {mDTOCT.NumberDocsUploaded}");

                    }
                  //  else
                   // {
                        WebApis.MDTWebApis.UpDateRecordsOCR(UT.WebUri, UT.MDTOCRController, mDTOCT.ID, UT.DocPages).GetAwaiter().GetResult();
                        Console.WriteLine($"Saveing file {OCRSavedFName}");
                        TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Saving OCR Text file {OCRSavedFName}");
                        File.WriteAllText(OCRSavedFName, UT.SBOCRResults.ToString());
                  //  }

                    //  UT.SB.AppendLine($"Not saving file {saveFName} for pdf file {mDTOCT.FileName},OCR doc page totals don't match Pages ocr { UT.DocPages} Docs Pages scanned {mDTOCT.NumberDocsScanned} Docs Pages uploaded {mDTOCT.NumberDocsUploaded}");
                    // }
                    // catch (Exception ex)
                    //  {
                    //    UT.TotalErrors++;
                    //    Console.WriteLine(ex.Message);
                    //    TL.TraceLogger.TraceLoggerInstance.TraceInformation($"ID {mDTOCT.ID} Getting MDT OCR Text for file {mDTOCT.FileName}, {ex.Message.Replace(",", "--")}");
                    //    UT.SB.AppendLine($"{mDTOCT.ID}, Getting MDT OCR Text for file {mDTOCT.FileName}, {ex.Message.Replace(",", "--")}");
                    //}
                }

            }
            catch (Exception ex)
            {
                UT.TotalErrors++;
                Console.WriteLine(ex.Message);
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"ID {mDTOCT.ID} Getting MDT OCR Text for file {mDTOCT.FileName}, {ex.Message.Replace(",", "--")}");
                UT.SB.AppendLine($"{mDTOCT.ID}, Getting MDT OCR Text for file {mDTOCT.FileName}, {ex.Message.Replace(",", "--")}");
            }

        }
    }
}
