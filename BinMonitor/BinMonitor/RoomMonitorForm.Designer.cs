namespace BinMonitor
{
    partial class RoomMonitorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoomMonitorForm));
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.lblLastUpdatedCaption = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblLastUpdated = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnLaunchManager = new System.Windows.Forms.ToolStripButton();
            this.btnLaunchUpdater = new System.Windows.Forms.ToolStripButton();
            this.BinsOverviewViewer = new BinMonitor.GridPanel();
            this.StatusStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusStrip
            // 
            this.StatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblLastUpdatedCaption,
            this.lblLastUpdated});
            this.StatusStrip.Location = new System.Drawing.Point(0, 732);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.StatusStrip.Size = new System.Drawing.Size(872, 25);
            this.StatusStrip.TabIndex = 0;
            this.StatusStrip.Text = "statusStrip1";
            // 
            // lblLastUpdatedCaption
            // 
            this.lblLastUpdatedCaption.Name = "lblLastUpdatedCaption";
            this.lblLastUpdatedCaption.Size = new System.Drawing.Size(100, 20);
            this.lblLastUpdatedCaption.Text = "Last Updated:";
            // 
            // lblLastUpdated
            // 
            this.lblLastUpdated.AutoSize = false;
            this.lblLastUpdated.Name = "lblLastUpdated";
            this.lblLastUpdated.Size = new System.Drawing.Size(752, 20);
            this.lblLastUpdated.Spring = true;
            this.lblLastUpdated.Text = "...";
            this.lblLastUpdated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLaunchManager,
            this.btnLaunchUpdater});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(872, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnLaunchManager
            // 
            this.btnLaunchManager.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLaunchManager.Image = ((System.Drawing.Image)(resources.GetObject("btnLaunchManager.Image")));
            this.btnLaunchManager.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLaunchManager.Name = "btnLaunchManager";
            this.btnLaunchManager.Size = new System.Drawing.Size(122, 24);
            this.btnLaunchManager.Text = "Launch Manager";
            this.btnLaunchManager.Click += new System.EventHandler(this.btnLaunchManager_Click);
            // 
            // btnLaunchUpdater
            // 
            this.btnLaunchUpdater.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLaunchUpdater.Image = ((System.Drawing.Image)(resources.GetObject("btnLaunchUpdater.Image")));
            this.btnLaunchUpdater.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLaunchUpdater.Name = "btnLaunchUpdater";
            this.btnLaunchUpdater.Size = new System.Drawing.Size(117, 24);
            this.btnLaunchUpdater.Text = "Launch Updater";
            this.btnLaunchUpdater.Click += new System.EventHandler(this.btnLaunchUpdater_Click);
            // 
            // BinsOverviewViewer
            // 
            this.BinsOverviewViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.BinsOverviewViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BinsOverviewViewer.Location = new System.Drawing.Point(0, 27);
            this.BinsOverviewViewer.Margin = new System.Windows.Forms.Padding(4);
            this.BinsOverviewViewer.MaxColumns = 10;
            this.BinsOverviewViewer.Name = "BinsOverviewViewer";
            this.BinsOverviewViewer.Size = new System.Drawing.Size(872, 705);
            this.BinsOverviewViewer.TabIndex = 4;
            // 
            // RoomMonitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 757);
            this.Controls.Add(this.BinsOverviewViewer);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.StatusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "RoomMonitorForm";
            this.Text = "Room Monitor - e-Docs USA, Inc.";
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblLastUpdatedCaption;
        private System.Windows.Forms.ToolStripStatusLabel lblLastUpdated;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private GridPanel BinsOverviewViewer;
        private System.Windows.Forms.ToolStripButton btnLaunchManager;
        private System.Windows.Forms.ToolStripButton btnLaunchUpdater;
    }
}