using EdocsUSA.Controls;
using Scanquire.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EdocsUSA.Utilities.Extensions;
using System.Threading.Tasks;
using EdocsUSA.Utilities;
using Scanquire.Public.ThirdParty;
using System.IO;
using System.Drawing;

namespace DemoArchivers
{
    public class MedicalRecordsSharefileArchiver : SQArchiverBase
    {
        public class MedicalrecordsSharefileArchiverInputDialog : TableLayoutPanelInputDialog
        {
            public ValidatingTextBox<string> UserId;
            public ValidatingTextBox<string> Password;
            public ValidatingTextBox<string> PatientId;
            public ComboBoxInputControl DocumentCategory;
            public DateTimePicker DocumentDate;

            private void InitializeComponent()
            {
                AddControl(new CaptionLabel("Patient ID"), 0, 0);
                PatientId = new ValidatingTextBox<string>()
                {
                    RequiresValue = true,
                    CharacterCasing = CharacterCasing.Upper
                };
                AddControl(PatientId, 1, 0);

                AddControl(new CaptionLabel("Document Category"), 0, 1);
                DocumentCategory = new ComboBoxInputControl()
                {
                    RequiresValue = true,
                    Caption = string.Empty,
                    CaptionWidth = 0
                };
                DocumentCategory.ValueComboBox.Sorted = true;
                DocumentCategory.ValueComboBox.DataSource = new List<string>()
                {
                    "",
                    "CONSENT",
                    "X-RAY",
                    "MISC"
                };
                AddControl(DocumentCategory, 1, 1);

                AddControl(new CaptionLabel("Document Date"), 0, 2);
                DocumentDate = new DateTimePicker();
                AddControl(DocumentDate, 1, 2);

                AddControl(new CaptionLabel("User ID"), 0, 3);
                UserId = new ValidatingTextBox<string>()
                {
                    RequiresValue = true,
                    Width=150
                };
                AddControl(UserId, 1, 3);

                AddControl(new CaptionLabel("Password"), 0, 4);
                Password = new ValidatingTextBox<string>()
                {
                    RequiresValue = true,
                    UseSystemPasswordChar = true,
                    Width=150
                };
                AddControl(Password, 1, 4);
            }

            public MedicalrecordsSharefileArchiverInputDialog() : base(2, 5)
            { 
                this.InitializeComponent();
                this.Size = new Size(480, 320);
            }

            public void Clear()
            {
                DocumentCategory.Clear();
                DocumentDate.Value = DateTime.Now.Date;
                Password.Clear();
            }
        }

        private string _HostName = "edocsusa.sharefile.com";
        public string HostName
        {
            get { return _HostName; }
            set { _HostName = value; }
        }

        private string _ClientId = "tP99c2xDkArdbLH1QDhYEa5hsRpXAGLd";
        public string ClientId
        {
            get { return _ClientId; }
            set { _ClientId = value; }
        }
        private string _ClientSecret = "jEFJEqK7rX9gnq3yeVSPBSAt7Q8Shi00sLM9HGvcUFvoVuG8";
        public string ClientSecret
        {
            get { return _ClientSecret; }
            set { _ClientSecret = value; }
        }

        private string _ClientFolderId = "fo7caef6-916e-4905-b8d2-a18f1293a477";
        public string ClientFolderId
        {
            get { return _ClientFolderId; }
            set { _ClientFolderId = value; }
        }

        private MedicalrecordsSharefileArchiverInputDialog InputDialog = new MedicalrecordsSharefileArchiverInputDialog();

        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            InputDialog.Clear();
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

                string userId = InputDialog.UserId.Value;
                string password = InputDialog.Password.Value;
                string patId = InputDialog.PatientId.Value;
                DateTime docDate = InputDialog.DocumentDate.Value;
                string docCat = InputDialog.DocumentCategory.ValueText;

                progress.Report(new ProgressEventArgs(0, 0, "Initializing sharefile connection"));

                string dir = patId;
                string fName = string.Join("_", patId, docDate.ToString("yyyyMMdd"), docCat);
                fName = Path.ChangeExtension(fName, file.FileExtension);

                OAuth2Token token = ShareFileV3.Authenticate(this.HostName, this.ClientId, this.ClientSecret, userId, password);
                if (token == null)
                { throw new InvalidOperationException("Unable to authenticate with ShareFile"); }

                string fId = ShareFileV3.CreateFolder(token, this.ClientFolderId, dir, "Patient Folder");
                ShareFileV3.UploadFile(token, fId, fName, file.Data);
            });
        
        }
    }
}
