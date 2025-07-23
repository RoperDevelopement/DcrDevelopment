using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EdocsUSA.Controls;
using EdocsUSA.Utilities.Logging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using Scanquire.Public.UserControls;
namespace Edocs.BSB.PublicWorks.Dep.Archiver
{
    public partial class BSBPPublicWorksDepArchiverDialog : Form
    {
        int x = 0;
        int y = 0;

        public bool ShowTotalDocsScanned
        { get; set; }
        public bool IncludeBlankDocs
        { get; set; }
        public string ImageFolder
        { get; set; }
        public BSBPPublicWorksDepArchiverDialog()
        {
            InitializeComponent();
        }
        public SQImageListViewer ImageViewerPWD
        {
            get { return PWDImageViewer; }
            set { PWDImageViewer = value; }
        }
        public string CmbBoxDep
        { get; set; }
        public string Department
        {
            get { return cmbBoxDept.Text; }
            set { cmbBoxDept.Text = value; }
        }
        public string ProjectYear
        {
            get { return cmbBoxYear.Text; }

            set { cmbBoxYear.Text = value; }
        }
        public string ProjectName
        {
            get { return txtBoxProjectName.Text; }
            set { txtBoxProjectName.Text = value; }
        }






        public string TotalScanned
        {
            get { return txtBoxTotalScanned.Text; }
            set { value = txtBoxTotalScanned.Text; }
        }


        private void btnOk_Click(object sender, EventArgs e)
        {

            //    if (!(int.TryParse(txtBoxPerNumber.Text, out int resultsPer)))
            //{
            //    MessageBox.Show("Permit Number required");
            //    return;
            //}
            if (string.IsNullOrWhiteSpace(txtBoxProjectName.Text))
            {
                MessageBox.Show("Need a Project Name", "Project Name", MessageBoxButtons.OK, MessageBoxIcon.Error);


                    return;
            }
            if (string.IsNullOrWhiteSpace(cmbBoxYear.Text))
            {
                MessageBox.Show("Need a Year", "Year", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }
            if (string.IsNullOrWhiteSpace(cmbBoxDept.Text))
            {
                MessageBox.Show("Need a Project Department", "Departmen", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

        }


        private void BSBPlanDepArchiverDialog_Shown(object sender, EventArgs e)
        {
            //  txtBoxPerNumber.Text = string.Empty;

            InitCmbBoxes();
            txtBoxTotalScanned.Text = TraceLogger.TraceLoggerInstance.GetTotalImagesScannedString(IncludeBlankDocs);
            txtBoxProjectName.Text = string.Empty;
            cmbBoxDept.Focus();

        }
        private void InitCmbBoxes()
        {
            if (cmbBoxYear.Items.Count == 0)
            {
                cmbBoxYear.BeginUpdate();
                int currYear = DateTime.Now.Year;
                for (int i = currYear; i >= 1800; i--)
                    cmbBoxYear.Items.Add(i.ToString());
                cmbBoxYear.EndUpdate();
            }
            if (cmbBoxDept.Items.Count == 0)
            {
                cmbBoxDept.BeginUpdate();
                foreach (string s in CmbBoxDep.Split('|'))
                {
                    cmbBoxDept.Items.Add(s);
                }
                cmbBoxDept.EndUpdate();
            }
        }


    }
}


