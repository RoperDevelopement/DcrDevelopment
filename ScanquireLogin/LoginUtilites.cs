using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using ScanQuire_SendEmails;
namespace ScanquireLogin
{
    class LoginUtilites
    {

        public static LoginUtilites LoginUtilitesInstance = null;
        public readonly string RegXNonDigitChar = "[^0-9]";
        public readonly string RegXNumsLetters = "[a-zA-Z0-9]*$";
        public readonly string RegXLowewrCase = "(.*[a-z].*)";
        public readonly string RegXLowewrUpperCase = "(.*[A-Z].*)";
        public readonly string RegXLoweDigits = @"(.*\d.*)";
        private readonly char DefaultPassWordChar = '*';



        public char RemovePassowrdChar
        { get { return char.Parse("\0"); } }

        protected LoginUtilites()
        { }
        static LoginUtilites()
        {

            if (LoginUtilitesInstance == null)
            {
                LoginUtilitesInstance = new LoginUtilites();
            }

        }
        public TextBox ChangePasWordChar(TextBox textBox)
        {
            if (textBox.PasswordChar != RemovePassowrdChar)
                textBox.PasswordChar = RemovePassowrdChar;
            else
                textBox.PasswordChar = DefaultPassWordChar;
            return textBox;
        }
        public bool CheckValidEmailAddress(string emailAddress)
        {
            try
            {

                Send_Emails.EmailInstance.NewMailMessage(emailAddress);
                return true;
            }
            catch { }
            return false;
        }

        public bool CheckValidString(string inStr,int strMinLength,string txtBox)
        {
            
            if((string.IsNullOrEmpty(inStr)))
            {
                MessageBox.Show($"{txtBox} cannot be empty", "Empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(inStr.Length < strMinLength)
            {
                MessageBox.Show($"{txtBox} length {inStr.Length.ToString()} {txtBox} min length has to be {strMinLength.ToString()}", "Invalid Length", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public bool CheckStringIsEmpty(string inStr)
        {
            if (!(string.IsNullOrEmpty(inStr)))
            {
                MessageBox.Show("String has a", "Empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            return true;
        }

        public bool CheckRegxString(string inStr,string regX,bool showMessageBox)
        {
            if(!(Regex.Match(inStr,regX).Success))
            {
                if(showMessageBox)
                    MessageBox.Show($"String  {inStr} does not match pattern {regX}", "Invalid String", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
    }
}
