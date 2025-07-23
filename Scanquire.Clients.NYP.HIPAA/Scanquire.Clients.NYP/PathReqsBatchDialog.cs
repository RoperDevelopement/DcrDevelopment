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

namespace Scanquire.Clients.NYP
{
    public partial class PathReqsBatchDialog : Form
    {
        public void Clear()
        {
            dpReceiptDate.Value = DateTime.Now.Date;
            rad_CatUncategorized.Checked = true;
        }

        public DateTime? ReceiptDate
        {
            get { return this.dpReceiptDate.Value; }
            set { this.dpReceiptDate.Value = value; }
        }
        public DateTime? DateOfService
        {
            get { return this.dpDateOfService.Value; }
            set { this.dpDateOfService.Value = value; }
        }


        public string Category
        {
            get
            {
                if (radCat_AMStats.Checked)
                { return "A.M. STATS"; }
                else if (rad_CatUncategorized.Checked)
                { return ""; }
                else if (radCat_Routine.Checked)
                { return "ROUTINE"; }
                else if (radCat_Stats.Checked)
                { return "STATS"; }
                else
                { return null; }
            }
            set
            {

                radCat_AMStats.Checked = value.Equals("A.M. STATS", StringComparison.OrdinalIgnoreCase);
                rad_CatUncategorized.Checked = value.Equals("", StringComparison.OrdinalIgnoreCase);
                radCat_Routine.Checked = value.Equals("ROUTINE", StringComparison.OrdinalIgnoreCase);
                radCat_Stats.Checked = value.Equals("STATS", StringComparison.OrdinalIgnoreCase);

            }
        }

        public PathReqsBatchDialog()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Enabled = true;
                this.StatusLabel.Text = string.Empty;
            }
        }

    }
}
