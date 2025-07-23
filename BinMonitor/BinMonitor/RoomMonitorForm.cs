using BinMonitor.Common;
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

namespace BinMonitor
{
    public partial class RoomMonitorForm : Form
    {
        Timer LastUpdatedTimer = new Timer() { Interval = 1000 };

        int CheckUploaderFrequency = Settings.Application.Default.CheckUploadFrequency;
        int CheckUploaderProblemThreshold = Settings.Application.Default.CheckUploadProblemThreshold;

        public RoomMonitorForm()
        { InitializeComponent(); }

        protected string BatchUploaderExePath = Settings.Application.Default.BatchUploaderPath;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.DesignMode == false)
            {
                InitializePnlOverviewIcons();
                LastUpdatedTimer.Tick += LastUpdatedTimer_Tick;
                LastUpdatedTimer.Start();
            }
        }

        void LastUpdatedTimer_Tick(object sender, EventArgs e)
        {
            bool error = false;
            string message = string.Empty;

            try
            {
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
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                error = true;
            }

            this.StatusStrip.BackColor = (error == true) ? Color.Red : SystemColors.Control;
            lblLastUpdated.Text = message;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure you want to exit?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
            { e.Cancel = true; }
            base.OnClosing(e);
        }

        protected void InitializePnlOverviewIcons()
        {
            //Add the bins
            BinsOverviewViewer.SuspendLayout();
            Bin[] bins = (Room.Instance.Values.OrderBy(bin => bin.Id)).ToArray();
            foreach (Bin bin in bins)
            {
                RoomStatusViewer roomViewer = new RoomStatusViewer();
                roomViewer.BorderStyle = BorderStyle.FixedSingle;
                roomViewer.Bin = bin;
                BinsOverviewViewer.Controls.Add(roomViewer);
            }
            BinsOverviewViewer.ResumeLayout();
        }

        RoomManagerForm ManagerForm = new RoomManagerForm();
        private void btnLaunchManager_Click(object sender, EventArgs e)
        {
            if (ManagerForm.IsDisposed)
            {
                ManagerForm = new RoomManagerForm();
                ManagerForm.Show();
            }
            ManagerForm.Show();
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






    }


}
