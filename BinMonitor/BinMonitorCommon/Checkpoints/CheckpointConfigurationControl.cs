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
    public partial class CheckpointConfigurationControl : UserControl
    {
        [Browsable(false)]
        public CheckPointConfiguration Value
        { 
            get
            {
                if (ValidateConfiguration() == false)
                { throw new InvalidOperationException("The input is not valid"); }

                CheckPointConfiguration configuration = new CheckPointConfiguration();
                
                configuration.Duration = TimeSpan.Parse(txtDuration.Text);
                configuration.Flash = chkFlash.Checked;
                configuration.EmailImmediately = chkEmailImmediately.Checked;
                configuration.EmailUntilComplete = chkEmailUntilComplete.Checked;
                if (string.IsNullOrWhiteSpace(txtEmailFrequency.Text) == false)
                { configuration.EmailFrequency = TimeSpan.Parse(txtEmailFrequency.Text); }
                if (chkEmailImmediately.Checked || chkEmailUntilComplete.Checked)
                { configuration.EmailRecipients = txtEmailRecipients.Text; }
                return configuration;
            }
        }

        public string Caption
        {
            get { return lblCaption.Text; }
            set { lblCaption.Text = value; }
        }

        public bool ValidateConfiguration()
        {
            bool errors = false;
            ErrorProvider.Clear();
            TimeSpan duration;
            if (string.IsNullOrEmpty(txtDuration.Text))
            {
                errors = true;
                ErrorProvider.SetError(txtDuration, "Value required");
            }
            if (TimeSpan.TryParse(txtDuration.Text, out duration) == false)
            {
                errors = true;
                ErrorProvider.SetError(txtDuration, "Integer value required");
            }
            TimeSpan emailFrequency;
            if ((chkEmailImmediately.Checked || chkEmailUntilComplete.Checked)
                && (((string.IsNullOrEmpty(txtEmailFrequency.Text) == false)
                && (TimeSpan.TryParse(txtEmailFrequency.Text, out emailFrequency) == false))))
            {
                errors = true;
                ErrorProvider.SetError(txtEmailFrequency, "Time Span value required");
            }
            if ((chkEmailImmediately.Checked || chkEmailUntilComplete.Checked)
                && (string.IsNullOrWhiteSpace(txtEmailRecipients.Text)))
            {
                errors = true;
                ErrorProvider.SetError(txtEmailRecipients, "Value required");
            }

            return (errors == false);
        }

        public CheckpointConfigurationControl()
        {
            InitializeComponent();
        }

        public void LoadFromExisting(CheckPointConfiguration configuration)
        {
            txtDuration.Text = configuration.Duration.ToString();
            chkEmailImmediately.Checked = configuration.EmailImmediately;
            chkEmailUntilComplete.Checked = configuration.EmailUntilComplete;
            chkFlash.Checked = configuration.Flash;
            if (configuration.EmailFrequency < TimeSpan.MaxValue)
            { txtEmailFrequency.Text = configuration.EmailFrequency.ToString(); }
            txtEmailRecipients.Text = configuration.EmailRecipients;
        }

        public void Clear()
        {
            txtDuration.Clear();
            chkEmailImmediately.Checked = false;
            chkEmailUntilComplete.Checked = false;
            chkFlash.Checked = false;
            txtEmailFrequency.Clear();
            txtEmailRecipients.Clear();
        }
        
    }
}
