using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using EdocsUSA.Utilities;
namespace Scanquire.Public
{
    public partial class RecoveryImages : Form
    {
        public RecoveryImages()
        {
            InitializeComponent();
        }
        public string[] ImagesFiles
        { get; set; }
      
         
        private  async void LoadFileNames()
        {
            string imageFilesFolder = SettingsManager.TempDirectoryPath;
           foreach(string file in Edocs_Utilities.EdocsUtilitiesInstance.GetFiles(imageFilesFolder, "*.txt"))
            {

                lBoxRecoveryFIles.Items.Add(Path.GetFileName(file));
            }
        }

        private void RecoveryImages_FormClosing(object sender, FormClosingEventArgs e)
        {
            lBoxRecoveryFIles.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetImageNames();
        }
        private async void GetImageNames()
        {
            if (lBoxRecoveryFIles.SelectedItems.Count > 0)
            {
                string imageFilesFolder = SettingsManager.TempDirectoryPath;
                string imageLocation = string.Empty;
               
                foreach (var item in lBoxRecoveryFIles.SelectedItems)


                {
                    imageLocation += $"{imageLocation}{File.ReadAllText(Path.Combine(imageFilesFolder, item.ToString()))}";

                }

                if (imageLocation.Trim().EndsWith(","))
                    imageLocation = imageLocation.Remove(imageLocation.Length - 1, 1);
                ImagesFiles = imageLocation.Trim().Split(',');
                this.Close();
            }
            else
                MessageBox.Show("No Image Files Select To Recovery", "No Files Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void RecoveryImages_Shown(object sender, EventArgs e)
        {
            lBoxRecoveryFIles.Items.Clear();
            LoadFileNames();
        }
    }
}
