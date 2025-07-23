namespace BinMonitor.Common
{
    partial class BinLookupBySpecimenIdDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BinLookupBySpecimenIdDialog));
            this.txtSpecimenId = new System.Windows.Forms.TextBox();
            this.lbBins = new System.Windows.Forms.ListBox();
            this.btnLookupBinBySpecimenId = new System.Windows.Forms.Button();
            this.lblSpecimenId = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSpecimenId
            // 
            this.txtSpecimenId.Location = new System.Drawing.Point(75, 12);
            this.txtSpecimenId.Name = "txtSpecimenId";
            this.txtSpecimenId.Size = new System.Drawing.Size(118, 20);
            this.txtSpecimenId.TabIndex = 22;
            this.txtSpecimenId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSpecimenId_KeyPress);
            // 
            // lbBins
            // 
            this.lbBins.DisplayMember = "Id";
            this.lbBins.FormattingEnabled = true;
            this.lbBins.Location = new System.Drawing.Point(16, 38);
            this.lbBins.Name = "lbBins";
            this.lbBins.Size = new System.Drawing.Size(258, 134);
            this.lbBins.Sorted = true;
            this.lbBins.TabIndex = 24;
            // 
            // btnLookupBinBySpecimenId
            // 
            this.btnLookupBinBySpecimenId.Location = new System.Drawing.Point(221, 11);
            this.btnLookupBinBySpecimenId.Name = "btnLookupBinBySpecimenId";
            this.btnLookupBinBySpecimenId.Size = new System.Drawing.Size(53, 21);
            this.btnLookupBinBySpecimenId.TabIndex = 23;
            this.btnLookupBinBySpecimenId.Text = "Lookup";
            this.btnLookupBinBySpecimenId.UseVisualStyleBackColor = true;
            this.btnLookupBinBySpecimenId.Click += new System.EventHandler(this.btnLookupBinBySpecimenId_Click);
            // 
            // lblSpecimenId
            // 
            this.lblSpecimenId.Location = new System.Drawing.Point(13, 15);
            this.lblSpecimenId.Name = "lblSpecimenId";
            this.lblSpecimenId.Size = new System.Drawing.Size(56, 21);
            this.lblSpecimenId.TabIndex = 21;
            this.lblSpecimenId.Text = "Specimen ID";
            this.lblSpecimenId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(102, 178);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 75);
            this.button1.TabIndex = 25;
            this.button1.Text = "&OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ErrorProvider
            // 
            this.ErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.ErrorProvider.ContainerControl = this;
            // 
            // BinLookupBySpecimenIdDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtSpecimenId);
            this.Controls.Add(this.lbBins);
            this.Controls.Add(this.btnLookupBinBySpecimenId);
            this.Controls.Add(this.lblSpecimenId);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BinLookupBySpecimenIdDialog";
            this.Text = "Lookup Bin By Specimen";
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSpecimenId;
        private System.Windows.Forms.ListBox lbBins;
        private System.Windows.Forms.Button btnLookupBinBySpecimenId;
        private System.Windows.Forms.Label lblSpecimenId;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ErrorProvider ErrorProvider;
    }
}