namespace BinMonitor
{
    partial class BinMonitorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BinMonitorForm));
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.lblLastUpdatedCaption = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblLastUpdated = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnLaunchManager = new System.Windows.Forms.ToolStripButton();
            //this.btnLaunchUpdater = new System.Windows.Forms.ToolStripButton();
            this.pnlNextDue = new System.Windows.Forms.Panel();
            this.mcvRoutine = new BinMonitor.Common.MasterCategoryStatusViewer();
            this.mcvReady = new BinMonitor.Common.MasterCategoryStatusViewer();
            this.mcvStat = new BinMonitor.Common.MasterCategoryStatusViewer();
            this.pnlOverviewText = new System.Windows.Forms.Panel();
            this.activeBinsViewer = new BinMonitor.Common.ActiveBinsViewer();
            this.BinsOverviewViewer = new BinMonitor.GridPanel();
            this.StatusStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pnlNextDue.SuspendLayout();
            this.pnlOverviewText.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblLastUpdatedCaption,
            this.lblLastUpdated});
            this.StatusStrip.Location = new System.Drawing.Point(0, 593);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(654, 22);
            this.StatusStrip.TabIndex = 0;
            this.StatusStrip.Text = "statusStrip1";
            // 
            // lblLastUpdatedCaption
            // 
            this.lblLastUpdatedCaption.Name = "lblLastUpdatedCaption";
            this.lblLastUpdatedCaption.Size = new System.Drawing.Size(79, 17);
            this.lblLastUpdatedCaption.Text = "Last Updated:";
            // 
            // lblLastUpdated
            // 
            this.lblLastUpdated.AutoSize = false;
            this.lblLastUpdated.Name = "lblLastUpdated";
            this.lblLastUpdated.Size = new System.Drawing.Size(560, 17);
            this.lblLastUpdated.Spring = true;
            this.lblLastUpdated.Text = "...";
            this.lblLastUpdated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLaunchManager
            //,this.btnLaunchUpdater
            });
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(654, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnLaunchManager
            // 
            this.btnLaunchManager.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLaunchManager.Image = ((System.Drawing.Image)(resources.GetObject("btnLaunchManager.Image")));
            this.btnLaunchManager.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLaunchManager.Name = "btnLaunchManager";
            this.btnLaunchManager.Size = new System.Drawing.Size(100, 22);
            this.btnLaunchManager.Text = "Launch Manager";
            this.btnLaunchManager.Click += new System.EventHandler(this.btnLaunchManager_Click);
            // 
            // btnLaunchUpdater
            // 
            /*
            this.btnLaunchUpdater.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLaunchUpdater.Image = ((System.Drawing.Image)(resources.GetObject("btnLaunchUpdater.Image")));
            this.btnLaunchUpdater.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLaunchUpdater.Name = "btnLaunchUpdater";
            this.btnLaunchUpdater.Size = new System.Drawing.Size(95, 22);
            this.btnLaunchUpdater.Text = "Launch Updater";
            this.btnLaunchUpdater.Click += new System.EventHandler(this.btnLaunchUpdater_Click);*/
            // 
            // pnlNextDue
            // 
            this.pnlNextDue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlNextDue.Controls.Add(this.mcvRoutine);
            this.pnlNextDue.Controls.Add(this.mcvReady);
            this.pnlNextDue.Controls.Add(this.mcvStat);
            this.pnlNextDue.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlNextDue.Location = new System.Drawing.Point(0, 25);
            this.pnlNextDue.Name = "pnlNextDue";
            this.pnlNextDue.Size = new System.Drawing.Size(136, 568);
            this.pnlNextDue.TabIndex = 2;
            // 
            // mcvRoutine
            // 
            this.mcvRoutine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mcvRoutine.Caption = "ROUTINE";
            this.mcvRoutine.Dock = System.Windows.Forms.DockStyle.Top;
            this.mcvRoutine.Location = new System.Drawing.Point(0, 334);
            this.mcvRoutine.MasterCategoryTitle = "ROUTINE";
            this.mcvRoutine.Name = "mcvRoutine";
            this.mcvRoutine.Size = new System.Drawing.Size(132, 167);
            this.mcvRoutine.TabIndex = 2;
            // 
            // mcvReady
            // 
            this.mcvReady.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mcvReady.Caption = "READY";
            this.mcvReady.Dock = System.Windows.Forms.DockStyle.Top;
            this.mcvReady.Location = new System.Drawing.Point(0, 167);
            this.mcvReady.MasterCategoryTitle = "READY";
            this.mcvReady.Name = "mcvReady";
            this.mcvReady.Size = new System.Drawing.Size(132, 167);
            this.mcvReady.TabIndex = 1;
            // 
            // mcvStat
            // 
            this.mcvStat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mcvStat.Caption = "STAT";
            this.mcvStat.Dock = System.Windows.Forms.DockStyle.Top;
            this.mcvStat.Location = new System.Drawing.Point(0, 0);
            this.mcvStat.MasterCategoryTitle = "STAT";
            this.mcvStat.Name = "mcvStat";
            this.mcvStat.Size = new System.Drawing.Size(132, 167);
            this.mcvStat.TabIndex = 0;
            // 
            // pnlOverviewText
            // 
            this.pnlOverviewText.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlOverviewText.Controls.Add(this.activeBinsViewer);
            this.pnlOverviewText.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlOverviewText.Location = new System.Drawing.Point(508, 25);
            this.pnlOverviewText.Name = "pnlOverviewText";
            this.pnlOverviewText.Size = new System.Drawing.Size(146, 568);
            this.pnlOverviewText.TabIndex = 3;
            // 
            // activeBinsViewer
            // 
            this.activeBinsViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.activeBinsViewer.Location = new System.Drawing.Point(0, 0);
            this.activeBinsViewer.Name = "activeBinsViewer";
            this.activeBinsViewer.Size = new System.Drawing.Size(142, 564);
            this.activeBinsViewer.TabIndex = 0;
            this.activeBinsViewer.TabStop = false;
            // 
            // BinsOverviewViewer
            // 
            this.BinsOverviewViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.BinsOverviewViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BinsOverviewViewer.Location = new System.Drawing.Point(136, 25);
            this.BinsOverviewViewer.MaxColumns = 10;
            this.BinsOverviewViewer.Name = "BinsOverviewViewer";
            this.BinsOverviewViewer.Size = new System.Drawing.Size(372, 568);
            this.BinsOverviewViewer.TabIndex = 4;
            // 
            // BinMonitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 615);
            this.Controls.Add(this.BinsOverviewViewer);
            this.Controls.Add(this.pnlNextDue);
            this.Controls.Add(this.pnlOverviewText);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.StatusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BinMonitorForm";
            this.Text = "L8 Bin Monitor - e-Docs USA, Inc.";
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnlNextDue.ResumeLayout(false);
            this.pnlOverviewText.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblLastUpdatedCaption;
        private System.Windows.Forms.ToolStripStatusLabel lblLastUpdated;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel pnlNextDue;
        private System.Windows.Forms.Panel pnlOverviewText;
        private GridPanel BinsOverviewViewer;
        private System.Windows.Forms.ToolStripButton btnLaunchManager;
        private Common.MasterCategoryStatusViewer mcvRoutine;
        private Common.MasterCategoryStatusViewer mcvReady;
        private Common.MasterCategoryStatusViewer mcvStat;
        private Common.ActiveBinsViewer activeBinsViewer;
        private System.Windows.Forms.ToolStripButton btnLaunchUpdater;
    }
}