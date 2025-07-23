namespace Scanquire.Clients.NYP
{
    partial class RecordManagerBatchDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecordManagerBatchDialog));
            this.BatchSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.dpReceiptDate = new EdocsUSA.Utilities.Controls.DatePicker();
            this.ReceiptDateLabel = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.CancelDialogButton = new System.Windows.Forms.Button();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.CategoryGroupBox = new System.Windows.Forms.GroupBox();
            this.rad_CatUncategorized = new System.Windows.Forms.RadioButton();
            this.radCat_Stats = new System.Windows.Forms.RadioButton();
            this.radCat_Routine = new System.Windows.Forms.RadioButton();
            this.radCat_AMStats = new System.Windows.Forms.RadioButton();
            this.BatchSettingsGroupBox.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.CategoryGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // BatchSettingsGroupBox
            // 
            this.BatchSettingsGroupBox.Controls.Add(this.dpReceiptDate);
            this.BatchSettingsGroupBox.Controls.Add(this.ReceiptDateLabel);
            this.BatchSettingsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BatchSettingsGroupBox.Location = new System.Drawing.Point(23, 7);
            this.BatchSettingsGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BatchSettingsGroupBox.Name = "BatchSettingsGroupBox";
            this.BatchSettingsGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BatchSettingsGroupBox.Size = new System.Drawing.Size(516, 94);
            this.BatchSettingsGroupBox.TabIndex = 8;
            this.BatchSettingsGroupBox.TabStop = false;
            this.BatchSettingsGroupBox.Text = "Batch Settings";
            // 
            // dpReceiptDate
            // 
            this.dpReceiptDate.DisplayFormat = "MM/dd/yyyy";
            this.dpReceiptDate.Location = new System.Drawing.Point(128, 41);
            this.dpReceiptDate.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.dpReceiptDate.Name = "dpReceiptDate";
            this.dpReceiptDate.Size = new System.Drawing.Size(143, 25);
            this.dpReceiptDate.TabIndex = 3;
            this.dpReceiptDate.Value = null;
            // 
            // ReceiptDateLabel
            // 
            this.ReceiptDateLabel.AutoSize = true;
            this.ReceiptDateLabel.Location = new System.Drawing.Point(27, 41);
            this.ReceiptDateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ReceiptDateLabel.Name = "ReceiptDateLabel";
            this.ReceiptDateLabel.Size = new System.Drawing.Size(90, 17);
            this.ReceiptDateLabel.TabIndex = 2;
            this.ReceiptDateLabel.Text = "Receipt Date";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 348);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(559, 25);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(18, 20);
            this.StatusLabel.Text = "...";
            // 
            // CancelDialogButton
            // 
            this.CancelDialogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelDialogButton.Location = new System.Drawing.Point(292, 203);
            this.CancelDialogButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CancelDialogButton.Name = "CancelDialogButton";
            this.CancelDialogButton.Size = new System.Drawing.Size(100, 92);
            this.CancelDialogButton.TabIndex = 10;
            this.CancelDialogButton.Text = "&Cancel";
            this.CancelDialogButton.UseVisualStyleBackColor = true;
            // 
            // SubmitButton
            // 
            this.SubmitButton.Location = new System.Drawing.Point(168, 203);
            this.SubmitButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(100, 92);
            this.SubmitButton.TabIndex = 9;
            this.SubmitButton.Text = "&Ok";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // CategoryGroupBox
            // 
            this.CategoryGroupBox.Controls.Add(this.rad_CatUncategorized);
            this.CategoryGroupBox.Controls.Add(this.radCat_Stats);
            this.CategoryGroupBox.Controls.Add(this.radCat_Routine);
            this.CategoryGroupBox.Controls.Add(this.radCat_AMStats);
            this.CategoryGroupBox.Location = new System.Drawing.Point(23, 108);
            this.CategoryGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CategoryGroupBox.Name = "CategoryGroupBox";
            this.CategoryGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CategoryGroupBox.Size = new System.Drawing.Size(516, 74);
            this.CategoryGroupBox.TabIndex = 12;
            this.CategoryGroupBox.TabStop = false;
            this.CategoryGroupBox.Text = "Category";
            // 
            // rad_CatUncategorized
            // 
            this.rad_CatUncategorized.AutoSize = true;
            this.rad_CatUncategorized.Checked = true;
            this.rad_CatUncategorized.Location = new System.Drawing.Point(24, 27);
            this.rad_CatUncategorized.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rad_CatUncategorized.Name = "rad_CatUncategorized";
            this.rad_CatUncategorized.Size = new System.Drawing.Size(121, 21);
            this.rad_CatUncategorized.TabIndex = 4;
            this.rad_CatUncategorized.TabStop = true;
            this.rad_CatUncategorized.Text = "Uncategorized";
            this.rad_CatUncategorized.UseVisualStyleBackColor = true;
            // 
            // radCat_Stats
            // 
            this.radCat_Stats.AutoSize = true;
            this.radCat_Stats.Location = new System.Drawing.Point(399, 27);
            this.radCat_Stats.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radCat_Stats.Name = "radCat_Stats";
            this.radCat_Stats.Size = new System.Drawing.Size(74, 21);
            this.radCat_Stats.TabIndex = 3;
            this.radCat_Stats.Text = "STATS";
            this.radCat_Stats.UseVisualStyleBackColor = true;
            // 
            // radCat_Routine
            // 
            this.radCat_Routine.AutoSize = true;
            this.radCat_Routine.Location = new System.Drawing.Point(292, 27);
            this.radCat_Routine.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radCat_Routine.Name = "radCat_Routine";
            this.radCat_Routine.Size = new System.Drawing.Size(91, 21);
            this.radCat_Routine.TabIndex = 2;
            this.radCat_Routine.Text = "ROUTINE";
            this.radCat_Routine.UseVisualStyleBackColor = true;
            // 
            // radCat_AMStats
            // 
            this.radCat_AMStats.AutoSize = true;
            this.radCat_AMStats.Location = new System.Drawing.Point(157, 27);
            this.radCat_AMStats.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radCat_AMStats.Name = "radCat_AMStats";
            this.radCat_AMStats.Size = new System.Drawing.Size(106, 21);
            this.radCat_AMStats.TabIndex = 0;
            this.radCat_AMStats.Text = "A.M. STATS";
            this.radCat_AMStats.UseVisualStyleBackColor = true;
            // 
            // RecordManagerBatchDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 373);
            this.Controls.Add(this.BatchSettingsGroupBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.CancelDialogButton);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.CategoryGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "RecordManagerBatchDialog";
            this.Text = "RecordManager";
            this.BatchSettingsGroupBox.ResumeLayout(false);
            this.BatchSettingsGroupBox.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.CategoryGroupBox.ResumeLayout(false);
            this.CategoryGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox BatchSettingsGroupBox;
        private EdocsUSA.Utilities.Controls.DatePicker dpReceiptDate;
        private System.Windows.Forms.Label ReceiptDateLabel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.Button CancelDialogButton;
        private System.Windows.Forms.Button SubmitButton;
        private System.Windows.Forms.GroupBox CategoryGroupBox;
        private System.Windows.Forms.RadioButton rad_CatUncategorized;
        private System.Windows.Forms.RadioButton radCat_Stats;
        private System.Windows.Forms.RadioButton radCat_Routine;
        private System.Windows.Forms.RadioButton radCat_AMStats;
    }
}