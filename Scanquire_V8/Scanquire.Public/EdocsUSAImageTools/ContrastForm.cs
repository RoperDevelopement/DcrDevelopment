using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Scanquire.Public.EdocsUSAImageTools
{
    public delegate void ChangeContrast(double contrast);
    public delegate void ImageRestore();
    public partial class ContrastForm : Form
    {
        public event ChangeContrast UpdateContrast;
        public event ImageRestore RestoreImageCont;
        public ContrastForm()
        {
            InitializeComponent();
            btnOK.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

        }

        public double ContrastValue
        {
            get
            {
                if (string.IsNullOrEmpty(txtContrastValue.Text))
                    txtContrastValue.Text = "0";
                return Convert.ToDouble(txtContrastValue.Text);
            }
            set { txtContrastValue.Text = value.ToString(); }
        }



        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            ContrastValue = trackBar1.Value;
        }

        private void txtContrastValue_TextChanged(object sender, EventArgs e)
        {

            trackBar1.Value = (int)ContrastValue;
            UpdateContrast(ContrastValue);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ContrastValue = 0;
            trackBar1.Value = 0;
            RestoreImageCont();

        }


    }
}