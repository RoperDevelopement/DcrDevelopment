using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using EDL = EdocsUSA.Utilities.Logging;
namespace Scanquire.Clients.NYP
{
    public partial class SpecimenRejectionRecordDialog : Form
    {
        public DateTime? ScanDate
        {
            get { return dpLogDate.Value; }
            set { dpLogDate.Value = value; }
        }
        public int CaseNumberMinLength
        { get; set; }
        public int CaseNumberSplitMinLength
        { get; set; }
        private string KeyData
        { get; set; }
        public bool CaseNumberRequiresDash
        { get; set; }
        public string RejectionReasonsFileDir
        { get; set; }
        public string RejectionResons
        { get; set; }
        public string CaseNumber
        {
            get { return txt_caseNumber.Text; }

        }
        public string LogReason
        {
            get { return cmbFormType.Text; }
        }


        public SpecimenRejectionRecordDialog()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool errors = false;
            errorProvider.Clear();

            if (ScanDate.HasValue == false)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("Scan Date require for Specimen Rejection Archiver");

                errorProvider.SetError(dpLogDate, "Scan Date require");
                errors = true;
            }
            if (string.IsNullOrWhiteSpace(txt_caseNumber.Text))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("Case Number require for Specimen Rejection Archiver");
                errorProvider.SetError(txt_caseNumber, "Case Number Required");

                errors = true;
            }



            if ((txt_caseNumber.Text.Length <= CaseNumberMinLength))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Invalid Case Number {txt_caseNumber.Text}  for Specimen Rejection Archiver");
                errorProvider.SetError(txt_caseNumber, string.Format("Invalid Case Number:{0} must have {1} characters or more (xxx-xxx) or  (xxxxxx)", txt_caseNumber.Text, CaseNumberMinLength));
                errors = true;
            }
            if (CaseNumberRequiresDash)
            {
                string[] caseNum = txt_caseNumber.Text.Split('-');
                if ((caseNum[0].Length > CaseNumberSplitMinLength) || (caseNum[0].Length == 1))
                {
                    EDL.TraceLogger.TraceLoggerInstance.TraceError($"Invalid Case Number {txt_caseNumber.Text}  for Specimen Rejection Archiver");
                    errorProvider.SetError(txt_caseNumber, string.Format("Invalid start Case Number:{0} must have  characters {1} before -  (xxx-xxx)", caseNum[0], CaseNumberSplitMinLength));
                    errors = true;
                }
            }
            //else
            //{
            //    if ((txt_caseNumber.Text.Trim().Length < CaseNumberMinLength))
            //    {
            //        EDL.TraceLogger.TraceLoggerInstance.TraceError($"Invalid Case Number {txt_caseNumber.Text}  for Specimen Rejection Archiver");
            //        errorProvider.SetError(txt_caseNumber, string.Format("Invalid Case Number:{0} must have {1} characters or more (xxxxxx)", txt_caseNumber.Text, CaseNumberMinLength));
            //        errors = true;
            //    }
            //}
            if (string.IsNullOrWhiteSpace(cmbFormType.Text))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Reason Required  for Specimen Rejection Archiver");
                errorProvider.SetError(cmbFormType, "Reason Required");
                errors = true;
            }

            if (errors == false)
            {
                CheckReason();
                this.DialogResult = DialogResult.OK;
            }
        }

        public void Clear()
        {
            errorProvider.Clear();
            dpLogDate.Clear();
            txt_caseNumber.Text = string.Empty;
            cmbFormType.Text = string.Empty;
            dpLogDate.Value = DateTime.Now;
        }

        protected void CheckReason()
        {
            string reason = cmbFormType.Text;
            for (int i = 0; i < cmbFormType.Items.Count; i++)
            {
                if (string.Compare(reason, cmbFormType.Items[i].ToString(), true) == 0)
                    return;

            }
            AddRejection(reason);
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            dpLogDate.Focus();
        }

        private void SpecimenRejectionRecordDialog_Load(object sender, EventArgs e)
        {
            dpLogDate.Value = DateTime.Now;
            Clear();
            cmbFormType.BeginUpdate();
            cmbFormType.Items.Clear();
            foreach (var rReasons in RejectionResons.Split(','))
            {
                if (!(string.IsNullOrWhiteSpace(rReasons)))
                {
                    cmbFormType.Items.Add(rReasons);
                }

            }
            cmbFormType.Sorted = true;
            cmbFormType.Text = string.Empty;
            cmbFormType.EndUpdate();

        }
        private void AddRejection(string newRej)
        {
            if (!(string.IsNullOrWhiteSpace(newRej)))
            {
                cmbFormType.BeginUpdate();
                cmbFormType.Items.Add(newRej);
                cmbFormType.Sorted = true;
                cmbFormType.Text = newRej;
                cmbFormType.EndUpdate();
                UpDateRejFile(newRej);


            }
            else
                MessageBox.Show("No New Rejecton Reason Added", "Need Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void UpDateRejFile(string newReason)
        {
            string rejFolder = RejectionReasonsFileDir.Replace("{UserFolder}", $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}");
            if (!(System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(rejFolder))))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(rejFolder));
            }
            if (System.IO.File.Exists(rejFolder))
            {
                string txtRejRes = System.IO.File.ReadAllText(rejFolder);
                txtRejRes = $"{txtRejRes},{newReason}";
                System.IO.File.WriteAllText(rejFolder, txtRejRes);


            }
            else
                System.IO.File.WriteAllText(rejFolder, newReason);
        }

        private void txt_caseNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                cmbFormType.Focus();
        }



        private void cmbFormType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnOk.PerformClick();
        }


    }
}

