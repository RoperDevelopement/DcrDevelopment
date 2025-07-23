using EdocsUSA.Controls;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using Scanquire.Public;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoArchivers
{
    public class MedicalRecords2 : SQFilesystemArchiver
    {
        protected class MedicalRecordsInputDialog : TableLayoutPanelInputDialog
        {
            public ValidatingTextBox<string> MRN;
            public ValidatingTextBox<string> FirstName;
            public ValidatingTextBox<string> LastName;
            public ValidatingTextBox<string> VisitID;
            public ValidatingTextBox<DateTime> DateOfService;

            private void InitializeComponent()
            {
                this.Size = new System.Drawing.Size(550, 250);

                CaptionLabel MrnCaptionLabel = new CaptionLabel("MRN");
                AddControl(MrnCaptionLabel, 0, 0);
                MRN = new ValidatingTextBox<string>()
                {
                    RequiresValue = true,
                    CharacterCasing = CharacterCasing.Upper
                };
                MRN.Value = "MR12309";
                AddControl(MRN, 1, 0);
                CaptionLabel FirstNameCaptionLabel = new CaptionLabel("First Name");
                AddControl(FirstNameCaptionLabel, 2, 0);
                FirstName = new ValidatingTextBox<string>()
                {
                    RequiresValue = true,
                    CharacterCasing = CharacterCasing.Upper
                };
                FirstName.Value = "JOHN";
                AddControl(FirstName, 3, 0);
                CaptionLabel LastNameCaption = new CaptionLabel("Last Name");
                AddControl(LastNameCaption, 4, 0);
                LastName = new ValidatingTextBox<string>()
                {
                    RequiresValue = true,
                    CharacterCasing = CharacterCasing.Upper
                };
                LastName.Value = "DOE";
                AddControl(LastName, 5, 0);
                CaptionLabel VisitIdCaptionLabel = new CaptionLabel("Visit ID");
                AddControl(VisitIdCaptionLabel, 0, 1);
                VisitID = new ValidatingTextBox<string>()
                {
                    RequiresValue = true,
                    CharacterCasing = CharacterCasing.Upper
                };
                VisitID.Value = "15002938";
                AddControl(VisitID, 1, 1);
                CaptionLabel DateOfServiceCaptionLabel = new CaptionLabel("DOS");
                AddControl(DateOfServiceCaptionLabel, 2, 1);
                
                DateOfService = new ValidatingTextBox<DateTime>() 
                { RequiresValue = true };
                DateOfService.Value = new DateTime(2013, 1, 1);
                AddControl(DateOfService, 3, 1);
            }

            public MedicalRecordsInputDialog()
                : base(6, 2)
            { InitializeComponent(); }

            protected override void OnShown(EventArgs e)
            {
                if (MRN.HasValue)
                {
                    VisitID.SelectAll();
                    VisitID.Focus();
                }
                else
                {
                    MRN.SelectAll();
                    MRN.Focus();
                }
            }

            protected void Clear()
            {
                VisitID.Clear();
                DateOfService.Clear();
            }
        }

        protected MedicalRecordsInputDialog InputDialog = new MedicalRecordsInputDialog();

        public override async Task Send(SQFile file, int fileNumber, IProgress<ProgressEventArgs> progress, CancellationToken cancelToken)
        {
            Debug.WriteLine("Sending file");

            InputDialog.TryShowDialog(DialogResult.OK);

            
            string relativeDirectory = InputDialog.MRN.Value
                + "_" + InputDialog.LastName.Value
                + "_" + InputDialog.FirstName.Value;
            string fileName = InputDialog.MRN.Value
                + "_" + InputDialog.LastName.Value
                + "_" + InputDialog.FirstName.Value
                + "_" + InputDialog.VisitID.Value
                + "_" + InputDialog.DateOfService.Value.ToString("yyyyMMdd");
            fileName = Path.ChangeExtension(fileName, file.FileExtension);
            string filePath = Path.Combine(relativeDirectory, fileName);

            SQFilesystemConnector.SaveFileResult saveResult;
            saveResult = await FilesystemConnector.SaveFile(filePath, file.Data);
            if (saveResult.Success)
            {
                Dictionary<string, string> logEntry = new Dictionary<string, string>();
                logEntry["MRN"] = InputDialog.MRN.Value;
                logEntry["Last Name"] = InputDialog.LastName.Value;
                logEntry["First Name"] = InputDialog.FirstName.Value;
                logEntry["Visit"] = InputDialog.VisitID.Value.ToString();
                logEntry["DOS"] = InputDialog.DateOfService.Value.ToShortDateString();
                logEntry["Ver"] = saveResult.VersionNumber.ToString();
                logEntry["Pages"] = file.PageCount.ToString();
                logEntry["Checksum"] = file.Checksum;
                logEntry["User"] = Environment.UserName;
                logEntry["Time"] = DateTime.Now.ToUniversalTime().ToShortDateString();
                logEntry["RelPath"] = saveResult.RelativeUriPath;

                Log.Append(logEntry);
            }
            //TODO: handle retry
            else throw new OperationCanceledException();
        }
        /*
        public override void Send(SQFile file)
        {
            if (InputDialog.ShowDialog(Global.HostWindow) == DialogResult.OK)
            {
                string relativeDirectory = InputDialog.MRN.Value
                    + "_" + InputDialog.LastName.Value
                    + "_" + InputDialog.FirstName.Value;
                string fileName = InputDialog.MRN.Value
                    + "_" + InputDialog.LastName.Value
                    + "_" + InputDialog.FirstName.Value
                    + "_" + InputDialog.VisitID.Value
                    + "_" + InputDialog.DateOfService.Value.ToString("yyyyMMdd");
                fileName = Path.ChangeExtension(fileName, file.FileExtension);
                string filePath = Path.Combine(RootPath, relativeDirectory, fileName);

                string logEntry = InputDialog.MRN.Value
                    + "," + InputDialog.LastName.Value
                    + "," + InputDialog.FirstName.Value
                    + "," + InputDialog.VisitID.Value
                    + "," + InputDialog.DateOfService.Value.ToShortDateString()
                    + "," + file.PageCount
                    + "," + file.Checksum
                    + "," + Environment.UserName
                    + "," + DateTime.Now.ToString();

                Send(filePath, file.Data);
                WriteLogEntry(logEntry);
            }
            else throw new OperationCanceledException();
        }
         */
    }
}
