using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EdocsUSA.Scanquire.Clients.PSE
{
    public partial class PSEFinancialRecordsDialog : Form
    {
        public PSEFinancialRecordsDialog()
        {
            InitializeComponent();
        }
        public string CmbBoxItems
        { get; set; }
        public DateTime StYear
        {
            get { return dtpSFinYear.Value; }
            set { dtpSFinYear.Value = value; }
        }
        public DateTime EndYear
        {
            get { return dtpEFinYear.Value; }
            set { dtpEFinYear.Value = value; }
        }
        public string FinIdentifier
        {
            get { return cmbBoxIdentifer.Text; }
            set { cmbBoxIdentifer.Text = value; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void PSEFinancialRecordsDialog_Shown(object sender, EventArgs e)
        {
            cmbBoxIdentifer.Items.Clear();
            cmbBoxIdentifer.BeginUpdate();
            cmbBoxIdentifer.Sorted = true;
            foreach (string cItems in CmbBoxItems.Split(','))
                cmbBoxIdentifer.Items.Add(cItems);
            cmbBoxIdentifer.EndUpdate();
            dtpEFinYear.Value = DateTime.Now;
            dtpEFinYear.Value = DateTime.Now;



        }
    }
}
