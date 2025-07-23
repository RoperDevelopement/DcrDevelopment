using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common
{
    public class BatchLookupControl : FlowLayoutPanel, ISpecimenBatchSource
    {
        ErrorProvider ErrorProvider;

        FlowLayoutPanel pnlLookupByBin;
        ComboBox cmbBinId;
        Label lblBinId;
        TextBox txtBinId;
        Button btnLookupById;

        FlowLayoutPanel pnlLookupBySpecimen;
        Label lblSpecimenId;
        TextBox txtSpecimenId;
        Button btnLookupBySpecimen;

        FlowLayoutPanel pnlSpecimenIdResults;
        ComboBox cmbSpecimenIdResults;

        FlowLayoutPanel pnlBatchId;
        Label lblBatchId;
        TextBox txtBatchId;
        Button btnClear;

        public event EventHandler<SelectedBatchChangedEventArgs> SelectedBatchChanged;

        protected void NotifySelectedBatchChanged(SpecimenBatch selectedBatch)
        {
            EventHandler<SelectedBatchChangedEventArgs> handler = this.SelectedBatchChanged;
            if (handler != null)
            { handler(this, new SelectedBatchChangedEventArgs(selectedBatch)); }
        }

        private SpecimenBatch _SelectedBatch = null;
        public SpecimenBatch SelectedBatch
        {
            get { return _SelectedBatch; }
            set
            {
                _SelectedBatch = value;
                OnSelectedBatchChanged(value);
            }
        }

        protected void OnSelectedBatchChanged(SpecimenBatch currentBatch)
        {
            txtBatchId.Clear();
            if (SelectedBatch != null)
            {
                txtBatchId.Text = SelectedBatch.Id;
            }

            NotifySelectedBatchChanged(currentBatch);
        }


        private void InitializeComponent()
        {
            ErrorProvider = new ErrorProvider()
            { BlinkStyle = ErrorBlinkStyle.NeverBlink };

            pnlLookupByBin = new FlowLayoutPanel()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };

            Controls.Add(pnlLookupByBin);

            lblBinId = new Label()
            {
                MinimumSize = new Size(0, 20),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "Bin Id",
            };
            pnlLookupByBin.Controls.Add(lblBinId);

            cmbBinId = new ComboBox();
            cmbBinId.SelectedValueChanged += cmbBinId_SelectedValueChanged;
            //cmbBinId.MouseDoubleClick += cmbBinId_MouseDoubleClick;
            cmbBinId.SelectedIndexChanged += cmbBinId_SelectedIndexChanged;
            cmbBinId.GotFocus += cmbBinId_GotFocus;
            //  cmbBinId.GotFocus += cmbBinId_GotFocus;
            //  cmbBinId.MouseHover += cmbBinId_MouseHover;
            pnlLookupByBin.Controls.Add(cmbBinId);
            //  cmbBinId.DataSource = null;
            //   cmbBinId.BeginUpdate();

            Bins.Instance.Reload();
            List<Bin> activeBins = Bins.Instance.ActiveBins.ToList();
            List<String> activeBinId = new List<string>();
            for (int i = 0; i < activeBins.Count; i++)
            {
                activeBinId.Add(activeBins[i].Id);
            }

            cmbBinId.DataSource = activeBinId.ToArray();
            //  cmbBinId.EndUpdate();
            // cmbBinId.Refresh();
            txtBinId = new TextBox() { Width = 50 };
            txtBinId.KeyPress += txtBinId_KeyPress;
            pnlLookupByBin.Controls.Add(txtBinId);

            btnLookupById = new Button()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Text = "Lookup",
                Margin = new Padding(0, 0, 25, 0)
            };
            btnLookupById.Click += btnLookupById_Click;
            pnlLookupByBin.Controls.Add(btnLookupById);

            pnlLookupBySpecimen = new FlowLayoutPanel()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };
            Controls.Add(pnlLookupBySpecimen);

            lblSpecimenId = new Label()
            {
                MinimumSize = new Size(0, 20),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "Specimen Id"
            };
            pnlLookupBySpecimen.Controls.Add(lblSpecimenId);

            txtSpecimenId = new TextBox() { };
            txtSpecimenId.KeyPress += txtSpecimenId_KeyPress;
            pnlLookupBySpecimen.Controls.Add(txtSpecimenId);

            btnLookupBySpecimen = new Button()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Text = "Lookup",
                Margin = new Padding(0, 0, 25, 0)
            };
            btnLookupBySpecimen.Click += btnLookupBySpecimen_Click;
            pnlLookupBySpecimen.Controls.Add(btnLookupBySpecimen);

            pnlSpecimenIdResults = new FlowLayoutPanel()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };
            Controls.Add(pnlSpecimenIdResults);

            cmbSpecimenIdResults = new ComboBox();
            cmbSpecimenIdResults.SelectedValueChanged += cmbSpecimenIdResults_SelectedValueChanged;
            pnlLookupBySpecimen.Controls.Add(cmbSpecimenIdResults);

            pnlBatchId = new FlowLayoutPanel()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = Padding.Empty,
                Margin = Padding.Empty,

            };
            Controls.Add(pnlBatchId);

            lblBatchId = new Label()
            {
                MinimumSize = new Size(0, 20),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "Batch Id"
            };
            pnlBatchId.Controls.Add(lblBatchId);

            txtBatchId = new TextBox() { Width = 250, Enabled = false };
            pnlBatchId.Controls.Add(txtBatchId);

            btnClear = new Button()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Text = "Clear"
            };
            btnClear.Click += btnClear_Click;
            pnlBatchId.Controls.Add(btnClear);

            this.GotFocus += BatchLookupControl_GotFocus;
        }

        private void cmbBinId_GotFocus(object sender, EventArgs e)
        {
            List<Bin> activeBins = Bins.Instance.ActiveBins.ToList();
           
                if (cmbBinId.Items.Count != activeBins.Count)
                {


                    //object selInd = cmbBinId.SelectedItem;
                    List<String> activeBinId = new List<string>();
                    for (int i = 0; i < activeBins.Count; i++)
                    {
                        activeBinId.Add(activeBins[i].Id);
                    }

                    cmbBinId.DataSource = activeBinId.ToArray();
                    if (activeBinId.Count != 0)
                    {
                        cmbBinId.SelectedIndex = 0;
                    }
                }
                else
            {
                if ((cmbBinId.Items.Count == 0) && (!(string.IsNullOrEmpty(cmbBinId.SelectedText))))
                    cmbBinId.SelectedText  = string.Empty;
                      
            }
             
        }

        private void cmbBinId_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  Bins.Instance.Reload();
            List<Bin> activeBins = Bins.Instance.ActiveBins.ToList();
           
                if (cmbBinId.Items.Count != activeBins.Count)
                {
                    object selInd = cmbBinId.SelectedItem;
                    List<String> activeBinId = new List<string>();
                    for (int i = 0; i < activeBins.Count; i++)
                    {
                        activeBinId.Add(activeBins[i].Id);
                    }

                    cmbBinId.DataSource = activeBinId.ToArray();
                    if (activeBinId.Count != 0)
                    {
                        cmbBinId.SelectedItem = selInd;
                    }
                }
           
        }

        //private void cmbBinId_MouseHover(object sender, EventArgs e)
        //{
        //    cmbBinId.SelectedIndex = -1;
        //    cmbSpecimenIdResults.SelectedIndex = -1;
        //    cmbSpecimenIdResults.DataSource = null;
        //    Bins.Instance.Reload();
        //    List<Bin> activeBins = Bins.Instance.ActiveBins.ToList();
        //    List<String> activeBinId = new List<string>();
        //    for (int i = 0; i < activeBins.Count; i++)
        //    {
        //        activeBinId.Add(activeBins[i].Id);
        //    }

        //    cmbBinId.DataSource = activeBinId.ToArray();
        //    //    //    cmbBinId.Refresh();
        //}

        //private void cmbBinId_GotFocus(object sender, EventArgs e)
        //{
        //    cmbBinId.SelectedIndex = -1;
        //        cmbSpecimenIdResults.SelectedIndex = -1;
        //        cmbSpecimenIdResults.DataSource = null;
        //       Bins.Instance.Reload();
        //       List<Bin> activeBins = Bins.Instance.ActiveBins.ToList();
        //       List<String> activeBinId = new List<string>();
        //        for (int i = 0; i < activeBins.Count; i++)
        //        {
        //           activeBinId.Add(activeBins[i].Id);
        //      }

        //      cmbBinId.DataSource = activeBinId.ToArray();
        //    //    cmbBinId.Refresh();
        // }

        //private void cmbBinId_TextChanged(object sender, EventArgs e)
        //{
        //    cmbBinId.SelectedIndex = -1;
        //    cmbBinId.DataSource = null;
        //    cmbSpecimenIdResults.SelectedIndex = -1;
        //    cmbSpecimenIdResults.DataSource = null;
        //    Bins.Instance.Reload();
        //    List<Bin> activeBins = Bins.Instance.ActiveBins.ToList();
        //    List<String> activeBinId = new List<string>();
        //    for (int i = 0; i < activeBins.Count; i++)
        //    {
        //        activeBinId.Add(activeBins[i].Id);
        //    }

        //    cmbBinId.DataSource = activeBinId.ToArray();
        //    cmbBinId.Refresh();
        //}

        //private void cmbBinId_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    cmbBinId.SelectedIndex = -1;
        //    cmbBinId.DataSource = null;
        //    cmbSpecimenIdResults.SelectedIndex = -1;
        //    cmbSpecimenIdResults.DataSource = null;
        //    Bins.Instance.Reload();
        //    List<Bin> activeBins = Bins.Instance.ActiveBins.ToList();
        //    List<String> activeBinId = new List<string>();
        //    for (int i = 0; i < activeBins.Count; i++)
        //    {
        //        activeBinId.Add(activeBins[i].Id);
        //    }

        //    cmbBinId.DataSource = activeBinId.ToArray();
        //    cmbBinId.Refresh();
        //}

        public void ReloadComBox()
        {
            Bins.Instance.Reload();
            List<Bin> activeBins = Bins.Instance.ActiveBins.ToList();
            List<String> activeBinId = new List<string>();
            cmbBinId.DataSource = null;

            for (int i = 0; i < activeBins.Count; i++)
            {
                activeBinId.Add(activeBins[i].Id);
            }

            cmbBinId.DataSource = activeBinId.ToArray();
            cmbBinId.Refresh();
        }
        void Clear()
        {
            this.SelectedBatch = null;
            txtBinId.Clear();
            txtSpecimenId.Clear();
            txtBatchId.Clear();

            cmbBinId.SelectedIndex = -1;
            cmbBinId.DataSource = null;
            cmbSpecimenIdResults.SelectedIndex = -1;
            cmbSpecimenIdResults.DataSource = null;
            Bins.Instance.Reload();
            List<Bin> activeBins = Bins.Instance.ActiveBins.ToList();
            List<String> activeBinId = new List<string>();
            for (int i = 0; i < activeBins.Count; i++)
            {
                activeBinId.Add(activeBins[i].Id);
            }

            cmbBinId.DataSource = activeBinId.ToArray();
            cmbBinId.Refresh();
        }

        void btnClear_Click(object sender, EventArgs e)
        { Clear(); }

        void BatchLookupControl_GotFocus(object sender, EventArgs e)
        {
            txtBinId.Focus();
        }

        void cmbSpecimenIdResults_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.SelectedBatch = null;
                txtBatchId.Clear();
                object selectedValue = cmbSpecimenIdResults.SelectedValue;
                if (selectedValue == null)
                { return; }

                string binId = (string)(selectedValue);
                txtBinId.Text = binId;
                btnLookupById.PerformClick();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                ErrorProvider.SetError(cmbSpecimenIdResults, ex.Message);
            }
        }

        void cmbBinId_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.SelectedBatch = null;
                string binId = cmbBinId.SelectedValue.ToString();

                binId = Bin.NormalizeId(binId);
                Bin bin = Bins.Instance[binId];
                if (bin == null)
                {
                    throw new InvalidOperationException("Specified bin does not exist.");
                    // MessageBox.Show("Specified bin does not exist.");
                }
                if (bin.HasBatch == false)
                {
                    MessageBox.Show("Specified bin does not contain an active Batch.");
                    throw new InvalidOperationException("Specified bin does not contain an active Batch.");
                }

                this.SelectedBatch = bin.EnsureGetBatch();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                ErrorProvider.SetError(cmbSpecimenIdResults, ex.Message);
            }
        }

        void btnLookupBySpecimen_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorProvider.Clear();
                this.SelectedBatch = null;
                cmbSpecimenIdResults.SelectedIndex = -1;
                cmbSpecimenIdResults.DataSource = null;

                string SpecimenId = txtSpecimenId.Text;
                if (string.IsNullOrWhiteSpace(SpecimenId))
                {
                    MessageBox.Show("Value required");
                    throw new InvalidOperationException("Value required");
                }

                string[] bins = (from Bin bin
                                in Bins.Instance.GetBinsContainingSpecimenWithWildcard(SpecimenId)
                                 select bin.Id).ToArray();
                if (bins.Length == 0)
                {
                    MessageBox.Show("The specified Specimen could not be found.");
                    throw new InvalidOperationException("The specified Specimen could not be found.");
                }

                cmbSpecimenIdResults.DataSource = bins;
                cmbSpecimenIdResults.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                ErrorProvider.SetError(btnLookupBySpecimen, ex.Message);
            }
        }

        void txtBinId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            { btnLookupById.PerformClick(); }
        }

        void txtSpecimenId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            { btnLookupBySpecimen.PerformClick(); }
        }

        void btnLookupById_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorProvider.Clear();
                this.SelectedBatch = null;
                string binId = txtBinId.Text;
                if (string.IsNullOrWhiteSpace(binId))
                {
                    throw new InvalidOperationException("Value required");
                    // MessageBox.Show("Value required");
                }
                binId = Bin.NormalizeId(binId);
                Bin bin = Bins.Instance[binId];
                if (bin == null)
                {
                    throw new InvalidOperationException("Specified bin does not exist.");
                    //MessageBox.Show("Specified bin does not exist.");
                }
                if (bin.HasBatch == false)
                {
                    MessageBox.Show("Specified bin does not contain an active Batch.");
                    throw new InvalidOperationException("Specified bin does not contain an active Batch.");
                }

                this.SelectedBatch = bin.EnsureGetBatch();

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                ErrorProvider.SetError(btnLookupById, ex.Message);
            }
        }

        public BatchLookupControl()
        {
            InitializeComponent();
        }

    }
}
