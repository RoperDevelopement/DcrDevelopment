
namespace Edocs.WorkFlow.Archiver.ArchiverForms
{
    partial class DialogWFUsers
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lBoxSelectedUsers = new System.Windows.Forms.ListBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lBoxUsers = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkBoxNotifyDone = new System.Windows.Forms.CheckBox();
            this.chkBoxNotifyEachView = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(78, 406);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 37);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "&Ok";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(417, 406);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 37);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkBoxNotifyEachView);
            this.groupBox1.Controls.Add(this.chkBoxNotifyDone);
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lBoxSelectedUsers);
            this.groupBox1.Controls.Add(this.btnRemove);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.lBoxUsers);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(588, 399);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Users To View / Sign:";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(255, 254);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 66);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "&Clear Selected Users";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Visible = false;
            this.btnClear.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(331, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Select User to Remove";
            // 
            // lBoxSelectedUsers
            // 
            this.lBoxSelectedUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lBoxSelectedUsers.FormattingEnabled = true;
            this.lBoxSelectedUsers.ItemHeight = 20;
            this.lBoxSelectedUsers.Location = new System.Drawing.Point(331, 47);
            this.lBoxSelectedUsers.Name = "lBoxSelectedUsers";
            this.lBoxSelectedUsers.Size = new System.Drawing.Size(249, 284);
            this.lBoxSelectedUsers.TabIndex = 4;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(253, 193);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "<<";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(253, 140);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = ">>";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lBoxUsers
            // 
            this.lBoxUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lBoxUsers.FormattingEnabled = true;
            this.lBoxUsers.ItemHeight = 20;
            this.lBoxUsers.Location = new System.Drawing.Point(6, 47);
            this.lBoxUsers.Name = "lBoxUsers";
            this.lBoxUsers.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lBoxUsers.Size = new System.Drawing.Size(243, 284);
            this.lBoxUsers.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select One or More User to Add";
            // 
            // chkBoxNotifyDone
            // 
            this.chkBoxNotifyDone.AutoSize = true;
            this.chkBoxNotifyDone.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkBoxNotifyDone.Location = new System.Drawing.Point(12, 337);
            this.chkBoxNotifyDone.Name = "chkBoxNotifyDone";
            this.chkBoxNotifyDone.Size = new System.Drawing.Size(195, 20);
            this.chkBoxNotifyDone.TabIndex = 7;
            this.chkBoxNotifyDone.Text = "Send Notification Completed";
            this.chkBoxNotifyDone.UseVisualStyleBackColor = true;
            // 
            // chkBoxNotifyEachView
            // 
            this.chkBoxNotifyEachView.AutoSize = true;
            this.chkBoxNotifyEachView.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkBoxNotifyEachView.Location = new System.Drawing.Point(10, 363);
            this.chkBoxNotifyEachView.Name = "chkBoxNotifyEachView";
            this.chkBoxNotifyEachView.Size = new System.Drawing.Size(229, 20);
            this.chkBoxNotifyEachView.TabIndex = 8;
            this.chkBoxNotifyEachView.Text = "Send Notifications After Each View";
            this.chkBoxNotifyEachView.UseVisualStyleBackColor = true;
            // 
            // DialogWFUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 466);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DialogWFUsers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormWFUsers";
            this.Load += new System.EventHandler(this.DialogWFUsers_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lBoxSelectedUsers;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListBox lBoxUsers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.CheckBox chkBoxNotifyEachView;
        private System.Windows.Forms.CheckBox chkBoxNotifyDone;
    }
}