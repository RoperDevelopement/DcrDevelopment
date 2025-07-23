using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common
{
    public partial class BatchTransferDialog : Form
    {
        public string OriginBatchId
        {
            get { return txtBatchId.Text; }
        }

        public SpecimenBatch OriginBatch
        {
            get { return SpecimenBatches.Instance[OriginBatchId]; }
        }

        public string[] SpecimensToTransfer
        {
            get
            {
                List<string> SpecimensToTransfer = new List<string>();
                foreach (object o in lbSpecimensToTransfer.Items)
                { SpecimensToTransfer.Add((string)(o)); }
                return SpecimensToTransfer.ToArray();
            }
        }

        public BatchTransferDialog()
        {
            InitializeComponent();
        }

        private void BatchTransferDialog_Load(object sender, EventArgs e)
        {

        }

        private void txtLookupBinId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            { btnLookupBinById.PerformClick(); }
        }

        private void btnLookupBinById_Click(object sender, EventArgs e)
        {
            try
            {
                lbOriginSpecimens.ClearSelected();
                lbOriginSpecimens.Items.Clear();
                lbSpecimensToTransfer.ClearSelected();
                lbSpecimensToTransfer.Items.Clear();

                string xFerBinId = txtLookupBinId.Text;
                if (string.IsNullOrWhiteSpace(xFerBinId))
                { throw new Exception("Transferred from Bin ID is required"); }
                xFerBinId = Bin.NormalizeId(xFerBinId);
                Bin bin = Bins.Instance.EnsureGetValue(xFerBinId);
                SpecimenBatch Batch = bin.EnsureGetBatch();
                txtBatchId.Text = Batch.Id;

                foreach (string Specimen in Batch.Specimens)
                {
                    lbOriginSpecimens.Items.Add(Specimen);
                }

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }

        public void Clear()
        {
            txtBatchId.Clear();
            txtLookupBinId.Clear();
            lbOriginSpecimens.ClearSelected();
            lbOriginSpecimens.Items.Clear();
            lbSpecimensToTransfer.ClearSelected();
            lbSpecimensToTransfer.Items.Clear();
        }

        private void btnAddSpecimen_Click(object sender, EventArgs e)
        {
            Array selectedItems = Array.CreateInstance(typeof(object), lbOriginSpecimens.SelectedItems.Count);
            lbOriginSpecimens.SelectedItems.CopyTo(selectedItems, 0);
            foreach (object o in selectedItems)
            { 
                lbSpecimensToTransfer.Items.Add(o);
                lbOriginSpecimens.Items.Remove(o);
            }
        }

        private void btnRemoveSpecimen_Click(object sender, EventArgs e)
        {
            Array selectedItems = Array.CreateInstance(typeof(object), lbSpecimensToTransfer.SelectedItems.Count);
            lbSpecimensToTransfer.SelectedItems.CopyTo(selectedItems, 0);
            foreach (object o in selectedItems)
            {
                lbOriginSpecimens.Items.Add(o);
                lbSpecimensToTransfer.Items.Remove(o);
            }
        }

        private void lbOriginSpecimens_DoubleClick(object sender, EventArgs e)
        {
            btnAddSpecimen.PerformClick();
        }

        private void lbSpecimensToTransfer_DoubleClick(object sender, EventArgs e)
        {
            btnRemoveSpecimen.PerformClick();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtBatchId.Text))
                { throw new Exception("Batch id is required"); }

                string BatchId = txtBatchId.Text;
                if (SpecimenBatches.Instance.ContainsKey(BatchId) == false)
                { throw new Exception("Specified Batch does not exist"); }
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message);
            }
        }
    }
}
