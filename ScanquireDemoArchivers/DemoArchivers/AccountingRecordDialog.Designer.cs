namespace DemoArchivers
{
    partial class AccountingRecordDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountingRecordDialog));
            this.ClosingYearLabel = new System.Windows.Forms.Label();
            this.MonthTextBox = new System.Windows.Forms.TextBox();
            this.RecordIdLabel = new System.Windows.Forms.Label();
            this.LineItemTextBox = new System.Windows.Forms.TextBox();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.CancelDialogButton = new System.Windows.Forms.Button();
            this.YearTextBox = new System.Windows.Forms.TextBox();
            this.BoxIdLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ClosingYearLabel
            // 
            this.ClosingYearLabel.AutoSize = true;
            this.ClosingYearLabel.Location = new System.Drawing.Point(75, 46);
            this.ClosingYearLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ClosingYearLabel.Name = "ClosingYearLabel";
            this.ClosingYearLabel.Size = new System.Drawing.Size(47, 17);
            this.ClosingYearLabel.TabIndex = 3;
            this.ClosingYearLabel.Text = "Month";
            // 
            // MonthTextBox
            // 
            this.MonthTextBox.Location = new System.Drawing.Point(140, 43);
            this.MonthTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.MonthTextBox.Name = "MonthTextBox";
            this.MonthTextBox.Size = new System.Drawing.Size(132, 22);
            this.MonthTextBox.TabIndex = 2;
            // 
            // RecordIdLabel
            // 
            this.RecordIdLabel.AutoSize = true;
            this.RecordIdLabel.Location = new System.Drawing.Point(57, 81);
            this.RecordIdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RecordIdLabel.Name = "RecordIdLabel";
            this.RecordIdLabel.Size = new System.Drawing.Size(65, 17);
            this.RecordIdLabel.TabIndex = 5;
            this.RecordIdLabel.Text = "Line Item";
            // 
            // LineItemTextBox
            // 
            this.LineItemTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.LineItemTextBox.Location = new System.Drawing.Point(140, 78);
            this.LineItemTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.LineItemTextBox.Name = "LineItemTextBox";
            this.LineItemTextBox.Size = new System.Drawing.Size(165, 22);
            this.LineItemTextBox.TabIndex = 5;
            // 
            // SubmitButton
            // 
            this.SubmitButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SubmitButton.Location = new System.Drawing.Point(71, 141);
            this.SubmitButton.Margin = new System.Windows.Forms.Padding(4);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(100, 92);
            this.SubmitButton.TabIndex = 13;
            this.SubmitButton.Text = "&Ok";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // CancelDialogButton
            // 
            this.CancelDialogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelDialogButton.Location = new System.Drawing.Point(179, 141);
            this.CancelDialogButton.Margin = new System.Windows.Forms.Padding(4);
            this.CancelDialogButton.Name = "CancelDialogButton";
            this.CancelDialogButton.Size = new System.Drawing.Size(100, 92);
            this.CancelDialogButton.TabIndex = 14;
            this.CancelDialogButton.Text = "&Cancel";
            this.CancelDialogButton.UseVisualStyleBackColor = true;
            // 
            // YearTextBox
            // 
            this.YearTextBox.Location = new System.Drawing.Point(140, 7);
            this.YearTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.YearTextBox.Name = "YearTextBox";
            this.YearTextBox.Size = new System.Drawing.Size(132, 22);
            this.YearTextBox.TabIndex = 1;
            // 
            // BoxIdLabel
            // 
            this.BoxIdLabel.AutoSize = true;
            this.BoxIdLabel.Location = new System.Drawing.Point(84, 10);
            this.BoxIdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BoxIdLabel.Name = "BoxIdLabel";
            this.BoxIdLabel.Size = new System.Drawing.Size(38, 17);
            this.BoxIdLabel.TabIndex = 15;
            this.BoxIdLabel.Text = "Year";
            // 
            // AccountingRecordDialog
            // 
            this.AcceptButton = this.SubmitButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 248);
            this.Controls.Add(this.YearTextBox);
            this.Controls.Add(this.BoxIdLabel);
            this.Controls.Add(this.CancelDialogButton);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.LineItemTextBox);
            this.Controls.Add(this.RecordIdLabel);
            this.Controls.Add(this.MonthTextBox);
            this.Controls.Add(this.ClosingYearLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AccountingRecordDialog";
            this.Text = "Accounting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ClosingYearLabel;
        private System.Windows.Forms.TextBox MonthTextBox;
        private System.Windows.Forms.Label RecordIdLabel;
        private System.Windows.Forms.TextBox LineItemTextBox;
        private System.Windows.Forms.Button SubmitButton;
        private System.Windows.Forms.Button CancelDialogButton;
        private System.Windows.Forms.TextBox YearTextBox;
        private System.Windows.Forms.Label BoxIdLabel;
    }
}