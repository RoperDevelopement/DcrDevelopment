
namespace Edocs.Demo.Archiver.Invoice
{
    partial class InvoiceArchiverDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InvoiceArchiverDialog));
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBoxInvCusNum = new System.Windows.Forms.TextBox();
            this.txtBoxInvTotal = new System.Windows.Forms.TextBox();
            this.txtBoxPONum = new System.Windows.Forms.TextBox();
            this.txtBoxInvNum = new System.Windows.Forms.TextBox();
            this.dtPickerInvDueDate = new System.Windows.Forms.DateTimePicker();
            this.dtPickerInvDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(167, 200);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "&Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtBoxInvCusNum);
            this.groupBox1.Controls.Add(this.txtBoxInvTotal);
            this.groupBox1.Controls.Add(this.txtBoxPONum);
            this.groupBox1.Controls.Add(this.txtBoxInvNum);
            this.groupBox1.Controls.Add(this.dtPickerInvDueDate);
            this.groupBox1.Controls.Add(this.dtPickerInvDate);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(550, 194);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Invoice:";
            // 
            // txtBoxInvCusNum
            // 
            this.txtBoxInvCusNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxInvCusNum.Location = new System.Drawing.Point(289, 16);
            this.txtBoxInvCusNum.Name = "txtBoxInvCusNum";
            this.txtBoxInvCusNum.Size = new System.Drawing.Size(140, 22);
            this.txtBoxInvCusNum.TabIndex = 11;
            // 
            // txtBoxInvTotal
            // 
            this.txtBoxInvTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxInvTotal.Location = new System.Drawing.Point(289, 161);
            this.txtBoxInvTotal.Name = "txtBoxInvTotal";
            this.txtBoxInvTotal.Size = new System.Drawing.Size(120, 22);
            this.txtBoxInvTotal.TabIndex = 10;
            this.txtBoxInvTotal.TextChanged += new System.EventHandler(this.txtBoxInvTotal_TextChanged);
            // 
            // txtBoxPONum
            // 
            this.txtBoxPONum.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxPONum.Location = new System.Drawing.Point(289, 47);
            this.txtBoxPONum.Name = "txtBoxPONum";
            this.txtBoxPONum.Size = new System.Drawing.Size(141, 22);
            this.txtBoxPONum.TabIndex = 9;
            // 
            // txtBoxInvNum
            // 
            this.txtBoxInvNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxInvNum.Location = new System.Drawing.Point(289, 72);
            this.txtBoxInvNum.Name = "txtBoxInvNum";
            this.txtBoxInvNum.Size = new System.Drawing.Size(141, 22);
            this.txtBoxInvNum.TabIndex = 3;
            // 
            // dtPickerInvDueDate
            // 
            this.dtPickerInvDueDate.Location = new System.Drawing.Point(289, 130);
            this.dtPickerInvDueDate.Name = "dtPickerInvDueDate";
            this.dtPickerInvDueDate.Size = new System.Drawing.Size(200, 26);
            this.dtPickerInvDueDate.TabIndex = 8;
            // 
            // dtPickerInvDate
            // 
            this.dtPickerInvDate.CustomFormat = "";
            this.dtPickerInvDate.Location = new System.Drawing.Point(289, 101);
            this.dtPickerInvDate.Name = "dtPickerInvDate";
            this.dtPickerInvDate.Size = new System.Drawing.Size(200, 26);
            this.dtPickerInvDate.TabIndex = 3;
            this.dtPickerInvDate.Leave += new System.EventHandler(this.dtPickerInvDate_Leave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(164, 166);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 16);
            this.label6.TabIndex = 7;
            this.label6.Text = "Invoice Total:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(164, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "PO Number:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(164, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "Invoice Due Date:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(164, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Date Of Invoice:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(164, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Invoice Number::";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(164, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Customer Number:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(258, 200);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // InvoiceArchiverDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 234);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InvoiceArchiverDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Invoice Archiver";
            this.Load += new System.EventHandler(this.InvoiceArchiverDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtPickerInvDueDate;
        private System.Windows.Forms.DateTimePicker dtPickerInvDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtBoxInvNum;
        private System.Windows.Forms.TextBox txtBoxInvCusNum;
        private System.Windows.Forms.TextBox txtBoxInvTotal;
        private System.Windows.Forms.TextBox txtBoxPONum;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}