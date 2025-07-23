namespace DemoArchivers
{
    partial class NypSendoutPackingSlipsInputDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NypSendoutPackingSlipsInputDialog));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblProcDate = new System.Windows.Forms.Label();
            this.txtProcDate = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(24, 58);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 75);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCancel.Location = new System.Drawing.Point(105, 58);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 75);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblProcDate
            // 
            this.lblProcDate.AutoSize = true;
            this.lblProcDate.Location = new System.Drawing.Point(35, 18);
            this.lblProcDate.Name = "lblProcDate";
            this.lblProcDate.Size = new System.Drawing.Size(30, 13);
            this.lblProcDate.TabIndex = 3;
            this.lblProcDate.Text = "Date";
            // 
            // txtProcDate
            // 
            this.txtProcDate.Location = new System.Drawing.Point(71, 15);
            this.txtProcDate.Name = "txtProcDate";
            this.txtProcDate.Size = new System.Drawing.Size(99, 20);
            this.txtProcDate.TabIndex = 4;
            // 
            // NypSendoutPackingSlipsInputDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(204, 152);
            this.Controls.Add(this.txtProcDate);
            this.Controls.Add(this.lblProcDate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NypSendoutPackingSlipsInputDialog";
            this.Text = "Packing Slips";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblProcDate;
        private System.Windows.Forms.TextBox txtProcDate;
    }
}