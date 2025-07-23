using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using Microsoft;
using System.Diagnostics;
using Tesseract;
using System.IO;
using System.Configuration;
using System.Reflection;
using Scanquire.Public.Extensions;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using ETL = EdocsUSA.Utilities.Logging;
using EdocsUSA.Utilities.Controls;
using Scanquire.Public.UserControls;
using System.Text.RegularExpressions;
using SE = ScanQuire_SendEmails;
namespace Scanquire.Public
{
    public partial class EmailImagesForm : Form
    {
        public enum EmaiImageTypes
        {
            PNG,
            PDF,
            TIF,
            JPG,
            BMP,
            GIF
        }
        public EmaiImageTypes ImageType
        { get; set; }
        public SQImageListViewerItem[] EmailImages
        { get; set; }
        public List<string> EmailAtt
        { get; set; }
        public EmailImagesForm()
        {
            InitializeComponent();
        }
        private Regex EmailValidation()
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(pattern, RegexOptions.IgnoreCase);
        }

        private bool EmailValidation(string emailAdd)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

          return  Regex.IsMatch(emailAdd, pattern, RegexOptions.IgnoreCase);
        }

        private async Task<bool> CheckEmailAdd(string emailAdd)
        {
            foreach(string add in emailAdd.Split(';'))
            {
                if(!(string.IsNullOrWhiteSpace(add)))
                { 
                if (!(EmailValidation(add)))
                    return false;
                }
            }
            return true;
        }
        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddAttachments_Click(object sender, EventArgs e)
        {
            EmailAttachmentsForm emailAttachmentsForm = new EmailAttachmentsForm();
            emailAttachmentsForm.EmailImages = EmailImages;
            emailAttachmentsForm.ImageType = ImageType;
            emailAttachmentsForm.ShowDialog();
            EmailAtt = emailAttachmentsForm.EmailAttachmentName;


        }

        private async void SendEmail_Click(object sender, EventArgs e)
        {
            try
            {

                if (await CheckTextBoxes())
                {
                    SE.Send_Emails.EmailInstance.SendEmail(EmailFrom.Text, EmailTO.Text, EmailCC.Text, EmailMessage.Text, EmailSubject.Text, EmailAtt);
                    MessageBox.Show($"Email sent to {EmailTO.Text}", "Emal Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Email not sent {ex.Message}", "Error Sending Emal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private async Task<bool> CheckTextBoxes()
        {
            if (string.IsNullOrWhiteSpace(EmailFrom.Text))
            {
                MessageBox.Show("Need Email From", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EmailFrom.Focus();
                return false;
            }
            else
            {
                if (!(await CheckEmailAdd(EmailFrom.Text)))
                {
                    MessageBox.Show($"Invalid Email From Address {EmailFrom.Text}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                   
            }
            if (string.IsNullOrWhiteSpace(EmailTO.Text))
            {
                MessageBox.Show("Need Email To", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EmailTO.Focus();
                return false;
            }
            else
            {
                if (!(await CheckEmailAdd(EmailTO.Text)))
                {
                    MessageBox.Show($"Invalid Email From Address {EmailTO.Text}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            if(!(string.IsNullOrWhiteSpace(EmailCC.Text)))
            {
                if (!(await CheckEmailAdd(EmailCC.Text)))
                {
                    MessageBox.Show($"Invalid Email From Address {EmailCC.Text}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(EmailSubject.Text))
            {
                MessageBox.Show("Need Email Subject", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EmailSubject.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(EmailMessage.Text))
            {
                MessageBox.Show("Need Email Message", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EmailMessage.Focus();
                return false;
            }
            if ((EmailAtt == null) || (EmailAtt.Count == 0))
            {
               DialogResult dr= MessageBox.Show("Send Email With No Attachments", "Attachments", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                    return false;
            }
            return true;
        }

        private void EmailFrom_Enter(object sender, EventArgs e)
        {
             
            ToolTip.SetToolTip(EmailFrom, "Email From");
        }

        private void EmailTO_Enter(object sender, EventArgs e)
        {
            ToolTip.SetToolTip(EmailTO, "Each Email To address seperated by ; eg(t1@t.com;e4@e.com");
        }

        private void EmailCC_Enter(object sender, EventArgs e)
        {
            ToolTip.SetToolTip(EmailCC, "Each Email To CC seperated by ; eg(t1@t.com;e4@e.com");
        }

       
    }
}
