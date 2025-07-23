
namespace Edocs.Create.Encrypted.PW
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtBoxKey = new System.Windows.Forms.TextBox();
            this.txtBoxPassWord = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnEnc = new System.Windows.Forms.Button();
            this.btndecrypt = new System.Windows.Forms.Button();
            this.labPWEncDec = new System.Windows.Forms.Label();
            this.txtBoxPwEncptDecpt = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.chkBoxLocalMachine = new System.Windows.Forms.CheckBox();
            this.labKeyLength = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtBoxKey
            // 
            this.txtBoxKey.Location = new System.Drawing.Point(87, 15);
            this.txtBoxKey.Name = "txtBoxKey";
            this.txtBoxKey.Size = new System.Drawing.Size(251, 20);
            this.txtBoxKey.TabIndex = 0;
            this.txtBoxKey.TextChanged += new System.EventHandler(this.txtBoxKey_TextChanged);
            // 
            // txtBoxPassWord
            // 
            this.txtBoxPassWord.Location = new System.Drawing.Point(125, 39);
            this.txtBoxPassWord.Name = "txtBoxPassWord";
            this.txtBoxPassWord.Size = new System.Drawing.Size(251, 20);
            this.txtBoxPassWord.TabIndex = 1;
            this.txtBoxPassWord.TextChanged += new System.EventHandler(this.txtBoxPassWord_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Key:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(26, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "PassWord:";
            // 
            // btnEnc
            // 
            this.btnEnc.Location = new System.Drawing.Point(29, 147);
            this.btnEnc.Name = "btnEnc";
            this.btnEnc.Size = new System.Drawing.Size(75, 23);
            this.btnEnc.TabIndex = 4;
            this.btnEnc.Text = "&Encryption";
            this.btnEnc.UseVisualStyleBackColor = true;
            this.btnEnc.Click += new System.EventHandler(this.btnEnc_Click);
            // 
            // btndecrypt
            // 
            this.btndecrypt.Location = new System.Drawing.Point(110, 147);
            this.btndecrypt.Name = "btndecrypt";
            this.btndecrypt.Size = new System.Drawing.Size(75, 23);
            this.btndecrypt.TabIndex = 5;
            this.btndecrypt.Text = "&Decrypt";
            this.btndecrypt.UseVisualStyleBackColor = true;
            this.btndecrypt.Click += new System.EventHandler(this.btndecrypt_Click);
            // 
            // labPWEncDec
            // 
            this.labPWEncDec.AutoSize = true;
            this.labPWEncDec.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labPWEncDec.Location = new System.Drawing.Point(26, 70);
            this.labPWEncDec.Name = "labPWEncDec";
            this.labPWEncDec.Size = new System.Drawing.Size(149, 18);
            this.labPWEncDec.TabIndex = 7;
            this.labPWEncDec.Text = "PassWord Encrypted";
            this.labPWEncDec.Visible = false;
            // 
            // txtBoxPwEncptDecpt
            // 
            this.txtBoxPwEncptDecpt.Location = new System.Drawing.Point(181, 69);
            this.txtBoxPwEncptDecpt.Name = "txtBoxPwEncptDecpt";
            this.txtBoxPwEncptDecpt.ReadOnly = true;
            this.txtBoxPwEncptDecpt.Size = new System.Drawing.Size(251, 20);
            this.txtBoxPwEncptDecpt.TabIndex = 6;
            this.txtBoxPwEncptDecpt.Visible = false;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(191, 147);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 8;
            this.btnExport.Text = "&Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(272, 147);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.CheckPathExists = false;
            this.saveFileDialog1.Filter = "All Files|*.*|Excel File|*.csv|txt TextFiles|*.txt";
            this.saveFileDialog1.FilterIndex = 3;
            this.saveFileDialog1.Title = "Save Information";
            // 
            // chkBoxLocalMachine
            // 
            this.chkBoxLocalMachine.AutoSize = true;
            this.chkBoxLocalMachine.Checked = true;
            this.chkBoxLocalMachine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxLocalMachine.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxLocalMachine.Location = new System.Drawing.Point(29, 103);
            this.chkBoxLocalMachine.Name = "chkBoxLocalMachine";
            this.chkBoxLocalMachine.Size = new System.Drawing.Size(252, 22);
            this.chkBoxLocalMachine.TabIndex = 10;
            this.chkBoxLocalMachine.Text = "Encryption Decrypt Local Machine";
            this.chkBoxLocalMachine.UseVisualStyleBackColor = true;
            // 
            // labKeyLength
            // 
            this.labKeyLength.AutoSize = true;
            this.labKeyLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labKeyLength.Location = new System.Drawing.Point(26, 126);
            this.labKeyLength.Name = "labKeyLength";
            this.labKeyLength.Size = new System.Drawing.Size(97, 18);
            this.labKeyLength.TabIndex = 11;
            this.labKeyLength.Text = "Key Length: 0";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 183);
            this.Controls.Add(this.labKeyLength);
            this.Controls.Add(this.chkBoxLocalMachine);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.labPWEncDec);
            this.Controls.Add(this.txtBoxPwEncptDecpt);
            this.Controls.Add(this.btndecrypt);
            this.Controls.Add(this.btnEnc);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBoxPassWord);
            this.Controls.Add(this.txtBoxKey);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Encrypt Decrypt Password";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxKey;
        private System.Windows.Forms.TextBox txtBoxPassWord;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnEnc;
        private System.Windows.Forms.Button btndecrypt;
        private System.Windows.Forms.Label labPWEncDec;
        private System.Windows.Forms.TextBox txtBoxPwEncptDecpt;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox chkBoxLocalMachine;
        private System.Windows.Forms.Label labKeyLength;
    }
}

