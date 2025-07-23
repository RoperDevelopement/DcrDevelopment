using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Edocs.Dillion.VCC.Archiver
{
    public partial class AddArchiverInfo : Form
    {
        public AddArchiverInfo()
        {
            InitializeComponent();
        }
        public string LabelText
        { get; set; }
        public string CityChurchBookType
        { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            if(string.IsNullOrWhiteSpace(rTxtBOxCityChurchBt.Text))
            {
                MessageBox.Show("Text box cannot be empty", "Empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }    
            CityChurchBookType = rTxtBOxCityChurchBt.Text.Trim();

        }

        private void AddArchiverInfo_Shown(object sender, EventArgs e)
        {
            rTxtBOxCityChurchBt.Text = CityChurchBookType;
            labCityBTChurch.Text = $"{LabelText} e.g.(each one followed by a command 1,2,etc..)";
          

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
