/*
 * Created by SharpDevelop.
 * User: Sam Brinly
 * Date: 4/5/2012
 * Time: 9:01 AM
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using EdocsUSA.Utilities;

namespace EdocsUSA.Controls
{
	/// <summary>
	/// Description of ComboBoxInputControl.
	/// </summary>
	public partial class ComboBoxInputControl : UserControl
	{
		public string ErrorMessage
		{
			get { return ErrorProvider.GetError(ValueComboBox); }
			set 
			{ 
				if (value.IsEmpty()) ErrorProvider.Clear();
				else ErrorProvider.SetError(ValueComboBox, value);
			}
		}
		
		private bool _RequiresValue = false;
		public bool RequiresValue
		{
			get { return _RequiresValue; }
			set { _RequiresValue = value; }
		}
		
		private Type _ValidatingType = typeof(string);
		public Type ValidatingType 
		{ 
			get { return _ValidatingType; }
			set { _ValidatingType = value; }
		}
		
		public string Caption
		{
			get { return CaptionLabel.Text; }
			set { CaptionLabel.Text = value; }
		}
		
		public int CaptionWidth
		{
			get { return CaptionLabel.Width; }
			set { CaptionLabel.Width = value; }
		}
		
		public string ValueText
		{
			get { return ValueComboBox.Text.IsEmpty() ? null : ValueComboBox.Text; }
			set 
			{
				string errorMessage;
				if (CheckValue(value, out errorMessage))
				{ ValueComboBox.Text = value; }
				else { throw new Exception(ErrorMessage); }
			}
		}
		
		public List<string> Items
		{ set { foreach (string item in value) { ValueComboBox.Items.Add(item); } } } 
		public ComboBoxInputControl()
		{
			InitializeComponent();
			ErrorPanel.Width = ErrorProvider.Icon.Width;
			ErrorProvider.SetIconAlignment(ValueComboBox, ErrorIconAlignment.MiddleRight);
			
			ValueComboBox.Items.Add("");
		}
		
		void ComboBoxInputControlValidating(object sender, CancelEventArgs e)
		{
			string errorMessage;
			CheckValue(ValueText, out errorMessage);
			ErrorMessage = errorMessage;
			
			if (ErrorMessage.IsEmpty()) e.Cancel = false;
			else e.Cancel = true;
		}
		
		public void Clear() { ValueComboBox.Text = string.Empty; }
		
		public void SetValueText(string value)
		{
			string errorMessage;
			if (CheckValue(value, out errorMessage) == false)
			{ throw new Exception(errorMessage); }
			
			ValueComboBox.Text = value;
		}
		
		public bool CheckValue(string value, out string errorMessage)
		{
			if (value.IsEmpty() && (!RequiresValue)) 
			{
				errorMessage = null;
				return true;
			}
			
			if (value.IsEmpty() && (RequiresValue))
			{
				errorMessage = "Value is required";
				return false;
			}
			
			errorMessage = null;
			return true;
		}
	}
}
