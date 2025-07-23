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
    public partial class ManageDefaultCategoryControl : UserControl
    {
        public IUserSource CredentialHost
        { get; set; }

        public ManageDefaultCategoryControl()
        {
            InitializeComponent();
            if (DesignMode == false)
            {
                cmbCategories.Source = SpecimenCategories.Instance;
            }
        }

        /*
        public Category GetActiveCategory()
        {
            if (cmbCategories.SelectedValue == null)
            { return null; }
            else
            { 
                string categoryTitle = (string)(cmbBatchCategory.SelectedValue);
                if (SpecimenCategories.Instance.ContainsKey(categoryTitle) == false)
                { return null; }
                return SpecimenCategories.Instance[categoryTitle];
            }
        }

        public Category EnsureGetActiveCategory()
        {
            Category category = GetActiveCategory();
            if (category == null)
            { throw new InvalidOperationException("The requested operation requires a category to be selected"); }
            else
            { return category; }
        }

        private void cmbBatchCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetCategorySettings();
        }
        */

        private void OnCmbCategories_SelectedKeyChanged()
        {
            Category category = cmbCategories.SelectedValue;
            if (category == null)
            {
                btnReset.Enabled = false;
                btnSaveChanges.Enabled = false;
                brnChangeColor.Enabled = false;
                lblCategoryColor.BackColor = SystemColors.Control;
                checkpointConfigurationControl1.Clear();
                checkpointConfigurationControl2.Clear();
                checkpointConfigurationControl3.Clear();
                checkpointConfigurationControl4.Clear();
                wfcfgCreate.Clear();
                wfcfgProcess.Clear();
                wfcfgRegister.Clear();
            }
            else
            {
                btnReset.Enabled = true;
                btnSaveChanges.Enabled = true;
                brnChangeColor.Enabled = true;
                lblCategoryColor.BackColor = category.Color.Value;
                checkpointConfigurationControl1.LoadFromExisting(category.CheckPoint1Configuration);
                checkpointConfigurationControl2.LoadFromExisting(category.CheckPoint2Configuration);
                checkpointConfigurationControl3.LoadFromExisting(category.CheckPoint3Configuration);
                checkpointConfigurationControl4.LoadFromExisting(category.CheckPoint4Configuration);
                wfcfgCreate.LoadFromExisting(category.CreateConfiguration);
                wfcfgRegister.LoadFromExisting(category.RegisterConfiguration);
                wfcfgProcess.LoadFromExisting(category.ProcessConfiguration);
            }
            
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            OnCmbCategories_SelectedKeyChanged();
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (ValidateInput() == false)
            {
                MessageBox.Show("One or more values is invalid, please review your input and try again");
                return;
            }

            try
            {
                User activeUser = CredentialHost.EnsureGetSelectedUser();

                Category category = cmbCategories.EnsureGetSelectedValue();
                category.Color.Value = lblCategoryColor.BackColor;
                category.CheckPoint1Configuration = checkpointConfigurationControl1.Value;
                category.CheckPoint2Configuration = checkpointConfigurationControl2.Value;
                category.CheckPoint3Configuration = checkpointConfigurationControl3.Value;
                category.CheckPoint4Configuration = checkpointConfigurationControl4.Value;
                category.CreateConfiguration = wfcfgCreate.Value;
                category.RegisterConfiguration = wfcfgRegister.Value;
                category.ProcessConfiguration = wfcfgProcess.Value;

                SpecimenCategories.Instance[category.Title] = category;
                MessageBox.Show(this, "Category Updated");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }

        private bool ValidateInput()
        {
            bool errors = false;
            ErrorProvider.Clear();

            if (cmbCategories.HasSelectedKey == false)
            {
                ErrorProvider.SetError(cmbCategories, "Value required");
                errors = true;
            }

            if (lblCategoryColor.BackColor == SystemColors.Control)
            {
                ErrorProvider.SetError(lblCategoryColor, "Value required");
                errors = true;
            }

            if (checkpointConfigurationControl1.Validate() == false)
            { errors = true; }
            if (checkpointConfigurationControl2.Validate() == false)
            { errors = true; }
            if (checkpointConfigurationControl3.Validate() == false)
            { errors = true; }
            if (checkpointConfigurationControl4.Validate() == false)
            { errors = true; }
            if (wfcfgCreate.Validate() == false)
            { errors = true; }
            if (wfcfgRegister.Validate() == false)
            { errors = true; }
            if (wfcfgProcess.Validate() == false)
            { errors = true; }

            return errors == false;
        }

   
        private void brnChangeColor_Click(object sender, EventArgs e)
        {
            try
            {
                if (CategoryColorDialog.ShowDialog(this) != DialogResult.OK)
                { throw new OperationCanceledException(); }

                lblCategoryColor.BackColor = CategoryColorDialog.Color;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }

        }

        private void cmbCategories_SelectedKeyChanged(object sender, EventArgs e)
        {
            OnCmbCategories_SelectedKeyChanged();
        }
    }
}
