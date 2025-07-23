using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EdocsUSA.Utilities;
using ScanQuireSqlCmds;
using ScanQuire_SendEmails;
namespace Scanquire.Public.UserControls
{
    public partial class DecriptPw : Form
    {
        public DecriptPw()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEncTxt.Text))
            {
                MessageBox.Show("Need to enter a string", "Empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            if (string.IsNullOrWhiteSpace(cmBoxChange.Text))
            {
                MessageBox.Show("Need to select what to encript", "Empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UpDateEncriptedPw encriptedPw = new UpDateEncriptedPw();
            if (cmBoxChange.Text == "DbUserName")
            {
                encriptedPw.DbUserName(txtEncTxt.Text);

            }
            else if (cmBoxChange.Text == "DbPassWord")
                encriptedPw.DbPw(txtEncTxt.Text);
            else if (cmBoxChange.Text == "EmailPassword")
            {
                Send_Emails.EmailInstance.UpDateEmailPw(cmBoxChange.Text);
            }
            else
            {
                MessageBox.Show("Invalid selection", "Invaild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmBoxChange.Text = string.Empty;
                return;
            }

            txtEncTxt.Text = string.Empty;
            
            MessageBox.Show("String decripted");

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
