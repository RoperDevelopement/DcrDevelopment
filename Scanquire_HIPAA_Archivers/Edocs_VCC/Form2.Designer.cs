
namespace Edocs.Diocese.Of.Helena.Archiver
{
    partial class DioceseofHelenaArchiverDialog
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
            this.label7 = new System.Windows.Forms.Label();
            this.dtDateRangeEnd = new System.Windows.Forms.DateTimePicker();
            this.dtDateRangeStart = new System.Windows.Forms.DateTimePicker();
            this.cBoxBookType = new System.Windows.Forms.ComboBox();
            this.cBoxChurch = new System.Windows.Forms.ComboBox();
            this.cBoxCity = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.txtBoxTotalScanned = new System.Windows.Forms.TextBox();
            this.labRecScanned = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(595, 170);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(10, 13);
            this.label7.TabIndex = 41;
            this.label7.Text = "-";
            // 
            // dtDateRangeEnd
            // 
            this.dtDateRangeEnd.CustomFormat = "MM/yyyyy";
            this.dtDateRangeEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDateRangeEnd.Location = new System.Drawing.Point(611, 168);
            this.dtDateRangeEnd.Name = "dtDateRangeEnd";
            this.dtDateRangeEnd.Size = new System.Drawing.Size(77, 20);
            this.dtDateRangeEnd.TabIndex = 33;
            // 
            // dtDateRangeStart
            // 
            this.dtDateRangeStart.CustomFormat = "MM/yyyyy";
            this.dtDateRangeStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDateRangeStart.Location = new System.Drawing.Point(524, 169);
            this.dtDateRangeStart.Name = "dtDateRangeStart";
            this.dtDateRangeStart.Size = new System.Drawing.Size(65, 20);
            this.dtDateRangeStart.TabIndex = 32;
            // 
            // cBoxBookType
            // 
            this.cBoxBookType.FormattingEnabled = true;
            this.cBoxBookType.Location = new System.Drawing.Point(203, 167);
            this.cBoxBookType.Name = "cBoxBookType";
            this.cBoxBookType.Size = new System.Drawing.Size(121, 21);
            this.cBoxBookType.TabIndex = 31;
            // 
            // cBoxChurch
            // 
            this.cBoxChurch.FormattingEnabled = true;
            this.cBoxChurch.Location = new System.Drawing.Point(483, 137);
            this.cBoxChurch.Name = "cBoxChurch";
            this.cBoxChurch.Size = new System.Drawing.Size(121, 21);
            this.cBoxChurch.TabIndex = 29;
            // 
            // cBoxCity
            // 
            this.cBoxCity.FormattingEnabled = true;
            this.cBoxCity.Items.AddRange(new object[] {
            "Butte",
            "Helena"});
            this.cBoxCity.Location = new System.Drawing.Point(156, 138);
            this.cBoxCity.Name = "cBoxCity";
            this.cBoxCity.Size = new System.Drawing.Size(121, 21);
            this.cBoxCity.TabIndex = 28;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(113, 169);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 18);
            this.label10.TabIndex = 40;
            this.label10.Text = "Book Type:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(419, 169);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 18);
            this.label9.TabIndex = 39;
            this.label9.Text = "Date Range:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(419, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 18);
            this.label4.TabIndex = 35;
            this.label4.Text = "Church:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(113, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 18);
            this.label2.TabIndex = 34;
            this.label2.Text = "City:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(359, 292);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 37;
            this.button1.Text = "&Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(278, 292);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 36;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtBoxTotalScanned
            // 
            this.txtBoxTotalScanned.Location = new System.Drawing.Point(429, 231);
            this.txtBoxTotalScanned.Name = "txtBoxTotalScanned";
            this.txtBoxTotalScanned.Size = new System.Drawing.Size(46, 20);
            this.txtBoxTotalScanned.TabIndex = 38;
            // 
            // labRecScanned
            // 
            this.labRecScanned.AutoSize = true;
            this.labRecScanned.Location = new System.Drawing.Point(292, 234);
            this.labRecScanned.Name = "labRecScanned";
            this.labRecScanned.Size = new System.Drawing.Size(137, 13);
            this.labRecScanned.TabIndex = 30;
            this.labRecScanned.Text = "Total Documents Scanned:";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dtDateRangeEnd);
            this.Controls.Add(this.dtDateRangeStart);
            this.Controls.Add(this.cBoxBookType);
            this.Controls.Add(this.cBoxChurch);
            this.Controls.Add(this.cBoxCity);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtBoxTotalScanned);
            this.Controls.Add(this.labRecScanned);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Shown += new System.EventHandler(this.Form2_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtDateRangeEnd;
        private System.Windows.Forms.DateTimePicker dtDateRangeStart;
        private System.Windows.Forms.ComboBox cBoxBookType;
        private System.Windows.Forms.ComboBox cBoxChurch;
        private System.Windows.Forms.ComboBox cBoxCity;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtBoxTotalScanned;
        private System.Windows.Forms.Label labRecScanned;
    }
}