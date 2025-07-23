namespace Scanquire
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.ProgressMonitor = new Scanquire.Public.UserControls.ProgressMonitorToolstripItem();
            this.TotalPagesCaptionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.TotalPagesLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.IncludedPagesCaptionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.IncludedPagesLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.SelectedPagesCaptionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.SelectedPagesLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ActivePageNumberCaptionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ActivePageNumberLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainMenuPanel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.MainMenuToolStrip = new Scanquire.Public.UserControls.SQToolStrip();
            this.NewDocumentMenuButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.NewFromScannerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewFromArchiveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewFromLocalFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewFromCommandMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewFromCustomMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AppendMenuButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.AppendFromScannerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AppendFromArchiveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AppendFromLocalFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AppendFromCommandMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AppendFromCustomMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InsertMenuButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.InsertFromScannerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InsertFromArchiveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InsertFromLocalFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InsertFromCommandMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InsertFromCustomMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveMenuButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.SaveToArchiveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToLocalFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.selectedToArchiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsDelCheckImages = new System.Windows.Forms.ToolStripButton();
            this.tsBtnEditImage = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsCropImageAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.tsCCRImage = new System.Windows.Forms.ToolStripMenuItem();
            this.scannerOptions = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsScanImageSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsCombBox = new System.Windows.Forms.ToolStripComboBox();
            this.tsImageOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteBlankImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsCheckBkankImages = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMenuAutoAQ = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripAutoQcChecked = new System.Windows.Forms.ToolStripMenuItem();
            this.autoQAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsRunAutoQA = new System.Windows.Forms.ToolStripMenuItem();
            this.tsRecoverImages = new System.Windows.Forms.ToolStripMenuItem();
            this.tsImageRecovery = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsMenuButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.ClearOnSaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generatePreviewImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewSessionLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsManageUsers = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.tsAddUsers = new System.Windows.Forms.ToolStripMenuItem();
            this.addUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetPassWordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsBtnWebSite = new System.Windows.Forms.ToolStripButton();
            this.DisplayHelpMenuButton = new System.Windows.Forms.ToolStripButton();
            this.tsBtnCloseScanQuire = new System.Windows.Forms.ToolStripButton();
            this.CurrentArchiverSelector = new Scanquire.Public.UserControls.SQArchiverSelector();
            this.ImageListViewer = new Scanquire.Public.UserControls.SQImageListViewer();
            this.selectedToLocalFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusStrip.SuspendLayout();
            this.MainMenuPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.MainMenuToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgressMonitor,
            this.TotalPagesCaptionLabel,
            this.TotalPagesLabel,
            this.IncludedPagesCaptionLabel,
            this.IncludedPagesLabel,
            this.SelectedPagesCaptionLabel,
            this.SelectedPagesLabel,
            this.ActivePageNumberCaptionLabel,
            this.ActivePageNumberLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 514);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(1458, 27);
            this.StatusStrip.TabIndex = 0;
            this.StatusStrip.Text = "statusStrip1";
            // 
            // ProgressMonitor
            // 
            this.ProgressMonitor.AutoSize = false;
            this.ProgressMonitor.Name = "ProgressMonitor";
            this.ProgressMonitor.Size = new System.Drawing.Size(200, 25);
            // 
            // TotalPagesCaptionLabel
            // 
            this.TotalPagesCaptionLabel.Name = "TotalPagesCaptionLabel";
            this.TotalPagesCaptionLabel.Size = new System.Drawing.Size(951, 22);
            this.TotalPagesCaptionLabel.Spring = true;
            this.TotalPagesCaptionLabel.Text = "Pages:";
            this.TotalPagesCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TotalPagesLabel
            // 
            this.TotalPagesLabel.Name = "TotalPagesLabel";
            this.TotalPagesLabel.Size = new System.Drawing.Size(25, 22);
            this.TotalPagesLabel.Text = "000";
            // 
            // IncludedPagesCaptionLabel
            // 
            this.IncludedPagesCaptionLabel.Name = "IncludedPagesCaptionLabel";
            this.IncludedPagesCaptionLabel.Size = new System.Drawing.Size(56, 22);
            this.IncludedPagesCaptionLabel.Text = "Included:";
            this.IncludedPagesCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IncludedPagesLabel
            // 
            this.IncludedPagesLabel.Name = "IncludedPagesLabel";
            this.IncludedPagesLabel.Size = new System.Drawing.Size(25, 22);
            this.IncludedPagesLabel.Text = "000";
            // 
            // SelectedPagesCaptionLabel
            // 
            this.SelectedPagesCaptionLabel.Name = "SelectedPagesCaptionLabel";
            this.SelectedPagesCaptionLabel.Size = new System.Drawing.Size(54, 22);
            this.SelectedPagesCaptionLabel.Text = "Selected:";
            this.SelectedPagesCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SelectedPagesLabel
            // 
            this.SelectedPagesLabel.Name = "SelectedPagesLabel";
            this.SelectedPagesLabel.Size = new System.Drawing.Size(25, 22);
            this.SelectedPagesLabel.Text = "000";
            // 
            // ActivePageNumberCaptionLabel
            // 
            this.ActivePageNumberCaptionLabel.Name = "ActivePageNumberCaptionLabel";
            this.ActivePageNumberCaptionLabel.Size = new System.Drawing.Size(82, 22);
            this.ActivePageNumberCaptionLabel.Text = "Active Page #:";
            this.ActivePageNumberCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ActivePageNumberLabel
            // 
            this.ActivePageNumberLabel.Name = "ActivePageNumberLabel";
            this.ActivePageNumberLabel.Size = new System.Drawing.Size(25, 22);
            this.ActivePageNumberLabel.Text = "000";
            // 
            // MainMenuPanel
            // 
            this.MainMenuPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainMenuPanel.Controls.Add(this.pictureBox1);
            this.MainMenuPanel.Controls.Add(this.MainMenuToolStrip);
            this.MainMenuPanel.Controls.Add(this.CurrentArchiverSelector);
            this.MainMenuPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainMenuPanel.Location = new System.Drawing.Point(0, 0);
            this.MainMenuPanel.Name = "MainMenuPanel";
            this.MainMenuPanel.Size = new System.Drawing.Size(1458, 75);
            this.MainMenuPanel.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox1.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.ErrorImage")));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(1354, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 71);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // MainMenuToolStrip
            // 
            this.MainMenuToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.MainMenuToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.MainMenuToolStrip.ImageScalingSize = new System.Drawing.Size(50, 50);
            this.MainMenuToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewDocumentMenuButton,
            this.AppendMenuButton,
            this.InsertMenuButton,
            this.SaveMenuButton,
            this.tsDelCheckImages,
            this.tsBtnEditImage,
            this.scannerOptions,
            this.SettingsMenuButton,
            this.tsManageUsers,
            this.tsBtnWebSite,
            this.DisplayHelpMenuButton,
            this.tsBtnCloseScanQuire});
            this.MainMenuToolStrip.Location = new System.Drawing.Point(250, 0);
            this.MainMenuToolStrip.Name = "MainMenuToolStrip";
            this.MainMenuToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MainMenuToolStrip.Size = new System.Drawing.Size(933, 72);
            this.MainMenuToolStrip.TabIndex = 1;
            this.MainMenuToolStrip.Text = "sqToolStrip1";
            // 
            // NewDocumentMenuButton
            // 
            this.NewDocumentMenuButton.DoubleClickEnabled = true;
            this.NewDocumentMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewFromScannerMenuItem,
            this.NewFromArchiveMenuItem,
            this.NewFromLocalFileMenuItem,
            this.NewFromCommandMenuItem,
            this.NewFromCustomMenuItem});
            this.NewDocumentMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("NewDocumentMenuButton.Image")));
            this.NewDocumentMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NewDocumentMenuButton.Name = "NewDocumentMenuButton";
            this.NewDocumentMenuButton.Size = new System.Drawing.Size(63, 69);
            this.NewDocumentMenuButton.Text = "&New";
            this.NewDocumentMenuButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.NewDocumentMenuButton.DoubleClick += new System.EventHandler(this.NewDocumentMenuButton_DoubleClick);
            this.NewDocumentMenuButton.MouseHover += new System.EventHandler(this.NewDocumentMenuButton_MouseHover);
            // 
            // NewFromScannerMenuItem
            // 
            this.NewFromScannerMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.NewFromScannerMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.NewFromScannerMenuItem.Name = "NewFromScannerMenuItem";
            this.NewFromScannerMenuItem.Size = new System.Drawing.Size(162, 22);
            this.NewFromScannerMenuItem.Text = "From &Scanner";
            this.NewFromScannerMenuItem.Click += new System.EventHandler(this.NewFromScannerMenuItem_Click);
            // 
            // NewFromArchiveMenuItem
            // 
            this.NewFromArchiveMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.NewFromArchiveMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.NewFromArchiveMenuItem.Name = "NewFromArchiveMenuItem";
            this.NewFromArchiveMenuItem.Size = new System.Drawing.Size(162, 22);
            this.NewFromArchiveMenuItem.Text = "From &Archive";
            this.NewFromArchiveMenuItem.Click += new System.EventHandler(this.NewFromArchiveMenuItem_Click);
            // 
            // NewFromLocalFileMenuItem
            // 
            this.NewFromLocalFileMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.NewFromLocalFileMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.NewFromLocalFileMenuItem.Name = "NewFromLocalFileMenuItem";
            this.NewFromLocalFileMenuItem.Size = new System.Drawing.Size(162, 22);
            this.NewFromLocalFileMenuItem.Text = "From &Local File";
            this.NewFromLocalFileMenuItem.Click += new System.EventHandler(this.NewFromLocalFileMenuItem_Click);
            // 
            // NewFromCommandMenuItem
            // 
            this.NewFromCommandMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.NewFromCommandMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.NewFromCommandMenuItem.Name = "NewFromCommandMenuItem";
            this.NewFromCommandMenuItem.Size = new System.Drawing.Size(162, 22);
            this.NewFromCommandMenuItem.Text = "From &Command";
            this.NewFromCommandMenuItem.Click += new System.EventHandler(this.NewFromCommandMenuItem_Click);
            // 
            // NewFromCustomMenuItem
            // 
            this.NewFromCustomMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.NewFromCustomMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.NewFromCustomMenuItem.Name = "NewFromCustomMenuItem";
            this.NewFromCustomMenuItem.Size = new System.Drawing.Size(162, 22);
            this.NewFromCustomMenuItem.Text = "From C&ustom";
            this.NewFromCustomMenuItem.Click += new System.EventHandler(this.NewFromCustomMenuItem_Click);
            // 
            // AppendMenuButton
            // 
            this.AppendMenuButton.DoubleClickEnabled = true;
            this.AppendMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AppendFromScannerMenuItem,
            this.AppendFromArchiveMenuItem,
            this.AppendFromLocalFileMenuItem,
            this.AppendFromCommandMenuItem,
            this.AppendFromCustomMenuItem});
            this.AppendMenuButton.Enabled = false;
            this.AppendMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("AppendMenuButton.Image")));
            this.AppendMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AppendMenuButton.Name = "AppendMenuButton";
            this.AppendMenuButton.Size = new System.Drawing.Size(63, 69);
            this.AppendMenuButton.Text = "&Append";
            this.AppendMenuButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.AppendMenuButton.DoubleClick += new System.EventHandler(this.AppendMenuButton_DoubleClick);
            // 
            // AppendFromScannerMenuItem
            // 
            this.AppendFromScannerMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AppendFromScannerMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.AppendFromScannerMenuItem.Name = "AppendFromScannerMenuItem";
            this.AppendFromScannerMenuItem.Size = new System.Drawing.Size(162, 22);
            this.AppendFromScannerMenuItem.Text = "From &Scanner";
            this.AppendFromScannerMenuItem.Click += new System.EventHandler(this.AppendFromScannerMenuItem_Click);
            // 
            // AppendFromArchiveMenuItem
            // 
            this.AppendFromArchiveMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AppendFromArchiveMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.AppendFromArchiveMenuItem.Name = "AppendFromArchiveMenuItem";
            this.AppendFromArchiveMenuItem.Size = new System.Drawing.Size(162, 22);
            this.AppendFromArchiveMenuItem.Text = "From &Archive";
            this.AppendFromArchiveMenuItem.Click += new System.EventHandler(this.AppendFromArchiveMenuItem_Click);
            // 
            // AppendFromLocalFileMenuItem
            // 
            this.AppendFromLocalFileMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AppendFromLocalFileMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.AppendFromLocalFileMenuItem.Name = "AppendFromLocalFileMenuItem";
            this.AppendFromLocalFileMenuItem.Size = new System.Drawing.Size(162, 22);
            this.AppendFromLocalFileMenuItem.Text = "From &Local File";
            this.AppendFromLocalFileMenuItem.Click += new System.EventHandler(this.AppendFromLocalFileMenuItem_Click);
            // 
            // AppendFromCommandMenuItem
            // 
            this.AppendFromCommandMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AppendFromCommandMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.AppendFromCommandMenuItem.Name = "AppendFromCommandMenuItem";
            this.AppendFromCommandMenuItem.Size = new System.Drawing.Size(162, 22);
            this.AppendFromCommandMenuItem.Text = "From &Command";
            this.AppendFromCommandMenuItem.Click += new System.EventHandler(this.AppendFromCommandMenuItem_Click);
            // 
            // AppendFromCustomMenuItem
            // 
            this.AppendFromCustomMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AppendFromCustomMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.AppendFromCustomMenuItem.Name = "AppendFromCustomMenuItem";
            this.AppendFromCustomMenuItem.Size = new System.Drawing.Size(162, 22);
            this.AppendFromCustomMenuItem.Text = "From C&ustom";
            this.AppendFromCustomMenuItem.Click += new System.EventHandler(this.AppendFromCustomMenuItem_Click);
            // 
            // InsertMenuButton
            // 
            this.InsertMenuButton.DoubleClickEnabled = true;
            this.InsertMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.InsertFromScannerMenuItem,
            this.InsertFromArchiveMenuItem,
            this.InsertFromLocalFileMenuItem,
            this.InsertFromCommandMenuItem,
            this.InsertFromCustomMenuItem});
            this.InsertMenuButton.Enabled = false;
            this.InsertMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("InsertMenuButton.Image")));
            this.InsertMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.InsertMenuButton.Name = "InsertMenuButton";
            this.InsertMenuButton.Size = new System.Drawing.Size(63, 69);
            this.InsertMenuButton.Text = "&Insert";
            this.InsertMenuButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.InsertMenuButton.DoubleClick += new System.EventHandler(this.InsertMenuButton_DoubleClick);
            // 
            // InsertFromScannerMenuItem
            // 
            this.InsertFromScannerMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.InsertFromScannerMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.InsertFromScannerMenuItem.Name = "InsertFromScannerMenuItem";
            this.InsertFromScannerMenuItem.Size = new System.Drawing.Size(162, 22);
            this.InsertFromScannerMenuItem.Text = "From &Scanner";
            this.InsertFromScannerMenuItem.Click += new System.EventHandler(this.InsertFromScannerMenuItem_Click);
            // 
            // InsertFromArchiveMenuItem
            // 
            this.InsertFromArchiveMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.InsertFromArchiveMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.InsertFromArchiveMenuItem.Name = "InsertFromArchiveMenuItem";
            this.InsertFromArchiveMenuItem.Size = new System.Drawing.Size(162, 22);
            this.InsertFromArchiveMenuItem.Text = "From &Archive";
            this.InsertFromArchiveMenuItem.Click += new System.EventHandler(this.InsertFromArchiveMenuItem_Click);
            // 
            // InsertFromLocalFileMenuItem
            // 
            this.InsertFromLocalFileMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.InsertFromLocalFileMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.InsertFromLocalFileMenuItem.Name = "InsertFromLocalFileMenuItem";
            this.InsertFromLocalFileMenuItem.Size = new System.Drawing.Size(162, 22);
            this.InsertFromLocalFileMenuItem.Text = "From &Local File";
            this.InsertFromLocalFileMenuItem.Click += new System.EventHandler(this.InsertFromLocalFileMenuItem_Click);
            // 
            // InsertFromCommandMenuItem
            // 
            this.InsertFromCommandMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.InsertFromCommandMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.InsertFromCommandMenuItem.Name = "InsertFromCommandMenuItem";
            this.InsertFromCommandMenuItem.Size = new System.Drawing.Size(162, 22);
            this.InsertFromCommandMenuItem.Text = "From &Command";
            this.InsertFromCommandMenuItem.Click += new System.EventHandler(this.InsertFromCommandMenuItem_Click);
            // 
            // InsertFromCustomMenuItem
            // 
            this.InsertFromCustomMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.InsertFromCustomMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.InsertFromCustomMenuItem.Name = "InsertFromCustomMenuItem";
            this.InsertFromCustomMenuItem.Size = new System.Drawing.Size(162, 22);
            this.InsertFromCustomMenuItem.Text = "From C&ustom";
            this.InsertFromCustomMenuItem.Click += new System.EventHandler(this.InsertFromCustomMenuItem_Click);
            // 
            // SaveMenuButton
            // 
            this.SaveMenuButton.DoubleClickEnabled = true;
            this.SaveMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveToArchiveMenuItem,
            this.SaveToLocalFileMenuItem,
            this.toolStripSeparator1,
            this.selectedToArchiveToolStripMenuItem,
            this.selectedToLocalFileToolStripMenuItem});
            this.SaveMenuButton.Enabled = false;
            this.SaveMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveMenuButton.Image")));
            this.SaveMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveMenuButton.Name = "SaveMenuButton";
            this.SaveMenuButton.Size = new System.Drawing.Size(63, 69);
            this.SaveMenuButton.Text = "&Save";
            this.SaveMenuButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SaveMenuButton.DoubleClick += new System.EventHandler(this.SaveMenuButton_DoubleClick);
            // 
            // SaveToArchiveMenuItem
            // 
            this.SaveToArchiveMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SaveToArchiveMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.SaveToArchiveMenuItem.Name = "SaveToArchiveMenuItem";
            this.SaveToArchiveMenuItem.Size = new System.Drawing.Size(185, 22);
            this.SaveToArchiveMenuItem.Text = "To &Archive";
            this.SaveToArchiveMenuItem.Click += new System.EventHandler(this.SaveToArchiveMenuItem_Click);
            // 
            // SaveToLocalFileMenuItem
            // 
            this.SaveToLocalFileMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SaveToLocalFileMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.SaveToLocalFileMenuItem.Name = "SaveToLocalFileMenuItem";
            this.SaveToLocalFileMenuItem.Size = new System.Drawing.Size(185, 22);
            this.SaveToLocalFileMenuItem.Text = "To &Local File";
            this.SaveToLocalFileMenuItem.Click += new System.EventHandler(this.SaveToLocalFileMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(182, 6);
            // 
            // selectedToArchiveToolStripMenuItem
            // 
            this.selectedToArchiveToolStripMenuItem.Name = "selectedToArchiveToolStripMenuItem";
            this.selectedToArchiveToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.selectedToArchiveToolStripMenuItem.Text = "Selected To A&rchive";
            this.selectedToArchiveToolStripMenuItem.Click += new System.EventHandler(this.selectedToArchiveToolStripMenuItem_Click);
            // 
            // tsDelCheckImages
            // 
            this.tsDelCheckImages.Enabled = false;
            this.tsDelCheckImages.Image = ((System.Drawing.Image)(resources.GetObject("tsDelCheckImages.Image")));
            this.tsDelCheckImages.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsDelCheckImages.Name = "tsDelCheckImages";
            this.tsDelCheckImages.Size = new System.Drawing.Size(118, 69);
            this.tsDelCheckImages.Text = "&Del Checked Images";
            this.tsDelCheckImages.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsDelCheckImages.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsDelCheckImages.Click += new System.EventHandler(this.TsDelCheckImages_Click);
            // 
            // tsBtnEditImage
            // 
            this.tsBtnEditImage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsCropImageAdd,
            this.tsCCRImage});
            this.tsBtnEditImage.Enabled = false;
            this.tsBtnEditImage.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnEditImage.Image")));
            this.tsBtnEditImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnEditImage.Name = "tsBtnEditImage";
            this.tsBtnEditImage.Size = new System.Drawing.Size(76, 69);
            this.tsBtnEditImage.Text = "Edit Image";
            this.tsBtnEditImage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsBtnEditImage.Click += new System.EventHandler(this.tsBtnEditImage_Click);
            // 
            // tsCropImageAdd
            // 
            this.tsCropImageAdd.Image = ((System.Drawing.Image)(resources.GetObject("tsCropImageAdd.Image")));
            this.tsCropImageAdd.Name = "tsCropImageAdd";
            this.tsCropImageAdd.Size = new System.Drawing.Size(185, 22);
            this.tsCropImageAdd.Text = "Crop Image Add Text";
            this.tsCropImageAdd.Click += new System.EventHandler(this.tsCropImageAdd_Click);
            // 
            // tsCCRImage
            // 
            this.tsCCRImage.Image = ((System.Drawing.Image)(resources.GetObject("tsCCRImage.Image")));
            this.tsCCRImage.Name = "tsCCRImage";
            this.tsCCRImage.Size = new System.Drawing.Size(185, 22);
            this.tsCCRImage.Text = "OCR Image";
            this.tsCCRImage.Click += new System.EventHandler(this.tsCCRImage_Click);
            // 
            // scannerOptions
            // 
            this.scannerOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsScanImageSettings,
            this.tsImageOptions,
            this.tsMenuAutoAQ,
            this.tsRecoverImages});
            this.scannerOptions.Image = ((System.Drawing.Image)(resources.GetObject("scannerOptions.Image")));
            this.scannerOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.scannerOptions.Name = "scannerOptions";
            this.scannerOptions.Size = new System.Drawing.Size(90, 69);
            this.scannerOptions.Text = "Scan &Options";
            this.scannerOptions.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // tsScanImageSettings
            // 
            this.tsScanImageSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsCombBox});
            this.tsScanImageSettings.Name = "tsScanImageSettings";
            this.tsScanImageSettings.Size = new System.Drawing.Size(180, 22);
            this.tsScanImageSettings.Text = "Scan Image Options";
            // 
            // tsCombBox
            // 
            this.tsCombBox.Items.AddRange(new object[] {
            "Mark Blank Documents",
            "Remove Blank Documents",
            "Scan Blank Documents"});
            this.tsCombBox.Name = "tsCombBox";
            this.tsCombBox.Size = new System.Drawing.Size(121, 23);
            this.tsCombBox.Text = "Mark Blank Documents";
            this.tsCombBox.SelectedIndexChanged += new System.EventHandler(this.TsCombBox_SelectedIndexChanged);
            // 
            // tsImageOptions
            // 
            this.tsImageOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteBlankImagesToolStripMenuItem,
            this.tsCheckBkankImages});
            this.tsImageOptions.Name = "tsImageOptions";
            this.tsImageOptions.Size = new System.Drawing.Size(180, 22);
            this.tsImageOptions.Text = "Image Options";
            // 
            // deleteBlankImagesToolStripMenuItem
            // 
            this.deleteBlankImagesToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.deleteBlankImagesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteBlankImagesToolStripMenuItem.Image")));
            this.deleteBlankImagesToolStripMenuItem.Name = "deleteBlankImagesToolStripMenuItem";
            this.deleteBlankImagesToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.deleteBlankImagesToolStripMenuItem.Text = "&Delete Checked Images";
            this.deleteBlankImagesToolStripMenuItem.ToolTipText = "Delete Checked Images";
            this.deleteBlankImagesToolStripMenuItem.Click += new System.EventHandler(this.deleteBlankImagesToolStripMenuItem_Click_1);
            // 
            // tsCheckBkankImages
            // 
            this.tsCheckBkankImages.Name = "tsCheckBkankImages";
            this.tsCheckBkankImages.Size = new System.Drawing.Size(197, 22);
            this.tsCheckBkankImages.Text = "Mark Blank Images";
            this.tsCheckBkankImages.Click += new System.EventHandler(this.tsCheckBkankImages_Click);
            // 
            // tsMenuAutoAQ
            // 
            this.tsMenuAutoAQ.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripAutoQcChecked,
            this.autoQAToolStripMenuItem,
            this.tsRunAutoQA});
            this.tsMenuAutoAQ.Name = "tsMenuAutoAQ";
            this.tsMenuAutoAQ.Size = new System.Drawing.Size(180, 22);
            this.tsMenuAutoAQ.Text = "AutoQA Options";
            // 
            // toolStripAutoQcChecked
            // 
            this.toolStripAutoQcChecked.Image = global::Scanquire.Properties.Resources.AutoQAChecked;
            this.toolStripAutoQcChecked.Name = "toolStripAutoQcChecked";
            this.toolStripAutoQcChecked.Size = new System.Drawing.Size(161, 22);
            this.toolStripAutoQcChecked.Text = "Checked Images";
            this.toolStripAutoQcChecked.ToolTipText = "AutoQa CheckedImages";
            this.toolStripAutoQcChecked.Click += new System.EventHandler(this.toolStripAutoQcChecked_Click);
            // 
            // autoQAToolStripMenuItem
            // 
            this.autoQAToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("autoQAToolStripMenuItem.Image")));
            this.autoQAToolStripMenuItem.Name = "autoQAToolStripMenuItem";
            this.autoQAToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.autoQAToolStripMenuItem.Text = "All Images";
            this.autoQAToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.autoQAToolStripMenuItem.ToolTipText = "Auto QA all Images";
            this.autoQAToolStripMenuItem.Click += new System.EventHandler(this.autoQAToolStripMenuItem_Click);
            // 
            // tsRunAutoQA
            // 
            this.tsRunAutoQA.CheckOnClick = true;
            this.tsRunAutoQA.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsRunAutoQA.Name = "tsRunAutoQA";
            this.tsRunAutoQA.Size = new System.Drawing.Size(161, 22);
            this.tsRunAutoQA.Text = "Scan AutoQA";
            this.tsRunAutoQA.ToolTipText = "Run AutoQa while scanning";
            this.tsRunAutoQA.Click += new System.EventHandler(this.tsRunAutoQA_Click);
            // 
            // tsRecoverImages
            // 
            this.tsRecoverImages.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsImageRecovery});
            this.tsRecoverImages.Name = "tsRecoverImages";
            this.tsRecoverImages.Size = new System.Drawing.Size(180, 22);
            this.tsRecoverImages.Text = "Recover";
            // 
            // tsImageRecovery
            // 
            this.tsImageRecovery.Image = ((System.Drawing.Image)(resources.GetObject("tsImageRecovery.Image")));
            this.tsImageRecovery.Name = "tsImageRecovery";
            this.tsImageRecovery.Size = new System.Drawing.Size(112, 22);
            this.tsImageRecovery.Text = "Images";
            this.tsImageRecovery.Click += new System.EventHandler(this.tsImageRecovery_Click);
            // 
            // SettingsMenuButton
            // 
            this.SettingsMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ClearOnSaveMenuItem,
            this.generatePreviewImagesToolStripMenuItem,
            this.ViewSessionLogToolStripMenuItem});
            this.SettingsMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("SettingsMenuButton.Image")));
            this.SettingsMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SettingsMenuButton.Name = "SettingsMenuButton";
            this.SettingsMenuButton.Size = new System.Drawing.Size(63, 69);
            this.SettingsMenuButton.Text = "&Settings";
            this.SettingsMenuButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // ClearOnSaveMenuItem
            // 
            this.ClearOnSaveMenuItem.Checked = true;
            this.ClearOnSaveMenuItem.CheckOnClick = true;
            this.ClearOnSaveMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ClearOnSaveMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ClearOnSaveMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ClearOnSaveMenuItem.Name = "ClearOnSaveMenuItem";
            this.ClearOnSaveMenuItem.Size = new System.Drawing.Size(206, 22);
            this.ClearOnSaveMenuItem.Text = "Clear On Save";
            // 
            // generatePreviewImagesToolStripMenuItem
            // 
            this.generatePreviewImagesToolStripMenuItem.CheckOnClick = true;
            this.generatePreviewImagesToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.generatePreviewImagesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.generatePreviewImagesToolStripMenuItem.Name = "generatePreviewImagesToolStripMenuItem";
            this.generatePreviewImagesToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.generatePreviewImagesToolStripMenuItem.Text = "Generate Preview Images";
            // 
            // ViewSessionLogToolStripMenuItem
            // 
            this.ViewSessionLogToolStripMenuItem.Name = "ViewSessionLogToolStripMenuItem";
            this.ViewSessionLogToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.ViewSessionLogToolStripMenuItem.Text = "View Session Log";
            this.ViewSessionLogToolStripMenuItem.Click += new System.EventHandler(this.ViewSessionLogToolStripMenuItem_Click);
            // 
            // tsManageUsers
            // 
            this.tsManageUsers.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator8,
            this.tsAddUsers,
            this.resetPassWordToolStripMenuItem});
            this.tsManageUsers.Image = ((System.Drawing.Image)(resources.GetObject("tsManageUsers.Image")));
            this.tsManageUsers.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsManageUsers.Name = "tsManageUsers";
            this.tsManageUsers.Size = new System.Drawing.Size(94, 69);
            this.tsManageUsers.Text = "&Manage Users";
            this.tsManageUsers.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsManageUsers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(154, 6);
            // 
            // tsAddUsers
            // 
            this.tsAddUsers.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addUserToolStripMenuItem,
            this.deleteUserToolStripMenuItem});
            this.tsAddUsers.Name = "tsAddUsers";
            this.tsAddUsers.Size = new System.Drawing.Size(157, 22);
            this.tsAddUsers.Text = "Users";
            // 
            // addUserToolStripMenuItem
            // 
            this.addUserToolStripMenuItem.Name = "addUserToolStripMenuItem";
            this.addUserToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addUserToolStripMenuItem.Text = "Add-Edit Users";
            this.addUserToolStripMenuItem.Click += new System.EventHandler(this.AddUserToolStripMenuItem_Click);
            // 
            // deleteUserToolStripMenuItem
            // 
            this.deleteUserToolStripMenuItem.Name = "deleteUserToolStripMenuItem";
            this.deleteUserToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteUserToolStripMenuItem.Text = "Delete User";
            this.deleteUserToolStripMenuItem.Click += new System.EventHandler(this.DeleteUserToolStripMenuItem_Click);
            // 
            // resetPassWordToolStripMenuItem
            // 
            this.resetPassWordToolStripMenuItem.Name = "resetPassWordToolStripMenuItem";
            this.resetPassWordToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.resetPassWordToolStripMenuItem.Text = "Reset PassWord";
            this.resetPassWordToolStripMenuItem.Click += new System.EventHandler(this.ResetPassWordToolStripMenuItem_Click);
            // 
            // tsBtnWebSite
            // 
            this.tsBtnWebSite.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnWebSite.Image")));
            this.tsBtnWebSite.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnWebSite.Name = "tsBtnWebSite";
            this.tsBtnWebSite.Size = new System.Drawing.Size(57, 69);
            this.tsBtnWebSite.Text = "Web Site";
            this.tsBtnWebSite.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsBtnWebSite.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsBtnWebSite.Click += new System.EventHandler(this.tsBtnWebSite_Click);
            this.tsBtnWebSite.MouseLeave += new System.EventHandler(this.tsBtnWebSite_MouseLeave);
            this.tsBtnWebSite.MouseHover += new System.EventHandler(this.tsBtnWebSite_MouseHover);
            // 
            // DisplayHelpMenuButton
            // 
            this.DisplayHelpMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("DisplayHelpMenuButton.Image")));
            this.DisplayHelpMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DisplayHelpMenuButton.Name = "DisplayHelpMenuButton";
            this.DisplayHelpMenuButton.Size = new System.Drawing.Size(54, 69);
            this.DisplayHelpMenuButton.Text = "&Help";
            this.DisplayHelpMenuButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.DisplayHelpMenuButton.Click += new System.EventHandler(this.DisplayHelpMenuButton_Click);
            // 
            // tsBtnCloseScanQuire
            // 
            this.tsBtnCloseScanQuire.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnCloseScanQuire.Image")));
            this.tsBtnCloseScanQuire.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnCloseScanQuire.Name = "tsBtnCloseScanQuire";
            this.tsBtnCloseScanQuire.Size = new System.Drawing.Size(95, 69);
            this.tsBtnCloseScanQuire.Text = "&Close Scanquire";
            this.tsBtnCloseScanQuire.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsBtnCloseScanQuire.ToolTipText = "Close Scanquire";
            this.tsBtnCloseScanQuire.Click += new System.EventHandler(this.TsBtnCloseScanQuire_Click);
            // 
            // CurrentArchiverSelector
            // 
            this.CurrentArchiverSelector.Dock = System.Windows.Forms.DockStyle.Left;
            this.CurrentArchiverSelector.Location = new System.Drawing.Point(0, 0);
            this.CurrentArchiverSelector.Margin = new System.Windows.Forms.Padding(0);
            this.CurrentArchiverSelector.MaximumSize = new System.Drawing.Size(250, 50);
            this.CurrentArchiverSelector.Name = "CurrentArchiverSelector";
            this.CurrentArchiverSelector.Padding = new System.Windows.Forms.Padding(3);
            this.CurrentArchiverSelector.Size = new System.Drawing.Size(250, 50);
            this.CurrentArchiverSelector.TabIndex = 0;
            // 
            // ImageListViewer
            // 
            this.ImageListViewer.ActiveItem = null;
            this.ImageListViewer.CurrentThumbnailSizeMode = Scanquire.Public.UserControls.SQImageListViewer.ThumbnailSizeMode.Small;
            this.ImageListViewer.DeskewAngle = 1F;
            this.ImageListViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageListViewer.FillColor = System.Drawing.Color.White;
            this.ImageListViewer.LargeThumbnailSize = new System.Drawing.Size(255, 330);
            this.ImageListViewer.Location = new System.Drawing.Point(0, 75);
            this.ImageListViewer.MediumThumbnailSize = new System.Drawing.Size(170, 220);
            this.ImageListViewer.Name = "ImageListViewer";
            this.ImageListViewer.Saved = true;
            this.ImageListViewer.Size = new System.Drawing.Size(1458, 439);
            this.ImageListViewer.SmallThumbnailSize = new System.Drawing.Size(85, 110);
            this.ImageListViewer.TabIndex = 2;
            this.ImageListViewer.ViewMode = Scanquire.Public.UserControls.SQImageListViewer.ImageThumbnailViewMode.ThumbnailsAndImage;
            this.ImageListViewer.ItemCountChanged += new System.EventHandler(this.ImageListViewer_ItemCountChanged);
            this.ImageListViewer.SelectedItemCountChanged += new System.EventHandler(this.ImageListViewer_SelectedItemsChanged);
            this.ImageListViewer.CheckedItemCountChanged += new System.EventHandler(this.ImageListViewer_CheckedItemsChanged);
            this.ImageListViewer.ActiveItemChanged += new System.EventHandler(this.ImageListViewer_ActiveItemChanged);
            // 
            // selectedToLocalFileToolStripMenuItem
            // 
            this.selectedToLocalFileToolStripMenuItem.Name = "selectedToLocalFileToolStripMenuItem";
            this.selectedToLocalFileToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.selectedToLocalFileToolStripMenuItem.Text = "Selected To Local &File";
            this.selectedToLocalFileToolStripMenuItem.Click += new System.EventHandler(this.selectedToLocalFileToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1458, 541);
            this.Controls.Add(this.ImageListViewer);
            this.Controls.Add(this.MainMenuPanel);
            this.Controls.Add(this.StatusStrip);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "Scanquire - e-Docs USA Version 7.0";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.MainMenuPanel.ResumeLayout(false);
            this.MainMenuPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.MainMenuToolStrip.ResumeLayout(false);
            this.MainMenuToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

     









        #endregion

        private System.Windows.Forms.StatusStrip StatusStrip;
        private Public.UserControls.ProgressMonitorToolstripItem ProgressMonitor;
        private System.Windows.Forms.Panel MainMenuPanel;
        private Public.UserControls.SQImageListViewer ImageListViewer;
        private Public.UserControls.SQArchiverSelector CurrentArchiverSelector;
        private Public.UserControls.SQToolStrip MainMenuToolStrip;
        private System.Windows.Forms.ToolStripDropDownButton NewDocumentMenuButton;
        private System.Windows.Forms.ToolStripDropDownButton AppendMenuButton;
        private System.Windows.Forms.ToolStripDropDownButton InsertMenuButton;
        private System.Windows.Forms.ToolStripDropDownButton SaveMenuButton;
        private System.Windows.Forms.ToolStripDropDownButton SettingsMenuButton;
        private System.Windows.Forms.ToolStripButton DisplayHelpMenuButton;
        private System.Windows.Forms.ToolStripMenuItem NewFromScannerMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewFromArchiveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewFromLocalFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewFromCommandMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewFromCustomMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AppendFromScannerMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AppendFromArchiveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AppendFromLocalFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AppendFromCommandMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AppendFromCustomMenuItem;
        private System.Windows.Forms.ToolStripMenuItem InsertFromScannerMenuItem;
        private System.Windows.Forms.ToolStripMenuItem InsertFromArchiveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem InsertFromLocalFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem InsertFromCommandMenuItem;
        private System.Windows.Forms.ToolStripMenuItem InsertFromCustomMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveToArchiveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveToLocalFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClearOnSaveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generatePreviewImagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel TotalPagesCaptionLabel;
        private System.Windows.Forms.ToolStripStatusLabel TotalPagesLabel;
        private System.Windows.Forms.ToolStripStatusLabel IncludedPagesCaptionLabel;
        private System.Windows.Forms.ToolStripStatusLabel IncludedPagesLabel;
        private System.Windows.Forms.ToolStripStatusLabel SelectedPagesCaptionLabel;
        private System.Windows.Forms.ToolStripStatusLabel SelectedPagesLabel;
        private System.Windows.Forms.ToolStripStatusLabel ActivePageNumberCaptionLabel;
        private System.Windows.Forms.ToolStripStatusLabel ActivePageNumberLabel;
        private System.Windows.Forms.ToolStripMenuItem ViewSessionLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton scannerOptions;
        private System.Windows.Forms.ToolStripDropDownButton tsManageUsers;
        private System.Windows.Forms.ToolStripMenuItem tsAddUsers;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem addUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetPassWordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsScanImageSettings;
        private System.Windows.Forms.ToolStripComboBox tsCombBox;
        private System.Windows.Forms.ToolStripMenuItem tsImageOptions;
        private System.Windows.Forms.ToolStripMenuItem deleteBlankImagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsCheckBkankImages;
        private System.Windows.Forms.ToolStripMenuItem tsMenuAutoAQ;
        private System.Windows.Forms.ToolStripMenuItem toolStripAutoQcChecked;
        private System.Windows.Forms.ToolStripMenuItem autoQAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsRunAutoQA;
        private System.Windows.Forms.ToolStripButton tsBtnCloseScanQuire;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripButton tsDelCheckImages;
        private System.Windows.Forms.ToolStripMenuItem tsRecoverImages;
        private System.Windows.Forms.ToolStripMenuItem tsImageRecovery;
        private System.Windows.Forms.ToolStripButton tsBtnWebSite;
        private System.Windows.Forms.ToolStripDropDownButton tsBtnEditImage;
        private System.Windows.Forms.ToolStripMenuItem tsCropImageAdd;
        private System.Windows.Forms.ToolStripMenuItem tsCCRImage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem selectedToArchiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectedToLocalFileToolStripMenuItem;
    }
}