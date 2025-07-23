using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;
using Edocs.DOH.DL.Doc.Utilities;
   
namespace Edocs.DOH.DL.Doc
{
    public partial class ReDownDocument : Form
    {
        public ReDownDocument()
        {
            InitializeComponent();
        }

        private void ReDownDocument_Shown(object sender, EventArgs e)
        {
            lViewReDownLoad.Visible = false;
            // StringBuilder sb = new System.Text.StringBuilder();

            //lViewReDownLoad.Items.Add(new ListViewItem(new string[] { "1", "2", "3", "4", "5", "6","7" }));
            //lViewReDownLoad.Items.Add(new ListViewItem(new string[] { "1", "2", "3", "4", "5", "6", "7" }));
            toolStripStatusLabel1.Text = "Getting Documents";

            //lViewReDownLoad.Items.Add("1", 0);
            //lViewReDownLoad.Items.Add("city", 1);
            //lViewReDownLoad.Items.Add("1", 2);
            //lViewReDownLoad.Items.Add("1", 3);
            //lViewReDownLoad.Items.Add("1", 4);
            //lViewReDownLoad.Items.Add("1", 5);
            //lViewReDownLoad.Items.Add("1", 6);
            LoadDocs().ConfigureAwait(false).GetAwaiter().GetResult();
            lViewReDownLoad.Visible = true;
            toolStripStatusLabel1.Text = "Select Documents to ReDownLoad";
        }
        private async Task LoadDocs()
        {
            IList<DOHDownLoadModel> dOHDownLoads = GetDocuments().ConfigureAwait(false).GetAwaiter().GetResult();
            if((dOHDownLoads != null) && (dOHDownLoads.Count() > 0))
                AddDocumentsToList(dOHDownLoads).ConfigureAwait(false).GetAwaiter().GetResult();
            else
            {
                MessageBox.Show("No Documents found to ReDownload", "No Documents", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.Cancel;

            }
        }
        private async Task AddDocumentsToList(IList<DOHDownLoadModel> loadModels)
        {
            lViewReDownLoad.BeginUpdate();
            lViewReDownLoad.Items.Clear();

            foreach (DOHDownLoadModel documents in loadModels)
            {
                lViewReDownLoad.Items.Add(new ListViewItem(new string[] { documents.ID.ToString(),documents.City,documents.Church,documents.BookType,documents.SDate, documents.EDate,documents.FName}));
            }
            lViewReDownLoad.EndUpdate();
        }
        private async Task<IList<DOHDownLoadModel>> GetDocuments()
        {
            List<DOHDownLoadModel> dlImformaiton = new List<DOHDownLoadModel>();
            try
            {
                string jsonString = DOHDownLoadDocuments.DownLoadDocsInstance.DownLoadDOHDOc(Edocs_Constants.SpReDownLoadDOHDocuments).ConfigureAwait(false).GetAwaiter().GetResult().ToString();
                var jString = JsonConvert.DeserializeObject<DOHDownLoadModel[]>(jsonString);

                dlImformaiton = jString.ToList();

            }
            catch (Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail($"Downloading Docs {ex.Message}");
                throw new Exception($"Downloading Docs {ex.Message}");
            }
            return dlImformaiton;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectDocsToReDownload().ConfigureAwait(false).GetAwaiter().GetResult();


            this.DialogResult = DialogResult.OK;
            

            //LoadDocs().ConfigureAwait(false).GetAwaiter().GetResult();
        }
        private async Task SelectDocsToReDownload()
        {
            foreach (ListViewItem item in lViewReDownLoad.CheckedItems)
            {
                toolStripStatusLabel1.Text = $"UpDating Document ID {item.Text}";
                DOHDownLoadDocuments.DownLoadDocsInstance.UpDateDownLoadDOHDOc(int.Parse(item.Text), Edocs_Constants.SpChangeDownLoadValue).ConfigureAwait(false).GetAwaiter().GetResult();
                // Do something with the checked item
                //  Console.WriteLine(item.Text);
            }
        }
        private void lViewReDownLoad_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
