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
    public partial class JHCCCRecordDialog : Form
    {
        public string BoxId
        {
            get { return BoxIdTextBox.Text; }
            set { BoxIdTextBox.Text = value; }
        }

        public string ClosingYear
        {
            get { return ClosingYearTextBox.Text; }
            set { ClosingYearTextBox.Text = value; }
        }

        public string RecordId
        {
            get { return RecordIdTextBox.Text; }
            set { RecordIdTextBox.Text = value; }
        }

        public string LastName
        {
            get { return LastNameTextBox.Text; }
            set { LastNameTextBox.Text = value; }
        }

        public string FirstName
        {
            get { return FirstNameTextBox.Text; }
            set { FirstNameTextBox.Text = value; }
        }

        public int ManualPageCount
        {
            get { return int.Parse(PageCountTextBox.Text); }
            set { PageCountTextBox.Text = value.ToString(); }
        }

        public JHCCCRecordDialog()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            RecordIdTextBox.Clear();
            LastNameTextBox.Clear();
            FirstNameTextBox.Clear();
            PageCountTextBox.Clear();
           
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (string.IsNullOrWhiteSpace(BoxIdTextBox.Text))
            { BoxIdTextBox.Focus(); }
            else
            { LastNameTextBox.Focus(); }
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {

        }
    }
}
