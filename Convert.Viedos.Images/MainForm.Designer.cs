
namespace Edocs.Convert.Viedos.Images
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.imgViedoImages = new System.Windows.Forms.ImageList(this.components);
            this.msDelete = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmConvertVedio = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmpng = new System.Windows.Forms.ToolStripMenuItem();
            this.bmpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jpgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmSaveImages = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSaveClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmIimportImages = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmcreate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmPlayViedo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmDeleteImage = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.labInfo = new System.Windows.Forms.Label();
            this.lViewVImgs = new System.Windows.Forms.ListView();
            this.openFDialog = new System.Windows.Forms.OpenFileDialog();
            this.sFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.imgView = new Edocs.Convert.Viedos.Images.Controls.ImageViewer();
            this.pBar = new System.Windows.Forms.ProgressBar();
            this.msDelete.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.imgView.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgViedoImages
            // 
            this.imgViedoImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imgViedoImages.ImageSize = new System.Drawing.Size(200, 16);
            this.imgViedoImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // msDelete
            // 
            this.msDelete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.tsmDeleteImage,
            this.exitToolStripMenuItem});
            this.msDelete.Location = new System.Drawing.Point(0, 0);
            this.msDelete.Name = "msDelete";
            this.msDelete.Size = new System.Drawing.Size(800, 24);
            this.msDelete.TabIndex = 0;
            this.msDelete.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmConvertVedio,
            this.toolStripSeparator2,
            this.tsmSaveImages,
            this.toolStripSeparator1,
            this.tsmIimportImages,
            this.tsmcreate,
            this.tsmPlayViedo});
            this.fileToolStripMenuItem.Image = global::Edocs.Convert.Viedos.Images.Properties.Resources.folderimg;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // tsmConvertVedio
            // 
            this.tsmConvertVedio.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmpng,
            this.bmpToolStripMenuItem,
            this.jpgToolStripMenuItem});
            this.tsmConvertVedio.Image = global::Edocs.Convert.Viedos.Images.Properties.Resources.folder;
            this.tsmConvertVedio.Name = "tsmConvertVedio";
            this.tsmConvertVedio.Size = new System.Drawing.Size(243, 22);
            this.tsmConvertVedio.Text = "&Video  to Convert to Image Type";
            // 
            // tsmpng
            // 
            this.tsmpng.Image = global::Edocs.Convert.Viedos.Images.Properties.Resources.file;
            this.tsmpng.Name = "tsmpng";
            this.tsmpng.Size = new System.Drawing.Size(102, 22);
            this.tsmpng.Text = ".Png";
            this.tsmpng.Click += new System.EventHandler(this.pngToolStripMenuItem_Click);
            // 
            // bmpToolStripMenuItem
            // 
            this.bmpToolStripMenuItem.Image = global::Edocs.Convert.Viedos.Images.Properties.Resources.bmp;
            this.bmpToolStripMenuItem.Name = "bmpToolStripMenuItem";
            this.bmpToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.bmpToolStripMenuItem.Text = ".Bmp";
            this.bmpToolStripMenuItem.Click += new System.EventHandler(this.bmpToolStripMenuItem_Click);
            // 
            // jpgToolStripMenuItem
            // 
            this.jpgToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.jpgToolStripMenuItem.Image = global::Edocs.Convert.Viedos.Images.Properties.Resources.jpg;
            this.jpgToolStripMenuItem.Name = "jpgToolStripMenuItem";
            this.jpgToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.jpgToolStripMenuItem.Text = ".Jpg";
            this.jpgToolStripMenuItem.Click += new System.EventHandler(this.jpgToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(240, 6);
            // 
            // tsmSaveImages
            // 
            this.tsmSaveImages.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.tsmSaveClose});
            this.tsmSaveImages.Image = global::Edocs.Convert.Viedos.Images.Properties.Resources.save;
            this.tsmSaveImages.Name = "tsmSaveImages";
            this.tsmSaveImages.Size = new System.Drawing.Size(243, 22);
            this.tsmSaveImages.Text = "&Save Images";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Image = global::Edocs.Convert.Viedos.Images.Properties.Resources.review;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItem1.Text = "Save and &Review";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // tsmSaveClose
            // 
            this.tsmSaveClose.Image = global::Edocs.Convert.Viedos.Images.Properties.Resources.close;
            this.tsmSaveClose.Name = "tsmSaveClose";
            this.tsmSaveClose.Size = new System.Drawing.Size(161, 22);
            this.tsmSaveClose.Text = "Save and &Close";
            this.tsmSaveClose.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(240, 6);
            // 
            // tsmIimportImages
            // 
            this.tsmIimportImages.Image = global::Edocs.Convert.Viedos.Images.Properties.Resources.import;
            this.tsmIimportImages.Name = "tsmIimportImages";
            this.tsmIimportImages.Size = new System.Drawing.Size(243, 22);
            this.tsmIimportImages.Text = "Import Images";
            this.tsmIimportImages.Click += new System.EventHandler(this.importImagesToolStripMenuItem_Click);
            // 
            // tsmcreate
            // 
            this.tsmcreate.Image = global::Edocs.Convert.Viedos.Images.Properties.Resources.creative;
            this.tsmcreate.Name = "tsmcreate";
            this.tsmcreate.Size = new System.Drawing.Size(243, 22);
            this.tsmcreate.Text = "Create Viedo";
            this.tsmcreate.Click += new System.EventHandler(this.createViedoToolStripMenuItem_Click);
            // 
            // tsmPlayViedo
            // 
            this.tsmPlayViedo.Image = global::Edocs.Convert.Viedos.Images.Properties.Resources.video_player;
            this.tsmPlayViedo.Name = "tsmPlayViedo";
            this.tsmPlayViedo.Size = new System.Drawing.Size(243, 22);
            this.tsmPlayViedo.Text = "Play Viedo";
            this.tsmPlayViedo.Click += new System.EventHandler(this.tsmPlayViedo_Click);
            // 
            // tsmDeleteImage
            // 
            this.tsmDeleteImage.Image = global::Edocs.Convert.Viedos.Images.Properties.Resources.delete;
            this.tsmDeleteImage.Name = "tsmDeleteImage";
            this.tsmDeleteImage.Size = new System.Drawing.Size(104, 20);
            this.tsmDeleteImage.Text = "&Delete Image";
            this.tsmDeleteImage.Click += new System.EventHandler(this.deleteImageToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::Edocs.Convert.Viedos.Images.Properties.Resources.exit128;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.labInfo);
            this.splitContainer1.Panel1.Controls.Add(this.lViewVImgs);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.imgView);
            this.splitContainer1.Size = new System.Drawing.Size(800, 426);
            this.splitContainer1.SplitterDistance = 110;
            this.splitContainer1.TabIndex = 2;
            // 
            // labInfo
            // 
            this.labInfo.AutoSize = true;
            this.labInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labInfo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.labInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labInfo.Location = new System.Drawing.Point(0, 408);
            this.labInfo.Name = "labInfo";
            this.labInfo.Size = new System.Drawing.Size(2, 18);
            this.labInfo.TabIndex = 2;
            // 
            // lViewVImgs
            // 
            this.lViewVImgs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lViewVImgs.HideSelection = false;
            this.lViewVImgs.Location = new System.Drawing.Point(0, 0);
            this.lViewVImgs.Name = "lViewVImgs";
            this.lViewVImgs.Size = new System.Drawing.Size(110, 426);
            this.lViewVImgs.TabIndex = 1;
            this.lViewVImgs.UseCompatibleStateImageBehavior = false;
            this.lViewVImgs.SelectedIndexChanged += new System.EventHandler(this.lViewVImgs_SelectedIndexChanged_1);
            // 
            // openFDialog
            // 
            this.openFDialog.FileName = "openFileDialog1";
            // 
            // imgView
            // 
            this.imgView.AutoScroll = true;
            this.imgView.Controls.Add(this.pBar);
            this.imgView.Cursor = System.Windows.Forms.Cursors.Hand;
            this.imgView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgView.Image = null;
            this.imgView.Location = new System.Drawing.Point(0, 0);
            this.imgView.Name = "imgView";
            this.imgView.ScalingMode = Edocs.Convert.Viedos.Images.Controls.ImageViewer.ImageScalingMode.Fit;
            this.imgView.SelectAnchor = new System.Drawing.Point(0, 0);
            this.imgView.SelectEnd = new System.Drawing.Point(0, 0);
            this.imgView.Size = new System.Drawing.Size(686, 426);
            this.imgView.TabIndex = 0;
            this.imgView.Text = "imageViewer1";
            this.imgView.ToolMode = Edocs.Convert.Viedos.Images.Controls.ImageViewer.MouseToolMode.Pan;
            this.imgView.ZoomLevel = 1F;
            this.imgView.ZoomMultiplier = 0.15F;
            // 
            // pBar
            // 
            this.pBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pBar.Location = new System.Drawing.Point(0, 404);
            this.pBar.Name = "pBar";
            this.pBar.Size = new System.Drawing.Size(686, 22);
            this.pBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pBar.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.msDelete);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Convert Veido";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.msDelete.ResumeLayout(false);
            this.msDelete.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.imgView.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageList imgViedoImages;
        private System.Windows.Forms.MenuStrip msDelete;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lViewVImgs;
        private Controls.ImageViewer imgView;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmConvertVedio;
        private System.Windows.Forms.OpenFileDialog openFDialog;
        private System.Windows.Forms.ToolStripMenuItem tsmpng;
        private System.Windows.Forms.ToolStripMenuItem bmpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jpgToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmSaveImages;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmSaveClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmDeleteImage;
        private System.Windows.Forms.ToolStripMenuItem tsmIimportImages;
        private System.Windows.Forms.SaveFileDialog sFileDialog;
        private System.Windows.Forms.ToolStripMenuItem tsmcreate;
        private System.Windows.Forms.ToolStripMenuItem tsmPlayViedo;
        private System.Windows.Forms.ProgressBar pBar;
        private System.Windows.Forms.Label labInfo;
    }
}

