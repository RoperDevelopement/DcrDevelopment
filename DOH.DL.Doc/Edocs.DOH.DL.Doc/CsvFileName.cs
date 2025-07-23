using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edocs.DOH.DL.Doc
{
    public partial class CsvFileName : Form
    {
        public string CsvDownLoadFileName
        {
            get { return txtCSVFName.Text; }
            set { txtCSVFName.Text = value; }
        }
        public string CsvDownLoadFolder
        {
            get { return txtBoxCSVDLFolder.Text; }
            set { txtBoxCSVDLFolder.Text = value; }
        }
        public CsvFileName()
        {
            InitializeComponent();
        }

        private async Task GetDownloadFolder()
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Select a CSV download folder";
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                folderBrowserDialog.ShowNewFolderButton = true;
                folderBrowserDialog.ShowNewFolderButton = true;
                if(!(string.IsNullOrWhiteSpace(CsvDownLoadFolder)))
                folderBrowserDialog.SelectedPath = CsvDownLoadFolder;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {

                    CsvDownLoadFolder = folderBrowserDialog.SelectedPath;
                    Properties.Settings.Default.CSVDownLoadFolder = CsvDownLoadFolder;
                    txtBoxCSVDLFolder.Text = CsvDownLoadFolder;
                  //  statusLabel.Text = $"DownLoad Folder:{DownLoadFoler}";
                    //  Properties.DownLoad.Default.DownLoadFolder = folderBrowserDialog.SelectedPath;
                    //  Properties.DownLoad.Default.Save();
                }
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace((txtCSVFName.Text)))
                {
                MessageBox.Show("Need a CSV FileName", "File Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((string.IsNullOrWhiteSpace(txtBoxCSVDLFolder.Text)))
            {
                MessageBox.Show("Need a CSV DownLoad Folder", "Downlaod Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!(txtCSVFName.Text.ToLower().EndsWith(".csv")))
                txtCSVFName.Text = $"{txtCSVFName.Text}.csv";
            if(!(System.IO.Directory.Exists(txtBoxCSVDLFolder.Text.Trim())))
            {
                MessageBox.Show($"Could not find Download Folder {txtBoxCSVDLFolder.Text}", "Downlaod Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                this.DialogResult = DialogResult.OK;
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cSVDownloadFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetDownloadFolder().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            GetDownloadFolder().ConfigureAwait(false).GetAwaiter().GetResult();
        }

         
    }
}
