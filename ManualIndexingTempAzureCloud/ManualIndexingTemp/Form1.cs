
using FreeImageAPI;
using Scanquire.Public;
using Scanquire.Public.ArchivesConstants;
using Scanquire.Public.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Threading;
using SE = Edocs.Send.Emails.Send_Emails;
using Edocs.HelperUtilities;
using EdocsUSA.Utilities.Logging;
using EdocsUSA.Utilities;
using EDL = EdocsUSA.Utilities.Logging;

namespace ManualIndexingTemp
{
    public partial class Form1 : Form
    {
        readonly string ErrorDocument = "ERROR";
        readonly string CsnNumber = "CsnNumber";
        List<Dictionary<string, object>> Records;
        int currentPosition = 0;
        int currentDisplayPosition = 0;
        DateTime stManIndex = DateTime.Now;
        //readonly string tessPath = (@"C:\Program Files (x86)\Tesseract-OCR\tessdata");
        bool ShowUpLoadWindow
        { get { return Properties.Settings.Default.ShowUploadWindow; } }
        string ValidIndexNumberRegex
        { get { return Properties.Settings.Default.ValidIndexNumberRegex; } }

        string JsonFileCurrent
        { get { return Properties.Settings.Default.ManuelIndexingJSFilesCurrent; } }
        string ManuelIndexingJSBackUp
        { get { return Properties.Settings.Default.ManuelIndexingJSBackUp; } }

        string OldIndexNumberRegex
        { get { return Properties.Settings.Default.OldIndexNumberRegex; } }

        string OldIndexNumberMessage
        { get { return Properties.Settings.Default.OldIndexNumberMessage; } }

        string ValidRequisitionNumberRegex
        { get { return Properties.Settings.Default.ValidRequisitionNumberRegx; } }

        string ValidCSNNumberRegex
        { get { return Properties.Settings.Default.ValidCSNNumbers; } }

        string DefaultIndexNumber
        { get { return Properties.Settings.Default.DefaultIndexNumber; } }

        string DefaultScanStationID
        { get { return Properties.Settings.Default.DefaultScanStationID; } }

        string Archiver
        { get { return Properties.Settings.Default.Archiver; } }

        string ArchiveRoot
        { get { return Properties.Settings.Default.ArchiveRoot; } }

        public string BatchId { get; set; }

        string BatchDir
        { get { return Path.Combine(ArchiveRoot, BatchId); } }

        string RecordsFilePath
        { get { return Path.Combine(BatchDir, Path.ChangeExtension(BatchId + "_records", "json")); } }

        string IndexNumber
        { get; set; }
        string ReqNumber
        { get; set; }
        string CSNNumber
        { get; set; }
        bool EnableUploadToSP
        { get { return Properties.Settings.Default.UploadToSP; } }

        string UploadToSPScriptPath
        { get { return Properties.Settings.Default.UploadToSPScriptPath; } }


        bool _RequiresRequisitionNumber = true;

        public bool RequiresRequisitionNumber
        {
            get { return _RequiresRequisitionNumber; }
            set { _RequiresRequisitionNumber = value; }
        }
        string RecordFilePath
        { get; set; }
        string ImageIndexNumber
        { get; set; }

        public string TraceFolder
        { get; set; }
        private StringBuilder ErrMessage
        {
            get; set;
        }
        //   private bool MissingRequisitionNumber
        //   {
        //      get; set;
        // }
        public string LogFolder
        {
            get { return (Properties.Settings.Default.LogFolder.Replace("{ApplicationDir}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); }
        }

        bool SendEmailsOnlyOnErrors
        { get { return Properties.Settings.Default.SendErrorEmailsOnly; } }
        //   private bool MissingIndexNumber
        // { get; set; }
        public Form1()
        {
            InitializeComponent();
            rbAddManeul.Checked = true;
            sqImageListViewer1.SplitContainer.SplitterDistance = 90;
            sqImageListViewer1.ThumbnailToolStrip.Visible = false;
            //  MissingRequisitionNumber = false;
            // MissingIndexNumber = false;
            ErrMessage = new StringBuilder();
            ErrMessage.AppendLine("Running Manual Indexing");
            SetVersion();


        }

        void SetVersion()
        {
            try
            {

                this.Text = $"{Utilities.GetAssemblyDescription()} Version 1.7.1 Copyright © 2014";
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Assembly version {this.Text}");
            }
            catch (Exception ex)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Getting Assembly version {ex.Message}");
            }

        }

        void MakeCopyJsonFile()
        {
            string filePath = RecordsFilePath;
            if (!(CopyJasonFile(filePath, JsonFileCurrent)))
                CopyJasonFile(filePath, ManuelIndexingJSBackUp);
        }
        bool CopyJasonFile(string sFolder, string jsonCurrent)
        {

            try
            {
                Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(jsonCurrent);

                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Copying json file from {sFolder} {Path.Combine(JsonFileCurrent, Path.GetFileName(sFolder))}");
                Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(sFolder, jsonCurrent, true, string.Empty, false);
                return true;
            }
            catch (Exception ex)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Error copying file {sFolder} to {Path.Combine(JsonFileCurrent, Path.GetFileName(sFolder))} {ex.Message}");
                EmailSend(true, $"Error copying file {sFolder} to {Path.Combine(JsonFileCurrent, Path.GetFileName(sFolder))} {ex.Message}");

            }
            return false;
        }
        void EmailSend(bool error)
        {
            if ((Environment.MachineName.ToUpper().StartsWith("DANC")))
                return;
            if ((SendEmailsOnlyOnErrors) && (!(error)))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning($"Not sending email since SendEmailsOnlyOnErrors is set to{SendEmailsOnlyOnErrors}");
                return;
            }



            try
            {
                TimeSpan ts = DateTime.Now - stManIndex;
                SE.EmailInstance.EmailAttachment = string.Empty;
                ErrMessage.AppendLine($"ran on computer {Environment.MachineName} user {Environment.UserName}  batchid {BatchId}");
                ErrMessage.AppendLine($"runtime {ts.Hours.ToString()}:{ts.Minutes.ToString()}:{ts.Seconds.ToString()}");
                SE.EmailInstance.EmailCC = Properties.Settings.Default.EmailCC;
                SE.EmailInstance.EmailFrom = Properties.Settings.Default.EmailFrom;
                SE.EmailInstance.EmailTo = Properties.Settings.Default.EmailTo;
                SE.EmailInstance.EmailPort = Properties.Settings.Default.EmailPort;
                SE.EmailInstance.EmailServer = Properties.Settings.Default.EmailServer;
                SE.EmailInstance.EmailPassord = Properties.Settings.Default.EmailPassword;
                SE.EmailInstance.EmailBody = ErrMessage.ToString(); ;
                SE.EmailInstance.EmailSubject = Properties.Settings.Default.EmailSubject;
                if (error)
                {
                    SE.EmailInstance.EmailSubject = SE.EmailInstance.EmailSubject.Replace("{ErrorSuccess}", "Error").Replace("{runDate}", DateTime.Now.ToString());
                }
                else
                    SE.EmailInstance.EmailSubject = SE.EmailInstance.EmailSubject.Replace("{ErrorSuccess}", "No Errors").Replace("{runDate}", DateTime.Now.ToString());
                SE.EmailInstance.SendEmail(false);

                EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Email sent ");
                if (error)
                {
                    SE.EmailInstance.EmailTo = Properties.Settings.Default.TextTo;
                    SE.EmailInstance.EmailCC = Properties.Settings.Default.EmailCC.Trim();
                    SE.EmailInstance.EmailBody = $"Error sending Batch id {BatchId}";
                    SE.EmailInstance.SendEmail(false);
                    EDL.TraceLogger.TraceLoggerInstance.TraceError("Text sent ");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Sending email {ex.Message}");
            }

        }
        void EmailSend(bool error, string message)
        {
            if ((Environment.MachineName.ToUpper().StartsWith("DANC")))
                return;
            if ((SendEmailsOnlyOnErrors) && (!(error)))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning($"Not sending email since SendEmailsOnlyOnErrors is set to{SendEmailsOnlyOnErrors}");
                return;
            }



            try
            {
                TimeSpan ts = DateTime.Now - stManIndex;
                SE.EmailInstance.EmailAttachment = string.Empty;
                ErrMessage.AppendLine($"ran on computer {Environment.MachineName} user {Environment.UserName} batchid {BatchId}");
                ErrMessage.AppendLine($"runtime {ts.Hours.ToString()}:{ts.Minutes.ToString()}:{ts.Seconds.ToString()} {message}");
                SE.EmailInstance.EmailCC = Properties.Settings.Default.EmailCC;
                SE.EmailInstance.EmailFrom = Properties.Settings.Default.EmailFrom;
                SE.EmailInstance.EmailTo = Properties.Settings.Default.EmailTo;
                SE.EmailInstance.EmailPort = Properties.Settings.Default.EmailPort;
                SE.EmailInstance.EmailServer = Properties.Settings.Default.EmailServer;
                SE.EmailInstance.EmailPassord = Properties.Settings.Default.EmailPassword;
                SE.EmailInstance.EmailBody = ErrMessage.ToString(); ;
                SE.EmailInstance.EmailSubject = Properties.Settings.Default.EmailSubject;
                if (error)
                {
                    SE.EmailInstance.EmailSubject = SE.EmailInstance.EmailSubject.Replace("{ErrorSuccess}", "Error").Replace("{runDate}", DateTime.Now.ToString());
                }
                else
                    SE.EmailInstance.EmailSubject = SE.EmailInstance.EmailSubject.Replace("{ErrorSuccess}", "No Errors").Replace("{runDate}", DateTime.Now.ToString());
                SE.EmailInstance.SendEmail(false);

                EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Email sent ");
                if (error)
                {
                    SE.EmailInstance.EmailTo = Properties.Settings.Default.TextTo;
                    SE.EmailInstance.EmailCC = Properties.Settings.Default.EmailCC.Trim();
                    SE.EmailInstance.EmailBody = $"Error sending Batch id {BatchId} error message {message}";
                    SE.EmailInstance.SendEmail(false);
                    EDL.TraceLogger.TraceLoggerInstance.TraceError("Text sent ");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Sending email {ex.Message}");
            }

        }

        protected IEnumerable<Scanquire.Public.SQImage> ReadRecordFile(string path)
        {
            using (FreeImageBitmap fib = FreeImageBitmapExtensions.FromBytes(File.ReadAllBytes(path)))
            {
                int frameCount = fib.FrameCount;
                int progressCurrent = 0;
                int progressTotal = frameCount;
                //  LF.Logger.LogInstance.WriteLoggingLogFile($"Getting image for {path}", false, LF.LoggingErrorType.Info);
                //Loop through the image's frames and yeild a new SQImage for each frame.			
                for (int i = 0; i < frameCount; i++)
                {
                    progressCurrent++;
                    fib.SelectActiveFrame(i);
                    using (FreeImageBitmap pageFib = (FreeImageBitmap)(fib.Clone()))
                    { yield return new SQImage(pageFib); }
                }
            }
        }


        protected bool NeedsIndexing(int index)
        {
            Dictionary<string, object> record = Records[index];
            object indexNumber = null;
            record.TryGetValue(JsonsFieldConstants.JsonFieldIndexNumber, out indexNumber);
            string sIndexNumber = (string)(indexNumber);

            object requisitionNumber = null;
            record.TryGetValue(JsonsFieldConstants.JsonFieldRequisitionNumber, out requisitionNumber);
            string sRequisitionNumber = (string)(requisitionNumber);
            object csnNumber = null;
            record.TryGetValue(CsnNumber, out csnNumber);
            string sCsnNum = (string)(csnNumber);

            if (string.IsNullOrWhiteSpace(sIndexNumber) == true)
            {
                return true;
            }

            if (string.Equals(sIndexNumber, DefaultIndexNumber, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            if (RequiresRequisitionNumber && (string.IsNullOrWhiteSpace((string)requisitionNumber)))
            {
                return true;
            }
            if (!(string.IsNullOrWhiteSpace(sCsnNum)))
            {
                if (sCsnNum.Contains('-'))
                    return true;
            }

            return false;
        }
        protected int GetRemainingCounter()
        {
            int ctr = 0;
            for (int i = currentPosition; i < Records.Count; i++)
            {
                if (NeedsIndexing(i))
                { ctr++; }
            }
            return ctr;
        }


        protected string AddZeros(string inStr, int maxDigits, int maxStrLength)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Adding zeros to string:{inStr} maxdigits:{maxDigits.ToString()} max string length:{maxStrLength.ToString()}");
            string zeros = string.Empty;
            if (inStr.Length < maxStrLength)
            {
                int loop = inStr.Length;
                while (loop < maxDigits)
                {
                    zeros += "0";
                    loop++;
                }
                inStr = $"{zeros.Trim()}{inStr}";
            }

            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"New sting:{inStr} after adding zeros");
            return inStr;
        }


        protected void DisplayRecordFile()
        {


            Dictionary<string, object> record = Records[currentDisplayPosition];

            string recordFileName = (string)(record[JsonsFieldConstants.JsonFieldFileName]);
            RecordFilePath = Path.Combine(BatchDir, recordFileName);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Displaying image {RecordFilePath}");

            sqImageListViewer1.ClearAll(false, true);
            foreach (SQImage image in ReadRecordFile(RecordFilePath))
            {
                //  MessageBox.Show(RecordFilePath);
                sqImageListViewer1.Add(image);
            }

            bool viewingCurrentRecord = currentDisplayPosition == currentPosition;

            sqImageListViewer1.BackColor = (viewingCurrentRecord) ? SystemColors.Control : Color.DarkOrange;

            sqImageListViewer1.NavigateTo(0);
            sqImageListViewer1.ActiveImageViewer.ScaleToFitWidth();

            txtViewPosition.Text = (currentDisplayPosition + 1).ToString();


            lblStatusMessage.BackColor = Color.White;
            lblStatusMessage.Text = "Need Index # ";
            if (!(rbAddManeul.Checked))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Checking to add zeros to index number");
                CheckFinNum().Wait();

            }

            // txtIndexNumber.Focus();
        }

        protected bool TryStep()
        {

            //sqImageListViewer1.ClearAll(false, true);

            txtIndexNumber.Clear();
            txtRequisitionNumber.Clear();
            txtBoxCSN.Clear();
            sqImageListViewer1.ClearAll(false, true);
            currentPosition++;

            //if at the end of the list, return false
            if (currentPosition >= Records.Count())
            { return false; }
            //If the item at the current index needs to be indexed, set it up and return true
            if (NeedsIndexing(currentPosition) == true)
            {
                Dictionary<string, object> record = Records[currentPosition];

                txtIndexPosition.Text = (currentPosition + 1).ToString();
                txtRemainingCounter.Text = GetRemainingCounter().ToString();

                currentDisplayPosition = currentPosition;
                // DisplayRecordFile();

                object indexNumber;
                if (record.TryGetValue(JsonsFieldConstants.JsonFieldIndexNumber, out indexNumber) == true)
                {

                    string sIndexNumber = (string)(indexNumber);
                    if (sIndexNumber.Equals(this.DefaultIndexNumber, StringComparison.OrdinalIgnoreCase) == false)
                    { txtIndexNumber.Text = (string)indexNumber; }
                    txtIndexNumber.Focus();
                }


                object reqNumber;
                if (record.TryGetValue(JsonsFieldConstants.JsonFieldRequisitionNumber, out reqNumber) == true)
                { txtRequisitionNumber.Text = (string)reqNumber; }
                //   DisplayRecordFile();
                // txtIndexNumber.Focus();

                object CSNNumber;
                if (record.TryGetValue(CsnNumber, out CSNNumber) == true)
                {
                    txtBoxCSN.Text = (string)CSNNumber;
                    //if (txtBoxCSN.Text.Contains("-"))
                    //{
                    //    txtBoxCSN.Focus();
                    //    DisplayRecordFile();
                    //    return true;
                    //}
                    //  txtBoxCSN.Focus();
                }
                if (string.IsNullOrWhiteSpace(txtIndexNumber.Text) && !(string.IsNullOrWhiteSpace(txtBoxCSN.Text)))
                {
                    txtIndexNumber.Text = txtBoxCSN.Text;
                    EDL.TraceLogger.TraceLoggerInstance.TraceWarning($"Using CSN number {txtBoxCSN.Text} for Index Number");
                    ProcessCurrentRecord();

                }
                DisplayRecordFile();
                txtIndexNumber.Focus();
                return true;
            }
            //Else continue to the next record.
            else
            {
                txtRemainingCounter.Text = "?";
                return TryStep();
            }
        }
        private void UpLoadSharePoint()
        {
            Environment.ExitCode = 0;
            bool processStated = true;


            if ((Environment.MachineName.ToUpper().StartsWith("DanCRoper")))
            {
                finalizedBatch = true;
                this.Close();
            }
            else
            {
                try
                {
                    if (EnableUploadToSP == true)
                    {

                        Process spProc = new Process();
                        spProc.StartInfo.FileName = UploadToSPScriptPath;
                        spProc.StartInfo.Arguments = $"/batchId:{BatchId} /archiver:{Archiver}";
                        spProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        if (ShowUpLoadWindow)
                            spProc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                        EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Starting process {spProc.StartInfo.FileName} with args {spProc.StartInfo.Arguments}");
                        if (!(spProc.Start()))
                        {
                            processStated = false;
                            EDL.TraceLogger.TraceLoggerInstance.TraceError($"Upload sharepoint process {spProc.StartInfo.FileName} with args {spProc.StartInfo.Arguments} did not start");
                            ErrMessage.AppendLine($"Upload sharepoint process {spProc.StartInfo.FileName} with args {spProc.StartInfo.Arguments} did not start");
                            EmailSend(true);
                            MessageBox.Show($"{Properties.Settings.Default.EdocsSupport}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Upload to sharepoint process {spProc.StartInfo.FileName} with args {spProc.StartInfo.Arguments} started");
                        }

                    }
                }
                catch (Exception ex)
                {
                    processStated = false;
                    EDL.TraceLogger.TraceLoggerInstance.TraceError($"Could not upload documents to SharePoint error {ex.Message}");
                    ErrMessage.AppendLine($"Could not upload documents to SharePoint error {ex.Message}");
                    EmailSend(true);
                    MessageBox.Show($"{Properties.Settings.Default.EdocsSupport} \r\n error message {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            if (processStated)
                EmailSend(false);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total records indexed {Records.Count.ToString()}");
            finalizedBatch = true;
            this.Close();
        }
        private void FinalizeBatch()
        {
            try
            {
                this.Enabled = false;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<string> updatedRecords = new List<string>();
                foreach (Dictionary<string, object> record in Records)
                {
                    string indexNumber = record[JsonsFieldConstants.JsonFieldIndexNumber].ToString();
                    //If the index number is 15 digits
                    //Split it into Client Code & MRN
                    //if (Regex.IsMatch(indexNumber, @"^\d{15}$"))
                    //{
                    //    record[JsonsFieldConstants.JsonFieldClientCode] = indexNumber.Substring(0, 6);
                    //    record[JsonsFieldConstants.JsonFieldPatientID] = indexNumber.Substring(6).TrimStart('0');
                    //}
                    updatedRecords.Add(serializer.Serialize(record));
                }

                File.WriteAllLines(RecordsFilePath, updatedRecords);

                lblStatusMessage.Text = "Uploading to sharepoint, please wait";
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation("uploading to share point");
                UpLoadSharePoint();


            }
            catch (Exception ex)
            {
                ErrMessage.AppendLine($"Error FinalizeBatch {ex.Message}");
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Error FinalizeBatch {ex.Message}");
                EmailSend(true);

                MessageBox.Show(ex.Message);
            }
            finally
            { this.Enabled = true; }
        }



        private static Dictionary<string, object> LoadBatchSettings(string path)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Loading settings for path {path}");

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> batchSettings = (Dictionary<string, object>)(serializer.DeserializeObject(File.ReadAllText(path)));
            return batchSettings;
        }

        private static List<Dictionary<string, object>> LoadRecords(string path)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Loading records for path {path}");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> records = new List<Dictionary<string, object>>();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total records loaded {File.ReadAllLines(path).Count().ToString()}");
            foreach (string line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line))
                { continue; }
                records.Add((Dictionary<string, object>)(serializer.DeserializeObject(line)));
            }
            return records;
        }

        private void LoadRecords()
        {
            // CopyJasonFile(RecordsFilePath);
            MakeCopyJsonFile();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Records = new List<Dictionary<string, object>>();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total records loaded {File.ReadAllLines(RecordsFilePath).Count().ToString()}");
            foreach (string line in File.ReadAllLines(RecordsFilePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                { continue; }
                Records.Add((Dictionary<string, object>)(serializer.DeserializeObject(line)));
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ProcessCurrentRecord();
        }
        private void ProcessCurrentRecord()
        {
            try
            {
                if (currentDisplayPosition != currentPosition)
                {
                    btnNavigateReset.PerformClick();
                    return;
                }
                IndexNumber = txtIndexNumber.Text.Trim();


                if (string.IsNullOrWhiteSpace(IndexNumber))
                {
                    EDL.TraceLogger.TraceLoggerInstance.TraceError("Index Number empty");
                    txtIndexNumber.Focus();
                    throw new Exception("Index number cannot be empty");
                }
                if (IndexNumber.Length <= 10)
                {
                    if (!(CheckValidCSNNumber(IndexNumber)))
                    {
                        EDL.TraceLogger.TraceLoggerInstance.TraceError($"Invalid Index Number {IndexNumber} as CSN number should match {ValidCSNNumberRegex} ");
                        throw new Exception($"Invalid Index Number {IndexNumber} as CSN number");
                    }
                }
                else
                {
                    if (Regex.IsMatch(IndexNumber, ValidIndexNumberRegex) == false)
                    {

                        EDL.TraceLogger.TraceLoggerInstance.TraceError($"Invalid Index Number {IndexNumber} format {ValidIndexNumberRegex}");
                        txtIndexNumber.Focus();
                        throw new Exception("Invalid index number");
                    }
                }


                ReqNumber = txtRequisitionNumber.Text.Trim();

                if (RequiresRequisitionNumber && string.IsNullOrWhiteSpace(ReqNumber))
                {
                    DialogResult dr = MessageBox.Show("Continue Missing Requisition #", "No Requisition #", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.No)
                    {


                        txtRequisitionNumber.Focus();
                        EDL.TraceLogger.TraceLoggerInstance.TraceError("Requisition # cannot be empty");
                        throw new Exception("Requisition # cannot be empty");
                    }
                    else
                        ErrMessage.AppendLine("Skipped adding req number");

                }
                if (string.IsNullOrWhiteSpace(ReqNumber))
                    EDL.TraceLogger.TraceLoggerInstance.TraceWarning($"Missing req number for index number {IndexNumber}");
                else

                {
                    if (Regex.IsMatch(ReqNumber, ValidRequisitionNumberRegex) == false)
                    {
                        EDL.TraceLogger.TraceLoggerInstance.TraceWarning($"Invalid req number {ReqNumber} should match regx expression {ValidIndexNumberRegex} ");
                        throw new Exception("Invalid requisition number");
                    }
                    Records[currentPosition][JsonsFieldConstants.JsonFieldRequisitionNumber] = ReqNumber;
                }
                CSNNumber = txtBoxCSN.Text;
                if (!(string.IsNullOrWhiteSpace(CSNNumber)))
                {
                    if (!(CheckValidCSNNumber(CSNNumber)))
                    {
                        EDL.TraceLogger.TraceLoggerInstance.TraceError($"Invalid CSN Number {CSNNumber} should match {ValidCSNNumberRegex} ");
                        throw new Exception("Invalid CSN number");
                    }
                    if (CsnNumber.Contains('-'))
                    {
                        EDL.TraceLogger.TraceLoggerInstance.TraceError($"Invalid CSN Number {CSNNumber} should match {ValidCSNNumberRegex} ");
                        throw new Exception("Invalid CSN number");
                    }
                    Records[currentPosition][CsnNumber] = CSNNumber;
                }


                Records[currentPosition][JsonsFieldConstants.JsonFieldIndexNumber] = IndexNumber;
                // MissingIndexNumber = false;
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Scan Operator entered Index number {IndexNumber} req number {ReqNumber} ");

                if (TryStep() == false)
                {
                    ErrMessage.AppendLine("All records indexed");
                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation("All records indexed");
                    MessageBox.Show("No more records");
                    FinalizeBatch();
                }
                txtIndexNumber.Focus();
            }
            catch (Exception ex)
            {

                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Error getting index number or rec number {ex.Message}");
                MessageBox.Show(ex.Message);
                txtIndexNumber.Focus();
            }
        }

        private void txtIndexNumber_TextChanged(object sender, EventArgs e)
        {
            txtIndexNumberLength.Text = txtIndexNumber.Text.Length.ToString();
            chkValidIndexNumber.Checked = IsValidIndexNumber(txtIndexNumber.Text.Trim());
        }

        private bool IsReqNumerIndexNumber(string value)
        {
            return Regex.IsMatch(value, ValidRequisitionNumberRegex);
        }

        private bool IsValidIndexNumber(string value)
        {
            return Regex.IsMatch(value, ValidIndexNumberRegex);
        }


        private bool IsOldIndexNumber(string value)
        { return Regex.IsMatch(value, OldIndexNumberRegex); }

        private void txtIndexNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Scan Operator pressed key {e.KeyChar}");
            switch (e.KeyChar)
            {
                case (char)(Keys.Return):
                    btnOk.PerformClick();
                    e.Handled = false;
                    break;
                case '+':
                    btnNavigateNext.PerformClick();
                    e.Handled = true;
                    break;
                case '-':
                    btnNavigatePrevious.PerformClick();
                    e.Handled = true;
                    break;
                case '*':
                    btnNavigateReset.PerformClick();
                    e.Handled = true;
                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                LoadRecords();
                txtTotalCount.Text = Records.Count.ToString();
                currentPosition = -1;

                if (TryStep() == false)
                {
                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"No records to index or missing req numbers total records {txtTotalCount.Text}");
                    ErrMessage.AppendLine("No Records to index");
                    EmailSend(false);
                    MessageBox.Show("No records to index");
                    FinalizeBatch();
                }
            }
            catch (Exception ex)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Showing form {ex.Message}");
                ErrMessage.AppendLine($"Error {ex.Message}");
                EmailSend(true);
                MessageBox.Show(ex.Message);
                Environment.ExitCode = -1;
                Application.Exit();
            }

        }
        bool finalizedBatch = false;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (finalizedBatch == false)
            {
                DialogResult dr = MessageBox.Show("Terminated process, your records will not be indexed or saved", "Process terminated", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    EDL.TraceLogger.TraceLoggerInstance.TraceWarning("Scan Operator terminated manuel indexing process, your records will not be indexed or saved");
                    ErrMessage.AppendLine("Terminated process, your records will not be indexed or saved");
                    EmailSend(true);
                    Environment.ExitCode = -1;
                }
                else
                {
                    EDL.TraceLogger.TraceLoggerInstance.TraceWarning("Scan Operator canceled terminated manuel indexing process");
                    e.Cancel = true;
                    return;
                }



            }
            TimeSpan ts = DateTime.Now - stManIndex;

            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Done nanualIndexing {ts.Hours}:{ts.Minutes}:{ts.Seconds}");
            if (Properties.Settings.Default.AuditLogFolder)
            {


                string ald = SettingsManager.AuditLogsUploadDirectroy.Replace("ManualIndexingTemp", "e-Docs USA");
                //ald = $"{Edocs.HelperUtilities.Utilities.CheckFolderPath(ald)}{Path.GetFileName(TraceFolder)}";
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Copying audit log {TraceFolder} to {ald}");
                CloseTraceLog();

                EdocsUSA.Utilities.Edocs_Utilities.EdocsUtilitiesInstance.CopyFiles(TraceFolder, ald, true, "Man*.log");

            }
            else
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"deleting files in folder {TraceFolder}");
                Edocs_Utilities.EdocsUtilitiesInstance.CleanUpLogFiles(TraceFolder, Properties.Settings.Default.DaysToKeepLogFiles, "*.*");
            }
        }
        private void CloseTraceLog()
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("ManuelIndex Completed");
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Closing trace logging file");
            EDL.TraceLogger.TraceLoggerInstance.CloseTraceFile();
        }

        private void txtRequisitionNumber_TextChanged(object sender, EventArgs e)
        {
            txtRequisitionNumberLength.Text = txtRequisitionNumber.Text.Length.ToString();
            chkValidRequisitionNumber.Checked = Regex.IsMatch(txtRequisitionNumber.Text.Trim(), ValidRequisitionNumberRegex);
        }

        private void txtRequisitionNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Scan operator pressed key {e.KeyChar}");
            switch (e.KeyChar)
            {
                case (char)(Keys.Enter):
                    btnUpdateFin.PerformClick();
                    e.Handled = false;
                    break;

                case '+':
                    btnNavigateNext.PerformClick();
                    e.Handled = true;
                    break;
                case '-':
                    btnNavigatePrevious.PerformClick();
                    e.Handled = true;
                    break;
                case '*':
                    btnNavigateReset.PerformClick();
                    e.Handled = true;
                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }

        private async Task CheckFinNum()
        {

            if (rBAddSevenZeros.Checked)
            {

                AddZeros(7).Wait();
            }
            else if (rBAddEightZeros.Checked)
            {

                AddZeros(8).Wait();
            }
            else if (rbCellTher.Checked)
            {
                CellThe().Wait();
            }
            else if (rbEmdeonReq.Checked)
            {

                EmdReq().Wait();
            }
            else if (rbError.Checked)
            {

                Error().Wait();
            }
            else if (rbBM.Checked)
            {
                BM().Wait();
            }
            else if (rbWC.Checked)
            {
                WC().Wait();
            }
            else if (rbPinks.Checked)
            {
                Pinks().Wait();
            }
            else if (rbProbLog.Checked)
            {

                ProbLog().Wait();
            }

            else if (rbCancelLog.Checked)
            {
                //  txtIndexNumber.Text = IndexNumber;
                //  txtRequisitionNumber.Text = ReqNumber;
                CancelLog().Wait();
            }

            else if (rbTestsLog.Checked)
            {
                //   txtIndexNumber.Text = IndexNumber;
                //  txtRequisitionNumber.Text = ReqNumber;
                TestLog().Wait();
            }

            else if (rbGracie.Checked)
                Gracie().Wait();
            else
                lblStatusMessage.Text = "Invalid button checked";
        }

        private async Task CancelLog()
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;
            if (CheckStringEmpy(txtIndexNumber.Text))
            {
                txtIndexNumber.Text = $"CANCEL";


            }
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Index number {txtIndexNumber.Text} afer adding CANCELLOG");
            SetFocusIndexIndexNumber();
        }

        private async Task TestLog()
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;
            if (CheckStringEmpy(txtIndexNumber.Text))
            {
                txtIndexNumber.Text = $"TSTLOG{DateTime.Now.ToString("MMddyyyy")}0";


            }
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Index number {txtIndexNumber.Text} afer adding TSTLOG{DateTime.Now.ToString("MMddyyyy")}0");
            SetFocusIndexIndexNumber();
        }
        private async Task ProbLog()
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;
            if (CheckStringEmpy(txtIndexNumber.Text))
            {
                //txtIndexNumber.Text = $"PRBLOG{DateTime.Now.ToString("MMddyyyy")}0";
                txtIndexNumber.Text = $"PRBLOG";



            }
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Index number {txtIndexNumber.Text} afer adding PRBLOG");
            SetFocusIndexIndexNumber();
        }
        private async Task Pinks()
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;
            if (CheckStringEmpy(txtIndexNumber.Text))
            {
                //if (!(string.IsNullOrWhiteSpace(txtRequisitionNumber.Text.Trim())))
                //    UseMrn("1180");
                //else
                //{
                txtIndexNumber.Text = "0001180";

                // }
            }
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Index number {txtIndexNumber.Text} afer adding 0001180");
            SetFocusIndexIndexNumber();

        }
        private async Task Error()
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(txtIndexNumber.Text))
            {
                txtIndexNumber.Text = $"{ErrorDocument}{DateTime.Now.ToString("MMddyyyy")}00";
                txtIndexNumber.Focus();

            }
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Index number {txtIndexNumber.Text} afer adding {ErrorDocument}{DateTime.Now.ToString("MMddyyyy")}00");
            SetFocusIndexIndexNumber();
        }

        private async Task BM()
        {
            txtRequisitionNumber.Text = string.Empty;
            txtIndexNumber.Text = "BMH0000";
            SetFocusIndexIndexNumber();
        }
        private async Task Gracie()
        {
            txtRequisitionNumber.Text = string.Empty;
            txtIndexNumber.Text = "Gracie1000";
            SetFocusIndexIndexNumber();
        }
        private async Task WC()
        {
            txtRequisitionNumber.Text = string.Empty;
            txtIndexNumber.Text = "NYPHWCD";
            SetFocusIndexIndexNumber();
        }
        private async Task EmdReq()
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;

            if (CheckStringEmpy(txtIndexNumber.Text))
            {
                //if (!(string.IsNullOrWhiteSpace(txtRequisitionNumber.Text.Trim())))
                //    UseMrn(string.Empty);
                //else
                //{
                txtIndexNumber.Text = txtIndexNumber.Text.PadLeft(5, '0');
                //  }


            }
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Index number {txtIndexNumber.Text} afer adding five zedros");
            SetFocusIndexIndexNumber();

        }

        private async Task CellThe()
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;
            if (CheckStringEmpy(txtIndexNumber.Text))
            {
                //if (!(string.IsNullOrWhiteSpace(txtRequisitionNumber.Text.Trim())))
                //    UseMrn("630");
                //else
                //{
                txtIndexNumber.Text = "0000630";

                //  }
            }
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Index number {txtIndexNumber.Text} afer adding 0000630");
            SetFocusIndexIndexNumber();
        }
        protected void LookupFin()
        {
            if (string.IsNullOrWhiteSpace(txtIndexNumber.Text))
            {
                MessageBox.Show("Index Number Required", "Required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIndexNumber.Focus();
            }
            else
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Checking index numer {txtIndexNumber.Text}");
                btnOk.PerformClick();
            }
        }

        private void btnLookupFin_Click(object sender, EventArgs e)
        {
            LookupFin();
        }


        private void btnNavigatePrevious_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentDisplayPosition <= 0)
                { throw new IndexOutOfRangeException("At begining"); }

                currentDisplayPosition -= 1;
                DisplayRecordFile();
            }
            catch (Exception ex)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"{ex.Message}");
                MessageBox.Show(this, ex.Message);
            }
        }

        private void btnNavigateReset_Click(object sender, EventArgs e)
        {
            try
            {
                currentDisplayPosition = this.currentPosition;
                DisplayRecordFile();
            }
            catch (Exception ex)

            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"{ex.Message}");
                MessageBox.Show(this, ex.Message);
            }
        }

        private void btnNavigateNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentDisplayPosition >= (Records.Count - 1))
                { throw new IndexOutOfRangeException("At end"); }

                currentDisplayPosition += 1;
                DisplayRecordFile();
            }
            catch (Exception ex)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"{ex.Message}");
                MessageBox.Show(this, ex.Message);
            }
        }

        private async Task SetRBtnColor(RadioButton radio)
        {
            if (radio.Checked)
            {
                if (!(string.IsNullOrEmpty(EDL.TraceLogger.TraceLoggerInstance.RunningAssembley)))
                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Setting radio button:{radio.Name} to red");
                radio.BackColor = Color.Red;
            }
            else
            {
                if (!(string.IsNullOrEmpty(EDL.TraceLogger.TraceLoggerInstance.RunningAssembley)))
                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Setting radio button:{radio.Name} to light green");
                radio.BackColor = Color.LightGreen;
            }
        }
        private bool CheckValidReqNumber(string reqNumber)
        {
            return (Regex.IsMatch(reqNumber, ValidRequisitionNumberRegex));
        }
        private bool CheckValidCSNNumber(string csnNumber)
        {
            return (Regex.IsMatch(csnNumber, ValidCSNNumberRegex));
        }

        //protected string AddZeros(string inStr, int maxDigits, int maxStrLength)
        //{
        //    string zeros = string.Empty;
        //    if (inStr.Length < maxStrLength)
        //    {
        //        int loop = inStr.Length;
        //        while (loop < maxDigits)
        //        {
        //            zeros += "0";
        //            loop++;
        //        }
        //        inStr = $"{zeros.Trim()}{inStr}";
        //    }

        //    return inStr;
        //}
        private void UseMrn(string addNumber)
        {
            try
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Using req number Adding  number {addNumber} to mrn number");
                string reqNumber = txtRequisitionNumber.Text.Trim();
                if (string.IsNullOrWhiteSpace(reqNumber))
                {
                    EDL.TraceLogger.TraceLoggerInstance.TraceError("Mrn number invalid");
                    throw new Exception("Req# cannot be blank");


                }
                else
                {
                    if (!(CheckValidReqNumber(reqNumber)))
                    {
                        EDL.TraceLogger.TraceLoggerInstance.TraceWarning($"Invalid req number {reqNumber} should match regx expression {ValidIndexNumberRegex} ");
                        throw new Exception("Invalid requisition number");
                    }
                    if (string.IsNullOrWhiteSpace(addNumber))
                        txtIndexNumber.Text = AddZeros(reqNumber, 15, 15);
                    else
                    {
                        reqNumber = $"{addNumber}{reqNumber}";
                        txtIndexNumber.Text = AddZeros(reqNumber, 15, 15);
                    }
                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Index number{txtIndexNumber.Text} afer using req number {reqNumber}");

                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Index number{txtIndexNumber.Text} afer using req number {reqNumber}");
                    txtIndexNumber.Focus();
                }
            }
            catch (Exception ex)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Error getting index number or rec number {ex.Message}");
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIndexNumber.Focus();
            }
        }
        private bool CheckStringEmpy(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return true;
            }
            txtIndexNumber.Focus();
            lblStatusMessage.Text = $"Index # has to be empty {str}";
            EDL.TraceLogger.TraceLoggerInstance.TraceWarning($"Index number{txtIndexNumber.Text} not empty");
            return false;
        }

        private void SetFocusIndexIndexNumber()
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Setting focus to Index number{txtIndexNumber.Text}");
            txtIndexNumber.SelectionStart = txtIndexNumber.Text.Length;
            txtIndexNumber.Focus();
        }
        private async Task AddZeros(int numZeros)
        {
            //if (CheckStringEmpy(txtIndexNumber.Text))
            //{
            //    if (!(string.IsNullOrWhiteSpace(txtRequisitionNumber.Text.Trim())))
            //        UseMrn(string.Empty);
            //else
            txtIndexNumber.Text = string.Empty;
            txtBoxCSN.Clear();
            txtIndexNumber.Text = txtIndexNumber.Text.PadLeft(numZeros, '0');

            //}
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"New Index number{txtIndexNumber.Text} after adding {numZeros.ToString()} zeros");
            SetFocusIndexIndexNumber();
        }
        private void rbAddManeul_CheckedChanged(object sender, EventArgs e)
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;
            txtBoxCSN.Clear();
            SetRBtnColor(rbAddManeul).Wait();

        }

        private void rBAddSevenZeros_CheckedChanged(object sender, EventArgs e)
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;
            txtBoxCSN.Clear();
            SetRBtnColor(rBAddSevenZeros).Wait();
            if (rBAddSevenZeros.Checked)
            {
                AddZeros(7).Wait();
            }
        }

        private void rBAddEightZeros_CheckedChanged(object sender, EventArgs e)
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;
            txtBoxCSN.Clear();
            SetRBtnColor(rBAddEightZeros).Wait();
            if (rBAddEightZeros.Checked)
            {
                AddZeros(8).Wait();
            }
        }

        private void rbEmdeonReq_CheckedChanged(object sender, EventArgs e)
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;
            txtBoxCSN.Clear();
            SetRBtnColor(rbEmdeonReq).Wait();
            if (rbEmdeonReq.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtIndexNumber.Text))
                    AddZeros(5).Wait();
            }

        }

        private void rbPinks_CheckedChanged(object sender, EventArgs e)
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;
            txtBoxCSN.Clear();
            SetRBtnColor(rbPinks).Wait();
            if (rbPinks.Checked)
            {
                Pinks().Wait();
            }
        }

        private void rbCellTher_CheckedChanged(object sender, EventArgs e)
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;
            txtBoxCSN.Clear();
            SetRBtnColor(rbCellTher).Wait();
            if (rbCellTher.Checked)
            {
                CellThe().Wait();
            }
        }

        private void rbError_CheckedChanged(object sender, EventArgs e)
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;
            txtBoxCSN.Clear();
            SetRBtnColor(rbError).Wait();
            if (rbError.Checked)
            {
                Error().Wait();
            }
        }

        private void rbCancelLog_CheckedChanged(object sender, EventArgs e)
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;
            txtBoxCSN.Clear();
            SetRBtnColor(rbCancelLog).Wait();
            if (rbCancelLog.Checked)
            {
                CancelLog().Wait();
            }
        }

        private void rbProbLog_CheckedChanged(object sender, EventArgs e)
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;
            txtBoxCSN.Clear();
            SetRBtnColor(rbProbLog).Wait();
            if (rbProbLog.Checked)
            {
                ProbLog().Wait();
            }
        }

        private void rbTestsLog_CheckedChanged(object sender, EventArgs e)
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;
            txtBoxCSN.Clear();
            SetRBtnColor(rbTestsLog).Wait();
            if (rbTestsLog.Checked)
            {
                TestLog().Wait();
            }
        }



        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.F1:
                    rbAddManeul.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.F2:
                    rbError.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.F3:
                    rbCancelLog.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.F4:
                    rbPinks.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.F5:
                    rbEmdeonReq.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.F6:
                    rbCellTher.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.F7:
                    rBAddSevenZeros.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.F8:
                    rBAddEightZeros.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.F9:
                    rbTestsLog.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.F10:
                    rbProbLog.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.F11:
                    rbBM.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.F12:
                    rbGracie.PerformClick();
                    e.Handled = true;
                    break;


                    //case Keys.ControlKey:
                    //    rbWC.PerformClick();
                    //    e.Handled = true;
                    //    break;



            }

        }

        private void rbBM_CheckedChanged(object sender, EventArgs e)
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;
            txtBoxCSN.Clear();
            SetRBtnColor(rbBM).Wait();
            if (rbBM.Checked)
            {
                BM().Wait();
            }
        }

        private void rbWC_CheckedChanged(object sender, EventArgs e)
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;
            txtBoxCSN.Clear();
            SetRBtnColor(rbWC).Wait();
            if (rbWC.Checked)
            {
                WC().Wait();
            }
        }

        private void rbGracie_CheckedChanged(object sender, EventArgs e)
        {
            txtIndexNumber.Text = string.Empty;
            txtRequisitionNumber.Text = string.Empty;
            txtBoxCSN.Clear();
            SetRBtnColor(rbGracie).Wait();
            if (rbGracie.Checked)
            {
                Gracie().Wait();
            }
        }


        private void rbEmdeonReq_Click(object sender, EventArgs e)
        {
            EmdReq().Wait();
        }

        private void rbError_Click(object sender, EventArgs e)
        {
            Error().Wait();
        }

        private void rBAddSevenZeros_Click(object sender, EventArgs e)
        {
            AddZeros(7).Wait();
        }

        private void rBAddEightZeros_Click(object sender, EventArgs e)
        {
            AddZeros(8).Wait();
        }

        private void rbProbLog_Click(object sender, EventArgs e)
        {
            ProbLog().Wait();
        }

        private void rbCancelLog_Click(object sender, EventArgs e)
        {
            CancelLog().Wait();
        }

        private void rbTestsLog_Click(object sender, EventArgs e)
        {
            TestLog().Wait();
        }

        private void txtBoxCSN_TextChanged(object sender, EventArgs e)
        {
            txtCSNLength.Text = txtBoxCSN.Text.Length.ToString();
            chkValidCSNNumber.Checked = CheckValidCSNNumber(txtBoxCSN.Text.Trim());
            //Regex.IsMatch(txtBoxCSN.Text.Trim(), ValidRequisitionNumberRegex);
        }

        private void txtBoxCSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Scan operator pressed key {e.KeyChar}");
            switch (e.KeyChar)
            {
                case (char)(Keys.Enter):
                    btnUpdateFin.PerformClick();
                    e.Handled = false;
                    break;

                case '+':
                    btnNavigateNext.PerformClick();
                    e.Handled = true;
                    break;
                case '-':
                    btnNavigatePrevious.PerformClick();
                    e.Handled = true;
                    break;
                case '*':
                    btnNavigateReset.PerformClick();
                    e.Handled = true;
                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }
    }
}
