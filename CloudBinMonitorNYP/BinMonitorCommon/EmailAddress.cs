using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlCommands;

namespace BinMonitor.Common
{ 
    public partial class EmailAddress : Form
    {
        public EmailAddress()
        {
            InitializeComponent();
        }

        private void EmailAddress_Shown(object sender, EventArgs e)
        {
            SqlCmd CmdSql = new SqlCommands.SqlCmd();
            using (SqlConnection sqlConnection = CmdSql.SqlConnection())
            {
                DataSet ds = CmdSql.GetEmailAddress(SqlCommands.SqlConstants.SpEmailAddress, sqlConnection);
                foreach (DataRow t in ds.Tables[0].Rows)
                {
                    chkEmailAdd.Items.Add(t.ItemArray[0].ToString());
                }

            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            txtEmailAdd.Text = string.Empty;
            foreach(var emailChecked in chkEmailAdd.CheckedItems)
            {
                
                txtEmailAdd.Text += emailChecked+";";
            }
            if (txtEmailAdd.Text.EndsWith(";"))
                txtEmailAdd.Text = txtEmailAdd.Text.Remove(txtEmailAdd.Text.Length - 1, 1);
            this.Close();
            
        }

        private void BtnCancle_Click(object sender, EventArgs e)
        {
            txtEmailAdd.Text = string.Empty;
            this.Close();
        }
    }
}
