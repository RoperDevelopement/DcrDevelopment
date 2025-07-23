using System;
using System.ComponentModel;
using System.Windows.Forms;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;

namespace EdocsUSA.Controls
{
	public class ValidatingTextBox<T> : TextBox
	{	
		protected ErrorProvider ErrorProvider = new ErrorProvider()
		{
			BlinkStyle = ErrorBlinkStyle.NeverBlink
		};
		
		public string ValidationHint { get; set; }
		
		private bool _RequiresValue = false;
		public bool RequiresValue 
		{ 
			get { return _RequiresValue; }
			set { _RequiresValue = value; }
		}
		
		public string ValidationRegex { get; set; }
		
		public int MinLength { get; set; }
		
		public T Value
		{
			get 
			{ 
				if (Text.IsEmpty()) return TypeExtensions.GetDefaultValue<T>();
				return Text.ConvertTo<T>();
			}
			set 
			{ 
				if (value == null) Text = string.Empty;
				else 
				{ Text = StringTools.ConvertFrom(value); }
			}
		}
		
		public bool HasValue { get { return Text.IsNotEmpty(); } }
		
		public ValidatingTextBox()
		{
			
		}
		
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			this.SelectAll();
		}
		
		protected override void OnValidating(CancelEventArgs e)
		{
			base.OnValidating(e);
			bool isValid = e.Cancel == false;
			string validationErrorMessage = String.Empty;
			
			if (Text.IsEmpty())
			{
				if (RequiresValue) 
				{ 
					isValid = false;
					validationErrorMessage = "Value required";
				}
				else 
				{ isValid = true; }
			}
			else
			{
				if (ValidationRegex.IsNotEmpty() && (Text.MatchesRegex(ValidationRegex) == false))
				{ 
					isValid = false;
					validationErrorMessage = "Regex fail " + ValidationRegex;
				}
				
				object value;
				if (TypeDescriptor.GetConverter(typeof(T)).TryConvertFrom(Text, out value) == false)
				{ 
					isValid = false;
					validationErrorMessage = "Conversion failure";
				}
			}
				
			if (isValid) 
			{
				ErrorProvider.Clear();
				e.Cancel = false;
			}
			else
			{
				ErrorProvider.SetError(this, validationErrorMessage);
				e.Cancel = true;
			}

		}
	}
}
