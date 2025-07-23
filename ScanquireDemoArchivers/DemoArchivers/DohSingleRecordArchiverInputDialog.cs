using Scanquire.Public.Sharepoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoArchivers
{
    public partial class DohSingleRecordArchiverInputDialog : Form
    {
        public SharepointRestCredentials Credentials
        { get { return this.SharepointCredentialsControl; } }

        public string AccessionNumber
        {
            get { return AccessionNumberTextBox.Text; }
            set { AccessionNumberTextBox.Text = value; }
        }

        public string FinancialNumber
        {
            get { return FinancialNumberTextBox.Text; }
            set { FinancialNumberTextBox.Text = value; }
        }

        public DateTime DateOfService
        {
            get { return DateOfServicePicker.Value; }
            set { DateOfServicePicker.Value = value; }
        }

        public string MedicalRecordNumber
        {
            get { return MedicalRecordNumberTextBox.Text; }
            set { MedicalRecordNumberTextBox.Text = value; }
        }

        private BindingList<string> _PerformingLabs = new BindingList<string>();
        public BindingList<string> PerformingLabs
        {
            get { return _PerformingLabs; }
            set { _PerformingLabs = value; }
        }

        public string PerformingLab
        {
            get { return (string)PerformingLabComboBox.SelectedValue; }
            set { PerformingLabComboBox.SelectedValue = value; }
        }

        public DohSingleRecordArchiverInputDialog()
        {
            InitializeComponent();
            PerformingLabComboBox.DataSource = PerformingLabs;
        }

        public void Clear()
        {
            AccessionNumberTextBox.Clear();
            FinancialNumberTextBox.Clear();
            MedicalRecordNumberTextBox.Clear();

        }

        private async void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;
                StatusLabel.Text = "Testing connection, please wait...";
                await Task.Factory.StartNew(() =>
                {
                    SharepointRestConnector connector = new SharepointRestConnector(Credentials.ServerAddress, Credentials.AuthorizationHeader, "Documents");
                    connector.TestConnection();
                });
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                string message = string.Concat(
                    "Failed to connect to the server, details:"
                    , Environment.NewLine, ex.Message
                    , Environment.NewLine, "Select OK to proceed anyway (records will need to be manually uploaded later)"
                    , Environment.NewLine, "Select Cancel to re-enter credentials or cancel");
                DialogResult r = MessageBox.Show(this, message, "Error Connecting", MessageBoxButtons.OKCancel);
                if (r == DialogResult.OK)
                { this.DialogResult = DialogResult.OK; }
            }
            finally
            {
                this.Enabled = true;
                this.StatusLabel.Text = string.Empty;
            }
        }
    }
}
