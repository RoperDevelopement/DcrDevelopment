namespace Scanquire.Public.UserControls
{
    partial class SQImageListViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SelectFillColorDialog = new System.Windows.Forms.ColorDialog();
            this.SplitContainer = new Scanquire.Public.UserControls.SQSplitContainer();
            this.ThumbnailPanel = new Scanquire.Public.UserControls.SQThumbnailPanel();
            this.ProgressMonitor = new Scanquire.Public.UserControls.ProgressMonitor();
            this.ThumbnailScrollBar = new Scanquire.Public.UserControls.SQThumbnailScrollBar();
            this.ThumbnailToolStrip = new Scanquire.Public.UserControls.SQToolStrip();
            this.ChangeThumbnailSizeButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.InvertCheckSelectedImagesButton = new System.Windows.Forms.ToolStripButton();
            this.RotateSelectedImagesRightButton = new System.Windows.Forms.ToolStripButton();
            this.RotateSelectedImagesLeftButton = new System.Windows.Forms.ToolStripButton();
            this.RotateSelectedImages180DegreesButton = new System.Windows.Forms.ToolStripButton();
            this.FlipSelectedImagesHorizontalButton = new System.Windows.Forms.ToolStripButton();
            this.FlipSelectedImagesVerticalButton = new System.Windows.Forms.ToolStripButton();
            this.ActiveImageBorderPanel = new System.Windows.Forms.Panel();
            this.ActiveImageViewer = new EdocsUSA.Controls.ImageViewer();
            this.ActiveImageToolStripTabControl = new System.Windows.Forms.TabControl();
            this.ViewToolStripTabPage = new System.Windows.Forms.TabPage();
            this.ViewActiveImageToolStrip = new Scanquire.Public.UserControls.SQToolStrip();
            this.ZoomInButton = new System.Windows.Forms.ToolStripButton();
            this.ZoomOutButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ScaleToFitViewerButton = new System.Windows.Forms.ToolStripButton();
            this.ScaleToOriginalSizeButton = new System.Windows.Forms.ToolStripButton();
            this.ScaleFitViewerWidthButton = new System.Windows.Forms.ToolStripButton();
            this.ScaleToFitViewerHeightButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.DisplayImageInfoButton = new System.Windows.Forms.ToolStripButton();
            this.EditToolStripTabPage = new System.Windows.Forms.TabPage();
            this.EditActiveImageToolStrip = new Scanquire.Public.UserControls.SQToolStrip();
            this.CropSelectedRegionButton = new System.Windows.Forms.ToolStripButton();
            this.FillSelectedRegionButton = new System.Windows.Forms.ToolStripButton();
            this.SelectFillColorButton = new System.Windows.Forms.ToolStripButton();
            this.DeskewLeftButton = new System.Windows.Forms.ToolStripButton();
            this.DeskewRightButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.UndoRevisionButton = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).BeginInit();
            this.SplitContainer.Panel1.SuspendLayout();
            this.SplitContainer.Panel2.SuspendLayout();
            this.SplitContainer.SuspendLayout();
            this.ThumbnailToolStrip.SuspendLayout();
            this.ActiveImageBorderPanel.SuspendLayout();
            this.ActiveImageToolStripTabControl.SuspendLayout();
            this.ViewToolStripTabPage.SuspendLayout();
            this.ViewActiveImageToolStrip.SuspendLayout();
            this.EditToolStripTabPage.SuspendLayout();
            this.EditActiveImageToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // SplitContainer
            // 
            this.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer.Name = "SplitContainer";
            // 
            // SplitContainer.Panel1
            // 
            this.SplitContainer.Panel1.Controls.Add(this.ThumbnailPanel);
            this.SplitContainer.Panel1.Controls.Add(this.ProgressMonitor);
            this.SplitContainer.Panel1.Controls.Add(this.ThumbnailScrollBar);
            this.SplitContainer.Panel1.Controls.Add(this.ThumbnailToolStrip);
            // 
            // SplitContainer.Panel2
            // 
            this.SplitContainer.Panel2.Controls.Add(this.ActiveImageBorderPanel);
            this.SplitContainer.Panel2.Controls.Add(this.ActiveImageToolStripTabControl);
            this.SplitContainer.Size = new System.Drawing.Size(674, 477);
            this.SplitContainer.SplitterDistance = 224;
            this.SplitContainer.TabIndex = 0;
            this.SplitContainer.TabStop = false;
            // 
            // ThumbnailPanel
            // 
            this.ThumbnailPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThumbnailPanel.Location = new System.Drawing.Point(55, 0);
            this.ThumbnailPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ThumbnailPanel.Name = "ThumbnailPanel";
            this.ThumbnailPanel.Size = new System.Drawing.Size(152, 452);
            this.ThumbnailPanel.TabIndex = 8;
            this.ThumbnailPanel.Click += new System.EventHandler(this.ThumbnailPanel_Click);
            this.ThumbnailPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.ThumbnailPanel_Paint);
            this.ThumbnailPanel.Resize += new System.EventHandler(this.ThumbnailPanel_Resize);
            // 
            // ProgressMonitor
            // 
            this.ProgressMonitor.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ProgressMonitor.Location = new System.Drawing.Point(55, 452);
            this.ProgressMonitor.Name = "ProgressMonitor";
            this.ProgressMonitor.Size = new System.Drawing.Size(152, 25);
            this.ProgressMonitor.SupportsCancellation = false;
            this.ProgressMonitor.TabIndex = 10;
            this.ProgressMonitor.Visible = false;
            // 
            // ThumbnailScrollBar
            // 
            this.ThumbnailScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.ThumbnailScrollBar.LargeChange = 1;
            this.ThumbnailScrollBar.Location = new System.Drawing.Point(207, 0);
            this.ThumbnailScrollBar.Maximum = 0;
            this.ThumbnailScrollBar.Name = "ThumbnailScrollBar";
            this.ThumbnailScrollBar.Size = new System.Drawing.Size(17, 477);
            this.ThumbnailScrollBar.TabIndex = 9;
            
            this.ThumbnailScrollBar.ValueChanged += new System.EventHandler(this.ThumbnailScrollBar_ValueChanged);
            // 
            // ThumbnailToolStrip
            // 
            this.ThumbnailToolStrip.Dock = System.Windows.Forms.DockStyle.Left;
            this.ThumbnailToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ThumbnailToolStrip.ImageScalingSize = new System.Drawing.Size(50, 50);
            this.ThumbnailToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChangeThumbnailSizeButton,
            this.toolStripSeparator4,
            this.InvertCheckSelectedImagesButton,
            this.RotateSelectedImagesRightButton,
            this.RotateSelectedImagesLeftButton,
            this.RotateSelectedImages180DegreesButton,
            this.FlipSelectedImagesHorizontalButton,
            this.FlipSelectedImagesVerticalButton});
            this.ThumbnailToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ThumbnailToolStrip.Name = "ThumbnailToolStrip";
            this.ThumbnailToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ThumbnailToolStrip.Size = new System.Drawing.Size(55, 477);
            this.ThumbnailToolStrip.TabIndex = 7;
            this.ThumbnailToolStrip.Text = "Thumbnail Tools";
            
            // 
            // ChangeThumbnailSizeButton
            // 
            this.ChangeThumbnailSizeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ChangeThumbnailSizeButton.Image = global::Scanquire.Public.Icons.Thumbnails;
            this.ChangeThumbnailSizeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ChangeThumbnailSizeButton.Name = "ChangeThumbnailSizeButton";
            this.ChangeThumbnailSizeButton.Size = new System.Drawing.Size(52, 54);
            this.ChangeThumbnailSizeButton.Text = "Change Thumbnail Size";
            this.ChangeThumbnailSizeButton.Click += new System.EventHandler(this.ChangeThumbnailSizeButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(52, 6);
            // 
            // InvertCheckSelectedImagesButton
            // 
            this.InvertCheckSelectedImagesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.InvertCheckSelectedImagesButton.Image = global::Scanquire.Public.Icons.CheckUncheckDocument;
            this.InvertCheckSelectedImagesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.InvertCheckSelectedImagesButton.Name = "InvertCheckSelectedImagesButton";
            this.InvertCheckSelectedImagesButton.Size = new System.Drawing.Size(52, 54);
            this.InvertCheckSelectedImagesButton.Text = "Check / UnCheck";
            this.InvertCheckSelectedImagesButton.Click += new System.EventHandler(this.InvertCheckSelectedImagesButton_Click);
            // 
            // RotateSelectedImagesRightButton
            // 
            this.RotateSelectedImagesRightButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RotateSelectedImagesRightButton.Image = global::Scanquire.Public.Icons.RotateRight;
            this.RotateSelectedImagesRightButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RotateSelectedImagesRightButton.Name = "RotateSelectedImagesRightButton";
            this.RotateSelectedImagesRightButton.Size = new System.Drawing.Size(52, 54);
            this.RotateSelectedImagesRightButton.Text = "Rotate Right";
            this.RotateSelectedImagesRightButton.Click += new System.EventHandler(this.RotateSelectedImagesRightButton_Click);
            // 
            // RotateSelectedImagesLeftButton
            // 
            this.RotateSelectedImagesLeftButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RotateSelectedImagesLeftButton.Image = global::Scanquire.Public.Icons.RotateLeft;
            this.RotateSelectedImagesLeftButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RotateSelectedImagesLeftButton.Name = "RotateSelectedImagesLeftButton";
            this.RotateSelectedImagesLeftButton.Size = new System.Drawing.Size(52, 54);
            this.RotateSelectedImagesLeftButton.Text = "Rotate Left";
            this.RotateSelectedImagesLeftButton.Click += new System.EventHandler(this.RotateSelectedImagesLeftButton_Click);
            // 
            // RotateSelectedImages180DegreesButton
            // 
            this.RotateSelectedImages180DegreesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RotateSelectedImages180DegreesButton.Image = global::Scanquire.Public.Icons.Rotate180;
            this.RotateSelectedImages180DegreesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RotateSelectedImages180DegreesButton.Name = "RotateSelectedImages180DegreesButton";
            this.RotateSelectedImages180DegreesButton.Size = new System.Drawing.Size(52, 54);
            this.RotateSelectedImages180DegreesButton.Text = "Rotate 180 Degrees";
            this.RotateSelectedImages180DegreesButton.Click += new System.EventHandler(this.RotateSelectedImages180DegreesButton_Click);
            // 
            // FlipSelectedImagesHorizontalButton
            // 
            this.FlipSelectedImagesHorizontalButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FlipSelectedImagesHorizontalButton.Image = global::Scanquire.Public.Icons.FlipHorizontal;
            this.FlipSelectedImagesHorizontalButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FlipSelectedImagesHorizontalButton.Name = "FlipSelectedImagesHorizontalButton";
            this.FlipSelectedImagesHorizontalButton.Size = new System.Drawing.Size(52, 54);
            this.FlipSelectedImagesHorizontalButton.Text = "Flip Horizontal";
            this.FlipSelectedImagesHorizontalButton.Click += new System.EventHandler(this.FlipSelectedImagesHorizontalButton_Click);
            // 
            // FlipSelectedImagesVerticalButton
            // 
            this.FlipSelectedImagesVerticalButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FlipSelectedImagesVerticalButton.Image = global::Scanquire.Public.Icons.FlipVertical;
            this.FlipSelectedImagesVerticalButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FlipSelectedImagesVerticalButton.Name = "FlipSelectedImagesVerticalButton";
            this.FlipSelectedImagesVerticalButton.Size = new System.Drawing.Size(52, 54);
            this.FlipSelectedImagesVerticalButton.Text = "Flip Vertical";
            this.FlipSelectedImagesVerticalButton.Click += new System.EventHandler(this.FlipSelectedImagesVerticalButton_Click);
            // 
            // ActiveImageBorderPanel
            // 
            this.ActiveImageBorderPanel.BackColor = System.Drawing.SystemColors.Control;
            this.ActiveImageBorderPanel.Controls.Add(this.ActiveImageViewer);
            this.ActiveImageBorderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActiveImageBorderPanel.Location = new System.Drawing.Point(0, 0);
            this.ActiveImageBorderPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ActiveImageBorderPanel.Name = "ActiveImageBorderPanel";
            this.ActiveImageBorderPanel.Padding = new System.Windows.Forms.Padding(5);
            this.ActiveImageBorderPanel.Size = new System.Drawing.Size(345, 477);
            this.ActiveImageBorderPanel.TabIndex = 3;
            // 
            // ActiveImageViewer
            // 
            this.ActiveImageViewer.AutoScroll = true;
            this.ActiveImageViewer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ActiveImageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActiveImageViewer.Image = null;
            this.ActiveImageViewer.Location = new System.Drawing.Point(5, 5);
            this.ActiveImageViewer.Name = "ActiveImageViewer";
            this.ActiveImageViewer.ScalingMode = EdocsUSA.Controls.ImageViewer.ImageScalingMode.Fit;
            this.ActiveImageViewer.SelectAnchor = new System.Drawing.Point(0, 0);
            this.ActiveImageViewer.SelectEnd = new System.Drawing.Point(0, 0);
            this.ActiveImageViewer.Size = new System.Drawing.Size(335, 467);
            this.ActiveImageViewer.TabIndex = 0;
            this.ActiveImageViewer.Text = "Active Image Viewer";
            this.ActiveImageViewer.ToolMode = EdocsUSA.Controls.ImageViewer.MouseToolMode.Pan;
            this.ActiveImageViewer.ZoomLevel = 1F;
            this.ActiveImageViewer.ZoomMultiplier = 0.15F;
            
            // 
            // ActiveImageToolStripTabControl
            // 
            this.ActiveImageToolStripTabControl.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.ActiveImageToolStripTabControl.Controls.Add(this.ViewToolStripTabPage);
            this.ActiveImageToolStripTabControl.Controls.Add(this.EditToolStripTabPage);
            this.ActiveImageToolStripTabControl.Dock = System.Windows.Forms.DockStyle.Right;
            this.ActiveImageToolStripTabControl.Location = new System.Drawing.Point(345, 0);
            this.ActiveImageToolStripTabControl.Multiline = true;
            this.ActiveImageToolStripTabControl.Name = "ActiveImageToolStripTabControl";
            this.ActiveImageToolStripTabControl.SelectedIndex = 0;
            this.ActiveImageToolStripTabControl.Size = new System.Drawing.Size(101, 477);
            this.ActiveImageToolStripTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.ActiveImageToolStripTabControl.TabIndex = 2;
            this.ActiveImageToolStripTabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.ActiveImageToolStripTabControl_Selected);
            // 
            // ViewToolStripTabPage
            // 
            this.ViewToolStripTabPage.Controls.Add(this.ViewActiveImageToolStrip);
            this.ViewToolStripTabPage.Location = new System.Drawing.Point(4, 4);
            this.ViewToolStripTabPage.Name = "ViewToolStripTabPage";
            this.ViewToolStripTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ViewToolStripTabPage.Size = new System.Drawing.Size(74, 469);
            this.ViewToolStripTabPage.TabIndex = 0;
            this.ViewToolStripTabPage.Text = "View";
            this.ViewToolStripTabPage.UseVisualStyleBackColor = true;
            
            // 
            // ViewActiveImageToolStrip
            // 
            this.ViewActiveImageToolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ViewActiveImageToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ViewActiveImageToolStrip.ImageScalingSize = new System.Drawing.Size(50, 50);
            this.ViewActiveImageToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ZoomInButton,
            this.ZoomOutButton,
            this.toolStripSeparator1,
            this.ScaleToFitViewerButton,
            this.ScaleToOriginalSizeButton,
            this.ScaleFitViewerWidthButton,
            this.ScaleToFitViewerHeightButton,
            this.toolStripSeparator2,
            this.DisplayImageInfoButton});
            this.ViewActiveImageToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.ViewActiveImageToolStrip.Location = new System.Drawing.Point(3, 3);
            this.ViewActiveImageToolStrip.Name = "ViewActiveImageToolStrip";
            this.ViewActiveImageToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ViewActiveImageToolStrip.Size = new System.Drawing.Size(68, 463);
            this.ViewActiveImageToolStrip.Stretch = true;
            this.ViewActiveImageToolStrip.TabIndex = 0;
            this.ViewActiveImageToolStrip.Text = "View Tool Strip";
            // 
            // ZoomInButton
            // 
            this.ZoomInButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ZoomInButton.Image = global::Scanquire.Public.Icons.ZoomIn;
            this.ZoomInButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ZoomInButton.Name = "ZoomInButton";
            this.ZoomInButton.Size = new System.Drawing.Size(66, 54);
            this.ZoomInButton.Text = "Zoom In";
            this.ZoomInButton.Click += new System.EventHandler(this.ZoomInButton_Click);
            // 
            // ZoomOutButton
            // 
            this.ZoomOutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ZoomOutButton.Image = global::Scanquire.Public.Icons.ZoomOut;
            this.ZoomOutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ZoomOutButton.Name = "ZoomOutButton";
            this.ZoomOutButton.Size = new System.Drawing.Size(66, 54);
            this.ZoomOutButton.Text = "Zoom Out";
            this.ZoomOutButton.Click += new System.EventHandler(this.ZoomOutButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(66, 6);
            // 
            // ScaleToFitViewerButton
            // 
            this.ScaleToFitViewerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ScaleToFitViewerButton.Image = global::Scanquire.Public.Icons.ScaleToViewer;
            this.ScaleToFitViewerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ScaleToFitViewerButton.Name = "ScaleToFitViewerButton";
            this.ScaleToFitViewerButton.Size = new System.Drawing.Size(66, 54);
            this.ScaleToFitViewerButton.Text = "Fit To Viewer";
            this.ScaleToFitViewerButton.Click += new System.EventHandler(this.ScaleToFitViewerButton_Click);
            // 
            // ScaleToOriginalSizeButton
            // 
            this.ScaleToOriginalSizeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ScaleToOriginalSizeButton.Image = global::Scanquire.Public.Icons.ScaleToOriginalSize;
            this.ScaleToOriginalSizeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ScaleToOriginalSizeButton.Name = "ScaleToOriginalSizeButton";
            this.ScaleToOriginalSizeButton.Size = new System.Drawing.Size(66, 54);
            this.ScaleToOriginalSizeButton.Text = "Original Size";
            this.ScaleToOriginalSizeButton.Click += new System.EventHandler(this.ScaleToOriginalSizeButton_Click);
            // 
            // ScaleFitViewerWidthButton
            // 
            this.ScaleFitViewerWidthButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ScaleFitViewerWidthButton.Image = global::Scanquire.Public.Icons.ScaleToViewerWidth;
            this.ScaleFitViewerWidthButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ScaleFitViewerWidthButton.Name = "ScaleFitViewerWidthButton";
            this.ScaleFitViewerWidthButton.Size = new System.Drawing.Size(66, 54);
            this.ScaleFitViewerWidthButton.Text = "Scale To Fit Viewer Width";
            this.ScaleFitViewerWidthButton.Click += new System.EventHandler(this.ScaleFitViewerWidthButton_Click);
            // 
            // ScaleToFitViewerHeightButton
            // 
            this.ScaleToFitViewerHeightButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ScaleToFitViewerHeightButton.Image = global::Scanquire.Public.Icons.ScaleToViewerHeight;
            this.ScaleToFitViewerHeightButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ScaleToFitViewerHeightButton.Name = "ScaleToFitViewerHeightButton";
            this.ScaleToFitViewerHeightButton.Size = new System.Drawing.Size(66, 54);
            this.ScaleToFitViewerHeightButton.Text = "Scale To Fit Viewer Height";
            this.ScaleToFitViewerHeightButton.Click += new System.EventHandler(this.ScaleToFitViewerHeightButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(66, 6);
            // 
            // DisplayImageInfoButton
            // 
            this.DisplayImageInfoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DisplayImageInfoButton.Image = global::Scanquire.Public.Icons.Information;
            this.DisplayImageInfoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DisplayImageInfoButton.Name = "DisplayImageInfoButton";
            this.DisplayImageInfoButton.Size = new System.Drawing.Size(66, 54);
            this.DisplayImageInfoButton.Text = "Display Image Information";
            this.DisplayImageInfoButton.Click += new System.EventHandler(this.DisplayImageInfoButton_Click);
            // 
            // EditToolStripTabPage
            // 
            this.EditToolStripTabPage.Controls.Add(this.EditActiveImageToolStrip);
            this.EditToolStripTabPage.Location = new System.Drawing.Point(4, 4);
            this.EditToolStripTabPage.Name = "EditToolStripTabPage";
            this.EditToolStripTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.EditToolStripTabPage.Size = new System.Drawing.Size(74, 469);
            this.EditToolStripTabPage.TabIndex = 1;
            this.EditToolStripTabPage.Text = "Edit";
            this.EditToolStripTabPage.UseVisualStyleBackColor = true;
            // 
            // EditActiveImageToolStrip
            // 
            this.EditActiveImageToolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditActiveImageToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.EditActiveImageToolStrip.ImageScalingSize = new System.Drawing.Size(50, 50);
            this.EditActiveImageToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CropSelectedRegionButton,
            this.FillSelectedRegionButton,
            this.SelectFillColorButton,
            this.DeskewLeftButton,
            this.DeskewRightButton,
            this.toolStripSeparator3,
            this.UndoRevisionButton});
            this.EditActiveImageToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.EditActiveImageToolStrip.Location = new System.Drawing.Point(3, 3);
            this.EditActiveImageToolStrip.Name = "EditActiveImageToolStrip";
            this.EditActiveImageToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.EditActiveImageToolStrip.Size = new System.Drawing.Size(68, 463);
            this.EditActiveImageToolStrip.Stretch = true;
            this.EditActiveImageToolStrip.TabIndex = 1;
            this.EditActiveImageToolStrip.Text = "View Tool Strip";
            // 
            // CropSelectedRegionButton
            // 
            this.CropSelectedRegionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CropSelectedRegionButton.Image = global::Scanquire.Public.Icons.Crop;
            this.CropSelectedRegionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CropSelectedRegionButton.Name = "CropSelectedRegionButton";
            this.CropSelectedRegionButton.Size = new System.Drawing.Size(66, 54);
            this.CropSelectedRegionButton.Text = "Crop Selected Region";
            this.CropSelectedRegionButton.Click += new System.EventHandler(this.CropSelectedRegionButton_Click);
            // 
            // FillSelectedRegionButton
            // 
            this.FillSelectedRegionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FillSelectedRegionButton.Image = global::Scanquire.Public.Icons.Fill;
            this.FillSelectedRegionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FillSelectedRegionButton.Name = "FillSelectedRegionButton";
            this.FillSelectedRegionButton.Size = new System.Drawing.Size(66, 54);
            this.FillSelectedRegionButton.Text = "Fill Selected Region";
            this.FillSelectedRegionButton.Click += new System.EventHandler(this.FillSelectedRegionButton_Click);
            // 
            // SelectFillColorButton
            // 
            this.SelectFillColorButton.BackColor = System.Drawing.Color.White;
            this.SelectFillColorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SelectFillColorButton.Image = global::Scanquire.Public.Icons.ColorWheel;
            this.SelectFillColorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SelectFillColorButton.Name = "SelectFillColorButton";
            this.SelectFillColorButton.Size = new System.Drawing.Size(66, 54);
            this.SelectFillColorButton.Text = "Select Fill Color";
            this.SelectFillColorButton.Click += new System.EventHandler(this.SelectFillColorButton_Click);
            // 
            // DeskewLeftButton
            // 
            this.DeskewLeftButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeskewLeftButton.Image = global::Scanquire.Public.Icons.DeskewLeft;
            this.DeskewLeftButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeskewLeftButton.Name = "DeskewLeftButton";
            this.DeskewLeftButton.Size = new System.Drawing.Size(66, 54);
            this.DeskewLeftButton.Text = "Deskew Left";
            this.DeskewLeftButton.Click += new System.EventHandler(this.DeskewLeftButton_Click);
            // 
            // DeskewRightButton
            // 
            this.DeskewRightButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeskewRightButton.Image = global::Scanquire.Public.Icons.DeskewRight;
            this.DeskewRightButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeskewRightButton.Name = "DeskewRightButton";
            this.DeskewRightButton.Size = new System.Drawing.Size(66, 54);
            this.DeskewRightButton.Text = "Deskew Right";
            this.DeskewRightButton.Click += new System.EventHandler(this.DeskewRightButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(66, 6);
            // 
            // UndoRevisionButton
            // 
            this.UndoRevisionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.UndoRevisionButton.Enabled = false;
            this.UndoRevisionButton.Image = global::Scanquire.Public.Icons.Undo;
            this.UndoRevisionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.UndoRevisionButton.Name = "UndoRevisionButton";
            this.UndoRevisionButton.Size = new System.Drawing.Size(66, 54);
            this.UndoRevisionButton.Text = "Undo Last Revision";
            this.UndoRevisionButton.Click += new System.EventHandler(this.UndoRevisionButton_Click);
            // 
            // SQImageListViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SplitContainer);
            this.Name = "SQImageListViewer";
            this.Size = new System.Drawing.Size(674, 477);
            
            this.SplitContainer.Panel1.ResumeLayout(false);
            this.SplitContainer.Panel1.PerformLayout();
            this.SplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).EndInit();
            this.SplitContainer.ResumeLayout(false);
            this.ThumbnailToolStrip.ResumeLayout(false);
            this.ThumbnailToolStrip.PerformLayout();
            this.ActiveImageBorderPanel.ResumeLayout(false);
            this.ActiveImageToolStripTabControl.ResumeLayout(false);
            this.ViewToolStripTabPage.ResumeLayout(false);
            this.ViewToolStripTabPage.PerformLayout();
            this.ViewActiveImageToolStrip.ResumeLayout(false);
            this.ViewActiveImageToolStrip.PerformLayout();
            this.EditToolStripTabPage.ResumeLayout(false);
            this.EditToolStripTabPage.PerformLayout();
            this.EditActiveImageToolStrip.ResumeLayout(false);
            this.EditActiveImageToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SQThumbnailScrollBar ThumbnailScrollBar;
        private SQThumbnailPanel ThumbnailPanel;
        private System.Windows.Forms.ToolStripButton ChangeThumbnailSizeButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton InvertCheckSelectedImagesButton;
        private System.Windows.Forms.ToolStripButton RotateSelectedImagesRightButton;
        private System.Windows.Forms.ToolStripButton RotateSelectedImagesLeftButton;
        private System.Windows.Forms.ToolStripButton RotateSelectedImages180DegreesButton;
        private System.Windows.Forms.ToolStripButton FlipSelectedImagesHorizontalButton;
        private System.Windows.Forms.ToolStripButton FlipSelectedImagesVerticalButton;
        private System.Windows.Forms.TabControl ActiveImageToolStripTabControl;
        private System.Windows.Forms.TabPage ViewToolStripTabPage;
        private SQToolStrip ViewActiveImageToolStrip;
        private System.Windows.Forms.ToolStripButton ZoomInButton;
        private System.Windows.Forms.ToolStripButton ZoomOutButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ScaleToFitViewerButton;
        private System.Windows.Forms.ToolStripButton ScaleToOriginalSizeButton;
        private System.Windows.Forms.ToolStripButton ScaleFitViewerWidthButton;
        private System.Windows.Forms.ToolStripButton ScaleToFitViewerHeightButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton DisplayImageInfoButton;
        private System.Windows.Forms.TabPage EditToolStripTabPage;
        private SQToolStrip EditActiveImageToolStrip;
        private System.Windows.Forms.ToolStripButton CropSelectedRegionButton;
        private System.Windows.Forms.ToolStripButton FillSelectedRegionButton;
        private System.Windows.Forms.ToolStripButton SelectFillColorButton;
        private System.Windows.Forms.ToolStripButton DeskewLeftButton;
        private System.Windows.Forms.ToolStripButton DeskewRightButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton UndoRevisionButton;
        private System.Windows.Forms.Panel ActiveImageBorderPanel;
        private System.Windows.Forms.ColorDialog SelectFillColorDialog;
        private ProgressMonitor ProgressMonitor;
        public SQSplitContainer SplitContainer;
        public EdocsUSA.Controls.ImageViewer ActiveImageViewer;
        public SQToolStrip ThumbnailToolStrip;
    }
}
