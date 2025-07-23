namespace ManualIndexingTemp
{
    partial class frmNavigateAll
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNavigateAll));
            this.btnNavigatePrevious = new System.Windows.Forms.Button();
            this.btnNavigateNext = new System.Windows.Forms.Button();
            this.btnNavigateReset = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sqImageListViewer1 = new Scanquire.Public.UserControls.SQImageListViewer();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNavigatePrevious
            // 
            this.btnNavigatePrevious.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnNavigatePrevious.Location = new System.Drawing.Point(0, 0);
            this.btnNavigatePrevious.Name = "btnNavigatePrevious";
            this.btnNavigatePrevious.Size = new System.Drawing.Size(75, 100);
            this.btnNavigatePrevious.TabIndex = 0;
            this.btnNavigatePrevious.Text = "<<";
            this.btnNavigatePrevious.UseVisualStyleBackColor = true;
            
            // 
            // btnNavigateNext
            // 
            this.btnNavigateNext.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnNavigateNext.Location = new System.Drawing.Point(676, 0);
            this.btnNavigateNext.Name = "btnNavigateNext";
            this.btnNavigateNext.Size = new System.Drawing.Size(75, 100);
            this.btnNavigateNext.TabIndex = 1;
            this.btnNavigateNext.Text = ">>";
            this.btnNavigateNext.UseVisualStyleBackColor = true;
            
            // 
            // btnNavigateReset
            // 
            this.btnNavigateReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNavigateReset.Location = new System.Drawing.Point(75, 0);
            this.btnNavigateReset.Name = "btnNavigateReset";
            this.btnNavigateReset.Size = new System.Drawing.Size(601, 100);
            this.btnNavigateReset.TabIndex = 2;
            this.btnNavigateReset.Text = "Reset";
            this.btnNavigateReset.UseVisualStyleBackColor = true;
            
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnNavigateReset);
            this.panel1.Controls.Add(this.btnNavigateNext);
            this.panel1.Controls.Add(this.btnNavigatePrevious);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(751, 100);
            this.panel1.TabIndex = 3;
            // 
            // sqImageListViewer1
            // 
            this.sqImageListViewer1.ActiveItem = null;
            this.sqImageListViewer1.CurrentThumbnailSizeMode = Scanquire.Public.UserControls.SQImageListViewer.ThumbnailSizeMode.Small;
            this.sqImageListViewer1.DeskewAngle = 1F;
            this.sqImageListViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sqImageListViewer1.FillColor = System.Drawing.Color.White;
            this.sqImageListViewer1.LargeThumbnailSize = new System.Drawing.Size(255, 330);
            this.sqImageListViewer1.Location = new System.Drawing.Point(0, 100);
            this.sqImageListViewer1.MediumThumbnailSize = new System.Drawing.Size(170, 220);
            this.sqImageListViewer1.Name = "sqImageListViewer1";
            this.sqImageListViewer1.Saved = true;
            this.sqImageListViewer1.Size = new System.Drawing.Size(751, 450);
            this.sqImageListViewer1.SmallThumbnailSize = new System.Drawing.Size(85, 110);
            this.sqImageListViewer1.TabIndex = 5;
            this.sqImageListViewer1.TabStop = false;
            this.sqImageListViewer1.ViewMode = Scanquire.Public.UserControls.SQImageListViewer.ImageThumbnailViewMode.ThumbnailsAndImage;
            // 
            // frmNavigateAll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 550);
            this.Controls.Add(this.sqImageListViewer1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmNavigateAll";
            this.Text = "Navigate All";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNavigatePrevious;
        private System.Windows.Forms.Button btnNavigateNext;
        private System.Windows.Forms.Button btnNavigateReset;
        private System.Windows.Forms.Panel panel1;
        private Scanquire.Public.UserControls.SQImageListViewer sqImageListViewer1;
    }
}