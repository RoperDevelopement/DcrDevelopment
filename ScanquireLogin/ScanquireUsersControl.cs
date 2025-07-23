using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ScanquireLogin
{
    public partial class ScanquireUsersControl : UserControl
    {
        public ScanquireUsersControl()
        {
            InitializeComponent();
            toolTipTxtBoxes.AutoPopDelay = 50000;
            toolTipTxtBoxes.ToolTipIcon = ToolTipIcon.Info;
        }
        
    
        private void TxtBoxFName_Enter(object sender, EventArgs e)
        {

            toolTipTxtBoxes.SetToolTip(txtBoxFName, "User FirstName");
            toolTipTxtBoxes.ToolTipTitle = "First Name";
          //  toolTipTxtBoxes.Show("User FirstName", txtBoxFName, 60);

            //

            //
            //ToolTip.IsBalloon = true;
            //

        }

        private void TxtBoxLName_Enter(object sender, EventArgs e)
        {

            toolTipTxtBoxes.SetToolTip(txtBoxLName, "User LastName");
            toolTipTxtBoxes.ToolTipTitle = "Last Name";
        }

        private void TxtBoxEmailAddress_Enter(object sender, EventArgs e)
        {
            toolTipTxtBoxes.SetToolTip(txtBoxEmailAddress, "emailaddress@.com");
            toolTipTxtBoxes.ToolTipTitle = "Email address";
        }

        private void TxtBoxLoginId_Enter(object sender, EventArgs e)
        {
            toolTipTxtBoxes.SetToolTip(txtBoxLoginId, "Scanquire login id min length 8");
            toolTipTxtBoxes.ToolTipTitle = "Login ID";
            
        }

        private void TxtBoxFName_MouseHover(object sender, EventArgs e)
        {
            toolTipTxtBoxes.SetToolTip(txtBoxFName, "User FirstName");
            toolTipTxtBoxes.ToolTipTitle = "First Name";
        }

        private void ChkBoxIsAdmin_MouseUp(object sender, MouseEventArgs e)
        {
            toolTipTxtBoxes.SetToolTip(chkBoxIsAdmin, "User is admin can make changes");
            toolTipTxtBoxes.ToolTipTitle = "User admin";
        }

        private void ChkBoxShowPassWord_Enter(object sender, EventArgs e)
        {
            toolTipTxtBoxes.SetToolTip(chkBoxIsAdmin, "Show the PassWord");
            toolTipTxtBoxes.ToolTipTitle = "Password";
        }

        private void TxtBoxLName_MouseHover(object sender, EventArgs e)
        {
            toolTipTxtBoxes.SetToolTip(txtBoxLName, "User LastName");
            toolTipTxtBoxes.ToolTipTitle = "Last Name";
        }

        

        private void ChkBoxShowPassWord_CheckedChanged(object sender, EventArgs e)
        {
            if(chkBoxShowPassWord.Checked)
            { 
             txtBoxPassWord.PasswordChar = LoginUtilites.LoginUtilitesInstance.RemovePassowrdChar;
               txtBoxVerifyPassWord.PasswordChar = LoginUtilites.LoginUtilitesInstance.RemovePassowrdChar;

            }
            else
            {
                txtBoxPassWord.PasswordChar = LoginUtilites.LoginUtilitesInstance.RemovePassowrdChar;
                txtBoxVerifyPassWord.PasswordChar = LoginUtilites.LoginUtilitesInstance.RemovePassowrdChar;
            }
        }

        private void TxtBoxPassWord_Enter(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if(!(textBox.ReadOnly))
            {

                toolTipTxtBoxes.ToolTipTitle = "PassWord";
                
            //PopUpWindow popUpWindow = new PopUpWindow();
              // popUpWindow.PopUpMessage = "Password word lenght must be 8 contain a one upper case/r/n /r/n contain a one lower case";
                toolTipTxtBoxes.SetToolTip(textBox, "Min length 8 and must contains a one upper case,one lower case,one non-digit, and one digit");
             
              //  popUpWindow.Show();
            }
        }
    }
}
