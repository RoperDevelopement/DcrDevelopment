namespace Scanquire.Public.UserControls
{
    partial class DecriptPw
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
            this.lblPW = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtEncTxt = new System.Windows.Forms.TextBox();
            this.cmBoxChange = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblPW
            // 
            this.lblPW.AutoSize = true;
            this.lblPW.Location = new System.Drawing.Point(49, 82);
            this.lblPW.Name = "lblPW";
            this.lblPW.Size = new System.Drawing.Size(87, 13);
            this.lblPW.TabIndex = 0;
            this.lblPW.Text = "Text To Decript: ";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(111, 179);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Encript String";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // txtEncTxt
            // 
            this.txtEncTxt.Location = new System.Drawing.Point(148, 78);
            this.txtEncTxt.Name = "txtEncTxt";
            this.txtEncTxt.Size = new System.Drawing.Size(177, 20);
            this.txtEncTxt.TabIndex = 1;
            // 
            // cmBoxChange
            // 
            this.cmBoxChange.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmBoxChange.FormattingEnabled = true;
            this.cmBoxChange.Items.AddRange(new object[] {
            "DbUserName",
            "DbPassWord",
            "EmailPassword"});
            this.cmBoxChange.Location = new System.Drawing.Point(148, 125);
            this.cmBoxChange.Name = "cmBoxChange";
            this.cmBoxChange.Size = new System.Drawing.Size(121, 21);
            this.cmBoxChange.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Text to Decript";
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(212, 179);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Exit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // DecriptPw
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 227);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmBoxChange);
            this.Controls.Add(this.txtEncTxt);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblPW);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "DecriptPw";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Encript String";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPW;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtEncTxt;
        private System.Windows.Forms.ComboBox cmBoxChange;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
    }
}