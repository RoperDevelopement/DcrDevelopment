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
    public partial class ActiveBinsViewer : UserControl
    {
        protected class BinStatusQuickView
        {
            public string BinId { get; set; }
            public string Category { get; set; }
            public Color CategoryColor { get; set; }
            public DateTime DStarted { get; set; }
            public string Started { get { return DStarted.ToString("MM/dd HH:mm"); } }

            public BinStatusQuickView(string binId, string category, Color categoryColor, DateTime dStarted)
            {
                this.BinId = binId;
                this.Category = category;
                this.CategoryColor = categoryColor;
                this.DStarted = dStarted;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.DesignMode == false)
            { 
                Bins.Instance.CollectionChanged += OnBins_CollectionChanged;
                Bins.Instance.ObjectChanged += OnBins_CollectionChanged;
                BinStatusViewer.RefreshTimer.Tick += (sender, t) => { OnBins_CollectionChanged(); };
                OnBins_CollectionChanged();
            }
        }

        public ActiveBinsViewer()
        {
            InitializeComponent();            
        }

        protected void OnBins_CollectionChanged()
        {
            BinStatusQuickView[] activeBatches =
                (
                    from Bin bin
                        in (Bins.Instance.ActiveBins)
                        .OrderBy(bin => bin.Batch.Category.MasterCategoryTitle)
                        .ThenBy(bin => bin.Batch.CheckpointOrigin)
                    select new BinStatusQuickView(bin.Id, bin.Batch.Category.MasterCategoryTitle, bin.Batch.Category.Color.Value, bin.Batch.CreatedAt)
                ).ToArray();
            dgvQuickView.DataSource = activeBatches;
            dgvQuickView.ClearSelection();
        }
        
        protected void OnBins_CollectionChanged(object sender, EventArgs e)
        { OnBins_CollectionChanged(); }

        private void dgvQuickView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            BinStatusQuickView bin = (BinStatusQuickView)(dgv.Rows[e.RowIndex].DataBoundItem);

            e.CellStyle.BackColor = bin.CategoryColor;
            if (bin.CategoryColor.GetBrightness() > 0.5)
            { e.CellStyle.ForeColor = Color.Black; }
            else
            { e.CellStyle.ForeColor = Color.White; }
        }


    }
}
