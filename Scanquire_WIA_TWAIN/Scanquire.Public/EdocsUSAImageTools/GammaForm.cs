using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Scanquire.Public.EdocsUSAImageTools
{
    public partial class GammaForm : Form
    {
        public GammaForm()
        {
            InitializeComponent();
            btnOK.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            
        }

        public double RedComponent
        {
            get 
            {
                if (string.IsNullOrEmpty(txtRedValue.Text))
                    txtRedValue.Text = "0";
                return Convert.ToDouble(txtRedValue.Text); 
            }
            set { txtRedValue.Text = value.ToString(); }
        }

        public double GreenComponent
        {
            get
            {
                if (string.IsNullOrEmpty(txtGreenValue.Text))
                    txtGreenValue.Text = "0";
                return Convert.ToDouble(txtGreenValue.Text);
            }
            set { txtGreenValue.Text = value.ToString(); }
        }

        public double BlueComponent
        {
            get
            {
                if (string.IsNullOrEmpty(txtBlueValue.Text))
                    txtBlueValue.Text = "0";
                return Convert.ToDouble(txtBlueValue.Text);
            }
            set { txtBlueValue.Text = value.ToString(); }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if ((GreenComponent < 0.2) || (GreenComponent > 5))
            {
                if(GreenComponent != 0)
                { 
                MessageBox.Show($"Green color between 0.2 and 5 invalid {GreenComponent.ToString()}", "Invalid Gamma", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    GreenComponent = 0;
                    txtGreenValue.Focus();
                e.Cancel = true;
                }
            }
            if ((BlueComponent < 0.2) || (BlueComponent > 5))
            {
                if (BlueComponent != 0)
                {
                    MessageBox.Show($"Blue color between 0.2 and 5 invalid {BlueComponent.ToString()}", "Invalid Gamma", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    BlueComponent = 0;
                    txtBlueValue.Focus();
                    e.Cancel = true;
                }
            }
            if ((RedComponent < 0.2) || (RedComponent > 5))
            {
                if (RedComponent != 0)
                {
                    MessageBox.Show($"Red color between 0.2 and 5 invalid {RedComponent.ToString()}", "Invalid Gamma", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    RedComponent = 0;
                  txtRedValue.Focus();
                    e.Cancel = true;
                }
            }
        }
            
    }
}