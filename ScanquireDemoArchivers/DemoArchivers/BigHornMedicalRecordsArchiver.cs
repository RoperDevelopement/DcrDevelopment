using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using EdocsUSA.Controls;
using Scanquire.Public;

namespace DemoArchivers
{
    public class BigHornMedicalRecordsArchiver : SQFilesystemArchiver
    {
        private string _RootPath = Path.Combine(SQFilesystemArchiver.DefaultBasePath, "Medical Records");
        public override string RootPath
        {
            get { return _RootPath; }
            set { _RootPath = value; }
        }


        protected class MedicalRecordsInputDialog : TableLayoutPanelInputDialog
        {
            public ValidatingTextBox<string> MRN;
            public ValidatingTextBox<string> FirstName;
            public ValidatingTextBox<string> LastName;
            public ValidatingTextBox<int> VisitID;
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
                AddControl(MRN, 1, 0);
                CaptionLabel FirstNameCaptionLabel = new CaptionLabel("First Name");
                AddControl(FirstNameCaptionLabel, 2, 0);
                FirstName = new ValidatingTextBox<string>()
                {
                    RequiresValue = true,
                    CharacterCasing = CharacterCasing.Upper
                };
                AddControl(FirstName, 3, 0);
                CaptionLabel LastNameCaption = new CaptionLabel("Last Name");
                AddControl(LastNameCaption, 4, 0);
                LastName = new ValidatingTextBox<string>()
                {
                    RequiresValue = true,
                    CharacterCasing = CharacterCasing.Upper
                };
                AddControl(LastName, 5, 0);
                CaptionLabel VisitIdCaptionLabel = new CaptionLabel("Visit ID");
                AddControl(VisitIdCaptionLabel, 0, 1);
                VisitID = new ValidatingTextBox<int>()
                {
                    RequiresValue = true,
                    CharacterCasing = CharacterCasing.Upper
                };
                AddControl(VisitID, 1, 1);
                CaptionLabel DateOfServiceCaptionLabel = new CaptionLabel("DOS");
                AddControl(DateOfServiceCaptionLabel, 2, 1);
                DateOfService = new ValidatingTextBox<DateTime>() { RequiresValue = true };
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

        private string _LogHeader = "MRN,Last Name,First Name,Visit ID,Date Of Service,Page Count,Checksum,User Name, Timestamp";
        public override string LogHeader
        {
            get { return _LogHeader; }
            set { _LogHeader = value; }
        }

        public override DoWorkEventHandler NewFromScanner()
        {
            if (InputDialog.ShowDialog(Global.HostWindow) == DialogResult.OK)
            { return base.NewFromScanner(); }
            else throw new OperationCanceledException();
        }

        public override DoWorkEventHandler NewFromBookmark()
        {
            Trace.TraceInformation(BookmarkImageBuilderName);
            return base.NewFromBookmark();
        }

        public MedicalRecordsArchiver()
        {
        }

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
    }
}
