namespace BinMonitor.Common
{
    partial class BinLookupControl
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
            this.pnlLookup = new System.Windows.Forms.Panel();
            this.BatchLookup = new BinMonitor.Common.BatchLookupControl();
            this.ManageBinControl = new BinMonitor.Common.BatchManagerControl();
            this.pnlLookup.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLookup
            // 
            this.pnlLookup.Controls.Add(this.BatchLookup);
            this.pnlLookup.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLookup.Location = new System.Drawing.Point(0, 0);
            this.pnlLookup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlLookup.Name = "pnlLookup";
            this.pnlLookup.Size = new System.Drawing.Size(825, 73);
            this.pnlLookup.TabIndex = 19;
            // 
            // BatchLookup
            // 
            this.BatchLookup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BatchLookup.Location = new System.Drawing.Point(0, 0);
            this.BatchLookup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BatchLookup.Name = "BatchLookup";
            this.BatchLookup.SelectedBatch = null;
            this.BatchLookup.Size = new System.Drawing.Size(825, 73);
            this.BatchLookup.TabIndex = 0;
            this.BatchLookup.SelectedBatchChanged += new System.EventHandler<BinMonitor.Common.SelectedBatchChangedEventArgs>(this.BatchLookup_SelectedBatchChanged);
            // 
            // ManageBinControl
            // 
            this.ManageBinControl.ActiveBatch = null;
            this.ManageBinControl.IsArchive = false;
            this.ManageBinControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ManageBinControl.Location = new System.Drawing.Point(0, 73);
            this.ManageBinControl.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.ManageBinControl.Name = "ManageBinControl";
            this.ManageBinControl.Size = new System.Drawing.Size(825, 617);
            this.ManageBinControl.TabIndex = 20;
            // 
            // BinLookupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ManageBinControl);
            this.Controls.Add(this.pnlLookup);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "BinLookupControl";
            this.Size = new System.Drawing.Size(825, 690);
            this.pnlLookup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLookup;
        private BatchManagerControl ManageBinControl;
        public BatchLookupControl BatchLookup;
    }
}
