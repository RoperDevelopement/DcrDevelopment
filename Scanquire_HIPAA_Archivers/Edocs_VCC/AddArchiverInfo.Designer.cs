
namespace Edocs.Dillion.VCC.Archiver
{
    partial class AddArchiverInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddArchiverInfo));
            this.labCityBTChurch = new System.Windows.Forms.Label();
            this.rTxtBOxCityChurchBt = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labCityBTChurch
            // 
            this.labCityBTChurch.AutoSize = true;
            this.labCityBTChurch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labCityBTChurch.Location = new System.Drawing.Point(191, 16);
            this.labCityBTChurch.Name = "labCityBTChurch";
            this.labCityBTChurch.Size = new System.Drawing.Size(145, 13);
            this.labCityBTChurch.TabIndex = 0;
            this.labCityBTChurch.Text = "Add or Delete new churches:";
            // 
            // rTxtBOxCityChurchBt
            // 
            this.rTxtBOxCityChurchBt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rTxtBOxCityChurchBt.Location = new System.Drawing.Point(191, 49);
            this.rTxtBOxCityChurchBt.Name = "rTxtBOxCityChurchBt";
            this.rTxtBOxCityChurchBt.Size = new System.Drawing.Size(450, 108);
            this.rTxtBOxCityChurchBt.TabIndex = 1;
            this.rTxtBOxCityChurchBt.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(282, 185);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "&Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(424, 185);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // AddArchiverInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 240);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.rTxtBOxCityChurchBt);
            this.Controls.Add(this.labCityBTChurch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "AddArchiverInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Archiver City, Church, or Book Type";
            this.Shown += new System.EventHandler(this.AddArchiverInfo_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labCityBTChurch;
        private System.Windows.Forms.RichTextBox rTxtBOxCityChurchBt;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}