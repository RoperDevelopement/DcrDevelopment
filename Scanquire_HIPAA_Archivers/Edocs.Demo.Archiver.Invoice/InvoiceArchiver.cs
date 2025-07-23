using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdocsUSA.Controls;
using Scanquire.Public;
using Scanquire.Public.Extensions;
using Scanquire.Public.UserControls;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using EdocsUSA.Utilities.Logging;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;
using static System.Net.WebRequestMethods;

namespace Edocs.Demo.Archiver.Invoice
{
    public class InvoiceArchiver : SQFilesystemArchiver
    {
        InvoiceArchiverDialog InputDialog = new InvoiceArchiverDialog();
        private readonly string RecordsJson = "_records.json";
        private readonly string SettingsJson = "_settings.json";
        public string DisPlayArchiverName
        { get; set; }
        public string SaveRootPath
        {
            get;
            set;
        }
        private string GetNewGuid()
        {
            return Guid.NewGuid().ToString();
        }
        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {


            //  InputDialog.TryShowDialog(DialogResult.OK);
            return base.AcquireFromScannerForNew(progress, cToken);
        }
        //  public override async Task Send(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        public async override Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cancelToken)
        {
             string filePath ="N/A";
            try
            {


                InputDialog.Text = DisPlayArchiverName;
                InputDialog.InvoiceDateDate = DateTime.Now;
                InputDialog.InvoiceDueDate = DateTime.Now.AddDays(30);
                InputDialog.InvoiceTotal = "0.00";

                InputDialog.TryShowDialog(DialogResult.OK);
                //  await base.Send(images, progress, cToken);
                string dirGuidName = GetNewGuid();
                string saveFolder = $"{Environment.UserDomainName}\\{DisPlayArchiverName}\\{dirGuidName}";
                string dirPath = Path.Combine(SaveRootPath, saveFolder);
                Dictionary<string, string> recordsJs = new Dictionary<string, string>();
                Dictionary<string, string> settingsJs = new Dictionary<string, string>();
                settingsJs["BatchID"] = dirGuidName;
                settingsJs["UploadFoler"] = "Upload folder to Store PDF Files";
                settingsJs["TotalPages"] = file.PageCount.ToString();
                settingsJs["TotalPDFFiles"] = "1";
                settingsJs["StoreProcedureName"] = "Name of stored procedure to upload Index Information";
                settingsJs["DataBaseName"] = "Name of DataBase";
                settingsJs["WebApi"] = "Name of Restful API used to upload informaiton";
                Log.FilePath = Path.Combine(dirPath, $"{dirGuidName}{SettingsJson}");
                Log.Append(settingsJs);
                  filePath = Path.Combine(dirPath, $"{GetNewGuid()}.pdf");
                recordsJs["FileName"] = Edocs_Utilities.EdocsUtilitiesInstance.GetFileName(filePath);
                recordsJs["DateInvoice"] = InputDialog.InvoiceDateDate.ToString();
                recordsJs["InvoiceDueDate"] = InputDialog.InvoiceDueDate.ToString();
                recordsJs["InvoiceNumber"] = InputDialog.InvoiceNumber;
                recordsJs["InvoicePONumber"] = InputDialog.InvoicePONumber;
                recordsJs["InvoiceCustomerNumber"] = InputDialog.InvoiceCustomerNumber;
                Log.FilePath = Path.Combine(dirPath, $"{dirGuidName}{RecordsJson}");
                Log.Append(recordsJs);
                SQFilesystemConnector.SaveFileResult saveFileResult = await FilesystemConnector.SaveFile(filePath, file.Data);
                
                MessageBox.Show($"PDF File Saved under {filePath}", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Creading PDF File Saved {filePath} {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                throw new OperationCanceledException(ex.Message);
            }
        }
        
            //public override async Task Send(SQFile sharepointFile, SQFile optixFile, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
            //{
            //}
        }
}
