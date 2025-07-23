using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edocs.Convert.Viedos.Images
{
    public partial class VIewViedoForm : Form
    {
        public string ViedoFileName
        { get; set; }

        public VIewViedoForm()
        {
            InitializeComponent();
        }

        private void VIewViedoForm_Load(object sender, EventArgs e)
        {
        axWindowsMediaPlayer1.Dock= DockStyle.Fill;
            axWindowsMediaPlayer1.settings.rate = 1.00;
            axWindowsMediaPlayer1.URL = ViedoFileName;
            axWindowsMediaPlayer1.settings.autoStart = true;



    }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Dispose();
            this.Close();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            UnCHeck();
            ts15.Checked = true;
            axWindowsMediaPlayer1.settings.rate = ConertStrTODouble(ts15.Text);
            axWindowsMediaPlayer1.Ctlcontrols.play();
            // axWindowsMediaPlayer1.openPlayer(ViedoFileName);

        }
         private double ConertStrTODouble(string instr)
        {
            if (double.TryParse(instr, out double resuts))
                return resuts;
            return 1.0;
        }

        private void ts10_Click(object sender, EventArgs e)
        {
            UnCHeck();
            ts10.Checked = true;
            axWindowsMediaPlayer1.settings.rate = ConertStrTODouble(ts10.Text);
            axWindowsMediaPlayer1.Ctlcontrols.play();

        }

        private void ts20_Click(object sender, EventArgs e)
        {
            UnCHeck();
            ts20.Checked = true;
            axWindowsMediaPlayer1.settings.rate = ConertStrTODouble(ts20.Text);
            axWindowsMediaPlayer1.Ctlcontrols.play();

        }

        private void ts25_Click(object sender, EventArgs e)
        {
            UnCHeck();
            ts25.Checked = true;
            axWindowsMediaPlayer1.settings.rate = ConertStrTODouble(ts25.Text);
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void ts30_Click(object sender, EventArgs e)
        {
            UnCHeck();
            ts30.Checked = true;
            axWindowsMediaPlayer1.settings.rate = ConertStrTODouble(ts30.Text);
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void ts35_Click(object sender, EventArgs e)
        {
            UnCHeck();
            ts35.Checked = true;
            axWindowsMediaPlayer1.settings.rate = ConertStrTODouble(ts35.Text);
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void ts40_Click(object sender, EventArgs e)
        {
            UnCHeck();
            ts40.Checked = true;
            axWindowsMediaPlayer1.settings.rate = ConertStrTODouble(ts40.Text);
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void ts45_Click(object sender, EventArgs e)
        {
            UnCHeck();
            ts45.Checked = true;
            axWindowsMediaPlayer1.settings.rate = ConertStrTODouble(ts45.Text);
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void ts50_Click(object sender, EventArgs e)
        {
            UnCHeck();
            ts50.Checked = true;
            axWindowsMediaPlayer1.settings.rate = ConertStrTODouble(ts50.Text);
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }
        private void UnCHeck()
        {
           
            ts10.Checked = false;
            ts15.Checked = false;
            ts20.Checked = false;
            ts25.Checked = false;
           
                ts30.Checked = false;
            ts35.Checked = false;
            ts40.Checked = false;
            ts45.Checked = false;
            ts50.Checked = false;

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            { 
            axWindowsMediaPlayer1.openPlayer(ViedoFileName);

            }
            catch(Exception ex)
            {

            }
        }
    }
}
