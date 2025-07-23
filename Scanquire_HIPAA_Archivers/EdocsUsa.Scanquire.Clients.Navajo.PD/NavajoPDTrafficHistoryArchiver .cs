using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using EdocsUSA.Controls;
using Scanquire.Public;
using Scanquire.Public.Extensions;
using Scanquire.Public.UserControls;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using EdocsUSA.Utilities.Logging;
namespace EdocsUsa.Scanquire.Clients.Navajo.PD
{
    public class NavajoPDTrafficHistoryArchiver : SQFilesystemArchiver
    {
        NavajoPDTrafficHistoryArchiverDialog InputDialog = new NavajoPDTrafficHistoryArchiverDialog();
        public string RootPath
        {
            get;
            set;
        }
        public string SaveRootPath
        { get; set; }
        public string DropDownItems
        { get; set; }
        public string SpStudentRecords
        { get; set; }
        public string SpStudentRecordsFinRecords
        { get; set; }
        public string WebApi
        { get; set; }
        public string ShowTotalRecordsDialog
        { get; set; }
        public string ShowBarCode
        { get; set; }
        public string AzureShareName
        { get; set; }
        public string UploadExe
        { get; set; }
        public string AzureWebApiController
        { get; set; }

        public string AzureDataBaseName
        { get; set; }

        public string DisPlayArchiverName
        { get; set; }
        public bool ShowCmdWindow
        { get; set; }
        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {


            //  InputDialog.TryShowDialog(DialogResult.OK);
            return base.AcquireFromScannerForNew(progress, cToken);
        }
        public async override Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cancelToken)
        {
            InputDialog.Text = DisPlayArchiverName;
            if (bool.TryParse(ShowBarCode, out bool result))
                InputDialog.ShowBarCode = result;
            InputDialog.TryShowDialog(DialogResult.OK);
        }
    }
}
