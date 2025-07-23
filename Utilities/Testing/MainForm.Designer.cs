/*
 * User: Sam
 * Date: 10/10/2011
 * Time: 10:26 AM
 */

 
namespace Testing
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.BoolInput = new EdocsUSA.Controls.InputControl();
			this.button1 = new System.Windows.Forms.Button();
			this.DateInput = new EdocsUSA.Controls.InputControl();
			this.BoolNInput = new EdocsUSA.Controls.InputControl();
			this.StringInput = new EdocsUSA.Controls.InputControl();
			this.IntInput = new EdocsUSA.Controls.InputControl();
			this.IntNInput = new EdocsUSA.Controls.InputControl();
			this.MessageLabel = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.comboBoxInputControl1 = new EdocsUSA.Controls.ComboBoxInputControl();
			this.SuspendLayout();
			// 
			// BoolInput
			// 
			this.BoolInput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BoolInput.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.BoolInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.BoolInput.Caption = "bool";
			this.BoolInput.CaptionWidth = 43;
			this.BoolInput.ErrorMessage = "";
			this.BoolInput.HintText = null;
			this.BoolInput.Location = new System.Drawing.Point(9, 9);
			this.BoolInput.Margin = new System.Windows.Forms.Padding(0);
			this.BoolInput.MinimumSize = new System.Drawing.Size(2, 20);
			this.BoolInput.Name = "BoolInput";
			this.BoolInput.Padding = new System.Windows.Forms.Padding(0, 0, 25, 0);
			this.BoolInput.RegEx = null;
			this.BoolInput.RequiresValue = false;
			this.BoolInput.Size = new System.Drawing.Size(150, 20);
			this.BoolInput.TabIndex = 0;
			this.BoolInput.ValidatingType = typeof(string);
			this.BoolInput.ValueText = null;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 264);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// DateInput
			// 
			this.DateInput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.DateInput.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.DateInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.DateInput.Caption = "date";
			this.DateInput.CaptionWidth = 43;
			this.DateInput.ErrorMessage = "";
			this.DateInput.HintText = null;
			this.DateInput.Location = new System.Drawing.Point(9, 38);
			this.DateInput.Margin = new System.Windows.Forms.Padding(0);
			this.DateInput.MinimumSize = new System.Drawing.Size(2, 20);
			this.DateInput.Name = "DateInput";
			this.DateInput.Padding = new System.Windows.Forms.Padding(0, 0, 25, 0);
			this.DateInput.RegEx = null;
			this.DateInput.RequiresValue = false;
			this.DateInput.Size = new System.Drawing.Size(150, 20);
			this.DateInput.TabIndex = 2;
			this.DateInput.ValidatingType = typeof(string);
			this.DateInput.ValueText = null;
			// 
			// BoolNInput
			// 
			this.BoolNInput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BoolNInput.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.BoolNInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.BoolNInput.Caption = "bool?";
			this.BoolNInput.CaptionWidth = 43;
			this.BoolNInput.ErrorMessage = "";
			this.BoolNInput.HintText = null;
			this.BoolNInput.Location = new System.Drawing.Point(9, 67);
			this.BoolNInput.Margin = new System.Windows.Forms.Padding(0);
			this.BoolNInput.MinimumSize = new System.Drawing.Size(2, 20);
			this.BoolNInput.Name = "BoolNInput";
			this.BoolNInput.RegEx = null;
			this.BoolNInput.RequiresValue = false;
			this.BoolNInput.Size = new System.Drawing.Size(150, 20);
			this.BoolNInput.TabIndex = 3;
			this.BoolNInput.ValidatingType = typeof(string);
			this.BoolNInput.ValueText = null;
			// 
			// StringInput
			// 
			this.StringInput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.StringInput.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.StringInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.StringInput.Caption = "string";
			this.StringInput.CaptionWidth = 43;
			this.StringInput.ErrorMessage = "";
			this.StringInput.HintText = null;
			this.StringInput.Location = new System.Drawing.Point(9, 99);
			this.StringInput.Margin = new System.Windows.Forms.Padding(0);
			this.StringInput.MinimumSize = new System.Drawing.Size(2, 20);
			this.StringInput.Name = "StringInput";
			this.StringInput.RegEx = null;
			this.StringInput.RequiresValue = false;
			this.StringInput.Size = new System.Drawing.Size(252, 46);
			this.StringInput.TabIndex = 4;
			this.StringInput.ValidatingType = typeof(string);
			this.StringInput.ValueText = null;
			// 
			// IntInput
			// 
			this.IntInput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.IntInput.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.IntInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.IntInput.Caption = "int";
			this.IntInput.CaptionWidth = 43;
			this.IntInput.ErrorMessage = "";
			this.IntInput.HintText = null;
			this.IntInput.Location = new System.Drawing.Point(9, 159);
			this.IntInput.Margin = new System.Windows.Forms.Padding(0);
			this.IntInput.MinimumSize = new System.Drawing.Size(2, 20);
			this.IntInput.Name = "IntInput";
			this.IntInput.RegEx = null;
			this.IntInput.RequiresValue = false;
			this.IntInput.Size = new System.Drawing.Size(150, 20);
			this.IntInput.TabIndex = 5;
			this.IntInput.ValidatingType = typeof(string);
			this.IntInput.ValueText = null;
			// 
			// IntNInput
			// 
			this.IntNInput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.IntNInput.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.IntNInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.IntNInput.Caption = "int?";
			this.IntNInput.CaptionWidth = 43;
			this.IntNInput.ErrorMessage = "";
			this.IntNInput.HintText = null;
			this.IntNInput.Location = new System.Drawing.Point(9, 192);
			this.IntNInput.Margin = new System.Windows.Forms.Padding(0);
			this.IntNInput.MinimumSize = new System.Drawing.Size(2, 20);
			this.IntNInput.Name = "IntNInput";
			this.IntNInput.RegEx = null;
			this.IntNInput.RequiresValue = false;
			this.IntNInput.Size = new System.Drawing.Size(150, 20);
			this.IntNInput.TabIndex = 6;
			this.IntNInput.ValidatingType = typeof(string);
			this.IntNInput.ValueText = null;
			// 
			// MessageLabel
			// 
			this.MessageLabel.Location = new System.Drawing.Point(105, 264);
			this.MessageLabel.Name = "MessageLabel";
			this.MessageLabel.Size = new System.Drawing.Size(100, 23);
			this.MessageLabel.TabIndex = 7;
			this.MessageLabel.Text = "label1";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(186, 35);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 8;
			this.button2.Text = "button2";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Button2Click);
			// 
			// comboBoxInputControl1
			// 
			this.comboBoxInputControl1.Caption = "Caption";
			this.comboBoxInputControl1.CaptionWidth = 43;
			this.comboBoxInputControl1.ErrorMessage = "";
			this.comboBoxInputControl1.Location = new System.Drawing.Point(12, 224);
			this.comboBoxInputControl1.Name = "comboBoxInputControl1";
			this.comboBoxInputControl1.RequiresValue = false;
			this.comboBoxInputControl1.Size = new System.Drawing.Size(249, 20);
			this.comboBoxInputControl1.TabIndex = 9;
			this.comboBoxInputControl1.ValidatingType = typeof(string);
			this.comboBoxInputControl1.ValueText = null;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
			this.ClientSize = new System.Drawing.Size(284, 311);
			this.Controls.Add(this.comboBoxInputControl1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.MessageLabel);
			this.Controls.Add(this.IntNInput);
			this.Controls.Add(this.IntInput);
			this.Controls.Add(this.StringInput);
			this.Controls.Add(this.BoolNInput);
			this.Controls.Add(this.DateInput);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.BoolInput);
			this.Name = "MainForm";
			this.Text = "Testing";
			this.ResumeLayout(false);
		}
		private EdocsUSA.Controls.ComboBoxInputControl comboBoxInputControl1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label MessageLabel;
		private EdocsUSA.Controls.InputControl IntNInput;
		private EdocsUSA.Controls.InputControl IntInput;
		private EdocsUSA.Controls.InputControl StringInput;
		private EdocsUSA.Controls.InputControl BoolNInput;
		private EdocsUSA.Controls.InputControl DateInput;
		private System.Windows.Forms.Button button1;
		private EdocsUSA.Controls.InputControl BoolInput;
	}
}
