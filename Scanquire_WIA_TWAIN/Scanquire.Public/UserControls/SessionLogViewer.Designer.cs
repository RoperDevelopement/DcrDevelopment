namespace Scanquire.Public.UserControls
{
    partial class SessionLogViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionLogViewer));
            this.SessionLogTextBox = new System.Windows.Forms.RichTextBox();
            this.TraceListenerComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // SessionLogTextBox
            // 
            this.SessionLogTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SessionLogTextBox.Location = new System.Drawing.Point(0, 21);
            this.SessionLogTextBox.Name = "SessionLogTextBox";
            this.SessionLogTextBox.Size = new System.Drawing.Size(624, 421);
            this.SessionLogTextBox.TabIndex = 0;
            this.SessionLogTextBox.Text = "";
            this.SessionLogTextBox.WordWrap = false;
            // 
            // TraceListenerComboBox
            // 
            this.TraceListenerComboBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.TraceListenerComboBox.FormattingEnabled = true;
            this.TraceListenerComboBox.Location = new System.Drawing.Point(0, 0);
            this.TraceListenerComboBox.Name = "TraceListenerComboBox";
            this.TraceListenerComboBox.Size = new System.Drawing.Size(624, 21);
            this.TraceListenerComboBox.TabIndex = 1;
            this.TraceListenerComboBox.SelectedValueChanged += new System.EventHandler(this.TraceListenerComboBox_SelectedValueChanged);
            // 
            // SessionLogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.SessionLogTextBox);
            this.Controls.Add(this.TraceListenerComboBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SessionLogViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Session Log Viewer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox SessionLogTextBox;
        private System.Windows.Forms.ComboBox TraceListenerComboBox;
    }
}