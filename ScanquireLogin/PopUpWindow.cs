using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ScanquireLogin
{
    public partial class PopUpWindow : Form
    {
        public string PopUpMessage
        { get; set; }
        public PopUpWindow()
        {
            InitializeComponent();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Close();
        }

        private void PopUpWindow_Shown(object sender, EventArgs e)
        {
            labMessage.Text = PopUpMessage;
            timer1.Start();
        }
    }
}
