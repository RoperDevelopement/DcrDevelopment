using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace BinMonitor.Common
{
    [Serializable]
    public class Bin : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            { this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public event EventHandler BatchChanged;
        protected void NotifyBatchChanged()
        {
            if (this.BatchChanged != null)
            { this.BatchChanged(this, null); }
        }

        #endregion Events

        #region Event Handlers

        protected void OnPropertyChanged(string propertyName)
        { NotifyPropertyChanged(propertyName); }

        protected void OnBatchChanged()
        { NotifyBatchChanged(); }

        protected void OnBatch_WorkflowChanged(object sender, EventArgs e)
        { OnBatchChanged(); }

        protected void OnBatch_CheckpointChanged(object sender, EventArgs e)
        { OnBatchChanged(); }

        protected void OnBatch_PropertyChanged(object sender, PropertyChangedEventArgs e)
        { OnBatchChanged(); }

        #endregion Event Handlers

        #region Properties

        private string _Id = null;
        public string Id 
        {
            get { return _Id; }
            set 
            { 
                _Id = value;
                OnPropertyChanged("Id");
            }
        }

        private string[] _SupportedCategories = new string[0];
        public string[] SupportedCategories    
        {
            get { return _SupportedCategories; }
            set 
            { 
                _SupportedCategories = value;
                OnPropertyChanged("SupportedCategories");
            }
        }

        public bool SupportsCategory(string categoryTitle)
        { return (SupportedCategories.Contains(categoryTitle, StringComparer.OrdinalIgnoreCase)); }

        public void EnsureSupportsCategory(string categoryTitle)
        {
            if (SupportsCategory(categoryTitle) == false)
            { throw new InvalidOperationException("The specified bin does not support batches of category " + categoryTitle); }
        }


        private string _BatchId;
        public string BatchId
        {
            get { return _BatchId; }
            set
            {
                SpecimenBatch previousValue = this.Batch;
                if (previousValue != null)
                {
                    previousValue.CheckpointChanged -= OnBatch_CheckpointChanged;
                    previousValue.WorkflowStepChanged -= OnBatch_WorkflowChanged;
                    previousValue.PropertyChanged -= OnBatch_PropertyChanged;
                }
                if (value != null)
                {
                    SpecimenBatch newValue = SpecimenBatches.Instance.EnsureGetValue(value);
                    newValue.CheckpointChanged += OnBatch_CheckpointChanged;
                    newValue.WorkflowStepChanged += OnBatch_WorkflowChanged;
                    newValue.PropertyChanged += OnBatch_PropertyChanged;
                }

                _BatchId = value;
                OnPropertyChanged("BatchId");
                OnPropertyChanged("Batch");
                OnBatchChanged();
            }
        }

        public SpecimenBatch Batch
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.BatchId))
                { return null; }
                SpecimenBatch Batch;
                if (SpecimenBatches.Instance.TryGetValue(this.BatchId, out Batch) == false)
                { return null; }
                return Batch;
            }
        }

        public bool TryGetBatch(out SpecimenBatch Batch)
        {
            Batch = this.Batch;
            return (Batch != null);
        }

        public SpecimenBatch EnsureGetBatch()
        {
            SpecimenBatch Batch;
            if (this.TryGetBatch(out Batch) == false)
            { throw new InvalidOperationException("The requested operation requires a valid Batch to be assigned to the bin"); }
            return Batch;
        }

        /*
        private SpecimenBatch _Contents = null;
        /// <summary>The SpecimenBatch currently occupying this bin.</summary>
        /// <remarks>Set allowed only for serialization, use AddBin and RemoveBin.</remarks>
        public SpecimenBatch Contents 
        {
            get { return _Contents; }
            set 
            {
                //if there was a previous value, unregister the events
                if (this._Contents != null)
                {
                    this._Contents.CheckpointChanged -= OnBatch_CheckpointChanged;
                    this._Contents.PropertyChanged -= OnBatch_PropertyChanged;
                    this._Contents.WorkflowStepChanged -= OnBatch_WorkflowChanged;
                }
                //if a new value is provided, register the events
                if (value != null)
                {
                    value.CheckpointChanged += OnBatch_CheckpointChanged;
                    value.PropertyChanged += OnBatch_PropertyChanged;
                    value.WorkflowStepChanged += OnBatch_WorkflowChanged;
                }

                _Contents = value;
                OnBatchChanged();
            }
        }
        */
        public bool HasBatch { get { return this.Batch != null; } }

        #endregion Properties

        #region Static Methods

        public static string NormalizeId(string id)
        { return id.PadLeft(3, '0'); }

        #endregion Static Methods


        public void AddBatch(string BatchId)
        {
            EnsureEmpty();
            this.BatchId = BatchId;
        }

        public void Clear()
        {
            this.BatchId = null;
            //Bins.Instance.Reload();
        }        

        public void EnsureEmpty()
        { 
            if (this.HasBatch) 
            {throw new InvalidOperationException("The requested operation requires an empty bin");}
        }

        public void EnsureNotEmpty()
        {
            if (this.HasBatch == false)
            { throw new InvalidOperationException("The requested operation cannot be performed on an empty bin"); }
        }
    }

    
    /*
    public static class BatchArchiver
    {
        public static string DirectoryPath
        { get { return @"CompeltedBatches"; } }

        public static void ArchvieBatch(SpecimenBatch Batch)
        {
            Directory.CreateDirectory(DirectoryPath);
            string BatchPath = Path.Combine(DirectoryPath, Batch.Id);
            BatchPath = Path.ChangeExtension(BatchPath, "xml");
            Serializer.Serialize(Batch, BatchPath);
        }
    }
    */

    /*
    public class BinManager
    {
        public event EventHandler Updated;

        protected void NotifyUpdated()
        { 
            if (this.Updated != null)
            {
                Debug.WriteLine("Bin manager notifying update");
                this.Updated(this, null); 
            }
        }

        private static BinManager _Instance = new BinManager();
        public static BinManager Instance
        { get { return _Instance; } }

        public BinManager()
        {

        }

        protected void OnBinContentsWorkflowUpdated(object sender, EventArgs e)
        {
            if (sender is Bin)
            {
                Bin bin = (Bin)sender;
                Bins.Instance[bin.Id] = bin;
            }
            else { Debug.WriteLine("OnBinContentsWorkflowUpdated from " + sender.GetType()); }
            NotifyUpdated(); 
        }

        protected void OnBinContentsChanged(object sender, EventArgs e)
        { 
            if (sender is Bin)
            { 
                Bin bin = (Bin)sender;
                Bins.Instance[bin.Id] = bin;
            }
            else { Debug.WriteLine("OnBinContentsWorkflowUpdated from " + sender.GetType()); }
            NotifyUpdated(); 
        }
        /*
        public Bin[] GetBinsByCategory(string category)
        {
            return (from Bin bin in Bins.Instance.Values
                   where bin.HasContents && bin.Contents.Category.Title.Equals(category, StringComparison.OrdinalIgnoreCase)
                   select bin).ToArray();
        }

        public Bin[] GetBinsByCategory_SortedByTimeRemaining(string category)
        {
            return (GetBinsByCategory(category).OrderBy(bin => bin.Contents.CheckPoint4.RemainingTime())).ToArray();
        }

        public Bin[] GetRoutineBins()
        { return GetBinsByCategory_SortedByTimeRemaining(SpecimenCategories.ROUTINE_TITLE); }

        public Bin[] GetStatBins()
        { return GetBinsByCategory_SortedByTimeRemaining(SpecimenCategories.STAT_TITLE); }

        public Bin[] GetTimedBins()
        { return GetBinsByCategory_SortedByTimeRemaining(SpecimenCategories.TIMED_TITLE); }

        public Bin[] GetProblemBins()
        { return GetBinsByCategory_SortedByTimeRemaining(SpecimenCategories.PROBLEM_TITLE); }
        */

        /*
        public SpecimenBatch EnsureGetSpecimenBatch(string binId)
        {
            Bin bin = Bins.Instance.EnsureGetValue(binId);
            bin.EnsureNotEmpty();
            return bin.Contents;
        }
         

        public void AddSpecimenBatch(string binId, SpecimenBatch Batch)
        {
            Bin bin = Bins.Instance.EnsureGetValue(binId);
            bin.EnsureEmpty();
            bin.Contents = Batch;
        }        

        public void UpdateComments(string binId, string newComments)
        {
            SpecimenBatch Batch = EnsureGetSpecimenBatch(binId);
            Batch.Comments = newComments;
        }

        public void StartCreation(string binId, string assignedBy, string assignedTo)
        {
            SpecimenBatch Batch = EnsureGetSpecimenBatch(binId);
            Batch.Creation.Start(assignedBy, assignedTo);
        }

        public void CompleteCreation(string binId, string completedBy)
        {
            SpecimenBatch Batch = EnsureGetSpecimenBatch(binId);
            Batch.Creation.Complete(completedBy);
        }

        public void StartRegistration(string binId, string assignedBy, string assignedTo)
        {
            SpecimenBatch Batch = EnsureGetSpecimenBatch(binId);
            Batch.Registration.Start(assignedBy, assignedTo);
        }

        public void CompleteRegistration(string binId, string completedBy)
        {
            SpecimenBatch Batch = EnsureGetSpecimenBatch(binId);
            Batch.Registration.Complete(completedBy);
        }

        public void StartProcessing(string binId, string assignedBy, string assignedTo)
        {
            SpecimenBatch Batch = EnsureGetSpecimenBatch(binId);
            Batch.Processing.Start(assignedBy, assignedTo);
        }

        public void CompleteProcessing(string binId, string completedBy)
        {
            SpecimenBatch Batch = EnsureGetSpecimenBatch(binId);
            Batch.Processing.Complete(completedBy);
            FinalizeBatch(binId); 
        }
        
        public void FinalizeBatch(string binId)
        {
            SpecimenBatch Batch = EnsureGetSpecimenBatch(binId);
            //TODO: Archive

            Bin bin = Bins.Instance.EnsureGetValue(binId);
            bin.Clear();
        }
         
    }
*/
    
}
