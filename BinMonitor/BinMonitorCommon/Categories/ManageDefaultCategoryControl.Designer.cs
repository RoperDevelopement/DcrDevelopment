namespace BinMonitor.Common
{
    partial class ManageDefaultCategoryControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grpCheckpoints = new System.Windows.Forms.GroupBox();
            this.checkpointConfigurationControl4 = new BinMonitor.Common.CheckpointConfigurationControl();
            this.checkpointConfigurationControl3 = new BinMonitor.Common.CheckpointConfigurationControl();
            this.checkpointConfigurationControl2 = new BinMonitor.Common.CheckpointConfigurationControl();
            this.checkpointConfigurationControl1 = new BinMonitor.Common.CheckpointConfigurationControl();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSaveChanges = new System.Windows.Forms.Button();
            this.lblCategoryColor = new System.Windows.Forms.Label();
            this.brnChangeColor = new System.Windows.Forms.Button();
            this.CategoryColorDialog = new System.Windows.Forms.ColorDialog();
            this.pnlScroll = new System.Windows.Forms.Panel();
            this.grpWorkflow = new System.Windows.Forms.GroupBox();
            this.wfcfgProcess = new BinMonitor.Common.WorkflowStepConfigurationControl();
            this.wfcfgRegister = new BinMonitor.Common.WorkflowStepConfigurationControl();
            this.wfcfgCreate = new BinMonitor.Common.WorkflowStepConfigurationControl();
            this.cmbCategories = new BinMonitor.Common.CategoriesComboBox();
            this.ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpCheckpoints.SuspendLayout();
            this.grpSettings.SuspendLayout();
            this.pnlScroll.SuspendLayout();
            this.grpWorkflow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // grpCheckpoints
            // 
            this.grpCheckpoints.Controls.Add(this.checkpointConfigurationControl4);
            this.grpCheckpoints.Controls.Add(this.checkpointConfigurationControl3);
            this.grpCheckpoints.Controls.Add(this.checkpointConfigurationControl2);
            this.grpCheckpoints.Controls.Add(this.checkpointConfigurationControl1);
            this.grpCheckpoints.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCheckpoints.Location = new System.Drawing.Point(0, 327);
            this.grpCheckpoints.Name = "grpCheckpoints";
            this.grpCheckpoints.Size = new System.Drawing.Size(633, 409);
            this.grpCheckpoints.TabIndex = 15;
            this.grpCheckpoints.TabStop = false;
            this.grpCheckpoints.Text = "Checkpoints";
            // 
            // checkpointConfigurationControl4
            // 
            this.checkpointConfigurationControl4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.checkpointConfigurationControl4.Caption = "4";
            this.checkpointConfigurationControl4.Location = new System.Drawing.Point(475, 18);
            this.checkpointConfigurationControl4.Name = "checkpointConfigurationControl4";
            this.checkpointConfigurationControl4.Size = new System.Drawing.Size(150, 185);
            this.checkpointConfigurationControl4.TabIndex = 10;
            this.checkpointConfigurationControl4.TabStop = false;
            // 
            // checkpointConfigurationControl3
            // 
            this.checkpointConfigurationControl3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.checkpointConfigurationControl3.Caption = "3";
            this.checkpointConfigurationControl3.Location = new System.Drawing.Point(319, 19);
            this.checkpointConfigurationControl3.Name = "checkpointConfigurationControl3";
            this.checkpointConfigurationControl3.Size = new System.Drawing.Size(150, 185);
            this.checkpointConfigurationControl3.TabIndex = 9;
            this.checkpointConfigurationControl3.TabStop = false;
            // 
            // checkpointConfigurationControl2
            // 
            this.checkpointConfigurationControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.checkpointConfigurationControl2.Caption = "2";
            this.checkpointConfigurationControl2.Location = new System.Drawing.Point(163, 19);
            this.checkpointConfigurationControl2.Name = "checkpointConfigurationControl2";
            this.checkpointConfigurationControl2.Size = new System.Drawing.Size(150, 185);
            this.checkpointConfigurationControl2.TabIndex = 8;
            this.checkpointConfigurationControl2.TabStop = false;
            // 
            // checkpointConfigurationControl1
            // 
            this.checkpointConfigurationControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.checkpointConfigurationControl1.Caption = "1";
            this.checkpointConfigurationControl1.Location = new System.Drawing.Point(7, 18);
            this.checkpointConfigurationControl1.Name = "checkpointConfigurationControl1";
            this.checkpointConfigurationControl1.Size = new System.Drawing.Size(150, 185);
            this.checkpointConfigurationControl1.TabIndex = 7;
            this.checkpointConfigurationControl1.TabStop = false;
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.btnReset);
            this.grpSettings.Controls.Add(this.btnSaveChanges);
            this.grpSettings.Controls.Add(this.lblCategoryColor);
            this.grpSettings.Controls.Add(this.brnChangeColor);
            this.grpSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSettings.Location = new System.Drawing.Point(0, 21);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(633, 134);
            this.grpSettings.TabIndex = 16;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Settings";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(320, 48);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 75);
            this.btnReset.TabIndex = 3;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSaveChanges
            // 
            this.btnSaveChanges.Location = new System.Drawing.Point(241, 48);
            this.btnSaveChanges.Name = "btnSaveChanges";
            this.btnSaveChanges.Size = new System.Drawing.Size(73, 75);
            this.btnSaveChanges.TabIndex = 2;
            this.btnSaveChanges.Text = "Save Changes";
            this.btnSaveChanges.UseVisualStyleBackColor = true;
            this.btnSaveChanges.Click += new System.EventHandler(this.btnSaveChanges_Click);
            // 
            // lblCategoryColor
            // 
            this.lblCategoryColor.AutoSize = true;
            this.lblCategoryColor.Location = new System.Drawing.Point(238, 24);
            this.lblCategoryColor.Name = "lblCategoryColor";
            this.lblCategoryColor.Size = new System.Drawing.Size(76, 13);
            this.lblCategoryColor.TabIndex = 20;
            this.lblCategoryColor.Text = "Category Color";
            // 
            // brnChangeColor
            // 
            this.brnChangeColor.AutoSize = true;
            this.brnChangeColor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.brnChangeColor.Location = new System.Drawing.Point(321, 19);
            this.brnChangeColor.Name = "brnChangeColor";
            this.brnChangeColor.Size = new System.Drawing.Size(26, 23);
            this.brnChangeColor.TabIndex = 1;
            this.brnChangeColor.Text = "...";
            this.brnChangeColor.UseVisualStyleBackColor = true;
            this.brnChangeColor.Click += new System.EventHandler(this.brnChangeColor_Click);
            // 
            // pnlScroll
            // 
            this.pnlScroll.Controls.Add(this.grpCheckpoints);
            this.pnlScroll.Controls.Add(this.grpWorkflow);
            this.pnlScroll.Controls.Add(this.grpSettings);
            this.pnlScroll.Controls.Add(this.cmbCategories);
            this.pnlScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScroll.Location = new System.Drawing.Point(0, 0);
            this.pnlScroll.Name = "pnlScroll";
            this.pnlScroll.Size = new System.Drawing.Size(633, 550);
            this.pnlScroll.TabIndex = 18;
            // 
            // grpWorkflow
            // 
            this.grpWorkflow.Controls.Add(this.wfcfgProcess);
            this.grpWorkflow.Controls.Add(this.wfcfgRegister);
            this.grpWorkflow.Controls.Add(this.wfcfgCreate);
            this.grpWorkflow.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpWorkflow.Location = new System.Drawing.Point(0, 155);
            this.grpWorkflow.Name = "grpWorkflow";
            this.grpWorkflow.Size = new System.Drawing.Size(633, 172);
            this.grpWorkflow.TabIndex = 16;
            this.grpWorkflow.TabStop = false;
            this.grpWorkflow.Text = "Workflow";
            this.grpWorkflow.Visible = false;
            // 
            // wfcfgProcess
            // 
            this.wfcfgProcess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wfcfgProcess.Caption = "Process";
            this.wfcfgProcess.Location = new System.Drawing.Point(389, 19);
            this.wfcfgProcess.Name = "wfcfgProcess";
            this.wfcfgProcess.Size = new System.Drawing.Size(134, 147);
            this.wfcfgProcess.TabIndex = 6;
            this.wfcfgProcess.TabStop = false;
            // 
            // wfcfgRegister
            // 
            this.wfcfgRegister.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wfcfgRegister.Caption = "Register";
            this.wfcfgRegister.Location = new System.Drawing.Point(249, 19);
            this.wfcfgRegister.Name = "wfcfgRegister";
            this.wfcfgRegister.Size = new System.Drawing.Size(134, 147);
            this.wfcfgRegister.TabIndex = 5;
            this.wfcfgRegister.TabStop = false;
            // 
            // wfcfgCreate
            // 
            this.wfcfgCreate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wfcfgCreate.Caption = "Create";
            this.wfcfgCreate.Location = new System.Drawing.Point(109, 19);
            this.wfcfgCreate.Name = "wfcfgCreate";
            this.wfcfgCreate.Size = new System.Drawing.Size(134, 147);
            this.wfcfgCreate.TabIndex = 4;
            this.wfcfgCreate.TabStop = false;
            // 
            // cmbCategories
            // 
            this.cmbCategories.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbCategories.Location = new System.Drawing.Point(0, 0);
            this.cmbCategories.Margin = new System.Windows.Forms.Padding(0);
            this.cmbCategories.Name = "cmbCategories";
            this.cmbCategories.Size = new System.Drawing.Size(633, 21);
            this.cmbCategories.TabIndex = 0;
            this.cmbCategories.SelectedKeyChanged += new System.EventHandler(this.cmbCategories_SelectedKeyChanged);
            // 
            // ErrorProvider
            // 
            this.ErrorProvider.ContainerControl = this;
            // 
            // ManageDefaultCategoryControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlScroll);
            this.Name = "ManageDefaultCategoryControl";
            this.Size = new System.Drawing.Size(633, 550);
            this.grpCheckpoints.ResumeLayout(false);
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.pnlScroll.ResumeLayout(false);
            this.grpWorkflow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpCheckpoints;
        private CheckpointConfigurationControl checkpointConfigurationControl4;
        private CheckpointConfigurationControl checkpointConfigurationControl3;
        private CheckpointConfigurationControl checkpointConfigurationControl2;
        private CheckpointConfigurationControl checkpointConfigurationControl1;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.Button brnChangeColor;
        private System.Windows.Forms.ColorDialog CategoryColorDialog;
        private System.Windows.Forms.Label lblCategoryColor;
        private System.Windows.Forms.Panel pnlScroll;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSaveChanges;
        private System.Windows.Forms.ErrorProvider ErrorProvider;
        private System.Windows.Forms.GroupBox grpWorkflow;
        private WorkflowStepConfigurationControl wfcfgProcess;
        private WorkflowStepConfigurationControl wfcfgRegister;
        private WorkflowStepConfigurationControl wfcfgCreate;
        private CategoriesComboBox cmbCategories;
    }
}
