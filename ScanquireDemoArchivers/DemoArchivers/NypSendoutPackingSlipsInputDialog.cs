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
    public partial class NypSendoutPackingSlipsInputDialog : Form
    {
        public DateTime? ProcDate
        {
            get
            {
                DateTime procDate;
                if (DateTime.TryParse(txtProcDate.Text, out procDate) == false)
                { return null; }
                else
                { return procDate; }
            }
            set
            { txtProcDate.Text = ProcDate.HasValue ? ProcDate.Value.ToString("dd/MM/yyyy") : string.Empty; }


        }

        public NypSendoutPackingSlipsInputDialog()
        {
            InitializeComponent();
        }
    }
}
