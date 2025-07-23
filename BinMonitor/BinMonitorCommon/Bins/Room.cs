using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace BinMonitor.Common
{
    public sealed class Room : SerializedObjectDictionary<Bin>
    {
        public override string DirectoryPath
        { get { return @"Data\Room"; } }

        private string _ServerQueuePath = @"Data\Server Queue";
        public string ServerQueuePath
        {
            get { return _ServerQueuePath; }
            set { _ServerQueuePath = value; }
        }

        static readonly Room _Instance = new Room();
        public static Room Instance
        { get { return _Instance; } }

        public IEnumerable<Bin> ActiveBins
        {
            get
            {
                return from Bin bin in this.Values
                       where bin.HasBatch
                       select bin;
            }
        }

        public IEnumerable<Bin> EmpbyBins
        {
            get
            {
                return from Bin bin in this.Values
                       where bin.HasBatch == false
                       select bin;
            }
        }

        public IEnumerable<Bin> GetBinsByCategory(string categoryTitle)
        {
            return from Bin bin in ActiveBins
                   where bin.Batch.Category.Title.Equals(categoryTitle, StringComparison.OrdinalIgnoreCase)
                   select bin;
        }

        public IEnumerable<Bin> GetBinsByMasterCategory(string title)
        {
            return from Bin bin in ActiveBins
                   where bin.Batch.Category.MasterCategoryTitle.Equals(title, StringComparison.OrdinalIgnoreCase)
                   select bin;
        }

        public IEnumerable<Bin> GetBinsContainingSpecimen(string SpecimenId)
        {
            return from Bin bin in this.Values
                   where bin.HasBatch
                   && bin.Batch.Specimens.Contains(SpecimenId, StringComparer.OrdinalIgnoreCase)
                   select bin;
        }

        public IEnumerable<Bin> ProblemBins
        { get { return GetBinsByMasterCategory(MasterCategories.PROBLEM_TITLE); } }

        public IEnumerable<Bin> RoutineBins
        { get { return GetBinsByMasterCategory(MasterCategories.ROUTINE_TITLE); } }

        public IEnumerable<Bin> StatBins
        { get { return GetBinsByMasterCategory(MasterCategories.STAT_TITLE); } }

        public IEnumerable<Bin> ReadyBins
        { get { return GetBinsByMasterCategory(MasterCategories.READY_TITLE); } }

        protected override void RegisterObjectChangedEvents(string key, Bin value)
        {
            PropertyChangedEventHandler onValuePropertyChanged = (sender, e) =>
            {
                OnObjectChanged(key, value);
            };
            value.PropertyChanged += onValuePropertyChanged;
            EventHandler onValueBatchChanged = (sender, e) =>
            {
                OnObjectChanged(key, value);
                /*
                if (value.Batch != null)
                {
                    string queueFileName = DateTime.Now.ToString("yyyyMMddhhmmssfff_" + value.BatchId);
                    queueFileName = Path.ChangeExtension(queueFileName, "xml");
                    string queueFilePath = Path.Combine(ServerQueuePath, queueFileName);
                    Serializer.Serialize(value.Batch, queueFilePath);
                }*/
            };
            value.BatchChanged += onValueBatchChanged;
        }
    }
}
