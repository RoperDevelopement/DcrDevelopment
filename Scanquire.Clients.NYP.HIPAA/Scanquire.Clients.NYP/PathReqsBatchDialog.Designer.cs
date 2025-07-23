namespace Scanquire.Clients.NYP
{
    partial class PathReqsBatchDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PathReqsBatchDialog));
            this.BatchSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.dpDateOfService = new EdocsUSA.Utilities.Controls.DatePicker();
            this.DateOfServiceLabel = new System.Windows.Forms.Label();
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
            this.BatchSettingsGroupBox.Controls.Add(this.dpDateOfService);
            this.BatchSettingsGroupBox.Controls.Add(this.DateOfServiceLabel);
            this.BatchSettingsGroupBox.Controls.Add(this.dpReceiptDate);
            this.BatchSettingsGroupBox.Controls.Add(this.ReceiptDateLabel);
            this.BatchSettingsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BatchSettingsGroupBox.Location = new System.Drawing.Point(17, 6);
            this.BatchSettingsGroupBox.Name = "BatchSettingsGroupBox";
            this.BatchSettingsGroupBox.Size = new System.Drawing.Size(387, 84);
            this.BatchSettingsGroupBox.TabIndex = 8;
            this.BatchSettingsGroupBox.TabStop = false;
            this.BatchSettingsGroupBox.Text = "Batch Settings";
            // 
            // dpDateOfService
            // 
            this.dpDateOfService.DisplayFormat = "MM/dd/yyyy";
            this.dpDateOfService.Location = new System.Drawing.Point(109, 53);
            this.dpDateOfService.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dpDateOfService.Name = "dpDateOfService";
            this.dpDateOfService.Size = new System.Drawing.Size(107, 20);
            this.dpDateOfService.TabIndex = 5;
            this.dpDateOfService.Value = null;
            // 
            // DateOfServiceLabel
            // 
            this.DateOfServiceLabel.AutoSize = true;
            this.DateOfServiceLabel.Location = new System.Drawing.Point(19, 53);
            this.DateOfServiceLabel.Name = "DateOfServiceLabel";
            this.DateOfServiceLabel.Size = new System.Drawing.Size(83, 13);
            this.DateOfServiceLabel.TabIndex = 4;
            this.DateOfServiceLabel.Text = "Date Of Service";
            // 
            // dpReceiptDate
            // 
            this.dpReceiptDate.DisplayFormat = "MM/dd/yyyy";
            this.dpReceiptDate.Location = new System.Drawing.Point(109, 24);
            this.dpReceiptDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dpReceiptDate.Name = "dpReceiptDate";
            this.dpReceiptDate.Size = new System.Drawing.Size(107, 20);
            this.dpReceiptDate.TabIndex = 3;
            this.dpReceiptDate.Value = null;
            // 
            // ReceiptDateLabel
            // 
            this.ReceiptDateLabel.AutoSize = true;
            this.ReceiptDateLabel.Location = new System.Drawing.Point(19, 24);
            this.ReceiptDateLabel.Name = "ReceiptDateLabel";
            this.ReceiptDateLabel.Size = new System.Drawing.Size(70, 13);
            this.ReceiptDateLabel.TabIndex = 2;
            this.ReceiptDateLabel.Text = "Receipt Date";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 281);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(419, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(16, 17);
            this.StatusLabel.Text = "...";
            // 
            // CancelDialogButton
            // 
            this.CancelDialogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelDialogButton.Location = new System.Drawing.Point(219, 188);
            this.CancelDialogButton.Name = "CancelDialogButton";
            this.CancelDialogButton.Size = new System.Drawing.Size(75, 75);
            this.CancelDialogButton.TabIndex = 10;
            this.CancelDialogButton.Text = "&Cancel";
            this.CancelDialogButton.UseVisualStyleBackColor = true;
            // 
            // SubmitButton
            // 
            this.SubmitButton.Location = new System.Drawing.Point(126, 188);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(75, 75);
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
            this.CategoryGroupBox.Location = new System.Drawing.Point(17, 98);
            this.CategoryGroupBox.Name = "CategoryGroupBox";
            this.CategoryGroupBox.Size = new System.Drawing.Size(387, 60);
            this.CategoryGroupBox.TabIndex = 12;
            this.CategoryGroupBox.TabStop = false;
            this.CategoryGroupBox.Text = "Category";
            // 
            // rad_CatUncategorized
            // 
            this.rad_CatUncategorized.AutoSize = true;
            this.rad_CatUncategorized.Checked = true;
            this.rad_CatUncategorized.Location = new System.Drawing.Point(21, 22);
            this.rad_CatUncategorized.Name = "rad_CatUncategorized";
            this.rad_CatUncategorized.Size = new System.Drawing.Size(94, 17);
            this.rad_CatUncategorized.TabIndex = 4;
            this.rad_CatUncategorized.TabStop = true;
            this.rad_CatUncategorized.Text = "Uncategorized";
            this.rad_CatUncategorized.UseVisualStyleBackColor = true;
            // 
            // radCat_Stats
            // 
            this.radCat_Stats.AutoSize = true;
            this.radCat_Stats.Location = new System.Drawing.Point(299, 22);
            this.radCat_Stats.Name = "radCat_Stats";
            this.radCat_Stats.Size = new System.Drawing.Size(60, 17);
            this.radCat_Stats.TabIndex = 3;
            this.radCat_Stats.Text = "STATS";
            this.radCat_Stats.UseVisualStyleBackColor = true;
            // 
            // radCat_Routine
            // 
            this.radCat_Routine.AutoSize = true;
            this.radCat_Routine.Location = new System.Drawing.Point(219, 22);
            this.radCat_Routine.Name = "radCat_Routine";
            this.radCat_Routine.Size = new System.Drawing.Size(74, 17);
            this.radCat_Routine.TabIndex = 2;
            this.radCat_Routine.Text = "ROUTINE";
            this.radCat_Routine.UseVisualStyleBackColor = true;
            // 
            // radCat_AMStats
            // 
            this.radCat_AMStats.AutoSize = true;
            this.radCat_AMStats.Location = new System.Drawing.Point(118, 22);
            this.radCat_AMStats.Name = "radCat_AMStats";
            this.radCat_AMStats.Size = new System.Drawing.Size(85, 17);
            this.radCat_AMStats.TabIndex = 0;
            this.radCat_AMStats.Text = "A.M. STATS";
            this.radCat_AMStats.UseVisualStyleBackColor = true;
            // 
            // PathReqsBatchDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 303);
            this.Controls.Add(this.BatchSettingsGroupBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.CancelDialogButton);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.CategoryGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PathReqsBatchDialog";
            this.Text = "Path Reqs";
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
        private EdocsUSA.Utilities.Controls.DatePicker dpDateOfService;
        private System.Windows.Forms.Label DateOfServiceLabel;
    }
}