using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BinMonitor.Common
{
    public class SpecimenBatches : SerializedObjectDictionary<SpecimenBatch>
    {

        //    private string _DirectoryPath = @"C:\Archives\Data\Specimen Batches";

        // private string _DirectoryPath = string.Format(@"{0}\Local\EdocsUsaBmC\Data\Specimen Batches", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        private string _DirectoryPath = BinUtilities.BinMonSpecimenBatchesFolder;
        public override string DirectoryPath
        {
            get { return _DirectoryPath; }
            set { _DirectoryPath = value; }
        }

        static readonly SpecimenBatches _Instance = new SpecimenBatches();
        public static SpecimenBatches Instance
        { get { return _Instance; } }



        protected override void RegisterObjectChangedEvents(string key, SpecimenBatch value)
        {
            PropertyChangedEventHandler onValuePropertyChanged = (sender, e) =>
            { OnObjectChanged(key, value); };
            value.PropertyChanged += onValuePropertyChanged;
            
            EventHandler OnValue_WorkflowStepChanged = (sender, e) =>
            { OnObjectChanged(key, value); };
            value.WorkflowStepChanged += OnValue_WorkflowStepChanged;

            EventHandler OnValue_CheckpointChanged = (sender, e) =>
            { OnObjectChanged(key, value); };
            value.CheckpointChanged += OnValue_CheckpointChanged;
        }
    }
}
