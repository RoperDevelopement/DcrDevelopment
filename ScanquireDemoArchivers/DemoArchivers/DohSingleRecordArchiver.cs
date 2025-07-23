using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EdocsUSA.Utilities.Extensions;
using Scanquire.Public;
using System.Windows.Forms;
using System.Threading.Tasks;
using EdocsUSA.Utilities;
using System.IO;


namespace DemoArchivers
{
    public class DohSingleRecordArchiver : SQArchiverBase
    {
        private string _SiteUrl = "http://edocsbackup/sites/doh/";
        public string SiteUrl
        {
            get { return _SiteUrl; }
            set { _SiteUrl = value; }
        }

        private string _LibraryName = "Sendouts";
        public string LibraryName
        {
            get { return _LibraryName; }
            set { _LibraryName = value; }
        }


        private string[] _PerformingLabs = new string[]
        {
            "",
            "Ambry Genetics",
            "ARUP",
            "Athena Diagnostics",
            "Blood Center of Wisconsin",
            "Esoterix",
            "Genoptix",
            "John Hopkins",
            "LabCorp",
            "Mayo Medical Laboratories",
            "Medical Neurogenetics",
            "Palo Alto",
            "Prevention Genetics",
            "Prometheus Laboratory",
            "Quest Diagnostics",
            "Rogosin Institute",
            "University of Rochester",
            "University of Washington"
        };
        public string[] PerformingLabs
        {
            get { return _PerformingLabs; }
            set { _PerformingLabs = value; }
        }


        protected DohSingleRecordArchiverInputDialog InputDialog = new DohSingleRecordArchiverInputDialog();

        public DohSingleRecordArchiver()
        {
            InputDialog = new DohSingleRecordArchiverInputDialog();
            foreach (string performingLab in this.PerformingLabs)
            { InputDialog.PerformingLabs.Add(performingLab); }
        }

        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            InputDialog.Clear();
            InputDialog.Credentials.ServerAddress = string.Concat(SiteUrl);
            InputDialog.Credentials.UserName = "administrator";
            InputDialog.Credentials.Domain = "edocsbackup";
            InputDialog.Credentials.Password = "e-Docs";

            InputDialog.TryShowDialog(DialogResult.OK);
            return base.AcquireFromScannerForNew(progress, cToken);
        }

        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromFile(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            throw new NotImplementedException();
        }

        public override async Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            await Task.Factory.StartNew(() =>
            {
                InputDialog.TryShowDialog(DialogResult.OK);
                
                string authorizationHeader = InputDialog.Credentials.AuthorizationHeader;

                progress.Report(new ProgressEventArgs(0, 0, "Initializing sharepoint connections"));

                SharepointRestConnector sharepoint = new SharepointRestConnector(SiteUrl, authorizationHeader, LibraryName);
                Dictionary<string, object> metadata = new Dictionary<string, object>();

                metadata["Scan_x0020_Date"] = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                metadata["Date_x0020_Of_x0020_Service"] = InputDialog.DateOfService.ToString("yyyy-MM-dd");
                metadata["Accession_x0020_Number"] = InputDialog.AccessionNumber;
                metadata["Financial_x0020_Number"] = InputDialog.FinancialNumber;
                metadata["Performing_x0020_Lab"] = InputDialog.PerformingLab;

                DateTime dateOfService = InputDialog.DateOfService;
                string Year = dateOfService.Year.ToString();
                string Month = dateOfService.Month.ToString().PadLeft(2, '0');
                string Day = dateOfService.Day.ToString().PadLeft(2, '0');
                
                progress.Report(new ProgressEventArgs(0, 0, "Uploading file"));
                sharepoint.CreateDirectory(Year);
                sharepoint.CreateDirectory(Year + "/" + Month);
                sharepoint.CreateDirectory(Year + "/" + Month + "/" + Day);
                
                string relativeDir = Year +"/"+ Month +"/" + Day + "/";
                string fileName = Guid.NewGuid().ToString();
                string fileNameWithEx = Path.ChangeExtension(fileName, file.FileExtension);
                sharepoint.UploadFile(file.Data, relativeDir, fileNameWithEx, metadata);

            });
        }


    }
}
