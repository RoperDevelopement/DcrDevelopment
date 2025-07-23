/*
 * User: Sam
 * Date: 10/10/2011
 * Time: 9:41 AM
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using EdocsUSA.Utilities;

namespace EdocsUSA.Controls
{
	/// <summary>
	/// Description of InputControl.
	/// </summary>
	public partial class InputControl : UserControl
	{
		public string ErrorMessage
		{
			get { return ErrorProvider.GetError(ValueTextBox); }
			set 
			{ 
				if (value.IsEmpty()) ErrorProvider.Clear();
				else ErrorProvider.SetError(ValueTextBox, value);
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
			get { return ValueTextBox.Text.IsEmpty() ? null : ValueTextBox.Text; }
			set 
			{
				string errorMessage;
				if (CheckValue(value, out errorMessage))
				{ ValueTextBox.Text = value; }
				else { throw new Exception(ErrorMessage); }
			}
		}
		
		private string _Regex = null;
		public string RegEx 
		{ 
			get { return _Regex; }
			set { _Regex = value; }
		}
		
		private string _HintText = null;
		public string HintText 
		{ 
			get { return _HintText; }
			set { _HintText = value; }
		}
		
		public InputControl()
		{
			InitializeComponent();
			ErrorPanel.Width = ErrorProvider.Icon.Width;
			ErrorProvider.SetIconAlignment(ValueTextBox, ErrorIconAlignment.MiddleRight);
		}
		
		protected void InputControlValidating(object sender, CancelEventArgs e)
		{
			string errorMessage;
			CheckValue(ValueText, out errorMessage);
			ErrorMessage = errorMessage;
			
			if (ErrorMessage.IsEmpty()) { e.Cancel = false; }
			else
			{ 
				if (HintText != null) ErrorMessage += Environment.NewLine + "Hint: " + HintText; 
				e.Cancel = true;
			}
			
			
		}
		
		public void Clear() { ValueTextBox.Text = string.Empty; }
		
		public void SetValueText(string value)
		{
			string errorMessage;
			if (CheckValue(value, out errorMessage) == false)
			{ throw new Exception(errorMessage); }
			
			ValueTextBox.Text = value;
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
			
			if (value.IsNotEmpty() && (RegEx.IsNotEmpty()) && (value.MatchesRegex(RegEx) == false))
			{
				errorMessage = "Value does not match regex pattern";
				return false;
			}
			
			TypeConverter converter = TypeDescriptor.GetConverter(ValidatingType);
			if (converter.IsValid(value) == false)
			{
				errorMessage = "Value is invalid";
				return false;
			}
			
			errorMessage = null;
			return true;
		}
		
		public T GetValue<T>()
		{ return ValueText.ConvertTo<T>(); }
	}
}
