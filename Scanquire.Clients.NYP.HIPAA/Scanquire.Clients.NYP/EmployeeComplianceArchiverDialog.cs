using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDL = EdocsUSA.Utilities.Logging;
namespace Scanquire.Clients.NYP
{
    public partial class EmployeeComplianceArchiverDialog : Form
    {

        public string LogFirstName
        {
            get { return txt_first_name.Text; }
            //set { txt_form_year = value; }
        }

        public string LogLastName
        {
            get { return txt_last_name.Text; }
            //set { txt_form_year = value; }
        }

        public string LogIDNumber
        {
            get { return txt_id_number.Text; }

        }
        public string LogDepartment
        {
            get { return department_dd.Text; }
        }

        public string LogJobTitle
        {
            get { return job_title_dd.Text; }
        }

        public string LogDocumentType
        {
            get { return document_type_dd.Text; }
        }

        public EmployeeComplianceArchiverDialog()
        {
            InitializeComponent();
            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool errors = false;
            errorProvider.Clear();
            if (string.IsNullOrWhiteSpace(txt_first_name.Text))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("Invalid First Name for Employee Compliance archiver");
                errorProvider.SetError(txt_first_name, "Invalid First Name");
                errors = true;
            }
            if (string.IsNullOrWhiteSpace(txt_last_name.Text))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("Invalid Last Name for Employee Compliance archiver");
                errorProvider.SetError(txt_last_name, "Invalid Last Name");
                errors = true;
            }

            if (string.IsNullOrWhiteSpace(txt_id_number.Text))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("Invalid Id Number for Employee Compliance archiver");
                errorProvider.SetError(txt_id_number, "Invalid Id Number");
                errors = true;
            }
            if (string.IsNullOrWhiteSpace(department_dd.Text))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("Invalid Department for Employee Compliance archiver");
                errorProvider.SetError(department_dd, "Invalid Department");
                errors = true;
            }

            if (string.IsNullOrWhiteSpace(job_title_dd.Text))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("Invalid Job Title for Employee Compliance archiver");
                errorProvider.SetError(job_title_dd, "Invalid Job Title");
                errors = true;
            }
            if (string.IsNullOrWhiteSpace(document_type_dd.Text))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("Invalid Document Type for Employee Compliance archiver");
                errorProvider.SetError(document_type_dd, "Invalid Document Type");
                errors = true;
            }
            if (!(errors))
            {
                { this.DialogResult = DialogResult.OK; }
            }
            

        }

        public void Clear()
        {
            errorProvider.Clear();
            txt_first_name.Text = string.Empty;
            txt_last_name.Text = string.Empty;
            txt_id_number.Text = string.Empty;
            department_dd.Text = string.Empty;
            job_title_dd.Text = string.Empty;
            document_type_dd.Text = string.Empty;
            txt_first_name.Focus();

        }


        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

        }


    }
}
