using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

namespace Scanquire.Public.EdocsUSAImageTools

{
    public partial class InsertImageForm : Form
    {
        OpenFileDialog oDlg;
        public InsertImageForm()
        {
            InitializeComponent();
            //oDlg = new OpenFileDialog(); // Open Dialog Initialization
           // oDlg.RestoreDirectory = true;
          //  oDlg.InitialDirectory = "C:\\";
          //  oDlg.Filter = "Jpg|*.jpg|gif|*.gif|png|*.png |bmp|*.bmp";
            // oDlg.FilterIndex = 1;
            //  oDlg.Filter = "jpg Files|*.jpg|gif Files|*.gif|png Files|*.png |bmp Files|*.bmp";
            btnOK.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
        }

        public int XPosition
        {
            get 
            {
                if (string.IsNullOrEmpty(txtX.Text))
                    txtX.Text = "0";
                return Convert.ToInt32(txtX.Text); 
            }
            set { txtX.Text = value.ToString(); }
        }

        public int YPosition
        {
            get
            {
                if (string.IsNullOrEmpty(txtY.Text))
                    txtY.Text = "0";
                return Convert.ToInt32(txtY.Text);
            }
            set { txtY.Text = value.ToString(); }
        }

        public int ImageWidth
        {
            get
            {
                if (string.IsNullOrEmpty(txtWidth.Text))
                    txtWidth.Text = "0";
                return Convert.ToInt32(txtWidth.Text);
            }
            set { txtWidth.Text = value.ToString(); }
        }

        public int ImageHeight
        {
            get
            {
                if (string.IsNullOrEmpty(txtHeight.Text))
                    txtHeight.Text = "0";
                return Convert.ToInt32(txtHeight.Text);
            }
            set { txtHeight.Text = value.ToString(); }
        }

        public string DisplayImagePath
        {
            get { return txtImage.Text; }
            set { txtImage.Text = value.ToString(); }
        }

        private void btnSelect_Click(object sender, EventArgs e)
{
            openFileDialog1.Filter = "Jpg|*.jpg|gif|*.gif|png|*.png|bmp|*.bmp|tif|*.tif";
            openFileDialog1.FilterIndex = 3;
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                txtImage.Text = openFileDialog1.FileName;
            }
        }
    }
}