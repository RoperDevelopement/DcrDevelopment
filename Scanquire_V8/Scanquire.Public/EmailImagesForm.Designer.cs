
namespace Scanquire.Public
{
    partial class EmailImagesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmailImagesForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.EmailSubject = new System.Windows.Forms.TextBox();
            this.EmailCC = new System.Windows.Forms.TextBox();
            this.EmailTO = new System.Windows.Forms.TextBox();
            this.EmailFrom = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.SendEmail = new System.Windows.Forms.ToolStripMenuItem();
            this.AddAttachments = new System.Windows.Forms.ToolStripMenuItem();
            this.Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.EmailMessage = new System.Windows.Forms.RichTextBox();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.EmailSubject);
            this.splitContainer1.Panel1.Controls.Add(this.EmailCC);
            this.splitContainer1.Panel1.Controls.Add(this.EmailTO);
            this.splitContainer1.Panel1.Controls.Add(this.EmailFrom);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.menuStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(832, 596);
            this.splitContainer1.SplitterDistance = 220;
            this.splitContainer1.TabIndex = 0;
            // 
            // EmailSubject
            // 
            this.EmailSubject.Location = new System.Drawing.Point(81, 168);
            this.EmailSubject.Multiline = true;
            this.EmailSubject.Name = "EmailSubject";
            this.EmailSubject.Size = new System.Drawing.Size(716, 29);
            this.EmailSubject.TabIndex = 3;
            // 
            // EmailCC
            // 
            this.EmailCC.Location = new System.Drawing.Point(83, 131);
            this.EmailCC.Multiline = true;
            this.EmailCC.Name = "EmailCC";
            this.EmailCC.Size = new System.Drawing.Size(716, 29);
            this.EmailCC.TabIndex = 2;
            this.EmailCC.Enter += new System.EventHandler(this.EmailCC_Enter);
            // 
            // EmailTO
            // 
            this.EmailTO.Location = new System.Drawing.Point(83, 93);
            this.EmailTO.Multiline = true;
            this.EmailTO.Name = "EmailTO";
            this.EmailTO.Size = new System.Drawing.Size(716, 29);
            this.EmailTO.TabIndex = 1;
            this.EmailTO.Enter += new System.EventHandler(this.EmailTO_Enter);
            // 
            // EmailFrom
            // 
            this.EmailFrom.Location = new System.Drawing.Point(83, 55);
            this.EmailFrom.Multiline = true;
            this.EmailFrom.Name = "EmailFrom";
            this.EmailFrom.Size = new System.Drawing.Size(300, 29);
            this.EmailFrom.TabIndex = 0;
            this.EmailFrom.Enter += new System.EventHandler(this.EmailFrom_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Email Subject:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Email CC:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Email To:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Email From:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SendEmail,
            this.AddAttachments,
            this.Exit});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(828, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // SendEmail
            // 
            this.SendEmail.Image = global::Scanquire.Public.Icons.send_mail5123;
            this.SendEmail.Name = "SendEmail";
            this.SendEmail.Size = new System.Drawing.Size(93, 20);
            this.SendEmail.Text = "&Send Email";
            this.SendEmail.Click += new System.EventHandler(this.SendEmail_Click);
            // 
            // AddAttachments
            // 
            this.AddAttachments.AutoToolTip = true;
            this.AddAttachments.Image = global::Scanquire.Public.Icons.emailatt512;
            this.AddAttachments.Name = "AddAttachments";
            this.AddAttachments.Size = new System.Drawing.Size(98, 20);
            this.AddAttachments.Text = "&Add Images";
            this.AddAttachments.ToolTipText = "Add Attachments";
            this.AddAttachments.Click += new System.EventHandler(this.AddAttachments_Click);
          
            // 
            // Exit
            // 
            this.Exit.Image = global::Scanquire.Public.Icons.remove_delete_exit_close_64;
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(54, 20);
            this.Exit.Text = "&Exit";
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.EmailMessage);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(828, 368);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Email Message";
            // 
            // EmailMessage
            // 
            this.EmailMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EmailMessage.Location = new System.Drawing.Point(3, 16);
            this.EmailMessage.Name = "EmailMessage";
            this.EmailMessage.Size = new System.Drawing.Size(822, 349);
            this.EmailMessage.TabIndex = 4;
            this.EmailMessage.Text = "";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Image = global::Scanquire.Public.Icons.send_mail64;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(141, 20);
            this.toolStripMenuItem1.Text = "toolStripMenuItem1";
            // 
            // ToolTip
            // 
            this.ToolTip.IsBalloon = true;
            this.ToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ToolTip.ToolTipTitle = "Send Email";
            // 
            // EmailImagesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSeaGreen;
            this.ClientSize = new System.Drawing.Size(832, 596);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EmailImagesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Email Images Form";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
       
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox EmailMessage;
        private System.Windows.Forms.TextBox EmailSubject;
        private System.Windows.Forms.TextBox EmailCC;
        private System.Windows.Forms.TextBox EmailTO;
        private System.Windows.Forms.TextBox EmailFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem SendEmail;
        private System.Windows.Forms.ToolStripMenuItem AddAttachments;
        private System.Windows.Forms.ToolStripMenuItem Exit;
        private System.Windows.Forms.ToolTip ToolTip;
    }
}