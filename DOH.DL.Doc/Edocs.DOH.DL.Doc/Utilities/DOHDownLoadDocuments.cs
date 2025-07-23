using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using BS = Edocs.Azure.Blob.Storage.AzureBlobStorage;
namespace Edocs.DOH.DL.Doc.Utilities
{
    public class DOHDownLoadDocuments
    {
        private static DOHDownLoadDocuments instance = null;

        private DOHDownLoadDocuments() { }
        public bool CancelDL
        { get; set; }
        public bool AppendDataGuid
        { get; set; }
        public static DOHDownLoadDocuments DownLoadDocsInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DOHDownLoadDocuments();
                }
                return instance;
            }
        }

        public async Task<StringBuilder> DownLoadDOHDOc(string spName)
        {
            StringBuilder jsonResult = new StringBuilder();
            try
            {
                using (SqlConnection connection = new SqlConnection(Edocs_Constants.AzureLocalSqlConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        connection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();


                        using (SqlDataReader reader = command.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                        {
                            if (!reader.HasRows)
                            {
                                jsonResult.Append("[]");
                            }
                            else
                            {
                                while (reader.ReadAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                                {
                                    jsonResult.Append(reader.GetString(0));
                                }
                            }
                        }

                        // Console.WriteLine(jsonResult.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail($"Getting documents that need downloaded {ex.Message}");
                throw new Exception($"Getting documents that need downloaded {ex.Message}");
            }
            return jsonResult;

        }

        public async Task UpDateDownLoadDOHDOc(int id, string spName)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(Edocs_Constants.AzureLocalSqlConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        connection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                        command.Parameters.Add(new SqlParameter(Edocs_Constants.ParmID, id));

                        command.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    }

                    // Console.WriteLine(jsonResult.ToString());
                }

            }
            catch (Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail($"Updating document id {id} {ex.Message}");
                throw new Exception($"Updating document id {id} {ex.Message}");
            }

        }
        public async Task RejectDOHDOc(int id, string userName, string rejectReason)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(Edocs_Constants.AzureLocalSqlConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(Edocs_Constants.Spsp_RejectDOHDocument, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        connection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                        command.Parameters.Add(new SqlParameter(Edocs_Constants.ParmID, id));
                        command.Parameters.Add(new SqlParameter(Edocs_Constants.ParmRejectReason, rejectReason));
                        command.Parameters.Add(new SqlParameter(Edocs_Constants.ParmUserName, userName));



                        command.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    }

                    // Console.WriteLine(jsonResult.ToString());
                }

            }
            catch (Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail($"Updating Reject reason for document id {id} {ex.Message}");
                throw new Exception($"Updating Reject reason for document id {id} {ex.Message}");
            }

        }

        public async Task<SqlDataReader> DownLoadDOHDOcCmd()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Edocs_Constants.AzureLocalSqlConnectionString))
                {
                    string sqlCmd = "SELECT  doh.[ID]  as ID,[FileName] as FName,[ICROCRFileName] as OCRFName,[DownLoadSubFolder] as DownLoadSubFolder,[Url] as Uri" +
                        ",csv.CSVFileName as CSVFName FROM[dbo].[DOHReords] doh join[dbo].[DOHCSVFiles] csv on doh.id = csv.ID where[Downloaded] = 0 FOR JSON PATH, WITHOUT_ARRAY_WRAPPER";
                    using (SqlCommand command = new SqlCommand(sqlCmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        connection.Open();

                        StringBuilder jsonResult = new StringBuilder();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows)
                            {
                                jsonResult.Append("[]");
                            }
                            else
                            {
                                while (await reader.ReadAsync())
                                {
                                    jsonResult.Append(reader.GetString(0));
                                }
                            }
                        }

                        //  Console.WriteLine(jsonResult.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail($"Download documents {ex.Message}");
                throw new Exception($"Download documents {ex.Message}");
            }
            return null;

        }
        public async Task DownLoadDocs(IList<DOHDownLoadModel> dOHDownLoadDocuments, System.Windows.Forms.DataGridView dataGridView, string downLoadFolder, System.Windows.Forms.ToolStripLabel toolStripLabel, System.Windows.Forms.ProgressBar progressBar, string csvFileName, string csvDownloadFolder)
        {
            try
            {

                StringBuilder sb = new StringBuilder();
                if (!(Directory.Exists(downLoadFolder)))
                    Directory.CreateDirectory(downLoadFolder);
                if (!(DOH_Utilites.DOHUtilitiesInstance.HasAccess(downLoadFolder)))
                    return;
                if (!(DOH_Utilites.DOHUtilitiesInstance.HasAccess(csvDownloadFolder)))
                    return;
                
                if ((dOHDownLoadDocuments == null) || (dOHDownLoadDocuments.Count() == 0))
                {
                    System.Windows.Forms.MessageBox.Show("No Documents found to download", "Download", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    toolStripLabel.Text = "No Documents found to Download";
                    toolStripLabel.BackColor = System.Drawing.Color.Red;
                    return;
                }

                toolStripLabel.BackColor = System.Drawing.Color.LightGreen;
                CancelDL = false;

                //}
                //if (dataGridView.RowCount > 0)
                //  {
                //   if(System.Windows.Forms.MessageBox.Show("Append Files","Append",System.Windows.Forms.MessageBoxButtons.YesNo,System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)

                //  else
                //  {
                //    if(File.Exists(Path.Combine(downLoadFolder, "DOHDownload.csv")))
                //    {
                //       string csv = File.ReadAllText(Path.Combine(downLoadFolder, "DOHDownload.csv"));
                //     sb.AppendLine(csv);
                //  }
                //}
                // }

                int totalDocsDL = dOHDownLoadDocuments.Count();
                int numDocs = (int)100 / totalDocsDL;
                int pCount = 0;
                double mib = 0.0;
                progressBar.Value = 5;

                foreach (DOHDownLoadModel documents in dOHDownLoadDocuments)
                {
                    pCount = pCount + numDocs;


                    System.Windows.Forms.Application.DoEvents();
                    toolStripLabel.Text = $"Downloading Document {Path.Combine(downLoadFolder, documents.FName)} Total Left to Download {--totalDocsDL}";
                    GetDOHDOcument(System.IO.Path.Combine(documents.Uri, documents.FName), downLoadFolder).ConfigureAwait(false).GetAwaiter().GetResult();
                    FileInfo fileInfo = new FileInfo(Path.Combine(downLoadFolder, documents.FName));
                    mib = DOH_Utilites.DOHUtilitiesInstance.GetFileSizeKB(fileInfo.Length);

                    sb.AppendLine($"{documents.City},{documents.Church},{documents.BookType},{documents.SDate} thru {documents.EDate},{documents.FName}");
                    dataGridView.Rows.Add(documents.ID, documents.City, documents.Church, documents.BookType, documents.SDate, documents.EDate, documents.ImagesScanned.ToString(), downLoadFolder, documents.FName, $"{mib.ToString("#.##")} kb", "View", "Reject");
                    if (CancelDL)
                    {
                        progressBar.Value = 100;
                        toolStripLabel.Text = "DownLoads Cancled";
                        toolStripLabel.BackColor = System.Drawing.Color.Red;
                        break;
                    }
                    UpDateDownLoadDOHDOc(documents.ID, Edocs_Constants.SpUpDateDownLoadDocument).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (progressBar.Value > progressBar.Maximum)
                        progressBar.Value = 100;
                    else
                        progressBar.Value = pCount;
                }

                toolStripLabel.Text = $"Total Documents Downloaded {dOHDownLoadDocuments.Count()} ";
                toolStripLabel.BackColor = System.Drawing.Color.Green;
                string csvFolder = SaveCsvFile(csvDownloadFolder, csvFileName, sb).ConfigureAwait(false).GetAwaiter().GetResult();
               
                FileInfo fileInfoCsv = new FileInfo(Path.Combine(csvFolder, csvFileName));
                mib = DOH_Utilites.DOHUtilitiesInstance.GetFileSizeKB(fileInfoCsv.Length);
                dataGridView.Rows.Add("", "N/A", "N/A", "CSV File", "N/A", "N/A", "N/A", csvFolder, csvFileName, $"{mib.ToString("#.##")} kb", $"0", "View", "");
                dataGridView.EndEdit();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Errors Found {ex.Message}", "Error Downloading", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Send_Emails.EmailInstance.SendEmail($"Downloading Documents from the Azure Cloud {ex.Message}");
                throw new Exception($"Downloading Documents from the Azure Cloud {ex.Message}");
            }
        }
        private async Task<string> SaveCsvFile(string csvDownloadFolder, string csvFileName, StringBuilder sb)
        {
            try
            {
              
            File.WriteAllText(Path.Combine(csvDownloadFolder, csvFileName), sb.ToString());
            }
            catch(Exception ex)
            {
               
                Send_Emails.EmailInstance.SendEmail($"Error Saving Csv file {Path.Combine(csvDownloadFolder, csvFileName)} {ex.Message}");
                csvDownloadFolder = SaveCsvFileTempFolder(csvFileName,sb).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            return csvDownloadFolder;

        }
        private async Task<string> SaveCsvFileTempFolder(string csvFileName, StringBuilder sb)
        {
            string csvDownloadFolder = Path.Combine(Path.GetTempPath(), "e-DocsUsa\\Doh");
            try
            {
              
              
           
                if (!(Directory.Exists(csvDownloadFolder)))
                    Directory.CreateDirectory(csvDownloadFolder);
            File.WriteAllText(Path.Combine(csvDownloadFolder, csvFileName), sb.ToString());
                System.Windows.Forms.MessageBox.Show($"Saved CSV File to different folder {Path.Combine(csvDownloadFolder, csvFileName)}", "CSV File", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail($"Error Saving csv file to {Path.Combine(csvDownloadFolder, csvFileName)} {ex.Message}");
                throw new Exception(ex.Message);
            }
            return csvDownloadFolder;
        }
        public double ConvertKBToMiB(double kilobytes)
        {
            return kilobytes * 0.00095367431640625;
        }
        public async Task GetDOHDOcument(string uri, string dlFolder)
        {
            try
            {

            

            BS.BlobStorageInstance.AzureBlobAccountKey = Edocs_Constants.AzureBlobAccountKey;
            BS.BlobStorageInstance.AzureBlobStorageConnectionString = Edocs_Constants.AzureBlobStorageConnectionString;
            BS.BlobStorageInstance.AzureBlobAccountName = Edocs_Constants.AzureBlobAccountName;
            // byte[] dohFname = BS.BlobStorageInstance.DownloadFileBytesAzureBlob(System.IO.Path.GetFileName(uri), Edocs_Constants.AzureContainer).ConfigureAwait(false).GetAwaiter().GetResult();

            byte[] dohFname = BS.BlobStorageInstance.DownloadFileAzureBlob(System.IO.Path.GetFileName(uri), Edocs_Constants.AzureContainer,string.Empty).ConfigureAwait(false).GetAwaiter().GetResult();
                if ((dohFname == null) || (dohFname.Length == 0))
                    throw new Exception($"Error downloading file from azure cloud file zero bytes {uri} saving to folder {dlFolder}");
            System.IO.File.WriteAllBytes(System.IO.Path.Combine(dlFolder, System.IO.Path.GetFileName(uri)), dohFname);
            }
            catch(Exception ex)
            {
                throw new Exception($"Error downloading file {uri} saving to folder {dlFolder} {ex.Message}");
            }
        }
    }
}
