using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Edocs.DOH.DL.Doc.Utilities;
using Newtonsoft.Json;
namespace Edocs.DOH.DL.Doc
{
    public partial class DOHMainForm : Form
    {
        delegate void DLDohDoc();
        public DOHMainForm()
        {
            InitializeComponent();
            
            // dataGVDlFiles.Rows.Add("1", "City","Church", "BookType","Sate","e", @"C:\Archives", "Butte St Joseph Marriage Register 07-01-1988 thru 09-17-2024.pdf", $"Mib", "View", "Reject");
            ///lll dataGVDlFiles.Rows.Add("1", "City", "Church", "BookType", "Sate", "e", @"C:\Archives", "Butte St Joseph Marriage Register 07-01-1988 thru 09-17-2024.pdf", $"Mib", "View", "Reject");
        }
        private string DownLoadFoler
        { get; set; }
        private string CSVDownLoadFolder
        { get; set; }
        private string CSVFileName
        { get; set; }
        private string DocID
        { get; set; }
        private string DocFolder
        { get; set; }
        private string CsvFName
        { get; set; }
        private async Task GetDrives()
        {
            string[] drives = Environment.GetLogicalDrives();
            foreach (string drive in drives)
            {
                DriveInfo di = new DriveInfo(drive);
                int driveImage;

                switch (di.DriveType)    //set the drive's icon
                {
                    case DriveType.CDRom:
                        driveImage = 3;
                        break;
                    case DriveType.Network:
                        driveImage = 6;
                        break;
                    case DriveType.NoRootDirectory:
                        driveImage = 8;
                        break;
                    case DriveType.Unknown:
                        driveImage = 8;
                        break;
                    default:
                        driveImage = 2;
                        break;
                }

                TreeNode node = new TreeNode(drive.Substring(0, 1), driveImage, driveImage);
                node.Tag = drive;

                if (di.IsReady == true)
                    node.Nodes.Add("...");

                compDrives.Nodes.Add(node);
            }
            TreeNode nodeToSelect = compDrives.Nodes[0]; // Replace with your target node
            compDrives.SelectedNode = nodeToSelect;
            compDrives.SelectedNode.BackColor = compDrives.BackColor;
            compDrives.SelectedNode.ForeColor = compDrives.ForeColor;
        }

        private void DOHMainForm_Shown(object sender, EventArgs e)
        {
            DOHMainForm.ActiveForm.Text= $"e_Docs USA Inc. Download DOH Documents Version {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
            DownLoadFoler = Properties.Settings.Default.DownLoadFoler;
            statusLabel.Text = $"DownLoad Folder {DownLoadFoler}";
            CsvFName = Properties.Settings.Default.CSVFileName;
            CSVDownLoadFolder = Properties.Settings.Default.CSVDownLoadFolder;
            if(string.IsNullOrWhiteSpace(CSVDownLoadFolder))
            {
                CSVDownLoadFolder = DownLoadFoler;
            }
            //    GetDrives().ConfigureAwait(false).GetAwaiter().GetResult();
            //   viewFiles.Rows.Add("c:\\", "No Files Found", "0", "");
            // AddFilesGView("C:\\").ConfigureAwait(false).GetAwaiter().GetResult();
            //DownLoadFoler =   Properties.DownLoad.Default.DownLoadFolder;
            //  if (string.IsNullOrWhiteSpace(DownLoadFoler))
            //      GetDownloadFolder().ConfigureAwait(false).GetAwaiter().GetResult();
            // string s = DOH_Utilites.DOHUtilitiesInstance.GetProgramDataFolder;
            //DOHDownLoadDocuments.DownLoadDocsInstance.DownLoadDOHDOcCmd().ConfigureAwait(false).GetAwaiter().GetResult();
            // DownloadDocs().ConfigureAwait(false).GetAwaiter().GetResult();
            if (string.IsNullOrWhiteSpace(DownLoadFoler))
            {
                statusLabel.Text = $"Need to select a DownLoad Folder";
            }

        }
        private async Task<IList<DOHDownLoadModel>> DownloadDocs()
        {
            List<DOHDownLoadModel> dlImformaiton = new List<DOHDownLoadModel>();
            try
            {
                
                
                string jsonString = DOHDownLoadDocuments.DownLoadDocsInstance.DownLoadDOHDOc(Edocs_Constants.SpDownLoadDOHDocuments).ConfigureAwait(false).GetAwaiter().GetResult().ToString();
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
        private void compDrives_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                if (e.Node.Nodes[0].Text == "..." && e.Node.Nodes[0].Tag == null)
                {
                    e.Node.Nodes.Clear();

                    //get the list of sub direcotires
                    string[] dirs = Directory.GetDirectories(e.Node.Tag.ToString());

                    foreach (string dir in dirs)
                    {
                        DirectoryInfo di = new DirectoryInfo(dir);
                        TreeNode node = new TreeNode(di.Name, 0, 1);

                        try
                        {
                            //keep the directory's full path in the tag for use later
                            node.Tag = dir;

                            //if the directory has sub directories add the place holder
                            if (di.GetDirectories().Count() > 0)
                                node.Nodes.Add(null, "...", 0, 0);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            //display a locked folder icon
                            node.ImageIndex = 12;
                            node.SelectedImageIndex = 12;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "DirectoryLister",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            e.Node.Nodes.Add(node);
                        }
                    }
                }
            }
        }

        private void DelDlDocs()
        {
            if (dataGVDlFiles.RowCount > 0)
            {
                dataGVDlFiles.CurrentCell = dataGVDlFiles[1, 0];
                dataGVDlFiles.BeginEdit(true);
            }
            dataGVDlFiles.Rows.Clear();
            cancelDownLoads.Visible = true;
            tabFiles.SelectedIndex = 0; ;
            proBar.Visible = true;
            //DownDOHLoadDocs().ConfigureAwait(false).GetAwaiter().GetResult();
           bgWorker.RunWorkerAsync();
        }
        private void documentsNotDownloadedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {


                if (string.IsNullOrWhiteSpace(DownLoadFoler))
                    GetDownloadFolder().ConfigureAwait(false).GetAwaiter().GetResult();

                if (!(bgWorker.IsBusy))
                {
                    CsvFileName cvsFileName = new CsvFileName();
                    //  cvsFileName.ShowDialog();
                    cvsFileName.CsvDownLoadFileName = CsvFName.Trim();
                    cvsFileName.CsvDownLoadFolder = CSVDownLoadFolder.Trim();
                    if (cvsFileName.ShowDialog() == DialogResult.OK)
                     {
                        CsvFName = cvsFileName.CsvDownLoadFileName;
                        CSVDownLoadFolder = cvsFileName.CsvDownLoadFolder;
                        
                        Properties.Settings.Default.CSVFileName = CsvFName;
                        Properties.Settings.Default.CSVDownLoadFolder = CSVDownLoadFolder;
               // DelDlDocs();
                        DLDohDoc dowonLoadDocs = new DLDohDoc(DelDlDocs);

                dataGVDlFiles.Invoke(dowonLoadDocs);
                    }


                    

                }
            }
            catch (Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail($"Error Downloading Doc {ex.Message}");
                MessageBox.Show($"Error Downloading Docs {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task DownDOHLoadDocs()
        {

            IList<DOHDownLoadModel> dOHDownLoadModels = DownloadDocs().ConfigureAwait(false).GetAwaiter().GetResult();
            DOHDownLoadDocuments.DownLoadDocsInstance.DownLoadDocs(dOHDownLoadModels, dataGVDlFiles, DownLoadFoler, statusLabel, proBar, CsvFName,CSVDownLoadFolder).ConfigureAwait(false).GetAwaiter().GetResult();

        }

        //private void viewFiles_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex == viewFiles.Columns["View"].Index && e.RowIndex >= 0)
        //    {
        //        // Perform your action here
        //        MessageBox.Show("Button clicked in row " + e.RowIndex.ToString());
        //    }
        //}
        private async Task AddFilesGView(string folderName)
        {
            string[] fNames = DOH_Utilites.DOHUtilitiesInstance.GetFiles(folderName, "*.*");


            if (viewFiles.RowCount > 0)
            {
                this.viewFiles.CurrentCell = this.viewFiles[1, 0];
                viewFiles.BeginEdit(true);
            }


            viewFiles.Rows.Clear();
            foreach (string fName in fNames)
            {
                FileInfo fileInfo = new FileInfo(fName);
                long fileSize = fileInfo.Length;
                double fileSizeInKB = fileSize / 1024.0;

                viewFiles.Rows.Add(folderName, Path.GetFileName(fName), $"{fileSize.ToString()} kb", "View");
            }
            if (fNames.Count() == 0)
            {
                viewFiles.Rows.Add(folderName, "No Files Found", "0", "");
            }
            viewFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            viewFiles.EndEdit();

        }
        private void compDrives_AfterExpand(object sender, TreeViewEventArgs e)
        {


            AddFilesGView(e.Node.Tag.ToString()).ConfigureAwait(true).GetAwaiter().GetResult();



        }

        private void compDrives_AfterSelect(object sender, TreeViewEventArgs e)
        {
            AddFilesGView(e.Node.Tag.ToString()).ConfigureAwait(true).GetAwaiter().GetResult();
        }

        private void selectRootDownloadFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(bgWorker.IsBusy))
                {
                    GetDownloadFolder().ConfigureAwait(false).GetAwaiter().GetResult();
                    // bgWorker.WorkerReportsProgress = true;
                }
            }
            catch (Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail($"Error Selecting download folder {ex.Message}");
                MessageBox.Show($"Error Selecting download folder {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private async Task GetDownloadFolder()
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Select a download folder";
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                folderBrowserDialog.ShowNewFolderButton = true;

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {

                    DownLoadFoler = folderBrowserDialog.SelectedPath;
                    Properties.Settings.Default.DownLoadFoler = DownLoadFoler;
                 if(DOH_Utilites.DOHUtilitiesInstance.HasAccess(DownLoadFoler))
                        statusLabel.Text = $"DownLoad Folder:{DownLoadFoler}";
                 else
                    statusLabel.Text = $"No write access to DownLoad Folder:{DownLoadFoler}";
                    //  Properties.DownLoad.Default.DownLoadFolder = folderBrowserDialog.SelectedPath;
                    //  Properties.DownLoad.Default.Save();
                }
            }

        }

        private void tabFiles_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                if (e.TabPageIndex == 1)
                {

                    GetDrives().ConfigureAwait(false).GetAwaiter().GetResult();
                    viewFiles.Rows.Add("c:\\", "No Files Found", "0", "");
                    AddFilesGView(@"C:\").ConfigureAwait(false).GetAwaiter().GetResult();

                    //string[] s = DownLoadFoler.Split(Path.DirectorySeparatorChar);
                    //TreeNode nodeToSelect = compDrives.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Text == s[s.Length - 1]);
                    //TreeNodeCollection nodes = compDrives.Nodes;
                    //foreach (TreeNode node in nodes)
                    //{
                    //    foreach (TreeNodeCollection c in node.Nodes)
                    //    {

                    //        if (node.Text == "My Nodename")
                    //        {
                    //            compDrives.SelectedNode = node;
                    //            compDrives.Select();
                    //            break;
                    //        }
                    //    }
                    //}
                    //  string[] s = DownLoadFoler.Split(Path.DirectorySeparatorChar);
                    //  TreeNode[] tn = compDrives.Nodes[0].Nodes.Find(s[s.Length-1], true);
                    //  for (int i = 0; i < tn.Length; i++)
                    //  {
                    //    compDrives.SelectedNode = tn[i];
                    //     compDrives.SelectedNode.BackColor = Color.Yellow;
                    // }
                }
                // if (nodeToSelect != null)
                // {
                //    compDrives.SelectedNode = nodeToSelect;
                //}
            }
            catch (Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail($"Error getting folders {ex.Message}");
                MessageBox.Show($"Error getting folders {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            try
            {

                DownDOHLoadDocs().ConfigureAwait(false).GetAwaiter().GetResult();
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail($"Error running background worker downloading documents {ex.Message}");
                MessageBox.Show($"Error running background worker downloading documents {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            statusLabel.Text = "Done Downloading";
            proBar.Visible = false;
            cancelDownLoads.Visible = false;
        }


        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cancelDownLoads_Click(object sender, EventArgs e)
        {
            try
            {
                DOHDownLoadDocuments.DownLoadDocsInstance.CancelDL = true;
                cancelDownLoads.Visible = false;
            }
            catch (Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail($"Error Canceling downloads {ex.Message}");
                MessageBox.Show($"Error Canceling downloads {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




                

      
        private async Task ViewPDFFilesDoc(int rowIndex)
        {
            DocFolder = Path.Combine(viewFiles.Rows[rowIndex].Cells[0].Value.ToString(), viewFiles.Rows[rowIndex].Cells[1].Value.ToString());
            try
            {

                if (viewFiles.Rows[rowIndex].Cells[1].Value.ToString().ToLower().EndsWith(".pdf"))
                {
                    try
                    {
                        DOH_Utilites.DOHUtilitiesInstance.StartProcess(Edocs_Constants.Crome, $"{Edocs_Constants.Quoat}{DocFolder}{Edocs_Constants.Quoat}", false, true);
                    }
                    catch (Exception ex)
                    {
                        Send_Emails.EmailInstance.SendEmail($"Error running {Edocs_Constants.Crome} with args {DocFolder} {ex.Message}");
                        DOH_Utilites.DOHUtilitiesInstance.StartProcess(Edocs_Constants.MSEdge, $"{Edocs_Constants.Quoat}{DocFolder}{Edocs_Constants.Quoat}", false, true);
                    }

                }
                else if (viewFiles.Rows[rowIndex].Cells[1].Value.ToString().ToLower().EndsWith(".csv"))
                {

                    DOH_Utilites.DOHUtilitiesInstance.StartProcess(Edocs_Constants.NotePad, $"{Edocs_Constants.Quoat}{DocFolder}{Edocs_Constants.Quoat}", false, false);
                }
                else
                    MessageBox.Show($"Invalid DOH File {viewFiles.Rows[rowIndex].Cells[1].Value.ToString()}", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail($"Error viewing file {DocFolder} {ex.Message}");
                MessageBox.Show($"Error viewing file {DocFolder} {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGVDlFiles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {


                if (e.ColumnIndex == 10 && e.RowIndex >= 0)
                {

                    ViewPDFDoc(e.RowIndex).ConfigureAwait(false).GetAwaiter().GetResult();

                }
                if (e.ColumnIndex == 11 && e.RowIndex >= 0)
                {
                    if (!(bgWorker.IsBusy))
                        RejectDocument(e.RowIndex).ConfigureAwait(false).GetAwaiter().GetResult();

                }
            }
            catch (Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail($"Error viewing file {ex.Message}");
                MessageBox.Show($"Error viewing file {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private async Task ViewPDFDoc(int rowIndex)
        {
            DocID = dataGVDlFiles.Rows[rowIndex].Cells[0].Value.ToString();

            DocFolder = Path.Combine(dataGVDlFiles.Rows[rowIndex].Cells[7].Value.ToString(), dataGVDlFiles.Rows[rowIndex].Cells[8].Value.ToString());
           
            if (dataGVDlFiles.Rows[rowIndex].Cells[8].Value.ToString().ToLower().EndsWith(".pdf"))
                DOH_Utilites.DOHUtilitiesInstance.StartProcess(Edocs_Constants.Crome, $"{Edocs_Constants.Quoat}{DocFolder}{Edocs_Constants.Quoat}", false, true);
            else if (dataGVDlFiles.Rows[rowIndex].Cells[8].Value.ToString().ToLower().EndsWith(".csv"))
            {

                DOH_Utilites.DOHUtilitiesInstance.StartProcess(Edocs_Constants.NotePad, $"{Edocs_Constants.Quoat}{DocFolder}{Edocs_Constants.Quoat}", false, false);
            }
            else
                MessageBox.Show($"Invalid DOH File {dataGVDlFiles.Rows[rowIndex].Cells[7].Value.ToString()}", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async Task RejectDocument(int rowIndex)
        {
            try
            {
                if (dataGVDlFiles.Rows[rowIndex].Cells[9].Value.ToString().ToLower().EndsWith(".csv"))
                {
                    MessageBox.Show("Cannot Reject CSV File", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DocID = dataGVDlFiles.Rows[rowIndex].Cells[0].Value.ToString();

                RejectDOHDocument rejectDOHDocument = new RejectDOHDocument();
                DocFolder = dataGVDlFiles.Rows[rowIndex].Cells[8].Value.ToString();
                rejectDOHDocument.ID = int.Parse(DocID);
                rejectDOHDocument.DocumetName = DocFolder;
                rejectDOHDocument.ShowDialog();
            }
            catch (Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail($"Error rejecting document {ex.Message}");
                MessageBox.Show($"Error rejecting document {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DOHMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void DOHMainForm_Load(object sender, EventArgs e)
        {
            cancelDownLoads.Visible = false;
           
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(bgWorker.IsBusy))
                {
                    if (string.IsNullOrWhiteSpace(DownLoadFoler))
                        GetDownloadFolder().ConfigureAwait(false).GetAwaiter().GetResult();
                    ReDownDocument reDownDocument = new ReDownDocument();
                    if (reDownDocument.ShowDialog() == DialogResult.OK)
                    {
                        documentsNotDownloadedToolStripMenuItem.PerformClick();
                    }
                }
            }
            catch (Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail($"Error Redownloading documents {ex.Message}");
                MessageBox.Show($"Error Redownloading documents {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void viewFiles_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 3 && e.RowIndex >= 0)
                {
                    ViewPDFFilesDoc(e.RowIndex).ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail($"Error Canceling downloads {ex.Message}");
                MessageBox.Show($"Error Canceling downloads {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void supportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }
        //private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}
    }
}

