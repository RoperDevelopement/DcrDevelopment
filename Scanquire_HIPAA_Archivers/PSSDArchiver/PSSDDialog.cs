using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scanquire.Public.UserControls;
using Scanquire.Public;
using Scanquire.Public.ArchivesConstants;
using EdocsUSA.Utilities.Logging;

namespace PSSDArchiver
{
    public partial class PSSDDialog : Form
    {
        public string Department
        { get; set; }
        public string OrigDepartment
        { get; set; }
        public SQImageListViewer PSDImage
        {
            get { return ImageViewerPSD; }
            set { ImageViewerPSD = value; }
        }

        public List<string> ListTrackingIDS
        { get; set; }
        public string BoxNumTrackID
        {
            get { return cmbBoxTrackingIDS.Text; }
            set { cmbBoxTrackingIDS.Text = value; }
        }

        public string RecordDepartment
        {
            get { return cmbBoxDept.Text; }
            set { cmbBoxDept.Text = value; }
        }
        public string OrgDepartment
        {
            get { return cmbBoxOrigDept.Text; }
            set { cmbBoxOrigDept.Text = value; }
        }
        public DateTime DatetRecsSDate
        {
            get { return dtRecsSDate.Value; }
            set { dtRecsSDate.Value = value; }
        }
        public DateTime DatetRecsEDate
        {
            get { return dRecsEDate.Value; }
            set { dRecsEDate.Value = value; }
        }
        public string DescRecords
        {
            get { return rTxtBoxDescRecords.Text; }
            set { rTxtBoxDescRecords.Text = value; }
        }
        public string MethFiling
        {
            get { return txtBoxMethFiling.Text; }
            set { txtBoxMethFiling.Text = value; }
        }
        public string Lname
        {
            get { return txtBoxLname.Text; }
            set { txtBoxLname.Text = value; }
        }
        public string Fname
        {
            get { return txtBoxFName.Text; }
            set { txtBoxFName.Text = value; }
        }
        public DateTime DateDOB
        {
            get { return dateDOB.Value; }
            set { dateDOB.Value = value; }
        }
        public string TotalScanned
        {
            get { return txtBoxTotalScanned.Text; }
            set { value = txtBoxTotalScanned.Text; }
        }
        public PSSDDialog()
        {
            InitializeComponent();
        }

        private void PSSDDialog_Shown(object sender, EventArgs e)
        {
            if (cmbBoxDept.Items.Count == 0)
            {
                LoadCmbBox(cmbBoxDept, Department);
            }
            if (cmbBoxOrigDept.Items.Count == 0)
            {
                LoadCmbBox(cmbBoxOrigDept, OrigDepartment);
            }
            txtBoxTotalScanned.Text = TraceLogger.TraceLoggerInstance.GetTotalImagesScannedString(false);
            AddTrackingIDS();
        }
        private async void AddTrackingIDS()
        {
            if ((ListTrackingIDS != null) && (ListTrackingIDS.Count() > 0))
            {
                if (cmbBoxTrackingIDS.Items.Count == 0)
                {
                    cmbBoxTrackingIDS.BeginUpdate();
                    foreach (string id in ListTrackingIDS)
                        cmbBoxTrackingIDS.Items.Add(id);
                    cmbBoxTrackingIDS.Sorted = true;
                     cmbBoxTrackingIDS.SelectedIndex = 0;
                    cmbBoxTrackingIDS.EndUpdate();
                }
            }
        }
        private async void LoadCmbBox(ComboBox comboBox, string items)
        {
            comboBox.BeginUpdate();
            foreach (string strItems in items.Split('|'))
            {
                comboBox.Items.Add(strItems);
            }
           // comboBox.SelectedIndex = 0;
            comboBox.EndUpdate();
            PSDImage.SplitContainer.SplitterDistance = 90;
            PSDImage.ThumbnailToolStrip.Visible = false;

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
