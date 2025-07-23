using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Edocs.DOH.DL.Doc.Utilities;
namespace Edocs.DOH.DL.Doc
{
    public partial class RejectDOHDocument : Form
    {
        public RejectDOHDocument()
        {
            InitializeComponent();
        }
        public int ID
        { get; set; }
        public string DocumetName
        { get; set; }

        private void RejectDOHDocument_Shown(object sender, EventArgs e)
        {
            label2.Text = $"{DocumetName}";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {


                if (string.IsNullOrWhiteSpace(rTxtReason.Text))
                {
                    MessageBox.Show("Need a Rejection Reason", "Reason", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Send_Emails.EmailInstance.SendEmail($"DOH Document {ID} was rejected by {Environment.UserName} reason {rTxtReason.Text}");
                DOHDownLoadDocuments.DownLoadDocsInstance.RejectDOHDOc(ID, Environment.UserName, rTxtReason.Text.Trim()).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail($"Error adding reject reason document id {ID} {ex.Message}");
                MessageBox.Show($"Error adding reject reason document id {ID} {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
        }
    }
}
