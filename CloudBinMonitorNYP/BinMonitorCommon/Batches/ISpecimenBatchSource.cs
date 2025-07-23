using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinMonitor.Common
{
    public class SelectedBatchChangedEventArgs : EventArgs
    {
        public SpecimenBatch SelectedBatch { get; protected set; }

        public SelectedBatchChangedEventArgs(SpecimenBatch selectedBatch) : base()
        { this.SelectedBatch = selectedBatch; }
    }

    public interface ISpecimenBatchSource
    {
        SpecimenBatch SelectedBatch { get; set; }

        event EventHandler<SelectedBatchChangedEventArgs> SelectedBatchChanged;
    }
}
