namespace BinMonitor.Common
{
    partial class SpecimenLookupControl
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
            this.txtSpecimenId = new System.Windows.Forms.TextBox();
            this.btnLookup = new System.Windows.Forms.Button();
            this.lblSpecimenId = new System.Windows.Forms.Label();
            this.lbBins = new System.Windows.Forms.ListBox();
            this.pnlLookup.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLookup
            // 
            this.pnlLookup.Controls.Add(this.txtSpecimenId);
            this.pnlLookup.Controls.Add(this.btnLookup);
            this.pnlLookup.Controls.Add(this.lblSpecimenId);
            this.pnlLookup.Location = new System.Drawing.Point(0, 0);
            this.pnlLookup.Name = "pnlLookup";
            this.pnlLookup.Size = new System.Drawing.Size(249, 21);
            this.pnlLookup.TabIndex = 0;
            // 
            // txtSpecimenId
            // 
            this.txtSpecimenId.Location = new System.Drawing.Point(56, 0);
            this.txtSpecimenId.Name = "txtSpecimenId";
            this.txtSpecimenId.Size = new System.Drawing.Size(118, 20);
            this.txtSpecimenId.TabIndex = 1;
            // 
            // btnLookup
            // 
            this.btnLookup.Location = new System.Drawing.Point(174, 0);
            this.btnLookup.Name = "btnLookup";
            this.btnLookup.Size = new System.Drawing.Size(75, 21);
            this.btnLookup.TabIndex = 2;
            this.btnLookup.Text = "Lookup";
            this.btnLookup.UseVisualStyleBackColor = true;
            this.btnLookup.Click += new System.EventHandler(this.btnLookup_Click);
            // 
            // lblSpecimenId
            // 
            this.lblSpecimenId.Location = new System.Drawing.Point(0, 0);
            this.lblSpecimenId.Name = "lblSpecimenId";
            this.lblSpecimenId.Size = new System.Drawing.Size(56, 21);
            this.lblSpecimenId.TabIndex = 0;
            this.lblSpecimenId.Text = "Specimen ID";
            this.lblSpecimenId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbBins
            // 
            this.lbBins.DisplayMember = "Id";
            this.lbBins.FormattingEnabled = true;
            this.lbBins.Location = new System.Drawing.Point(0, 21);
            this.lbBins.Name = "lbBins";
            this.lbBins.Size = new System.Drawing.Size(249, 173);
            this.lbBins.TabIndex = 1;
            // 
            // SpecimenLookupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbBins);
            this.Controls.Add(this.pnlLookup);
            this.Name = "SpecimenLookupControl";
            this.Size = new System.Drawing.Size(249, 198);
            this.pnlLookup.ResumeLayout(false);
            this.pnlLookup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLookup;
        private System.Windows.Forms.TextBox txtSpecimenId;
        private System.Windows.Forms.Button btnLookup;
        private System.Windows.Forms.Label lblSpecimenId;
        private System.Windows.Forms.ListBox lbBins;
    }
}
