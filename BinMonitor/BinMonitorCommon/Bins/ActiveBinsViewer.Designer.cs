namespace BinMonitor.Common
{
    partial class ActiveBinsViewer
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
            this.dgvQuickView = new System.Windows.Forms.DataGridView();
            this.BinID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Started = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuickView)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvQuickView
            // 
            this.dgvQuickView.AllowUserToAddRows = false;
            this.dgvQuickView.AllowUserToDeleteRows = false;
            this.dgvQuickView.AllowUserToResizeColumns = false;
            this.dgvQuickView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvQuickView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvQuickView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BinID,
            this.Started});
            this.dgvQuickView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvQuickView.EnableHeadersVisualStyles = false;
            this.dgvQuickView.Location = new System.Drawing.Point(0, 0);
            this.dgvQuickView.Name = "dgvQuickView";
            this.dgvQuickView.RowHeadersVisible = false;
            this.dgvQuickView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvQuickView.ShowEditingIcon = false;
            this.dgvQuickView.Size = new System.Drawing.Size(150, 357);
            this.dgvQuickView.TabIndex = 1;
            this.dgvQuickView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvQuickView_CellFormatting);
            // 
            // BinID
            // 
            this.BinID.DataPropertyName = "BinID";
            this.BinID.Frozen = true;
            this.BinID.HeaderText = "Bin";
            this.BinID.Name = "BinID";
            this.BinID.ReadOnly = true;
            this.BinID.Width = 40;
            // 
            // Started
            // 
            this.Started.DataPropertyName = "Started";
            this.Started.Frozen = true;
            this.Started.HeaderText = "Started";
            this.Started.Name = "Started";
            this.Started.ReadOnly = true;
            this.Started.Width = 200;
            // 
            // ActiveBinsViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvQuickView);
            this.Name = "ActiveBinsViewer";
            this.Size = new System.Drawing.Size(150, 357);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuickView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvQuickView;
        private System.Windows.Forms.DataGridViewTextBoxColumn BinID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Started;
    }
}
