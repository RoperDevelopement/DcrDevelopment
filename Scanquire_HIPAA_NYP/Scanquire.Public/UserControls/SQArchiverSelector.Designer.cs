namespace Scanquire.Public.UserControls
{
    partial class SQArchiverSelector
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
            this.CurrentArchiverComboBox = new System.Windows.Forms.ComboBox();
            this.CaptionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CurrentArchiverComboBox
            // 
            this.CurrentArchiverComboBox.DisplayMember = "Key";
            this.CurrentArchiverComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CurrentArchiverComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CurrentArchiverComboBox.FormattingEnabled = true;
            this.CurrentArchiverComboBox.Location = new System.Drawing.Point(3, 19);
            this.CurrentArchiverComboBox.Name = "CurrentArchiverComboBox";
            this.CurrentArchiverComboBox.Size = new System.Drawing.Size(244, 21);
            this.CurrentArchiverComboBox.TabIndex = 0;
            this.CurrentArchiverComboBox.TabStop = false;
            this.CurrentArchiverComboBox.ValueMember = "Value";
            // 
            // CaptionLabel
            // 
            this.CaptionLabel.AutoSize = true;
            this.CaptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.CaptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CaptionLabel.Location = new System.Drawing.Point(3, 3);
            this.CaptionLabel.Name = "CaptionLabel";
            this.CaptionLabel.Size = new System.Drawing.Size(118, 16);
            this.CaptionLabel.TabIndex = 1;
            this.CaptionLabel.Text = "Current Archiver";
            // 
            // SQArchiverSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CurrentArchiverComboBox);
            this.Controls.Add(this.CaptionLabel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(250, 50);
            this.Name = "SQArchiverSelector";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(250, 50);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CurrentArchiverComboBox;
        private System.Windows.Forms.Label CaptionLabel;
    }
}
