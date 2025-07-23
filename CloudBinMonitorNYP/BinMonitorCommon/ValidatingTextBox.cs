using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BinMonitor.Common
{
    public class ValidatingTextBox : TextBox
    {
        protected TypeConverter ValidatingTypeConverter = null;

        protected Type ValidatingType = null;

        public string ValidatingTypeName
        {
            get { return TypeDescriptor.GetConverter(typeof(Type)).ConvertToString(ValidatingType); }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    ValidatingType = null;
                    ValidatingTypeConverter = null;
                }
                else
                {
                    ValidatingType = Type.GetType(value, true, true);
                    ValidatingTypeConverter = TypeDescriptor.GetConverter(ValidatingType);
                }
            }
        }

        public string ValidatingRegex { get; set; }

        [Browsable(false)]
        public string ValidationErrorMessage { get; protected set; }

        public int MinLength { get; set; }

        public string MinValue { get; set; }

        public string MaxValue { get; set; }

        public ValidatingTextBox()
            : base()
        { }

        protected bool TryCompareText(string value1Text, string value2Text, out int result, out string errorMessage)
        {
            if (ValidatingType == null)
            {
                result = default(int);
                errorMessage = "ValidatingType has not been defined";
                return false;
            }

            if (ValidatingType.GetInterface("IComparable") == null)
            {
                result = default(int);
                errorMessage = "ValidatingType does not implement IComparable";
                return false;
            }

            IComparable value1;
            try
            { value1 = (IComparable)(ValidatingTypeConverter.ConvertFromString(value1Text)); }
            catch (Exception ex)
            {
                result = default(int);
                errorMessage = string.Format("Error converting {0} to type {1} - {2}", value1Text, ValidatingType, ex.Message);
                return false;
            }

            IComparable value2;
            try
            { value2 = (IComparable)(ValidatingTypeConverter.ConvertFromString(value2Text)); }
            catch (Exception ex)
            {
                result = default(int);
                errorMessage = string.Format("Error converting {0} to type {1} - {2}", value2Text, ValidatingType, ex.Message);
                return false;
            }

            result = value1.CompareTo(value2);
            errorMessage = null;
            return true;
        }

        protected bool ValidateMinValue(out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(MinValue))
            {
                errorMessage = null;
                return true;
            }

            int result;
            if (TryCompareText(Text, MinValue, out result, out errorMessage) == false)
            { return false; }

            if (result < 0)
            {
                errorMessage = string.Format("Value must be greater than or equal to {0}", MinValue);
                return false;
            }

            errorMessage = null;
            return true;
        }

        protected bool ValidateMaxValue(out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(MaxValue) == true)
            {
                errorMessage = null;
                return true;
            }

            int result;
            if (TryCompareText(Text, MaxValue, out result, out errorMessage) == false)
            { return false; }

            if (result > 0)
            {
                errorMessage = string.Format("Value must be less than or equal to {0}", MaxValue);
                return false;
            }

            errorMessage = null;
            return true;
        }


        protected bool ValidateRegex(out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(ValidatingRegex))
            {
                errorMessage = null;
                return true;
            }

            if (Regex.IsMatch(Text, ValidatingRegex) == false)
            {
                errorMessage = string.Format("Text does not match RegEx pattern ({0})", ValidatingRegex);
                return false;
            }

            errorMessage = null;
            return true;
        }

        protected bool ValidateType(out string errorMessage)
        {
            if (ValidatingType == null)
            {
                errorMessage = null;
                return true;
            }

            try
            {
                ValidatingTypeConverter.ConvertFromString(Text);
                errorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = string.Format("Unable to convert {0} to type {1} - {2}", Text, ValidatingType, ex.Message);
                return false;
            }
        }

        protected bool ValidateMinLength(out string errorMessage)
        {
            if (MinLength <= 0)
            {
                errorMessage = null;
                return true;
            }

            if (Text.Length < MinLength)
            {
                errorMessage = string.Format("Value must contain at least {0} characters", MinLength);
                return false;
            }

            errorMessage = null;
            return true;
        }

        protected bool ValidateMaxLength(out string errorMessage)
        {
            if (MaxLength <= 0)
            {
                errorMessage = null;
                return true;
            }

            if (Text.Length > MaxLength)
            {
                errorMessage = string.Format("Value must contain less than {0} characters", MaxLength);
                return false;
            }

            errorMessage = null;
            return true;
        }

        protected override void OnValidating(System.ComponentModel.CancelEventArgs e)
        {
            ValidationErrorMessage = null;
            string validationErrorMessage;

            if (e.Cancel == true)
            { ValidationErrorMessage = "Unknown Error"; }
            else if ((ValidateMinLength(out validationErrorMessage) == false)
                     || (ValidateMaxLength(out validationErrorMessage) == false)
                     || (ValidateRegex(out validationErrorMessage) == false)
                     || (ValidateType(out validationErrorMessage) == false)
                     || (ValidateMinValue(out validationErrorMessage) == false)
                     || (ValidateMaxValue(out validationErrorMessage) == false))
            {
                ValidationErrorMessage = validationErrorMessage;
                e.Cancel = true;
            }

            base.OnValidating(e);
        }
    }
}
