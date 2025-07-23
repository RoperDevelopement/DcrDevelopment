using EdocsUSA.Utilities;
//using ETL = EdocsUSA.Utilities.Edocs_Utilities;
using EDL = EdocsUSA.Utilities.Logging;
using EdocsUSA.Utilities.Extensions;
using Microsoft;
using Scanquire.Public;

using Scanquire.Public.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
 
using Edocs.Ocr.Pdf;
using FreeImageAPI;
using System.Net.NetworkInformation;
using Newtonsoft.Json.Linq;

namespace Scanquire
{
    public partial class MainForm : Form
    {

        /// <summary>The currently active SQArchiver.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISQArchiver SelectedArchiver
        { get { return CurrentArchiverSelector.SelectedArchiver; } }

        /// <summary>
        /// Specialized archiver to use for all To/From Local Archiver operations.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISQArchiver LocalArchiver
        { get { return SQArchivers.Instance["Local"]; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected Properties.Scanquire DefaultSettings = Properties.Scanquire.Default;

        /// <summary>
        /// If true: Clear the active batch anytime a save operation completes.
        /// If false: Active batch will remain after a save operation complates.
        /// </summary>
        public bool ClearOnSave
        {
            get { return ClearOnSaveMenuItem.Checked; }
            set { ClearOnSaveMenuItem.Checked = value; }
        }

        private int ScanDocumentSettings
        { get; set; }
        public MainForm()
        {
            InitializeComponent();
            this.Text = Properties.Scanquire.Default.ScanQuireVersion;
            this.Visible = true;
            ScanDocumentSettings = 2;


        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //Set e.Cancel depending on wether we should really close or not.
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Closing Scanquire form:{this.Name} reason {e.CloseReason.ToString()}");
            switch (e.CloseReason)
            {
                case CloseReason.TaskManagerClosing:
                case CloseReason.WindowsShutDown:
                    //If force close, go ahead and close
                    e.Cancel = false;
                    break;
                case CloseReason.ApplicationExitCall:
                case CloseReason.FormOwnerClosing:
                case CloseReason.MdiFormClosing:
                case CloseReason.None:
                case CloseReason.UserClosing:
                default:
                    //If the image list viewer is saved, go ahead and close
                    if (ImageListViewer.Saved == true)
                    { e.Cancel = false; }
                    else //ImageListViewe is not saved
                    {
                        //Confirm the exit with the user
                        DialogResult r = MessageBox.Show("The active document has changes that have not been saved.  Are you sure you want to exit?", "Confirm", MessageBoxButtons.YesNo);
                        if (r == DialogResult.Yes)
                        {
                            EDL.TraceLogger.TraceLoggerInstance.TraceWarning("Exit scanquire without saving images");
                            e.Cancel = false;
                        }
                        else
                        {

                            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Cancel Exit scanquire as imageds are not saved");
                            e.Cancel = true;
                            if (DefaultSettings.DataBase)
                            {
                                CheckInactiveTimeOut();
                            }


                        }
                    }
                    break;
            }

            //If really exiting, clear the ImageListBox to dispose all image files.
            //Save the last selected archiverScanquire_HIPAA
            if (e.Cancel == false)
            {
                CloseForm();
            }
            base.OnFormClosing(e);

        }

        private void CloseForm()
        {
            ImageListViewer.ClearAll(false, true);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Saving last archiver " + CurrentArchiverSelector.SelectedArchiverName);
            DefaultSettings.PreviousArchiverName = CurrentArchiverSelector.SelectedArchiverName;
            DefaultSettings.UserScanSetting = ScanDocumentSettings;
            DefaultSettings.UserAutoQASetting = tsRunAutoQA.Checked;
            DefaultSettings.UseTwain = tsMenuTwainScanner.Checked;
            DefaultSettings.Save();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Closed scanquire");
            EDL.TraceLogger.TraceLoggerInstance.Dispose();
            if (Scanquire.Properties.Scanquire.Default.AuditLogs)
                Edocs_Utilities.EdocsUtilitiesInstance.CopyFiles(SettingsManager.AuditLogsDirectroy, SettingsManager.AuditLogsUploadDirectroy, true, "*.*");


        }
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.IsInDesignMode() == false)
            {
                //Load all available archivers and select the last used one from the previous session.
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Loading Archivers");
                await CurrentArchiverSelector.LoadAllArchivers();
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Setting archiver to " + DefaultSettings.PreviousArchiverName);
                CurrentArchiverSelector.SelectedArchiverName = DefaultSettings.PreviousArchiverName;
            }

        }

        private void EnableDisableButtons(bool enableDisable)
        {
            NewDocumentMenuButton.Enabled = enableDisable;
            AppendMenuButton.Enabled = enableDisable;
            TSSettingPrintImage.Enabled = enableDisable;
            InsertMenuButton.Enabled = enableDisable;
            SaveMenuButton.Enabled = enableDisable;
            tsBtnEditImage.Enabled = enableDisable;
            CurrentArchiverSelector.Enabled = enableDisable;
            scannerOptions.Enabled = enableDisable;
            SettingsMenuButton.Enabled = enableDisable;
            tsDelCheckImages.Enabled = enableDisable;
            if (DefaultSettings.DataBase)
                tsManageUsers.Enabled = enableDisable;
            DisplayHelpMenuButton.Enabled = enableDisable;
            EMailImages.Enabled = enableDisable;
            ResetTimeIdle();

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting key pressed for dialog:{this.Name}");
            //Check for hotkeys.
            switch (keyData)
            {
                case Keys.Control | Keys.I: //Insert
                    InsertFromScannerMenuItem.PerformClick();
                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Pressed Control I {keyData}");
                    return true;
                case Keys.Control | Keys.G: //Append From Scanner
                    AppendFromScannerMenuItem.PerformClick();
                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Pressed Control I {keyData}");
                    return true;
                case Keys.Control | Keys.N: //New From Scanner
                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Pressed Control N {keyData}");
                    NewFromScannerMenuItem.PerformClick();
                    return true;
                case Keys.Control | Keys.O: //New From Archive
                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Pressed Control O {keyData}");
                    NewFromArchiveMenuItem.PerformClick();
                    return true;
                case Keys.Control | Keys.S: //Save To Archive.
                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Pressed Control s {keyData}");
                    SaveToArchiveMenuItem.PerformClick();
                    return true;
                default:
                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation(string.Format("Key pressed:{0}", keyData));
                    return base.ProcessCmdKey(ref msg, keyData);
            }
        }


        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            base.OnHelpRequested(hevent);
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Show help file");
            ShowHelpFile();
            hevent.Handled = true;
        }



        protected delegate Task AcquireTask(IEnumerable<Task<SQImage>> source);

        private async Task Acquire(ISQArchiver archiver, SQAcquireIntent intent, SQAcquireSource source)
        {
            Edocs_Utilities.EdocsUtilitiesInstance.DecriptImage = false;
            if (tsImageRecovery.Checked)
                Edocs_Utilities.EdocsUtilitiesInstance.DecriptImage = true;
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Image arvhiver {archiver.ToString()}");
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Image intent {intent.ToString()}");
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Image source {source.ToString()}");
            RunGcc();
            EnableDisableButtons(false);
            //this.Enabled = false;
            //Save current state of ImageListViewer to restore upon completion.
            SQImageListViewer.ImageThumbnailViewMode previousViewMode = ImageListViewer.ViewMode;
            SQImageListViewer.ThumbnailSizeMode previousThumbnailSize = ImageListViewer.CurrentThumbnailSizeMode;

            //Set the ImageListViewer to large thumbnails only for the duration of the acquire.
            ImageListViewer.ViewMode = SQImageListViewer.ImageThumbnailViewMode.Thumbnails;
            ImageListViewer.CurrentThumbnailSizeMode = SQImageListViewer.ThumbnailSizeMode.Large;

            //Configure the progress and cancelation token for the acquire process.
            CancellationTokenSource cTokenSource = new CancellationTokenSource();
            CancellationToken cToken = cTokenSource.Token;
            Progress<ProgressEventArgs> acquireProgress = new Progress<ProgressEventArgs>();
            ProgressMonitor.StartMonitoring(acquireProgress, cTokenSource);

            //Timer to track how long the acquire takes.
            DateTime stTime = DateTime.Now;
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation(string.Format("Start time: {0}", stTime.ToString()));
            MemoryUsed("Memory usage before scanning images:");

            if (tsMenuTwainScanner.Checked)
                SQTwain.Instance.UseTwain = true;
            else
                SQTwain.Instance.UseTwain = false;

            Stopwatch acquireTimer = new Stopwatch();
            acquireTimer.Start();
            if (ScanDocumentSettings == 0)
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Check for blank documents");
            else if (ScanDocumentSettings == 1)
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Remove blank documents");
            else
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Scan blank documents");

            try
            {
                //If this is for a new file, clear the active images.
                if (intent == SQAcquireIntent.New)
                { ImageListViewer.ClearAll(true, true); }

                //Initiate the acquire tasks.  This may not actually start pulling images untill enumration.
                IEnumerable<Task<SQImage>> acquireTasks = archiver.Acquire(intent, source, acquireProgress, cToken);

                //Process the acquired images depending on the intent.
                switch (intent)
                {
                    case SQAcquireIntent.Append:
                        foreach (Task<SQImage> acquireTask in acquireTasks)
                        {
                            SQImage image = await acquireTask;
                            if (ScanDocumentSettings != 2)
                            {
                                //   if (AutoQaCheckBlankImages.AutoQaCheckBlankImagesInstance.IsImageBlank(image))
                                var results = await Task.Factory.StartNew(() => AutoQaCheckBlankImages.AutoQaCheckBlankImagesInstance.IsImageBlankAynsc(image));
                                if (results.Result)
                                {

                                    if (ScanDocumentSettings == 0)
                                    {
                                        ImageListViewer.Add(image);
                                        SQImageListViewerItem[] item = ImageListViewer.Items.ToArray();
                                        ImageListViewer.UnCheck(item[item.Length - 1]);
                                    }
                                }
                                else
                                {
                                    if (tsRunAutoQA.Checked)
                                        image = await Task.Factory.StartNew(() => AutoQaCheckBlankImages.AutoQaCheckBlankImagesInstance.AutoQa(image, acquireProgress));
                                    ImageListViewer.Add(image);

                                }


                            }
                            else
                            {
                                if (tsRunAutoQA.Checked)
                                    image = await Task.Factory.StartNew(() => AutoQaCheckBlankImages.AutoQaCheckBlankImagesInstance.AutoQa(image, acquireProgress));
                                ImageListViewer.Add(image);
                            }

                        }
                        break;
                    case SQAcquireIntent.Insert:
                        int insertIndex = ImageListViewer.ActiveItemIndex;
                        foreach (Task<SQImage> acquireTask in acquireTasks)
                        {
                            SQImage image = await acquireTask;
                            ImageListViewer.Insert(insertIndex, image);
                            if (ScanDocumentSettings == 0)
                            {
                                var results = await Task.Factory.StartNew(() => AutoQaCheckBlankImages.AutoQaCheckBlankImagesInstance.IsImageBlankAynsc(image));
                                //if (AutoQaCheckBlankImages.AutoQaCheckBlankImagesInstance.IsImageBlank(image))
                                if (results.Result)
                                {
                                    SQImageListViewerItem[] item = ImageListViewer.Items.ToArray();
                                    ImageListViewer.UnCheck(item[insertIndex]);
                                }
                            }
                            insertIndex++;
                        }
                        break;
                    case SQAcquireIntent.New:

                        goto case SQAcquireIntent.Append;
                    default:
                        EDL.TraceLogger.TraceLoggerInstance.TraceWarning("Unexpected SQAcquireIntent " + intent);
                        goto case SQAcquireIntent.Append;
                }
                ProgressMonitor.StopMonitoring(acquireProgress, Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessCompleted);
            }
            catch (OperationCanceledException oe)
            {

                EDL.TraceLogger.TraceLoggerInstance.TraceWarning($"Scanning OperationCanceledException {oe.Message}");
                ProgressMonitor.StopMonitoring(acquireProgress, Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessCanceled);

            }
            catch (Exception ex)
            {
                ProgressMonitor.StopMonitoring(acquireProgress, Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessFailed);
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Acquire error message:{ex.Message}");
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Acquire stack trace:{ex.StackTrace}");
                MessageBox.Show(ex.Message);
            }

            //Log the acquire duration.
            TimeSpan ts = DateTime.Now - stTime;
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Scanning end time " + DateTime.Now.ToString());
            acquireTimer.Stop();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Acquire completed in " + acquireTimer.ElapsedMilliseconds);

            //Reset the ImageListViewer state.
            ImageListViewer.ViewMode = previousViewMode;
            ImageListViewer.CurrentThumbnailSizeMode = previousThumbnailSize;
            ImageListViewer.Enabled = true;

            EDL.TraceLogger.TraceLoggerInstance.TraceInformation(string.Format("Total time: {0}HH:{1}MM:{2}SS", ts.Hours, ts.Minutes, ts.Seconds));
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation(string.Format("Total milliseconds: {0}ms:", ts.TotalMilliseconds));
            MemoryUsed("Memory usage after scanning images:");
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation(string.Format("Total Images Scanned: {0}", ImageListViewer.ItemCount));
            //  this.Enabled = true;
            EnableDisableButtons(true);
            if (tsImageRecovery.Checked)
            {
                SetRecoverImages();
            }

        }

        protected void MemoryUsed(string message)
        {
            try
            {
                Process currentProc = Process.GetCurrentProcess();
                currentProc.Refresh();
                long memoryUsed = currentProc.PrivateMemorySize64 / (int)(1024);
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation(string.Format("{0} {1}", message, memoryUsed));
            }
            catch (Exception ex)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Checking memory usage:{ex.Message}");
            }
        }
        private void RunGcc()
        {
            try
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Running garabge collection");
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Running gcc:{ex.Message}");
            }
        }

        protected async Task SaveSelected(ISQArchiver archiver)
        {

            CancellationTokenSource cTokenSource = new CancellationTokenSource();
            CancellationToken cToken = cTokenSource.Token;
            Progress<ProgressEventArgs> progress = new Progress<ProgressEventArgs>();
            ProgressMonitor.StartMonitoring(progress, cTokenSource);
            bool tempClear = ClearOnSave;
            try
            {
                EDL.TraceLogger.TraceLoggerInstance.TotalImages = ImageListViewer.Selected.ToArray().Length;
                if (EDL.TraceLogger.TraceLoggerInstance.TotalImages < 2)
                {
                    DialogResult dr = MessageBox.Show($"You only have {EDL.TraceLogger.TraceLoggerInstance.TotalImages} Image Selected", "Save", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Cancel)
                    {
                        ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessCompleted);

                        return;
                    }
                }
                //Initialize the progress and cancelation for the save process.
                ImageListViewer.Enabled = false;
                SQImageListViewerItem[] selImages = ImageListViewer.Items.ToArray();
                await archiver.Send(ImageListViewer.SelectedImages.ToArray(), progress, cToken);

                ImageListViewer.Saved = true;
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Done sending Selected Images to archiver:{archiver.ToString()}");

                if (ClearOnSave)
                {
                    DialogResult drClearImages = MessageBox.Show($"Delete the {EDL.TraceLogger.TraceLoggerInstance.TotalImages} Images that have been archiver ", "Delete Images", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (drClearImages == DialogResult.No)
                    {
                        ClearOnSave = false;
                    }
                }
                if (ClearOnSave)
                {
                    for (int i = 0; i < selImages.Count(); i++)
                        if (selImages[i].Selected)
                        {
                            SQImage image = selImages[i].Value;
                            SQImageListViewerItem sQImageListViewer = selImages[i];

                            SQImageEditLock image_Lock = image.BeginEdit();
                            ImageListViewer.Remove(sQImageListViewer);
                            image.DiscardEdit(image_Lock);
                        }
                }
                if (ImageListViewer.ItemCount == 0)
                {
                    ImageListViewer.ClearAll(false, true);
                    NewDocumentMenuButton.Enabled = true;
                }
                else
                {
                    EnableDisableButtons(true);
                }
                ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessCompleted);


            }
            catch (OperationCanceledException oe)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning($"Archiver OperationCanceledException :{oe.Message}");
                ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessCanceled);
                MessageBox.Show(this, "Save Cancelled");
                NewDocumentMenuButton.Enabled = true;
                EnableDisableButtons(true);
            }
            catch (Exception ex)
            {
                ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessFailed);
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Save Selected Images to archiver failed meesage:{ex.Message}");
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Save Selected Images to archiver stack trace:{ex.StackTrace}");
                MessageBox.Show("Save failed " + Environment.NewLine + ex.Message);
                NewDocumentMenuButton.Enabled = true;
                EnableDisableButtons(true);
            }
            //Initialize the progress and cancelation for the save process.
            ImageListViewer.Enabled = true;
            CurrentArchiverSelector.Enabled = true;

            ClearOnSave = tempClear;
            // this.Enabled = true;
            //  ImageListViewer.Enabled = true;
            ResetTimeIdle();
        }
        protected async Task Save(ISQArchiver archiver)
        {
            //Initialize the progress and cancelation for the save process.
            CancellationTokenSource cTokenSource = new CancellationTokenSource();
            CancellationToken cToken = cTokenSource.Token;
            Progress<ProgressEventArgs> progress = new Progress<ProgressEventArgs>();
            ProgressMonitor.StartMonitoring(progress, cTokenSource);

            try
            {
                NewDocumentMenuButton.Enabled = false;
                EnableDisableButtons(false);
                EDL.TraceLogger.TraceLoggerInstance.TotalImages = ImageListViewer.CheckedImages.ToArray().Length;
                await archiver.Send(ImageListViewer.CheckedImages.ToArray(), progress, cToken);
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Done sending to archiver:{archiver.ToString()}");
                ImageListViewer.Saved = true;
                if (ClearOnSave)
                {
                    ImageListViewer.ClearAll(false, true);

                }
                ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessCompleted);
            }
            catch (OperationCanceledException oe)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning($"Archiver OperationCanceledException :{oe.Message}");
                ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessCanceled);
                MessageBox.Show(this, "Save Cancelled");
                //   NewDocumentMenuButton.Enabled = true;
                //   EnableDisableButtons(true);
            }
            catch (Exception ex)
            {
                ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessFailed);
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Save to archiver failed meesage:{ex.Message}");
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Save to archiver stack trace:{ex.StackTrace}");
                MessageBox.Show("Save failed " + Environment.NewLine + ex.Message);
                //  NewDocumentMenuButton.Enabled = true;
                // EnableDisableButtons(true);
            }
            if (ImageListViewer.ItemCount > 0)
                EnableDisableButtons(true);
            else
                EnableDisableButtons(false);
            NewDocumentMenuButton.Enabled = true;
            ImageListViewer.Refresh();
            ResetTimeIdle();
        }

        private async void NewFromScannerMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewLogFile();
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Start acquiring new image from scanner");

            await Acquire(SelectedArchiver, SQAcquireIntent.New, SQAcquireSource.Scanner);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Done acquiring new image from scanner");
        }

        private async void NewFromArchiveMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewLogFile();
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Start acquiring images from archive folder");
            EnableDisableButtons(false);
            await Acquire(SelectedArchiver, SQAcquireIntent.New, SQAcquireSource.File);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Done acquiring images from archive folder");
            EnableDisableButtons(true);
        }

        private async void NewFromLocalFileMenuItem_Click(object sender, EventArgs e)
        {

            CreateNewLogFile();
            CheckInactiveTimeOut();
            EnableDisableButtons(false);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Start acquiring images from local folder");
            await Acquire(LocalArchiver, SQAcquireIntent.New, SQAcquireSource.File);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Done acquiring images from local folder");
            EnableDisableButtons(true);
        }

        private async void NewFromCommandMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Start acquiring images from command file");
            await Acquire(SelectedArchiver, SQAcquireIntent.New, SQAcquireSource.Command);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Done acquiring images from command file");
            ResetTimeIdle();
        }

        private async void NewFromCustomMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Start acquiring images from custom");
            await Acquire(SelectedArchiver, SQAcquireIntent.New, SQAcquireSource.Custom);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Done acquiring images from custom");
            ResetTimeIdle();
        }

        private async void AppendFromScannerMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Start to Append images from scanner");
            await Acquire(SelectedArchiver, SQAcquireIntent.Append, SQAcquireSource.Scanner);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Done Append images from scanner");
        }

        private async void AppendFromArchiveMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Append images from archive folder");
            await Acquire(SelectedArchiver, SQAcquireIntent.Append, SQAcquireSource.File);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Done Append images from archive folder");
        }

        private async void AppendFromLocalFileMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Append images from local folder");
            await Acquire(LocalArchiver, SQAcquireIntent.Append, SQAcquireSource.File);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Done Append images from local folder");
        }

        private async void AppendFromCommandMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Append images from local folder");
            await Acquire(SelectedArchiver, SQAcquireIntent.Append, SQAcquireSource.Command);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Done Append images from local folder");
            ResetTimeIdle();
        }

        private async void AppendFromCustomMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Append images from custom");
            await Acquire(SelectedArchiver, SQAcquireIntent.Append, SQAcquireSource.Custom);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Done images from local folder");
            ResetTimeIdle();
        }

        private async void InsertFromScannerMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Insert images from scanner");
            await Acquire(SelectedArchiver, SQAcquireIntent.Insert, SQAcquireSource.Scanner);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Done images from scanner");
        }

        private async void InsertFromArchiveMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Insert images from local folder");
            await Acquire(SelectedArchiver, SQAcquireIntent.Insert, SQAcquireSource.File);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Done Insert images from local folder");
        }

        private async void InsertFromLocalFileMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Insert images from archive folder");
            await Acquire(LocalArchiver, SQAcquireIntent.Insert, SQAcquireSource.File);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Done Insert images from archive folder");
        }

        private async void InsertFromCommandMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Insert images from command");
            await Acquire(SelectedArchiver, SQAcquireIntent.Insert, SQAcquireSource.Command);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Done Insert images from command");
            ResetTimeIdle();
        }

        private async void InsertFromCustomMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Insert images from custom");
            await Acquire(SelectedArchiver, SQAcquireIntent.Insert, SQAcquireSource.Custom);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Done Insert images from custom");
            ResetTimeIdle();
        }

        private async void SaveToArchiveMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();

            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Start saving to archive folder: {CurrentArchiverSelector.SelectedArchiverName}");
            DialogResult result = MessageBox.Show($"Do You want to save to archiver {CurrentArchiverSelector.SelectedArchiverName}?", "Save Archiver", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning($"User Cancel save to archiver {CurrentArchiverSelector.SelectedArchiverName}");
                return;
            }

            MemoryUsed("Memory usage before archive:");
            RunGcc();

            // EnableDisableButtons(false);
            await Save(SelectedArchiver);
            RunGcc();
            MemoryUsed("Memory usage after archive:");
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Done saving to archiver: {CurrentArchiverSelector.SelectedArchiverName}");
            ResetTimeIdle();
            if (!(CurrentArchiverSelector.Enabled))
                CurrentArchiverSelector.Enabled = true;
            //   NewDocumentMenuButton.Enabled = true;
        }

        private async void SaveToLocalFileMenuItem_Click(object sender, EventArgs e)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Start saving to local folder");
            CheckInactiveTimeOut();
            DialogResult result = MessageBox.Show($"Do You want to save to archiver {CurrentArchiverSelector.SelectedArchiverName}?", "Save Archiver", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning($"User Cancel save to archiver {CurrentArchiverSelector.SelectedArchiverName}");
                return;
            }
            //EnableDisableButtons(false);
            //  EnableDisableButtons(false);
            MemoryUsed("Memory usage before save local file:");
            await Save(LocalArchiver);
            RunGcc();
            MemoryUsed("Memory usage after save local file:");
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Done saving to archiver: {CurrentArchiverSelector.SelectedArchiverName}");
            ResetTimeIdle();

            NewDocumentMenuButton.Enabled = true;

        }

        private void ViewSessionLog()
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("View log file");
            SessionLogViewer viewer = new SessionLogViewer();
            viewer.ShowDialog(this);
            ResetTimeIdle();
        }

        private void DisplayHelpMenuButton_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Opening Help File");
            ShowHelpFile();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Closing Help File");
            ResetTimeIdle();
        }

        HelpDialog HelpDialog = new HelpDialog();

        protected void ShowHelpFile()
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Opening Help File");
            HelpDialog.ShowDialog();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Closing Help File");
            ResetTimeIdle();
        }

        private void ImageListViewer_CheckedItemsChanged(object sender, EventArgs e)
        {

            IncludedPagesLabel.Text = ImageListViewer.CheckedItemCount.ToString().PadLeft(3, '0');
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Image List Viewer checked items changed {IncludedPagesLabel.Text}");
        }

        private void ImageListViewer_SelectedItemsChanged(object sender, EventArgs e)
        {

            SelectedPagesLabel.Text = ImageListViewer.SelectedItemCount.ToString().PadLeft(3, '0');
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Image List Viewer checked delected items changed {SelectedPagesLabel.Text}");
        }

        private void ImageListViewer_ActiveItemChanged(object sender, EventArgs e)
        {

            ActivePageNumberLabel.Text = ImageListViewer.ActiveItem == null ? "-1".PadLeft(3, ' ')
                : (ImageListViewer.ActiveItem.Index + 1).ToString().PadLeft(3, '0');
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Image List Viewer active item changed {ActivePageNumberLabel.Text}");
        }

        private void ImageListViewer_ItemCountChanged(object sender, EventArgs e)
        {

            if (SaveMenuButton.Enabled)
            {
                SaveMenuButton.Enabled = ImageListViewer.ItemCount > 0;
                AppendMenuButton.Enabled = ImageListViewer.ItemCount > 0;
                InsertMenuButton.Enabled = ImageListViewer.ItemCount > 0;
                tsDelCheckImages.Enabled = ImageListViewer.ItemCount > 0;
                tsBtnEditImage.Enabled = ImageListViewer.ItemCount > 0;
                TSSettingPrintImage.Enabled = ImageListViewer.ItemCount > 0;
                tsBtnEditImage.Enabled = ImageListViewer.ItemCount > 0;
                tsDelCheckImages.Enabled = ImageListViewer.ItemCount > 0;
                EMailImages.Enabled = ImageListViewer.ItemCount > 0;

            }


            TotalPagesLabel.Text = ImageListViewer.ItemCount.ToString().PadLeft(3, '0');
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Image List Viewer list item count changed item changed {TotalPagesLabel.Text}");
        }

        private void ViewSessionLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();

            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Viewing session log");
            ViewSessionLog();
        }


        private void NewDocumentMenuButton_DoubleClick(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("New from scanner clicked");
            NewFromScannerMenuItem.PerformClick();
        }

        private void SaveMenuButton_DoubleClick(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Saved to archived clicked");
            SaveToArchiveMenuItem.PerformClick();
        }

        private void AppendMenuButton_DoubleClick(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Append from scanner clicked");
            AppendFromScannerMenuItem.PerformClick();
        }

        private void InsertMenuButton_DoubleClick(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Insert imaged clicked");
            InsertFromScannerMenuItem.PerformClick();
        }


        private void RemoveBlankImages()
        {
            ResetTimeIdle();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Remove blank images");
            int numBlankImagesRemoved = 0;
            if (ImageListViewer.UnChecked.ToArray().Length > 0)
            {
                numBlankImagesRemoved++;
                SQImageListViewerItem[] item = ImageListViewer.Items.ToArray();


                for (int items = 0; items < item.Length; items++)
                {
                    if (!(item[items].Checked))
                    {
                        numBlankImagesRemoved++;
                        ImageListViewer.Remove(item[items]);
                    }
                }


            }
            else
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning("No images selected to delete");
                MessageBox.Show("No images selected", "No Images", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Number of blank images removed {numBlankImagesRemoved.ToString()}");
            ImageListViewer.Refresh();
        }

        private void AutoQa(IEnumerable<SQImage> images, CancellationToken cToken)
        {

            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Running auto qa");

            var parallelQuery = images.AsParallel();
            //   parallelQuery.ForAll((image) => AutoQaCheckBlankImages.AutoQaCheckBlankImagesInstance.AutoQa(image)); //-ML
            foreach (var n in parallelQuery)
            {
                cToken.ThrowIfCancellationRequested();
                n.Save(true);

            }

        }

        private void AutoQA(IEnumerable<SQImage> images, bool delBlankImages)
        {
            CancellationTokenSource cTokenSource = new CancellationTokenSource();
            CancellationToken cToken = cTokenSource.Token;
            Progress<ProgressEventArgs> progress = new Progress<ProgressEventArgs>();
            ProgressMonitor.StartMonitoring(progress, cTokenSource);
            ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessRunningAutoQa);
            ProgressMonitor.StartMonitoring(progress, cTokenSource);
            try
            {
                if (delBlankImages)
                    RemoveBlankImages();
                //   AutoQa(ImageListViewer.CheckedImages.ToArray(), cToken);
                //  AutoQaCheckBlankImages.AutoQaCheckBlankImagesInstance.AutoQa(ImageListViewer.CheckedImages.ToArray(), cToken, progress);
                AutoQaCheckBlankImages.AutoQaCheckBlankImagesInstance.AutoQa(images, cToken, progress);
            }
            catch (Exception ex)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Error running AutoQa message {ex.Message}");
                //MessageBox.Show($"Error running AutoQa message {ex.Message}", "AutoQA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessAutoQaComplete);
            //if (AutoQaCheckBlankImages.AutoQaCheckBlankImagesInstance.ErrorsRunningQutoQA)
            //  ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessFailed);

        }

        private async Task AutoQAAsync()
        {
            try
            {
                bool delBlankImagaes = false;
                if (ImageListViewer.UnChecked.ToArray().Length > 0)
                    delBlankImagaes = true;
                //  EnableDisableButtons(false);
                await Task.Factory.StartNew(() => AutoQA(ImageListViewer.CheckedImages.ToArray(), delBlankImagaes));


            }
            catch { }
            finally
            {
                EnableDisableButtons(true);
            }
        }







        private void deleteBlankImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            RemoveBlankImages();
        }

        private void SetEncryptPw()
        {
            if ((!(string.IsNullOrWhiteSpace(DefaultSettings.SetEncPasswords))) && (DefaultSettings.SetEncPasswords.ToLower() == "setpw"))
            {
                DecriptPw pw = new DecriptPw();
                pw.ShowDialog();
                DefaultSettings.SetEncPasswords = "edocsuseinc";
                DefaultSettings.Save();
            }
        }
        private void LoginUser()
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Get user login information");
            this.Enabled = false;
            Public.ScanQuireUsers.ScanQuireUser.ScanQuireInstance.Login();
            this.Visible = true;
            EDL.TraceLogger.TraceLoggerInstance.UserName = Public.ScanQuireUsers.ScanQuireUser.ScanQuireInstance.ScanQuireUserLoggedIn;
            if (string.IsNullOrEmpty(Public.ScanQuireUsers.ScanQuireUser.ScanQuireInstance.ScanQuireUserLoggedIn))
                this.Close();
            if (Public.ScanQuireUsers.ScanQuireUser.ScanQuireInstance.IsAdmin)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"User logged in {Public.ScanQuireUsers.ScanQuireUser.ScanQuireInstance.ScanQuireUserLoggedIn} is an admin");
                tsAddUsers.Visible = true;

                this.Text = string.Format("{0} User Logged in {1} admin user", this.Text, Public.ScanQuireUsers.ScanQuireUser.ScanQuireInstance.ScanQuireUserLoggedIn);
            }
            else
            {
                tsAddUsers.Visible = false;
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"User logged in {Public.ScanQuireUsers.ScanQuireUser.ScanQuireInstance.ScanQuireUserLoggedIn} is not an admin");
                tsManageUsers.Text = "Change PassWord";
                this.Text = string.Format("{0} User Logged in {1} standard user", this.Text, Public.ScanQuireUsers.ScanQuireUser.ScanQuireInstance.ScanQuireUserLoggedIn);
            }
            this.Enabled = true;
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"User logged in {this.Text}");
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (!(DefaultSettings.ConvertViedoToImagesMenu))
                convertViedoImagesToolStripMenuItem.Visible = false;
            Scanquire.Public.ScanQuireUsers.ScanQuireUser.ScanQuireInstance.InActiveTimeout = 0;
            MemoryUsed("Memory usage start of scanquire:");
            SetTsOptions();
            if (DefaultSettings.DataBase)
            {
                Scanquire.Public.ScanQuireUsers.ScanQuireUser.ScanQuireInstance.InActiveTimeout = DefaultSettings.InActiveTimeOut;
                SetEncryptPw();
                LoginUser();

            }
            else
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Not using database");
                Public.ScanQuireUsers.ScanQuireUser.ScanQuireInstance.ScanQuireUserLoggedIn = Environment.UserName;
                string.Format("{0} User {1}", this.Text, Public.ScanQuireUsers.ScanQuireUser.ScanQuireInstance.ScanQuireUserLoggedIn);
                tsManageUsers.Visible = false;
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"User logged in {this.Text}");
            }

        }

        private void AddUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Add edit user information");
            Public.ScanQuireUsers.ScanQuireUser.ScanQuireInstance.AddUsers(0);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Done adding edit user information");
        }

        private void DeleteUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Edit user information");
            Public.ScanQuireUsers.ScanQuireUser.ScanQuireInstance.AddUsers(1);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Done editing user information");
        }

        private void ResetPassWordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Change password");
            Public.ScanQuireUsers.ScanQuireUser.ScanQuireInstance.RestPassword();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Done changing password");
        }

        private void MarkBlankImages()
        {

            CancellationTokenSource cTokenSource = new CancellationTokenSource();
            CancellationToken cToken = cTokenSource.Token;
            Progress<ProgressEventArgs> progress = new Progress<ProgressEventArgs>();
            ProgressMonitor.StartMonitoring(progress, cTokenSource);
            ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessRunningAutoQa);
            ProgressMonitor.StartMonitoring(progress, cTokenSource);
            if (ImageListViewer.ItemCount > 0)
            {

                SQImageListViewerItem[] item = ImageListViewer.Items.ToArray();


                for (int items = 0; items < item.Length; items++)
                {
                    if (item[items].Checked)
                    {
                        // var results = await Task.Factory.StartNew(() => AutoQaCheckBlankImages.AutoQaCheckBlankImagesInstance.IsImageBlankAynsc(item[items].Value));
                        var results = AutoQaCheckBlankImages.AutoQaCheckBlankImagesInstance.IsImageBlank(item[items].Value);

                        if (results)//   if()
                            item[items].Checked = false;
                    }

                }
                ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessAutoQaComplete);

            }
        }
        private async void MarkBlankImagesAsync()
        {
            DateTime st = DateTime.Now;
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Start checking for blank documents:{st.ToString()}");
            EnableDisableButtons(false);
            int totalImagesDone = 0;
            int totalToProcessed = ImageListViewer.ItemCount;
            CancellationTokenSource cTokenSource = new CancellationTokenSource();
            CancellationToken cToken = cTokenSource.Token;
            Progress<ProgressEventArgs> progress = new Progress<ProgressEventArgs>();
            ProgressMonitor.StartMonitoring(progress, cTokenSource);
            ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.StartMarkBlankDocuments);
            ProgressMonitor.StartMonitoring(progress, cTokenSource);
            IProgress<ProgressEventArgs> progresss = progress;
            try
            {
                if (totalToProcessed > 0)
                {
                    progresss.Report(new ProgressEventArgs(totalImagesDone, totalToProcessed, $"Document {totalImagesDone.ToString()} of {totalToProcessed.ToString()}"));

                    SQImageListViewerItem[] item = ImageListViewer.Items.ToArray();


                    for (int items = 0; items < item.Length; items++)
                    {
                        progresss.Report(new ProgressEventArgs(++totalImagesDone, totalToProcessed, $"Document {totalImagesDone.ToString()} of {totalToProcessed.ToString()}"));
                        if (item[items].Checked)
                        {
                            var results = await Task.Factory.StartNew(() => AutoQaCheckBlankImages.AutoQaCheckBlankImagesInstance.IsImageBlankAynsc(item[items].Value));
                            if (results.Result)//   if()
                                item[items].Checked = false;
                        }

                    }
                    ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.EndMarkBlankDocuments);
                }
            }
            catch (Exception ex)
            {
                ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessFailed);
                MessageBox.Show($"Marking BlankDocuments {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            EnableDisableButtons(true);
            DateTime et = DateTime.Now;
            TimeSpan ts = et - st;
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Done checking for blank documents:{et.ToString()}  total time:{string.Format("{0:00}", ts.Hours)}:{string.Format("{0:00}", ts.Minutes)}:{string.Format("{0:00}", ts.Seconds)}:{string.Format("{0:000}", ts.Milliseconds)}");
        }


        private void CurrentArchiverSelector_MouseHover1(object sender, System.EventArgs e)
        {
            CheckInactiveTimeOut();
        }

        private void ImageListViewer_MouseHover(object sender, System.EventArgs e)
        {
            CheckInactiveTimeOut();
        }
        private void MainMenuToolStrip_MouseHover(object sender, System.EventArgs e)
        {
            CheckInactiveTimeOut();
        }

        private void ResetTimeIdle()
        {
            Public.ScanQuireUsers.ScanQuireUser.ScanQuireInstance.TimeIdle = DateTime.Now;
        }

        private void NewDocumentMenuButton_MouseHover(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
        }
        private void CheckInactiveTimeOut()
        {
            if (Public.ScanQuireUsers.ScanQuireUser.ScanQuireInstance.CheckInaviteTimeOut())
                LoginUser();
        }

        private void TsCombBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            ScanDocumentSettings = tsCombBox.SelectedIndex;
        }

        private void tsCheckBkankImages_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();

            //   MarkBlankImages();
            MarkBlankImagesAsync();
        }

        private void deleteBlankImagesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            RemoveBlankImages();
        }

        private async void toolStripAutoQcChecked_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Uncheck checked images");
            if (ImageListViewer.UnCheckedImages.Count() == 0)
            {
                MessageBox.Show("No Images Checked", "Checked", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                await Task.Factory.StartNew(() => AutoQA(ImageListViewer.UnCheckedImages.ToArray(), false));
            // await AutoQAAsync();

        }

        private async void autoQAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();


            EnableDisableButtons(false);
            await AutoQAAsync();

        }

        private void tsRunAutoQA_Click(object sender, EventArgs e)
        {
            if (tsRunAutoQA.Checked)
            {
                tsRunAutoQA.ForeColor = System.Drawing.SystemColors.ActiveCaption;


            }
            else
            {

                tsRunAutoQA.ForeColor = System.Drawing.SystemColors.ControlText;

            }
        }

        private void SetTsOptions()
        {

            tsCombBox.SelectedIndex = 2;
            bool showTScannerSettings = false;

            Edocs_Utilities.EdocsUtilitiesInstance.BakupOrgImages = DefaultSettings.BackUpOrgImages;
            if (string.IsNullOrWhiteSpace(DefaultSettings.LabReqsWebSite))
                tsBtnWebSite.Visible = false;
            if (DefaultSettings.ShowTSScannerSettings)
            {
                if (!(DefaultSettings.ShowTSAutoQA))
                {

                    tsMenuAutoAQ.Visible = false;
                }
                else
                {
                    tsRunAutoQA.Checked = DefaultSettings.UserAutoQASetting;
                    showTScannerSettings = true;
                }
                SetScannerTypeChecked(DefaultSettings.UseTwain);

                if (!(DefaultSettings.ShowTSImageOptions))
                {
                    ScanDocumentSettings = 2;
                    tsImageOptions.Visible = false;
                }
                else
                    showTScannerSettings = true;
                if (!(DefaultSettings.ShosTSScanOptios))
                {

                    tsScanImageSettings.Visible = false;
                }
                else
                {
                    tsCombBox.SelectedIndex = DefaultSettings.UserScanSetting;
                    showTScannerSettings = true;
                }
            }

            if (!(showTScannerSettings))
                scannerOptions.Visible = false;
        }

        private void TsBtnCloseScanQuire_Click(object sender, EventArgs e)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Closing scanquire");
            this.Close();
        }

        private void TsDelCheckImages_Click(object sender, EventArgs e)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Deleting Blank Images");

            CheckInactiveTimeOut();
            RemoveBlankImages();


        }
        private void CreateNewLogFile()
        {
            TimeSpan ts = DateTime.Now - EDL.TraceLogger.TraceLoggerInstance.TraceLogFileCreated;
            if (ts.Days >= 1)
            {
                string ra = EDL.TraceLogger.TraceLoggerInstance.RunningAssembley;
                string TraceFilePath = string.Empty;
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Opening new log file");
                EDL.TraceLogger.TraceLoggerInstance.CloseTraceFile();
                if (Scanquire.Properties.Scanquire.Default.AuditLogs)
                {

                    // TraceFilePath = Path.Combine(SettingsManager.AuditLogsDirectroy, string.Format("ScanQuire_{0}.log", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
                    TraceFilePath = EDL.TraceLogger.TraceLoggerInstance.GetTraceFileName(SettingsManager.AuditLogsDirectroy, "ScanQuire", Scanquire.Properties.Scanquire.Default.DaysToKeepLogFiles, "*.*", SettingsManager.AuditLogsUploadDirectroy);
                    Edocs_Utilities.EdocsUtilitiesInstance.CopyFiles(SettingsManager.AuditLogsDirectroy, SettingsManager.AuditLogsUploadDirectroy, true, "*.*");
                }
                else
                {
                    TraceFilePath = EDL.TraceLogger.TraceLoggerInstance.GetTraceFileName(SettingsManager.TempDirectoryPath, "ScanQuire", Scanquire.Properties.Scanquire.Default.DaysToKeepLogFiles, "*.*", SettingsManager.TempDirectoryPath);
                }
                EDL.TraceLogger.TraceLoggerInstance.RunningAssembley = ra;

                EDL.TraceLogger.TraceLoggerInstance.OpenTraceLogFile(TraceFilePath, "SCANQUIRELOGFILE");
                EDL.TraceLogger.TraceLoggerInstance.WriteTraceHeader();
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Created new log file:{TraceFilePath}");
            }

        }



        private void tsImageRecovery_Click(object sender, EventArgs e)
        {
            SetRecoverImages();
        }
        private void SetRecoverImages()
        {
            if (!(tsImageRecovery.Checked))
            {
                tsImageRecovery.Checked = true;
                tsImageRecovery.CheckState = CheckState.Checked;
                Edocs_Utilities.EdocsUtilitiesInstance.DecriptImage = false;
            }
            else
            {
                tsImageRecovery.Checked = false;
                tsImageRecovery.CheckState = CheckState.Unchecked;
            }
        }
        private async void OpenWebPage()
        {
            try
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Opening web site {DefaultSettings.LabReqsWebSite} using web browser {DefaultSettings.WebBrowser}");
                EdocsUSA.Utilities.Edocs_Utilities.EdocsUtilitiesInstance.StartProcess(DefaultSettings.WebBrowser, DefaultSettings.LabReqsWebSite, false);
            }
            catch (Exception ex)
            {

                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Opening web site {DefaultSettings.LabReqsWebSite} using web browser {DefaultSettings.WebBrowser} {ex.Message}");
                MessageBox.Show($"Could not Open web site {DefaultSettings.LabReqsWebSite} {ex.Message}", "Web Site", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        private void tsBtnWebSite_Click(object sender, EventArgs e)
        {
            OpenWebPage();
        }

        private void tsBtnWebSite_MouseHover(object sender, EventArgs e)
        {
            tsBtnWebSite.Text = "Open LabReqs Web Site";
        }
        private void tsBtnWebSite_MouseLeave(object sender, EventArgs e)
        {
            tsBtnWebSite.Text = "&Web Site";
        }


        //private async Task EditImages(SQImageListViewerItem item)
        //{




        //    FormCropResizeImage formCropResizeImage = new FormCropResizeImage();
        //    FreeImageBitmap freeImage = null;
        //    item.Value.BeginEdit();


        //    formCropResizeImage.SaveBitMap = new Bitmap(item.Value.WorkingCopy.ToBitmap());
        //    formCropResizeImage.ShowDialog();
        //    if (string.Compare(formCropResizeImage.SaveFormat, "NoSave") != 0)
        //    {



        //        CancellationTokenSource cTokenSource = new CancellationTokenSource();
        //        CancellationToken cToken = cTokenSource.Token;
        //        Progress<ProgressEventArgs> progress = new Progress<ProgressEventArgs>();
        //        try
        //        {


        //            ProgressMonitor.StopMonitoring(progress, "UpDating Image Changes", Color.Yellow);
        //            ProgressMonitor.StartMonitoring(progress, cTokenSource);
        //            freeImage = new FreeImageBitmap(formCropResizeImage.SaveBitMap, formCropResizeImage.SaveBitMap.Width, formCropResizeImage.SaveBitMap.Height);
        //            if (string.Compare(formCropResizeImage.SaveFormat, "CopySave", true) == 0)
        //            {
        //                SQImage sQImage = new SQImage(freeImage);
        //                sQImage.BeginEdit();
        //                ImageListViewer.Insert(item.Index + 1, sQImage);



        //            }
        //            else
        //            {
        //                item.Value.WorkingCopy = freeImage;
        //                item.Value.Save(true);
        //            }
        //            ProgressMonitor.StopMonitoring(progress, "Done UpDating Image Changes", Color.LightGreen);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Error UpDating Image {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessFailed);
        //        }
        //        if (formCropResizeImage != null)
        //            formCropResizeImage.Dispose();
        //    }


        //}

        //private void tsCropImageAdd_Click(object sender, EventArgs e)
        //{

        //    try
        //    {


        //        SQImageListViewerItem item = ImageListViewer.ActiveItem;
        //        EditImages(item).ConfigureAwait(true).GetAwaiter().GetResult();

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error Editing Image {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    //else
        //    //    MessageBox.Show("Select Image to Edit", "No Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //}

        private void tsCCRImage_Click(object sender, EventArgs e)
        {

            try
            {

                SQImageListViewerItem item = ImageListViewer.ActiveItem;
                OCRImage(item).ConfigureAwait(true).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Editing Image {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task OCRImage(SQImageListViewerItem item)
        {
            FormOcrPdf formOcr = new FormOcrPdf();

            item.Value.BeginEdit();
            CancellationTokenSource cTokenSource = new CancellationTokenSource();
            CancellationToken cToken = cTokenSource.Token;
            Progress<ProgressEventArgs> progress = new Progress<ProgressEventArgs>();
            formOcr.FreeImage = new FreeImageBitmap(item.Value.WorkingCopy);
            formOcr.ScanedBitMap = new Bitmap(item.Value.WorkingCopy.ToBitmap());
            formOcr.ShowDialog();
            if (formOcr.SaveCopy.HasValue)
            {
                if (formOcr.SaveCopy.Value)
                {
                    ProgressMonitor.StopMonitoring(progress, "UpDating Image Changes", Color.Yellow);
                    ProgressMonitor.StartMonitoring(progress, cTokenSource);
                    //FreeImageBitmap freeImage = null;
                    SQImage sQImage = new SQImage(new FreeImageBitmap(formOcr.FreeImage));
                    sQImage.BeginEdit();
                    ImageListViewer.Insert(item.Index + 1, sQImage);
                }
                else
                {


                    if (formOcr.FreeImage != null)
                    {
                        ProgressMonitor.StopMonitoring(progress, "UpDating Image Changes", Color.Yellow);
                        ProgressMonitor.StartMonitoring(progress, cTokenSource);
                        item.Value.WorkingCopy = new FreeImageBitmap(formOcr.FreeImage);
                        item.Value.Save(true);
                    }

                }
                if (formOcr.FreeImage != null)
                    formOcr.FreeImage.Dispose();
            }
            formOcr.Dispose();
            ProgressMonitor.StopMonitoring(progress, "Done UpDating Image Changes", Color.LightGreen);
        }



        private async Task PrintImages()
        {
            try
            {
                string imageFolder = Path.Combine(SettingsManager.TempDirectoryPath, "PrintImageFolder");
                //   Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(imageFolder);
                Edocs_Utilities.EdocsUtilitiesInstance.DelFolder(imageFolder, true);
                Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(imageFolder);

                SQImageListViewerItem[] item = ImageListViewer.Items.ToArray();
                int imagesCount = 0;
                for (int i = 0; i < item.Count(); i++)
                {
                    if (item[i].Selected)
                    {

                        SQImage image = item[i].Value;
                        SQImageEditLock image_Lock = image.BeginEdit();
                        Image bm = (Bitmap)image.WorkingCopy;
                        //    Edocs_Utilities.EdocsUtilitiesInstance.PrintBitMap = (Bitmap)image.WorkingCopy;
                        image.DiscardEdit(image_Lock);
                        Edocs_Utilities.EdocsUtilitiesInstance.PrintImageFileName = Path.Combine(imageFolder, $"PrintImage_{imagesCount.ToString()}.png");
                        bm.Save(Edocs_Utilities.EdocsUtilitiesInstance.PrintImageFileName, System.Drawing.Imaging.ImageFormat.Png);
                        bm.Dispose();

                        if (imagesCount == 0)
                        {
                            if (PrintDialog.ShowDialog() == DialogResult.OK)
                            {

                                PrintDocument.DocumentName = "Scanquire Images";
                                PrintDocument.Print();
                            }
                            else
                                return;
                        }
                        else
                            PrintDocument.Print();
                        imagesCount++;

                    }
                }
                if (imagesCount == 0)

                    MessageBox.Show("No Images Selected to Print", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Printing Images {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            e.Graphics.DrawImage(Image.FromFile(Edocs_Utilities.EdocsUtilitiesInstance.PrintImageFileName), e.Graphics.VisibleClipBounds);


        }

        private async void EmailPDF_Click(object sender, EventArgs e)
        {
            EmailImagesForm emailImagesForm = new EmailImagesForm();
            emailImagesForm.ImageType = EmailImagesForm.EmaiImageTypes.PDF;
            emailImagesForm.EmailImages = ImageListViewer.Items.ToArray();
            emailImagesForm.ShowDialog();
        }

        private async void EmailBmp_Click(object sender, EventArgs e)
        {
            EmailImagesForm emailImagesForm = new EmailImagesForm();
            emailImagesForm.ImageType = EmailImagesForm.EmaiImageTypes.BMP;
            emailImagesForm.EmailImages = ImageListViewer.Items.ToArray();
            emailImagesForm.ShowDialog();
        }

        private async void EmailTifImage_Click(object sender, EventArgs e)
        {
            EmailImagesForm emailImagesForm = new EmailImagesForm();
            emailImagesForm.ImageType = EmailImagesForm.EmaiImageTypes.TIF;
            emailImagesForm.EmailImages = ImageListViewer.Items.ToArray();
            emailImagesForm.ShowDialog();
        }

        private async void EmailJpgImage_Click(object sender, EventArgs e)
        {
            EmailImagesForm emailImagesForm = new EmailImagesForm();
            emailImagesForm.ImageType = EmailImagesForm.EmaiImageTypes.JPG;
            emailImagesForm.EmailImages = ImageListViewer.Items.ToArray();
            emailImagesForm.ShowDialog();
        }

        private async void EmailPngImages_Click(object sender, EventArgs e)
        {
            EmailImagesForm emailImagesForm = new EmailImagesForm();
            emailImagesForm.ImageType = EmailImagesForm.EmaiImageTypes.JPG;
            emailImagesForm.EmailImages = ImageListViewer.Items.ToArray();
            emailImagesForm.ShowDialog();
        }

        private async void EmailGifImages_Click(object sender, EventArgs e)
        {
            EmailImagesForm emailImagesForm = new EmailImagesForm();
            emailImagesForm.ImageType = EmailImagesForm.EmaiImageTypes.GIF;
            emailImagesForm.EmailImages = ImageListViewer.Items.ToArray();
            emailImagesForm.ShowDialog();
        }

        private async void selectedImagesToArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Start saving Selected Images to archive: {CurrentArchiverSelector.SelectedArchiverName}");
            DialogResult result = MessageBox.Show($"Do You want to save Selected Images  to archiver {CurrentArchiverSelector.SelectedArchiverName}?", "Save Archiver", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning($"User Cancel save Selected Images to archiver {CurrentArchiverSelector.SelectedArchiverName}");
                return;
            }
            EnableDisableButtons(false);
            MemoryUsed("Memory usage before archive:");
            await SaveSelected(SelectedArchiver);
            RunGcc();
            MemoryUsed("Memory usage after archive:");
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Done saving Selected Images to archiver: {CurrentArchiverSelector.SelectedArchiverName}");
            ResetTimeIdle();
            NewDocumentMenuButton.Enabled = true;
        }

        private async void selectedImagesToLocalFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckInactiveTimeOut();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Start saving Selected Images to archive: {CurrentArchiverSelector.SelectedArchiverName}");
            DialogResult result = MessageBox.Show($"Do You want to Selected Images to archiver {CurrentArchiverSelector.SelectedArchiverName}?", "Save Archiver", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning($"User Cancel save Selected Images to archiver {CurrentArchiverSelector.SelectedArchiverName}");
                return;
            }
            EnableDisableButtons(false);
            MemoryUsed("Memory usage before archive:");
            await SaveSelected(SelectedArchiver);
            RunGcc();
            MemoryUsed("Memory usage after archive:");
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Done saving Selected Images to archiver: {CurrentArchiverSelector.SelectedArchiverName}");
            ResetTimeIdle();
            NewDocumentMenuButton.Enabled = true;
        }

        private async Task ConvertViedoToImages()
        {
            Progress<ProgressEventArgs> progress = new Progress<ProgressEventArgs>();


            try
            {
                CancellationTokenSource cTokenSource = new CancellationTokenSource();
                CancellationToken cToken = cTokenSource.Token;

                ProgressMonitor.StartMonitoring(progress, cTokenSource);
                ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessRunningConvertToViedoImages);
                ProgressMonitor.StartMonitoring(progress, cTokenSource);
                string convertApp = Path.Combine(SettingsManager.ApplicationDirectory, DefaultSettings.ConvertViedosImagesExe);
                Edocs_Utilities.EdocsUtilitiesInstance.StartProcess(convertApp, "-iv viedo", true);
                ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessCompleted);
                Scanquire.Public.SQFilesystemConnector.ConvertedImages = true;
                SQFilesystemConnector.ConvertedImagesFolder = DefaultSettings.ImagesConvertedFolder;
                if (ImageListViewer.ItemCount > 0)
                    await Acquire(LocalArchiver, SQAcquireIntent.Append, SQAcquireSource.File);
                else
                    await Acquire(LocalArchiver, SQAcquireIntent.New, SQAcquireSource.File);
            }
            catch (Exception ex)
            {
                ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessFailed);
                MessageBox.Show($"Error Converting Images to Viedo {ex.Message}");
            }
            //Scanquire.Public.AcquireFromFile


        }


        private async void convertViedoToImagesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            await ConvertViedoToImages();
        }

        private async void convertImagesToViedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ImageListViewer.ItemCount > 0)
            {
                Progress<ProgressEventArgs> progress = new Progress<ProgressEventArgs>();
                try
                {
                    CancellationTokenSource cTokenSource = new CancellationTokenSource();
                    CancellationToken cToken = cTokenSource.Token;

                    ProgressMonitor.StartMonitoring(progress, cTokenSource);
                    ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessRunningConvertImagesToViedo);
                    ProgressMonitor.StartMonitoring(progress, cTokenSource);

                    await Task.Factory.StartNew(() => Scanquire.Public.CreateVIedoImages.ConvertImagesToViedoInstance.CreateViedo(ImageListViewer.CheckedImages.ToArray(), cToken, DefaultSettings.ImagesToConvertedWF, DefaultSettings.ConvertViedosImagesExe));
                    ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessCompleted);
                }
                catch (Exception ex)
                {
                    ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessFailed);
                    MessageBox.Show($"Error Converting Images to Viedo {ex.Message}");
                }
            }


        }

        private async void SettingPrintImage_Click(object sender, EventArgs e)
        {

            await PrintImages();
        }

        private async void printMultiImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ImageListViewer.ItemCount > 0)
            {
                string wf = Path.Combine(Path.GetDirectoryName(SettingsManager.TempDirectoryPath), "PrintWizard");
                await Task.Factory.StartNew(() => PrintImagesWizard.PrintImagesWizardInstance.PrintMulitImages(ImageListViewer.CheckedImages.ToArray(), wf));
            }
        }

        private async void printSelectedImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ImageListViewer.UnCheckedImages.Count() == 0)
            {
                MessageBox.Show("No Images Checked to Print", "Checked", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string wf = Path.Combine(Path.GetDirectoryName(SettingsManager.TempDirectoryPath), "PrintWizard");
                await Task.Factory.StartNew(() => PrintImagesWizard.PrintImagesWizardInstance.PrintMulitImages(ImageListViewer.UnCheckedImages.ToArray(), wf));
            }

        }

        private void SetScannerTypeChecked(bool twain)
        {
            if (twain)
            {
                tsMenuTwainScanner.Checked = true;
                tsMenuTwainScanner.CheckState = CheckState.Checked;
                tsMenuTwainScanner.ForeColor = System.Drawing.SystemColors.HotTrack;
                tsMenuWiaScanner.Checked = false;
                tsMenuWiaScanner.ForeColor = System.Drawing.SystemColors.ControlText;
                tsMenuWiaScanner.CheckState = CheckState.Unchecked;
            }
            else
            {
                tsMenuTwainScanner.Checked = false;
                tsMenuTwainScanner.CheckState = CheckState.Unchecked;
                tsMenuTwainScanner.ForeColor = System.Drawing.SystemColors.ControlText;
                tsMenuWiaScanner.ForeColor = System.Drawing.SystemColors.HotTrack;
                tsMenuWiaScanner.Checked = true;
                tsMenuWiaScanner.CheckState = CheckState.Checked;
            }
        }
        private void tsMenuTwainScanner_Click(object sender, EventArgs e)
        {

            SetScannerTypeChecked(true);
        }

        private void tsMenuWiaScanner_Click(object sender, EventArgs e)
        {
            // tsMenuTwainScanner.BackColor = System.Drawing.SystemColors.Control;
            SetScannerTypeChecked(false);
        }
        private async Task ImageTools(SQImageListViewerItem item)
        {
            CancellationTokenSource cTokenSource = new CancellationTokenSource();
            CancellationToken cToken = cTokenSource.Token;
            Progress<ProgressEventArgs> progress = new Progress<ProgressEventArgs>();
            using (var formTools = new Scanquire.Public.EdocsUSAImageTools.ToolImage())
            {
                try
                { 
                FreeImageBitmap freeImage = null;
                item.Value.BeginEdit();
                    //  formTools.imageHandler.CurrentBitmap = item.Value.LatestRevision.GetOriginalImageBitmap();
                   // Bitmap bitmap = new Bitmap(item.Value.WorkingCopy.ToBitmap());
                    formTools.imageHandler.CurrentBitmap = new Bitmap(item.Value.WorkingCopy.ToBitmap());
                    formTools.ShowDialog();
                if (string.IsNullOrWhiteSpace(formTools.SaveFormat))
                {
                    ProgressMonitor.StopMonitoring(progress, "Image Changes not saved", Color.Red);


                }
                else
                {
                    ProgressMonitor.StopMonitoring(progress, "UpDating Image Changes", Color.Yellow);
                    ProgressMonitor.StartMonitoring(progress, cTokenSource);
                    freeImage = new FreeImageBitmap(formTools.imageHandler.CurrentBitmap, formTools.imageHandler.CurrentBitmap.Width, formTools.imageHandler.CurrentBitmap.Height);
                    if (string.Compare(formTools.SaveFormat, "CopySave", true) == 0)
                    {
                        SQImage sQImage = new SQImage(freeImage);
                        sQImage.BeginEdit();
                        ImageListViewer.Insert(item.Index + 1, sQImage);



                    }
                    else
                    {
                        item.Value.WorkingCopy = freeImage;
                        item.Value.Save(true);
                    }
                    ProgressMonitor.StopMonitoring(progress, "Done UpDating Image Changes", Color.LightGreen);
                }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error UpDating Image {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ProgressMonitor.StopMonitoring(progress, Scanquire.Public.UserControls.ProgressMonitor.StopMonitoringReason.ProcessFailed);
                }
                GC.Collect();
            }
        }
        private void imageToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQImageListViewerItem item = ImageListViewer.ActiveItem;
            ImageTools(item).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
