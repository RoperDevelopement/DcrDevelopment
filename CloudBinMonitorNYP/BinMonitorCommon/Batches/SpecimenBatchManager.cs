using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common
{    
    public static class SpecimenBatchManager
    {
        private const string INVALID_PERMISSIONS_MESSAGE = "You do not have the required permissions to complete the specified operation";

        //   public static string _ArchivePath = @"Data\Batch Archive";
        //  public static string _ArchivePath = string.Format("{0}\\Local\\EdocsUsaBmC\\Data\\Batch Archive", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        public static string _ArchivePath = BinUtilities.BinMonBatchArchiveFolder;
        public static string ArchivePath
        {
            get { return _ArchivePath; }
            set { _ArchivePath = value; }
        }

        //private static string _TransactionQueuePath = @"Data\Transaction Queue";
        //private static string _TransactionQueuePath = string.Format("{0}\\Local\\EdocsUsaBmC\\Data\\Transaction Queue", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        private static string _TransactionQueuePath = BinUtilities.BinMonTransactionQueueFolder;
        public static string TransactionQueuePath
        {
            get { return _TransactionQueuePath; }
            set { _TransactionQueuePath = value; }
        }

        public static MasterCategoryPermissions GetCategoryPermissions(SpecimenBatch batch, User user)
        {
            if (user.UserProfile.CategoryPermissions.ContainsKey(batch.Category.MasterCategoryTitle) == false)
            {
                string message = string.Format("Unable to find permissions for profile ({0}) on category ({1})"
                    , user.UserProfile.Id, batch.Category.MasterCategoryTitle);
                throw new KeyNotFoundException(message);
            }
            return user.UserProfile.CategoryPermissions[batch.Category.MasterCategoryTitle]; 
            
        }

        public static void ArchiveTransaction(SpecimenBatch batch)
        {
            DateTime timeStamp = DateTime.Now;
            string fileName = string.Format("{0}_{1}"
                , timeStamp.ToString("yyyyMMddhhmmssfff")
                , batch.Id);
            string fileNameWithExt = Path.ChangeExtension(fileName, "xml");
            string filePath = Path.Combine(TransactionQueuePath, fileNameWithExt);
            Directory.CreateDirectory(TransactionQueuePath);
            Serializer.Serialize(batch, filePath);
            BinMonitorSqlServer.SqlServerInstance.RegisterBinSqlServer(batch);
            BmSqlServerXmlFiles.AddBatchesCloud.UploadSpecimenBatchesXmlFile(batch.Id,batch.BinId,SqlCommands.SqlCmd.ProcessingBatchTrue);
           
           
        }

        public static void ArchiveBatch(SpecimenBatch batch)
        {
            string fileName = batch.Id;
            string fileNameWithExt = Path.ChangeExtension(fileName, "xml");
            string filePath = Path.Combine(ArchivePath, fileNameWithExt);
            Directory.CreateDirectory(ArchivePath);
            Serializer.Serialize(batch, filePath);
        }

        public static void AssignNewBatch(SpecimenBatch batch, string binId, User user)
        {
            MasterCategoryPermissions p = GetCategoryPermissions(batch, user);
            if (p.CanCreate == false)
            { throw new SecurityException(INVALID_PERMISSIONS_MESSAGE); }

            Bin bin = Bins.Instance.EnsureGetValue(binId);
            bin.EnsureSupportsCategory(batch.Category.Title);
            bin.AddBatch(batch.Id);

            ArchiveTransaction(batch);
          //  AddUpDateBmXmlFiles.AddBatchesCloud.UploadSpecimenBatchesXmlFile(batch.Id);
            BmSqlServerXmlFiles.AddBatchesCloud.UpDateBinXmlFile(batch.BinId);
        }

        public static void AddComment(SpecimenBatch batch, string comment, User user)
        {
            if (GetCategoryPermissions(batch, user).CanAddComment == false)
            { throw new SecurityException(INVALID_PERMISSIONS_MESSAGE); }

            batch.AddComment(comment, user.Id, DateTime.Now);
            ArchiveTransaction(batch);
           // AddUpDateBmXmlFiles.AddBatchesCloud.UploadSpecimenBatchesXmlFile(batch.Id);
        }

        public static void AssignRegistration(SpecimenBatch batch, User assignedBy, User assignedTo)
        {
            if (GetCategoryPermissions(batch, assignedBy).CanAssign == false)
            { throw new SecurityException(INVALID_PERMISSIONS_MESSAGE); }

            if (batch.Registration.HasStarted)
            { throw new InvalidOperationException("Registration has already been started."); }

            batch.Registration.Start(assignedBy.Id, assignedTo.Id);
            ArchiveTransaction(batch);
            //AddUpDateBmXmlFiles.AddBatchesCloud.UploadSpecimenBatchesXmlFile(batch.Id);
        }

        public static void CheckOutForRegistration(SpecimenBatch batch, User assignedTo)
        {
            if (GetCategoryPermissions(batch, assignedTo).CanCheckOut == false)
            { throw new SecurityException(INVALID_PERMISSIONS_MESSAGE); }

            if (batch.Registration.HasStarted)
            { throw new InvalidOperationException("Registration has already been started on the specified batch."); }

            batch.Registration.Start(assignedTo.Id, assignedTo.Id);
            ArchiveTransaction(batch);
        }

        public static void CompleteRegistration(SpecimenBatch batch, User completedBy)
        {
            MasterCategoryPermissions perm = GetCategoryPermissions(batch,completedBy);
            if (perm.CanCheckIn == false)
            { throw new SecurityException(INVALID_PERMISSIONS_MESSAGE); }

            if (batch.Registration.HasStarted == false)
            { throw new InvalidOperationException("Registration has not been started on the specified batch."); }
            
            if (batch.Registration.HasCompleted)
            { throw new InvalidOperationException("Registration has already been completed on the specified batch"); }

            batch.Registration.Complete(completedBy.Id);
            BinMonitorSqlServer.SqlServerInstance.RegisterBinSqlServer(batch);
            BmSqlServerXmlFiles.AddBatchesCloud.UploadSpecimenBatchesXmlFile(batch.Id,batch.BinId,SqlCommands.SqlCmd.ProcessingBatchTrue);
            

            //ArchiveTransaction(batch);
        }

        public static void AssignProcessing(SpecimenBatch batch, User assignedBy, User assignedTo)
        {
            if (GetCategoryPermissions(batch, assignedBy).CanAssign == false)
            { throw new SecurityException(INVALID_PERMISSIONS_MESSAGE); }

            if (batch.Processing.HasStarted)
            { throw new InvalidOperationException("Processing has already been started on the specified batch."); }

            if (batch.Registration.HasStarted 
                && (batch.Registration.HasCompleted == false)
                && (batch.Registration.AssignedTo.Equals(assignedTo.Id, StringComparison.OrdinalIgnoreCase) == false))
            { throw new InvalidOperationException("Registration has been assigned to a different user and has not been completed."); }

            batch.Processing.Start(assignedBy.Id, assignedTo.Id);
            ArchiveTransaction(batch);
          //  AddUpDateBmXmlFiles.AddBatchesCloud.UploadSpecimenBatchesXmlFile(batch.Id);
        }

        public static void CheckOutForProcessing(SpecimenBatch batch, User checkedOutBy)
        {
            if (GetCategoryPermissions(batch, checkedOutBy).CanCheckOut == false)
            { throw new SecurityException(INVALID_PERMISSIONS_MESSAGE); }

            if (batch.Processing.HasStarted)
            { throw new InvalidOperationException("Processing has already been started on the specified batch."); }

            batch.Processing.Start(checkedOutBy.Id, checkedOutBy.Id);
            ArchiveTransaction(batch);
          //  AddUpDateBmXmlFiles.AddBatchesCloud.UploadSpecimenBatchesXmlFile(batch.Id);
        }

        public static void CompleteProcessing(SpecimenBatch batch, User completedBy)
        {
            MasterCategoryPermissions perm = GetCategoryPermissions(batch, completedBy);
            if (perm.CanCheckIn == false)
            { throw new SecurityException(INVALID_PERMISSIONS_MESSAGE); }

            if (batch.Processing.HasStarted == false)
            { throw new InvalidOperationException("Processing has not been started on the specified batch."); }

            //if (batch.Processing.HasCompleted)
            //{ throw new InvalidOperationException("Processing has already been compelted on the specified batch."); }

            batch.Processing.Complete(completedBy.Id);
            ArchiveTransaction(batch);
          //  AddUpDateBmXmlFiles.AddBatchesCloud.UploadSpecimenBatchesXmlFile(batch.Id);

        }

        public static void CheckOutForRegistrationAndProcessing(SpecimenBatch batch, User checkedOutBy)
        {
            MasterCategoryPermissions perm = GetCategoryPermissions(batch, checkedOutBy);
            if (perm.CanCheckOut == false)
            { throw new SecurityException("Specified user does not have permission to check out bins of the specified category."); }

            if (batch.Registration.HasStarted)
            { throw new InvalidOperationException("Registration has already been started on the specified batch."); }

            if (batch.Processing.HasStarted)
            { throw new InvalidOperationException("Processing has already been started on the specified batch."); }

            batch.Registration.Start(checkedOutBy.Id, checkedOutBy.Id);
            batch.Processing.Start(checkedOutBy.Id, checkedOutBy.Id);
            BinMonitorSqlServer.SqlServerInstance.RegisterBinSqlServer(batch);
            BmSqlServerXmlFiles.AddBatchesCloud.UploadSpecimenBatchesXmlFile(batch.Id,batch.BinId,SqlCommands.SqlCmd.ProcessingBatchTrue);
            
        }

        public static void FinalizeBatch(SpecimenBatch batch, User finalizedBy, bool autoCompleteWorkflowSteps)
        {
            MasterCategoryPermissions perm = GetCategoryPermissions(batch, finalizedBy);
            
            if (perm.CanClose == false)
            { throw new SecurityException(INVALID_PERMISSIONS_MESSAGE); }

            //if (batch.IsClosed)
            //{ throw new InvalidOperationException("The specified batch has already been closed"); }

            if (batch.Registration.HasStarted == false)
            { throw new InvalidOperationException("Registration has not been started on the specified batch."); }

            if (batch.Processing.HasStarted == false)
            { throw new InvalidOperationException("Processing has not been started on the specified batch."); }

            if (batch.Registration.HasCompleted == false)
            {
                if (autoCompleteWorkflowSteps == false)
                { throw new InvalidOperationException("Registration has not been completed on the specified batch."); }
                if (perm.CanCheckIn == false)
                { throw new InvalidOperationException("User does not have permission to complete registration on the specified batch."); }
            }

            if (batch.Processing.HasCompleted == false)
            {
                if (autoCompleteWorkflowSteps == false)
                { throw new InvalidOperationException("Processing has not been completed on the specified batch"); }
                if (perm.CanCheckIn == false)
                { throw new InvalidOperationException("User does not have permission to complete processing on the specified batch."); }
            }

            if (batch.Registration.HasCompleted == false)
            { batch.Registration.Complete(finalizedBy.Id); }

            if (batch.Processing.HasCompleted == false)
            { batch.Processing.Complete(finalizedBy.Id); }
            
            
            Bin bin = Bins.Instance.EnsureGetValue(batch.BinId);
            SpecimenBatches.Instance.EnsureMoveToArchive(batch.Id);
            
            batch.IsClosed = true;
            batch.ClosedAt = DateTime.Now;
            batch.ClosedBy = finalizedBy.Id;
            ArchiveTransaction(batch);
            ArchiveBatch(batch);
            batch.NotifyClosed();
            bin.Clear();
            //   AddUpDateBmXmlFiles.AddBatchesCloud.UploadSpecimenBatchesXmlFile(batch.Id);
            BinMonitorSqlServer.SqlServerInstance.RegisterBinSqlServer(batch);
            BmSqlServerXmlFiles.AddBatchesCloud.UpDateBinXmlFile(batch.BinId);
            
         //   BatchLookupControl BatchLookup = new BatchLookupControl();
         //   BatchLookup.Refresh();
            //   BatchLookupControl d = new BatchLookupControl();
            /// d.ReloadComBox();


        }

        public static SpecimenBatch GetNextPriorityBatch(params string[] masterCategoryTitles)
        {
            return SpecimenBatches.Instance.Values
                .Where(b =>
                    (b.IsCheckedOut == false)
                    && (b.Processing.HasCompleted == false)
                    && (masterCategoryTitles.Contains(b.Category.MasterCategoryTitle)))
                .OrderBy(b => b.CheckpointOrigin)
                .FirstOrDefault();
            /*
            SpecimenBatch[] batches = SpecimenBatches.Instance.Values
                .Where(b => 
                    (b.Processing.HasStarted == false) &&
                    (masterCategoryTitles.Contains(b.Category.MasterCategoryTitle)))
                .OrderBy(b => b.CheckpointOrigin)
                .ToArray();
            if (batches.Length == 0)
            { return null; }
            else
            { return batches[0]; }
            */
        }

        public static string[] PriorityBatches = new string[] { MasterCategories.STAT_TITLE, MasterCategories.ROUTINE_TITLE };

        public static void ConfirmNextPriorityBatch(IWin32Window owner, SpecimenBatch batch)
        {
            SpecimenBatch nextPriorityBatch = SpecimenBatchManager.GetNextPriorityBatch(PriorityBatches);
            if (nextPriorityBatch == null)
            { return; }
            if (object.ReferenceEquals(nextPriorityBatch, batch) == false)
            {
                string message = string.Format("A higher priority batch was found in bin {0}\nTo continue with bin {1} click Ok, otherwise click Cancel.", nextPriorityBatch.BinId, batch.BinId);
                if (MessageBox.Show(owner, message, "Confirm", MessageBoxButtons.OKCancel) != DialogResult.OK)
                { throw new OperationCanceledException(); }
                
            }
        }
    }
}
