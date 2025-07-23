namespace ScanquireLogin
{
    partial class LogInForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogInForm));
            this.labId = new System.Windows.Forms.Label();
            this.labPW = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogOn = new System.Windows.Forms.Button();
            this.btnCan = new System.Windows.Forms.Button();
            this.chkBoxShowPW = new System.Windows.Forms.CheckBox();
            this.cmbBoxUserID = new System.Windows.Forms.ComboBox();
            this.labTitle = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // labId
            // 
            this.labId.AutoSize = true;
            this.labId.Location = new System.Drawing.Point(65, 81);
            this.labId.Name = "labId";
            this.labId.Size = new System.Drawing.Size(51, 13);
            this.labId.TabIndex = 0;
            this.labId.Text = "LogIn ID:";
            // 
            // labPW
            // 
            this.labPW.AutoSize = true;
            this.labPW.Location = new System.Drawing.Point(65, 128);
            this.labPW.Name = "labPW";
            this.labPW.Size = new System.Drawing.Size(59, 13);
            this.labPW.TabIndex = 2;
            this.labPW.Text = "PassWord:";
            // 
            // txtPassword
            // 
            this.txtPassword.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtPassword.Location = new System.Drawing.Point(140, 128);
            this.txtPassword.MaxLength = 50;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.Text = "EdocsAdminPassword";
            this.txtPassword.Enter += new System.EventHandler(this.TxtPassword_Enter);
            this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtPassword_KeyPress);
            // 
            // btnLogOn
            // 
            this.btnLogOn.BackColor = System.Drawing.Color.LightBlue;
            this.btnLogOn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLogOn.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnLogOn.Location = new System.Drawing.Point(107, 189);
            this.btnLogOn.Name = "btnLogOn";
            this.btnLogOn.Size = new System.Drawing.Size(75, 23);
            this.btnLogOn.TabIndex = 4;
            this.btnLogOn.Text = "&Log On";
            this.btnLogOn.UseVisualStyleBackColor = false;
            this.btnLogOn.Click += new System.EventHandler(this.BtnLogOn_Click);
            // 
            // btnCan
            // 
            this.btnCan.BackColor = System.Drawing.Color.LightBlue;
            this.btnCan.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnCan.Location = new System.Drawing.Point(206, 189);
            this.btnCan.Name = "btnCan";
            this.btnCan.Size = new System.Drawing.Size(75, 23);
            this.btnCan.TabIndex = 1;
            this.btnCan.Text = "&Cancel";
            this.btnCan.UseVisualStyleBackColor = false;
            this.btnCan.Click += new System.EventHandler(this.Button2_Click);
            // 
            // chkBoxShowPW
            // 
            this.chkBoxShowPW.AutoSize = true;
            this.chkBoxShowPW.Location = new System.Drawing.Point(140, 156);
            this.chkBoxShowPW.Name = "chkBoxShowPW";
            this.chkBoxShowPW.Size = new System.Drawing.Size(105, 17);
            this.chkBoxShowPW.TabIndex = 6;
            this.chkBoxShowPW.Text = "Show PassWord";
            this.chkBoxShowPW.UseVisualStyleBackColor = true;
            this.chkBoxShowPW.CheckStateChanged += new System.EventHandler(this.ChkBoxShowPW_CheckStateChanged);
            // 
            // cmbBoxUserID
            // 
            this.cmbBoxUserID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxUserID.FormattingEnabled = true;
            this.cmbBoxUserID.Location = new System.Drawing.Point(140, 77);
            this.cmbBoxUserID.Name = "cmbBoxUserID";
            this.cmbBoxUserID.Size = new System.Drawing.Size(250, 21);
            this.cmbBoxUserID.TabIndex = 7;
            // 
            // labTitle
            // 
            this.labTitle.AutoSize = true;
            this.labTitle.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labTitle.Location = new System.Drawing.Point(73, 18);
            this.labTitle.Name = "labTitle";
            this.labTitle.Size = new System.Drawing.Size(256, 33);
            this.labTitle.TabIndex = 8;
            this.labTitle.Text = "Log On ScanQuire";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(140, 99);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 23);
            this.progressBar1.TabIndex = 9;
            this.progressBar1.Value = 5;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            // 
            // LogInForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(416, 240);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.labTitle);
            this.Controls.Add(this.cmbBoxUserID);
            this.Controls.Add(this.btnCan);
            this.Controls.Add(this.btnLogOn);
            this.Controls.Add(this.chkBoxShowPW);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.labPW);
            this.Controls.Add(this.labId);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LogInForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LogIn Scanquire";
            this.Shown += new System.EventHandler(this.LogInForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labId;
        private System.Windows.Forms.Label labPW;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogOn;
        private System.Windows.Forms.Button btnCan;
        private System.Windows.Forms.CheckBox chkBoxShowPW;
        private System.Windows.Forms.ComboBox cmbBoxUserID;
        private System.Windows.Forms.Label labTitle;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}