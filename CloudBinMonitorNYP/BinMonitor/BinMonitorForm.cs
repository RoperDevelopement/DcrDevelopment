using BinMonitor.Common;
using UpDateEdocsSoftware;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Reflection;
using System.Threading.Tasks;
namespace BinMonitor
{
    public partial class BinMonitorForm : Form
    {
        Timer LastUpdatedTimer = new Timer() { Interval = 1000 };
        Timer RefreshBinTimer = new Timer() { Interval = 60000 };
        bool normalShutDown = true;
        int CheckUploaderFrequency = Settings.Application.Default.CheckUploadFrequency;
        int CheckUploaderProblemThreshold = Settings.Application.Default.CheckUploadProblemThreshold;
        protected string strLastUpdated = DateTime.Now.ToString();
        public BinMonitorForm()
        {



            InitializeComponent();
            lblLastUpdated.Text = string.Format("BinMonitor:{0}", DateTime.Now.ToString());
        }

        protected string BatchUploaderExePath = Settings.Application.Default.BatchUploaderPath;




        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.DesignMode == false)
            {
                InitializePnlOverviewIcons(false);
                LastUpdatedTimer.Tick += LastUpdatedTimer_Tick;
                RefreshBinTimer.Tick += RefreshBinTimer_Tick;
                RefreshBinTimer.Interval = Settings.Application.Default.CheckNewBinsFrequency;

                LastUpdatedTimer.Stop();


                //  UpDateEdocsSoftWareCloud.UpdateCloudInstance.CheckForUpdates();
            }

        }


        private void RefreshBinTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                UpdateBm();
                

            }
            catch { }
            RefreshBinTimer.Start();

        }

        void LastUpdatedTimer_Tick(object sender, EventArgs e)
        {
            bool error = false;
            string message = string.Empty;

            try
            {
                /*
                string uploaderExeName = Path.GetFileNameWithoutExtension(this.BatchUploaderExePath);
                string uploaderDirPath = Path.GetDirectoryName(this.BatchUploaderExePath);
                string lastUpdatedFilePath = Path.Combine(uploaderDirPath, "Data", "LastUpdated.txt");

                if (Directory.Exists(uploaderDirPath) == false)
                { 
                    message = string.Format("Uploader executable directory not found ({0})", uploaderDirPath);
                    error = true;
                }
                else if (File.Exists(BatchUploaderExePath) == false)
                {
                    message = string.Format("Uploader executable not found ({0})", this.BatchUploaderExePath);
                    error = true;
                }
                else if (Process.GetProcessesByName(uploaderExeName).Length <= 0)
                {
                    message = string.Format("Uploader process not running ({0})", uploaderExeName);
                    error = true;
                }                
                else if (File.Exists(lastUpdatedFilePath) == false)
                { 
                    message = string.Format("Uploader has not reported any updates ({0})", lastUpdatedFilePath);
                    error = true;
                }
                else
                {
                    FileInfo fi = new FileInfo(lastUpdatedFilePath);
                    DateTime updateTime = fi.LastWriteTime;
                    if (((TimeSpan)(DateTime.Now - updateTime)).TotalMilliseconds > this.CheckUploaderProblemThreshold)
                    {
                        message = string.Format("Uploader has not updated since {0}", updateTime.ToString());
                        error = true;
                    }
                    else
                    {
                        message = string.Format("Last updated at {0}", updateTime.ToString());
                        error = false;
                    }


                }   */

            }
            catch (Exception ex)
            {
                message = ex.Message;
                error = true;
            }

            this.StatusStrip.BackColor = (error == true) ? Color.Red : SystemColors.Control;
            if (string.IsNullOrWhiteSpace(strLastUpdated))
                lblLastUpdated.Text = message;
            strLastUpdated = string.Format("BinMonitor:{0}", DateTime.Now.ToString());

        }

        protected override void OnClosing(CancelEventArgs e)
        {

            if (normalShutDown)
            {
                if (MessageBox.Show(this, "Are you sure you want to exit?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
                { e.Cancel = true; }
            }
            try
            {
                this.Hide();
                Trace.Flush();
                Trace.Close();
                BinMonitorSqlServer.SqlServerInstance.UpLoadLogFile();
                BinUtilities.BinMointorUtilties.CleanUpFolders();
            }
            catch
            { }
            base.OnClosing(e);
        }


        protected void InitializePnlOverviewIcons(bool clearControls)
        {
            //Add the bins

            BinsOverviewViewer.SuspendLayout();
            if (clearControls)
            {
                Bins.Instance.DirectoryPath = BinUtilities.BinMonBinsFolder;
                Bins.Instance.Reload();
                BinsOverviewViewer.Controls.Clear();
            }
            Bin[] bins = (Bins.Instance.Values.OrderBy(bin => bin.Id)).ToArray();
            foreach (Bin bin in bins)
            {
                BinStatusViewer binViewer = new BinStatusViewer();
                binViewer.BorderStyle = BorderStyle.FixedSingle;
                binViewer.Bin = bin;
                BinsOverviewViewer.Controls.Add(binViewer);
            }
            BinsOverviewViewer.ResumeLayout();
        }

        BinManagerForm ManagerForm = new BinManagerForm(true);
        private void btnLaunchManager_Click(object sender, EventArgs e)
        {
            //this.Hide();
            //UserPickerDialog dlg = new UserPickerDialog();
            //dlg.StartPosition = FormStartPosition.CenterParent;
            //dlg.Show();
            btnLaunchManager.Enabled = false;
            StatusStrip.Text = "Getting information from the cloud:";
            StatusStrip.Update();

            //  BinMonitorSqlServer.SqlServerInstance.UserBinsBatches();
            //    BinMonitorSqlServer.SqlServerInstance.GetUnactiveBatches();
            if (ManagerForm.IsDisposed)
            { ManagerForm = new BinManagerForm(false); }
            //    ManagerForm.MaximizeBox = false;
            ///  ManagerForm.WindowState = FormWindowState.Maximized;
            // ManagerForm.ControlBox = false;
            StatusStrip.Text = "LastUpdate:";
            StatusStrip.Update();
            btnLaunchManager.Enabled = true;
            ManagerForm.Show();

        }

        private async Task RunUpdates(bool showNoUpdatesFound)
        {
            lblLastUpdated.Text = "Checking for SoftWare Updates";
            this.Refresh();
            string versionNumber = BinUtilities.BinMointorUtilties.GetAssemblyVersion();
            RunUpDates runUp = new RunUpDates(Settings.Application.Default.CheckForUpDates, versionNumber);
            bool upDatesFound = await Task.Factory.StartNew(() => runUp.GetUpdates());
            if (upDatesFound)
            {
                DialogResult result = MessageBox.Show("Install UpDates", "Updates Found", MessageBoxButtons.YesNo);
                tsbCheckForUpdates.Text = "Install Updates";
                if (result == DialogResult.Yes)
                {

                    if (runUp.RunUpdate())
                    {
                        normalShutDown = false;
                        this.Close();
                    }
                }
            }
            else
            {
                if (showNoUpdatesFound)
                    MessageBox.Show("No Updates found", "No Updates", MessageBoxButtons.OK);
            }
            
            lblLastUpdated.Text = string.Format("BinMonitor:{0}", DateTime.Now.ToString());
            this.Refresh();
        }

        private void btnLaunchUpdater_Click(object sender, EventArgs e)
        {
            try
            {
                string exeFullPath = Path.GetFullPath(this.BatchUploaderExePath);
                string exeDirPath = Path.GetDirectoryName(exeFullPath);
                if (Directory.Exists(exeDirPath) == false)
                { throw new DirectoryNotFoundException(string.Format("Unable to find uploader directory ({0})", exeDirPath)); }
                if (File.Exists(this.BatchUploaderExePath) == false)
                { throw new FileNotFoundException(string.Format("Unable to find uploader executable ({0})", this.BatchUploaderExePath)); }
                ProcessStartInfo pi = new ProcessStartInfo();
                //pi.UseShellExecute = true;
                pi.WorkingDirectory = exeDirPath;
                pi.FileName = exeFullPath;
                //MessageBox.Show(string.Format("Launching ({0}) from ({1})", pi.FileName, pi.WorkingDirectory));
                Process.Start(pi);
            }
            catch (Exception ex)
            { MessageBox.Show("Error launching the uploader." + Environment.NewLine + ex.Message); }
        }



        private void BinMonitorForm_Shown(object sender, EventArgs e)
        {
            string versionNumber = BinUtilities.BinMointorUtilties.GetAssemblyVersion();
           this.Text = string.Format("{0} version {1}", BinUtilities.BinMointorUtilties.GetAssemblyDescription(), versionNumber);
         //    RunUpdates(false);
            RefreshBinTimer.Start();
        }

        private void tsbCheckForUpdates_Click(object sender, EventArgs e)
        {
            return;
            if (tsbCheckForUpdates.Text.Trim() == "Install Updates")
            {
                string versionNumber = BinUtilities.BinMointorUtilties.GetAssemblyVersion();
                RunUpDates runUp = new RunUpDates(Settings.Application.Default.CheckForUpDates, versionNumber);
                if (runUp.RunUpdate())
                {
                    normalShutDown = false;
                    this.Close();
                }
            }
            else
            {
                BinUtilities.BinMointorUtilties.DeleteFile(BinUtilities.UpDateFileName);
                RunUpdates(true);
            }

        }
        private void UpdateBm()
        {
            RefreshBinTimer.Stop();
            strLastUpdated = DateTime.Now.ToString();
            Trace.TraceInformation("Checnking for new user batches");
            BinMonitorSqlServer.SqlServerInstance.UserBinsBatches();
            BinMonitorSqlServer.SqlServerInstance.GetUnactiveBatches();
            InitializePnlOverviewIcons(true);
            lblLastUpdated.Text = string.Format("BinMonitor:{0}", DateTime.Now.ToString());
        }
        private void tlsButtonUpdateBm_Click(object sender, EventArgs e)
        {

            try
            {
                UpdateBm();
            }
            catch { }
            RefreshBinTimer.Start();
            

        }

        //private async Task BinMonitorForm_ShownAsync(object sender, EventArgs e)
        //{

        //    string versionNumber = BinUtilities.BinMointorUtilties.GetAssemblyVersion();
        //    this.Text = string.Format("{0} version {1}", BinUtilities.BinMointorUtilties.GetAssemblyDescription(), versionNumber);
        //    await RunUpdates();
        //}
    }


}
