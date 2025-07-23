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
    public partial class ComplianceLogRecordDialog : Form
    {
        public string LogStation
        {
            get { return cmbLogStation.Text; }
            set { cmbLogStation.Text = value; }
        }

        public string LogStationFoler
        { get; set; }
        public DateTime? LogDate
        {
            get { return dpLogDate.Value; }
            set { dpLogDate.Value = value; }
        }
        public string CmbBoxItems
        { get; set; }
        public Image CurrentImage
        {
            get { return imgCurrent.Image; }
            set
            {
                if (imgCurrent.Image != null)
                { imgCurrent.Image.Dispose(); }
                imgCurrent.Image = value;
            }
        }

        public ComplianceLogRecordDialog()
        {
            InitializeComponent();

            txtBoxLogStation.Text = string.Empty;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool errors = false;
            errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(LogStation))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("ComplianceLog archiver cmbLogStation value required");
                errorProvider.SetError(cmbLogStation, "Value Required");
                errors = true;
            }

            if (LogDate.HasValue == false)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("ComplianceLog archiver dplogdate value required");
                errorProvider.SetError(dpLogDate, "Value Required");
                errors = true;
            }

            if (errors == false)
            { this.DialogResult = DialogResult.OK; }
        }

        private void UpDateRejFile(string newlogStation)
        {
            string lStationFolder = LogStationFoler.Replace("{UserFolder}", $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}");
            if (!(System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(lStationFolder))))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(lStationFolder));
            }
            if (System.IO.File.Exists(lStationFolder))
            {
                string txtRejRes = System.IO.File.ReadAllText(lStationFolder);
                txtRejRes = $"{txtRejRes},{newlogStation}";
                System.IO.File.WriteAllText(lStationFolder, txtRejRes);


            }
            else
                System.IO.File.WriteAllText(lStationFolder, newlogStation);
        }
        public void Clear()
        {
            errorProvider.Clear();
            cmbLogStation.SelectedIndex = -1;
            dpLogDate.Clear();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (cmbLogStation.Items.Count == 0)
            {


                cmbLogStation.BeginUpdate();
                cmbLogStation.Items.Clear();
                cmbLogStation.Items.Add("");
                cmbLogStation.Sorted = true;
                foreach (string cItems in CmbBoxItems.Split(','))
                {
                    if (!(string.IsNullOrWhiteSpace(cItems)))
                        cmbLogStation.Items.Add(cItems.Trim());
                }
                cmbLogStation.EndUpdate();
                cmbLogStation.SelectedIndex = -1;
                dpLogDate.Focus();
            }
        }

        private void btnAddLogStation_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBoxLogStation.Text))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("ComplianceLog archiver New log station cannot be blank");
                errorProvider.SetError(txtBoxLogStation, "Log Station Name Required");
            }
            else
            {
                if (!(cmbLogStation.Items.Contains(txtBoxLogStation.Text.Trim())))
                {
                    cmbLogStation.Items.Add(txtBoxLogStation.Text.Trim());
                    CmbBoxItems = $"{CmbBoxItems},{txtBoxLogStation.Text.Trim()}";
                    cmbLogStation.Text = txtBoxLogStation.Text.Trim();
                    UpDateRejFile(txtBoxLogStation.Text.Trim());
                }
                else
                {
                    errorProvider.SetError(txtBoxLogStation, $"Log Station {txtBoxLogStation.Text} all ready exists");
                    cmbLogStation.Text = txtBoxLogStation.Text;
                }
                txtBoxLogStation.Text = string.Empty;


            }
        }
    }
}
