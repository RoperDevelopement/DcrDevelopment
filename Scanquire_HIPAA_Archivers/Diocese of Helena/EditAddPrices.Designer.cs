
namespace Edocs.Diocese.Of.Helena.Archiver
{
    partial class EditAddPrices
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditAddPrices));
            this.dataGridDocPrices = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DOCSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.edit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txxBoxDocSize = new System.Windows.Forms.TextBox();
            this.txtBoxDocPrice = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDocPrices)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridDocPrices
            // 
            this.dataGridDocPrices.AllowUserToAddRows = false;
            this.dataGridDocPrices.AllowUserToDeleteRows = false;
            this.dataGridDocPrices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridDocPrices.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridDocPrices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridDocPrices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.DOCSize,
            this.DocPrice,
            this.edit});
            this.dataGridDocPrices.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridDocPrices.Location = new System.Drawing.Point(0, 0);
            this.dataGridDocPrices.MultiSelect = false;
            this.dataGridDocPrices.Name = "dataGridDocPrices";
            this.dataGridDocPrices.ReadOnly = true;
            this.dataGridDocPrices.Size = new System.Drawing.Size(555, 172);
            this.dataGridDocPrices.TabIndex = 0;
            this.dataGridDocPrices.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.MaxInputLength = 10;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ID.Visible = false;
            // 
            // DOCSize
            // 
            this.DOCSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DOCSize.HeaderText = "Document Size";
            this.DOCSize.Name = "DOCSize";
            this.DOCSize.ReadOnly = true;
            this.DOCSize.Width = 96;
            // 
            // DocPrice
            // 
            this.DocPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DocPrice.HeaderText = "Document Price";
            this.DocPrice.Name = "DocPrice";
            this.DocPrice.ReadOnly = true;
            this.DocPrice.Width = 99;
            // 
            // edit
            // 
            this.edit.HeaderText = "";
            this.edit.Name = "edit";
            this.edit.ReadOnly = true;
            this.edit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.edit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(169, 284);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "&Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(284, 284);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "&Add";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(148, 210);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Document Size:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(148, 248);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Document Price:$";
            // 
            // txxBoxDocSize
            // 
            this.txxBoxDocSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txxBoxDocSize.Location = new System.Drawing.Point(257, 209);
            this.txxBoxDocSize.Name = "txxBoxDocSize";
            this.txxBoxDocSize.Size = new System.Drawing.Size(211, 24);
            this.txxBoxDocSize.TabIndex = 5;
            // 
            // txtBoxDocPrice
            // 
            this.txtBoxDocPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDocPrice.Location = new System.Drawing.Point(257, 243);
            this.txtBoxDocPrice.Name = "txtBoxDocPrice";
            this.txtBoxDocPrice.Size = new System.Drawing.Size(131, 24);
            this.txtBoxDocPrice.TabIndex = 6;
            // 
            // EditAddPrices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 326);
            this.Controls.Add(this.txtBoxDocPrice);
            this.Controls.Add(this.txxBoxDocSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridDocPrices);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditAddPrices";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Document Prices";
            this.Load += new System.EventHandler(this.EditAddPrices_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDocPrices)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridDocPrices;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txxBoxDocSize;
        private System.Windows.Forms.TextBox txtBoxDocPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DOCSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocPrice;
        private System.Windows.Forms.DataGridViewButtonColumn edit;
    }
}