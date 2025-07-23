
namespace Scanquire.Public.EDocs_Tools
{
    partial class ImageToolEdocs
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveKeepOrginallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveKeepOrginallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripComboBox();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blueFIlterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.greenFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gammaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brightnessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contrastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graycaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemReSize = new System.Windows.Forms.ToolStripMenuItem();
            this.rotateFlipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cropToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.textToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shapeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.exitToolStripMenuItem,
            this.colorToolStripMenuItem,
            this.imageToolStripMenuItem,
            this.insetToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(800, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.saveKeepOrginallToolStripMenuItem,
            this.saveKeepOrginallyToolStripMenuItem});
            this.fileToolStripMenuItem.Image = global::Scanquire.Public.Icons.save2;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Image = global::Scanquire.Public.Icons.Undo;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.undoToolStripMenuItem.Text = "&Undo ";
            // 
            // saveKeepOrginallToolStripMenuItem
            // 
            this.saveKeepOrginallToolStripMenuItem.Image = global::Scanquire.Public.Icons.SaveFile;
            this.saveKeepOrginallToolStripMenuItem.Name = "saveKeepOrginallToolStripMenuItem";
            this.saveKeepOrginallToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveKeepOrginallToolStripMenuItem.Text = "Save &Copy";
            // 
            // saveKeepOrginallyToolStripMenuItem
            // 
            this.saveKeepOrginallyToolStripMenuItem.Image = global::Scanquire.Public.Icons.SaveFile;
            this.saveKeepOrginallyToolStripMenuItem.Name = "saveKeepOrginallyToolStripMenuItem";
            this.saveKeepOrginallyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveKeepOrginallyToolStripMenuItem.Text = "&Save";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imageInfoToolStripMenuItem,
            this.zoomToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // imageInfoToolStripMenuItem
            // 
            this.imageInfoToolStripMenuItem.Name = "imageInfoToolStripMenuItem";
            this.imageInfoToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.imageInfoToolStripMenuItem.Text = "&Image Info";
            // 
            // zoomToolStripMenuItem
            // 
            this.zoomToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            this.zoomToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.zoomToolStripMenuItem.Text = "&Zoom";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Items.AddRange(new object[] {
            "50%",
            "75%",
            "100%",
            "150%",
            "200%",
            "300%",
            "400%",
            "500%"});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(180, 23);
            this.toolStripMenuItem2.Text = "Zoom Percent";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::Scanquire.Public.Icons.exit64;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.exitToolStripMenuItem.Text = "&Exit";
            // 
            // colorToolStripMenuItem
            // 
            this.colorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filterToolStripMenuItem,
            this.gammaToolStripMenuItem,
            this.brightnessToolStripMenuItem,
            this.contrastToolStripMenuItem,
            this.graycaleToolStripMenuItem,
            this.invertToolStripMenuItem});
            this.colorToolStripMenuItem.Name = "colorToolStripMenuItem";
            this.colorToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.colorToolStripMenuItem.Text = "&Color";
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.redFilterToolStripMenuItem,
            this.blueFIlterToolStripMenuItem,
            this.greenFilterToolStripMenuItem});
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            this.filterToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.filterToolStripMenuItem.Text = "&Filter";
            // 
            // redFilterToolStripMenuItem
            // 
            this.redFilterToolStripMenuItem.Name = "redFilterToolStripMenuItem";
            this.redFilterToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.redFilterToolStripMenuItem.Text = "&Red Filter";
            // 
            // blueFIlterToolStripMenuItem
            // 
            this.blueFIlterToolStripMenuItem.Name = "blueFIlterToolStripMenuItem";
            this.blueFIlterToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.blueFIlterToolStripMenuItem.Text = "&Blue FIlter";
            // 
            // greenFilterToolStripMenuItem
            // 
            this.greenFilterToolStripMenuItem.Name = "greenFilterToolStripMenuItem";
            this.greenFilterToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.greenFilterToolStripMenuItem.Text = "&Green Filter";
            // 
            // gammaToolStripMenuItem
            // 
            this.gammaToolStripMenuItem.Name = "gammaToolStripMenuItem";
            this.gammaToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.gammaToolStripMenuItem.Text = "&Gamma";
            // 
            // brightnessToolStripMenuItem
            // 
            this.brightnessToolStripMenuItem.Name = "brightnessToolStripMenuItem";
            this.brightnessToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.brightnessToolStripMenuItem.Text = "&Brightness";
            // 
            // contrastToolStripMenuItem
            // 
            this.contrastToolStripMenuItem.Name = "contrastToolStripMenuItem";
            this.contrastToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.contrastToolStripMenuItem.Text = "&Contrast";
            // 
            // graycaleToolStripMenuItem
            // 
            this.graycaleToolStripMenuItem.Name = "graycaleToolStripMenuItem";
            this.graycaleToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.graycaleToolStripMenuItem.Text = "&Graycale";
            // 
            // invertToolStripMenuItem
            // 
            this.invertToolStripMenuItem.Name = "invertToolStripMenuItem";
            this.invertToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.invertToolStripMenuItem.Text = "&Invert";
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemReSize,
            this.rotateFlipToolStripMenuItem,
            this.cropToolStripMenuItem});
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.imageToolStripMenuItem.Text = "&Image";
            // 
            // menuItemReSize
            // 
            this.menuItemReSize.Name = "menuItemReSize";
            this.menuItemReSize.Size = new System.Drawing.Size(180, 22);
            this.menuItemReSize.Text = "&Resize";
            this.menuItemReSize.Click += new System.EventHandler(this.menuItemReSize_Click);
            // 
            // rotateFlipToolStripMenuItem
            // 
            this.rotateFlipToolStripMenuItem.Name = "rotateFlipToolStripMenuItem";
            this.rotateFlipToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.rotateFlipToolStripMenuItem.Text = "Rotate & &Flip";
            // 
            // cropToolStripMenuItem
            // 
            this.cropToolStripMenuItem.Name = "cropToolStripMenuItem";
            this.cropToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.cropToolStripMenuItem.Text = "&Crop";
            // 
            // insetToolStripMenuItem
            // 
            this.insetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imageToolStripMenuItem1,
            this.textToolStripMenuItem,
            this.shapeToolStripMenuItem});
            this.insetToolStripMenuItem.Name = "insetToolStripMenuItem";
            this.insetToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.insetToolStripMenuItem.Text = "&Inset";
            // 
            // imageToolStripMenuItem1
            // 
            this.imageToolStripMenuItem1.Name = "imageToolStripMenuItem1";
            this.imageToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.imageToolStripMenuItem1.Text = "&Image";
            // 
            // textToolStripMenuItem
            // 
            this.textToolStripMenuItem.Name = "textToolStripMenuItem";
            this.textToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.textToolStripMenuItem.Text = "&Text";
            // 
            // shapeToolStripMenuItem
            // 
            this.shapeToolStripMenuItem.Name = "shapeToolStripMenuItem";
            this.shapeToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.shapeToolStripMenuItem.Text = "&Shape";
            // 
            // ImageToolEdocs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "ImageToolEdocs";
            this.Text = "Edocs";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ImageToolEdocs_Paint);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveKeepOrginallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveKeepOrginallyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blueFIlterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem greenFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gammaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem brightnessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contrastToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graycaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemReSize;
        private System.Windows.Forms.ToolStripMenuItem rotateFlipToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cropToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem textToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shapeToolStripMenuItem;

       
    }
}