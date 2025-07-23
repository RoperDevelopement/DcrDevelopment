using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DemoArchivers
{
    public partial class AccountingRecordDialog : Form
    {
        public string Year
        {
            get { return YearTextBox.Text; }
            set { YearTextBox.Text = value; }
        }

        public string Month
        {
            get { return MonthTextBox.Text; }
            set { MonthTextBox.Text = value; }
        }

        public string LineItem
        {
            get { return LineItemTextBox.Text; }
            set { LineItemTextBox.Text = value; }
        }

        public AccountingRecordDialog()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            LineItemTextBox.Clear();
            MonthTextBox.Clear();
            YearTextBox.Clear();
           
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (string.IsNullOrWhiteSpace(YearTextBox.Text))
            { YearTextBox.Focus(); }
            else
            { MonthTextBox.Focus(); }
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {

        }
    }
}
