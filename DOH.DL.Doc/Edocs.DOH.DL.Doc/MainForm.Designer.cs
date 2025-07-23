
namespace Edocs.DOH.DL.Doc
{
    partial class DOHMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DOHMainForm));
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectRootDownloadFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.documentsNotDownloadedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelDownLoads = new System.Windows.Forms.ToolStripMenuItem();
            this.supportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabFiles = new System.Windows.Forms.TabControl();
            this.tabDonwLoaded = new System.Windows.Forms.TabPage();
            this.proBar = new System.Windows.Forms.ProgressBar();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataGVDlFiles = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.City = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Church = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BookType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImagesScanned = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DLFoler = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DlFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DlFileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ViewDl = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Reject = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tabViewFiles = new System.Windows.Forms.TabPage();
            this.viewFiles = new System.Windows.Forms.DataGridView();
            this.Folder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.View = new System.Windows.Forms.DataGridViewButtonColumn();
            this.compDrives = new System.Windows.Forms.TreeView();
            this.menuStrip1.SuspendLayout();
            this.tabFiles.SuspendLayout();
            this.tabDonwLoaded.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGVDlFiles)).BeginInit();
            this.tabViewFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.viewFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // bgWorker
            // 
            this.bgWorker.WorkerReportsProgress = true;
            this.bgWorker.WorkerSupportsCancellation = true;
            this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_DoWork);
            this.bgWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorker_ProgressChanged);
            this.bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.downloadToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.cancelDownLoads,
            this.supportToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(599, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectRootDownloadFolderToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // selectRootDownloadFolderToolStripMenuItem
            // 
            this.selectRootDownloadFolderToolStripMenuItem.Name = "selectRootDownloadFolderToolStripMenuItem";
            this.selectRootDownloadFolderToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.selectRootDownloadFolderToolStripMenuItem.Text = "&Select Download Folder";
            this.selectRootDownloadFolderToolStripMenuItem.Click += new System.EventHandler(this.selectRootDownloadFolderToolStripMenuItem_Click);
            // 
            // downloadToolStripMenuItem
            // 
            this.downloadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.documentsNotDownloadedToolStripMenuItem,
            this.toolStripMenuItem2});
            this.downloadToolStripMenuItem.Name = "downloadToolStripMenuItem";
            this.downloadToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.downloadToolStripMenuItem.Text = "&Download";
            // 
            // documentsNotDownloadedToolStripMenuItem
            // 
            this.documentsNotDownloadedToolStripMenuItem.Name = "documentsNotDownloadedToolStripMenuItem";
            this.documentsNotDownloadedToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.documentsNotDownloadedToolStripMenuItem.Text = "&New Documents";
            this.documentsNotDownloadedToolStripMenuItem.Click += new System.EventHandler(this.documentsNotDownloadedToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(205, 22);
            this.toolStripMenuItem2.Text = "&ReDownload Documents";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "&Settings";
            this.settingsToolStripMenuItem.Visible = false;
            // 
            // cancelDownLoads
            // 
            this.cancelDownLoads.Name = "cancelDownLoads";
            this.cancelDownLoads.Size = new System.Drawing.Size(117, 20);
            this.cancelDownLoads.Text = "&Cancel Downloads";
            this.cancelDownLoads.Click += new System.EventHandler(this.cancelDownLoads_Click);
            // 
            // supportToolStripMenuItem
            // 
            this.supportToolStripMenuItem.Name = "supportToolStripMenuItem";
            this.supportToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.supportToolStripMenuItem.Text = "&Support";
            this.supportToolStripMenuItem.Click += new System.EventHandler(this.supportToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // tabFiles
            // 
            this.tabFiles.Controls.Add(this.tabDonwLoaded);
            this.tabFiles.Controls.Add(this.tabViewFiles);
            this.tabFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFiles.Location = new System.Drawing.Point(0, 24);
            this.tabFiles.Name = "tabFiles";
            this.tabFiles.SelectedIndex = 0;
            this.tabFiles.Size = new System.Drawing.Size(599, 426);
            this.tabFiles.TabIndex = 9;
            this.tabFiles.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabFiles_Selected);
            // 
            // tabDonwLoaded
            // 
            this.tabDonwLoaded.Controls.Add(this.proBar);
            this.tabDonwLoaded.Controls.Add(this.statusStrip1);
            this.tabDonwLoaded.Controls.Add(this.dataGVDlFiles);
            this.tabDonwLoaded.Location = new System.Drawing.Point(4, 22);
            this.tabDonwLoaded.Name = "tabDonwLoaded";
            this.tabDonwLoaded.Padding = new System.Windows.Forms.Padding(3);
            this.tabDonwLoaded.Size = new System.Drawing.Size(591, 400);
            this.tabDonwLoaded.TabIndex = 0;
            this.tabDonwLoaded.Text = "Down Loaded Files";
            this.tabDonwLoaded.UseVisualStyleBackColor = true;
            // 
            // proBar
            // 
            this.proBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.proBar.Location = new System.Drawing.Point(3, 352);
            this.proBar.MarqueeAnimationSpeed = 50;
            this.proBar.Name = "proBar";
            this.proBar.Size = new System.Drawing.Size(585, 23);
            this.proBar.TabIndex = 2;
            this.proBar.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(3, 375);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(585, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.statusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(118, 17);
            this.statusLabel.Text = "toolStripStatusLabel1";
            // 
            // dataGVDlFiles
            // 
            this.dataGVDlFiles.AllowUserToAddRows = false;
            this.dataGVDlFiles.AllowUserToDeleteRows = false;
            this.dataGVDlFiles.AllowUserToOrderColumns = true;
            this.dataGVDlFiles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGVDlFiles.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGVDlFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGVDlFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.City,
            this.Church,
            this.BookType,
            this.StartDate,
            this.EndDate,
            this.ImagesScanned,
            this.DLFoler,
            this.DlFileName,
            this.DlFileSize,
            this.ViewDl,
            this.Reject});
            this.dataGVDlFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGVDlFiles.Location = new System.Drawing.Point(3, 3);
            this.dataGVDlFiles.MultiSelect = false;
            this.dataGVDlFiles.Name = "dataGVDlFiles";
            this.dataGVDlFiles.Size = new System.Drawing.Size(585, 394);
            this.dataGVDlFiles.TabIndex = 0;
            this.dataGVDlFiles.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGVDlFiles_CellContentClick);
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Visible = false;
            // 
            // City
            // 
            this.City.HeaderText = "City";
            this.City.Name = "City";
            // 
            // Church
            // 
            this.Church.HeaderText = "Church";
            this.Church.Name = "Church";
            // 
            // BookType
            // 
            this.BookType.HeaderText = "BookType";
            this.BookType.Name = "BookType";
            // 
            // StartDate
            // 
            this.StartDate.HeaderText = "Start Date";
            this.StartDate.Name = "StartDate";
            // 
            // EndDate
            // 
            this.EndDate.HeaderText = "End Date";
            this.EndDate.Name = "EndDate";
            // 
            // ImagesScanned
            // 
            this.ImagesScanned.HeaderText = "Images Scanned";
            this.ImagesScanned.Name = "ImagesScanned";
            // 
            // DLFoler
            // 
            this.DLFoler.HeaderText = "Down Load Folder";
            this.DLFoler.Name = "DLFoler";
            // 
            // DlFileName
            // 
            this.DlFileName.HeaderText = "File Name";
            this.DlFileName.Name = "DlFileName";
            // 
            // DlFileSize
            // 
            this.DlFileSize.HeaderText = "File Size";
            this.DlFileSize.Name = "DlFileSize";
            // 
            // ViewDl
            // 
            this.ViewDl.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ViewDl.HeaderText = "View";
            this.ViewDl.Name = "ViewDl";
            this.ViewDl.Text = "View";
            this.ViewDl.ToolTipText = "View PDF File";
            this.ViewDl.UseColumnTextForButtonValue = true;
            // 
            // Reject
            // 
            this.Reject.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Reject.HeaderText = "Reject";
            this.Reject.Name = "Reject";
            this.Reject.Text = "Reject";
            this.Reject.ToolTipText = "Reject Document";
            this.Reject.UseColumnTextForButtonValue = true;
            // 
            // tabViewFiles
            // 
            this.tabViewFiles.Controls.Add(this.viewFiles);
            this.tabViewFiles.Controls.Add(this.compDrives);
            this.tabViewFiles.Location = new System.Drawing.Point(4, 22);
            this.tabViewFiles.Name = "tabViewFiles";
            this.tabViewFiles.Padding = new System.Windows.Forms.Padding(3);
            this.tabViewFiles.Size = new System.Drawing.Size(591, 400);
            this.tabViewFiles.TabIndex = 1;
            this.tabViewFiles.Text = "View Files";
            this.tabViewFiles.UseVisualStyleBackColor = true;
            // 
            // viewFiles
            // 
            this.viewFiles.AllowUserToAddRows = false;
            this.viewFiles.AllowUserToDeleteRows = false;
            this.viewFiles.AllowUserToOrderColumns = true;
            this.viewFiles.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.viewFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.viewFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Folder,
            this.FileName,
            this.FSize,
            this.View});
            this.viewFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewFiles.Location = new System.Drawing.Point(124, 3);
            this.viewFiles.Name = "viewFiles";
            this.viewFiles.Size = new System.Drawing.Size(464, 394);
            this.viewFiles.TabIndex = 1;
            this.viewFiles.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.viewFiles_CellContentClick_1);
            // 
            // Folder
            // 
            this.Folder.HeaderText = "Folder";
            this.Folder.Name = "Folder";
            this.Folder.Width = 105;
            // 
            // FileName
            // 
            this.FileName.HeaderText = "File Name";
            this.FileName.Name = "FileName";
            this.FileName.Width = 106;
            // 
            // FSize
            // 
            this.FSize.HeaderText = "File Size";
            this.FSize.Name = "FSize";
            this.FSize.Width = 105;
            // 
            // View
            // 
            this.View.HeaderText = "View Doc";
            this.View.Name = "View";
            this.View.Text = "View Doc";
            this.View.Width = 105;
            // 
            // compDrives
            // 
            this.compDrives.Dock = System.Windows.Forms.DockStyle.Left;
            this.compDrives.Location = new System.Drawing.Point(3, 3);
            this.compDrives.Name = "compDrives";
            this.compDrives.Size = new System.Drawing.Size(121, 394);
            this.compDrives.TabIndex = 0;
            this.compDrives.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.compDrives_BeforeExpand);
            this.compDrives.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.compDrives_AfterExpand);
            this.compDrives.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.compDrives_AfterSelect);
            // 
            // DOHMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 450);
            this.Controls.Add(this.tabFiles);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "DOHMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "e_Docs USA Inc. Download Documents V1.2";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DOHMainForm_FormClosing);
            this.Load += new System.EventHandler(this.DOHMainForm_Load);
            this.Shown += new System.EventHandler(this.DOHMainForm_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabFiles.ResumeLayout(false);
            this.tabDonwLoaded.ResumeLayout(false);
            this.tabDonwLoaded.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGVDlFiles)).EndInit();
            this.tabViewFiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.viewFiles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bgWorker;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem documentsNotDownloadedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectRootDownloadFolderToolStripMenuItem;
        private System.Windows.Forms.TabControl tabFiles;
        private System.Windows.Forms.TabPage tabDonwLoaded;
        private System.Windows.Forms.TabPage tabViewFiles;
        private System.Windows.Forms.DataGridView viewFiles;
        private System.Windows.Forms.TreeView compDrives;
        private System.Windows.Forms.DataGridView dataGVDlFiles;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripMenuItem cancelDownLoads;
        private System.Windows.Forms.ProgressBar proBar;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Folder;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FSize;
        private System.Windows.Forms.DataGridViewButtonColumn View;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn City;
        private System.Windows.Forms.DataGridViewTextBoxColumn Church;
        private System.Windows.Forms.DataGridViewTextBoxColumn BookType;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImagesScanned;
        private System.Windows.Forms.DataGridViewTextBoxColumn DLFoler;
        private System.Windows.Forms.DataGridViewTextBoxColumn DlFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DlFileSize;
        private System.Windows.Forms.DataGridViewButtonColumn ViewDl;
        private System.Windows.Forms.DataGridViewButtonColumn Reject;
        private System.Windows.Forms.ToolStripMenuItem supportToolStripMenuItem;
    }
}

