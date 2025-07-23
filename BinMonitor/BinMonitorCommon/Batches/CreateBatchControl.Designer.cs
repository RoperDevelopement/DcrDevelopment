namespace BinMonitor.Common
{
    partial class CreateBatchControl
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
            this.txtComments = new System.Windows.Forms.TextBox();
            this.lblComments = new System.Windows.Forms.Label();
            this.txtBinId = new System.Windows.Forms.TextBox();
            this.lblBinId = new System.Windows.Forms.Label();
            this.btnAddSpecimenToBatch = new System.Windows.Forms.Button();
            this.txtAddSpecimenToBatch = new System.Windows.Forms.TextBox();
            this.lbBatchContents = new System.Windows.Forms.ListBox();
            this.lblBatchContents = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.lblBatchCategory = new System.Windows.Forms.Label();
            this.grpWorkflow = new System.Windows.Forms.GroupBox();
            this.wfcfgProcess = new BinMonitor.Common.WorkflowStepConfigurationControl();
            this.wfcfgRegister = new BinMonitor.Common.WorkflowStepConfigurationControl();
            this.wfcfgCreate = new BinMonitor.Common.WorkflowStepConfigurationControl();
            this.gpbBatchDetails = new System.Windows.Forms.GroupBox();
            this.cmbUsersForProcessing = new BinMonitor.Common.UsersComboBox();
            this.cmbUsersForRegistration = new BinMonitor.Common.UsersComboBox();
            this.cmbCategories = new BinMonitor.Common.CategoriesComboBox();
            this.lblAssignProcessing = new System.Windows.Forms.Label();
            this.chkRequiresRegistration = new System.Windows.Forms.CheckBox();
            this.lblAssignRegistrationTo = new System.Windows.Forms.Label();
            this.btnTransferFrom = new System.Windows.Forms.Button();
            this.txtTransferredFromBatchId = new System.Windows.Forms.TextBox();
            this.lblTransferredFrom = new System.Windows.Forms.Label();
            this.btnRemoveSelectedSpecimens = new System.Windows.Forms.Button();
            this.grpCheckpoints = new System.Windows.Forms.GroupBox();
            this.lblCheckpointOrigin = new System.Windows.Forms.Label();
            this.dtpCheckpointOrigin = new System.Windows.Forms.DateTimePicker();
            this.checkpointConfigurationControl4 = new BinMonitor.Common.CheckpointConfigurationControl();
            this.checkpointConfigurationControl3 = new BinMonitor.Common.CheckpointConfigurationControl();
            this.checkpointConfigurationControl2 = new BinMonitor.Common.CheckpointConfigurationControl();
            this.checkpointConfigurationControl1 = new BinMonitor.Common.CheckpointConfigurationControl();
            this.pnlScroll = new System.Windows.Forms.Panel();
            this.pnlControl = new System.Windows.Forms.Panel();
            this.ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpWorkflow.SuspendLayout();
            this.gpbBatchDetails.SuspendLayout();
            this.grpCheckpoints.SuspendLayout();
            this.pnlScroll.SuspendLayout();
            this.pnlControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // txtComments
            // 
            this.txtComments.Location = new System.Drawing.Point(263, 155);
            this.txtComments.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.Size = new System.Drawing.Size(260, 42);
            this.txtComments.TabIndex = 4;
            // 
            // lblComments
            // 
            this.lblComments.AutoSize = true;
            this.lblComments.Location = new System.Drawing.Point(176, 159);
            this.lblComments.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblComments.Name = "lblComments";
            this.lblComments.Size = new System.Drawing.Size(74, 17);
            this.lblComments.TabIndex = 15;
            this.lblComments.Text = "Comments";
            this.lblComments.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtBinId
            // 
            this.txtBinId.Location = new System.Drawing.Point(263, 94);
            this.txtBinId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBinId.Name = "txtBinId";
            this.txtBinId.Size = new System.Drawing.Size(100, 22);
            this.txtBinId.TabIndex = 1;
            // 
            // lblBinId
            // 
            this.lblBinId.AutoSize = true;
            this.lblBinId.Location = new System.Drawing.Point(203, 97);
            this.lblBinId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBinId.Name = "lblBinId";
            this.lblBinId.Size = new System.Drawing.Size(45, 17);
            this.lblBinId.TabIndex = 8;
            this.lblBinId.Text = "Bin ID";
            this.lblBinId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnAddSpecimenToBatch
            // 
            this.btnAddSpecimenToBatch.Location = new System.Drawing.Point(535, 55);
            this.btnAddSpecimenToBatch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddSpecimenToBatch.Name = "btnAddSpecimenToBatch";
            this.btnAddSpecimenToBatch.Size = new System.Drawing.Size(87, 28);
            this.btnAddSpecimenToBatch.TabIndex = 7;
            this.btnAddSpecimenToBatch.TabStop = false;
            this.btnAddSpecimenToBatch.Text = "Add";
            this.btnAddSpecimenToBatch.UseVisualStyleBackColor = true;
            this.btnAddSpecimenToBatch.Click += new System.EventHandler(this.btnAddSpecimenToBatch_Click);
            // 
            // txtAddSpecimenToBatch
            // 
            this.txtAddSpecimenToBatch.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAddSpecimenToBatch.Location = new System.Drawing.Point(535, 26);
            this.txtAddSpecimenToBatch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtAddSpecimenToBatch.Name = "txtAddSpecimenToBatch";
            this.txtAddSpecimenToBatch.Size = new System.Drawing.Size(181, 22);
            this.txtAddSpecimenToBatch.TabIndex = 3;
            this.txtAddSpecimenToBatch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAddSpecimenToBatch_KeyPress);
            // 
            // lbBatchContents
            // 
            this.lbBatchContents.FormattingEnabled = true;
            this.lbBatchContents.ItemHeight = 16;
            this.lbBatchContents.Location = new System.Drawing.Point(535, 91);
            this.lbBatchContents.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbBatchContents.Name = "lbBatchContents";
            this.lbBatchContents.Size = new System.Drawing.Size(181, 196);
            this.lbBatchContents.TabIndex = 5;
            this.lbBatchContents.TabStop = false;
            // 
            // lblBatchContents
            // 
            this.lblBatchContents.AutoSize = true;
            this.lblBatchContents.Location = new System.Drawing.Point(461, 30);
            this.lblBatchContents.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBatchContents.Name = "lblBatchContents";
            this.lblBatchContents.Size = new System.Drawing.Size(64, 17);
            this.lblBatchContents.TabIndex = 4;
            this.lblBatchContents.Text = "Contents";
            this.lblBatchContents.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(428, 5);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 62);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "&Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(320, 5);
            this.btnCreate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(100, 62);
            this.btnCreate.TabIndex = 8;
            this.btnCreate.Text = "&Ok\r";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // lblBatchCategory
            // 
            this.lblBatchCategory.AutoSize = true;
            this.lblBatchCategory.Location = new System.Drawing.Point(185, 129);
            this.lblBatchCategory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBatchCategory.Name = "lblBatchCategory";
            this.lblBatchCategory.Size = new System.Drawing.Size(65, 17);
            this.lblBatchCategory.TabIndex = 2;
            this.lblBatchCategory.Text = "Category";
            this.lblBatchCategory.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // grpWorkflow
            // 
            this.grpWorkflow.Controls.Add(this.wfcfgProcess);
            this.grpWorkflow.Controls.Add(this.wfcfgRegister);
            this.grpWorkflow.Controls.Add(this.wfcfgCreate);
            this.grpWorkflow.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpWorkflow.Location = new System.Drawing.Point(0, 370);
            this.grpWorkflow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpWorkflow.Name = "grpWorkflow";
            this.grpWorkflow.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpWorkflow.Size = new System.Drawing.Size(848, 203);
            this.grpWorkflow.TabIndex = 15;
            this.grpWorkflow.TabStop = false;
            this.grpWorkflow.Text = "Workflow";
            // 
            // wfcfgProcess
            // 
            this.wfcfgProcess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wfcfgProcess.Caption = "Process";
            this.wfcfgProcess.Location = new System.Drawing.Point(523, 15);
            this.wfcfgProcess.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.wfcfgProcess.Name = "wfcfgProcess";
            this.wfcfgProcess.Size = new System.Drawing.Size(178, 180);
            this.wfcfgProcess.TabIndex = 2;
            this.wfcfgProcess.TabStop = false;
            // 
            // wfcfgRegister
            // 
            this.wfcfgRegister.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wfcfgRegister.Caption = "Register";
            this.wfcfgRegister.Location = new System.Drawing.Point(336, 15);
            this.wfcfgRegister.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.wfcfgRegister.Name = "wfcfgRegister";
            this.wfcfgRegister.Size = new System.Drawing.Size(178, 180);
            this.wfcfgRegister.TabIndex = 1;
            this.wfcfgRegister.TabStop = false;
            // 
            // wfcfgCreate
            // 
            this.wfcfgCreate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wfcfgCreate.Caption = "Create";
            this.wfcfgCreate.Location = new System.Drawing.Point(148, 15);
            this.wfcfgCreate.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.wfcfgCreate.Name = "wfcfgCreate";
            this.wfcfgCreate.Size = new System.Drawing.Size(178, 180);
            this.wfcfgCreate.TabIndex = 0;
            this.wfcfgCreate.TabStop = false;
            // 
            // gpbBatchDetails
            // 
            this.gpbBatchDetails.Controls.Add(this.cmbUsersForProcessing);
            this.gpbBatchDetails.Controls.Add(this.cmbUsersForRegistration);
            this.gpbBatchDetails.Controls.Add(this.cmbCategories);
            this.gpbBatchDetails.Controls.Add(this.lblAssignProcessing);
            this.gpbBatchDetails.Controls.Add(this.chkRequiresRegistration);
            this.gpbBatchDetails.Controls.Add(this.lblAssignRegistrationTo);
            this.gpbBatchDetails.Controls.Add(this.btnTransferFrom);
            this.gpbBatchDetails.Controls.Add(this.txtTransferredFromBatchId);
            this.gpbBatchDetails.Controls.Add(this.lblTransferredFrom);
            this.gpbBatchDetails.Controls.Add(this.btnRemoveSelectedSpecimens);
            this.gpbBatchDetails.Controls.Add(this.txtComments);
            this.gpbBatchDetails.Controls.Add(this.lblComments);
            this.gpbBatchDetails.Controls.Add(this.txtBinId);
            this.gpbBatchDetails.Controls.Add(this.lblBinId);
            this.gpbBatchDetails.Controls.Add(this.btnAddSpecimenToBatch);
            this.gpbBatchDetails.Controls.Add(this.txtAddSpecimenToBatch);
            this.gpbBatchDetails.Controls.Add(this.lbBatchContents);
            this.gpbBatchDetails.Controls.Add(this.lblBatchContents);
            this.gpbBatchDetails.Controls.Add(this.lblBatchCategory);
            this.gpbBatchDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpbBatchDetails.Location = new System.Drawing.Point(0, 0);
            this.gpbBatchDetails.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gpbBatchDetails.Name = "gpbBatchDetails";
            this.gpbBatchDetails.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gpbBatchDetails.Size = new System.Drawing.Size(848, 299);
            this.gpbBatchDetails.TabIndex = 11;
            this.gpbBatchDetails.TabStop = false;
            this.gpbBatchDetails.Text = "Batch Details";
            // 
            // cmbUsersForProcessing
            // 
            this.cmbUsersForProcessing.Location = new System.Drawing.Point(264, 260);
            this.cmbUsersForProcessing.Margin = new System.Windows.Forms.Padding(0);
            this.cmbUsersForProcessing.Name = "cmbUsersForProcessing";
            this.cmbUsersForProcessing.Size = new System.Drawing.Size(260, 26);
            this.cmbUsersForProcessing.TabIndex = 7;
            // 
            // cmbUsersForRegistration
            // 
            this.cmbUsersForRegistration.Enabled = false;
            this.cmbUsersForRegistration.Location = new System.Drawing.Point(265, 223);
            this.cmbUsersForRegistration.Margin = new System.Windows.Forms.Padding(0);
            this.cmbUsersForRegistration.Name = "cmbUsersForRegistration";
            this.cmbUsersForRegistration.Size = new System.Drawing.Size(260, 26);
            this.cmbUsersForRegistration.TabIndex = 6;
            // 
            // cmbCategories
            // 
            this.cmbCategories.Location = new System.Drawing.Point(264, 126);
            this.cmbCategories.Margin = new System.Windows.Forms.Padding(0);
            this.cmbCategories.Name = "cmbCategories";
            this.cmbCategories.Size = new System.Drawing.Size(260, 26);
            this.cmbCategories.TabIndex = 2;
            this.cmbCategories.SelectedKeyChanged += new System.EventHandler(this.cmbCategories_SelectedKeyChanged);
            // 
            // lblAssignProcessing
            // 
            this.lblAssignProcessing.AutoSize = true;
            this.lblAssignProcessing.Location = new System.Drawing.Point(128, 265);
            this.lblAssignProcessing.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAssignProcessing.Name = "lblAssignProcessing";
            this.lblAssignProcessing.Size = new System.Drawing.Size(124, 17);
            this.lblAssignProcessing.TabIndex = 26;
            this.lblAssignProcessing.Text = "Assign Processing";
            this.lblAssignProcessing.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkRequiresRegistration
            // 
            this.chkRequiresRegistration.AutoSize = true;
            this.chkRequiresRegistration.Location = new System.Drawing.Point(265, 198);
            this.chkRequiresRegistration.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkRequiresRegistration.Name = "chkRequiresRegistration";
            this.chkRequiresRegistration.Size = new System.Drawing.Size(167, 21);
            this.chkRequiresRegistration.TabIndex = 5;
            this.chkRequiresRegistration.Text = "Requires Registration";
            this.chkRequiresRegistration.UseVisualStyleBackColor = true;
            this.chkRequiresRegistration.CheckedChanged += new System.EventHandler(this.chkSkipRegistration_CheckedChanged);
            // 
            // lblAssignRegistrationTo
            // 
            this.lblAssignRegistrationTo.AutoSize = true;
            this.lblAssignRegistrationTo.Location = new System.Drawing.Point(127, 228);
            this.lblAssignRegistrationTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAssignRegistrationTo.Name = "lblAssignRegistrationTo";
            this.lblAssignRegistrationTo.Size = new System.Drawing.Size(130, 17);
            this.lblAssignRegistrationTo.TabIndex = 22;
            this.lblAssignRegistrationTo.Text = "Assign Registration";
            this.lblAssignRegistrationTo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnTransferFrom
            // 
            this.btnTransferFrom.Location = new System.Drawing.Point(263, 23);
            this.btnTransferFrom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTransferFrom.Name = "btnTransferFrom";
            this.btnTransferFrom.Size = new System.Drawing.Size(191, 28);
            this.btnTransferFrom.TabIndex = 21;
            this.btnTransferFrom.TabStop = false;
            this.btnTransferFrom.Text = "Transfer From";
            this.btnTransferFrom.UseVisualStyleBackColor = true;
            this.btnTransferFrom.Click += new System.EventHandler(this.btnTransferredFromLookup_Click);
            // 
            // txtTransferredFromBatchId
            // 
            this.txtTransferredFromBatchId.Location = new System.Drawing.Point(263, 58);
            this.txtTransferredFromBatchId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTransferredFromBatchId.Name = "txtTransferredFromBatchId";
            this.txtTransferredFromBatchId.ReadOnly = true;
            this.txtTransferredFromBatchId.Size = new System.Drawing.Size(187, 22);
            this.txtTransferredFromBatchId.TabIndex = 20;
            this.txtTransferredFromBatchId.TabStop = false;
            // 
            // lblTransferredFrom
            // 
            this.lblTransferredFrom.Location = new System.Drawing.Point(113, 62);
            this.lblTransferredFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTransferredFrom.Name = "lblTransferredFrom";
            this.lblTransferredFrom.Size = new System.Drawing.Size(139, 25);
            this.lblTransferredFrom.TabIndex = 18;
            this.lblTransferredFrom.Text = "Transferred From";
            this.lblTransferredFrom.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnRemoveSelectedSpecimens
            // 
            this.btnRemoveSelectedSpecimens.Location = new System.Drawing.Point(636, 55);
            this.btnRemoveSelectedSpecimens.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRemoveSelectedSpecimens.Name = "btnRemoveSelectedSpecimens";
            this.btnRemoveSelectedSpecimens.Size = new System.Drawing.Size(87, 28);
            this.btnRemoveSelectedSpecimens.TabIndex = 17;
            this.btnRemoveSelectedSpecimens.TabStop = false;
            this.btnRemoveSelectedSpecimens.Text = "Remove";
            this.btnRemoveSelectedSpecimens.UseVisualStyleBackColor = true;
            this.btnRemoveSelectedSpecimens.Click += new System.EventHandler(this.btnRemoveSelectedSpecimens_Click);
            // 
            // grpCheckpoints
            // 
            this.grpCheckpoints.Controls.Add(this.lblCheckpointOrigin);
            this.grpCheckpoints.Controls.Add(this.dtpCheckpointOrigin);
            this.grpCheckpoints.Controls.Add(this.checkpointConfigurationControl4);
            this.grpCheckpoints.Controls.Add(this.checkpointConfigurationControl3);
            this.grpCheckpoints.Controls.Add(this.checkpointConfigurationControl2);
            this.grpCheckpoints.Controls.Add(this.checkpointConfigurationControl1);
            this.grpCheckpoints.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCheckpoints.Location = new System.Drawing.Point(0, 573);
            this.grpCheckpoints.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpCheckpoints.Name = "grpCheckpoints";
            this.grpCheckpoints.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpCheckpoints.Size = new System.Drawing.Size(848, 290);
            this.grpCheckpoints.TabIndex = 14;
            this.grpCheckpoints.TabStop = false;
            this.grpCheckpoints.Text = "Checkpoints";
            // 
            // lblCheckpointOrigin
            // 
            this.lblCheckpointOrigin.AutoSize = true;
            this.lblCheckpointOrigin.Location = new System.Drawing.Point(8, 36);
            this.lblCheckpointOrigin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCheckpointOrigin.Name = "lblCheckpointOrigin";
            this.lblCheckpointOrigin.Size = new System.Drawing.Size(46, 17);
            this.lblCheckpointOrigin.TabIndex = 33;
            this.lblCheckpointOrigin.Text = "Origin";
            // 
            // dtpCheckpointOrigin
            // 
            this.dtpCheckpointOrigin.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            this.dtpCheckpointOrigin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpCheckpointOrigin.Location = new System.Drawing.Point(59, 28);
            this.dtpCheckpointOrigin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpCheckpointOrigin.Name = "dtpCheckpointOrigin";
            this.dtpCheckpointOrigin.Size = new System.Drawing.Size(189, 22);
            this.dtpCheckpointOrigin.TabIndex = 32;
            this.dtpCheckpointOrigin.TabStop = false;
            // 
            // checkpointConfigurationControl4
            // 
            this.checkpointConfigurationControl4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.checkpointConfigurationControl4.Caption = "4";
            this.checkpointConfigurationControl4.Location = new System.Drawing.Point(636, 55);
            this.checkpointConfigurationControl4.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.checkpointConfigurationControl4.Name = "checkpointConfigurationControl4";
            this.checkpointConfigurationControl4.Size = new System.Drawing.Size(199, 227);
            this.checkpointConfigurationControl4.TabIndex = 11;
            this.checkpointConfigurationControl4.TabStop = false;
            // 
            // checkpointConfigurationControl3
            // 
            this.checkpointConfigurationControl3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.checkpointConfigurationControl3.Caption = "3";
            this.checkpointConfigurationControl3.Location = new System.Drawing.Point(428, 55);
            this.checkpointConfigurationControl3.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.checkpointConfigurationControl3.Name = "checkpointConfigurationControl3";
            this.checkpointConfigurationControl3.Size = new System.Drawing.Size(199, 227);
            this.checkpointConfigurationControl3.TabIndex = 2;
            this.checkpointConfigurationControl3.TabStop = false;
            // 
            // checkpointConfigurationControl2
            // 
            this.checkpointConfigurationControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.checkpointConfigurationControl2.Caption = "2";
            this.checkpointConfigurationControl2.Location = new System.Drawing.Point(220, 55);
            this.checkpointConfigurationControl2.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.checkpointConfigurationControl2.Name = "checkpointConfigurationControl2";
            this.checkpointConfigurationControl2.Size = new System.Drawing.Size(199, 227);
            this.checkpointConfigurationControl2.TabIndex = 1;
            this.checkpointConfigurationControl2.TabStop = false;
            // 
            // checkpointConfigurationControl1
            // 
            this.checkpointConfigurationControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.checkpointConfigurationControl1.Caption = "1";
            this.checkpointConfigurationControl1.Location = new System.Drawing.Point(12, 55);
            this.checkpointConfigurationControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.checkpointConfigurationControl1.Name = "checkpointConfigurationControl1";
            this.checkpointConfigurationControl1.Size = new System.Drawing.Size(199, 227);
            this.checkpointConfigurationControl1.TabIndex = 0;
            this.checkpointConfigurationControl1.TabStop = false;
            // 
            // pnlScroll
            // 
            this.pnlScroll.Controls.Add(this.grpCheckpoints);
            this.pnlScroll.Controls.Add(this.grpWorkflow);
            this.pnlScroll.Controls.Add(this.pnlControl);
            this.pnlScroll.Controls.Add(this.gpbBatchDetails);
            this.pnlScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScroll.Location = new System.Drawing.Point(0, 0);
            this.pnlScroll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlScroll.Name = "pnlScroll";
            this.pnlScroll.Size = new System.Drawing.Size(848, 908);
            this.pnlScroll.TabIndex = 1;
            // 
            // pnlControl
            // 
            this.pnlControl.Controls.Add(this.btnCreate);
            this.pnlControl.Controls.Add(this.btnClear);
            this.pnlControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControl.Location = new System.Drawing.Point(0, 299);
            this.pnlControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlControl.Name = "pnlControl";
            this.pnlControl.Size = new System.Drawing.Size(848, 71);
            this.pnlControl.TabIndex = 16;
            // 
            // ErrorProvider
            // 
            this.ErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.ErrorProvider.ContainerControl = this;
            // 
            // CreateBatchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlScroll);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CreateBatchControl";
            this.Size = new System.Drawing.Size(848, 908);
            this.grpWorkflow.ResumeLayout(false);
            this.gpbBatchDetails.ResumeLayout(false);
            this.gpbBatchDetails.PerformLayout();
            this.grpCheckpoints.ResumeLayout(false);
            this.grpCheckpoints.PerformLayout();
            this.pnlScroll.ResumeLayout(false);
            this.pnlControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CheckpointConfigurationControl checkpointConfigurationControl4;
        private CheckpointConfigurationControl checkpointConfigurationControl3;
        private CheckpointConfigurationControl checkpointConfigurationControl1;
        private WorkflowStepConfigurationControl wfcfgCreate;
        private System.Windows.Forms.TextBox txtComments;
        private System.Windows.Forms.Label lblComments;
        private System.Windows.Forms.TextBox txtBinId;
        private System.Windows.Forms.Label lblBinId;
        private System.Windows.Forms.Button btnAddSpecimenToBatch;
        private System.Windows.Forms.TextBox txtAddSpecimenToBatch;
        private System.Windows.Forms.ListBox lbBatchContents;
        private System.Windows.Forms.Label lblBatchContents;
        private WorkflowStepConfigurationControl wfcfgProcess;
        private WorkflowStepConfigurationControl wfcfgRegister;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label lblBatchCategory;
        private CheckpointConfigurationControl checkpointConfigurationControl2;
        private System.Windows.Forms.GroupBox grpWorkflow;
        private System.Windows.Forms.GroupBox gpbBatchDetails;
        private System.Windows.Forms.GroupBox grpCheckpoints;
        private System.Windows.Forms.Panel pnlScroll;
        private System.Windows.Forms.ErrorProvider ErrorProvider;
        private System.Windows.Forms.Button btnTransferFrom;
        private System.Windows.Forms.TextBox txtTransferredFromBatchId;
        private System.Windows.Forms.Label lblTransferredFrom;
        private System.Windows.Forms.Button btnRemoveSelectedSpecimens;
        private System.Windows.Forms.Label lblAssignProcessing;
        private System.Windows.Forms.CheckBox chkRequiresRegistration;
        private System.Windows.Forms.Label lblAssignRegistrationTo;
        private UsersComboBox cmbUsersForProcessing;
        private UsersComboBox cmbUsersForRegistration;
        private CategoriesComboBox cmbCategories;
        private System.Windows.Forms.Panel pnlControl;
        private System.Windows.Forms.Label lblCheckpointOrigin;
        private System.Windows.Forms.DateTimePicker dtpCheckpointOrigin;
    }
}
