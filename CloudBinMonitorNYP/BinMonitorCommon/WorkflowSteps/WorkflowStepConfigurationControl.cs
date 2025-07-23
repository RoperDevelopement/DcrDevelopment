using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common
{
    public partial class WorkflowStepConfigurationControl : UserControl
    {
        public string Caption
        {
            get { return lblCaption.Text; }
            set { lblCaption.Text = value; }
        }

        [Browsable(false)]
        public WorkflowStepConfiguration Value
        {
            get
            {
                if (ValidateConfiguration() == false)
                { throw new InvalidOperationException("Input is not valid"); }

                WorkflowStepConfiguration configuration = new WorkflowStepConfiguration();
                configuration.EmailOnStart = chkEmailOnStart.Checked;
                configuration.EmailOnCompletion = chkEmailOnComplete.Checked;
                configuration.IncludeContentsInEmail = chkIncludeContentsInEmail.Checked;
                configuration.EmailRecipients = txtEmailRecipients.Text;

                return configuration;
            }
        }

        public WorkflowStepConfigurationControl()
        {
            InitializeComponent();
        }

        public bool ValidateConfiguration()
        {
            bool errors = false;
            ErrorProvider.Clear();
            
            if ((chkEmailOnStart.Checked || chkEmailOnComplete.Checked)
                && (string.IsNullOrWhiteSpace(txtEmailRecipients.Text)))
            {
                errors = true;
                ErrorProvider.SetError(txtEmailRecipients, "Value required");
            }

            return (errors == false);
        }

        public void LoadFromExisting(WorkflowStepConfiguration configuration)
        {
            chkEmailOnStart.Checked = configuration.EmailOnStart;
            chkEmailOnComplete.Checked = configuration.EmailOnCompletion;
            chkIncludeContentsInEmail.Checked = configuration.IncludeContentsInEmail;
            txtEmailRecipients.Text = configuration.EmailRecipients;
        }

        public void Clear()
        {
            chkEmailOnStart.Checked = false;
            chkEmailOnComplete.Checked = false;
            chkIncludeContentsInEmail.Checked = false;
            txtEmailRecipients.Clear();
        }

        private void txtEmailRecipients_Enter(object sender, EventArgs e)
        {
            txtEmailRecipients.Text = string.Empty;
           EmailAddress emailAddress = new EmailAddress();
            emailAddress.ShowDialog();
            txtEmailRecipients.Text = emailAddress.txtEmailAdd.Text;
        }
    }
}
