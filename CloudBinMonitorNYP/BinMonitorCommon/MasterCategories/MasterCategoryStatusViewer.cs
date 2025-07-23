using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace BinMonitor.Common
{
    public partial class MasterCategoryStatusViewer : UserControl
    {
        public MasterCategoryStatusViewer()
        {
            InitializeComponent();
            Bins.Instance.CollectionChanged += OnBins_CollectionChanged;
            Bins.Instance.ObjectChanged += OnBins_CollectionChanged;
            OnBins_CollectionChanged();
        }

        private string _MasterCategoryTitle = null;
        public string MasterCategoryTitle
        {
            get { return _MasterCategoryTitle; }
            set
            {
                string oldTitle = _MasterCategoryTitle;
                _MasterCategoryTitle = value;
                OnMasterCategoryTitleChanged(oldTitle, value);
            }
        }

        public string Caption
        {
            get
            { return lblCaption.Text; }
            set
            { lblCaption.Text = value; }
        }

        public MasterCategory MasterCategory
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.MasterCategoryTitle))
                { return null; }
                if (MasterCategories.Instance.ContainsKey(this.MasterCategoryTitle) == false)
                { return null; }
                return MasterCategories.Instance[this.MasterCategoryTitle];
            }
        }

        public MasterCategory EnsureGetMasterCategory()
        {
            MasterCategory mc = this.MasterCategory;
            if (mc == null)
            { throw new InvalidOperationException("The requested operation requires a valid MasterCategory"); }
            return mc;
        }

        public void OnMasterCategoryTitleChanged(string oldTitle, string newTitle)
        {
            OnBins_CollectionChanged();
        }

        public void OnBins_CollectionChanged()
        {
            MasterCategory mc = this.MasterCategory;
          //  return;
            if (DesignMode)
            { return; }
            else if (mc == null)
            {
                binStatusViewer1.Bin = null;
                binStatusViewer2.Bin = null;
                binStatusViewer3.Bin = null;
            }
            else
            {
                Bin[] binsInCategory = Bins.Instance.GetBinsByMasterCategory(this.MasterCategoryTitle).ToArray();
                Bin[] unProcessedBins = binsInCategory.Where(bin => bin.Batch.Processing.HasStarted == false).ToArray();
                Bin[] sortedUnProcessedBins = unProcessedBins.OrderBy(bin => bin.Batch.CheckpointOrigin).ToArray();

                binStatusViewer1.Bin = sortedUnProcessedBins.Length >= 1 ? sortedUnProcessedBins[0] : null;
                binStatusViewer2.Bin = sortedUnProcessedBins.Length >= 2 ? sortedUnProcessedBins[1] : null;
                binStatusViewer3.Bin = sortedUnProcessedBins.Length >= 3 ? sortedUnProcessedBins[2] : null;
            }
        }

        public void OnBins_CollectionChanged(object sender, EventArgs e)
        { OnBins_CollectionChanged(); }
    }
}
