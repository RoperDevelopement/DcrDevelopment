namespace BinMonitor.Common
{
    partial class MasterCategoryStatusViewer
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
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.splitBottom = new System.Windows.Forms.SplitContainer();
            this.lblCaption = new System.Windows.Forms.Label();
            this.binStatusViewer1 = new BinMonitor.Common.BinStatusViewer();
            this.binStatusViewer2 = new BinMonitor.Common.BinStatusViewer();
            this.binStatusViewer3 = new BinMonitor.Common.BinStatusViewer();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitBottom)).BeginInit();
            this.splitBottom.Panel1.SuspendLayout();
            this.splitBottom.Panel2.SuspendLayout();
            this.splitBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Enabled = false;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.binStatusViewer1);
            this.splitMain.Panel1.Controls.Add(this.lblCaption);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.splitBottom);
            this.splitMain.Size = new System.Drawing.Size(300, 300);
            this.splitMain.SplitterDistance = 200;
            this.splitMain.SplitterWidth = 1;
            this.splitMain.TabIndex = 0;
            this.splitMain.TabStop = false;
            // 
            // splitBottom
            // 
            this.splitBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitBottom.Enabled = false;
            this.splitBottom.Location = new System.Drawing.Point(0, 0);
            this.splitBottom.Name = "splitBottom";
            // 
            // splitBottom.Panel1
            // 
            this.splitBottom.Panel1.Controls.Add(this.binStatusViewer2);
            // 
            // splitBottom.Panel2
            // 
            this.splitBottom.Panel2.Controls.Add(this.binStatusViewer3);
            this.splitBottom.Size = new System.Drawing.Size(300, 99);
            this.splitBottom.SplitterDistance = 150;
            this.splitBottom.SplitterWidth = 1;
            this.splitBottom.TabIndex = 0;
            // 
            // lblCaption
            // 
            this.lblCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCaption.Location = new System.Drawing.Point(0, 0);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(300, 28);
            this.lblCaption.TabIndex = 2;
            this.lblCaption.Text = "Category";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // binStatusViewer1
            // 
            this.binStatusViewer1.Bin = null;
            this.binStatusViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.binStatusViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.binStatusViewer1.Location = new System.Drawing.Point(0, 28);
            this.binStatusViewer1.Name = "binStatusViewer1";
            this.binStatusViewer1.Size = new System.Drawing.Size(300, 172);
            this.binStatusViewer1.TabIndex = 1;
            // 
            // binStatusViewer2
            // 
            this.binStatusViewer2.Bin = null;
            this.binStatusViewer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.binStatusViewer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.binStatusViewer2.Location = new System.Drawing.Point(0, 0);
            this.binStatusViewer2.Name = "binStatusViewer2";
            this.binStatusViewer2.Size = new System.Drawing.Size(150, 99);
            this.binStatusViewer2.TabIndex = 0;
            // 
            // binStatusViewer3
            // 
            this.binStatusViewer3.Bin = null;
            this.binStatusViewer3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.binStatusViewer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.binStatusViewer3.Location = new System.Drawing.Point(0, 0);
            this.binStatusViewer3.Name = "binStatusViewer3";
            this.binStatusViewer3.Size = new System.Drawing.Size(149, 99);
            this.binStatusViewer3.TabIndex = 0;
            // 
            // MasterCategoryStatusViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitMain);
            this.Name = "MasterCategoryStatusViewer";
            this.Size = new System.Drawing.Size(300, 300);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.splitBottom.Panel1.ResumeLayout(false);
            this.splitBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBottom)).EndInit();
            this.splitBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitMain;
        private BinStatusViewer binStatusViewer1;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.SplitContainer splitBottom;
        private BinStatusViewer binStatusViewer2;
        private BinStatusViewer binStatusViewer3;
    }
}
