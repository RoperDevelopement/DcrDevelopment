using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static System.Net.WebRequestMethods;
using System.Reflection;
using SE = Edocs.Send.Emails.Send_Emails;
namespace MDT.Checkfor.Doc.Esm
{
    class DocEsm
    {
        static readonly string Document = "Document";
        static readonly string Easement = "Easement";
        static readonly string MDTFolder = @"M:\MDTPDFFiles";
        static readonly string ArgMDTFolder = "/mdtf:";
        static readonly string ArgEmailCC = "/ecc:";
        static readonly string ArgDpf = "/dpf:";
        static string OutputFolder = @"D:\Archives\MDTCompare\MDTCompareResults.txt";
        static readonly string ReportHtmlFile = @"{ApplicationFolder}\ReportMissingFiles.html";
       //   static   string EmailCC = "tressa.orizotti@edocsusa.com";
        static   string EmailCC = string.Empty;
        static StringBuilder sb = new StringBuilder();
        static StringBuilder sbHtml = new StringBuilder();
        static List<string> LFoldersProcessed = new List<string>();
        static bool DeleProcessFile = false;
        static void Main(string[] args)
        {
            try
            {



                if (args.Length == 0)
                {
                    GetFolderAllProcessed().ConfigureAwait(false).GetAwaiter().GetResult();

                    sb.AppendLine($"Processing root folder {MDTFolder}");
                    ProcessFoler().ConfigureAwait(false).GetAwaiter().GetResult();
                    WriteOutFile(string.Empty);
                }
                else
                {

                    GetInputArg(args).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                //if (!(Directory.Exists(Path.GetDirectoryName(OutputFolder))))
                //    Directory.CreateDirectory(Path.GetDirectoryName(OutputFolder));
                Console.WriteLine($"Usage: {ArgMDTFolder}MDTFoler {ArgDpf} to delete all ready check folders {ArgEmailCC} send cc to tressa");
                Console.WriteLine($"Usage: nothing to process all foders under {MDTFolder}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            //   CheckDocEas().ConfigureAwait(false).GetAwaiter().GetResult();
        }
        static async Task GetInputArg(string[] args)
        {
            string processFolder = string.Empty;
            foreach (string inputArgs in args)
            {
                if (inputArgs.StartsWith(ArgDpf, StringComparison.OrdinalIgnoreCase))
                {
                    DeleProcessFile = true;
                    continue;
                }
                if (inputArgs.StartsWith(ArgEmailCC, StringComparison.OrdinalIgnoreCase))
                {
                    EmailCC= "tressa.orizotti@edocsusa.com";
                    continue;
                }

                if (inputArgs.StartsWith(ArgMDTFolder, StringComparison.OrdinalIgnoreCase))
                {
                    processFolder = inputArgs.Substring(ArgMDTFolder.Length);
                }

            }
            GetFolderAllProcessed().ConfigureAwait(false).GetAwaiter().GetResult();
            if (!(string.IsNullOrWhiteSpace(processFolder)))
            {
                sb.AppendLine($"Processing root folder {processFolder}");
                Console.WriteLine($"Processing root folder {processFolder}");
                ProcessFoler(processFolder).ConfigureAwait(false).GetAwaiter().GetResult();
                WriteOutFile(processFolder);
            }
            else
            {
                sb.AppendLine($"Processing root folder {MDTFolder}");
                Console.WriteLine($"Processing root folder {MDTFolder}");
                ProcessFoler().ConfigureAwait(false).GetAwaiter().GetResult();
                WriteOutFile(string.Empty);
            }
        }
        static void WriteOutFile(string folderName)
        {
            if (!(Directory.Exists(Path.GetDirectoryName(OutputFolder))))
                Directory.CreateDirectory(Path.GetDirectoryName(OutputFolder));
            if (!(string.IsNullOrEmpty(folderName)))
            {

                int index = folderName.LastIndexOf("\\");
                folderName = folderName.Substring(++index);
                folderName = OutputFolder.Replace("MDTCompareResults.txt", $"{folderName}_Results.txt");
                System.IO.File.WriteAllText(folderName, sb.ToString());
                // OutputFolder = $"{(char)34}{OutputFolder}{(char)34}";
            }
            else
                System.IO.File.WriteAllText(OutputFolder, sb.ToString());

            EmailReport().ConfigureAwait(false).GetAwaiter().GetResult();
            SaveFoldersProcessed().ConfigureAwait(false).GetAwaiter().GetResult();

        }
        static async Task GetFolderAllProcessed()
        {
            string allReadyProcessed = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            allReadyProcessed = Path.Combine(allReadyProcessed, "MDTProcessed\\MDTFoldersProcessed.txt");
            if (!(Directory.Exists(System.IO.Path.GetDirectoryName(allReadyProcessed))))
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(allReadyProcessed));
            if ((DeleProcessFile) && (System.IO.File.Exists(allReadyProcessed)))
            {
                Console.WriteLine($"Deleting process file {allReadyProcessed}");
                sb.AppendLine($"Deleting process file {allReadyProcessed}");
                System.IO.File.Delete(allReadyProcessed);
            }
            if (System.IO.File.Exists(allReadyProcessed))
            {
                LFoldersProcessed = System.IO.File.ReadAllLines(allReadyProcessed).ToList();
            }
        }
        static async Task SaveFoldersProcessed()
        {
            string allReadyProcessed = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            allReadyProcessed = Path.Combine(allReadyProcessed, "MDTProcessed\\MDTFoldersProcessed.txt");
            if (System.IO.File.Exists(allReadyProcessed))
                System.IO.File.Delete(allReadyProcessed);
            Console.WriteLine($"Saving process file {allReadyProcessed}");
            sb.AppendLine($"Savig process file {allReadyProcessed}");
            using (System.IO.StreamWriter sw = new StreamWriter(allReadyProcessed, false, Encoding.ASCII))
            {
                foreach (var line in LFoldersProcessed)
                    sw.WriteLine(line);
                sw.Flush();
            }
        }
        static async Task EmailReport()
        {
            string htmlFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string htmlSaveFile = Path.Combine(htmlFilePath, $"HtmlFileRep\\HtmlRep_{DateTime.Now.ToString("yyyyy_MM_dd_HH_mm_ss")}.html");
            if (!(Directory.Exists(System.IO.Path.GetDirectoryName(htmlSaveFile))))
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(htmlSaveFile));
            string htmlFile = ReportHtmlFile.Replace("{ApplicationFolder}", htmlFilePath);
            string htmFileTxt = System.IO.File.ReadAllText(htmlFile, Encoding.ASCII);
            htmFileTxt = htmFileTxt.Replace("{DateTimeReportRan}", DateTime.Now.ToString()).Replace("{TableData}", sbHtml.ToString());
            System.IO.File.WriteAllText(htmlSaveFile, htmFileTxt);
            string emailMessag = $"Html file attached for results of either {Document.ToUpper()} or {Easement.ToUpper()} PDF file missing for MDT";
            string emailSubject = $"{Document.ToUpper()} or {Easement.ToUpper()} PDF file missing for MDT for machine {Environment.MachineName}";
            SE.EmailInstance.SendEmail(string.Empty, EmailCC, emailMessag, emailSubject, htmlSaveFile, true, string.Empty);
        }
        static async Task ProcessFoler(string folderName)
        {
            sbHtml.AppendLine($"<tr><td>Processing Single Folder</td><td>{folderName}</td></tr>");
            foreach (var mdfFilesRoot in Directory.GetDirectories(folderName, "*.*", SearchOption.AllDirectories))
            {
                try
                {
                    Console.WriteLine($"Processing root foler {mdfFilesRoot}");
                    CheckDocEas(mdfFilesRoot, folderName).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                { Console.WriteLine(ex); }
            }
        }
        static async Task ProcessFoler()
        {
            sbHtml.AppendLine($"<tr><td>Processing all Folders under</td><td>{MDTFolder}</td></tr>");
            foreach (var mdfFilesRoot in Directory.GetDirectories(MDTFolder, "*.*", SearchOption.AllDirectories))
            {
                // sb.AppendLine($"Processing root foler {mdfFilesRoot}");
                Console.WriteLine($"Processing root foler {mdfFilesRoot}");
                foreach (var mdfFiles in Directory.GetDirectories(mdfFilesRoot, "*.*", SearchOption.AllDirectories))
                {
                    // sb.AppendLine($"Processing  foler {mdfFilesRoot}");
                    Console.WriteLine($"Processing  foler {mdfFilesRoot}");
                    CheckDocEas(mdfFiles, MDTFolder).ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }
        }
        static async Task CheckDocEas(string mdfFiles, string rootFolder)
        {


            //  foreach (var mdfFiles in Directory.GetDirectories(folder, "*.*", SearchOption.AllDirectories))
            //   {
            //     sb.AppendLine($"Getting files for folder {mdfFiles}");
            Console.WriteLine($"Getting files for folder {mdfFiles}");
            string[] numFiles = Directory.GetFiles(mdfFiles, "*.pdf");
            Console.WriteLine($"Total files returned {numFiles.Count()}");

            //    sb.AppendLine($"Total files found {numFiles.Count()} in folder {mdfFiles}");
            if (numFiles.Count() != 2)
            {
                sbHtml.AppendLine($"<tr><td>Total files returned {numFiles.Count()} under folder</td><td>{mdfFiles}</td></tr>");
                //sbHtml.AppendLine($"<tr><td></td><td></td></tr>");
                sb.AppendLine("");
                if (numFiles.Count() == 0)
                {
                    sb.AppendLine($"No files found in  folder {Path.GetDirectoryName(mdfFiles)}");
                    sbHtml.AppendLine($"<tr><td>No files found</td><td>{Path.GetDirectoryName(mdfFiles)}</td></tr>");
                    return;
                }
                string[] foundFiles = Directory.GetFiles(mdfFiles, "*.pdf");
                sb.AppendLine($"Found {foundFiles.Count()} in folder {mdfFiles}");
                foreach (var files in foundFiles)
                {
                    string fName = Path.GetFileName(files);
                    if (!(LFoldersProcessed.Contains(Path.GetDirectoryName(files))))
                    {

                        LFoldersProcessed.Add(Path.GetDirectoryName(files));
                    }

                    else
                    {
                        sb.AppendLine($"All ready procees folder {Path.GetDirectoryName(files)}");
                        Console.WriteLine($"All ready procees folder {Path.GetDirectoryName(files)}");
                        sbHtml.AppendLine($"<tr><td>All ready procees folder</td><td>{Path.GetDirectoryName(files)}</td></tr>");
                        break;
                    }



                    // if (files.Count() != 2)
                    // {
                    if (fName.ToLower().StartsWith(Document.ToLower()))
                    {
                        sb.AppendLine($"{Easement.ToUpper()} not found {Path.GetDirectoryName(files)}");
                        sbHtml.AppendLine($"<tr><td>{Easement.ToUpper()} not found</td><td>{Document.ToUpper()} found  {files}</td></tr>");
                        SearchForFile(files, rootFolder, Easement).GetAwaiter().GetResult();
                        break;
                        //sb.AppendLine(" ");
                    }
                    else
                    {
                        sb.AppendLine($"{Document.ToUpper()} not in folder {Path.GetDirectoryName(files)}");
                        sbHtml.AppendLine($"<tr><td>{Document.ToUpper()} not found</td><td>{Easement.ToUpper()} found {files}</td></tr>");
                        SearchForFile(files, rootFolder, Document).GetAwaiter().GetResult();
                        break;
                        //sb.AppendLine(" ");
                    }

                    // }
                    // else
                    //   {
                    //     string compFilesResults = CheckFileNames(numFiles).ConfigureAwait(false).GetAwaiter().GetResult();
                    //     if (!(string.IsNullOrWhiteSpace(compFilesResults)))
                    //        sb.AppendLine(compFilesResults);
                    // }
                    //  break;
                }
            }

            //else
            //{
            //    string compFilesResults = CheckFileNames(numFiles).ConfigureAwait(false).GetAwaiter().GetResult();
            //    if (!(string.IsNullOrWhiteSpace(compFilesResults)))
            //        sb.AppendLine(compFilesResults);
            //}
            //   }


        }
        static async Task SearchForFile(string srcFile, string rootfolder, string docType)
        {
            string searchFile = $"{System.IO.Path.GetFileNameWithoutExtension(srcFile)}";
            int index = searchFile.IndexOf("_");
            if (index > 0)
                searchFile = searchFile.Substring(index);
            bool foundMatch = false;
            searchFile = $"{docType}{searchFile}*.pdf";
            string[] matches = Directory.GetFiles(rootfolder, searchFile, SearchOption.AllDirectories);
            if (matches.Length > 0)
            {
                foreach (var match in matches)
                {
                    if (string.Compare(match, srcFile, StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        foundMatch = true;
                        sb.AppendLine($"Possibale matches {match} for doc type  {docType}");
                        sbHtml.AppendLine($"<tr><td>Possibale matches doc type {docType}</td><td> {match}</td></tr>");
                    }

                }
            }

            if (!(foundMatch))
            {
                string strFileName = string.Empty;
                string[] searchFiles = searchFile.Split(' ');
                for (int i = 0; i < searchFiles.Count(); i++)
                {
                    if (!(string.IsNullOrWhiteSpace(strFileName)))
                        strFileName = strFileName.Replace("*", "").Trim();
                    strFileName += $" {searchFiles[i]}*.pdf";
                    if (SearchForFile(strFileName.Trim(), rootfolder, docType, srcFile).ConfigureAwait(false).GetAwaiter().GetResult())
                    {
                        foundMatch = true;
                        break;
                    }
                }
            }
            if (!(foundMatch))
                sb.AppendLine($"No matches found for sreach file {srcFile} for doc type  {docType}");

        }
        static async Task<bool> SearchForFile(string searchFile, string rootfolder, string docType, string srcFile)
        {
            bool foundMatch = false;
            string[] matches = Directory.GetFiles(rootfolder, searchFile, SearchOption.AllDirectories);
            if (matches.Length > 0)
            {
                foreach (var match in matches)
                {
                    if (string.Compare(match, srcFile, StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        foundMatch = true;
                        sb.AppendLine($"Possibale matches {match} for doc type  {docType}");
                        sbHtml.AppendLine($"<tr><td>Possibale matches doc type {docType}</td><td> {match}</td></tr>");
                    }

                }
            }
            return foundMatch;

        }
        static async Task<string> CheckFileNames(string[] files)
        {
            Console.WriteLine($"Compaing file names for files {files[0]} {files[1]}");
            string fileName1 = Path.GetFileName(files[0]);
            int indexUndreScore = fileName1.IndexOf("_");
            fileName1 = fileName1.Substring(indexUndreScore);
            string fileName2 = Path.GetFileName(files[1]);
            indexUndreScore = fileName2.IndexOf("_");
            fileName2 = fileName2.Substring(indexUndreScore);
            if (string.Compare(fileName1, fileName2, true) != 0)
            {
                Console.WriteLine("skipping compate");
                //return string.Empty;
                //  return ($"Project Name and Number not the same for files {files[0]} {files[1]}");//
            }
            return string.Empty;
        }
    }

}
