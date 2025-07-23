
namespace EdocsUSA.Utilities.EdocsWia
{
    partial class WIA_Settings_Dialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WIA_Settings_Dialog));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtBoxLength = new System.Windows.Forms.TextBox();
            this.txtBoxWidth = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtBoxTop = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtBoxLeft = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cBoxPaperSize = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tBoxContrast = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tBarContrast = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.cBoxColor = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBoxBrightness = new System.Windows.Forms.TextBox();
            this.TBarBrightness = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.cBoxScannerType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numCustomDPI = new System.Windows.Forms.NumericUpDown();
            this.cBoxDPI = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.imageViewer1 = new EdocsUSA.Controls.ImageViewer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tBarContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TBarBrightness)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCustomDPI)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel1.Controls.Add(this.cBoxPaperSize);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.cBoxScannerType);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1142, 529);
            this.splitContainer1.SplitterDistance = 397;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtBoxLength);
            this.groupBox3.Controls.Add(this.txtBoxWidth);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.txtBoxTop);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.txtBoxLeft);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(12, 305);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(295, 100);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Scanning Area[inches]";
            // 
            // txtBoxLength
            // 
            this.txtBoxLength.Location = new System.Drawing.Point(177, 60);
            this.txtBoxLength.MaxLength = 5;
            this.txtBoxLength.Name = "txtBoxLength";
            this.txtBoxLength.Size = new System.Drawing.Size(69, 20);
            this.txtBoxLength.TabIndex = 12;
            this.txtBoxLength.Leave += new System.EventHandler(this.txtBoxLength_Leave);
            // 
            // txtBoxWidth
            // 
            this.txtBoxWidth.Location = new System.Drawing.Point(50, 56);
            this.txtBoxWidth.MaxLength = 4;
            this.txtBoxWidth.Name = "txtBoxWidth";
            this.txtBoxWidth.Size = new System.Drawing.Size(50, 20);
            this.txtBoxWidth.TabIndex = 11;
            this.txtBoxWidth.Leave += new System.EventHandler(this.txtBoxWidth_Leave);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 60);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "Width:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(128, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "Height :";
            // 
            // txtBoxTop
            // 
            this.txtBoxTop.Location = new System.Drawing.Point(162, 28);
            this.txtBoxTop.Name = "txtBoxTop";
            this.txtBoxTop.Size = new System.Drawing.Size(69, 20);
            this.txtBoxTop.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(128, 32);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Top:";
            // 
            // txtBoxLeft
            // 
            this.txtBoxLeft.Location = new System.Drawing.Point(40, 28);
            this.txtBoxLeft.Name = "txtBoxLeft";
            this.txtBoxLeft.Size = new System.Drawing.Size(69, 20);
            this.txtBoxLeft.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(28, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Left:";
            // 
            // cBoxPaperSize
            // 
            this.cBoxPaperSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxPaperSize.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cBoxPaperSize.FormattingEnabled = true;
            this.cBoxPaperSize.Location = new System.Drawing.Point(174, 105);
            this.cBoxPaperSize.Name = "cBoxPaperSize";
            this.cBoxPaperSize.Size = new System.Drawing.Size(203, 21);
            this.cBoxPaperSize.TabIndex = 7;
            this.cBoxPaperSize.SelectedIndexChanged += new System.EventHandler(this.cBoxPaperSize_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tBoxContrast);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tBarContrast);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cBoxColor);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtBoxBrightness);
            this.groupBox2.Controls.Add(this.TBarBrightness);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox2.Location = new System.Drawing.Point(12, 150);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(353, 149);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Front";
            // 
            // tBoxContrast
            // 
            this.tBoxContrast.Location = new System.Drawing.Point(246, 105);
            this.tBoxContrast.MaxLength = 3;
            this.tBoxContrast.Name = "tBoxContrast";
            this.tBoxContrast.Size = new System.Drawing.Size(69, 20);
            this.tBoxContrast.TabIndex = 9;
            this.tBoxContrast.TextChanged += new System.EventHandler(this.tBoxContrast_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(137, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Contrast:";
            // 
            // tBarContrast
            // 
            this.tBarContrast.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tBarContrast.LargeChange = 10;
            this.tBarContrast.Location = new System.Drawing.Point(126, 92);
            this.tBarContrast.Maximum = 100;
            this.tBarContrast.Minimum = -100;
            this.tBarContrast.Name = "tBarContrast";
            this.tBarContrast.Size = new System.Drawing.Size(104, 45);
            this.tBarContrast.TabIndex = 7;
            this.tBarContrast.TickFrequency = 0;
            this.tBarContrast.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.tBarContrast.Scroll += new System.EventHandler(this.tBarContrast_Scroll);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Image Mode:";
            // 
            // cBoxColor
            // 
            this.cBoxColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cBoxColor.FormattingEnabled = true;
            this.cBoxColor.Items.AddRange(new object[] {
            "Black and White",
            "Color",
            "Gray Scale"});
            this.cBoxColor.Location = new System.Drawing.Point(6, 32);
            this.cBoxColor.Name = "cBoxColor";
            this.cBoxColor.Size = new System.Drawing.Size(118, 21);
            this.cBoxColor.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(141, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Brightness:";
            // 
            // txtBoxBrightness
            // 
            this.txtBoxBrightness.Location = new System.Drawing.Point(246, 32);
            this.txtBoxBrightness.MaxLength = 4;
            this.txtBoxBrightness.Name = "txtBoxBrightness";
            this.txtBoxBrightness.Size = new System.Drawing.Size(69, 20);
            this.txtBoxBrightness.TabIndex = 1;
            this.txtBoxBrightness.TextChanged += new System.EventHandler(this.txtBoxBrightness_TextChanged);
            // 
            // TBarBrightness
            // 
            this.TBarBrightness.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TBarBrightness.LargeChange = 10;
            this.TBarBrightness.Location = new System.Drawing.Point(126, 27);
            this.TBarBrightness.Maximum = 100;
            this.TBarBrightness.Minimum = -100;
            this.TBarBrightness.Name = "TBarBrightness";
            this.TBarBrightness.Size = new System.Drawing.Size(104, 45);
            this.TBarBrightness.TabIndex = 0;
            this.TBarBrightness.TickFrequency = 0;
            this.TBarBrightness.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.TBarBrightness.Scroll += new System.EventHandler(this.TBarBrightness_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(171, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Paper Size:";
            // 
            // cBoxScannerType
            // 
            this.cBoxScannerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxScannerType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cBoxScannerType.FormattingEnabled = true;
            this.cBoxScannerType.Items.AddRange(new object[] {
            " "});
            this.cBoxScannerType.Location = new System.Drawing.Point(176, 54);
            this.cBoxScannerType.Name = "cBoxScannerType";
            this.cBoxScannerType.Size = new System.Drawing.Size(118, 21);
            this.cBoxScannerType.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(171, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Scanner Type:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numCustomDPI);
            this.groupBox1.Controls.Add(this.cBoxDPI);
            this.groupBox1.Location = new System.Drawing.Point(14, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(144, 106);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Resolution";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(79, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "dpi";
            // 
            // numCustomDPI
            // 
            this.numCustomDPI.Enabled = false;
            this.numCustomDPI.Location = new System.Drawing.Point(20, 57);
            this.numCustomDPI.Maximum = new decimal(new int[] {
            1200,
            0,
            0,
            0});
            this.numCustomDPI.Minimum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.numCustomDPI.Name = "numCustomDPI";
            this.numCustomDPI.Size = new System.Drawing.Size(53, 20);
            this.numCustomDPI.TabIndex = 1;
            this.numCustomDPI.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.numCustomDPI.ValueChanged += new System.EventHandler(this.numCustomDPI_ValueChanged);
            // 
            // cBoxDPI
            // 
            this.cBoxDPI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxDPI.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cBoxDPI.FormattingEnabled = true;
            this.cBoxDPI.Items.AddRange(new object[] {
            "600",
            "500",
            "400",
            "300",
            "200",
            "Custom"});
            this.cBoxDPI.Location = new System.Drawing.Point(20, 19);
            this.cBoxDPI.Name = "cBoxDPI";
            this.cBoxDPI.Size = new System.Drawing.Size(69, 21);
            this.cBoxDPI.TabIndex = 0;
            this.cBoxDPI.SelectedIndexChanged += new System.EventHandler(this.cBoxDPI_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.imageViewer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(741, 529);
            this.panel1.TabIndex = 1;
            this.panel1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panel1_Scroll);
            // 
            // imageViewer1
            // 
            this.imageViewer1.AutoScroll = true;
            this.imageViewer1.AutoScrollMinSize = new System.Drawing.Size(738, 339);
            this.imageViewer1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.imageViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageViewer1.Image = global::EdocsUSA.Utilities.Properties.Resources.edocs_logo;
            this.imageViewer1.Location = new System.Drawing.Point(0, 0);
            this.imageViewer1.Name = "imageViewer1";
            this.imageViewer1.ScalingMode = EdocsUSA.Controls.ImageViewer.ImageScalingMode.Fit;
            this.imageViewer1.SelectAnchor = new System.Drawing.Point(0, 0);
            this.imageViewer1.SelectEnd = new System.Drawing.Point(0, 0);
            this.imageViewer1.Size = new System.Drawing.Size(739, 527);
            this.imageViewer1.TabIndex = 2;
            this.imageViewer1.Text = "imageViewer1";
            this.imageViewer1.ToolMode = EdocsUSA.Controls.ImageViewer.MouseToolMode.Pan;
            this.imageViewer1.ZoomLevel = 0.9853333F;
            this.imageViewer1.ZoomMultiplier = 0.15F;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.button7);
            this.panel2.Controls.Add(this.button6);
            this.panel2.Controls.Add(this.button5);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 479);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1142, 50);
            this.panel2.TabIndex = 1;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(497, 4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 21;
            this.button7.Text = "button7";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.AutoSize = true;
            this.button6.BackColor = System.Drawing.Color.DodgerBlue;
            this.button6.Dock = System.Windows.Forms.DockStyle.Right;
            this.button6.Image = global::EdocsUSA.Utilities.Properties.Resources.icons8_zoom_out_40;
            this.button6.Location = new System.Drawing.Point(971, 0);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(46, 46);
            this.button6.TabIndex = 20;
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Dock = System.Windows.Forms.DockStyle.Left;
            this.button5.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.button5.FlatAppearance.BorderSize = 2;
            this.button5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SpringGreen;
            this.button5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Firebrick;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Location = new System.Drawing.Point(174, 0);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(87, 46);
            this.button5.TabIndex = 19;
            this.button5.Text = "&Reset";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.AutoSize = true;
            this.button4.BackColor = System.Drawing.Color.DodgerBlue;
            this.button4.Dock = System.Windows.Forms.DockStyle.Right;
            this.errorProvider.SetIconAlignment(this.button4, System.Windows.Forms.ErrorIconAlignment.TopLeft);
            this.button4.Image = global::EdocsUSA.Utilities.Properties.Resources.icons8_zoom_in_40;
            this.button4.Location = new System.Drawing.Point(1017, 0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(46, 46);
            this.button4.TabIndex = 13;
            this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Left;
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.LightGreen;
            this.button3.FlatAppearance.BorderSize = 2;
            this.button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SpringGreen;
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Firebrick;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(87, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(87, 46);
            this.button3.TabIndex = 12;
            this.button3.Text = "&Exit";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Left;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.LightGreen;
            this.button2.FlatAppearance.BorderSize = 2;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SpringGreen;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Firebrick;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(0, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(87, 46);
            this.button2.TabIndex = 11;
            this.button2.Text = "&Save";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.LightGreen;
            this.button1.FlatAppearance.BorderSize = 2;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SpringGreen;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Firebrick;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(1063, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 46);
            this.button1.TabIndex = 10;
            this.button1.Text = "&Preview";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // WIA_Settings_Dialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 529);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WIA_Settings_Dialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WIA Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WIA_Settings_Dialog_FormClosing);
            this.Load += new System.EventHandler(this.WIA_Settings_Dialog_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tBarContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TBarBrightness)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCustomDPI)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TrackBar TBarBrightness;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numCustomDPI;
        private System.Windows.Forms.ComboBox cBoxDPI;
        private System.Windows.Forms.TextBox txtBoxBrightness;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cBoxScannerType;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cBoxColor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cBoxPaperSize;
        private System.Windows.Forms.TextBox tBoxContrast;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TrackBar tBarContrast;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtBoxTop;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBoxLeft;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBoxWidth;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtBoxLength;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Button button5;
        private EdocsUSA.Controls.ImageViewer imageViewer1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
    }
}