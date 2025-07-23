using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SP = Microsoft.SharePoint.Client;
using BinMonitor.Common;
using System.Diagnostics;
using BinMonitor.Common.Sharepoint;
using System.Net;

namespace Edocs.Clients.NYP.L8SpecimenBatchUploader
{
    public partial class frmBatchUploader : Form
    {
        Timer Timer;
        
        string Host = Settings.Sharepoint.Default.ServerAddress;
        string ListTitle = Settings.Sharepoint.Default.ListTitle;
        string UserName = Settings.Sharepoint.Default.UserName;
        string _Password = null;
        string Password
        {
            get 
            {
                if (_Password == null)
                { _Password = ProtectedDataHelper.UnProtectString(Settings.Sharepoint.Default.Password, null, System.Security.Cryptography.DataProtectionScope.CurrentUser); }
                return _Password;
            }
        }
        
        string SiteTitle = "";
        
        string InputDir = Settings.Application.Default.QueuePath;
        

        int UpdateFrequency = Settings.Application.Default.UploadFrequency;
        
        SP.ClientAuthenticationMode AuthMode = SP.ClientAuthenticationMode.FormsAuthentication;

        NetworkCredential _Credentials = null;
        NetworkCredential Credentials 
        {
            get 
            {
                if (_Credentials == null)
                {_Credentials = new NetworkCredential(UserName, Password);}
                return _Credentials;
            }
        }

        public frmBatchUploader()
        {
            InitializeComponent();

            Timer = new Timer() { Interval = UpdateFrequency };
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        string[] GetInputFiles()
        {
            return Directory.GetFiles(InputDir).OrderBy(fname => fname).ToArray();
        }

        async Task ProcessFile(SP.List spList, string path)
        {   
            byte[] fileData = File.ReadAllBytes(path);
            string inputFileName = Path.GetFileName(path);
            AddMessage(string.Format("Processing {0}...", inputFileName), true);
            SpecimenBatch Batch = Serializer.Deserialize<SpecimenBatch>(fileData);
            string outputFileName = Path.ChangeExtension(Batch.Id, "txt");
            Dictionary<string, object> fields = new Dictionary<string, object>();
            fields["Batch ID"] = Batch.Id;
            fields["Bin ID"] = Batch.BinId;
            fields["Transferred From"] = Batch.TransferredFrom;
            fields["Master Category"] = Batch.Category.MasterCategoryTitle;
            fields["Category"] = Batch.Category.Title;
            fields["Started At"] = Batch.CreatedAt.ToUniversalTime().ToString(SPHelper.DateTimeStringFormat);
            fields["Started By"] = Batch.CreatedBy;

            //Checkpoint Configuration
            fields["Checkpoint Origin"] = Batch.CheckpointOrigin.ToUniversalTime().ToString(SPHelper.DateTimeStringFormat);
            if ((Batch.CheckPoint1 != null) && (Batch.CheckPoint1.Configuration != null))
            { fields["Checkpoint 1 Duration"] = Batch.CheckPoint1.Configuration.Duration.TotalDays; }
            if ((Batch.CheckPoint2 != null) && (Batch.CheckPoint2.Configuration != null))
            { fields["Checkpoint 2 Duration"] = Batch.CheckPoint2.Configuration.Duration.TotalDays; }
            if ((Batch.CheckPoint3 != null) && (Batch.CheckPoint3.Configuration != null))
            { fields["Checkpoint 3 Duration"] = Batch.CheckPoint3.Configuration.Duration.TotalDays; }
            if ((Batch.CheckPoint4 != null) && (Batch.CheckPoint4.Configuration != null))
            { fields["Checkpoint 4 Duration"] = Batch.CheckPoint4.Configuration.Duration.TotalDays; }

            //Workflow progress
            if ((Batch.Registration != null) && (Batch.Registration.HasStarted))
            {
                fields["Registration Started At"] = Batch.Registration.StartedAt.ToUniversalTime().ToString(SPHelper.DateTimeStringFormat);
                fields["Registration Assigned By"] = Batch.Registration.AssignedBy;
                fields["Registration Assigned To"] = Batch.Registration.AssignedTo;
            }
            if ((Batch.Registration != null) && (Batch.Registration.HasCompleted))
            {
                fields["Registration Completed At"] = Batch.Registration.CompletedAt.ToUniversalTime().ToString(SPHelper.DateTimeStringFormat);
                fields["Registration Completed By"] = Batch.Registration.CompletedBy;
            }
            if ((Batch.Processing != null) && (Batch.Processing.HasStarted))
            {
                fields["Processing Started At"] = Batch.Processing.StartedAt.ToUniversalTime().ToString(SPHelper.DateTimeStringFormat);
                fields["Processing Assigned By"] = Batch.Processing.AssignedBy;
                fields["Processing Assigned To"] = Batch.Processing.AssignedTo;
            }
            if ((Batch.Processing != null) && (Batch.Processing.HasCompleted))
            {
                fields["Processing Completed At"] = Batch.Processing.CompletedAt.ToUniversalTime().ToString(SPHelper.DateTimeStringFormat);
                fields["Processing Completed By"] = Batch.Processing.CompletedBy;
            }

            //Closed status
            if (Batch.IsClosed)
            {
                fields["Closed"] = true;
                fields["Closed At"] = Batch.ClosedAt.ToUniversalTime().ToString(SPHelper.DateTimeStringFormat);
                fields["Closed By"] = Batch.ClosedBy;
            }

            if (string.IsNullOrWhiteSpace(Batch.Comments) == false)
            { fields["Comments"] = Batch.Comments; }
            if (Batch.Specimens.Count > 0)
            {
                fields["Specimen Count"] = Batch.Specimens.Count;
                fields["Specimens"] = string.Join(Environment.NewLine, Batch.Specimens); 
            }
            /*
            int ellapsedCheckpoints = 0;
            if (Batch.CheckPoint1 != null && Batch.CheckPoint1.Elapsed())
            { ellapsedCheckpoints++; }
            if (Batch.CheckPoint2 != null && Batch.CheckPoint2.Elapsed())
            { ellapsedCheckpoints++; }
            if (Batch.CheckPoint3 != null && Batch.CheckPoint3.Elapsed())
            { ellapsedCheckpoints++; }
            if (Batch.CheckPoint4 != null & Batch.CheckPoint4.Elapsed())
            { ellapsedCheckpoints++; }
            fields["Ellapsed Checkpoints"] = ellapsedCheckpoints;
            */
            AddMessage("Sending...", false);
            await SharepointConnector.SendFile(spList, fileData, outputFileName, fields);
            AddMessage("Sent", false);
        }

        async Task ProcessDirectory(SP.List spList, string path)
        {
            string[] inputFiles = GetInputFiles();
            foreach (string inputFile in inputFiles)
            {
                string fileName = Path.GetFileName(inputFile);
                try
                {
                    await ProcessFile(spList, inputFile);
                    File.Delete(inputFile);
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error processing file ({0}) - ({1})", fileName, ex.Message);
                    Trace.TraceError(message);
                    Trace.TraceError(ex.StackTrace);
                    AddMessage(message, true);
                }                
            }
            AddMessage("Done", true);
        }

        async Task Step()
        {
            Timer.Stop();

            pnlControls.Enabled = false;
            ClearMessage();
            AddMessage(DateTime.Now.ToString(), true);
            AddMessage("Attempting to synchronize, please wait...", true);
            try
            {
                AddMessage("Initializing Sharepoint Connection", true);
                SP.ClientContext spContext;
                SP.Web spSite;
                SP.List spList;
                try
                {
                    AddMessage("Authenticating...", true);
                    
                    spContext = await SharepointConnector.Authenticate(Host, AuthMode, Credentials);
                    AddMessage("Getting Site...", false);
                    spSite = await SharepointConnector.GetSite(spContext, SiteTitle);
                    AddMessage("Getting List...", false);
                    spList = await SharepointConnector.GetList(spSite, ListTitle);
                    AddMessage("Done", false);
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error connecting to host ({0}) - ({1})", Host, ex.Message);
                    Trace.TraceError(message);
                    AddMessage(message, true);
                    return;
                }


                AddMessage("Searching for files", true);
                if (Directory.Exists(InputDir) == false)
                {
                    string message = string.Format("Unable to find InputDir ({0})", InputDir);
                    Trace.TraceError(message);
                    AddMessage(message, true);
                    return;
                }

                await ProcessDirectory(spList, InputDir);
                File.WriteAllBytes(@"Data\LastUpdated.txt", new byte[0]);
            }
            finally
            {
                Timer.Start();
                pnlControls.Enabled = true;
                txtLastUpdated.Text = DateTime.Now.ToString();
                txtNextUpdate.Text = (DateTime.Now + TimeSpan.FromMilliseconds(Timer.Interval)).ToString();
            }
        }

        async void Timer_Tick(object sender, EventArgs e)
        {
            await Step();
        }
        void ClearMessage()
        { txtMessage.Invoke((MethodInvoker)(delegate { txtMessage.Clear(); })); }

        void AddMessage(string message, bool newLine)
        {
            txtMessage.Invoke((MethodInvoker)(delegate
            {
                if ((newLine) && (string.IsNullOrWhiteSpace(txtMessage.Text) == false))
                { txtMessage.AppendText(Environment.NewLine); }
                txtMessage.AppendText(message); 
            }));
            
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            await Step();
        }
    }
}
