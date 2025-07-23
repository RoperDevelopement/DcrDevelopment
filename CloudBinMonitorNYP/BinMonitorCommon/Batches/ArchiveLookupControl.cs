using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common
{
    public class ArchiveLookupControl : FlowLayoutPanel, ISpecimenBatchSource
    {
        ErrorProvider ErrorProvider;

        
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
        List<SpecimenBatch> batchesForComboBox = new List<SpecimenBatch>();
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
                Text="Specimen Id"
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
                        
            txtBatchId = new TextBox() { Width=250, Enabled = false };
            pnlBatchId.Controls.Add(txtBatchId);

            btnClear = new Button()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Text = "Clear"
            };
            btnClear.Click += btnClear_Click;
            pnlBatchId.Controls.Add(btnClear);

        }

        void Clear()
        {
            this.SelectedBatch = null;
            txtSpecimenId.Clear();
            txtBatchId.Clear();
            cmbSpecimenIdResults.SelectedIndex = -1;
            cmbSpecimenIdResults.DataSource = null;            
        }

        void btnClear_Click(object sender, EventArgs e)
        { Clear(); }


        void cmbSpecimenIdResults_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.SelectedBatch = null;
                
                txtBatchId.Clear();
                
                object selectedValue = cmbSpecimenIdResults.SelectedValue;
                if (selectedValue == null)
                { return; }
                this.SelectedBatch = batchesForComboBox[cmbSpecimenIdResults.SelectedIndex];

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
                batchesForComboBox.Clear();

                string SpecimenId = txtSpecimenId.Text;
                if (string.IsNullOrWhiteSpace(SpecimenId))
                {
                    MessageBox.Show("Value required");
                    throw new InvalidOperationException("Value required");
                }
                SpecimenId = SpecimenId.ToUpper();
                String batchPath = SpecimenBatches.Instance.DirectoryPath;
                SpecimenBatches.Instance.DirectoryPath = SpecimenBatches.Instance.ArchiveDirectoryPath;
                SpecimenBatches.Instance.Reload();

                List<SpecimenBatch> test = SpecimenBatches.Instance.Values.ToList();
                /*
                string[] bins = (from Bin bin 
                                in Bins.Instance.GetBinsContainingSpecimenWithWildcard(SpecimenId) 
                                select bin.Id).ToArray();
                */
                string[] bins;
                List<String> binResult = new List<string>();
                int resultsCount = 0;
                for (int i = 0; i < test.Count; i++)
                {
                 //   List<string> test2;
                  //  List<string> temp = test[i].Specimens;
                   // test2 = temp.Where(x => x.Contains(SpecimenId)).ToList();
                   // if (test2.Count > 0)
                   if(test[i].Id.ToUpper() == SpecimenId)
                    
                    {   
                        
                        binResult.Add(test[i].BinId);
                        //SpecimenBatch tempBatch = test[i];
                        test[i].IsClosed = true;
                        batchesForComboBox.Add(test[i]);
                        resultsCount++;
                    }

                }
                
                bins = binResult.ToArray();
                if (resultsCount == 0)
                {
                    MessageBox.Show("The specified Specimen could not be found.");
                  //  throw new InvalidOperationException("The specified Specimen could not be found.");
                }

                SpecimenBatches.Instance.DirectoryPath = batchPath;
                SpecimenBatches.Instance.Reload();


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
                string binId = "";
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
                   // MessageBox.Show("Specified bin does not exist.");
                }
                if (bin.HasBatch == false)
                {
                    MessageBox.Show("Specified bin does not contain an active Batch.");
                    //throw new InvalidOperationException("Specified bin does not contain an active Batch.");
                }

                this.SelectedBatch = bin.EnsureGetBatch();

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
        }

        public ArchiveLookupControl()
        {
            InitializeComponent();
        }

    }
}
