using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using EdocsUSA.Controls;
using Scanquire.Public;
using Scanquire.Public.Extensions;
using Scanquire.Public.UserControls;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using EdocsUSA.Utilities.Logging;


namespace Edocs.Potomac.Elementary.School.Archiver
{
    public class PESArchiver : SQFilesystemArchiver
    {
        PESArchiverDialog InputDialog = new PESArchiverDialog();//PotomacElementarySchoolArchiverDialog();
        private string _RootPath = @"C:\Archives\AltaCare\Medical Records\";
        private string _DropDownItems = @"C:\Archives\AltaCare\Medical Records\";
        public string RootPath
        {
            get { return _RootPath; }
            set { _RootPath = value; }
        }
        //public string DropDownItems
        //{
        //    get { return _DropDownItems; }
        //    set { _DropDownItems = value; }
        //}


        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromFile(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            throw new NotImplementedException();
        }
        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
             

            InputDialog.TryShowDialog(DialogResult.OK);
            return base.AcquireFromScannerForNew(progress, cToken);
        }
        public async override Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cancelToken)
        {
            InputDialog.TryShowDialog(DialogResult.OK);
            // string dirPath = string.Format("{0}\\{1}", RootPath, InputDialog.MedicaRecordNumber);
            //Directory.CreateDirectory(RootPath);
            ////   string fileName = string.Format("{0}_{1}_{2}_{3}_{4}", InputDialog.MedicaRecordNumber, InputDialog.LastName, InputDialog.FirstName, InputDialog.StSchoolYear, InputDialog.ESchoolYear);
            ////    string fileNameWithExt = Path.ChangeExtension(fileName, file.FileExtension);
            ////  string filePath = Path.Combine(dirPath, fileNameWithExt);

            ////   SQFilesystemConnector.SaveFileResult saveFileResult = await FilesystemConnector.SaveFile(filePath, file.Data);
            //string relPat = RootPath + "\\log.csv";
            //Dictionary<string, string> logEntry = new Dictionary<string, string>();
            ////   logEntry["Medical Record #"] = InputDialog.MedicaRecordNumber;
            ////   logEntry["Last Name"] = InputDialog.LastName;
            //////   logEntry["First Name"] = InputDialog.FirstName;
            ////  logEntry["School Year"] = string.Format("{0} {1}", InputDialog.StSchoolYear, InputDialog.ESchoolYear);
            //logEntry["User Name"] = Environment.UserName;
            //logEntry["Image Count"] = file.PageCount.ToString();
            //logEntry["TimeStamp"] = DateTime.Now.ToString();


            ////   logEntry["File Path"] = relPat;
            //Log.Append(logEntry);
            //   InputDialog.CLear();
            //CsvFileDictionaryLogger c = new CsvFileDictionaryLogger();

        }
    }
}
