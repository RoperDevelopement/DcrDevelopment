
namespace E_Docs.De_Dpeckle.Image
{
    partial class De_Dpeckle_Image
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBoxFinal = new System.Windows.Forms.PictureBox();
            this.pictureBoxOrg = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFinal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOrg)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBoxFinal);
            this.panel1.Controls.Add(this.pictureBoxOrg);
            this.panel1.Location = new System.Drawing.Point(35, 46);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(847, 658);
            this.panel1.TabIndex = 0;
            // 
            // pictureBoxFinal
            // 
            this.pictureBoxFinal.Location = new System.Drawing.Point(480, 30);
            this.pictureBoxFinal.Name = "pictureBoxFinal";
            this.pictureBoxFinal.Size = new System.Drawing.Size(334, 616);
            this.pictureBoxFinal.TabIndex = 1;
            this.pictureBoxFinal.TabStop = false;
            this.pictureBoxFinal.Click += new System.EventHandler(this.pictureBoxFinal_Click);
            // 
            // pictureBoxOrg
            // 
            this.pictureBoxOrg.Location = new System.Drawing.Point(17, 30);
            this.pictureBoxOrg.Name = "pictureBoxOrg";
            this.pictureBoxOrg.Size = new System.Drawing.Size(405, 616);
            this.pictureBoxOrg.TabIndex = 0;
            this.pictureBoxOrg.TabStop = false;
            this.pictureBoxOrg.Click += new System.EventHandler(this.pictureBoxOrg_Click);
            // 
            // De_Dpeckle_Image
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 704);
            this.Controls.Add(this.panel1);
            this.Name = "De_Dpeckle_Image";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFinal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOrg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBoxFinal;
        private System.Windows.Forms.PictureBox pictureBoxOrg;
    }
}

