using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common
{
    public partial class CategoryPickerDialog : Form
    {
        protected string SelectedKey;

        public CategoryPickerDialog()
        {
            InitializeComponent();

            cmbCategories.Source = SpecimenCategories.Instance;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                cmbCategories.EnsureGetSelectedKey();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error");
            }
        }

        public static Category SelectCategory(IWin32Window parent)
        {
            CategoryPickerDialog dlg = new CategoryPickerDialog();
            dlg.StartPosition = FormStartPosition.CenterParent;

            if (dlg.ShowDialog(parent) != DialogResult.OK)
            {
                throw new OperationCanceledException();
            }

            return dlg.cmbCategories.EnsureGetSelectedValue();
        }
    }
}
